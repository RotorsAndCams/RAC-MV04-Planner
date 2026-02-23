using Accord.Video.FFMPEG;
using MV04.Camera;
using MV04.Settings;
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
        #region Fields

        VideoFileWriter _writer = new VideoFileWriter();
        System.Timers.Timer _videoRecorderTimer;
        int _segmentLength = 30; // seconds
        int _frameRate = 25/1;
        bool _recordingInProgress;

        object _recordingLock = new object();

        #endregion

        #region Methods


        #endregion
    }
}
