namespace MV04.Camera
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraSettingsForm));
            this.btn_NUC = new System.Windows.Forms.Button();
            this.tlp_Buttons = new System.Windows.Forms.TableLayoutPanel();
            this.btn_StartStopRecording = new System.Windows.Forms.Button();
            this.btn_AdvancedSettings = new System.Windows.Forms.Button();
            this.btn_DayCamera = new System.Windows.Forms.Button();
            this.btn_Reconnect = new System.Windows.Forms.Button();
            this.btn_NightCamera = new System.Windows.Forms.Button();
            this.uc_CameraSettings = new MV04.Settings.uc_CameraSettings();
            this.tlp_Base = new System.Windows.Forms.TableLayoutPanel();
            this.tlp_Buttons.SuspendLayout();
            this.tlp_Base.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_NUC
            // 
            resources.ApplyResources(this.btn_NUC, "btn_NUC");
            this.btn_NUC.BackColor = System.Drawing.Color.Black;
            this.btn_NUC.ForeColor = System.Drawing.Color.White;
            this.btn_NUC.Name = "btn_NUC";
            this.btn_NUC.UseVisualStyleBackColor = false;
            this.btn_NUC.Click += new System.EventHandler(this.btn_NUC_Click);
            // 
            // tlp_Buttons
            // 
            resources.ApplyResources(this.tlp_Buttons, "tlp_Buttons");
            this.tlp_Buttons.Controls.Add(this.btn_StartStopRecording, 0, 2);
            this.tlp_Buttons.Controls.Add(this.btn_DayCamera, 0, 0);
            this.tlp_Buttons.Controls.Add(this.btn_Reconnect, 1, 1);
            this.tlp_Buttons.Controls.Add(this.btn_NightCamera, 1, 0);
            this.tlp_Buttons.Controls.Add(this.btn_NUC, 0, 1);
            this.tlp_Buttons.Controls.Add(this.btn_AdvancedSettings, 1, 5);
            this.tlp_Buttons.ForeColor = System.Drawing.Color.White;
            this.tlp_Buttons.Name = "tlp_Buttons";
            // 
            // btn_StartStopRecording
            // 
            resources.ApplyResources(this.btn_StartStopRecording, "btn_StartStopRecording");
            this.btn_StartStopRecording.BackColor = System.Drawing.Color.Black;
            this.btn_StartStopRecording.ForeColor = System.Drawing.Color.White;
            this.btn_StartStopRecording.Name = "btn_StartStopRecording";
            this.btn_StartStopRecording.UseVisualStyleBackColor = false;
            this.btn_StartStopRecording.Click += new System.EventHandler(this.btn_StartStopRecording_Click);
            // 
            // btn_AdvancedSettings
            // 
            this.btn_AdvancedSettings.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btn_AdvancedSettings, "btn_AdvancedSettings");
            this.btn_AdvancedSettings.ForeColor = System.Drawing.Color.White;
            this.btn_AdvancedSettings.Image = global::MV04.Camera.Properties.Resources.more_50;
            this.btn_AdvancedSettings.Name = "btn_AdvancedSettings";
            this.btn_AdvancedSettings.UseVisualStyleBackColor = false;
            // 
            // btn_DayCamera
            // 
            resources.ApplyResources(this.btn_DayCamera, "btn_DayCamera");
            this.btn_DayCamera.BackColor = System.Drawing.Color.Black;
            this.btn_DayCamera.ForeColor = System.Drawing.Color.White;
            this.btn_DayCamera.Image = global::MV04.Camera.Properties.Resources.day_camera_50;
            this.btn_DayCamera.Name = "btn_DayCamera";
            this.btn_DayCamera.UseVisualStyleBackColor = false;
            this.btn_DayCamera.Click += new System.EventHandler(this.btn_DayCamera_Click);
            // 
            // btn_Reconnect
            // 
            resources.ApplyResources(this.btn_Reconnect, "btn_Reconnect");
            this.btn_Reconnect.BackColor = System.Drawing.Color.Black;
            this.btn_Reconnect.ForeColor = System.Drawing.Color.White;
            this.btn_Reconnect.Image = global::MV04.Camera.Properties.Resources.connect_50;
            this.btn_Reconnect.Name = "btn_Reconnect";
            this.btn_Reconnect.UseVisualStyleBackColor = false;
            this.btn_Reconnect.Click += new System.EventHandler(this.btn_Reconnect_Click);
            // 
            // btn_NightCamera
            // 
            resources.ApplyResources(this.btn_NightCamera, "btn_NightCamera");
            this.btn_NightCamera.BackColor = System.Drawing.Color.Black;
            this.btn_NightCamera.ForeColor = System.Drawing.Color.White;
            this.btn_NightCamera.Image = global::MV04.Camera.Properties.Resources.night_camera_50;
            this.btn_NightCamera.Name = "btn_NightCamera";
            this.btn_NightCamera.UseVisualStyleBackColor = false;
            this.btn_NightCamera.Click += new System.EventHandler(this.btn_NightCamera_Click);
            // 
            // uc_CameraSettings
            // 
            resources.ApplyResources(this.uc_CameraSettings, "uc_CameraSettings");
            this.uc_CameraSettings.Name = "uc_CameraSettings";
            // 
            // tlp_Base
            // 
            resources.ApplyResources(this.tlp_Base, "tlp_Base");
            this.tlp_Base.Controls.Add(this.uc_CameraSettings, 1, 0);
            this.tlp_Base.Controls.Add(this.tlp_Buttons, 0, 0);
            this.tlp_Base.Name = "tlp_Base";
            // 
            // CameraSettingsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
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
        private System.Windows.Forms.Button btn_AdvancedSettings;
        private Settings.uc_CameraSettings uc_CameraSettings;
        private System.Windows.Forms.Button btn_StartStopRecording;
        private System.Windows.Forms.TableLayoutPanel tlp_Base;
    }
}