using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

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
                throw new ArgumentNullException("options");

            _options = options;
        }

        public async Task StartAsync(IntPtr hostWindowHandle)
        {
            if (hostWindowHandle == IntPtr.Zero)
                throw new ArgumentException("Host window handle cannot be zero.", "hostWindowHandle");

            _hostWindowHandle = hostWindowHandle;

            if (!File.Exists(_options.FfplayPath))
                throw new FileNotFoundException("ffplay.exe was not found.", _options.FfplayPath);

            try
            {
                StartFanOutIfNeeded();

                StopDisplayOnly();

                StartFfplayDisplay();

                _ffplayWindow = await WaitForVisibleFfplayWindowAsync(
                    _ffplayProcess,
                    _options.WindowFindTimeoutMs
                );

                if (_ffplayWindow == IntPtr.Zero)
                {
                    StopDisplayOnly();

                    string packetInfo =
                        "RTP packets received: " + (_rtpFanOut != null ? _rtpFanOut.ReceivedPacketCount : 0) +
                        ", forwarded: " + (_rtpFanOut != null ? _rtpFanOut.ForwardedPacketCount : 0) +
                        ", dropped: " + (_rtpFanOut != null ? _rtpFanOut.DroppedPacketCount : 0);

                    throw new InvalidOperationException(
                        "Could not find ffplay video window after starting local fan-out.\n\n" +
                        packetInfo + "\n\n" +
                        "If RTP packets are 0, the camera is not reaching the C# fan-out.\n" +
                        "If RTP packets are increasing but ffplay has no window, the local forwarded RTP stream is not being recognized by ffplay."
                    );
                }

                EmbedWindow(_ffplayWindow, _hostWindowHandle);
                Resize(_hostWidth, _hostHeight);

                if (IsSegmentRecording)
                    RaiseStatus("Display streaming and recording.");
                else
                    RaiseStatus("Display streaming.");
            }
            catch (Exception ex)
            {
                RaiseError(ex);
                throw;
            }
        }

        public async Task RestartAsync(IntPtr hostWindowHandle)
        {
            StopDisplayOnly();

            if (_options.RestartDelayMs > 0)
            {
                RaiseStatus("Restarting display...");
                await Task.Delay(_options.RestartDelayMs);
            }

            await StartAsync(hostWindowHandle);
        }

        public void Stop()
        {
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

            int targetX = 0;
            int targetY = 0;
            int targetWidth = hostWidth;
            int targetHeight = hostHeight;

            if (_options.FitMode == VideoFitMode.OriginalStreamSize)
            {
                targetWidth = _options.StreamWidth;
                targetHeight = _options.StreamHeight;
                targetX = Math.Max(0, (hostWidth - targetWidth) / 2);
                targetY = Math.Max(0, (hostHeight - targetHeight) / 2);
            }
            else if (_options.FitMode == VideoFitMode.FitKeepAspectRatio)
            {
                double streamAspect = (double)_options.StreamWidth / _options.StreamHeight;
                double panelAspect = (double)hostWidth / hostHeight;

                if (panelAspect > streamAspect)
                {
                    targetHeight = hostHeight;
                    targetWidth = (int)(targetHeight * streamAspect);
                }
                else
                {
                    targetWidth = hostWidth;
                    targetHeight = (int)(targetWidth / streamAspect);
                }

                targetX = (hostWidth - targetWidth) / 2;
                targetY = (hostHeight - targetHeight) / 2;
            }

            MoveWindow(
                _ffplayWindow,
                targetX,
                targetY,
                targetWidth,
                targetHeight,
                true
            );

            UpdateWindow(_ffplayWindow);
        }

        public async Task CaptureCurrentFrameAsync(string outputFilePath)
        {
            if (_hostWindowHandle == IntPtr.Zero)
                throw new InvalidOperationException("Cannot capture frame because the host window is not available.");

            if (string.IsNullOrWhiteSpace(outputFilePath))
                throw new ArgumentException("Output file path cannot be empty.", "outputFilePath");

            string directory = Path.GetDirectoryName(outputFilePath);

            if (!string.IsNullOrWhiteSpace(directory))
                Directory.CreateDirectory(directory);

            await Task.Run(delegate
            {
                Rectangle bounds = GetWindowRectangle(_hostWindowHandle);

                if (bounds.Width <= 0 || bounds.Height <= 0)
                    throw new InvalidOperationException("Cannot capture frame because the host window has invalid size.");

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

        public Task StartSegmentRecordingAsync(
            string outputDirectory,
            int segmentLengthSeconds,
            string filePrefix)
        {
            if (IsSegmentRecording)
                throw new InvalidOperationException("Segment recording is already running.");

            if (!File.Exists(_options.FfmpegPath))
                throw new FileNotFoundException("ffmpeg.exe was not found.", _options.FfmpegPath);

            if (segmentLengthSeconds <= 0)
                throw new ArgumentOutOfRangeException("segmentLengthSeconds", "Segment length must be greater than zero.");

            if (string.IsNullOrWhiteSpace(outputDirectory))
                throw new ArgumentException("Output directory cannot be empty.", "outputDirectory");

            Directory.CreateDirectory(outputDirectory);

            try
            {
                StartFanOutIfNeeded();

                if (_rtpFanOut != null && _recordingRtpEndpoint != null)
                    _rtpFanOut.AddOutputEndpoint(_recordingRtpEndpoint);

                string args = BuildSegmentRecordingArguments(
                    outputDirectory,
                    segmentLengthSeconds,
                    filePrefix
                );

                RaiseStatus("Starting segment recording...");

                Debug.WriteLine("========== STARTING FFMPEG SEGMENT RECORDING ==========");
                Debug.WriteLine("Path: " + _options.FfmpegPath);
                Debug.WriteLine("Working Directory: " + GetFfmpegWorkingDirectory());
                Debug.WriteLine("Args: " + args);
                Debug.WriteLine("=======================================================");

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

                if (!started)
                    throw new InvalidOperationException("Could not start ffmpeg recording process.");

                _recordingProcess.BeginErrorReadLine();
                _recordingProcess.BeginOutputReadLine();

                RaiseStatus("Recording segments. Segment length: " + segmentLengthSeconds + "s");
            }
            catch (Exception ex)
            {
                StopSegmentRecording();
                RaiseError(ex);
                throw;
            }

            return Task.FromResult(0);
        }

        public void StopSegmentRecording()
        {
            try
            {
                if (_recordingProcess != null && !_recordingProcess.HasExited)
                {
                    try
                    {
                        _recordingProcess.StandardInput.WriteLine("q");
                        _recordingProcess.StandardInput.Flush();
                    }
                    catch
                    {
                    }

                    if (!_recordingProcess.WaitForExit(2000))
                    {
                        _recordingProcess.Kill();
                        _recordingProcess.WaitForExit(1000);
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (_recordingProcess != null)
                {
                    _recordingProcess.Dispose();
                    _recordingProcess = null;
                }
            }

            if (_rtpFanOut != null && _recordingRtpEndpoint != null)
                _rtpFanOut.RemoveOutputEndpoint(_recordingRtpEndpoint);

            if (IsRunning)
                RaiseStatus("Recording stopped. Display still running.");
            else
                RaiseStatus("Recording stopped.");
        }

        public void Dispose()
        {
            Stop();
        }

        private void StartFanOutIfNeeded()
        {
            if (_rtpFanOut != null && _rtpFanOut.IsRunning)
                return;

            StopFanOut();

            IPAddress localAddress = IPAddress.Parse(_options.LocalHost);

            _displayRtpEndpoint = new IPEndPoint(localAddress, _options.LocalDisplayRtpPort);
            _recordingRtpEndpoint = new IPEndPoint(localAddress, _options.LocalRecordingRtpPort);

            _rtpFanOut = new UdpPacketFanOut(
                "RTP",
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
        }

        private void StopFanOut()
        {
            try
            {
                if (_rtpFanOut != null)
                    _rtpFanOut.Dispose();
            }
            catch
            {
            }

            _rtpFanOut = null;
        }

        private void StartFfplayDisplay()
        {
            string args = BuildFfplayArguments();

            Debug.WriteLine("========== STARTING FFPLAY DISPLAY ==========");
            Debug.WriteLine("Path: " + _options.FfplayPath);
            Debug.WriteLine("Working Directory: " + GetFfplayWorkingDirectory());
            Debug.WriteLine("Args: " + args);
            Debug.WriteLine("=============================================");

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

            if (!started)
                throw new InvalidOperationException("Could not start ffplay display process.");

            _ffplayProcess.BeginErrorReadLine();
            _ffplayProcess.BeginOutputReadLine();
        }

        private void StopDisplayOnly()
        {
            try
            {
                if (_ffplayProcess != null && !_ffplayProcess.HasExited)
                {
                    _ffplayProcess.Kill();
                    _ffplayProcess.WaitForExit(1000);
                }
            }
            catch
            {
            }
            finally
            {
                if (_ffplayProcess != null)
                {
                    _ffplayProcess.Dispose();
                    _ffplayProcess = null;
                }

                _ffplayWindow = IntPtr.Zero;
            }
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
            string filePrefix)
        {
            string recordUrl = "rtp://0.0.0.0:" + _options.LocalRecordingRtpPort;

            string safePrefix = MakeSafeFileName(filePrefix);
            string extension = string.IsNullOrWhiteSpace(_options.RecordingExtension)
                ? "mkv"
                : _options.RecordingExtension.TrimStart('.');

            string outputPattern = Path.Combine(
                outputDirectory,
                safePrefix + "_%Y%m%d_%H%M%S." + extension
            );

            string args =
                "-hide_banner " +
                "-loglevel warning " +
                "-fflags nobuffer " +
                "-flags low_delay " +
                "-i \"" + recordUrl + "\" " +
                "-an ";

            if (_options.RecordWithStreamCopy)
            {
                args += "-c:v copy ";
            }
            else
            {
                args += "-c:v libx264 -preset ultrafast -tune zerolatency ";
            }

            args +=
                "-f segment " +
                "-segment_time " + segmentLengthSeconds + " " +
                "-reset_timestamps 1 " +
                "-strftime 1 " +
                "\"" + outputPattern + "\"";

            return args;
        }

        private async Task<IntPtr> WaitForVisibleFfplayWindowAsync(Process process, int timeoutMs)
        {
            Stopwatch sw = Stopwatch.StartNew();

            while (sw.ElapsedMilliseconds < timeoutMs)
            {
                if (process.HasExited)
                    return IntPtr.Zero;

                IntPtr visibleWindow = FindVisibleWindowForProcess(process.Id);

                if (visibleWindow != IntPtr.Zero)
                    return visibleWindow;

                await Task.Delay(50);
            }

            return IntPtr.Zero;
        }

        private IntPtr FindVisibleWindowForProcess(int processId)
        {
            IntPtr foundWindow = IntPtr.Zero;

            EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
            {
                int windowProcessId;
                GetWindowThreadProcessId(hWnd, out windowProcessId);

                if (windowProcessId != processId)
                    return true;

                if (!IsWindowVisible(hWnd))
                    return true;

                if (GetParent(hWnd) == IntPtr.Zero)
                {
                    foundWindow = hWnd;
                    return false;
                }

                return true;
            }, IntPtr.Zero);

            return foundWindow;
        }

        private void EmbedWindow(IntPtr childHandle, IntPtr parentHandle)
        {
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

        private static Rectangle GetWindowRectangle(IntPtr hWnd)
        {
            RECT rect;

            if (!GetWindowRect(hWnd, out rect))
                throw new InvalidOperationException("Could not get host window rectangle.");

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
            {
                value = value.Replace(invalidChars[i], '_');
            }

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
            if (!string.IsNullOrWhiteSpace(e.Data))
                Debug.WriteLine("FFPLAY STDERR: " + e.Data);
        }

        private void FfplayProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
                Debug.WriteLine("FFPLAY STDOUT: " + e.Data);
        }

        private void FfplayProcess_Exited(object sender, EventArgs e)
        {
            RaiseStatus("Display stopped.");
        }

        private void RecordingProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
                Debug.WriteLine("FFMPEG RECORD STDERR: " + e.Data);
        }

        private void RecordingProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
                Debug.WriteLine("FFMPEG RECORD STDOUT: " + e.Data);
        }

        private void RecordingProcess_Exited(object sender, EventArgs e)
        {
            RaiseStatus("Segment recording stopped.");
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

            if (handler != null)
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