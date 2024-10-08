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
            string name = this.tb_Name.Text;

            if (name.Length < 3)
            {
                MessageBox.Show("Enter a valid name");
                return;
            }

            //var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);




            //check valid text
            //save text to xml

            Save();
            this.Close();

        }

        private void Save()
        {
            var fileToWrite = MissionPlanner.Utilities.Settings.GetUserDataDirectory() + PilotDataFileName;

            //if (!File.Exists(fileToWrite))
            //{ 
            //    File.Create(fileToWrite);
            //}

            var lst = JsonConvert.DeserializeObject<List<PilotUserLog>>(File.ReadAllText(fileToWrite)) ?? new List<PilotUserLog>();

            lst.Add(new PilotUserLog() { Name = this.tb_Name.Text, Datetime = DateTime.Now.ToString("yyyyMMddHHmmss") });

            

            File.WriteAllText(fileToWrite, lst.ToJSON());
        }
    }

    public class PilotUserLog
    {
        public string Name;
        public string Datetime;
    }
}
