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

namespace MissionPlanner.StreamViewer
{
    public sealed class FfplayFanoutCameraStreamPlayer : ICameraStreamPlayer
    {
        private readonly CameraStreamOptions _options;

        private readonly object _displayLock = new object();
        private readonly object _recordingLock = new object();

        private UdpPacketFanOut _rtpFanOut;

        private IPEndPoint _displayRtpEndpoint;
        private IPEndPoint _recordingRtpEndpoint;

        private Process _ffplayProcess;
        private Process _recordingProcess;

        private IntPtr _ffplayWindow = IntPtr.Zero;
        private IntPtr _hostWindowHandle = IntPtr.Zero;

        private int _hostWidth;
        private int _hostHeight;

        private static readonly IntPtr HWND_TOP = new IntPtr(0);

        private const long WS_CLIPSIBLINGS = 0x04000000L;

        private const int WM_SIZE = 0x0005;
        private const int WM_WINDOWPOSCHANGED = 0x0047;

        private const uint SWP_FRAMECHANGED = 0x0020;
        private const uint SWP_SHOWWINDOW = 0x0040;

        private const uint RDW_INVALIDATE = 0x0001;
        private const uint RDW_UPDATENOW = 0x0100;
        private const uint RDW_ALLCHILDREN = 0x0080;


        private volatile bool _desiredDisplayRunning;
        private volatile bool _displayStartInProgress;
        private volatile bool _retryLoopRunning;
        private volatile bool _recordingStartInProgress;

        public event EventHandler<string> StatusChanged;
        public event EventHandler<Exception> ErrorOccurred;

        public bool IsConnected
        {
            get
            {
                return _rtpFanOut != null && _rtpFanOut.IsRunning;
            }
        }

        public bool IsDisplayRunning
        {
            get
            {
                return IsConnected &&
                       IsProcessAlive(_ffplayProcess) &&
                       _ffplayWindow != IntPtr.Zero;
            }
        }

        public bool IsSegmentRecording
        {
            get
            {
                return IsProcessAlive(_recordingProcess);
            }
        }

        public FfplayFanoutCameraStreamPlayer(CameraStreamOptions options)
        {
            _options = options ?? new CameraStreamOptions();
        }

