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
        List<string> _bitmapsForVideo = new List<string>();
        System.Timers.Timer _videoRecorderTimer;
        int _frameRate = 10;

        bool _recordingInProgress;
        

        public VideoRecorder()
        {
            _videoRecorderTimer = new System.Timers.Timer();
            _videoRecorderTimer.Elapsed += _videoRecorderTimer_Elapsed;
            _videoRecorderTimer.Interval = 10*1000;

        }

        private void _videoRecorderTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                List<string> actList;

                lock (_bitmapsForVideo)
                {
                    actList = new List<string>(_bitmapsForVideo);
                    _bitmapsForVideo.Clear();
                }
                    
                Task.Factory.StartNew(() => { WriteVideoToFile(actList); });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Recorder - _videoSaveSegmentTimer_Tick " + ex.Message);
            }
        }

        private void WriteVideoToFile(List<string> actualImageList)
        {
            if (_recordingInProgress)
            {
                using (VideoFileWriter writer = new VideoFileWriter())
                {
                    writer.Open(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//testrecord" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".mp4", 1920, 1080, _frameRate, VideoCodec.MPEG4);

                    foreach (string location in actualImageList)
                    {
                        try
                        {
                            Bitmap bm = System.Drawing.Image.FromFile(location) as Bitmap;
                            Bitmap bm_formatted = new Bitmap(bm, 1920, 1080);
                            writer.WriteVideoFrame(bm_formatted);
                        }
                        catch { }
                        
                    }
                    writer.Close();
                }
            }

            //#region delete stored images
            //foreach (string location in actualImageList)
            //{
            //    try
            //    {
            //        File.Delete(location);
            //    }
            //    catch { }
            //}
            //#endregion

        }



        //----------

        public void AddNewImage(Bitmap bm)
        {
            Task.Factory.StartNew(() => { WriteImageToFile(bm); });

        }

        private void WriteImageToFile(Bitmap bm)
        {
            try
            {
                string name = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//screenshot-" + System.DateTime.Now.ToString("yyyymmddhhmmss") + ".png";

                _bitmapsForVideo.Add(name);

                bm.Save(name, ImageFormat.Png); //Task.Factory.StartNew(() => { bm.Save(name, ImageFormat.Png); });
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
            _recordingInProgress = false;
            _videoRecorderTimer.Stop();
            _videoRecorderTimer.Close();
        }


        
    }
}
