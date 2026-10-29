using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace MissionPlanner.GCSViews
{
    public partial class VideoHostPanel : Panel
    {
        private const int WM_PARENTNOTIFY = 0x0210;

        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_MBUTTONDOWN = 0x0207;

        private const double ASPECTRATIO = (double)16 / 9;

        private DateTime lastClick;
        private const int DoubleClickMs = 500;

        public event EventHandler<VideoClickEventArgs> VideoClick;

        public VideoHostPanel()
        {
            InitializeComponent();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_PARENTNOTIFY)
            {
                int eventId = m.WParam.ToInt32() & 0xFFFF;

                if (eventId == WM_LBUTTONDOWN ||
                    eventId == WM_RBUTTONDOWN ||
                    eventId == WM_MBUTTONDOWN)
                {

                    // Handle double click
                    bool doubleClick = (DateTime.Now - lastClick).TotalMilliseconds < DoubleClickMs;
                    lastClick = DateTime.Now;
                    if (!doubleClick) return;

                    int x = (short)(m.LParam.ToInt32() & 0xFFFF);
                    int y = (short)((m.LParam.ToInt32() >> 16) & 0xFFFF);

                    Rectangle videoRect = GetVideoRect(this.Width, this.Height, ASPECTRATIO);

                    Debug.WriteLine($"Rect: x: {videoRect.X}, y: {videoRect.Y}, w: {videoRect.Width}, h: {videoRect.Height}");

                    if (!videoRect.Contains(x, y))
                    {
                        Debug.WriteLine($"Click is out of bounds: x:{x} y:{y}");
                        return;
                    }

                    int localX = Math.Max(0, x - videoRect.X);
                    int localY = Math.Max(0, y - videoRect.Y);

                    Debug.WriteLine($"Output: x: {localX}, y: {localY}");

                    MouseButtons button = MouseButtons.Left;

                    if (eventId == WM_RBUTTONDOWN)
                        button = MouseButtons.Right;
                    else if (eventId == WM_MBUTTONDOWN)
                        button = MouseButtons.Middle;


                    VideoClick?.Invoke(this,
                        new VideoClickEventArgs(
                            button,
                            localX,
                            localY,
                            videoRect.Width,
                            videoRect.Height));
                }
            }

            base.WndProc(ref m);
        }

        private static Rectangle GetVideoRect(int panelWidth, int panelHeight, double videoAspectRatio)
        {
            double panelAspectRatio = (double)panelWidth / panelHeight;

            if (panelAspectRatio > videoAspectRatio)
            {
                int height = panelHeight;
                int width = (int)(height * videoAspectRatio);

                return new Rectangle(
                    (panelWidth - width) / 2,
                    0,
                    width,
                    height);
            }
            else
            {
                int width = panelWidth;
                int height = (int)(width / videoAspectRatio);

                return new Rectangle(
                    0,
                    (panelHeight - height) / 2,
                    width,
                    height);
            }
        }
    }

    public class VideoClickEventArgs : EventArgs
    {
        public int X { get; }
        public int Y { get; }

        public int Width { get; }
        public int Height { get; }

        public MouseButtons Button { get; }

        public VideoClickEventArgs(MouseButtons button, int x, int y, int width, int height)
        {
            Button = button;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
