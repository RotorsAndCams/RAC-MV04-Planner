using Accord.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MissionPlanner.Utilities.adsb;

namespace MissionPlanner.GCSViews
{
    public class VideoRecorder
    {
        VideoFileWriter _writer = new VideoFileWriter();
        System.Timers.Timer _videoRecorderTimer;
        int _frameRate = 10;
        bool _recordingInProgress;

        object _recordingLock = new object();
        

        public VideoRecorder()
        {
            _videoRecorderTimer = new System.Timers.Timer();
            _videoRecorderTimer.Elapsed += _videoRecorderTimer_Elapsed;
            _videoRecorderTimer.Interval = 30*1000;

        }

        private void _videoRecorderTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {

                if (!_recordingInProgress)
                {
                    Stop();

                    return;
                }
                    

                _writer.Flush();
                _writer.Close();
                _videoRecorderTimer.Stop();
                
            }
            catch{}
        }

        public void AddNewImage(Bitmap bm)
        {
            try
            {
                if (_writer.IsOpen)
                {
                    _writer.WriteVideoFrame(bm);
                }
                else
                {

                    _writer.Open(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//testrecord" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp4", 1920, 1080, _frameRate, VideoCodec.MPEG4, 100000);
                    _writer.WriteVideoFrame(bm);
                    _videoRecorderTimer.Start();
                }
            }
            catch { }


        }

        public void Start()
        {
            _recordingInProgress = true;
            _videoRecorderTimer.Start();
        }

        public void Stop()
        {
            lock (_videoRecorderTimer)
            {
                try
                {
                    _recordingInProgress = false;
                    _videoRecorderTimer.Stop();
                    _videoRecorderTimer.Close();
                    _writer.Flush();

                    _writer.Dispose();
                }
                catch { }
            }
            

            

            

            

        }

    }
}
