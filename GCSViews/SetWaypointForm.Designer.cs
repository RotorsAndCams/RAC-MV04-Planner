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
            this.btn_Solution3 = new System.Windows.Forms.Button();
            this.btn_Solution2 = new System.Windows.Forms.Button();
            this.btn_Solution1 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
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
            this.lb_Lat.Location = new System.Drawing.Point(56, 18);
            this.lb_Lat.Name = "lb_Lat";
            this.lb_Lat.Size = new System.Drawing.Size(80, 20);
            this.lb_Lat.TabIndex = 0;
            this.lb_Lat.Text = "Latitude:";
            this.lb_Lat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ln_Lon
            // 
            this.ln_Lon.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ln_Lon.AutoSize = true;
            this.ln_Lon.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ln_Lon.Location = new System.Drawing.Point(49, 74);
            this.ln_Lon.Name = "ln_Lon";
            this.ln_Lon.Size = new System.Drawing.Size(94, 20);
            this.ln_Lon.TabIndex = 1;
            this.ln_Lon.Text = "Longitude:";
            // 
            // lb_Alt
            // 
            this.lb_Alt.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lb_Alt.AutoSize = true;
            this.lb_Alt.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lb_Alt.Location = new System.Drawing.Point(58, 130);
            this.lb_Alt.Name = "lb_Alt";
            this.lb_Alt.Size = new System.Drawing.Size(76, 20);
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
            this.tb_GeoCoord.Location = new System.Drawing.Point(3, 3);
            this.tb_GeoCoord.Name = "tb_GeoCoord";
            this.tb_GeoCoord.RowCount = 3;
            this.tb_GeoCoord.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tb_GeoCoord.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tb_GeoCoord.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tb_GeoCoord.Size = new System.Drawing.Size(387, 168);
            this.tb_GeoCoord.TabIndex = 6;
            // 
            // rtb_Latitude
            // 
            this.rtb_Latitude.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_Latitude.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_Latitude.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rtb_Latitude.Location = new System.Drawing.Point(196, 3);
            this.rtb_Latitude.Multiline = false;
            this.rtb_Latitude.Name = "rtb_Latitude";
            this.rtb_Latitude.Size = new System.Drawing.Size(188, 50);
            this.rtb_Latitude.TabIndex = 3;
            this.rtb_Latitude.Text = "47.50269208677946";
            // 
            // rtb_Longitude
            // 
            this.rtb_Longitude.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_Longitude.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_Longitude.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rtb_Longitude.Location = new System.Drawing.Point(196, 59);
            this.rtb_Longitude.Multiline = false;
            this.rtb_Longitude.Name = "rtb_Longitude";
            this.rtb_Longitude.Size = new System.Drawing.Size(188, 50);
            this.rtb_Longitude.TabIndex = 4;
            this.rtb_Longitude.Text = "19.20124639140316";
            // 
            // rtb_Altitude
            // 
            this.rtb_Altitude.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_Altitude.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_Altitude.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rtb_Altitude.Location = new System.Drawing.Point(196, 115);
            this.rtb_Altitude.Multiline = false;
            this.rtb_Altitude.Name = "rtb_Altitude";
            this.rtb_Altitude.Size = new System.Drawing.Size(188, 50);
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
            this.tlp_Base.Name = "tlp_Base";
            this.tlp_Base.RowCount = 2;
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Base.Size = new System.Drawing.Size(393, 349);
            this.tlp_Base.TabIndex = 7;
            // 
            // pnl_Buttons
            // 
            this.pnl_Buttons.Controls.Add(this.button1);
            this.pnl_Buttons.Controls.Add(this.btn_Solution3);
            this.pnl_Buttons.Controls.Add(this.btn_Solution2);
            this.pnl_Buttons.Controls.Add(this.btn_Solution1);
            this.pnl_Buttons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Buttons.Location = new System.Drawing.Point(3, 177);
            this.pnl_Buttons.Name = "pnl_Buttons";
            this.pnl_Buttons.Size = new System.Drawing.Size(387, 169);
            this.pnl_Buttons.TabIndex = 7;
            // 
            // btn_Solution3
            // 
            this.btn_Solution3.Location = new System.Drawing.Point(9, 113);
            this.btn_Solution3.Name = "btn_Solution3";
            this.btn_Solution3.Size = new System.Drawing.Size(211, 49);
            this.btn_Solution3.TabIndex = 2;
            this.btn_Solution3.Text = "Go to here (Latest MP solution)";
            this.btn_Solution3.UseVisualStyleBackColor = true;
            this.btn_Solution3.Click += new System.EventHandler(this.btn_Solution3_Click);
            // 
            // btn_Solution2
            // 
            this.btn_Solution2.Location = new System.Drawing.Point(9, 58);
            this.btn_Solution2.Name = "btn_Solution2";
            this.btn_Solution2.Size = new System.Drawing.Size(211, 49);
            this.btn_Solution2.TabIndex = 1;
            this.btn_Solution2.Text = "Go to here (AGL)";
            this.btn_Solution2.UseVisualStyleBackColor = true;
            this.btn_Solution2.Click += new System.EventHandler(this.btn_Solution2_Click);
            // 
            // btn_Solution1
            // 
            this.btn_Solution1.Location = new System.Drawing.Point(9, 3);
            this.btn_Solution1.Name = "btn_Solution1";
            this.btn_Solution1.Size = new System.Drawing.Size(211, 49);
            this.btn_Solution1.TabIndex = 0;
            this.btn_Solution1.Text = "Go to here (Fly to here)";
            this.btn_Solution1.UseVisualStyleBackColor = true;
            this.btn_Solution1.Click += new System.EventHandler(this.btn_Solution1_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(226, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 49);
            this.button1.TabIndex = 3;
            this.button1.Text = "kaki";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SetWaypointForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 349);
            this.Controls.Add(this.tlp_Base);
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
    }
}