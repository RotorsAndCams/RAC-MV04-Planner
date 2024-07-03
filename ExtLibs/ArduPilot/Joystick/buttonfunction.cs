using MV04.GCS;

namespace MissionPlanner.Joystick
{
    public enum buttonfunction
    {
        // Base functions
        ChangeMode,
        Do_Set_Relay,
        Do_Repeat_Relay,
        Do_Set_Servo,
        Do_Repeat_Servo,
        Arm,
        Disarm,
        Digicam_Control,
        TakeOff,
        Mount_Mode,
        Toggle_Pan_Stab,
        Gimbal_pnt_track,
        Mount_Control_0,
        Button_axis0,
        Button_axis1,

        // MV04 functions
        MV04_SnapShot = buttonfunction_mv04.MV04_SnapShot,
        MV04_FlightMode = buttonfunction_mv04.MV04_FlightMode,
        MV04_ImageSensor = buttonfunction_mv04.MV04_ImageSensor,
        MV04_Arm = buttonfunction_mv04.MV04_Arm,
        MV04_Pitch = buttonfunction_mv04.MV04_Pitch,
        MV04_Zoom = buttonfunction_mv04.MV04_Zoom
    }
}
