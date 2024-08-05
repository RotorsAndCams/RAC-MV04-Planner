using CoordinateSharp;
using log4net;
using MissionPlanner.Utilities;
using MV04.Camera;
using MV04.Settings;
using NextVisionVideoControlLibrary;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionPlanner.GCSViews
{
    public partial class CameraFullScreenForm : Form
    {
        #region Fields

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        VideoControl VideoControl;
        (int major, int minor, int build) VideoControlDLLVersion;
        string CameraStreamIP;
        int CameraStreamPort;
        bool CameraStreamAJC = false;
        new Font DefaultFont;
        Brush DefaultBrush;
        Rectangle VideoRectangle;
        Graphics VideoGraphics;
        HudElements HudElements = new HudElements();

        Timer FetchHudDataTimer = new Timer();

        (int major, int minor, int build) CameraControlDLLVersion;

        Random rnd = new Random();

        private int X;
        private int Y;

        #region Conversion multipliers
        const double Meter_to_Feet = 3.2808399;
        const double Mps_to_Kmph = 3.6;
        const double Mps_to_Knots = 1.94384449;
        #endregion

        #endregion


        object _bgimagelock = new object();
        Graphics _gr;
        Image img;
        public CameraFullScreenForm()
        {
            InitializeComponent();

            //this.FormClosing += CameraFullScreenForm_FormClosing;

            SetStopButtonVisible();

            btn_StopTracking.BringToFront();
            btn_Close.BringToFront();


            pb_Gstreamer.Paint += Pb_Gstreamer_Paint;

            CameraView.instance.event_BroadCastImage += Instance_event_BroadCastImage;


            //GStreamer.onNewImage += (sender, image) =>
            //{
            //    try
            //    {
            //        //if (image == null) return;

            //        //img = new Bitmap(image.Width, image.Height, 4 * image.Width,
            //        //            System.Drawing.Imaging.PixelFormat.Format32bppPArgb,
            //        //            image.LockBits(Rectangle.Empty, null, SKColorType.Bgra8888)
            //        //                .Scan0);

            //        //if (img == null) return;

            //        if (InvokeRequired)
            //            Invoke(new Action(() => pb_Gstreamer.Invalidate()));
            //        else
            //            pb_Gstreamer.Invalidate();

            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Gst error" + ex.Message);
            //    }
            //};


        }

        private void Instance_event_BroadCastImage(object sender, BitMapEventArgs e)
        {
            img = e.img;
            if (InvokeRequired)
                Invoke(new Action(() => pb_Gstreamer.Invalidate()));
            else
                pb_Gstreamer.Invalidate();
        }

        private void Pb_Gstreamer_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (img != null)
                {
                    lock (_bgimagelock)
                    {
                        e.Graphics.DrawImage(img, 0, 0, pb_Gstreamer.Width, pb_Gstreamer.Height);
                    }

                    if (_gr != e.Graphics)
                        _gr = e.Graphics;

                    //OnNewFrame(img.Width, img.Height);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
                //img.Dispose();
                //img = null;
            }
        }

        private void OnNewFrame(int width, int height)
        {
            // frame_buf is 1920 x 1080 x 3 long
            // real frame is width x height

            // Create drawing objects

            _gr.InterpolationMode = InterpolationMode.High;
            _gr.SmoothingMode = SmoothingMode.HighQuality;
            _gr.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            _gr.CompositingQuality = CompositingQuality.HighQuality;
            VideoRectangle = new Rectangle()
            {
                X = (int)Math.Round(_gr.VisibleClipBounds.X),
                Y = (int)Math.Round(_gr.VisibleClipBounds.Y),
                Width = (int)Math.Round(_gr.VisibleClipBounds.Width),
                Height = (int)Math.Round(_gr.VisibleClipBounds.Height)
            };

            // Datetime
            Rectangle Datetime = DrawText(HudElements.Time, new Point(3, 3), ContentAlignment.TopLeft, HorizontalAlignment.Left);

            // Battery
            Rectangle Battery = DrawText(HudElements.Battery, new Point(VideoRectangle.Width - 3, 3), ContentAlignment.TopRight, HorizontalAlignment.Right);

            int topLeft = Datetime.Right;
            int topStep = ((Battery.Left - topLeft) / 4) / 2;

            // AGL
            DrawText(HudElements.AGL, new Point(topLeft + topStep, 3), ContentAlignment.TopCenter, HorizontalAlignment.Left);

            // Velocity
            DrawText(HudElements.Velocity, new Point(topLeft + (3 * topStep), 3), ContentAlignment.TopCenter, HorizontalAlignment.Left);

            // TGD
            DrawText(HudElements.TGD, new Point(topLeft + (5 * topStep), 3), ContentAlignment.TopCenter, HorizontalAlignment.Left);

            // Signal strengths
            DrawText(HudElements.SignalStrengths, new Point(topLeft + (7 * topStep), 3), ContentAlignment.TopCenter, HorizontalAlignment.Right);

            // Camera info
            DrawText(HudElements.Camera, new Point(3, Datetime.Bottom + 20), ContentAlignment.TopLeft, HorizontalAlignment.Right);

            // Next waypoint
            Rectangle nextWP = DrawText(HudElements.ToWaypoint, new Point(VideoRectangle.Width - 3, Battery.Bottom + 20), ContentAlignment.TopRight, HorizontalAlignment.Right);

            // Operator distance
            DrawText(HudElements.FromOperator, new Point(VideoRectangle.Width - 3, nextWP.Bottom + 20), ContentAlignment.TopRight, HorizontalAlignment.Right);

            // Coords
            DrawText(HudElements.DroneGps, new Point(0, VideoRectangle.Height - 3), ContentAlignment.BottomLeft, HorizontalAlignment.Left);
            DrawText(HudElements.TargetGps, new Point(VideoRectangle.Width - 3, VideoRectangle.Height - 3), ContentAlignment.BottomRight, HorizontalAlignment.Right);

            #region Crosshairs
            int lineHeight = (int)Math.Round(VideoRectangle.Height * 0.1);
            Pen linePen = new Pen(Color.Red, 1);

            if (HudElements.Crosshairs == CrosshairsType.Plus) // Plus
            {
                _gr.DrawLine(linePen,
                    VideoRectangle.Width / 2, VideoRectangle.Height / 2,
                    (VideoRectangle.Width / 2) + lineHeight, VideoRectangle.Height / 2);
                _gr.DrawLine(linePen,
                    VideoRectangle.Width / 2, VideoRectangle.Height / 2,
                    VideoRectangle.Width / 2, (VideoRectangle.Height / 2) + lineHeight);
                _gr.DrawLine(linePen,
                    VideoRectangle.Width / 2, VideoRectangle.Height / 2,
                    (VideoRectangle.Width / 2) - lineHeight, VideoRectangle.Height / 2);
                _gr.DrawLine(linePen,
                    VideoRectangle.Width / 2, VideoRectangle.Height / 2,
                    VideoRectangle.Width / 2, (VideoRectangle.Height / 2) - lineHeight);
            }
            else // Horizontal
            {
                // Draw center ^ character
                _gr.DrawLine(new Pen(Color.Red, 3),
                    VideoRectangle.Width / 2, VideoRectangle.Height / 2,
                    (VideoRectangle.Width / 2) - Math.Min(lineHeight / 2, HudElements.LineSpacing), (VideoRectangle.Height / 2) + lineHeight);
                _gr.DrawLine(new Pen(Color.Red, 3),
                    VideoRectangle.Width / 2, VideoRectangle.Height / 2,
                    (VideoRectangle.Width / 2) + Math.Min(lineHeight / 2, HudElements.LineSpacing), (VideoRectangle.Height / 2) + lineHeight);

                for (int i = 1; i <= 3; i++)
                {
                    // Draw lines to the right
                    _gr.DrawLine(linePen,
                        (VideoRectangle.Width / 2) + (i * HudElements.LineSpacing), VideoRectangle.Height / 2,
                        (VideoRectangle.Width / 2) + (i * HudElements.LineSpacing), (VideoRectangle.Height / 2) + lineHeight);

                    // Draw lines to the left
                    _gr.DrawLine(linePen,
                        (VideoRectangle.Width / 2) - (i * HudElements.LineSpacing), VideoRectangle.Height / 2,
                        (VideoRectangle.Width / 2) - (i * HudElements.LineSpacing), (VideoRectangle.Height / 2) + lineHeight);
                }

                // Draw number under first right line
                DrawText(HudElements.LineDistance.ToString(), new Point((VideoRectangle.Width / 2) + HudElements.LineSpacing, (VideoRectangle.Height / 2) + lineHeight + 3), ContentAlignment.TopCenter, HorizontalAlignment.Center, new Font(DefaultFont.FontFamily, this.Font.SizeInPoints, FontStyle.Regular));
            }
            #endregion

        }

        private Rectangle DrawText(string text, Point position, ContentAlignment rectangleAlignment = ContentAlignment.TopLeft, HorizontalAlignment textAlignment = HorizontalAlignment.Left, Font textFont = null, Brush textBrush = null, Rectangle? drawArea = null, Graphics drawGraphics = null)
        {
            // Null check text
            text = text ?? "";

            // Set nullables
            textFont = textFont ?? DefaultFont;
            textBrush = textBrush ?? DefaultBrush;
            drawArea = drawArea ?? VideoRectangle;
            drawGraphics = _gr;

            // Check position
            if (position.X >= 0
                && position.X <= drawArea.Value.Width
                && position.Y >= 0
                && position.Y <= drawArea.Value.Height)
            {
                // Draw text
                StringAlignment textHorizontalAlignment = StringAlignment.Near; // Relative to top left corner
                switch (textAlignment)
                {
                    case HorizontalAlignment.Center:
                        textHorizontalAlignment = StringAlignment.Center;
                        break;
                    case HorizontalAlignment.Right:
                        textHorizontalAlignment = StringAlignment.Far;
                        break;
                    default: // HorizontalAlignment.Left
                        break;
                }
                StringFormat textFormat = new StringFormat()
                {
                    Alignment = textHorizontalAlignment,
                    LineAlignment = StringAlignment.Center, // Relative to top left corner
                    FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.NoClip
                };

                Size textSize = TextSize(text, textFont, drawGraphics);
                switch (rectangleAlignment)
                {
                    case ContentAlignment.TopCenter:
                        position.X -= textSize.Width / 2;
                        break;
                    case ContentAlignment.TopRight:
                        position.X -= textSize.Width + 1;
                        break;
                    case ContentAlignment.MiddleLeft:
                        position.Y -= textSize.Height / 2;
                        break;
                    case ContentAlignment.MiddleCenter:
                        position.X -= textSize.Width / 2;
                        position.Y -= textSize.Height / 2;
                        break;
                    case ContentAlignment.MiddleRight:
                        position.X -= textSize.Width + 1;
                        position.Y -= textSize.Height / 2;
                        break;
                    case ContentAlignment.BottomLeft:
                        position.Y -= textSize.Height + 1;
                        break;
                    case ContentAlignment.BottomCenter:
                        position.X -= textSize.Width / 2;
                        position.Y -= textSize.Height + 1;
                        break;
                    case ContentAlignment.BottomRight:
                        position.X -= textSize.Width + 1;
                        position.Y -= textSize.Height + 1;
                        break;
                    default: // ContentAlignment.TopLeft
                        break;
                }
                Rectangle textRectangle = new Rectangle()
                {
                    Size = textSize,
                    Location = position
                };

                // Draw text on control
                drawGraphics.DrawString(text, textFont, textBrush, textRectangle, textFormat);

                // Return rectangle
                return textRectangle;
            }
            else
            {
                return new Rectangle();
            }
        }

        private Size TextSize(string text, Font font, Graphics graphics)
        {
            SizeF size = graphics.MeasureString(text.Split('\n').OrderByDescending(s => s.Length).FirstOrDefault(), font);
            return new Size()
            {
                Width = (int)Math.Ceiling(size.Width) + 4,
                Height = (int)Math.Ceiling(size.Height * (text.Count(c => c == '\n') + 1))
            };
        }

        //private void CameraFullScreenForm_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    e.Cancel = true;
        //    this.Hide();
        //}



        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btn_StopTracking_Click(object sender, EventArgs e)
        {
            CameraHandler.Instance.StopTracking(true);
            CameraView.IsCameraTrackingModeActive = false;

            SetStopButtonVisible();
        }

        private void SetStopButtonVisible()
        {
            if (CameraView.IsCameraTrackingModeActive)
                this.btn_StopTracking.Visible = true;
            else
                this.btn_StopTracking.Visible = false;
        }
    }
}
