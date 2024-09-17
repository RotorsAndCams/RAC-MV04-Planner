# MV04 GCS Remote functions<!-- omit from toc -->

1. [How to add new joystick button function](#how-to-add-new-joystick-button-function)

## How to add new joystick button function

1. Add new item(s) to the `MissionPlanner.Joystick.buttonfunction` enum in **ExtLibs/ArduPilot/Joystick/buttonfunction.cs**
	- This will add them to the joystick button function drop-down lists in MP at **Config/Optional Hardware/Joystick**
3. (optional) Add new case(s) to the `switch` in the `but_settings_Click` function at **MissionPlanner/Joystick/JoystickSetup.cs**
	- Also create a new `form` for each switch-case to open, where the user can set parameter(s) for that button function
	- This will open the created `form` when the **Settings*** button is pressed on the Joystick setup screen
4. Add new `case(s)` to the `switch` in the `ProcessButtonEvent` function at **ExtLibs/ArduPilot/Joystick/JoystickBase.cs**
	- This is what will execute when the joystick button assigned to this function is pressed by the user during flight
