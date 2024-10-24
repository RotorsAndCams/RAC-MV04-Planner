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

        private static int NoneAxis = 0; // joystickaxis.None

        private static int VirtualAxis = 4; // joystickaxis.ARz

        public static Dictionary<int, RCChannel> RCChannels = new Dictionary<int, RCChannel>
        {
            {1, new RCChannel(MV04_JoyRole.UAV_Roll, NoneAxis, "Roll", true)},
            {2, new RCChannel(MV04_JoyRole.UAV_Pitch, NoneAxis, "Pitch", true)},
            {3, new RCChannel(MV04_JoyRole.UAV_Throttle, NoneAxis, "Throttle / Zoom", true)},
            {4, new RCChannel(MV04_JoyRole.UAV_Yaw, NoneAxis, "UAV Yaw", false)},
            {5, new RCChannel(MV04_JoyRole.Cam_Zoom, NoneAxis, "Camera Zoom", false)},
            {6, new RCChannel(MV04_JoyRole.Cam_Pitch, NoneAxis, "Camera Pitch", false)},
            {7, new RCChannel(MV04_JoyRole.Cam_Yaw, NoneAxis, "Yaw", true)}
        };
        #endregion

        #region Methods
        public static int GetChannelForJoyRole(MV04_JoyRole role)
        {
            return RCChannels.Single(ch => ch.Value.Role == role).Key;
        }
        
        public static int GetAxisForJoyRole(MV04_JoyRole role)
        {
            return RCChannels.Single(ch => ch.Value.Role == role).Value.Axis;
        }

        /// <summary>
        /// Return a set of joystick axes paired to RC channels for the given mode
        /// </summary>
        public static Dictionary<int, int> GetAxisSet(MV04_JoyFlightMode mode)
        {
            Dictionary<int, int> result = new Dictionary<int, int> // Manual is the default case
            {
                {1, GetAxisForJoyRole(MV04_JoyRole.UAV_Roll)},
                {2, GetAxisForJoyRole(MV04_JoyRole.UAV_Pitch)},
                {3, GetAxisForJoyRole(MV04_JoyRole.UAV_Throttle)},
                {4, VirtualAxis}, // UAV Yaw
                {5, NoneAxis},    // Cam Pitch
                {6, NoneAxis},    // Cam Zoom
                {7, GetAxisForJoyRole(MV04_JoyRole.Cam_Yaw)},
            };

            switch (mode)
            {
                case MV04_JoyFlightMode.TapToFly:
                case MV04_JoyFlightMode.Auto:
                case MV04_JoyFlightMode.Follow:
                    result[1] = NoneAxis; // UAV Roll
                    result[2] = NoneAxis; // UAV Pitch
                    result[3] = NoneAxis; // UAV Throttle
                    result[5] = GetAxisForJoyRole(MV04_JoyRole.UAV_Pitch);    // Cam Pitch
                    result[6] = GetAxisForJoyRole(MV04_JoyRole.UAV_Throttle); // Cam Zoom
                    break;
                case MV04_JoyFlightMode.Manual:
                case MV04_JoyFlightMode.Loiter:
                default:
                    break;
            }

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
