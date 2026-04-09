using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using MissionPlanner.Controls;
using MissionPlanner.GCSViews;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MissionPlanner.Utilities.LTM;

namespace MissionPlanner.DropSystem
{
    public class DropTarget
    {
        public string Name { get; set; }
        public PointLatLng DropPosition {  get; set; }
        public int DropAltitude { get; set; }
        public int ServoChannel { get; set; }
        GMapOverlay MapOverlay { get; set; }

        public DropTarget(string name, GMapOverlay overlay)
        {
            Name = name;
            MapOverlay = overlay;
        }
        public DropTarget(string name, PointLatLng pos, int alt, int servo, GMapOverlay overlay)
        {
            Name = name;
            DropPosition = pos;
            DropAltitude = alt;
            ServoChannel = servo;
            MapOverlay = overlay;
        }

        #region Data Input

        public void GetName(string p_Name)
        {
            Name = p_Name;
        }

        public void GetPositionFromInputMessageBox(PointLatLng p_MousePosition)
        {
            string position = p_MousePosition.Lat + ";" + p_MousePosition.Lng;
            if (DialogResult.Cancel == InputBox.Show("Enter the drop coords", "Enter in lat;lon format - leave it if click coordinate is ok", ref position))
                return;

            if (!position.Contains(";"))
            {
                CustomMessageBox.Show("Bad format");
                return;
            }

            string[] parts = position.Split(';');

            if (parts.Length == 2 &&
                float.TryParse(parts[0], NumberStyles.Number, new CultureInfo("hu-HU"), out float a) &&
                float.TryParse(parts[1], NumberStyles.Number, new CultureInfo("hu-HU"), out float b))
            {
                DropPosition = new PointLatLng(a, b);
            }
            else
                CustomMessageBox.Show("Invalid parameter");
            
        }

        public void GetPositionFromMousePosition(PointLatLng p_MousePosition)
        {
            DropPosition = p_MousePosition;
        }

        public void GetAltitude()
        {
            string alt = "40";
            if (DialogResult.Cancel == InputBox.Show("Enter Alt", "Enter drop position alt", ref alt))
                return;

            int intalt = (int)(100 * CurrentState.multiplieralt);
            if (!int.TryParse(alt, out intalt))
            {
                CustomMessageBox.Show("Bad Alt");
                return;
            }
            DropAltitude = intalt;

        }

        public void GetServoData(string servo = "6")
        {
            if (DialogResult.Cancel == InputBox.Show("Enter servo", "Enter servo channel", ref servo))
                return;

            int intservo = (int)(100 * CurrentState.multiplieralt);
            if (!int.TryParse(servo, out intservo))
            {
                CustomMessageBox.Show("Bad servo");
                return;
            }
            ServoChannel = intservo;

        }

        #endregion


        #region Display on map

        public void DrawOnMapDropTarget()
        {
            if (!isAllItemInitialized())
                return;

            var marker_old = MapOverlay.Markers.FirstOrDefault(m => m.ToolTipText.Contains(Name));

            MapOverlay.Markers.Remove(marker_old);

            //create new marker
            var marker = new GMarkerGoogle(DropPosition, GMarkerGoogleType.orange_small)
            {
                ToolTipText = Name + " drop \nLat:" + DropPosition.Lat + "; Lng:" + DropPosition.Lng + "\n Alt: " + DropAltitude + "\n Servo channel: " + ServoChannel,
                ToolTipMode = MarkerTooltipMode.Always
            };

            MapOverlay.Markers.Add(marker);

            if (!MainV2.instance.FlightData.gMapControl1.Overlays.Contains(MapOverlay))
                MainV2.instance.FlightData.gMapControl1.Overlays.Add(MapOverlay);

            MainV2.instance.FlightData.gMapControl1.UpdateMarkerLocalPosition(marker);
        }

        #endregion


        #region Helpers

        public bool isAllItemInitialized()
        {
            if (string.IsNullOrWhiteSpace(Name)) return false;
            if (DropPosition == null) return false;
            if (DropAltitude == default(int)) return false;
            if (ServoChannel == default(int)) return false;
            if (MapOverlay == null) return false;

            return true;
        }

        #endregion
    }
}
