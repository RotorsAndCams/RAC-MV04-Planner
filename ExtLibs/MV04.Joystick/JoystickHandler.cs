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
        MV04_Yaw,
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

    public enum buttonfunction_mv04_Yaw_option
    {
        Stop,
        Left,
        Right
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

        public static Dictionary<int, RCChannel> RCChannels = new Dictionary<int, RCChannel>
        {
            {1, new RCChannel(MV04_JoyRole.UAV_Roll, NoneAxis, "Roll", true)},
            {2, new RCChannel(MV04_JoyRole.UAV_Pitch, NoneAxis, "Pitch", true)},
            {3, new RCChannel(MV04_JoyRole.UAV_Throttle, NoneAxis, "Throttle / Zoom", true)},
            {4, new RCChannel(MV04_JoyRole.UAV_Yaw, NoneAxis, "Yaw", true)},
            {5, new RCChannel(MV04_JoyRole.Cam_Zoom, NoneAxis, "Camera Zoom", false)},
            {6, new RCChannel(MV04_JoyRole.Cam_Pitch, NoneAxis, "Camera Pitch", false)},
            {7, new RCChannel(MV04_JoyRole.Cam_Yaw, NoneAxis, "Camera Yaw", false)}
        };

        #endregion

        #region Methods

        /// <summary>
        /// Get the channel number for the given role
        /// </summary>
        public static int GetChannelForJoyRole(MV04_JoyRole role)
        {
            return RCChannels.Single(ch => ch.Value.Role == role).Key;
        }

        /// <summary>
        /// Get the axis number for the given role
        /// </summary>
        public static int GetAxisForJoyRole(MV04_JoyRole role)
        {
            return RCChannels.Single(ch => ch.Value.Role == role).Value.Axis;
        }

        /// <summary>
        /// Return a set of joystick axes paired to RC channels for the given mode
        /// </summary>
        public static Dictionary<int, int> GetAxisSet(MV04_JoyFlightMode mode)
        {
            Dictionary<int, int> result = new Dictionary<int, int>();

            switch (mode)
            {
                case MV04_JoyFlightMode.TapToFly:
                case MV04_JoyFlightMode.Auto:
                case MV04_JoyFlightMode.Follow:
                    // Cam control only
                    result[1] = NoneAxis;                                       // UAV Roll
                    result[2] = NoneAxis;                                       // UAV Pitch
                    result[3] = NoneAxis;                                       // UAV Throttle
                    result[4] = NoneAxis;                                       // UAV Yaw
                    result[5] = GetAxisForJoyRole(MV04_JoyRole.UAV_Pitch);      // Cam Pitch
                    result[6] = GetAxisForJoyRole(MV04_JoyRole.UAV_Throttle);   // Cam Zoom
                    result[7] = GetAxisForJoyRole(MV04_JoyRole.UAV_Yaw);        // Cam Yaw
                    break;
                
                case MV04_JoyFlightMode.Manual:
                default:
                    // UAV control only
                    result[1] = GetAxisForJoyRole(MV04_JoyRole.UAV_Roll);       // UAV Roll
                    result[2] = GetAxisForJoyRole(MV04_JoyRole.UAV_Pitch);      // UAV Pitch
                    result[3] = GetAxisForJoyRole(MV04_JoyRole.UAV_Throttle);   // UAV Throttle
                    result[4] = GetAxisForJoyRole(MV04_JoyRole.UAV_Yaw);        // UAV Yaw
                    result[5] = NoneAxis;                                       // Cam Pitch
                    result[6] = NoneAxis;                                       // Cam Zoom
                    result[7] = NoneAxis;                                       // Cam Yaw
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