        public Task ConnectAsync()
        {
            try
            {
                StartFanOutIfNeeded();
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

            await StartDisplayAttemptAsync();
        }

        public async Task RestartAsync(IntPtr hostWindowHandle)
        {
            _hostWindowHandle = hostWindowHandle;
            _desiredDisplayRunning = true;

            StopDisplay();

            if (_options.RestartDelayMs > 0)
                await Task.Delay(_options.RestartDelayMs);

            await StartDisplayAttemptAsync();
        }

        public void StopDisplay()
        {
            _desiredDisplayRunning = false;
            StopDisplayProcess();
        }

        public void Stop()
        {
            _desiredDisplayRunning = false;

            StopSegmentRecording();
            StopDisplayProcess();
            StopFanOut();

            RaiseStatus("Stream stopped.");
        }

        private const uint SWP_NOACTIVATE = 0x0010;

        public void Resize(int hostWidth, int hostHeight)
        {
            _hostWidth = hostWidth;
            _hostHeight = hostHeight;

            if (_ffplayWindow == IntPtr.Zero)
                return;

            if (hostWidth <= 0 || hostHeight <= 0)
                return;

            Rectangle target = GetVideoTargetRectangle(hostWidth, hostHeight);

            if (target.Width <= 0 || target.Height <= 0)
                return;

            Debug.WriteLine(
                "Resize ffplay window. " +
                "Host=" + hostWidth + "x" + hostHeight +
                ", Target=" + target.X + "," + target.Y + "," + target.Width + "x" + target.Height +
                ", FitMode=" + _options.FitMode
            );

            ApplyNativeVideoWindowPosition(target, true);
        }

        private void ApplyNativeVideoWindowPosition(Rectangle target, bool forceRefresh)
        {
            try
            {
                if (_ffplayWindow == IntPtr.Zero)
                    return;

                if (target.Width <= 0 || target.Height <= 0)
                    return;

                ShowWindow(_ffplayWindow, SW_SHOW);

                // Move the child window to the calculated contain rectangle.
                MoveWindow(
                    _ffplayWindow,
                    target.X,
                    target.Y,
                    target.Width,
                    target.Height,
                    true
                );

                // Make sure it is topmost within the parent, visible, and receives a proper frame update.
                SetWindowPos(
                    _ffplayWindow,
                    HWND_TOP,
                    target.X,
                    target.Y,
                    target.Width,
                    target.Height,
                    SWP_SHOWWINDOW | SWP_FRAMECHANGED
                );

                if (forceRefresh)
                {
                    // These mimic what happens when you click / resize the host window.
                    SendMessage(_ffplayWindow, WM_SIZE, IntPtr.Zero, MakeLParam(target.Width, target.Height));
                    SendMessage(_ffplayWindow, WM_WINDOWPOSCHANGED, IntPtr.Zero, IntPtr.Zero);

                    RedrawWindow(
                        _ffplayWindow,
                        IntPtr.Zero,
                        IntPtr.Zero,
                        RDW_INVALIDATE | RDW_UPDATENOW | RDW_ALLCHILDREN
                    );

                    if (_hostWindowHandle != IntPtr.Zero)
                    {
                        RedrawWindow(
                            _hostWindowHandle,
                            IntPtr.Zero,
                            IntPtr.Zero,
                            RDW_INVALIDATE | RDW_UPDATENOW | RDW_ALLCHILDREN
                        );
                    }
                }

                UpdateWindow(_ffplayWindow);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ApplyNativeVideoWindowPosition failed: " + ex);
                RaiseError(ex);
            }
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
                    return Task.FromResult(0);

                if (IsProcessAlive(_recordingProcess))
                    return Task.FromResult(0);

                _recordingStartInProgress = true;
            }

            bool endpointAdded = false;

            try
            {
                if (string.IsNullOrWhiteSpace(_options.FfmpegPath) || !File.Exists(_options.FfmpegPath))
                {
                    RaiseStatus("Recording not started: invalid ffmpeg path.");
                    return Task.FromResult(0);
                }

                if (string.IsNullOrWhiteSpace(outputDirectory))
                {
                    RaiseStatus("Recording not started: output directory is empty.");
                    return Task.FromResult(0);
                }

                if (segmentLengthSeconds <= 0)
                    segmentLengthSeconds = 60;

                Directory.CreateDirectory(outputDirectory);

                StartFanOutIfNeeded();

                IPAddress localAddress = GetLocalForwardAddress();

                int recordingPort = FindAvailableUdpPort(_options.LocalRecordingRtpPort, 50);

                _recordingRtpEndpoint = new IPEndPoint(localAddress, recordingPort);

                if (_rtpFanOut != null)
                {
                    _rtpFanOut.AddOutputEndpoint(_recordingRtpEndpoint);
                    endpointAdded = true;
                }

                string args = BuildSegmentRecordingArguments(
                    outputDirectory,
                    segmentLengthSeconds,
                    string.IsNullOrWhiteSpace(filePrefix) ? "streamRecord" : filePrefix,
                    recordingPort
                );

                Debug.WriteLine("========== STARTING FFMPEG RECORDING ==========");
                Debug.WriteLine("Path: " + _options.FfmpegPath);
                Debug.WriteLine("Working directory: " + GetWorkingDirectory(_options.FfmpegPath));
                Debug.WriteLine("Args: " + args);
                Debug.WriteLine("===============================================");

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = _options.FfmpegPath,
                    Arguments = args,
                    WorkingDirectory = GetWorkingDirectory(_options.FfmpegPath),
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

                if (_recordingProcess.Start())
                {
                    _recordingProcess.BeginErrorReadLine();
                    _recordingProcess.BeginOutputReadLine();

                    RaiseStatus("Recording started. Segment length: " + segmentLengthSeconds + "s.");
                }
                else
                {
                    if (endpointAdded && _rtpFanOut != null)
                        _rtpFanOut.RemoveOutputEndpoint(_recordingRtpEndpoint);

                    RaiseStatus("Recording did not start.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("StartSegmentRecordingAsync failed: " + ex);
                RaiseError(ex);

                if (endpointAdded && _rtpFanOut != null && _recordingRtpEndpoint != null)
                    _rtpFanOut.RemoveOutputEndpoint(_recordingRtpEndpoint);

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

            if (_rtpFanOut != null && _recordingRtpEndpoint != null)
                _rtpFanOut.RemoveOutputEndpoint(_recordingRtpEndpoint);

            _recordingRtpEndpoint = null;

            RaiseStatus("Recording stopped.");
        }

        public void Dispose()
        {
            Stop();
        }

        private async Task StartDisplayAttemptAsync()
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
                    RaiseStatus("Display waiting for host window.");
                    StartRetryLoopIfNeeded();
                    return;
                }

                if (string.IsNullOrWhiteSpace(_options.FfplayPath) || !File.Exists(_options.FfplayPath))
                {
                    RaiseStatus("Display not started: invalid ffplay path.");
                    StartRetryLoopIfNeeded();
                    return;
                }

                StopDisplayProcess();

                string args = BuildFfplayArguments();

                Debug.WriteLine("========== STARTING FFPLAY DISPLAY ==========");
                Debug.WriteLine("Path: " + _options.FfplayPath);
                Debug.WriteLine("Working directory: " + GetWorkingDirectory(_options.FfplayPath));
                Debug.WriteLine("Args: " + args);
                Debug.WriteLine("WindowFindTimeoutMs: " + _options.WindowFindTimeoutMs);
                Debug.WriteLine("=============================================");

                StartFfplayProcess(args);

                if (!IsProcessAlive(_ffplayProcess))
                {
                    RaiseStatus("ffplay did not start.");
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
                        "RTP received=" + (_rtpFanOut != null ? _rtpFanOut.ReceivedPacketCount : 0) +
                        ", forwarded=" + (_rtpFanOut != null ? _rtpFanOut.ForwardedPacketCount : 0) +
                        ", dropped=" + (_rtpFanOut != null ? _rtpFanOut.DroppedPacketCount : 0) +
                        ", ignored=" + (_rtpFanOut != null ? _rtpFanOut.IgnoredPacketCount : 0);

                    Debug.WriteLine("Visible ffplay window not found. " + packetInfo);

                    StopDisplayProcess();
                    RaiseStatus("Display waiting for camera stream.");
                    StartRetryLoopIfNeeded();
                    return;
                }

                EmbedFfplayWindow(_ffplayWindow, _hostWindowHandle);

                if (_hostWidth <= 0 || _hostHeight <= 0)
                {
                    RECT rect;

                    if (GetWindowRect(_hostWindowHandle, out rect))
                    {
                        _hostWidth = Math.Max(1, rect.Right - rect.Left);
                        _hostHeight = Math.Max(1, rect.Bottom - rect.Top);
                    }
                }

                Resize(_hostWidth, _hostHeight);

                RaiseStatus("Display streaming.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("StartDisplayAttemptAsync failed: " + ex);
                RaiseError(ex);

                StopDisplayProcess();
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
                    while (_desiredDisplayRunning && !IsDisplayRunning)
                    {
                        int delay = _options.SilentRetryDelayMs > 0
                            ? _options.SilentRetryDelayMs
                            : 5000;

                        await Task.Delay(delay);

                        if (!_desiredDisplayRunning || IsDisplayRunning)
                            break;

                        await StartDisplayAttemptAsync();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Display retry loop failed: " + ex);
                    RaiseError(ex);
                }
                finally
                {
                    _retryLoopRunning = false;

                    if (_desiredDisplayRunning && !IsDisplayRunning)
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

                IPAddress localAddress = GetLocalForwardAddress();

                _displayRtpEndpoint = new IPEndPoint(localAddress, _options.LocalDisplayRtpPort);

                _rtpFanOut = new UdpPacketFanOut(
                    "RTP",
                    _options.LocalBindIpAddress,
                    _options.CameraSourceIpAddress,
                    _options.CameraRtpPort,
                    _options.FanOutReceiveBufferBytes,
                    _options.FanOutSoftDropEnabled,
                    _options.FanOutBacklogDropThresholdBytes,
                    _options.FanOutMaxDrainPackets,
                    _displayRtpEndpoint
                );

                _rtpFanOut.StatusChanged += FanOut_StatusChanged;
                _rtpFanOut.ErrorOccurred += FanOut_ErrorOccurred;

                _rtpFanOut.Start();

                RaiseStatus("Stream receiver connected.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("StartFanOutIfNeeded failed: " + ex);
                RaiseError(ex);
            }
        }

        private void StopFanOut()
        {
            try
            {
                if (_rtpFanOut != null)
                {
                    _rtpFanOut.StatusChanged -= FanOut_StatusChanged;
                    _rtpFanOut.ErrorOccurred -= FanOut_ErrorOccurred;
                    _rtpFanOut.Dispose();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("StopFanOut failed: " + ex);
                RaiseError(ex);
            }

            _rtpFanOut = null;
            _displayRtpEndpoint = null;
            _recordingRtpEndpoint = null;
        }

        private void StartFfplayProcess(string args)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = _options.FfplayPath,
                    Arguments = args,
                    WorkingDirectory = GetWorkingDirectory(_options.FfplayPath),
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

                if (_ffplayProcess.Start())
                {
                    _ffplayProcess.BeginErrorReadLine();
                    _ffplayProcess.BeginOutputReadLine();

                    Debug.WriteLine("ffplay started. PID=" + _ffplayProcess.Id);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("StartFfplayProcess failed: " + ex);
                RaiseError(ex);
            }
        }

        private void StopDisplayProcess()
        {
            try
            {
                StopProcess(_ffplayProcess, "ffplay");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("StopDisplayProcess failed: " + ex);
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

        private void StopProcess(Process process, string name)
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
                    Debug.WriteLine(name + " did not exit after Kill().");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("StopProcess failed for " + name + ": " + ex);
                RaiseError(ex);
            }
        }

        private async Task<IntPtr> WaitForVisibleFfplayWindowAsync(Process process, int timeoutMs)
        {
            if (process == null)
                return IntPtr.Zero;

            if (timeoutMs <= 0)
                timeoutMs = 15000;

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
                        return mainWindow;

                    IntPtr enumWindow = FindVisibleWindowForProcess(process.Id);

                    if (enumWindow != IntPtr.Zero)
                        return enumWindow;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("WaitForVisibleFfplayWindowAsync failed: " + ex.Message);
                }

                await Task.Delay(100);
            }

            return IntPtr.Zero;
        }

        private IntPtr FindVisibleWindowForProcess(int processId)
        {
            IntPtr found = IntPtr.Zero;

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
                        found = hWnd;
                        return false;
                    }

                    if (parent == IntPtr.Zero)
                    {
                        found = hWnd;
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

            return found;
        }

        private void EmbedFfplayWindow(IntPtr childHandle, IntPtr parentHandle)
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
                style |= WS_CLIPSIBLINGS;

                SetWindowLongPtr(childHandle, GWL_STYLE, new IntPtr(style));

                SetWindowPos(
                    childHandle,
                    HWND_TOP,
                    0,
                    0,
                    1,
                    1,
                    SWP_SHOWWINDOW | SWP_FRAMECHANGED
                );

                ShowWindow(childHandle, SW_SHOW);
                UpdateWindow(childHandle);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EmbedFfplayWindow failed: " + ex);
                RaiseError(ex);
            }
        }

