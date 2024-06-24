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
        MV04_SnapShot = 20,
        MV04_FlightMode,
        MV04_ImageSensor,
        MV04_Arm,
        MV04_Pitch,
        MV04_Zoom
    }
}
