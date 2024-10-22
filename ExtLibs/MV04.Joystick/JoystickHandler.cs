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
    public enum MV04_JoyRole
    {
        UAV_Roll,
        UAV_Pitch,
        UAV_Throttle,
        UAV_Yaw,
        Cam_Zoom,
        Cam_Pitch,
        Cam_Yaw
    }

    public enum MV04_JoyFlightMode
    {
        Manual,
        Loiter,
        TapToFly,
        Auto,
        Follow
    }
    #endregion

    public struct MainAxes
    {
        public int RollAxis;
        public int PitchAxis;
        public int ThrottleAxis;
        public int YawAxis;
    }

    public class RCChannel
    {
        public MV04_JoyRole Role { get; private set; }
        public int Axis { get; set; }
        public string Name { get; private set; }
        public bool Show { get; private set; }

        public RCChannel(MV04_JoyRole role, int axis, string name, bool show)
        {
            Role = role;
            Axis = axis;
            Name = name;
            Show = show;
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

        public static Dictionary<int, RCChannel> RCChannels = new Dictionary<int, RCChannel>
        {
            {1, new RCChannel(MV04_JoyRole.UAV_Roll, 0, "Roll", true)},
            {2, new RCChannel(MV04_JoyRole.UAV_Pitch, 0, "Pitch", true)},
            {3, new RCChannel(MV04_JoyRole.UAV_Throttle, 0, "Throttle / Zoom", true)},
            {4, new RCChannel(MV04_JoyRole.UAV_Yaw, 0, "UAV Yaw", false)},
            {5, new RCChannel(MV04_JoyRole.Cam_Zoom, 0, "Camera Zoom", false)},
            {6, new RCChannel(MV04_JoyRole.Cam_Pitch, 0, "Camera Pitch", false)},
            {7, new RCChannel(MV04_JoyRole.Cam_Yaw, 0, "Yaw", true)}
        };

        private static int SingleYawVirtualJoystickAxis = 4; // joystickaxis.ARz;
        #endregion

        #region Methods
        private static int GetRCChannelForJoyRole(MV04_JoyRole role)
        {
            return RCChannels.Single(ch => ch.Value.Role == role).Key;
        }

        /// <summary>
        /// Return a set of joystick axes paired to RC channels for the given mode
        /// </summary>
        public static Dictionary<int, int> GetAxisSet(MV04_JoyFlightMode mode)
        {
            Dictionary<int, int> result = new Dictionary<int, int>();

            // TODO: Fill result according to mode and RCChannels

            return result;
        }

        /// <summary>
        /// Trigger a JoystickModeChanged event with the given parameters
        /// </summary>
        public static void TriggerJoystickModeChangedEvent(MV04_JoyFlightMode mode)
        {
            if (JoystickModeChanged != null)
            {
                JoystickModeChanged(null, new JoystickModeChangedEventArgs(mode));
            }
        }
        #endregion
    }
    #endregion
}
