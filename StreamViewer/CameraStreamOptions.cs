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

        public string RecordingExtension { get; set; }
        public bool RecordWithStreamCopy { get; set; }

        // Larger than before. This reduces excessive drops.
        public int FanOutReceiveBufferBytes { get; set; }

        // Soft dropping means:
        // forward every packet normally, only drop if socket backlog grows too large.
        public bool FanOutSoftDropEnabled { get; set; }

        // If socket.Available exceeds this, the fan-out drains a few old packets.
        public int FanOutBacklogDropThresholdBytes { get; set; }

        // Maximum packets to drain when backlog is too high.
        public int FanOutMaxDrainPackets { get; set; }

        public CameraStreamOptions()
        {
            FfplayPath = @"C:\Users\zoltan.kovacs\Downloads\ffmpeg-8.1.1-full_build\ffmpeg-8.1.1-full_build\bin\ffplay.exe";
            FfmpegPath = @"C:\Users\zoltan.kovacs\Downloads\ffmpeg-8.1.1-full_build\ffmpeg-8.1.1-full_build\bin\ffmpeg.exe";

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
            WindowFindTimeoutMs = 10000;

            RecordingExtension = "mkv";
            RecordWithStreamCopy = true;

            // Softer defaults.
            FanOutReceiveBufferBytes = 1024 * 1024;
            FanOutSoftDropEnabled = true;
            FanOutBacklogDropThresholdBytes = 512 * 1024;
            FanOutMaxDrainPackets = 16;
        }
    }
}