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
            this.btn_AdvancedSettings = new System.Windows.Forms.Button();
            this.tlp_Base = new System.Windows.Forms.TableLayoutPanel();
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
            this.btn_NUC.Location = new System.Drawing.Point(3, 91);
            this.btn_NUC.Name = "btn_NUC";
            this.btn_NUC.Size = new System.Drawing.Size(142, 82);
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
            this.tlp_Buttons.Controls.Add(this.btn_AdvancedSettings, 1, 5);
            this.tlp_Buttons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_Buttons.ForeColor = System.Drawing.Color.White;
            this.tlp_Buttons.Location = new System.Drawing.Point(3, 3);
            this.tlp_Buttons.Name = "tlp_Buttons";
            this.tlp_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlp_Buttons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_Buttons.Size = new System.Drawing.Size(296, 460);
            this.tlp_Buttons.TabIndex = 1;
            // 
            // btn_StartStopRecording
            // 
            this.btn_StartStopRecording.BackColor = System.Drawing.Color.Black;
            this.btn_StartStopRecording.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_StartStopRecording.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_StartStopRecording.ForeColor = System.Drawing.Color.White;
            this.btn_StartStopRecording.Location = new System.Drawing.Point(3, 179);
            this.btn_StartStopRecording.Name = "btn_StartStopRecording";
            this.btn_StartStopRecording.Size = new System.Drawing.Size(142, 82);
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
            this.btn_DayCamera.Size = new System.Drawing.Size(142, 82);
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
            this.btn_Reconnect.Location = new System.Drawing.Point(151, 91);
            this.btn_Reconnect.Name = "btn_Reconnect";
            this.btn_Reconnect.Size = new System.Drawing.Size(142, 82);
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
            this.btn_NightCamera.Size = new System.Drawing.Size(142, 82);
            this.btn_NightCamera.TabIndex = 3;
            this.btn_NightCamera.Text = "Night camera";
            this.btn_NightCamera.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn_NightCamera.UseVisualStyleBackColor = false;
            this.btn_NightCamera.Click += new System.EventHandler(this.btn_NightCamera_Click);
            // 
            // btn_AdvancedSettings
            // 
            this.btn_AdvancedSettings.BackColor = System.Drawing.Color.Black;
            this.btn_AdvancedSettings.BackgroundImage = global::MissionPlanner.Properties.Resources.more_50;
            this.btn_AdvancedSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_AdvancedSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_AdvancedSettings.ForeColor = System.Drawing.Color.White;
            this.btn_AdvancedSettings.Location = new System.Drawing.Point(151, 375);
            this.btn_AdvancedSettings.Name = "btn_AdvancedSettings";
            this.btn_AdvancedSettings.Size = new System.Drawing.Size(142, 82);
            this.btn_AdvancedSettings.TabIndex = 5;
            this.btn_AdvancedSettings.UseVisualStyleBackColor = false;
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
            this.tlp_Base.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_DayCamera;
        private System.Windows.Forms.Button btn_NightCamera;
        private System.Windows.Forms.Button btn_NUC;
        private System.Windows.Forms.Button btn_Reconnect;
        private System.Windows.Forms.TableLayoutPanel tlp_Buttons;
        private System.Windows.Forms.Button btn_AdvancedSettings;
        private uc_CameraSettings uc_CameraSettings;
        private System.Windows.Forms.Button btn_StartStopRecording;
        private System.Windows.Forms.TableLayoutPanel tlp_Base;
    }
}