namespace MissionPlanner.StreamViewer
{
    public enum VideoFitMode
    {
        FitKeepAspectRatio,
        Stretch,
        OriginalStreamSize
    }

    public enum FfplaySyncMode
    {
        Video,
        External
    }

    public sealed class CameraStreamOptions
    {
        public string FfplayPath { get; set; }
        public string FfmpegPath { get; set; }

        // Example input after parsing:
        // CameraSourceIpAddress = "192.168.73.100"
        // CameraRtpPort = 11024
        //
        // For debugging, you may set CameraSourceIpAddress = ""
        // to accept packets from any sender.
        public string CameraSourceIpAddress { get; set; }

        // Usually keep this as "0.0.0.0".
        public string LocalBindIpAddress { get; set; }

        public int CameraRtpPort { get; set; }

        public int LocalDisplayRtpPort { get; set; }
        public int LocalRecordingRtpPort { get; set; }

        public string LocalHost { get; set; }

        public int StreamWidth { get; set; }
        public int StreamHeight { get; set; }

        public VideoFitMode FitMode { get; set; }
        public FfplaySyncMode SyncMode { get; set; }

        public bool UseSetPtsZero { get; set; }
        public bool UseMaxDelayZero { get; set; }
        public bool UseAvioDirect { get; set; }

        public bool DisableAudio { get; set; }
        public bool HideConsoleWindow { get; set; }

        public int RestartDelayMs { get; set; }
        public int WindowFindTimeoutMs { get; set; }
        public int SilentRetryDelayMs { get; set; }

        public string RecordingExtension { get; set; }
        public bool RecordWithStreamCopy { get; set; }

        public int FanOutReceiveBufferBytes { get; set; }
        public bool FanOutSoftDropEnabled { get; set; }
        public int FanOutBacklogDropThresholdBytes { get; set; }
        public int FanOutMaxDrainPackets { get; set; }

        public bool AutoConnectStream { get; set; }
        public bool AutoDisplayStream { get; set; }
        public bool AutoStartRecording { get; set; }

        public string AutoRecordingDirectory { get; set; }
        public int AutoRecordingSegmentSeconds { get; set; }
        public string AutoRecordingFilePrefix { get; set; }

        // Temporary debug switch.
        // Set false for production.
        public bool ShowDebugMessageBoxes { get; set; }

        public CameraStreamOptions()
        {
            FfplayPath = "";
            FfmpegPath = "";

            CameraSourceIpAddress = "";
            LocalBindIpAddress = "0.0.0.0";

            CameraRtpPort = 11024;

            LocalDisplayRtpPort = 12000;
            LocalRecordingRtpPort = 12002;

            LocalHost = "127.0.0.1";

            StreamWidth = 1920;
            StreamHeight = 1080;

            FitMode = VideoFitMode.FitKeepAspectRatio;
            SyncMode = FfplaySyncMode.External;

            UseSetPtsZero = true;
            UseMaxDelayZero = true;
            UseAvioDirect = false;

            DisableAudio = true;
            HideConsoleWindow = true;

            RestartDelayMs = 300;
            WindowFindTimeoutMs = 3000;
            SilentRetryDelayMs = 3000;

            RecordingExtension = "mkv";
            RecordWithStreamCopy = true;

            FanOutReceiveBufferBytes = 1024 * 1024;
            FanOutSoftDropEnabled = true;
            FanOutBacklogDropThresholdBytes = 512 * 1024;
            FanOutMaxDrainPackets = 16;

            AutoConnectStream = false;
            AutoDisplayStream = false;
            AutoStartRecording = false;

            AutoRecordingDirectory = "";
            AutoRecordingSegmentSeconds = 10;
            AutoRecordingFilePrefix = "camera";

            ShowDebugMessageBoxes = false;
        }
    }
}