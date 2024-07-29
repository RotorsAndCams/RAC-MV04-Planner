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
            this.btn_ResetZoom = new System.Windows.Forms.Button();
            this.tlp_ButtonSection1 = new System.Windows.Forms.TableLayoutPanel();
            this.btn_TripSwitchOnOff = new System.Windows.Forms.Button();
            this.btn_ChangeCrosshair = new System.Windows.Forms.Button();
            this.btn_FullScreen = new System.Windows.Forms.Button();
            this.btn_FPVCameraMode = new System.Windows.Forms.Button();
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
            this.tlp_AGLContainer = new System.Windows.Forms.TableLayoutPanel();
            this.cs_ColorSliderAltitude = new MissionPlanner.Controls.ColorSlider();
            this.tlp_AGLIncrement = new System.Windows.Forms.TableLayoutPanel();
            this.btn_SetAlt = new System.Windows.Forms.Button();
            this.btn_Down = new System.Windows.Forms.Button();
            this.btn_Up = new System.Windows.Forms.Button();
            this.tlp_AGLData = new System.Windows.Forms.TableLayoutPanel();
            this.lb_AltitudeValue = new System.Windows.Forms.Label();
            this.lb_Agl = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pb_CameraGstream = new System.Windows.Forms.Panel();
            this.tlp_ControlsBase.SuspendLayout();
            this.tlp_Buttonsection2.SuspendLayout();
            this.tlp_ButtonSection1.SuspendLayout();
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
            this.tlp_AGLContainer.SuspendLayout();
            this.tlp_AGLIncrement.SuspendLayout();
            this.tlp_AGLData.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlp_ControlsBase
            // 
            resources.ApplyResources(this.tlp_ControlsBase, "tlp_ControlsBase");
            this.tlp_ControlsBase.Controls.Add(this.tlp_Buttonsection2, 0, 2);
            this.tlp_ControlsBase.Controls.Add(this.tlp_ButtonSection1, 0, 1);
            this.tlp_ControlsBase.Controls.Add(this.tlp_InfoBase, 0, 0);
            this.tlp_ControlsBase.Controls.Add(this.tlp_AGLContainer, 0, 3);
            this.tlp_ControlsBase.Name = "tlp_ControlsBase";
            // 
            // tlp_Buttonsection2
            // 
            resources.ApplyResources(this.tlp_Buttonsection2, "tlp_Buttonsection2");
            this.tlp_Buttonsection2.Controls.Add(this.btn_StopTracking, 1, 0);
            this.tlp_Buttonsection2.Controls.Add(this.btn_ResetZoom, 0, 0);
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
            // tlp_ButtonSection1
            // 
            resources.ApplyResources(this.tlp_ButtonSection1, "tlp_ButtonSection1");
            this.tlp_ButtonSection1.Controls.Add(this.btn_TripSwitchOnOff, 0, 0);
            this.tlp_ButtonSection1.Controls.Add(this.btn_ChangeCrosshair, 1, 0);
            this.tlp_ButtonSection1.Controls.Add(this.btn_FullScreen, 1, 1);
            this.tlp_ButtonSection1.Controls.Add(this.btn_FPVCameraMode, 0, 1);
            this.tlp_ButtonSection1.Name = "tlp_ButtonSection1";
            // 
            // btn_TripSwitchOnOff
            // 
            this.btn_TripSwitchOnOff.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btn_TripSwitchOnOff, "btn_TripSwitchOnOff");
            this.btn_TripSwitchOnOff.ForeColor = System.Drawing.Color.White;
            this.btn_TripSwitchOnOff.Image = global::MissionPlanner.Properties.Resources.power_64;
            this.btn_TripSwitchOnOff.Name = "btn_TripSwitchOnOff";
            this.btn_TripSwitchOnOff.UseVisualStyleBackColor = false;
            this.btn_TripSwitchOnOff.Click += new System.EventHandler(this.btn_TripSwitchOnOff_Click);
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
            // btn_FPVCameraMode
            // 
            this.btn_FPVCameraMode.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.btn_FPVCameraMode, "btn_FPVCameraMode");
            this.btn_FPVCameraMode.ForeColor = System.Drawing.Color.White;
            this.btn_FPVCameraMode.Name = "btn_FPVCameraMode";
            this.btn_FPVCameraMode.UseVisualStyleBackColor = false;
            this.btn_FPVCameraMode.Click += new System.EventHandler(this.btn_FPVCameraMode_Click);
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
            // tlp_AGLContainer
            // 
            this.tlp_AGLContainer.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.tlp_AGLContainer, "tlp_AGLContainer");
            this.tlp_AGLContainer.Controls.Add(this.cs_ColorSliderAltitude, 1, 0);
            this.tlp_AGLContainer.Controls.Add(this.tlp_AGLIncrement, 0, 0);
            this.tlp_AGLContainer.ForeColor = System.Drawing.Color.RosyBrown;
            this.tlp_AGLContainer.Name = "tlp_AGLContainer";
            // 
            // cs_ColorSliderAltitude
            // 
            this.cs_ColorSliderAltitude.BackColor = System.Drawing.Color.Black;
            this.cs_ColorSliderAltitude.BarInnerColor = System.Drawing.Color.Chartreuse;
            this.cs_ColorSliderAltitude.BarOuterColor = System.Drawing.Color.DarkGreen;
            this.cs_ColorSliderAltitude.BarPenColor = System.Drawing.Color.Silver;
            this.cs_ColorSliderAltitude.BorderRoundRectSize = new System.Drawing.Size(15, 15);
            resources.ApplyResources(this.cs_ColorSliderAltitude, "cs_ColorSliderAltitude");
            this.cs_ColorSliderAltitude.ForeColor = System.Drawing.Color.Black;
            this.cs_ColorSliderAltitude.LargeChange = ((uint)(10u));
            this.cs_ColorSliderAltitude.Maximum = 500;
            this.cs_ColorSliderAltitude.Minimum = 50;
            this.cs_ColorSliderAltitude.MouseWheelBarPartitions = 50;
            this.cs_ColorSliderAltitude.Name = "cs_ColorSliderAltitude";
            this.cs_ColorSliderAltitude.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.cs_ColorSliderAltitude.SmallChange = ((uint)(1u));
            this.cs_ColorSliderAltitude.ThumbInnerColor = System.Drawing.Color.White;
            this.cs_ColorSliderAltitude.ThumbRoundRectSize = new System.Drawing.Size(10, 20);
            this.cs_ColorSliderAltitude.ThumbSize = 40;
            this.cs_ColorSliderAltitude.ValueChanged += new System.EventHandler(this.cs_ColorSliderAltitude_ValueChanged);
            // 
            // tlp_AGLIncrement
            // 
            resources.ApplyResources(this.tlp_AGLIncrement, "tlp_AGLIncrement");
            this.tlp_AGLIncrement.Controls.Add(this.btn_SetAlt, 0, 3);
            this.tlp_AGLIncrement.Controls.Add(this.btn_Down, 0, 2);
            this.tlp_AGLIncrement.Controls.Add(this.btn_Up, 0, 0);
            this.tlp_AGLIncrement.Controls.Add(this.tlp_AGLData, 0, 1);
            this.tlp_AGLIncrement.Name = "tlp_AGLIncrement";
            // 
            // btn_SetAlt
            // 
            resources.ApplyResources(this.btn_SetAlt, "btn_SetAlt");
            this.btn_SetAlt.BackColor = System.Drawing.Color.Black;
            this.btn_SetAlt.ForeColor = System.Drawing.Color.Black;
            this.btn_SetAlt.Image = global::MissionPlanner.Properties.Resources.confirm_64;
            this.btn_SetAlt.Name = "btn_SetAlt";
            this.btn_SetAlt.UseVisualStyleBackColor = false;
            this.btn_SetAlt.Click += new System.EventHandler(this.btn_SetAlt_Click);
            // 
            // btn_Down
            // 
            resources.ApplyResources(this.btn_Down, "btn_Down");
            this.btn_Down.BackColor = System.Drawing.Color.Black;
            this.btn_Down.ForeColor = System.Drawing.Color.Black;
            this.btn_Down.Image = global::MissionPlanner.Properties.Resources.icons8_down_64__1_;
            this.btn_Down.Name = "btn_Down";
            this.btn_Down.UseVisualStyleBackColor = false;
            this.btn_Down.Click += new System.EventHandler(this.btn_Down_Click);
            // 
            // btn_Up
            // 
            resources.ApplyResources(this.btn_Up, "btn_Up");
            this.btn_Up.BackColor = System.Drawing.Color.Black;
            this.btn_Up.ForeColor = System.Drawing.Color.Black;
            this.btn_Up.Image = global::MissionPlanner.Properties.Resources.icons8_upward_arrow_64;
            this.btn_Up.Name = "btn_Up";
            this.btn_Up.UseVisualStyleBackColor = false;
            this.btn_Up.Click += new System.EventHandler(this.btn_Up_Click);
            // 
            // tlp_AGLData
            // 
            this.tlp_AGLData.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.tlp_AGLData, "tlp_AGLData");
            this.tlp_AGLData.Controls.Add(this.lb_AltitudeValue, 0, 1);
            this.tlp_AGLData.Controls.Add(this.lb_Agl, 0, 0);
            this.tlp_AGLData.Name = "tlp_AGLData";
            // 
            // lb_AltitudeValue
            // 
            resources.ApplyResources(this.lb_AltitudeValue, "lb_AltitudeValue");
            this.lb_AltitudeValue.ForeColor = System.Drawing.Color.White;
            this.lb_AltitudeValue.Name = "lb_AltitudeValue";
            // 
            // lb_Agl
            // 
            resources.ApplyResources(this.lb_Agl, "lb_Agl");
            this.lb_Agl.ForeColor = System.Drawing.Color.White;
            this.lb_Agl.Name = "lb_Agl";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.tlp_ControlsBase, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.pb_CameraGstream, 0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // pb_CameraGstream
            // 
            resources.ApplyResources(this.pb_CameraGstream, "pb_CameraGstream");
            this.pb_CameraGstream.Name = "pb_CameraGstream";
            // 
            // CameraView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "CameraView";
            this.tlp_ControlsBase.ResumeLayout(false);
            this.tlp_Buttonsection2.ResumeLayout(false);
            this.tlp_ButtonSection1.ResumeLayout(false);
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
            this.tlp_AGLContainer.ResumeLayout(false);
            this.tlp_AGLIncrement.ResumeLayout(false);
            this.tlp_AGLIncrement.PerformLayout();
            this.tlp_AGLData.ResumeLayout(false);
            this.tlp_AGLData.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tlp_ControlsBase;
        private System.Windows.Forms.TableLayoutPanel tlp_SensorLights;
        private System.Windows.Forms.TableLayoutPanel tlp_DeviceStatusInfo;
        private System.Windows.Forms.Panel pnl_SettingsPanel;
        private System.Windows.Forms.TableLayoutPanel tlp_InfoBase;
        private System.Windows.Forms.Panel pnl_DeviceState;
        private System.Windows.Forms.TableLayoutPanel tlp_AGLContainer;
        private System.Windows.Forms.TableLayoutPanel tlp_AGLIncrement;
        private System.Windows.Forms.Button btn_Down;
        private System.Windows.Forms.Button btn_Up;
        private System.Windows.Forms.TableLayoutPanel tlp_AGLData;
        private System.Windows.Forms.Label lb_AltitudeValue;
        private System.Windows.Forms.Label lb_Agl;
        private System.Windows.Forms.Label lb_CameraStatusValue;
        private System.Windows.Forms.Label lb_DroneStatusValue;
        private System.Windows.Forms.Label lb_Camera;
        private System.Windows.Forms.Label lb_Drone;
        private System.Windows.Forms.TableLayoutPanel tlp_ButtonSection1;
        private System.Windows.Forms.Button btn_ResetZoom;
        private System.Windows.Forms.Button btn_Settings;
        private System.Windows.Forms.Button btn_FullScreen;
        private System.Windows.Forms.Button btn_StopTracking;
        private System.Windows.Forms.Button btn_ChangeCrosshair;
        private Controls.ColorSlider cs_ColorSliderAltitude;
        private System.Windows.Forms.PictureBox pb_PositionIndicator;
        private System.Windows.Forms.PictureBox pb_InfraLight;
        private System.Windows.Forms.PictureBox pb_DroneTakeOff;
        private System.Windows.Forms.Button btn_FPVCameraMode;
        private System.Windows.Forms.TableLayoutPanel tlp_Buttonsection2;
        private System.Windows.Forms.TableLayoutPanel tlp_StatusUpper;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lb_GCSSelectedState;
        private System.Windows.Forms.Label lb_GCSSelectedStateValue;
        private System.Windows.Forms.Panel pnl_Drone;
        private System.Windows.Forms.Panel pnl_GCS;
        private System.Windows.Forms.Panel pnl_Camera;
        private System.Windows.Forms.Button btn_SetAlt;
        private System.Windows.Forms.Button btn_TripSwitchOnOff;
        private System.Windows.Forms.Panel pb_CameraGstream;
    }
}
