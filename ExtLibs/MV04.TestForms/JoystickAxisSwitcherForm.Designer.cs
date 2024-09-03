namespace MV04.TestForms
{
    partial class JoystickAxisSwitcherForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton_PitchPitch = new System.Windows.Forms.RadioButton();
            this.radioButton_ThrottleZoom = new System.Windows.Forms.RadioButton();
            this.radioButton_UAV = new System.Windows.Forms.RadioButton();
            this.radioButton_Camera = new System.Windows.Forms.RadioButton();
            this.button_Set = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_PitchPitch);
            this.groupBox1.Controls.Add(this.radioButton_ThrottleZoom);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(215, 45);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Axis pair";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton_UAV);
            this.groupBox2.Controls.Add(this.radioButton_Camera);
            this.groupBox2.Location = new System.Drawing.Point(12, 63);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(215, 45);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Axis mode";
            // 
            // radioButton_PitchPitch
            // 
            this.radioButton_PitchPitch.AutoSize = true;
            this.radioButton_PitchPitch.Location = new System.Drawing.Point(6, 19);
            this.radioButton_PitchPitch.Name = "radioButton_PitchPitch";
            this.radioButton_PitchPitch.Size = new System.Drawing.Size(82, 17);
            this.radioButton_PitchPitch.TabIndex = 2;
            this.radioButton_PitchPitch.TabStop = true;
            this.radioButton_PitchPitch.Text = "Pitch - Pitch";
            this.radioButton_PitchPitch.UseVisualStyleBackColor = true;
            // 
            // radioButton_ThrottleZoom
            // 
            this.radioButton_ThrottleZoom.AutoSize = true;
            this.radioButton_ThrottleZoom.Location = new System.Drawing.Point(106, 19);
            this.radioButton_ThrottleZoom.Name = "radioButton_ThrottleZoom";
            this.radioButton_ThrottleZoom.Size = new System.Drawing.Size(97, 17);
            this.radioButton_ThrottleZoom.TabIndex = 3;
            this.radioButton_ThrottleZoom.TabStop = true;
            this.radioButton_ThrottleZoom.Text = "Throttle - Zoom";
            this.radioButton_ThrottleZoom.UseVisualStyleBackColor = true;
            // 
            // radioButton_UAV
            // 
            this.radioButton_UAV.AutoSize = true;
            this.radioButton_UAV.Location = new System.Drawing.Point(6, 19);
            this.radioButton_UAV.Name = "radioButton_UAV";
            this.radioButton_UAV.Size = new System.Drawing.Size(47, 17);
            this.radioButton_UAV.TabIndex = 4;
            this.radioButton_UAV.TabStop = true;
            this.radioButton_UAV.Text = "UAV";
            this.radioButton_UAV.UseVisualStyleBackColor = true;
            // 
            // radioButton_Camera
            // 
            this.radioButton_Camera.AutoSize = true;
            this.radioButton_Camera.Location = new System.Drawing.Point(106, 19);
            this.radioButton_Camera.Name = "radioButton_Camera";
            this.radioButton_Camera.Size = new System.Drawing.Size(61, 17);
            this.radioButton_Camera.TabIndex = 5;
            this.radioButton_Camera.TabStop = true;
            this.radioButton_Camera.Text = "Camera";
            this.radioButton_Camera.UseVisualStyleBackColor = true;
            // 
            // button_Set
            // 
            this.button_Set.Location = new System.Drawing.Point(152, 114);
            this.button_Set.Name = "button_Set";
            this.button_Set.Size = new System.Drawing.Size(75, 23);
            this.button_Set.TabIndex = 6;
            this.button_Set.Text = "Set";
            this.button_Set.UseVisualStyleBackColor = true;
            this.button_Set.Click += new System.EventHandler(this.button_Set_Click);
            // 
            // JoystickAxisSwitcherForm
            // 
            this.AcceptButton = this.button_Set;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 147);
            this.Controls.Add(this.button_Set);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "JoystickAxisSwitcherForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Joystick Axis Switcher Form";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton_PitchPitch;
        private System.Windows.Forms.RadioButton radioButton_ThrottleZoom;
        private System.Windows.Forms.RadioButton radioButton_UAV;
        private System.Windows.Forms.RadioButton radioButton_Camera;
        private System.Windows.Forms.Button button_Set;
    }
}