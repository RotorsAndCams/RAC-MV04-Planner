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
            this.btn_Polarity1 = new System.Windows.Forms.Button();
            this.btn_NUC = new System.Windows.Forms.Button();
            this.btn_BIT = new System.Windows.Forms.Button();
            this.btn_Polarity2 = new System.Windows.Forms.Button();
            this.pnl_DisabledControlsByDefault = new System.Windows.Forms.Panel();
            this.btn_WhiteHot = new System.Windows.Forms.Button();
            this.btn_BlackHot = new System.Windows.Forms.Button();
            this.tlp_VisibleElements = new System.Windows.Forms.TableLayoutPanel();
            this.btn_DayCamera = new System.Windows.Forms.Button();
            this.btn_Reconnect = new System.Windows.Forms.Button();
            this.btn_NightCamera = new System.Windows.Forms.Button();
            this.btn_AdvancedSettings = new System.Windows.Forms.Button();
            this.pnl_CameraSettings = new System.Windows.Forms.Panel();
            this.uc_CameraSettings = new MV04.Settings.uc_CameraSettings();
            this.pnl_DisabledControlsByDefault.SuspendLayout();
            this.tlp_VisibleElements.SuspendLayout();
            this.pnl_CameraSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Polarity1
            // 
            resources.ApplyResources(this.btn_Polarity1, "btn_Polarity1");
            this.btn_Polarity1.BackColor = System.Drawing.Color.Black;
            this.btn_Polarity1.ForeColor = System.Drawing.Color.White;
            this.btn_Polarity1.Name = "btn_Polarity1";
            this.btn_Polarity1.UseVisualStyleBackColor = false;
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
            // btn_BIT
            // 
            resources.ApplyResources(this.btn_BIT, "btn_BIT");
            this.btn_BIT.BackColor = System.Drawing.Color.Black;
            this.btn_BIT.ForeColor = System.Drawing.Color.White;
            this.btn_BIT.Name = "btn_BIT";
            this.btn_BIT.UseVisualStyleBackColor = false;
            // 
            // btn_Polarity2
            // 
            resources.ApplyResources(this.btn_Polarity2, "btn_Polarity2");
            this.btn_Polarity2.BackColor = System.Drawing.Color.Black;
            this.btn_Polarity2.ForeColor = System.Drawing.Color.White;
            this.btn_Polarity2.Name = "btn_Polarity2";
            this.btn_Polarity2.UseVisualStyleBackColor = false;
            // 
            // pnl_DisabledControlsByDefault
            // 
            this.pnl_DisabledControlsByDefault.Controls.Add(this.btn_BIT);
            this.pnl_DisabledControlsByDefault.Controls.Add(this.btn_Polarity2);
            this.pnl_DisabledControlsByDefault.Controls.Add(this.btn_WhiteHot);
            this.pnl_DisabledControlsByDefault.Controls.Add(this.btn_Polarity1);
            this.pnl_DisabledControlsByDefault.Controls.Add(this.btn_BlackHot);
            resources.ApplyResources(this.pnl_DisabledControlsByDefault, "pnl_DisabledControlsByDefault");
            this.pnl_DisabledControlsByDefault.Name = "pnl_DisabledControlsByDefault";
            // 
            // btn_WhiteHot
            // 
            resources.ApplyResources(this.btn_WhiteHot, "btn_WhiteHot");
            this.btn_WhiteHot.BackColor = System.Drawing.Color.Black;
            this.btn_WhiteHot.ForeColor = System.Drawing.Color.White;
            this.btn_WhiteHot.Image = global::MV04.Camera.Properties.Resources.istanding_man_50;
            this.btn_WhiteHot.Name = "btn_WhiteHot";
            this.btn_WhiteHot.UseVisualStyleBackColor = false;
            // 
            // btn_BlackHot
            // 
            resources.ApplyResources(this.btn_BlackHot, "btn_BlackHot");
            this.btn_BlackHot.BackColor = System.Drawing.Color.Black;
            this.btn_BlackHot.ForeColor = System.Drawing.Color.White;
            this.btn_BlackHot.Image = global::MV04.Camera.Properties.Resources.standing_man_50;
            this.btn_BlackHot.Name = "btn_BlackHot";
            this.btn_BlackHot.UseVisualStyleBackColor = false;
            // 
            // tlp_VisibleElements
            // 
            resources.ApplyResources(this.tlp_VisibleElements, "tlp_VisibleElements");
            this.tlp_VisibleElements.Controls.Add(this.btn_DayCamera, 0, 0);
            this.tlp_VisibleElements.Controls.Add(this.btn_Reconnect, 1, 1);
            this.tlp_VisibleElements.Controls.Add(this.btn_NightCamera, 1, 0);
            this.tlp_VisibleElements.Controls.Add(this.btn_NUC, 0, 1);
            this.tlp_VisibleElements.Controls.Add(this.btn_AdvancedSettings, 1, 2);
            this.tlp_VisibleElements.ForeColor = System.Drawing.Color.White;
            this.tlp_VisibleElements.Name = "tlp_VisibleElements";
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
            // btn_AdvancedSettings
            // 
            resources.ApplyResources(this.btn_AdvancedSettings, "btn_AdvancedSettings");
            this.btn_AdvancedSettings.BackColor = System.Drawing.Color.Black;
            this.btn_AdvancedSettings.ForeColor = System.Drawing.Color.White;
            this.btn_AdvancedSettings.Image = global::MV04.Camera.Properties.Resources.more_50;
            this.btn_AdvancedSettings.Name = "btn_AdvancedSettings";
            this.btn_AdvancedSettings.UseVisualStyleBackColor = false;
            this.btn_AdvancedSettings.Click += new System.EventHandler(this.btn_AdvancedSettings_Click);
            // 
            // pnl_CameraSettings
            // 
            this.pnl_CameraSettings.Controls.Add(this.uc_CameraSettings);
            resources.ApplyResources(this.pnl_CameraSettings, "pnl_CameraSettings");
            this.pnl_CameraSettings.Name = "pnl_CameraSettings";
            // 
            // uc_CameraSettings
            // 
            resources.ApplyResources(this.uc_CameraSettings, "uc_CameraSettings");
            this.uc_CameraSettings.Name = "uc_CameraSettings";
            // 
            // CameraSettingsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.pnl_CameraSettings);
            this.Controls.Add(this.pnl_DisabledControlsByDefault);
            this.Controls.Add(this.tlp_VisibleElements);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CameraSettingsForm";
            this.ShowIcon = false;
            this.pnl_DisabledControlsByDefault.ResumeLayout(false);
            this.pnl_DisabledControlsByDefault.PerformLayout();
            this.tlp_VisibleElements.ResumeLayout(false);
            this.tlp_VisibleElements.PerformLayout();
            this.pnl_CameraSettings.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_DayCamera;
        private System.Windows.Forms.Button btn_NightCamera;
        private System.Windows.Forms.Button btn_BlackHot;
        private System.Windows.Forms.Button btn_WhiteHot;
        private System.Windows.Forms.Button btn_Polarity1;
        private System.Windows.Forms.Button btn_Polarity2;
        private System.Windows.Forms.Button btn_NUC;
        private System.Windows.Forms.Button btn_BIT;
        private System.Windows.Forms.Button btn_Reconnect;
        private System.Windows.Forms.TableLayoutPanel tlp_VisibleElements;
        private System.Windows.Forms.Panel pnl_DisabledControlsByDefault;
        private System.Windows.Forms.Button btn_AdvancedSettings;
        private System.Windows.Forms.Panel pnl_CameraSettings;
        private Settings.uc_CameraSettings uc_CameraSettings;
    }
}