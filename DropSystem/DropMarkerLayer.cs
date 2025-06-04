using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using MissionPlanner.Controls;

namespace MissionPlanner.DropSystem
{
    public class DropMarkerLayer
    {
        private readonly myGMAP _map;

        // Three overlays, one per marker type
        private readonly GMapOverlay _targetOverlay;
        private readonly GMapOverlay _impactOverlay;
        private readonly GMapOverlay _dropOverlay;
    
        public DropMarkerLayer(myGMAP mapControl)
            {
                _map = mapControl;
                _targetOverlay = new GMapOverlay("targetOverlay");
                _impactOverlay = new GMapOverlay("impactOverlay");
                _dropOverlay = new GMapOverlay("dropOverlay");

                _map.Overlays.Add( _targetOverlay );
                _map.Overlays.Add( _impactOverlay );
                _map.Overlays.Add( _dropOverlay );
            }

        // Clear and redraw the Target marker in red
        public void ShowTarget(PointLatLng targetLoc)
            {
                _targetOverlay.Markers.Clear();
                var tgtMarker = new GMarkerGoogle(targetLoc, GMarkerGoogleType.red_dot)
                    {
                        ToolTipText = "Target",
                        ToolTipMode = MarkerTooltipMode.OnMouseOver
                    };
                _targetOverlay.Markers.Add( tgtMarker );
                _map.Refresh();
            }

        // Clear and redo the impact marker in blue
        public void ShowImpact(PointLatLng impactLoc)
            {
                _impactOverlay.Markers.Clear();
                var impMarker = new GMarkerGoogle(impactLoc, GMarkerGoogleType.red_dot)
                    {
                        ToolTipText = "Predicted Impact",
                        ToolTipMode = MarkerTooltipMode.OnMouseOver
                    };
                _impactOverlay.Markers.Add(impMarker);
                _map.Refresh();
            }

        // Clear and redo the actual drop marker in green
        public void ShowActualDrop(PointLatLng dropLoc)
            {
                _dropOverlay.Markers.Clear();
                var dropMarker = new GMarkerGoogle(dropLoc, GMarkerGoogleType.green_dot)//green_small
                {
                    ToolTipText = "Actual drop",
                    ToolTipMode = MarkerTooltipMode.OnMouseOver
                };
                _dropOverlay.Markers.Add(dropMarker);
                _map.Refresh();
            }

        // Remove all three overlays
        public void ClearAll()
        {
            _targetOverlay.Markers.Clear();
            _impactOverlay.Markers.Clear();
            _dropOverlay.Markers.Clear();
            _map.Refresh();
        }
    }
}
