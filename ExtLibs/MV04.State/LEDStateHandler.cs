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

    public enum enum_PositionLEDState_IR
    {
        Off,
        On
    }

    public enum enum_PositionLEDState_RedLight 
    {
        Off,
        On
    }


    public class LEDStateChangedEventArgs: EventArgs
    {
        public enum_LandingLEDState LandingLEDState { get; set; }
        public enum_PositionLEDState_IR PositionLEDState_IR { get; set; }
        public enum_PositionLEDState_RedLight PositionLEDState_RedLight { get; set; }

        public LEDStateChangedEventArgs(enum_LandingLEDState landing, enum_PositionLEDState_IR position_IR, enum_PositionLEDState_RedLight position_RedLight)
        {
            LandingLEDState = landing;
            PositionLEDState_IR = position_IR;
            PositionLEDState_RedLight = position_RedLight;
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

        private static enum_PositionLEDState_IR _positionStateIR;
        public static enum_PositionLEDState_IR PositionLEDState_IR
        {
            get { return _positionStateIR; }
            set
            {
                _positionStateIR = value;
                OnValueChanged();
            }
        }

        private static enum_PositionLEDState_RedLight _postionRedLight;
        public static enum_PositionLEDState_RedLight PositionLEDState_RedLight
        {
            get { return _postionRedLight;}
            set
            {
                _postionRedLight = value;
                OnValueChanged();
            }
        }


        private static void OnValueChanged()
        {
            if (LedStateChanged != null)
                LedStateChanged(null, new LEDStateChangedEventArgs(_landingState, _positionStateIR, _postionRedLight));
        }
        
    }
}
