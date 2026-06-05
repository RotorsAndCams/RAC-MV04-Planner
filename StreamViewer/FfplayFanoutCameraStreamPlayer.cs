using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionPlanner.StreamViewer
{
    public sealed class FfplayFanoutCameraStreamPlayer : ICameraStreamPlayer
    {
        private readonly CameraStreamOptions _options;

        private UdpPacketFanOut _rtpFanOut;

        private IPEndPoint _displayRtpEndpoint;
        private IPEndPoint _recordingRtpEndpoint;

        private Process _ffplayProcess;
        private Process _recordingProcess;

        private IntPtr _ffplayWindow = IntPtr.Zero;
        private IntPtr _hostWindowHandle = IntPtr.Zero;

        private int _hostWidth;
        private int _hostHeight;

        private volatile bool _desiredDisplayRunning;
        private volatile bool _displayStartInProgress;
        private volatile bool _retryLoopRunning;

        private readonly object _displayLock = new object();

        private readonly object _recordingLock = new object();
        private volatile bool _recordingStartInProgress;
        private int _activeRecordingRtpPort = -1;

        private DateTime _lastDebugMessageBoxUtc = DateTime.MinValue;

        public event EventHandler<string> StatusChanged;
        public event EventHandler<Exception> ErrorOccurred;

        public bool IsRunning
        {
            get
            {
                return _rtpFanOut != null &&
                       _rtpFanOut.IsRunning &&
                       _ffplayProcess != null &&
                       !_ffplayProcess.HasExited &&
                       _ffplayWindow != IntPtr.Zero;
            }
        }

        public bool IsSegmentRecording
        {
            get
            {
                return _recordingProcess != null &&
                       !_recordingProcess.HasExited;
            }
        }

        public FfplayFanoutCameraStreamPlayer(CameraStreamOptions options)
        {
            if (options == null)
                options = new CameraStreamOptions();

            _options = options;
        }

        public Task ConnectAsync()
        {
            try
            {
                StartFanOutIfNeeded();
                RaiseStatus("Stream receiver connected.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ConnectAsync failed: " + ex);
                RaiseError(ex);
            }

            return Task.FromResult(0);
        }

        public async Task StartAsync(IntPtr hostWindowHandle)
        {
            _hostWindowHandle = hostWindowHandle;
            _desiredDisplayRunning = true;

            await SafeStartDisplayAttemptAsync();
        }

        public async Task RestartAsync(IntPtr hostWindowHandle)
        {
            _hostWindowHandle = hostWindowHandle;
            _desiredDisplayRunning = true;

            StopDisplayOnly();

            if (_options.RestartDelayMs > 0)
                await Task.Delay(_options.RestartDelayMs);

            await SafeStartDisplayAttemptAsync();
        }

        public void Stop()
        {
            _desiredDisplayRunning = false;

            StopSegmentRecording();
            StopDisplayOnly();
            StopFanOut();

            RaiseStatus("Stopped.");
        }

        public void Resize(int hostWidth, int hostHeight)
        {
            _hostWidth = hostWidth;
            _hostHeight = hostHeight;

            if (_ffplayWindow == IntPtr.Zero)
                return;

            if (hostWidth <= 0 || hostHeight <= 0)
                return;

            Rectangle target = GetContainedVideoRectangle(hostWidth, hostHeight);

            MoveWindow(
                _ffplayWindow,
                target.X,
                target.Y,
                target.Width,
                target.Height,
                true
            );

            UpdateWindow(_ffplayWindow);
        }

        public async Task CaptureCurrentFrameAsync(string outputFilePath)
        {
            try
            {
                if (_hostWindowHandle == IntPtr.Zero)
                    return;

                if (string.IsNullOrWhiteSpace(outputFilePath))
                    return;

                string directory = Path.GetDirectoryName(outputFilePath);

                if (!string.IsNullOrWhiteSpace(directory))
                    Directory.CreateDirectory(directory);

                await Task.Run(delegate
                {
                    Rectangle bounds = GetWindowRectangle(_hostWindowHandle);

                    if (bounds.Width <= 0 || bounds.Height <= 0)
                        return;

                    using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format24bppRgb))
                    {
                        using (Graphics graphics = Graphics.FromImage(bitmap))
                        {
                            graphics.CopyFromScreen(
                                bounds.Left,
                                bounds.Top,
                                0,
                                0,
                                bounds.Size,
                                CopyPixelOperation.SourceCopy
                            );
                        }

                        SaveBitmapByExtension(bitmap, outputFilePath);
                    }
                });

                RaiseStatus("Frame captured: " + outputFilePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("CaptureCurrentFrameAsync failed: " + ex);
                RaiseError(ex);

                ShowDebugMessageBox(
                    "Stream debug - capture failed",
                    ex.ToString(),
                    false
                );
            }
        }

        public Task StartSegmentRecordingAsync(
            string outputDirectory,
            int segmentLengthSeconds,
            string filePrefix)
        {
            lock (_recordingLock)
            {
                if (_recordingStartInProgress)
                {
                    Debug.WriteLine("Recording start ignored: recording is already starting.");
                    return Task.FromResult(0);
                }

                if (IsProcessAlive(_recordingProcess))
                {
                    Debug.WriteLine("Recording start ignored: recording process is already running.");
                    return Task.FromResult(0);
                }

                _recordingStartInProgress = true;
            }

            bool endpointAdded = false;

            try
            {
                if (string.IsNullOrWhiteSpace(_options.FfmpegPath) || !File.Exists(_options.FfmpegPath))
                {
                    RaiseStatus("Recording not started: ffmpeg path is invalid.");

                    ShowDebugMessageBox(
                        "Stream debug - invalid ffmpeg path",
                        "ffmpeg path:\n" + _options.FfmpegPath,
                        true
                    );

                    return Task.FromResult(0);
                }

                if (segmentLengthSeconds <= 0)
                    segmentLengthSeconds = 10;

                if (string.IsNullOrWhiteSpace(outputDirectory))
                {
                    RaiseStatus("Recording not started: output directory is empty.");
                    return Task.FromResult(0);
                }

                Directory.CreateDirectory(outputDirectory);

                StartFanOutIfNeeded();

                IPAddress localAddress;

                if (!IPAddress.TryParse(_options.LocalHost, out localAddress))
                    localAddress = IPAddress.Loopback;

                int selectedRecordingPort = FindAvailableUdpPort(
                    _options.LocalRecordingRtpPort,
                    50
                );

                _activeRecordingRtpPort = selectedRecordingPort;
                _recordingRtpEndpoint = new IPEndPoint(localAddress, selectedRecordingPort);

                string args = BuildSegmentRecordingArguments(
                    outputDirectory,
                    segmentLengthSeconds,
                    string.IsNullOrWhiteSpace(filePrefix) ? "camera" : filePrefix,
                    selectedRecordingPort
                );

                Debug.WriteLine("========== STARTING FFMPEG RECORDING ==========");
                Debug.WriteLine("Path: " + _options.FfmpegPath);
                Debug.WriteLine("Working directory: " + GetFfmpegWorkingDirectory());
                Debug.WriteLine("Recording local RTP port: " + selectedRecordingPort);
                Debug.WriteLine("Args: " + args);
                Debug.WriteLine("===============================================");

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = _options.FfmpegPath,
                    Arguments = args,
                    WorkingDirectory = GetFfmpegWorkingDirectory(),

                    UseShellExecute = false,
                    CreateNoWindow = _options.HideConsoleWindow,

                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true
                };

                _recordingProcess = new Process
                {
                    StartInfo = psi,
                    EnableRaisingEvents = true
                };

                _recordingProcess.ErrorDataReceived += RecordingProcess_ErrorDataReceived;
                _recordingProcess.OutputDataReceived += RecordingProcess_OutputDataReceived;
                _recordingProcess.Exited += RecordingProcess_Exited;

                bool started = _recordingProcess.Start();

                if (started)
                {
                    _recordingProcess.BeginErrorReadLine();
                    _recordingProcess.BeginOutputReadLine();

                    RaiseStatus(
                        "Recording segments. Segment length: " +
                        segmentLengthSeconds +
                        "s, port: " +
                        selectedRecordingPort
                    );
                }
                else
                {
                    RaiseStatus("Recording not started: ffmpeg process did not start.");

                    if (endpointAdded && _rtpFanOut != null && _recordingRtpEndpoint != null)
                        _rtpFanOut.RemoveOutputEndpoint(_recordingRtpEndpoint);

                    ShowDebugMessageBox(
                        "Stream debug - recording did not start",
                        "ffmpeg process Start() returned false.",
                        true
                    );
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("StartSegmentRecordingAsync failed: " + ex);
                RaiseError(ex);                

                ShowDebugMessageBox(
                    "Stream debug - StartSegmentRecordingAsync exception",
                    ex.ToString(),
                    true
                );

                StopSegmentRecording();
            }
            finally
            {
                lock (_recordingLock)
                {
                    _recordingStartInProgress = false;
                }
            }

            return Task.FromResult(0);
        }

        public void StopSegmentRecording()
        {
            try
            {
                StopProcess(_recordingProcess, "ffmpeg recording");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("StopSegmentRecording failed: " + ex);
                RaiseError(ex);
            }
            finally
            {
                if (_recordingProcess != null)
                {
                    try
                    {
                        _recordingProcess.ErrorDataReceived -= RecordingProcess_ErrorDataReceived;
                        _recordingProcess.OutputDataReceived -= RecordingProcess_OutputDataReceived;
                        _recordingProcess.Exited -= RecordingProcess_Exited;
                    }
                    catch
                    {
                    }

                    try
                    {
                        _recordingProcess.Dispose();
                    }
                    catch
                    {
                    }

                    _recordingProcess = null;
                }

                lock (_recordingLock)
                {
                    _recordingStartInProgress = false;
                }
            }           

            _activeRecordingRtpPort = -1;

            if (IsRunning)
                RaiseStatus("Recording stopped. Display still running.");
            else
                RaiseStatus("Recording stopped.");
        }

        public void Dispose()
        {
            Stop();
        }

        private async Task SafeStartDisplayAttemptAsync()
        {
            if (_displayStartInProgress)
                return;

            lock (_displayLock)
            {
                if (_displayStartInProgress)
                    return;

                _displayStartInProgress = true;
            }

            try
            {
                StartFanOutIfNeeded();

                if (!_desiredDisplayRunning)
                    return;

                if (_hostWindowHandle == IntPtr.Zero)
                {
                    RaiseStatus("Display waiting for host window handle.");
                    StartRetryLoopIfNeeded();
                    return;
                }

                if (string.IsNullOrWhiteSpace(_options.FfplayPath) || !File.Exists(_options.FfplayPath))
                {
                    string message =
                        "Display not started: ffplay path is invalid.\n\n" +
                        "ffplay path:\n" +
                        _options.FfplayPath;

                    Debug.WriteLine(message);
                    RaiseStatus(message);

                    ShowDebugMessageBox("Stream debug - invalid ffplay path", message, true);

                    StartRetryLoopIfNeeded();
                    return;
                }

                StopDisplayOnly();

                string ffplayArgs = BuildFfplayArguments();

                Debug.WriteLine("========== STARTING FFPLAY DISPLAY ==========");
                Debug.WriteLine("Path: " + _options.FfplayPath);
                Debug.WriteLine("Working directory: " + GetFfplayWorkingDirectory());
                Debug.WriteLine("Args: " + ffplayArgs);
                Debug.WriteLine("=============================================");

                StartFfplayDisplay();

                if (_ffplayProcess == null)
                {
                    RaiseStatus("ffplay process did not start.");
                    StartRetryLoopIfNeeded();
                    return;
                }

                _ffplayWindow = await WaitForVisibleFfplayWindowAsync(
                    _ffplayProcess,
                    _options.WindowFindTimeoutMs
                );

                if (_ffplayWindow == IntPtr.Zero)
                {
                    string packetInfo =
                        "RTP packets received: " + (_rtpFanOut != null ? _rtpFanOut.ReceivedPacketCount : 0) +
                        ", forwarded: " + (_rtpFanOut != null ? _rtpFanOut.ForwardedPacketCount : 0) +
                        ", dropped: " + (_rtpFanOut != null ? _rtpFanOut.DroppedPacketCount : 0) +
                        ", ignored: " + (_rtpFanOut != null ? _rtpFanOut.IgnoredPacketCount : 0);

                    string message =
                        "Visible ffplay window was not found.\n\n" +
                        packetInfo + "\n\n" +
                        "ffplay args:\n" + ffplayArgs + "\n\n" +
                        "ffplay will be restarted after the retry delay.";

                    Debug.WriteLine(message);
                    RaiseStatus("Waiting for camera stream...");

                    StopDisplayOnly();
                    StartRetryLoopIfNeeded();
                    return;
                }

                AdoptFfplayWindow(_ffplayWindow);

                if (IsSegmentRecording)
                    RaiseStatus("Display streaming and recording.");
                else
                    RaiseStatus("Display streaming.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SafeStartDisplayAttemptAsync failed: " + ex);
                RaiseError(ex);

                ShowDebugMessageBox(
                    "Stream debug - SafeStartDisplayAttemptAsync exception",
                    ex.ToString(),
                    true
                );

                StopDisplayOnly();
                StartRetryLoopIfNeeded();
            }
            finally
            {
                _displayStartInProgress = false;
            }
        }

        private void StartRetryLoopIfNeeded()
        {
            if (_retryLoopRunning)
                return;

            _retryLoopRunning = true;

            Task.Run(async delegate
            {
                try
                {
                    while (_desiredDisplayRunning && !IsRunning)
                    {
                        int delay = _options.SilentRetryDelayMs > 0
                            ? _options.SilentRetryDelayMs
                            : 3000;

                        await Task.Delay(delay);

                        if (!_desiredDisplayRunning || IsRunning)
                            break;

                        await SafeStartDisplayAttemptAsync();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Retry loop failed: " + ex);
                    RaiseError(ex);

                    ShowDebugMessageBox(
                        "Stream debug - retry loop exception",
                        ex.ToString(),
                        true
                    );
                }
                finally
                {
                    _retryLoopRunning = false;

                    if (_desiredDisplayRunning && !IsRunning)
                        StartRetryLoopIfNeeded();
                }
            });
        }

        private void StartFanOutIfNeeded()
        {
            try
            {
                if (_rtpFanOut != null && _rtpFanOut.IsRunning)
                    return;

                StopFanOut();

                IPAddress localAddress;

                if (!IPAddress.TryParse(_options.LocalHost, out localAddress))
                    localAddress = IPAddress.Loopback;

                _displayRtpEndpoint = new IPEndPoint(localAddress, _options.LocalDisplayRtpPort);
                _recordingRtpEndpoint = new IPEndPoint(localAddress, _options.LocalRecordingRtpPort);

                string message =
                    "Starting RTP fan-out.\n\n" +
                    "Local bind IP: " + _options.LocalBindIpAddress + "\n" +
                    "Camera source IP filter: " + _options.CameraSourceIpAddress + "\n" +
                    "Camera RTP port: " + _options.CameraRtpPort + "\n" +
                    "Display endpoint: " + _displayRtpEndpoint + "\n" +
                    "Recording endpoint: " + _recordingRtpEndpoint + "\n" +
                    "Receive buffer: " + _options.FanOutReceiveBufferBytes + "\n" +
                    "Soft drop enabled: " + _options.FanOutSoftDropEnabled + "\n" +
                    "Backlog threshold: " + _options.FanOutBacklogDropThresholdBytes + "\n" +
                    "Max drain packets: " + _options.FanOutMaxDrainPackets;

                Debug.WriteLine(message);

                // Important:
                // Match the old working behavior: always forward to BOTH display and recording ports.
                // UDP forwarding to the recording port is harmless even before ffmpeg starts.
                _rtpFanOut = new UdpPacketFanOut(
                    "RTP",
                    _options.LocalBindIpAddress,
                    _options.CameraSourceIpAddress,
                    _options.CameraRtpPort,
                    _options.FanOutReceiveBufferBytes,
                    _options.FanOutSoftDropEnabled,
                    _options.FanOutBacklogDropThresholdBytes,
                    _options.FanOutMaxDrainPackets,
                    _displayRtpEndpoint,
                    _recordingRtpEndpoint
                );

                _rtpFanOut.StatusChanged += FanOut_StatusChanged;
                _rtpFanOut.ErrorOccurred += FanOut_ErrorOccurred;

                _rtpFanOut.Start();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("StartFanOutIfNeeded failed: " + ex);
                RaiseError(ex);

                ShowDebugMessageBox(
                    "Stream debug - StartFanOutIfNeeded exception",
                    ex.ToString(),
                    true
                );
            }
        }

        private void StopFanOut()
        {
            try
            {
                if (_rtpFanOut != null)
                    _rtpFanOut.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("StopFanOut failed: " + ex);
                RaiseError(ex);
            }

            _rtpFanOut = null;
        }

        private void StartFfplayDisplay()
        {
            try
            {
                string args = BuildFfplayArguments();

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = _options.FfplayPath,
                    Arguments = args,
                    WorkingDirectory = GetFfplayWorkingDirectory(),

                    UseShellExecute = false,
                    CreateNoWindow = _options.HideConsoleWindow,

                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };

                _ffplayProcess = new Process
                {
                    StartInfo = psi,
                    EnableRaisingEvents = true
                };

                _ffplayProcess.ErrorDataReceived += FfplayProcess_ErrorDataReceived;
                _ffplayProcess.OutputDataReceived += FfplayProcess_OutputDataReceived;
                _ffplayProcess.Exited += FfplayProcess_Exited;

                bool started = _ffplayProcess.Start();

                if (started)
                {
                    _ffplayProcess.BeginErrorReadLine();
                    _ffplayProcess.BeginOutputReadLine();

                    Debug.WriteLine("ffplay process started. PID=" + _ffplayProcess.Id);
                }
                else
                {
                    RaiseStatus("Display not started: ffplay process did not start.");

                    ShowDebugMessageBox(
                        "Stream debug - ffplay did not start",
                        "ffplay process Start() returned false.",
                        true
                    );
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("StartFfplayDisplay failed: " + ex);
                RaiseError(ex);

                ShowDebugMessageBox(
                    "Stream debug - StartFfplayDisplay exception",
                    ex.ToString(),
                    true
                );
            }
        }

        private void StopDisplayOnly()
        {
            try
            {
                StopProcess(_ffplayProcess, "ffplay");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("StopDisplayOnly failed: " + ex);
                RaiseError(ex);
            }
            finally
            {
                if (_ffplayProcess != null)
                {
                    try
                    {
                        _ffplayProcess.ErrorDataReceived -= FfplayProcess_ErrorDataReceived;
                        _ffplayProcess.OutputDataReceived -= FfplayProcess_OutputDataReceived;
                        _ffplayProcess.Exited -= FfplayProcess_Exited;
                    }
                    catch
                    {
                    }

                    try
                    {
                        _ffplayProcess.Dispose();
                    }
                    catch
                    {
                    }

                    _ffplayProcess = null;
                }

                _ffplayWindow = IntPtr.Zero;
            }
        }

        private void StopProcess(Process process, string processName)
        {
            if (process == null)
                return;

            try
            {
                if (process.HasExited)
                    return;

                try
                {
                    if (process.StartInfo != null && process.StartInfo.RedirectStandardInput)
                    {
                        process.StandardInput.WriteLine("q");
                        process.StandardInput.Flush();
                    }
                }
                catch
                {
                }

                if (process.WaitForExit(1000))
                    return;

                try
                {
                    process.Kill();
                }
                catch
                {
                }

                try
                {
                    process.WaitForExit(1000);
                }
                catch
                {
                }

                if (!process.HasExited)
                    Debug.WriteLine(processName + " did not exit after Kill().");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("StopProcess failed for " + processName + ": " + ex);
                RaiseError(ex);
            }
        }

        private bool IsProcessAlive(Process process)
        {
            try
            {
                return process != null && !process.HasExited;
            }
            catch
            {
                return false;
            }
        }

        private int FindAvailableUdpPort(int preferredPort, int maxAttempts)
        {
            if (preferredPort <= 0)
                preferredPort = 12002;

            if (maxAttempts <= 0)
                maxAttempts = 50;

            int port = preferredPort;

            for (int i = 0; i < maxAttempts; i++)
            {
                if (IsUdpPortAvailable(port))
                    return port;

                Debug.WriteLine("UDP port " + port + " is busy, trying next recording port.");

                port += 2;
            }

            return preferredPort;
        }

        private bool IsUdpPortAvailable(int port)
        {
            Socket socket = null;

            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.ExclusiveAddressUse = false;

                socket.SetSocketOption(
                    SocketOptionLevel.Socket,
                    SocketOptionName.ReuseAddress,
                    true
                );

                socket.Bind(new IPEndPoint(IPAddress.Any, port));

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                try
                {
                    if (socket != null)
                        socket.Close();
                }
                catch
                {
                }

                try
                {
                    if (socket != null)
                        socket.Dispose();
                }
                catch
                {
                }
            }
        }

        private void AdoptFfplayWindow(IntPtr windowHandle)
        {
            try
            {
                if (windowHandle == IntPtr.Zero)
                    return;

                if (_hostWindowHandle == IntPtr.Zero)
                    return;

                EmbedWindow(windowHandle, _hostWindowHandle);

                if (_hostWidth <= 0 || _hostHeight <= 0)
                {
                    RECT hostRect;

                    if (GetWindowRect(_hostWindowHandle, out hostRect))
                    {
                        _hostWidth = Math.Max(1, hostRect.Right - hostRect.Left);
                        _hostHeight = Math.Max(1, hostRect.Bottom - hostRect.Top);
                    }
                }

                Resize(_hostWidth, _hostHeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("AdoptFfplayWindow failed: " + ex);
                RaiseError(ex);

                ShowDebugMessageBox(
                    "Stream debug - AdoptFfplayWindow exception",
                    ex.ToString(),
                    true
                );
            }
        }

        private Rectangle GetContainedVideoRectangle(int hostWidth, int hostHeight)
        {
            if (_options.FitMode == VideoFitMode.Stretch)
                return new Rectangle(0, 0, hostWidth, hostHeight);

            if (_options.FitMode == VideoFitMode.OriginalStreamSize)
            {
                int originalX = (hostWidth - _options.StreamWidth) / 2;
                int originalY = (hostHeight - _options.StreamHeight) / 2;

                return new Rectangle(
                    originalX,
                    originalY,
                    _options.StreamWidth,
                    _options.StreamHeight
                );
            }

            int sourceWidth = _options.StreamWidth > 0 ? _options.StreamWidth : hostWidth;
            int sourceHeight = _options.StreamHeight > 0 ? _options.StreamHeight : hostHeight;

            double sourceAspect = (double)sourceWidth / sourceHeight;
            double hostAspect = (double)hostWidth / hostHeight;

            int targetWidth;
            int targetHeight;

            if (hostAspect > sourceAspect)
            {
                targetHeight = hostHeight;
                targetWidth = (int)(targetHeight * sourceAspect);
            }
            else
            {
                targetWidth = hostWidth;
                targetHeight = (int)(targetWidth / sourceAspect);
            }

            if (targetWidth < 1)
                targetWidth = 1;

            if (targetHeight < 1)
                targetHeight = 1;

            int targetX = (hostWidth - targetWidth) / 2;
            int targetY = (hostHeight - targetHeight) / 2;

            return new Rectangle(targetX, targetY, targetWidth, targetHeight);
        }

        private string BuildFfplayArguments()
        {
            string syncMode = _options.SyncMode == FfplaySyncMode.External
                ? "ext"
                : "video";

            string displayUrl = "rtp://0.0.0.0:" + _options.LocalDisplayRtpPort;

            string args =
                "-hide_banner " +
                "-loglevel warning " +
                "-fflags nobuffer " +
                "-flags low_delay " +
                "-framedrop " +
                "-sync " + syncMode + " " +
                "-x " + _options.StreamWidth + " " +
                "-y " + _options.StreamHeight + " ";

            if (_options.DisableAudio)
                args += "-an ";

            if (_options.UseAvioDirect)
                args += "-avioflags direct ";

            if (_options.UseMaxDelayZero)
                args += "-max_delay 0 ";

            if (_options.UseSetPtsZero)
                args += "-vf \"setpts=0\" ";

            args +=
                "-noborder " +
                "\"" + displayUrl + "\"";

            return args;
        }

        private string BuildSegmentRecordingArguments(
            string outputDirectory,
            int segmentLengthSeconds,
            string filePrefix,
            int recordingRtpPort)
        {
            string recordUrl = "rtp://0.0.0.0:" + recordingRtpPort;

            string safePrefix = MakeSafeFileName(filePrefix);
            string extension = string.IsNullOrWhiteSpace(_options.RecordingExtension)
                ? "mkv"
                : _options.RecordingExtension.TrimStart('.');

            string outputPattern = Path.Combine(
                outputDirectory,
                safePrefix + "_%03d." + extension
            );

            string args =
                "-hide_banner " +
                "-loglevel warning " +
                "-fflags nobuffer " +
                "-flags low_delay " +
                "-i \"" + recordUrl + "\" " +
                "-an ";

            if (_options.RecordWithStreamCopy)
                args += "-c:v copy ";
            else
                args += "-c:v libx264 -preset ultrafast -tune zerolatency ";

            args +=
                "-f segment " +
                "-segment_time " + segmentLengthSeconds + " " +
                "-reset_timestamps 1 " +
                "\"" + outputPattern + "\"";

            return args;
        }

        private async Task<IntPtr> WaitForVisibleFfplayWindowAsync(Process process, int timeoutMs)
        {
            if (process == null)
                return IntPtr.Zero;

            Stopwatch sw = Stopwatch.StartNew();

            while (sw.ElapsedMilliseconds < timeoutMs)
            {
                try
                {
                    if (process.HasExited)
                        return IntPtr.Zero;

                    process.Refresh();

                    IntPtr mainWindow = process.MainWindowHandle;

                    if (mainWindow != IntPtr.Zero && IsWindowVisible(mainWindow))
                    {
                        Debug.WriteLine("ffplay MainWindowHandle found: " + mainWindow);
                        return mainWindow;
                    }

                    IntPtr bestWindow = FindVisibleWindowForProcess(process.Id);

                    if (bestWindow != IntPtr.Zero)
                        return bestWindow;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("WaitForVisibleFfplayWindowAsync polling failed: " + ex.Message);
                }

                await Task.Delay(50);
            }

            return IntPtr.Zero;
        }

        private IntPtr FindVisibleWindowForProcess(int processId)
        {
            IntPtr foundWindow = IntPtr.Zero;

            try
            {
                EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
                {
                    int windowProcessId;
                    GetWindowThreadProcessId(hWnd, out windowProcessId);

                    if (windowProcessId != processId)
                        return true;

                    bool visible = IsWindowVisible(hWnd);
                    IntPtr parent = GetParent(hWnd);
                    string className = GetClassNameSafe(hWnd);
                    string title = GetWindowTextSafe(hWnd);

                    Debug.WriteLine(
                        "ffplay window candidate: handle=" + hWnd +
                        ", visible=" + visible +
                        ", parent=" + parent +
                        ", class='" + className +
                        "', title='" + title + "'"
                    );

                    if (!visible)
                        return true;

                    if (className.IndexOf("SDL", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        foundWindow = hWnd;
                        return false;
                    }

                    if (parent == IntPtr.Zero)
                    {
                        foundWindow = hWnd;
                        return false;
                    }

                    return true;
                }, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("FindVisibleWindowForProcess failed: " + ex);
                RaiseError(ex);
            }

            return foundWindow;
        }

        private void EmbedWindow(IntPtr childHandle, IntPtr parentHandle)
        {
            try
            {
                if (childHandle == IntPtr.Zero || parentHandle == IntPtr.Zero)
                    return;

                ShowWindow(childHandle, SW_RESTORE);
                ShowWindow(childHandle, SW_SHOW);

                SetParent(childHandle, parentHandle);

                IntPtr stylePtr = GetWindowLongPtr(childHandle, GWL_STYLE);
                long style = stylePtr.ToInt64();

                style &= ~WS_POPUP;
                style |= WS_CHILD;
                style |= WS_VISIBLE;

                SetWindowLongPtr(childHandle, GWL_STYLE, new IntPtr(style));

                ShowWindow(childHandle, SW_SHOW);
                UpdateWindow(childHandle);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EmbedWindow failed: " + ex);
                RaiseError(ex);

                ShowDebugMessageBox(
                    "Stream debug - EmbedWindow exception",
                    ex.ToString(),
                    true
                );
            }
        }

        private static Rectangle GetWindowRectangle(IntPtr hWnd)
        {
            RECT rect;

            if (!GetWindowRect(hWnd, out rect))
                return Rectangle.Empty;

            return Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        private static void SaveBitmapByExtension(Bitmap bitmap, string outputFilePath)
        {
            string extension = Path.GetExtension(outputFilePath).ToLowerInvariant();

            ImageFormat format;

            if (extension == ".jpg" || extension == ".jpeg")
                format = ImageFormat.Jpeg;
            else if (extension == ".bmp")
                format = ImageFormat.Bmp;
            else if (extension == ".gif")
                format = ImageFormat.Gif;
            else if (extension == ".tif" || extension == ".tiff")
                format = ImageFormat.Tiff;
            else
                format = ImageFormat.Png;

            bitmap.Save(outputFilePath, format);
        }

        private static string MakeSafeFileName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "camera";

            char[] invalidChars = Path.GetInvalidFileNameChars();

            for (int i = 0; i < invalidChars.Length; i++)
                value = value.Replace(invalidChars[i], '_');

            return value;
        }

        private string GetFfplayWorkingDirectory()
        {
            string directory = Path.GetDirectoryName(_options.FfplayPath);

            return string.IsNullOrWhiteSpace(directory)
                ? AppDomain.CurrentDomain.BaseDirectory
                : directory;
        }

        private string GetFfmpegWorkingDirectory()
        {
            string directory = Path.GetDirectoryName(_options.FfmpegPath);

            return string.IsNullOrWhiteSpace(directory)
                ? AppDomain.CurrentDomain.BaseDirectory
                : directory;
        }

        private void ShowDebugMessageBox(string title, string message, bool important)
        {
            try
            {
                if (!_options.ShowDebugMessageBoxes)
                    return;

                if (!important)
                    return;

                if ((DateTime.UtcNow - _lastDebugMessageBoxUtc).TotalSeconds < 2)
                    return;

                _lastDebugMessageBoxUtc = DateTime.UtcNow;

                MessageBox.Show(
                    message,
                    title,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
            catch
            {
            }
        }

        private void FanOut_StatusChanged(object sender, string message)
        {
            Debug.WriteLine("FANOUT: " + message);
        }

        private void FanOut_ErrorOccurred(object sender, Exception ex)
        {
            Debug.WriteLine("FANOUT ERROR: " + ex);
            RaiseError(ex);

            ShowDebugMessageBox(
                "Stream debug - fan-out error",
                ex.ToString(),
                true
            );
        }

        private void FfplayProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Data))
                return;

            Debug.WriteLine("FFPLAY STDERR: " + e.Data);

            string lower = e.Data.ToLowerInvariant();

            bool fatalStartupError =
                lower.Contains("bind") ||
                lower.Contains("address already in use") ||
                lower.Contains("no such file") ||
                lower.Contains("permission denied") ||
                lower.Contains("error opening input") ||
                lower.Contains("could not open");

            if (fatalStartupError)
            {
                ShowDebugMessageBox(
                    "ffplay fatal stderr",
                    e.Data,
                    true
                );
            }
        }

        private void FfplayProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
                Debug.WriteLine("FFPLAY STDOUT: " + e.Data);
        }

        private void FfplayProcess_Exited(object sender, EventArgs e)
        {
            _ffplayWindow = IntPtr.Zero;

            if (_desiredDisplayRunning)
            {
                RaiseStatus("Display exited. Retrying...");
                StartRetryLoopIfNeeded();
            }
            else
            {
                RaiseStatus("Display stopped.");
            }
        }

        private void RecordingProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Data))
                return;

            Debug.WriteLine("FFMPEG RECORD STDERR: " + e.Data);

            string lower = e.Data.ToLowerInvariant();

            bool fatalStartupError =
                lower.Contains("bind") ||
                lower.Contains("address already in use") ||
                lower.Contains("no such file") ||
                lower.Contains("permission denied") ||
                lower.Contains("error opening input") ||
                lower.Contains("could not open");

            if (fatalStartupError)
            {
                ShowDebugMessageBox(
                    "ffmpeg recording fatal stderr",
                    e.Data,
                    true
                );
            }
        }

        private void RecordingProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
                Debug.WriteLine("FFMPEG RECORD STDOUT: " + e.Data);
        }

        private void RecordingProcess_Exited(object sender, EventArgs e)
        {
            lock (_recordingLock)
            {
                _recordingStartInProgress = false;
            }

            _activeRecordingRtpPort = -1;

            RaiseStatus("Segment recording stopped.");
        }

        private string GetClassNameSafe(IntPtr hWnd)
        {
            try
            {
                StringBuilder builder = new StringBuilder(256);
                GetClassName(hWnd, builder, builder.Capacity);
                return builder.ToString();
            }
            catch
            {
                return "";
            }
        }

        private string GetWindowTextSafe(IntPtr hWnd)
        {
            try
            {
                StringBuilder builder = new StringBuilder(512);
                GetWindowText(hWnd, builder, builder.Capacity);
                return builder.ToString();
            }
            catch
            {
                return "";
            }
        }

        private void RaiseStatus(string message)
        {
            EventHandler<string> handler = StatusChanged;

            if (handler != null)
                handler(this, message);
        }

        private void RaiseError(Exception ex)
        {
            EventHandler<Exception> handler = ErrorOccurred;

            if (handler != null && ex != null)
                handler(this, ex);
        }

        private const int GWL_STYLE = -16;

        private const long WS_CHILD = 0x40000000L;
        private const long WS_VISIBLE = 0x10000000L;
        private const long WS_POPUP = 0x80000000L;

        private const int SW_SHOW = 5;
        private const int SW_RESTORE = 9;

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(
            IntPtr hWnd,
            int X,
            int Y,
            int nWidth,
            int nHeight,
            bool bRepaint
        );

        [DllImport("user32.dll")]
        private static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int processId);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        private static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
                return GetWindowLongPtr64(hWnd, nIndex);

            return new IntPtr(GetWindowLong32(hWnd, nIndex));
        }

        private static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);

            return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true)]
        private static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
    }
}