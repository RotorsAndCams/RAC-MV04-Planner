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

        private bool _compCalibInProgress = false;
        private int _compassMotStatus;
        private System.Timers.Timer _compassCalibTimer;

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
            _compCalibInProgress = false;
            button_CompCalib.Text = "Start";
            button_CompCalib.Enabled = true;
            label_CompassCalib.Text = "";
            textBox_CompassCalib.Text = "";
            _compassCalibTimer = new System.Timers.Timer();
            _compassCalibTimer.Elapsed += _compassCalibTimer_Elapsed;
        }

        private void button_AccelCalib_Click(object sender, EventArgs e)
        {
            if (!_accelCalibInProgress) // Start
            {
                try
                {
                    Log.Info("Sending accel command");

                    MainV2.comPort.doCommand((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, MAVLink.MAV_CMD.PREFLIGHT_CALIBRATION, 0, 0, 0, 0, 1, 0, 0);

                    _accelCalibInProgress = true;

                    _statusText = MainV2.comPort.SubscribeToPacketType(MAVLink.MAVLINK_MSG_ID.STATUSTEXT, receivedPacket, (byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent);
                    _commandLong = MainV2.comPort.SubscribeToPacketType(MAVLink.MAVLINK_MSG_ID.COMMAND_LONG, receivedPacket, (byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent);

                    button_AccelCalib.Text = "Next";

                    button_CompCalib.Enabled = false;
                    
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
                    MainV2.comPort.sendPacket(new MAVLink.mavlink_command_long_t { param1 = (float)_currentPos, command = (ushort)MAVLink.MAV_CMD.ACCELCAL_VEHICLE_POS }, MainV2.comPort.sysidcurrent, MainV2.comPort.compidcurrent);
                }
                catch (Exception ex)
                {
                    _accelCalibInProgress = false;
                    Log.Error("Exception on level", ex);
                    CustomMessageBox.Show("Failed to level", "ERROR");
                }
            }
        }

        private void button_CompCalib_Click(object sender, EventArgs e)
        {
            if (!_compCalibInProgress) // Start
            {
                try
                {
                    MainV2.comPort.doCommand((byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent, MAVLink.MAV_CMD.PREFLIGHT_CALIBRATION, 0, 0, 0, 0, 0, 1, 0);

                    _compassMotStatus = MainV2.comPort.SubscribeToPacketType(MAVLink.MAVLINK_MSG_ID.COMPASSMOT_STATUS, receivedPacket, (byte)MainV2.comPort.sysidcurrent, (byte)MainV2.comPort.compidcurrent);
                   
                    _compCalibInProgress = true;
                    button_CompCalib.Text = "Stop";

                    MainV2.comPort.MAV.cs.messages.Clear();
                    _compassCalibTimer.Start();

                    button_AccelCalib.Enabled = false;
                }
                catch
                {
                    CustomMessageBox.Show("Compassmot requires AC 3.2+", "ERROR");
                }
            }
            else // Stop
            {
                try
                {
                    MainV2.comPort.SendAck();
                    MainV2.comPort.UnSubscribeToPacketType(_compassMotStatus);

                    _compCalibInProgress = false;
                    button_CompCalib.Text = "Start";
                    _compassCalibTimer.Stop();

                    button_AccelCalib.Enabled = true;
                }
                catch {}
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
                            label_AccelCalib.Text = "Done";
                            button_AccelCalib.Text = "Done";
                            button_AccelCalib.Enabled = false;

                            button_CompCalib.Enabled = true;
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

            if (arg.msgid == (uint)MAVLink.MAVLINK_MSG_ID.COMPASSMOT_STATUS)
            {
                var status = (MAVLink.mavlink_compassmot_status_t)arg.data;

                var msg = "Current: "
                    + status.current.ToString("0.00")
                    + "\nx,y,z "
                    + status.CompensationX.ToString("0.00") + ","
                    + status.CompensationY.ToString("0.00") + ","
                    + status.CompensationZ.ToString("0.00")
                    + "\nThrottle: "
                    + (status.throttle / 10.0)
                    + "\nInterference: "
                    + status.interference;

                Invoke((MethodInvoker)delegate
                {
                    label_CompassCalib.Text = msg;
                });
            }

            return true;
        }

        private void _compassCalibTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            StringBuilder message = new StringBuilder();

            MainV2.comPort.MAV.cs.messages.ForEach(x => { message.AppendLine(x.message); });

            textBox_CompassCalib.Text = message.ToString();
            textBox_CompassCalib.SelectionStart = textBox_CompassCalib.Text.Length;
            textBox_CompassCalib.ScrollToCaret();
        }

        private void CalibForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _accelCalibInProgress = false;
                _compCalibInProgress = false;

                if (MainV2.comPort.BaseStream.IsOpen)
                {
                    MainV2.comPort.SendAck();
                }

                MainV2.comPort.UnSubscribeToPacketType(_statusText);
                MainV2.comPort.UnSubscribeToPacketType(_commandLong);
                MainV2.comPort.UnSubscribeToPacketType(_compassMotStatus);

                MainV2.comPort.giveComport = false;
            }
            catch {}

            _compassCalibTimer.Stop();
        }
    }
}
