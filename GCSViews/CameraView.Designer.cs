namespace MissionPlanner.GCSViews
{
    partial class CameraView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraView));
            this.tlp_ControlsBase = new System.Windows.Forms.TableLayoutPanel();
            this.tlp_Buttonsection2 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_StopTracking = new System.Windows.Forms.Button();
            this.btn_FPVCameraMode = new System.Windows.Forms.Button();
            this.btn_Surveillance = new System.Windows.Forms.Button();
            this.btn_NUC = new System.Windows.Forms.Button();
            this.btn_ResetZoom = new System.Windows.Forms.Button();
            this.tlp_Zoom = new System.Windows.Forms.TableLayoutPanel();
            this.btn_ZoomMinus = new System.Windows.Forms.Button();
            this.btn_ZoomPlus = new System.Windows.Forms.Button();
            this.tlp_InfoBase = new System.Windows.Forms.TableLayoutPanel();
            this.pnl_SettingsPanel = new System.Windows.Forms.Panel();
            this.btn_Settings = new System.Windows.Forms.Button();
            this.pnl_DeviceState = new System.Windows.Forms.Panel();
            this.tlp_StatusUpper = new System.Windows.Forms.TableLayoutPanel();
            this.tlp_DeviceStatusInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lb_DroneStatusValue = new System.Windows.Forms.Label();
            this.lb_GCSSelectedStateValue = new System.Windows.Forms.Label();
            this.pnl_GCS = new System.Windows.Forms.Panel();
            this.lb_GCSSelectedState = new System.Windows.Forms.Label();
            this.pnl_Drone = new System.Windows.Forms.Panel();
            this.lb_Drone = new System.Windows.Forms.Label();
            this.pnl_Camera = new System.Windows.Forms.Panel();
            this.lb_Camera = new System.Windows.Forms.Label();
            this.lb_CameraStatusValue = new System.Windows.Forms.Label();
            this.tlp_SensorLights = new System.Windows.Forms.TableLayoutPanel();
            this.pb_DroneTakeOff = new System.Windows.Forms.PictureBox();
            this.pb_InfraLight = new System.Windows.Forms.PictureBox();
            this.pb_PositionIndicator = new System.Windows.Forms.PictureBox();
            this.tlp_ButtonSection1 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_TripSwitchOnOff = new System.Windows.Forms.Button();
            this.btn_Polarity = new System.Windows.Forms.Button();
            this.btn_ChangeCrosshair = new System.Windows.Forms.Button();
            this.btn_FullScreen = new System.Windows.Forms.Button();
            this.tlp_CVBase = new System.Windows.Forms.TableLayoutPanel();
            this.vv_VLC = new LibVLCSharp.WinForms.VideoView();
            this.tlp_ControlsBase.SuspendLayout();
            this.tlp_Buttonsection2.SuspendLayout();
            this.tlp_Zoom.SuspendLayout();
            this.tlp_InfoBase.SuspendLayout();
            this.pnl_SettingsPanel.SuspendLayout();
            this.pnl_DeviceState.SuspendLayout();
            this.tlp_StatusUpper.SuspendLayout();
            this.tlp_DeviceStatusInfo.SuspendLayout();
            this.pnl_GCS.SuspendLayout();
            this.pnl_Drone.SuspendLayout();
            this.pnl_Camera.SuspendLayout();
            this.tlp_SensorLights.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_DroneTakeOff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_InfraLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_PositionIndicator)).BeginInit();
            this.tlp_ButtonSection1.SuspendLayout();
            this.tlp_CVBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vv_VLC)).BeginInit();
            this.SuspendLayout();
            // 
            // tlp_ControlsBase
            // 
            resources.ApplyResources(this.tlp_ControlsBase, "tlp_ControlsBase");
            this.tlp_ControlsBase.Controls.Add(this.tlp_Buttonsection2, 0, 2);
            this.tlp_ControlsBase.Controls.Add(this.tlp_InfoBase, 0, 0);
            this.tlp_ControlsBase.Controls.Add(this.tlp_ButtonSection1, 0, 1);
            this.tlp_ControlsBase.Name = "tlp_ControlsBase";
            // 
            // tlp_Buttonsection2
            // 
            resources.ApplyResources(this.tlp_Buttonsection2, "tlp_Buttonsection2");
            this.tlp_Buttonsection2.Controls.Add(this.btn_StopTracking, 0, 2);
            this.tlp_Buttonsection2.Controls.Add(this.btn_FPVCameraMode, 0, 0);
            this.tlp_Buttonsection2.Controls.Add(this.btn_Surveillance, 1, 0);
            this.tlp_Buttonsection2.Controls.Add(this.btn_NUC, 1, 2);
            this.tlp_Buttonsection2.Controls.Add(this.btn_ResetZoom, 0, 1);
            this.tlp_Buttonsection2.Controls.Add(this.tlp_Zoom, 1, 1);
            this.tlp_Buttonsection2.Name = "tlp_Buttonsection2";
            // 
            // btn_StopTracking
            // 
            this.btn_StopTracking.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btn_StopTracking, "btn_StopTracking");
            this.btn_StopTracking.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_StopTracking.Image = global::MissionPlanner.Properties.Resources.icons8_stop_sign_50;
            this.btn_StopTracking.Name = "btn_StopTracking";
            this.btn_StopTracking.UseVisualStyleBackColor = false;
            this.btn_StopTracking.Click += new System.EventHandler(this.btn_StopTracking_Click);
            // 
            // btn_FPVCameraMode
            // 
            this.btn_FPVCameraMode.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btn_FPVCameraMode, "btn_FPVCameraMode");
            this.btn_FPVCameraMode.ForeColor = System.Drawing.Color.White;
            this.btn_FPVCameraMode.Name = "btn_FPVCameraMode";
            this.btn_FPVCameraMode.UseVisualStyleBackColor = false;
            this.btn_FPVCameraMode.Click += new System.EventHandler(this.btn_FPVCameraMode_Click);
            // 
            // btn_Surveillance
            // 
            this.btn_Surveillance.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btn_Surveillance, "btn_Surveillance");
            this.btn_Surveillance.ForeColor = System.Drawing.Color.White;
            this.btn_Surveillance.Name = "btn_Surveillance";
            this.btn_Surveillance.UseVisualStyleBackColor = false;
            this.btn_Surveillance.Click += new System.EventHandler(this.btn_Surveillance_Click);
            // 
            // btn_NUC
            // 
            this.btn_NUC.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btn_NUC, "btn_NUC");
            this.btn_NUC.ForeColor = System.Drawing.Color.White;
            this.btn_NUC.Name = "btn_NUC";
            this.btn_NUC.UseVisualStyleBackColor = false;
            this.btn_NUC.Click += new System.EventHandler(this.btn_NUC_Click);
            // 
            // btn_ResetZoom
            // 
            this.btn_ResetZoom.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btn_ResetZoom, "btn_ResetZoom");
            this.btn_ResetZoom.ForeColor = System.Drawing.Color.White;
            this.btn_ResetZoom.Image = global::MissionPlanner.Properties.Resources.icons8_reset_50;
            this.btn_ResetZoom.Name = "btn_ResetZoom";
            this.btn_ResetZoom.UseVisualStyleBackColor = false;
            this.btn_ResetZoom.Click += new System.EventHandler(this.btn_ResetZoom_Click);
            // 
            // tlp_Zoom
            // 
            resources.ApplyResources(this.tlp_Zoom, "tlp_Zoom");
            this.tlp_Zoom.Controls.Add(this.btn_ZoomMinus, 0, 1);
            this.tlp_Zoom.Controls.Add(this.btn_ZoomPlus, 0, 0);
            this.tlp_Zoom.Name = "tlp_Zoom";
            // 
            // btn_ZoomMinus
            // 
            this.btn_ZoomMinus.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btn_ZoomMinus, "btn_ZoomMinus");
            this.btn_ZoomMinus.ForeColor = System.Drawing.Color.White;
            this.btn_ZoomMinus.Name = "btn_ZoomMinus";
            this.btn_ZoomMinus.UseVisualStyleBackColor = false;
            this.btn_ZoomMinus.Click += new System.EventHandler(this.btn_ZoomMinus_Click);
            // 
            // btn_ZoomPlus
            // 
            this.btn_ZoomPlus.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btn_ZoomPlus, "btn_ZoomPlus");
            this.btn_ZoomPlus.ForeColor = System.Drawing.Color.White;
            this.btn_ZoomPlus.Name = "btn_ZoomPlus";
            this.btn_ZoomPlus.UseVisualStyleBackColor = false;
            this.btn_ZoomPlus.Click += new System.EventHandler(this.btn_ZoomPlus_Click);
            // 
            // tlp_InfoBase
            // 
            resources.ApplyResources(this.tlp_InfoBase, "tlp_InfoBase");
            this.tlp_InfoBase.Controls.Add(this.pnl_SettingsPanel, 1, 0);
            this.tlp_InfoBase.Controls.Add(this.pnl_DeviceState, 0, 0);
            this.tlp_InfoBase.Name = "tlp_InfoBase";
            // 
            // pnl_SettingsPanel
            // 
            this.pnl_SettingsPanel.Controls.Add(this.btn_Settings);
            resources.ApplyResources(this.pnl_SettingsPanel, "pnl_SettingsPanel");
            this.pnl_SettingsPanel.Name = "pnl_SettingsPanel";
            // 
            // btn_Settings
            // 
            this.btn_Settings.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btn_Settings, "btn_Settings");
            this.btn_Settings.ForeColor = System.Drawing.Color.Black;
            this.btn_Settings.Image = global::MissionPlanner.Properties.Resources.icons8_settings_100;
            this.btn_Settings.Name = "btn_Settings";
            this.btn_Settings.UseVisualStyleBackColor = false;
            this.btn_Settings.Click += new System.EventHandler(this.btn_Settings_Click);
            // 
            // pnl_DeviceState
            // 
            this.pnl_DeviceState.Controls.Add(this.tlp_StatusUpper);
            resources.ApplyResources(this.pnl_DeviceState, "pnl_DeviceState");
            this.pnl_DeviceState.Name = "pnl_DeviceState";
            // 
            // tlp_StatusUpper
            // 
            resources.ApplyResources(this.tlp_StatusUpper, "tlp_StatusUpper");
            this.tlp_StatusUpper.Controls.Add(this.tlp_DeviceStatusInfo, 0, 0);
            this.tlp_StatusUpper.Controls.Add(this.tlp_SensorLights, 0, 1);
            this.tlp_StatusUpper.Name = "tlp_StatusUpper";
            // 
            // tlp_DeviceStatusInfo
            // 
            this.tlp_DeviceStatusInfo.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.tlp_DeviceStatusInfo, "tlp_DeviceStatusInfo");
            this.tlp_DeviceStatusInfo.Controls.Add(this.lb_DroneStatusValue, 1, 0);
            this.tlp_DeviceStatusInfo.Controls.Add(this.lb_GCSSelectedStateValue, 1, 2);
            this.tlp_DeviceStatusInfo.Controls.Add(this.pnl_GCS, 0, 2);
            this.tlp_DeviceStatusInfo.Controls.Add(this.pnl_Drone, 0, 0);
            this.tlp_DeviceStatusInfo.Controls.Add(this.pnl_Camera, 0, 1);
            this.tlp_DeviceStatusInfo.Controls.Add(this.lb_CameraStatusValue, 1, 1);
            this.tlp_DeviceStatusInfo.Name = "tlp_DeviceStatusInfo";
            // 
            // lb_DroneStatusValue
            // 
            resources.ApplyResources(this.lb_DroneStatusValue, "lb_DroneStatusValue");
            this.lb_DroneStatusValue.ForeColor = System.Drawing.Color.White;
            this.lb_DroneStatusValue.Name = "lb_DroneStatusValue";
            // 
            // lb_GCSSelectedStateValue
            // 
            resources.ApplyResources(this.lb_GCSSelectedStateValue, "lb_GCSSelectedStateValue");
            this.lb_GCSSelectedStateValue.ForeColor = System.Drawing.Color.White;
            this.lb_GCSSelectedStateValue.Name = "lb_GCSSelectedStateValue";
            // 
            // pnl_GCS
            // 
            this.pnl_GCS.Controls.Add(this.lb_GCSSelectedState);
            resources.ApplyResources(this.pnl_GCS, "pnl_GCS");
            this.pnl_GCS.Name = "pnl_GCS";
            // 
            // lb_GCSSelectedState
            // 
            resources.ApplyResources(this.lb_GCSSelectedState, "lb_GCSSelectedState");
            this.lb_GCSSelectedState.ForeColor = System.Drawing.Color.White;
            this.lb_GCSSelectedState.Name = "lb_GCSSelectedState";
            // 
            // pnl_Drone
            // 
            this.pnl_Drone.Controls.Add(this.lb_Drone);
            resources.ApplyResources(this.pnl_Drone, "pnl_Drone");
            this.pnl_Drone.Name = "pnl_Drone";
            // 
            // lb_Drone
            // 
            resources.ApplyResources(this.lb_Drone, "lb_Drone");
            this.lb_Drone.ForeColor = System.Drawing.Color.White;
            this.lb_Drone.Name = "lb_Drone";
            // 
            // pnl_Camera
            // 
            this.pnl_Camera.Controls.Add(this.lb_Camera);
            resources.ApplyResources(this.pnl_Camera, "pnl_Camera");
            this.pnl_Camera.Name = "pnl_Camera";
            // 
            // lb_Camera
            // 
            resources.ApplyResources(this.lb_Camera, "lb_Camera");
            this.lb_Camera.ForeColor = System.Drawing.Color.White;
            this.lb_Camera.Name = "lb_Camera";
            // 
            // lb_CameraStatusValue
            // 
            resources.ApplyResources(this.lb_CameraStatusValue, "lb_CameraStatusValue");
            this.lb_CameraStatusValue.ForeColor = System.Drawing.Color.White;
            this.lb_CameraStatusValue.Name = "lb_CameraStatusValue";
            // 
            // tlp_SensorLights
            // 
            this.tlp_SensorLights.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.tlp_SensorLights, "tlp_SensorLights");
            this.tlp_SensorLights.Controls.Add(this.pb_DroneTakeOff, 2, 0);
            this.tlp_SensorLights.Controls.Add(this.pb_InfraLight, 1, 0);
            this.tlp_SensorLights.Controls.Add(this.pb_PositionIndicator, 0, 0);
            this.tlp_SensorLights.Name = "tlp_SensorLights";
            // 
            // pb_DroneTakeOff
            // 
            resources.ApplyResources(this.pb_DroneTakeOff, "pb_DroneTakeOff");
            this.pb_DroneTakeOff.Image = global::MissionPlanner.Properties.Resources.drone_takeoff_;
            this.pb_DroneTakeOff.Name = "pb_DroneTakeOff";
            this.pb_DroneTakeOff.TabStop = false;
            // 
            // pb_InfraLight
            // 
            resources.ApplyResources(this.pb_InfraLight, "pb_InfraLight");
            this.pb_InfraLight.Image = global::MissionPlanner.Properties.Resources.motion_sensor_30;
            this.pb_InfraLight.Name = "pb_InfraLight";
            this.pb_InfraLight.TabStop = false;
            // 
            // pb_PositionIndicator
            // 
            resources.ApplyResources(this.pb_PositionIndicator, "pb_PositionIndicator");
            this.pb_PositionIndicator.Image = global::MissionPlanner.Properties.Resources.headlight_30;
            this.pb_PositionIndicator.Name = "pb_PositionIndicator";
            this.pb_PositionIndicator.TabStop = false;
            // 
            // tlp_ButtonSection1
            // 
            resources.ApplyResources(this.tlp_ButtonSection1, "tlp_ButtonSection1");
            this.tlp_ButtonSection1.Controls.Add(this.btn_TripSwitchOnOff, 0, 0);
            this.tlp_ButtonSection1.Controls.Add(this.btn_Polarity, 0, 1);
            this.tlp_ButtonSection1.Controls.Add(this.btn_ChangeCrosshair, 1, 0);
            this.tlp_ButtonSection1.Controls.Add(this.btn_FullScreen, 1, 1);
            this.tlp_ButtonSection1.Name = "tlp_ButtonSection1";
            // 
            // btn_TripSwitchOnOff
            // 
            this.btn_TripSwitchOnOff.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btn_TripSwitchOnOff, "btn_TripSwitchOnOff");
            this.btn_TripSwitchOnOff.ForeColor = System.Drawing.Color.White;
            this.btn_TripSwitchOnOff.Image = global::MissionPlanner.Properties.Resources.icons8_power_64_inv;
            this.btn_TripSwitchOnOff.Name = "btn_TripSwitchOnOff";
            this.btn_TripSwitchOnOff.UseVisualStyleBackColor = false;
            this.btn_TripSwitchOnOff.Click += new System.EventHandler(this.btn_TripSwitchOnOff_Click);
            // 
            // btn_Polarity
            // 
            this.btn_Polarity.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btn_Polarity, "btn_Polarity");
            this.btn_Polarity.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_Polarity.Image = global::MissionPlanner.Properties.Resources.icons8_invert_50;
            this.btn_Polarity.Name = "btn_Polarity";
            this.btn_Polarity.UseVisualStyleBackColor = false;
            this.btn_Polarity.Click += new System.EventHandler(this.btn_Polarity_Click);
            // 
            // btn_ChangeCrosshair
            // 
            this.btn_ChangeCrosshair.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btn_ChangeCrosshair, "btn_ChangeCrosshair");
            this.btn_ChangeCrosshair.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_ChangeCrosshair.Image = global::MissionPlanner.Properties.Resources.icons8_crosshair_50;
            this.btn_ChangeCrosshair.Name = "btn_ChangeCrosshair";
            this.btn_ChangeCrosshair.UseVisualStyleBackColor = false;
            this.btn_ChangeCrosshair.Click += new System.EventHandler(this.btn_ChangeCrosshair_Click);
            // 
            // btn_FullScreen
            // 
            this.btn_FullScreen.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btn_FullScreen, "btn_FullScreen");
            this.btn_FullScreen.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_FullScreen.Image = global::MissionPlanner.Properties.Resources.icons8_fullscreen_50;
            this.btn_FullScreen.Name = "btn_FullScreen";
            this.btn_FullScreen.UseVisualStyleBackColor = false;
            this.btn_FullScreen.Click += new System.EventHandler(this.btn_FullScreen_Click);
            // 
            // tlp_CVBase
            // 
            resources.ApplyResources(this.tlp_CVBase, "tlp_CVBase");
            this.tlp_CVBase.Controls.Add(this.tlp_ControlsBase, 1, 0);
            this.tlp_CVBase.Controls.Add(this.vv_VLC, 0, 0);
            this.tlp_CVBase.Name = "tlp_CVBase";
            // 
            // vv_VLC
            // 
            this.vv_VLC.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.vv_VLC, "vv_VLC");
            this.vv_VLC.ForeColor = System.Drawing.Color.Black;
            this.vv_VLC.MediaPlayer = null;
            this.vv_VLC.Name = "vv_VLC";
            // 
            // CameraView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.tlp_CVBase);
            this.Name = "CameraView";
            this.tlp_ControlsBase.ResumeLayout(false);
            this.tlp_Buttonsection2.ResumeLayout(false);
            this.tlp_Zoom.ResumeLayout(false);
            this.tlp_InfoBase.ResumeLayout(false);
            this.pnl_SettingsPanel.ResumeLayout(false);
            this.pnl_DeviceState.ResumeLayout(false);
            this.tlp_StatusUpper.ResumeLayout(false);
            this.tlp_DeviceStatusInfo.ResumeLayout(false);
            this.tlp_DeviceStatusInfo.PerformLayout();
            this.pnl_GCS.ResumeLayout(false);
            this.pnl_GCS.PerformLayout();
            this.pnl_Drone.ResumeLayout(false);
            this.pnl_Drone.PerformLayout();
            this.pnl_Camera.ResumeLayout(false);
            this.pnl_Camera.PerformLayout();
            this.tlp_SensorLights.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_DroneTakeOff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_InfraLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_PositionIndicator)).EndInit();
            this.tlp_ButtonSection1.ResumeLayout(false);
            this.tlp_CVBase.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vv_VLC)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tlp_ControlsBase;
        private System.Windows.Forms.TableLayoutPanel tlp_Buttonsection2;
        private System.Windows.Forms.Button btn_ResetZoom;
        private System.Windows.Forms.TableLayoutPanel tlp_ButtonSection1;
        private System.Windows.Forms.Button btn_TripSwitchOnOff;
        private System.Windows.Forms.Button btn_ChangeCrosshair;
        private System.Windows.Forms.Button btn_FullScreen;
        private System.Windows.Forms.Button btn_FPVCameraMode;
        private System.Windows.Forms.TableLayoutPanel tlp_InfoBase;
        private System.Windows.Forms.Panel pnl_SettingsPanel;
        private System.Windows.Forms.Button btn_Settings;
        private System.Windows.Forms.Panel pnl_DeviceState;
        private System.Windows.Forms.TableLayoutPanel tlp_StatusUpper;
        private System.Windows.Forms.TableLayoutPanel tlp_DeviceStatusInfo;
        private System.Windows.Forms.Label lb_DroneStatusValue;
        private System.Windows.Forms.Label lb_GCSSelectedStateValue;
        private System.Windows.Forms.Panel pnl_GCS;
        private System.Windows.Forms.Label lb_GCSSelectedState;
        private System.Windows.Forms.Panel pnl_Drone;
        private System.Windows.Forms.Label lb_Drone;
        private System.Windows.Forms.Panel pnl_Camera;
        private System.Windows.Forms.Label lb_Camera;
        private System.Windows.Forms.Label lb_CameraStatusValue;
        private System.Windows.Forms.TableLayoutPanel tlp_SensorLights;
        private System.Windows.Forms.PictureBox pb_DroneTakeOff;
        private System.Windows.Forms.PictureBox pb_InfraLight;
        private System.Windows.Forms.PictureBox pb_PositionIndicator;
        private System.Windows.Forms.TableLayoutPanel tlp_CVBase;
        private LibVLCSharp.WinForms.VideoView vv_VLC;
        private System.Windows.Forms.Button btn_Surveillance;
        private System.Windows.Forms.Button btn_NUC;
        private System.Windows.Forms.Button btn_Polarity;
        private System.Windows.Forms.Button btn_StopTracking;
        private System.Windows.Forms.TableLayoutPanel tlp_Zoom;
        private System.Windows.Forms.Button btn_ZoomMinus;
        private System.Windows.Forms.Button btn_ZoomPlus;
    }
}