        private Rectangle GetVideoTargetRectangle(int hostWidth, int hostHeight)
        {
            if (hostWidth <= 0 || hostHeight <= 0)
                return Rectangle.Empty;

            if (_options.FitMode == VideoFitMode.Stretch)
                return new Rectangle(0, 0, hostWidth, hostHeight);

            int sourceWidth = _options.StreamWidth > 0
                ? _options.StreamWidth
                : hostWidth;

            int sourceHeight = _options.StreamHeight > 0
                ? _options.StreamHeight
                : hostHeight;

            if (_options.FitMode == VideoFitMode.OriginalStreamSize)
            {
                int originalX = (hostWidth - sourceWidth) / 2;
                int originalY = (hostHeight - sourceHeight) / 2;

                return new Rectangle(
                    originalX,
                    originalY,
                    sourceWidth,
                    sourceHeight
                );
            }

            double sourceAspect = (double)sourceWidth / (double)sourceHeight;
            double hostAspect = (double)hostWidth / (double)hostHeight;

            int targetWidth;
            int targetHeight;

            if (hostAspect > sourceAspect)
            {
                targetHeight = hostHeight;
                targetWidth = (int)Math.Round(targetHeight * sourceAspect);
            }
            else
            {
                targetWidth = hostWidth;
                targetHeight = (int)Math.Round(targetWidth / sourceAspect);
            }

            if (targetWidth < 1)
                targetWidth = 1;

            if (targetHeight < 1)
                targetHeight = 1;

            if (targetWidth > hostWidth)
                targetWidth = hostWidth;

            if (targetHeight > hostHeight)
                targetHeight = hostHeight;

            int targetX = (hostWidth - targetWidth) / 2;
            int targetY = (hostHeight - targetHeight) / 2;

            return new Rectangle(
                targetX,
                targetY,
                targetWidth,
                targetHeight
            );
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

            args += "-noborder \"" + displayUrl + "\"";

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

        private IPAddress GetLocalForwardAddress()
        {
            IPAddress localAddress;

            if (!IPAddress.TryParse(_options.LocalHost, out localAddress))
                localAddress = IPAddress.Loopback;

            return localAddress;
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
                socket.ExclusiveAddressUse = true;
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
                return "streamRecord";

            char[] invalidChars = Path.GetInvalidFileNameChars();

            for (int i = 0; i < invalidChars.Length; i++)
                value = value.Replace(invalidChars[i], '_');

            return value;
        }

        private static string GetWorkingDirectory(string executablePath)
        {
            string directory = Path.GetDirectoryName(executablePath);

            if (string.IsNullOrWhiteSpace(directory))
                return AppDomain.CurrentDomain.BaseDirectory;

            return directory;
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

        private void FanOut_StatusChanged(object sender, string message)
        {
            Debug.WriteLine("FANOUT: " + message);
        }

        private void FanOut_ErrorOccurred(object sender, Exception ex)
        {
            Debug.WriteLine("FANOUT ERROR: " + ex);
            RaiseError(ex);
        }

        private void FfplayProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Data))
                return;

            Debug.WriteLine("FFPLAY STDERR: " + e.Data);
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
                RaiseStatus("Display exited. Retrying.");
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

            if (_rtpFanOut != null && _recordingRtpEndpoint != null)
                _rtpFanOut.RemoveOutputEndpoint(_recordingRtpEndpoint);

            _recordingRtpEndpoint = null;

            RaiseStatus("Recording exited.");
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

        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOZORDER = 0x0004;        

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

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(
    IntPtr hWnd,
    IntPtr hWndInsertAfter,
    int X,
    int Y,
    int cx,
    int cy,
    uint uFlags
);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SendMessage(
            IntPtr hWnd,
            int Msg,
            IntPtr wParam,
            IntPtr lParam
        );

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RedrawWindow(
            IntPtr hWnd,
            IntPtr lprcUpdate,
            IntPtr hrgnUpdate,
            uint flags
        );

        private static IntPtr MakeLParam(int lowWord, int highWord)
        {
            int value = (highWord << 16) | (lowWord & 0xFFFF);
            return new IntPtr(value);
        }

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