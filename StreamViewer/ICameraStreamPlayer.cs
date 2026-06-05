using System;
using System.Threading.Tasks;

namespace MissionPlanner.StreamViewer
{
    public interface ICameraStreamPlayer : IDisposable
    {
        bool IsConnected { get; }
        bool IsDisplayRunning { get; }
        bool IsSegmentRecording { get; }

        event EventHandler<string> StatusChanged;
        event EventHandler<Exception> ErrorOccurred;

        Task ConnectAsync();

        Task StartAsync(IntPtr hostWindowHandle);
        Task RestartAsync(IntPtr hostWindowHandle);

        void StopDisplay();
        void Stop();

        void Resize(int hostWidth, int hostHeight);

        Task CaptureCurrentFrameAsync(string outputFilePath);

        Task StartSegmentRecordingAsync(
            string outputDirectory,
            int segmentLengthSeconds,
            string filePrefix
        );

        void StopSegmentRecording();
    }
}