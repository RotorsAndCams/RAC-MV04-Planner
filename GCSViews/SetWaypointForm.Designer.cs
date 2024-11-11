namespace MissionPlanner.GCSViews
{
    partial class SetWaypointForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lb_Lat = new System.Windows.Forms.Label();
            this.ln_Lon = new System.Windows.Forms.Label();
            this.lb_Alt = new System.Windows.Forms.Label();
            this.tb_GeoCoord = new System.Windows.Forms.TableLayoutPanel();
            this.rtb_Latitude = new System.Windows.Forms.RichTextBox();
            this.rtb_Longitude = new System.Windows.Forms.RichTextBox();
            this.rtb_Altitude = new System.Windows.Forms.RichTextBox();
            this.tlp_Base = new System.Windows.Forms.TableLayoutPanel();
            this.pnl_Buttons = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.btn_DirectOverride = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btn_DirectCommand = new System.Windows.Forms.Button();
            this.btn_Go2 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btn_Solution3 = new System.Windows.Forms.Button();
            this.btn_Solution2 = new System.Windows.Forms.Button();
            this.btn_Solution1 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.tb_GeoCoord.SuspendLayout();
            this.tlp_Base.SuspendLayout();
            this.pnl_Buttons.SuspendLayout();
            this.SuspendLayout();
            // 
            // lb_Lat
            // 
            this.lb_Lat.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lb_Lat.AutoSize = true;
            this.lb_Lat.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lb_Lat.Location = new System.Drawing.Point(134, 31);
            this.lb_Lat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_Lat.Name = "lb_Lat";
            this.lb_Lat.Size = new System.Drawing.Size(96, 25);
            this.lb_Lat.TabIndex = 0;
            this.lb_Lat.Text = "Latitude:";
            this.lb_Lat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ln_Lon
            // 
            this.ln_Lon.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ln_Lon.AutoSize = true;
            this.ln_Lon.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ln_Lon.Location = new System.Drawing.Point(125, 118);
            this.ln_Lon.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ln_Lon.Name = "ln_Lon";
            this.ln_Lon.Size = new System.Drawing.Size(114, 25);
            this.ln_Lon.TabIndex = 1;
            this.ln_Lon.Text = "Longitude:";
            // 
            // lb_Alt
            // 
            this.lb_Alt.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lb_Alt.AutoSize = true;
            this.lb_Alt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lb_Alt.Location = new System.Drawing.Point(136, 206);
            this.lb_Alt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lb_Alt.Name = "lb_Alt";
            this.lb_Alt.Size = new System.Drawing.Size(92, 25);
            this.lb_Alt.TabIndex = 2;
            this.lb_Alt.Text = "Altitude:";
            // 
            // tb_GeoCoord
            // 
            this.tb_GeoCoord.ColumnCount = 2;
            this.tb_GeoCoord.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tb_GeoCoord.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tb_GeoCoord.Controls.Add(this.lb_Lat, 0, 0);
            this.tb_GeoCoord.Controls.Add(this.ln_Lon, 0, 1);
            this.tb_GeoCoord.Controls.Add(this.lb_Alt, 0, 2);
            this.tb_GeoCoord.Controls.Add(this.rtb_Latitude, 1, 0);
            this.tb_GeoCoord.Controls.Add(this.rtb_Longitude, 1, 1);
            this.tb_GeoCoord.Controls.Add(this.rtb_Altitude, 1, 2);
            this.tb_GeoCoord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_GeoCoord.Location = new System.Drawing.Point(4, 4);
            this.tb_GeoCoord.Margin = new System.Windows.Forms.Padding(4);
            this.tb_GeoCoord.Name = "tb_GeoCoord";
            this.tb_GeoCoord.RowCount = 3;
            this.tb_GeoCoord.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tb_GeoCoord.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tb_GeoCoord.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tb_GeoCoord.Size = new System.Drawing.Size(730, 263);
            this.tb_GeoCoord.TabIndex = 6;
            // 
            // rtb_Latitude
            // 
            this.rtb_Latitude.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_Latitude.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_Latitude.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rtb_Latitude.Location = new System.Drawing.Point(369, 4);
            this.rtb_Latitude.Margin = new System.Windows.Forms.Padding(4);
            this.rtb_Latitude.Multiline = false;
            this.rtb_Latitude.Name = "rtb_Latitude";
            this.rtb_Latitude.Size = new System.Drawing.Size(357, 79);
            this.rtb_Latitude.TabIndex = 3;
            this.rtb_Latitude.Text = "47.50269208677946";
            // 
            // rtb_Longitude
            // 
            this.rtb_Longitude.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_Longitude.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_Longitude.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rtb_Longitude.Location = new System.Drawing.Point(369, 91);
            this.rtb_Longitude.Margin = new System.Windows.Forms.Padding(4);
            this.rtb_Longitude.Multiline = false;
            this.rtb_Longitude.Name = "rtb_Longitude";
            this.rtb_Longitude.Size = new System.Drawing.Size(357, 79);
            this.rtb_Longitude.TabIndex = 4;
            this.rtb_Longitude.Text = "19.20124639140316";
            // 
            // rtb_Altitude
            // 
            this.rtb_Altitude.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_Altitude.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_Altitude.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rtb_Altitude.Location = new System.Drawing.Point(369, 178);
            this.rtb_Altitude.Margin = new System.Windows.Forms.Padding(4);
            this.rtb_Altitude.Multiline = false;
            this.rtb_Altitude.Name = "rtb_Altitude";
            this.rtb_Altitude.Size = new System.Drawing.Size(357, 81);
            this.rtb_Altitude.TabIndex = 5;
            this.rtb_Altitude.Text = "25";
            // 
            // tlp_Base
            // 
            this.tlp_Base.ColumnCount = 1;
            this.tlp_Base.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Base.Controls.Add(this.tb_GeoCoord, 0, 0);
            this.tlp_Base.Controls.Add(this.pnl_Buttons, 0, 1);
            this.tlp_Base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_Base.Location = new System.Drawing.Point(0, 0);
            this.tlp_Base.Margin = new System.Windows.Forms.Padding(4);
            this.tlp_Base.Name = "tlp_Base";
            this.tlp_Base.RowCount = 2;
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Base.Size = new System.Drawing.Size(738, 542);
            this.tlp_Base.TabIndex = 7;
            // 
            // pnl_Buttons
            // 
            this.pnl_Buttons.Controls.Add(this.button5);
            this.pnl_Buttons.Controls.Add(this.button4);
            this.pnl_Buttons.Controls.Add(this.btn_DirectOverride);
            this.pnl_Buttons.Controls.Add(this.button3);
            this.pnl_Buttons.Controls.Add(this.btn_DirectCommand);
            this.pnl_Buttons.Controls.Add(this.btn_Go2);
            this.pnl_Buttons.Controls.Add(this.button2);
            this.pnl_Buttons.Controls.Add(this.button1);
            this.pnl_Buttons.Controls.Add(this.btn_Solution3);
            this.pnl_Buttons.Controls.Add(this.btn_Solution2);
            this.pnl_Buttons.Controls.Add(this.btn_Solution1);
            this.pnl_Buttons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Buttons.Location = new System.Drawing.Point(4, 275);
            this.pnl_Buttons.Margin = new System.Windows.Forms.Padding(4);
            this.pnl_Buttons.Name = "pnl_Buttons";
            this.pnl_Buttons.Size = new System.Drawing.Size(730, 263);
            this.pnl_Buttons.TabIndex = 7;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(518, 4);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(141, 60);
            this.button4.TabIndex = 9;
            this.button4.Text = "loop ";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // btn_DirectOverride
            // 
            this.btn_DirectOverride.Location = new System.Drawing.Point(369, 149);
            this.btn_DirectOverride.Margin = new System.Windows.Forms.Padding(4);
            this.btn_DirectOverride.Name = "btn_DirectOverride";
            this.btn_DirectOverride.Size = new System.Drawing.Size(141, 60);
            this.btn_DirectOverride.TabIndex = 8;
            this.btn_DirectOverride.Text = "direct false yaw";
            this.btn_DirectOverride.UseVisualStyleBackColor = true;
            this.btn_DirectOverride.Click += new System.EventHandler(this.btn_DirectOverride_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(369, 81);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(141, 60);
            this.button3.TabIndex = 7;
            this.button3.Text = "direct true yaw";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // btn_DirectCommand
            // 
            this.btn_DirectCommand.Location = new System.Drawing.Point(369, 4);
            this.btn_DirectCommand.Margin = new System.Windows.Forms.Padding(4);
            this.btn_DirectCommand.Name = "btn_DirectCommand";
            this.btn_DirectCommand.Size = new System.Drawing.Size(141, 60);
            this.btn_DirectCommand.TabIndex = 6;
            this.btn_DirectCommand.Text = "direct command";
            this.btn_DirectCommand.UseVisualStyleBackColor = true;
            this.btn_DirectCommand.Click += new System.EventHandler(this.btn_DirectCommand_Click);
            // 
            // btn_Go2
            // 
            this.btn_Go2.Location = new System.Drawing.Point(199, 139);
            this.btn_Go2.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Go2.Name = "btn_Go2";
            this.btn_Go2.Size = new System.Drawing.Size(141, 60);
            this.btn_Go2.TabIndex = 5;
            this.btn_Go2.Text = "go2";
            this.btn_Go2.UseVisualStyleBackColor = true;
            this.btn_Go2.Click += new System.EventHandler(this.btn_Go2_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(199, 71);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(141, 60);
            this.button2.TabIndex = 4;
            this.button2.Text = "go";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(199, 4);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(141, 60);
            this.button1.TabIndex = 3;
            this.button1.Text = "mukodott";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_Solution3
            // 
            this.btn_Solution3.BackColor = System.Drawing.Color.Gray;
            this.btn_Solution3.Location = new System.Drawing.Point(12, 139);
            this.btn_Solution3.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Solution3.Name = "btn_Solution3";
            this.btn_Solution3.Size = new System.Drawing.Size(179, 60);
            this.btn_Solution3.TabIndex = 2;
            this.btn_Solution3.Text = "Go to here (Latest MP solution)";
            this.btn_Solution3.UseVisualStyleBackColor = false;
            this.btn_Solution3.Click += new System.EventHandler(this.btn_Solution3_Click);
            // 
            // btn_Solution2
            // 
            this.btn_Solution2.BackColor = System.Drawing.Color.Gray;
            this.btn_Solution2.Location = new System.Drawing.Point(12, 71);
            this.btn_Solution2.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Solution2.Name = "btn_Solution2";
            this.btn_Solution2.Size = new System.Drawing.Size(179, 60);
            this.btn_Solution2.TabIndex = 1;
            this.btn_Solution2.Text = "Go to here (AGL)";
            this.btn_Solution2.UseVisualStyleBackColor = false;
            this.btn_Solution2.Click += new System.EventHandler(this.btn_Solution2_Click);
            // 
            // btn_Solution1
            // 
            this.btn_Solution1.BackColor = System.Drawing.Color.Gray;
            this.btn_Solution1.Location = new System.Drawing.Point(12, 4);
            this.btn_Solution1.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Solution1.Name = "btn_Solution1";
            this.btn_Solution1.Size = new System.Drawing.Size(179, 60);
            this.btn_Solution1.TabIndex = 0;
            this.btn_Solution1.Text = "Go to here (Fly to here)";
            this.btn_Solution1.UseVisualStyleBackColor = false;
            this.btn_Solution1.Click += new System.EventHandler(this.btn_Solution1_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(518, 81);
            this.button5.Margin = new System.Windows.Forms.Padding(4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(141, 60);
            this.button5.TabIndex = 10;
            this.button5.Text = "go to wp flightdata v2";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // SetWaypointForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 542);
            this.Controls.Add(this.tlp_Base);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetWaypointForm";
            this.ShowIcon = false;
            this.Text = "SetWaypointForm";
            this.tb_GeoCoord.ResumeLayout(false);
            this.tb_GeoCoord.PerformLayout();
            this.tlp_Base.ResumeLayout(false);
            this.pnl_Buttons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lb_Lat;
        private System.Windows.Forms.Label ln_Lon;
        private System.Windows.Forms.Label lb_Alt;
        private System.Windows.Forms.TableLayoutPanel tb_GeoCoord;
        private System.Windows.Forms.TableLayoutPanel tlp_Base;
        private System.Windows.Forms.Panel pnl_Buttons;
        private System.Windows.Forms.RichTextBox rtb_Latitude;
        private System.Windows.Forms.RichTextBox rtb_Longitude;
        private System.Windows.Forms.RichTextBox rtb_Altitude;
        private System.Windows.Forms.Button btn_Solution3;
        private System.Windows.Forms.Button btn_Solution2;
        private System.Windows.Forms.Button btn_Solution1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btn_Go2;
        private System.Windows.Forms.Button btn_DirectCommand;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btn_DirectOverride;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}