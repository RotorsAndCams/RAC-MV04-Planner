using System;
using System.Collections.Generic;
using System.Linq;

namespace MV04.Joystick
{
    #region Button
    public enum buttonfunction_mv04
    {
        MV04_SnapShot = 20,
        MV04_FlightMode,
        MV04_ImageSensor,
        MV04_Arm,
        MV04_Pitch,
        MV04_Zoom
    }

    public enum buttonfunction_mv04_FlightMode_option
    {
        Manual,
        TapToFly,
        Auto,
        Follow
    }

    public enum buttonfunction_mv04_ImageSensor_option
    {
        Day,
        Night
    }

    public enum buttonfunction_mv04_Arm_option
    {
        Safe,
        Armed
    }

    public enum buttonfunction_mv04_Pitch_option
    {
        Stop,
        Up,
        Down
    }

    public enum buttonfunction_mv04_Zoom_option
    {
        Stop,
        In,
        Out
    }
    #endregion

    #region Axis
    
    #region Enums
    /*public enum MV04_JoyAxisRole
    {
        UAV_Roll,
        UAV_Pitch,
        UAV_Throttle,
        UAV_Yaw,
        Cam_Zoom,
        Cam_Pitch,
        Cam_Yaw
    }

    public enum MV04_AxisPair
    {
        Pitch_Pitch,
        Throttle_Zoom
    }

    public enum MV04_AxisMode
    {
        UAV,
        Cam
    }*/

    public enum MV04_JoyFlightMode
    {
        Manual,
        Loiter,
        TapToFly,
        Auto,
        Follow
    }
    #endregion

    public class MV04_RCChannel
    {
        public int RCNum { get; set; }
        public int JoyAxis { get; set; }

        public MV04_RCChannel(int rCNum, int joyAxis)
        {
            RCNum = rCNum;
            JoyAxis = joyAxis;
        }
    }

    public class JoystickModeChangedEventArgs: EventArgs
    {
        public MV04_JoyFlightMode Mode { get; set; }

        public JoystickModeChangedEventArgs(MV04_JoyFlightMode mode)
        {
            Mode = mode;
        }
    }

    public static class JoystickHandler
    {
        #region Fields
        public static event EventHandler<JoystickModeChangedEventArgs> JoystickModeChanged;

        /*public static HashSet<MV04_Axis> JoystickAxies = new HashSet<MV04_Axis>
        {
            new MV04_Axis(MV04_JoystickFunction.UAV_Roll, 1, 0),
            new MV04_Axis(MV04_JoystickFunction.UAV_Pitch, 2, 0),
            new MV04_Axis(MV04_JoystickFunction.UAV_Throttle, 3, 0),
            new MV04_Axis(MV04_JoystickFunction.UAV_Yaw, 4, 0),
            new MV04_Axis(MV04_JoystickFunction.Cam_Pitch, 5, 0),
            new MV04_Axis(MV04_JoystickFunction.Cam_Zoom, 6, 0)
        };*/

        public static Dictionary<int, (string Name, bool Show)> MV04_RCChannelNames = new Dictionary<int, (string, bool)>
        {
            // RCNum, (Name, Show)
            {1, ("Roll", true)},
            {2, ("Pitch", true)},
            {3, ("Throttle / Zoom", true)},
            {4, ("UAV Yaw", false)},
            {5, ("Camera Zoom", false)},
            {6, ("Camera Pitch", false)},
            {7, ("Yaw", true)} // Camera Yaw
        };

        private static int SingleYawVirtualJoystickAxis = 4; // joystickaxis.ARz;

        public static Dictionary<MV04_JoyFlightMode, HashSet<MV04_RCChannel>> MV04_RCChannelSets = new Dictionary<MV04_JoyFlightMode, HashSet<MV04_RCChannel>>
        {
            {MV04_JoyFlightMode.Manual, new HashSet<MV04_RCChannel>{
                new MV04_RCChannel(1, 0),
                new MV04_RCChannel(2, 0),
                new MV04_RCChannel(3, 0),
                new MV04_RCChannel(4, 0),
                new MV04_RCChannel(5, 0),
                new MV04_RCChannel(6, 0),
                new MV04_RCChannel(7, 0),
            }},
            {MV04_JoyFlightMode.Loiter, new HashSet<MV04_RCChannel>{
                new MV04_RCChannel(1, 0),
                new MV04_RCChannel(2, 0),
                new MV04_RCChannel(3, 0),
                new MV04_RCChannel(4, 0),
                new MV04_RCChannel(5, 0),
                new MV04_RCChannel(6, 0),
                new MV04_RCChannel(7, 0),
            }},
            {MV04_JoyFlightMode.TapToFly, new HashSet<MV04_RCChannel>{
                new MV04_RCChannel(1, 0),
                new MV04_RCChannel(2, 0),
                new MV04_RCChannel(3, 0),
                new MV04_RCChannel(4, 0),
                new MV04_RCChannel(5, 0),
                new MV04_RCChannel(6, 0),
                new MV04_RCChannel(7, 0),
            }},
            {MV04_JoyFlightMode.Auto, new HashSet<MV04_RCChannel>{
                new MV04_RCChannel(1, 0),
                new MV04_RCChannel(2, 0),
                new MV04_RCChannel(3, 0),
                new MV04_RCChannel(4, 0),
                new MV04_RCChannel(5, 0),
                new MV04_RCChannel(6, 0),
                new MV04_RCChannel(7, 0),
            }},
            {MV04_JoyFlightMode.Follow, new HashSet<MV04_RCChannel>{
                new MV04_RCChannel(1, 0),
                new MV04_RCChannel(2, 0),
                new MV04_RCChannel(3, 0),
                new MV04_RCChannel(4, 0),
                new MV04_RCChannel(5, 0),
                new MV04_RCChannel(6, 0),
                new MV04_RCChannel(7, 0),
            }},
        };
        #endregion

        #region Methods
        /// <summary>
        /// Trigger a JoystickModeChanged event with the given parameters
        /// </summary>
        /// <param name="mode"></param>
        public static void TriggerJoystickModeChangedEvent(MV04_JoyFlightMode mode)
        {
            if (JoystickModeChanged != null)
            {
                JoystickModeChanged(null, new JoystickModeChangedEventArgs(mode));
            }
        }

        /*public static int GetAxis(MV04_AxisPair axisPair)
        {
            if (axisPair == MV04_AxisPair.Pitch_Pitch)
            {
                if (JoystickAxies.Single(x => x.Function == MV04_JoyAxisRole.UAV_Pitch).RCAxis > 0)
                {
                    return JoystickAxies.Single(x => x.Function == MV04_JoyAxisRole.UAV_Pitch).RCAxis;
                }
                else //if (JoystickAxies.Single(x => x.Function == MV04_JoystickFunction.Cam_Pitch).RCAxis > 0)
                {
                    return JoystickAxies.Single(x => x.Function == MV04_JoyAxisRole.Cam_Pitch).RCAxis;
                }
            }
            else
            {
                if (JoystickAxies.Single(x => x.Function == MV04_JoyAxisRole.UAV_Throttle).RCAxis > 0)
                {
                    return JoystickAxies.Single(x => x.Function == MV04_JoyAxisRole.UAV_Throttle).RCAxis;
                }
                else //if (JoystickAxies.Single(x => x.Function == MV04_JoystickFunction.Cam_Zoom).RCAxis > 0)
                {
                    return JoystickAxies.Single(x => x.Function == MV04_JoyAxisRole.Cam_Zoom).RCAxis;
                }
            }
        }*/
        #endregion
    }
    #endregion
}
