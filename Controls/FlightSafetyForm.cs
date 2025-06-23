using log4net;
using MV04.State;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace MissionPlanner.Controls
{
    public partial class FlightSafetyForm : Form
    {
        /// <summary>
        /// The MAVLink interface
        /// </summary>
        public MAVLinkInterface MAVLink { get; private set; }

        /// <summary>
        /// Parameter names for the Parameters tab
        /// </summary>
        public HashSet<string> ParamSetList { get; private set; }

        private bool _mavConnected
        {
            get
            {
                return MAVLink != null
                    && MAVLink.BaseStream != null
                    && MAVLink.BaseStream.IsOpen
                    && MAVLink.MAV.param.Count > 0;
            }
        }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public FlightSafetyForm(MAVLinkInterface mav, HashSet<string> paramList)
        {
            InitializeComponent();

            this.MAVLink = mav;
            ParamSetList = paramList;

            AddParamSetControls();
        }

        private void AddParamSetControls()
        {
            if (!_mavConnected)
            {
                log.Error("Cannot add ParamSet controls - MAV not connected");
                return;
            }

            // Fill ParamSet tab
            foreach (string param in ParamSetList)
            {
                if (!MAVLink.MAV.param.ContainsKey(param))
                {
                    log.Error($"Cannot add ParamSet control - Param \"{param}\" not found");
                    continue;
                }

                flowLayoutPanel_ParamSet.Controls.Add(new ParamSet2(MAVLink, param));
            }

            // Register ParamSet2.Reload() for relevant event(s)
            tabControl1.SelectedIndexChanged += ReloadParamSetControls;
        }

        private void ReloadParamSetControls(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage_ParamSet)
            {
                foreach (Control control in flowLayoutPanel_ParamSet.Controls)
                {
                    if (control is ParamSet2)
                    {
                        (control as ParamSet2).Reload();
                    }
                }
            }
        }

        private void button_FlightPlanCheck_Click(object sender, EventArgs e)
        {
            MainV2.CheckFlightPlan(null, new MV04StateChangeEventArgs() { PreviousState = MV04_State.Manual, NewState = MV04_State.Auto });
        }
    }
}
