﻿using CoordinateSharp;
using log4net;
using MissionPlanner.Utilities;
using MV04.Camera;
using MV04.Settings;
using NextVisionVideoControlLibrary;
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


        public CameraFullScreenForm()
        {
            InitializeComponent();

            InitCameraSettings();
            StartCameraVideoStream();
            this.FormClosing += CameraFullScreenForm_FormClosing;
            this.VisibleChanged += CameraFullScreenForm_VisibleChanged;

            SetStopButtonVisible();

#if DEBUG

            this.VideoControl.Paint += CameraFullScreenForm_Paint;


#endif

        }

        private void CameraFullScreenForm_Paint(object sender, PaintEventArgs e)
        {
#if DEBUG

            //var g = e.Graphics;
            //Pen myp = new Pen(System.Drawing.Color.Red, 4);
            //Font fy = new Font("Helvetica", 10, FontStyle.Bold);
            //Brush br = new SolidBrush(System.Drawing.Color.Red);
            ////g.DrawString(X.ToString() + ", " + Y.ToString(), fy, br, X, Y);
            //g.FillEllipse(br, X, Y, 10, 10);

            //Point res = CameraView.Translate(new Point(X, Y), this.pnl_CameraView.Size, new Size(1920, 1200));

            //Brush br2 = new SolidBrush(Color.Yellow);
            //g.FillEllipse(br2, res.X, res.Y, 10, 10);

#endif
        }

        private void CameraFullScreenForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                
                //StartCameraVideoStream();
                //this.pnl_CameraView.Controls.Add(VideoControl);
                //VideoControl.Dock = DockStyle.Fill;
                //CameraHandler.Instance.CameraControlConnect(
                //IPAddress.Parse(SettingManager.Get(Setting.CameraControlIP)),
                //int.Parse(SettingManager.Get(Setting.CameraControlPort)));
            }
            else
            {
                //
            }
        }

        private void CameraFullScreenForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void InitCameraSettings()
        {
            // Video control
            VideoControl = CameraHandler.Instance.CameraVideoControl;
            VideoControlDLLVersion = CameraHandler.Instance.StreamDLLVersion;
            CameraStreamIP = SettingManager.Get(Setting.CameraStreamIP);
            CameraStreamPort = int.Parse(SettingManager.Get(Setting.CameraStreamPort));
            FetchHudDataTimer.Interval = 100; // 10Hz
            FetchHudDataTimer.Tick += (sender, eventArgs) => FetchHudData();

            // Create default drawing objects
            DefaultFont = new Font(FontFamily.GenericMonospace, this.Font.SizeInPoints * 2f);
            DefaultBrush = new SolidBrush(Color.Red);

            // Camera control
            CameraControlDLLVersion = CameraHandler.Instance.CameraControlDLLVersion;

            // Snapshot & video save location
            CameraHandler.Instance.MediaSavePath = MissionPlanner.Utilities.Settings.GetUserDataDirectory() + "MV04_media" + Path.DirectorySeparatorChar;

            // SysID for camera functions
            CameraHandler.sysID = MainV2.comPort.sysidcurrent;
            MainV2.comPort.MavChanged += (sender, eventArgs) => CameraHandler.sysID = MainV2.comPort.sysidcurrent; // Update sysID on new connection

            this.pnl_CameraView.Controls.Add(VideoControl);
            VideoControl.Dock = DockStyle.Fill;
        }

        private void StartCameraVideoStream()
        {
            if (CameraHandler.Instance.StartStream(IPAddress.Parse(CameraStreamIP), CameraStreamPort, OnNewFrame, OnVideoClickFullScreen))
            {
                //AddToOSDDebug("Video stream started");

                FetchHudData();
                FetchHudDataTimer.Start();
            }
            CameraHandler.Instance.CameraControlConnect(
                IPAddress.Parse(SettingManager.Get(Setting.CameraControlIP)),
                int.Parse(SettingManager.Get(Setting.CameraControlPort)));
        }
        /// <summary>
        /// Handles a mouse click on the video area 
        /// the CameraView event will be triggered
        /// </summary>
        private void OnVideoClickFullScreen(int x, int y)
        {
            //X = MousePosition.X;
            //Y = MousePosition.Y;

            //var translatedPoint = CameraView.Translate(new Point(X, Y), this.pnl_CameraView.Size, CameraView.Trip5Size);

            //CameraHandler.Instance.StartTracking(translatedPoint);

            //CameraView.IsCameraTrackingModeActive = true;

            //SetStopButtonVisible();
        }

        /// <summary>
        /// Handles every new frame
        /// </summary>
        private void OnNewFrame(byte[] frame_buf, stream_status status, int width, int height)
        {
            // frame_buf is 1920 x 1080 x 3 long
            // real frame is width x height

            // Create drawing objects
            VideoGraphics = Graphics.FromImage(new Bitmap(width, height, 3 * width, System.Drawing.Imaging.PixelFormat.Format24bppRgb, Marshal.UnsafeAddrOfPinnedArrayElement(frame_buf, 0)));
            VideoGraphics.InterpolationMode = InterpolationMode.High;
            VideoGraphics.SmoothingMode = SmoothingMode.HighQuality;
            VideoGraphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            VideoGraphics.CompositingQuality = CompositingQuality.HighQuality;
            VideoRectangle = new Rectangle()
            {
                X = (int)Math.Round(VideoGraphics.VisibleClipBounds.X),
                Y = (int)Math.Round(VideoGraphics.VisibleClipBounds.Y),
                Width = (int)Math.Round(VideoGraphics.VisibleClipBounds.Width),
                Height = (int)Math.Round(VideoGraphics.VisibleClipBounds.Height)
            };

            if (status == stream_status.StreamDetectionOk)
            {
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
                    VideoGraphics.DrawLine(linePen,
                        VideoRectangle.Width / 2, VideoRectangle.Height / 2,
                        (VideoRectangle.Width / 2) + lineHeight, VideoRectangle.Height / 2);
                    VideoGraphics.DrawLine(linePen,
                        VideoRectangle.Width / 2, VideoRectangle.Height / 2,
                        VideoRectangle.Width / 2, (VideoRectangle.Height / 2) + lineHeight);
                    VideoGraphics.DrawLine(linePen,
                        VideoRectangle.Width / 2, VideoRectangle.Height / 2,
                        (VideoRectangle.Width / 2) - lineHeight, VideoRectangle.Height / 2);
                    VideoGraphics.DrawLine(linePen,
                        VideoRectangle.Width / 2, VideoRectangle.Height / 2,
                        VideoRectangle.Width / 2, (VideoRectangle.Height / 2) - lineHeight);
                }
                else // Horizontal
                {
                    // Draw center ^ character
                    VideoGraphics.DrawLine(new Pen(Color.Red, 3),
                        VideoRectangle.Width / 2, VideoRectangle.Height / 2,
                        (VideoRectangle.Width / 2) - Math.Min(lineHeight / 2, HudElements.LineSpacing), (VideoRectangle.Height / 2) + lineHeight);
                    VideoGraphics.DrawLine(new Pen(Color.Red, 3),
                        VideoRectangle.Width / 2, VideoRectangle.Height / 2,
                        (VideoRectangle.Width / 2) + Math.Min(lineHeight / 2, HudElements.LineSpacing), (VideoRectangle.Height / 2) + lineHeight);

                    for (int i = 1; i <= 3; i++)
                    {
                        // Draw lines to the right
                        VideoGraphics.DrawLine(linePen,
                            (VideoRectangle.Width / 2) + (i * HudElements.LineSpacing), VideoRectangle.Height / 2,
                            (VideoRectangle.Width / 2) + (i * HudElements.LineSpacing), (VideoRectangle.Height / 2) + lineHeight);

                        // Draw lines to the left
                        VideoGraphics.DrawLine(linePen,
                            (VideoRectangle.Width / 2) - (i * HudElements.LineSpacing), VideoRectangle.Height / 2,
                            (VideoRectangle.Width / 2) - (i * HudElements.LineSpacing), (VideoRectangle.Height / 2) + lineHeight);
                    }

                    // Draw number under first right line
                    DrawText(HudElements.LineDistance.ToString(), new Point((VideoRectangle.Width / 2) + HudElements.LineSpacing, (VideoRectangle.Height / 2) + lineHeight + 3), ContentAlignment.TopCenter, HorizontalAlignment.Center, new Font(DefaultFont.FontFamily, this.Font.SizeInPoints, FontStyle.Regular));
                }
                #endregion
            }
            else // Stream is not OK, Stream needs therapy
            {
                // Clean screen
                VideoGraphics.Clear(Color.FromArgb(0, 128, 0));

                // Print status message
                string message = "";
                switch (status)
                {
                    case stream_status.StreamIdle:
                        message = "STREAM IDLE";
                        break;
                    case stream_status.StreamAcquiring:
                        message = "AQUIRING STREAM";
                        break;
                    case stream_status.StreamLost:
                        message = "STREAM LOST";
                        break;
                    default: break;
                }
                DrawText(message, new Point((VideoRectangle.Width / 2) + rnd.Next(-20, 21), (VideoRectangle.Height / 2) + rnd.Next(-20, 21)), ContentAlignment.MiddleCenter, HorizontalAlignment.Center, new Font(DefaultFont.FontFamily, DefaultFont.Size * 2f, FontStyle.Bold));
            }
        }

        /// <summary>
        /// Draws a text on the control at a given location
        /// </summary>
        private Rectangle DrawText(string text, Point position, ContentAlignment rectangleAlignment = ContentAlignment.TopLeft, HorizontalAlignment textAlignment = HorizontalAlignment.Left, Font textFont = null, Brush textBrush = null, Rectangle? drawArea = null, Graphics drawGraphics = null)
        {
            // Null check text
            text = text ?? "";

            // Set nullables
            textFont = textFont ?? DefaultFont;
            textBrush = textBrush ?? DefaultBrush;
            drawArea = drawArea ?? VideoRectangle;
            drawGraphics = drawGraphics ?? VideoGraphics;

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

        /// <summary>
        /// Calculates the bounding rectangle size for a text
        /// </summary>
        private Size TextSize(string text, Font font, Graphics graphics)
        {
            SizeF size = graphics.MeasureString(text.Split('\n').OrderByDescending(s => s.Length).FirstOrDefault(), font);
            return new Size()
            {
                Width = (int)Math.Ceiling(size.Width) + 4,
                Height = (int)Math.Ceiling(size.Height * (text.Count(c => c == '\n') + 1))
            };
        }

        /// <summary>
        /// Fetches fresh data for the overlay elements
        /// </summary>
        private void FetchHudData()
        {
            CurrentState cs = MainV2.comPort.MAV.cs;

            // Date and time
            DateTime now = DateTime.Now;
            int utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(now).Hours;
            HudElements.Time = $"{now.Day.ToString().PadLeft(2, '0')}{now.ToString("MMM", new CultureInfo("en-US")).ToUpperInvariant()}{now.Year}\n{now.ToString("HH:mm:ss")}\nUTC{(utcOffset >= 0 ? "+" : "")}{utcOffset}";

            /// Above Ground Level
            HudElements.AGL = "AGL";
            switch (SettingManager.Get(Setting.AltFormat))
            {
                case "ft":
                    HudElements.AGL += ((int)Math.Round(cs.alt * Meter_to_Feet)).ToString().PadLeft(4);
                    break;
                default: // "m"
                    HudElements.AGL += ((int)Math.Round(cs.alt)).ToString().PadLeft(4); // cs.alt is in meters
                    break;
            }
            HudElements.AGL += SettingManager.Get(Setting.AltFormat).ToUpper();

            // Horizontal velocity (ground speed)
            HudElements.Velocity = "VEL";
            switch (SettingManager.Get(Setting.SpeedFormat))
            {
                case "kmph":
                    HudElements.Velocity += ((int)Math.Round(cs.groundspeed * Mps_to_Kmph)).ToString().PadLeft(4) + "KM/H";
                    break;
                case "knots":
                    HudElements.Velocity += ((int)Math.Round(cs.groundspeed * Mps_to_Knots)).ToString().PadLeft(4) + "KTS";
                    break;
                default: // mps
                    HudElements.Velocity += ((int)Math.Round(cs.groundspeed)).ToString().PadLeft(4) + "M/S"; // cs.groundspeed is in m/s
                    break;
            }

            // Target distance (slant range)
            HudElements.TGD = "TGD";
            bool hasGndCrsRep = CameraHandler.Instance.HasCameraReport(MavProto.MavReportType.GndCrsReport);
            double slantRange = hasGndCrsRep ? ((MavProto.GndCrsReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.GndCrsReport]).gndCrsSlantRange : 0;
            switch (SettingManager.Get(Setting.DistFormat))
            {
                case "km":
                    HudElements.TGD += ((int)Math.Round(slantRange * 1000)).ToString().PadLeft(5);
                    break;
                case "ft":
                    HudElements.TGD += ((int)Math.Round(slantRange * Meter_to_Feet)).ToString().PadLeft(5);
                    break;
                default: // m
                    HudElements.TGD += ((int)Math.Round(slantRange)).ToString().PadLeft(5);
                    break;
            }
            HudElements.TGD += SettingManager.Get(Setting.DistFormat).ToUpper();

            // Battery percentage
            HudElements.Battery = "BAT"
                + cs.battery_remaining.ToString().PadLeft(4) + "%"; // Percentage
            //HudElements.Battery += $"\n00:{rnd.Next(0, 60).ToString().PadLeft(2, '0')}:{rnd.Next(0, 60).ToString().PadLeft(2, '0')}"; // Remaining time

            // Radio & GPS signal strength
            HudElements.SignalStrengths = "RADIO" + cs.linkqualitygcs.ToString().PadLeft(8) + "%";  // Radio signal percentage
            string gpsStr;
            switch (cs.gpsstatus)
            {
                case 0: gpsStr = "NO GPS"; break;
                case 1: gpsStr = "NO FIX"; break;
                case 2: gpsStr = "2D FIX"; break;
                case 3: gpsStr = "3D FIX"; break;
                case 4: gpsStr = "DGPS FIX"; break;
                case 5: gpsStr = "RTK LOW"; break;
                case 6: gpsStr = "RTK FIX"; break;
                default: gpsStr = cs.gpsstatus.ToString(); break;
            }
            HudElements.SignalStrengths += "\nGPS" + gpsStr.PadLeft(11);                            // GPS signal percentage

            // Operator (home) distance
            HudElements.FromOperator = "OPERATOR";
            switch (SettingManager.Get(Setting.DistFormat))
            {
                case "km":
                    HudElements.FromOperator += ((int)Math.Round(cs.DistToHome * 1000)).ToString().PadLeft(5);
                    break;
                case "ft":
                    HudElements.FromOperator += ((int)Math.Round(cs.DistToHome * Meter_to_Feet)).ToString().PadLeft(5);
                    break;
                default: // m
                    HudElements.FromOperator += ((int)Math.Round(cs.DistToHome)).ToString().PadLeft(5); // cs.DistToHome is in meters
                    break;
            }
            HudElements.FromOperator += SettingManager.Get(Setting.DistFormat).ToUpper();

            // Next waypoint distance
            HudElements.ToWaypoint = "WAYPOINT";
            switch (SettingManager.Get(Setting.DistFormat))
            {
                case "km":
                    HudElements.ToWaypoint += (cs.wp_dist * 1000).ToString().PadLeft(5);
                    break;
                case "ft":
                    HudElements.ToWaypoint += (cs.wp_dist * Meter_to_Feet).ToString().PadLeft(5);
                    break;
                default: // m
                    HudElements.ToWaypoint += cs.wp_dist.ToString().PadLeft(5); // cs.wp_dist is in meters
                    break;
            }
            HudElements.ToWaypoint += SettingManager.Get(Setting.DistFormat).ToUpper();
            TimeSpan to_wp = TimeSpan.FromSeconds(cs.tot);
            HudElements.ToWaypoint += $"\n{to_wp.Hours.ToString().PadLeft(2, '0')}:{to_wp.Minutes.ToString().PadLeft(2, '0')}:{to_wp.Seconds.ToString().PadLeft(2, '0')}";

            // Camera angles
            bool hasSysRep = CameraHandler.Instance.HasCameraReport(MavProto.MavReportType.SystemReport);
            HudElements.Camera = "CAM "
                + "PITCH"
                + (hasSysRep ? (int)Math.Round(((MavProto.SysReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.SystemReport]).pitch) : 0).ToString().PadLeft(5) + "°"
                + "\nYAW"
                + (hasSysRep ? (int)Math.Round(((MavProto.SysReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.SystemReport]).roll) : 0).ToString().PadLeft(7) + "°";

            // UAV position
            Coordinate droneCoord = new Coordinate(cs.lat, cs.lng, DateTime.Now);
            string dronePos;
            switch (SettingManager.Get(Setting.GPSType).ToUpper())
            {
                case "MGRS":
                    dronePos = droneCoord.MGRS.ToString();
                    break;
                default: // WGS84
                    dronePos = droneCoord.UTM.ToString();
                    break;
            }
            HudElements.DroneGps = "UAV"
                + SettingManager.Get(Setting.GPSType).ToUpper().PadLeft(dronePos.Length - 3)
                + $"\n" + dronePos;

            CameraHandler.Instance.DronePos = droneCoord; // Update CameraHandler

            // Camera target position
            Coordinate targetCoord = new Coordinate(
                hasGndCrsRep ? ((MavProto.GndCrsReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.GndCrsReport]).gndCrsLat : 0,
                hasGndCrsRep ? ((MavProto.GndCrsReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.GndCrsReport]).gndCrsLon : 0,
                DateTime.Now);
            string targetPos;
            switch (SettingManager.Get(Setting.GPSType).ToUpper())
            {
                case "MGRS":
                    targetPos = targetCoord.MGRS.ToString();
                    break;
                default: // WGS84
                    targetPos = targetCoord.UTM.ToString();
                    break;
            }
            HudElements.TargetGps = "TRG"
                + SettingManager.Get(Setting.GPSType).ToUpper().PadLeft(targetPos.Length - 3)
                + $"\n" + targetPos;

            CameraHandler.Instance.TargPos = targetCoord; // Update CameraHandler

            HudElements.LineDistance = 10;
            // TODO: Optimize HudElements.LineDistance on the fly to make it easy to read on the screen

            HudElements.LineSpacing = PixelsForMeters(
                hasGndCrsRep ? ((MavProto.GndCrsReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.GndCrsReport]).gndCrsSlantRange : 100.0,
                hasSysRep ? ((MavProto.SysReport)CameraHandler.Instance.CameraReports[MavProto.MavReportType.SystemReport]).fov : 60.0,
                VideoRectangle.Width, HudElements.LineDistance);

            SetStopButtonVisible();
        }

        /// <summary>
        /// Pixel count for a given horizontal distance (in meters) at the camera target
        /// </summary>
        /// <param name="slantRange">Camera target distance in meters</param>
        /// <param name="fovDegrees">Camera field of view in degrees</param>
        /// <param name="fovPixels">Camera field of view in pixels (video horizontal resolution)</param>
        /// <param name="hMeters">Desired horizontal distance for the return value</param>
        /// <returns>Pixel count for a given horizontal distance (in meters) at the camera target</returns>
        private int PixelsForMeters(double slantRange, double fovDegrees, int fovPixels, int hMeters = 10)
        {
            double fovMeters = 2.0 * slantRange * Math.Tan(MathHelper.Radians(fovDegrees / 2.0));
            int pixelPerMeter = (int)Math.Round((double)fovPixels / fovMeters); // Use Math.Ceiling() instead?
            return pixelPerMeter * hMeters;
        }

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