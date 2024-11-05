using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IronPython.Modules._ast;
using static IronPython.Modules.PythonIterTools;
using static MAVLink;

namespace MissionPlanner.Controls
{
    public partial class CalibForm : Form
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private bool _accelCalibInProgress = false;
        private int _statusText,
                    _commandLong;
        private MAVLink.ACCELCAL_VEHICLE_POS _currentPos;

        public CalibForm()
        {
            InitializeComponent();
            Utilities.ThemeManager.ApplyThemeTo(this);

            // Accel calib
            _accelCalibInProgress = false;
            button_AccelCalib.Text = "Start";
            button_AccelCalib.Enabled = true;
            label_AccelCalib.Text = "";

            // Compass calib
            // ...
        }

        private void button_AccelCalib_Click(object sender, EventArgs e)
        {
            if (!_accelCalibInProgress) // Start
            {
                try
                {
                    Log.Info("Sending accel command");

                    if (MainV2.comPort.doCommand((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent,
                        MAVLink.MAV_CMD.PREFLIGHT_CALIBRATION, 0, 0, 0, 0, 1, 0, 0))
                    {
                        _accelCalibInProgress = true;

                        _statusText = MainV2.comPort.SubscribeToPacketType(MAVLink.MAVLINK_MSG_ID.STATUSTEXT, receivedPacket, (byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent);
                        _commandLong = MainV2.comPort.SubscribeToPacketType(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, receivedPacket, (byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent);

                        button_AccelCalib.Text = "Click when Done";
                    }
                    else
                    {
                        _accelCalibInProgress = false;
                        Log.Error("Exception on level");
                        CustomMessageBox.Show("Failed to level", "ERROR");
                    }
                }
                catch (Exception ex)
                {
                    _accelCalibInProgress = false;
                    Log.Error("Exception on level", ex);
                    CustomMessageBox.Show("Failed to level", "ERROR");
                }
            }
            else // Continue
            {
                try
                {
                    MainV2.comPort.sendPacket(new MAVLink.mavlink_command_long_t { param1 = (float)_currentPos, command = (ushort)MAVLink.MAV_CMD.ACCELCAL_VEHICLE_POS },
                        MainV2.comPort.sysidcurrent, MainV2.comPort.compidcurrent);
                }
                catch (Exception ex)
                {
                    _accelCalibInProgress = false;
                    Log.Error("Exception on level", ex);
                    CustomMessageBox.Show("Failed to level", "ERROR");
                }
            }
        }

        private bool receivedPacket(MAVLink.MAVLinkMessage arg)
        {
            if (arg.msgid == (uint)MAVLink.MAVLINK_MSG_ID.STATUSTEXT)
            {
                string message = Encoding.ASCII.GetString(arg.ToStructure<MAVLink.mavlink_statustext_t>().text);

                Invoke((MethodInvoker)delegate
                {
                    label_AccelCalib.Text = message;
                });

                if (message.ToLower().Contains("calibration successful")
                    || message.ToLower().Contains("calibration failed"))
                {
                    try
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            button_AccelCalib.Text = "Done";
                            button_AccelCalib.Enabled = false;
                        });

                        _accelCalibInProgress = false;
                        MainV2.comPort.UnSubscribeToPacketType(_statusText);
                        MainV2.comPort.UnSubscribeToPacketType(_commandLong);
                    }
                    catch {}
                }
            }

            if (arg.msgid == (uint)MAVLink.MAVLINK_MSG_ID.COMMAND_LONG)
            {
                mavlink_command_long_t message = arg.ToStructure<mavlink_command_long_t>();
                if (message.command == (ushort)MAVLink.MAV_CMD.ACCELCAL_VEHICLE_POS)
                {
                    _currentPos = (MAVLink.ACCELCAL_VEHICLE_POS)message.param1;

                    Invoke((MethodInvoker)delegate
                    {
                        label_AccelCalib.Text = "Please place vehicle " + _currentPos.ToString();
                    });
                }
            }

            return true;
        }
    }
}
