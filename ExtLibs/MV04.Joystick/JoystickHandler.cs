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
    public enum MV04_JoystickFunction
    {
        UAV_Roll,
        UAV_Pitch,
        UAV_Throttle,
        UAV_Yaw,
        Cam_Pitch,
        Cam_Zoom,
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
    }
    #endregion

    public class MV04_Axis
    {
        public MV04_JoystickFunction Function { get; set; }
        public int RCChannelNo { get; set; }
        public int RCAxis { get; set; }

        public MV04_Axis(MV04_JoystickFunction function, int rCChannelNo, int rCAxis)
        {
            Function = function;
            RCChannelNo = rCChannelNo;
            RCAxis = rCAxis;
        }
    }

    public class JoystickAxiesChangedEventArgs: EventArgs
    {
        public MV04_AxisPair AxisPair { get; set; }
        public MV04_AxisMode AxisMode { get; set; }

        public JoystickAxiesChangedEventArgs(MV04_AxisPair axisPair, MV04_AxisMode axisMode)
        {
            AxisPair = axisPair;
            AxisMode = axisMode;
        }
    }

    public static class JoystickHandler
    {
        #region Fields
        public static event EventHandler<JoystickAxiesChangedEventArgs> JoystickAxiesChanged;

        public static HashSet<MV04_Axis> JoystickAxies = new HashSet<MV04_Axis>
        {
            new MV04_Axis(MV04_JoystickFunction.UAV_Roll, 1, 0),
            new MV04_Axis(MV04_JoystickFunction.UAV_Pitch, 2, 0),
            new MV04_Axis(MV04_JoystickFunction.UAV_Throttle, 3, 0),
            new MV04_Axis(MV04_JoystickFunction.UAV_Yaw, 4, 0),
            new MV04_Axis(MV04_JoystickFunction.Cam_Pitch, 5, 0),
            new MV04_Axis(MV04_JoystickFunction.Cam_Zoom, 6, 0),
            new MV04_Axis(MV04_JoystickFunction.Cam_Yaw, 7, 0),
        };
        #endregion

        #region Methods
        /// <summary>
        /// Trigger a JoystickAxiesChanged event with the given parameters
        /// </summary>
        /// <param name="axisPair">Changed axis pair</param>
        /// <param name="axisMode">Control target set</param>
        public static void TriggerJoystickAxiesChangedEvent(MV04_AxisPair axisPair, MV04_AxisMode axisMode)
        {
            if (JoystickAxiesChanged != null)
            {
                JoystickAxiesChanged(null, new JoystickAxiesChangedEventArgs(axisPair, axisMode));
            }
        }

        /// <summary>
        /// Return the set joystick axis for the given axis pair
        /// </summary>
        /// <param name="axisPair">Axis pair to get the set joystick axis for</param>
        /// <returns>joystickaxis enum value as an int</returns>
        public static int GetAxis(MV04_AxisPair axisPair)
        {
            if (axisPair == MV04_AxisPair.Pitch_Pitch)
            {
                if (JoystickAxies.Single(x => x.Function == MV04_JoystickFunction.UAV_Pitch).RCAxis > 0)
                {
                    return JoystickAxies.Single(x => x.Function == MV04_JoystickFunction.UAV_Pitch).RCAxis;
                }
                else //if (JoystickAxies.Single(x => x.Function == MV04_JoystickFunction.Cam_Pitch).RCAxis > 0)
                {
                    return JoystickAxies.Single(x => x.Function == MV04_JoystickFunction.Cam_Pitch).RCAxis;
                }
            }
            else
            {
                if (JoystickAxies.Single(x => x.Function == MV04_JoystickFunction.UAV_Throttle).RCAxis > 0)
                {
                    return JoystickAxies.Single(x => x.Function == MV04_JoystickFunction.UAV_Throttle).RCAxis;
                }
                else //if (JoystickAxies.Single(x => x.Function == MV04_JoystickFunction.Cam_Zoom).RCAxis > 0)
                {
                    return JoystickAxies.Single(x => x.Function == MV04_JoystickFunction.Cam_Zoom).RCAxis;
                }
            }
        }
        #endregion
    }
    #endregion
}
