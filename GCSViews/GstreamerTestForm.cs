using MissionPlanner.Controls;
using MissionPlanner.Utilities;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionPlanner.GCSViews
{
    public partial class GstreamerTestForm : Form
    {
        //public HUD testHud = new MissionPlanner.Controls.HUD();

        public GstreamerTestForm()
        {


            InitializeComponent();


            //this.Controls.Add(testHud);
            //testHud.Dock = DockStyle.Fill;

            TestGstreamer();


        }

        private readonly object _bgimagelock = new object();
        private void TestGstreamer()
        {

            //rtspsrc location=rtsp://192.168.0.203:554/live0 latency=0 ! decodebin ! autovideosink sync=false queue max-size-buffers=1 max-size-bytes=0 max-size-time=0
            string url = @"videotestsrc pattern=pinwheel ! video/x-raw, width=1280, height=720, framerate=30/1 ! videoconvert ! video/x-raw,format=BGRA ! appsink name=outsink";

            try
            {
                GStreamer.StartA(url);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(ex.ToString(), Strings.ERROR);
            }

            GStreamer.onNewImage += (sender, image) =>
            {
                //lock (this._bgimagelock)
                //{
                //    try
                //    {
                //        if (image == null)
                //        {
                //            testHud.bgimage = null;
                //            return;
                //        }


                //        var old = testHud.bgimage;
                //        testHud.bgimage = (Image)new Bitmap(image.Width, image.Height, 4 * image.Width,
                //                System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                //                image.LockBits(Rectangle.Empty, null, SKColorType.Bgra8888)
                //                    .Scan0);

                //        if (old != null)
                //            old.Dispose();
                //    }
                //    catch
                //    {
                //    }

                //}

                var img = (Image)new Bitmap(image.Width, image.Height, 4 * image.Width,
                                System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
                                image.LockBits(Rectangle.Empty, null, SKColorType.Bgra8888)
                                    .Scan0);

                var gr = pictureBox1.CreateGraphics();
                gr.DrawImage(img, 0,0, pictureBox1.Width, pictureBox1.Height);

                
            };
        }

        private void pb_Gstreamer_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("X: " + e.X + ", Y: " + e.Y);
        }
    }
}
