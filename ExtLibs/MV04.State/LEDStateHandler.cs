using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MV04.State
{
    public enum enum_LandingLEDState
    {
        Off,
        On
    }

    public enum enum_PositionLEDState
    {
        Off,
        IR,
        RedLight
    }

    public class LEDStateChangedEventArgs: EventArgs
    {
        public enum_LandingLEDState LandingLEDState { get; set; }
        public enum_PositionLEDState PositionLEDState { get; set; }

        public LEDStateChangedEventArgs(enum_LandingLEDState landing, enum_PositionLEDState position)
        {
            LandingLEDState = landing;
            PositionLEDState = position;
        }
    }

    public static class LEDStateHandler
    {
        public static event EventHandler<LEDStateChangedEventArgs> LedStateChanged;

        private static enum_LandingLEDState _landingState;
        public static enum_LandingLEDState LandingLEDState
        {
            get { return _landingState; }
            set
            {
                _landingState = value;
                OnValueChanged();
            }
        }

        private static enum_PositionLEDState _positionState;
        public static enum_PositionLEDState PositionLEDState
        {
            get { return _positionState; }
            set
            {
                _positionState = value;
                OnValueChanged();
            }
        }

        private static void OnValueChanged()
        {
            if (LedStateChanged != null)
                LedStateChanged(null, new LEDStateChangedEventArgs(_landingState, _positionState));
        }
        
    }
}
