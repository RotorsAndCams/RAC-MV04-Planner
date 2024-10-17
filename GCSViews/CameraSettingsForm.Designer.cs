namespace MissionPlanner.GCSViews
{
    partial class CameraSettingsForm
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
            this.btn_NUC = new System.Windows.Forms.Button();
            this.tlp_Buttons = new System.Windows.Forms.TableLayoutPanel();
            this.btn_StartStopRecording = new System.Windows.Forms.Button();
            this.btn_DayCamera = new System.Windows.Forms.Button();
            this.btn_Reconnect = new System.Windows.Forms.Button();
            this.btn_NightCamera = new System.Windows.Forms.Button();
            this.tlp_Base = new System.Windows.Forms.TableLayoutPanel();
            this.btn_EmergencyStop = new System.Windows.Forms.Button();
            this.lb_StopCounter = new System.Windows.Forms.Label();
            this.uc_CameraSettings = new MissionPlanner.GCSViews.uc_CameraSettings();
            this.tlp_Buttons.SuspendLayout();
            this.tlp_Base.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_NUC
            // 
            this.btn_NUC.BackColor = System.Drawing.Color.Black;
            this.btn_NUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_NUC.ForeColor = System.Drawing.Color.White;
            this.btn_NUC.Location = new System.Drawing.Point(3, 95);
            this.btn_NUC.Name = "btn_NUC";
            this.btn_NUC.Size = new System.Drawing.Size(142, 86);
            this.btn_NUC.TabIndex = 4;
            this.btn_NUC.Text = "NUC";
            this.btn_NUC.UseVisualStyleBackColor = false;
            this.btn_NUC.Click += new System.EventHandler(this.btn_NUC_Click);
            // 
            // tlp_Buttons
            // 
            this.tlp_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Buttons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Buttons.Controls.Add(this.btn_StartStopRecording, 0, 2);
            this.tlp_Buttons.Controls.Add(this.btn_DayCamera, 0, 0);
            this.tlp_Buttons.Controls.Add(this.btn_Reconnect, 1, 1);
            this.tlp_Buttons.Controls.Add(this.btn_NightCamera, 1, 0);
            this.tlp_Buttons.Controls.Add(this.btn_NUC, 0, 1);
            this.tlp_Buttons.Controls.Add(this.btn_EmergencyStop, 1, 4);
            this.tlp_Buttons.Controls.Add(this.lb_StopCounter, 1, 3);
            this.tlp_Buttons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_Buttons.ForeColor = System.Drawing.Color.White;
            this.tlp_Buttons.Location = new System.Drawing.Point(3, 3);
            this.tlp_Buttons.Name = "tlp_Buttons";
            this.tlp_Buttons.RowCount = 5;
            this.tlp_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.08032F));
            this.tlp_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.08032F));
            this.tlp_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.08032F));
            this.tlp_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.87952F));
            this.tlp_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.87952F));
            this.tlp_Buttons.Size = new System.Drawing.Size(296, 460);
            this.tlp_Buttons.TabIndex = 1;
            // 
            // btn_StartStopRecording
            // 
            this.btn_StartStopRecording.BackColor = System.Drawing.Color.Black;
            this.btn_StartStopRecording.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_StartStopRecording.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_StartStopRecording.ForeColor = System.Drawing.Color.White;
            this.btn_StartStopRecording.Location = new System.Drawing.Point(3, 187);
            this.btn_StartStopRecording.Name = "btn_StartStopRecording";
            this.btn_StartStopRecording.Size = new System.Drawing.Size(142, 86);
            this.btn_StartStopRecording.TabIndex = 0;
            this.btn_StartStopRecording.Text = "Stop Recording";
            this.btn_StartStopRecording.UseVisualStyleBackColor = false;
            this.btn_StartStopRecording.Click += new System.EventHandler(this.btn_StartStopRecording_Click);
            // 
            // btn_DayCamera
            // 
            this.btn_DayCamera.BackColor = System.Drawing.Color.Black;
            this.btn_DayCamera.BackgroundImage = global::MissionPlanner.Properties.Resources.day_camera_50;
            this.btn_DayCamera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_DayCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_DayCamera.ForeColor = System.Drawing.Color.White;
            this.btn_DayCamera.Location = new System.Drawing.Point(3, 3);
            this.btn_DayCamera.Name = "btn_DayCamera";
            this.btn_DayCamera.Size = new System.Drawing.Size(142, 86);
            this.btn_DayCamera.TabIndex = 1;
            this.btn_DayCamera.Text = "Day camera";
            this.btn_DayCamera.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_DayCamera.UseVisualStyleBackColor = false;
            this.btn_DayCamera.Click += new System.EventHandler(this.btn_DayCamera_Click);
            // 
            // btn_Reconnect
            // 
            this.btn_Reconnect.BackColor = System.Drawing.Color.Black;
            this.btn_Reconnect.BackgroundImage = global::MissionPlanner.Properties.Resources.connect_50;
            this.btn_Reconnect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_Reconnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Reconnect.ForeColor = System.Drawing.Color.White;
            this.btn_Reconnect.Location = new System.Drawing.Point(151, 95);
            this.btn_Reconnect.Name = "btn_Reconnect";
            this.btn_Reconnect.Size = new System.Drawing.Size(142, 86);
            this.btn_Reconnect.TabIndex = 2;
            this.btn_Reconnect.Text = "Camera Reconnect";
            this.btn_Reconnect.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_Reconnect.UseVisualStyleBackColor = false;
            this.btn_Reconnect.Click += new System.EventHandler(this.btn_Reconnect_Click);
            // 
            // btn_NightCamera
            // 
            this.btn_NightCamera.BackColor = System.Drawing.Color.Black;
            this.btn_NightCamera.BackgroundImage = global::MissionPlanner.Properties.Resources.night_camera_50;
            this.btn_NightCamera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_NightCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_NightCamera.ForeColor = System.Drawing.Color.White;
            this.btn_NightCamera.Location = new System.Drawing.Point(151, 3);
            this.btn_NightCamera.Name = "btn_NightCamera";
            this.btn_NightCamera.Size = new System.Drawing.Size(142, 86);
            this.btn_NightCamera.TabIndex = 3;
            this.btn_NightCamera.Text = "Night camera";
            this.btn_NightCamera.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_NightCamera.UseVisualStyleBackColor = false;
            this.btn_NightCamera.Click += new System.EventHandler(this.btn_NightCamera_Click);
            // 
            // tlp_Base
            // 
            this.tlp_Base.ColumnCount = 2;
            this.tlp_Base.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Base.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Base.Controls.Add(this.uc_CameraSettings, 1, 0);
            this.tlp_Base.Controls.Add(this.tlp_Buttons, 0, 0);
            this.tlp_Base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_Base.Location = new System.Drawing.Point(0, 0);
            this.tlp_Base.Name = "tlp_Base";
            this.tlp_Base.RowCount = 1;
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Base.Size = new System.Drawing.Size(605, 466);
            this.tlp_Base.TabIndex = 2;
            // 
            // btn_EmergencyStop
            // 
            this.btn_EmergencyStop.BackColor = System.Drawing.Color.Black;
            this.btn_EmergencyStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_EmergencyStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_EmergencyStop.ForeColor = System.Drawing.Color.White;
            this.btn_EmergencyStop.Location = new System.Drawing.Point(151, 370);
            this.btn_EmergencyStop.Name = "btn_EmergencyStop";
            this.btn_EmergencyStop.Size = new System.Drawing.Size(142, 87);
            this.btn_EmergencyStop.TabIndex = 7;
            this.btn_EmergencyStop.Text = "Emergency Stop";
            this.btn_EmergencyStop.UseVisualStyleBackColor = false;
            this.btn_EmergencyStop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_EmergencyStop_MouseDown);
            this.btn_EmergencyStop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_EmergencyStop_MouseUp);
            // 
            // lb_StopCounter
            // 
            this.lb_StopCounter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lb_StopCounter.AutoSize = true;
            this.lb_StopCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lb_StopCounter.ForeColor = System.Drawing.Color.Red;
            this.lb_StopCounter.Location = new System.Drawing.Point(173, 311);
            this.lb_StopCounter.Name = "lb_StopCounter";
            this.lb_StopCounter.Size = new System.Drawing.Size(98, 20);
            this.lb_StopCounter.TabIndex = 8;
            this.lb_StopCounter.Text = "Motor stop 3";
            this.lb_StopCounter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lb_StopCounter.Visible = false;
            // 
            // uc_CameraSettings
            // 
            this.uc_CameraSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uc_CameraSettings.Location = new System.Drawing.Point(305, 3);
            this.uc_CameraSettings.Name = "uc_CameraSettings";
            this.uc_CameraSettings.Size = new System.Drawing.Size(297, 460);
            this.uc_CameraSettings.TabIndex = 0;
            // 
            // CameraSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(605, 466);
            this.Controls.Add(this.tlp_Base);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CameraSettingsForm";
            this.ShowIcon = false;
            this.tlp_Buttons.ResumeLayout(false);
            this.tlp_Buttons.PerformLayout();
            this.tlp_Base.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_DayCamera;
        private System.Windows.Forms.Button btn_NightCamera;
        private System.Windows.Forms.Button btn_NUC;
        private System.Windows.Forms.Button btn_Reconnect;
        private System.Windows.Forms.TableLayoutPanel tlp_Buttons;
        private uc_CameraSettings uc_CameraSettings;
        private System.Windows.Forms.Button btn_StartStopRecording;
        private System.Windows.Forms.TableLayoutPanel tlp_Base;
        private System.Windows.Forms.Button btn_EmergencyStop;
        private System.Windows.Forms.Label lb_StopCounter;
    }
}