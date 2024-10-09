using MissionPlanner.Utilities;
using MV04.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IronPython.Modules._ast;

namespace MissionPlanner.GCSViews
{
    public partial class GetUserNameForm : Form
    {
        private const string PilotDataFileName = "PilotLog.json";

        public GetUserNameForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProceedApplication();
        }

        private void tb_Name_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ProceedApplication();
            }
        }

        private void ProceedApplication()
        {
            if (!CheckInputIsValid())
            {
                return;
            }
                

            Save();
            this.Close();
        }

        private bool CheckInputIsValid()
        {
            if (this.tb_Name.Text.Length < 3)
            {
                CustomMessageBox.Show("Enter a valid name");
                return false;
            }
            return true;
        }

        private void Save()
        {
            try
            {
                var fileToWrite = MissionPlanner.Utilities.Settings.GetUserDataDirectory() + PilotDataFileName;

                var lst = new List<PilotUserLog>();

                if (File.Exists(fileToWrite))
                    lst = JsonConvert.DeserializeObject<List<PilotUserLog>>(File.ReadAllText(fileToWrite));

                lst.Add(new PilotUserLog() { Name = this.tb_Name.Text, Datetime = DateTime.Now.ToString("yyyyMMddHHmmss") });

                File.WriteAllText(fileToWrite, lst.ToJSON());
            }
            catch(Exception ex)
            {
                //continue normal run
                this.Close();
            }

            
        }

        
    }

    public class PilotUserLog
    {
        public string Name;
        public string Datetime;
    }
}
