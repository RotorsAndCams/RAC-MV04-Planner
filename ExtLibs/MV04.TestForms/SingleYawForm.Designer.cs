namespace MV04.TestForms
{
    partial class SingleYawForm
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
            this.button_SwitchSingleYaw = new System.Windows.Forms.Button();
            this.label_LastSent = new System.Windows.Forms.Label();
            this.numericUpDown_CameraYaw = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_Kp = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBox_ForceYaw = new System.Windows.Forms.CheckBox();
            this.button_SetYingleYawParams = new System.Windows.Forms.Button();
            this.textBox_Log = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_CameraYaw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Kp)).BeginInit();
            this.SuspendLayout();
            // 
            // button_SwitchSingleYaw
            // 
            this.button_SwitchSingleYaw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SwitchSingleYaw.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_SwitchSingleYaw.Location = new System.Drawing.Point(12, 379);
            this.button_SwitchSingleYaw.Name = "button_SwitchSingleYaw";
            this.button_SwitchSingleYaw.Size = new System.Drawing.Size(410, 50);
            this.button_SwitchSingleYaw.TabIndex = 4;
            this.button_SwitchSingleYaw.Text = "Single-Yaw ON";
            this.button_SwitchSingleYaw.UseVisualStyleBackColor = true;
            this.button_SwitchSingleYaw.Click += new System.EventHandler(this.button_SwitchSingleYaw_Click);
            // 
            // label_LastSent
            // 
            this.label_LastSent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_LastSent.AutoSize = true;
            this.label_LastSent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_LastSent.Location = new System.Drawing.Point(12, 432);
            this.label_LastSent.Name = "label_LastSent";
            this.label_LastSent.Size = new System.Drawing.Size(51, 20);
            this.label_LastSent.TabIndex = 9;
            this.label_LastSent.Text = "label5";
            // 
            // numericUpDown_CameraYaw
            // 
            this.numericUpDown_CameraYaw.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown_CameraYaw.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.numericUpDown_CameraYaw.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown_CameraYaw.Location = new System.Drawing.Point(342, 12);
            this.numericUpDown_CameraYaw.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDown_CameraYaw.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numericUpDown_CameraYaw.Name = "numericUpDown_CameraYaw";
            this.numericUpDown_CameraYaw.Size = new System.Drawing.Size(80, 26);
            this.numericUpDown_CameraYaw.TabIndex = 11;
            // 
            // numericUpDown_Kp
            // 
            this.numericUpDown_Kp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown_Kp.DecimalPlaces = 1;
            this.numericUpDown_Kp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.numericUpDown_Kp.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown_Kp.Location = new System.Drawing.Point(342, 44);
            this.numericUpDown_Kp.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDown_Kp.Name = "numericUpDown_Kp";
            this.numericUpDown_Kp.Size = new System.Drawing.Size(80, 26);
            this.numericUpDown_Kp.TabIndex = 12;
            this.numericUpDown_Kp.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(12, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 20);
            this.label5.TabIndex = 13;
            this.label5.Text = "Camera Yaw";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label6.Location = new System.Drawing.Point(12, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 20);
            this.label6.TabIndex = 14;
            this.label6.Text = "Single-Yaw Kp";
            // 
            // checkBox_ForceYaw
            // 
            this.checkBox_ForceYaw.AutoSize = true;
            this.checkBox_ForceYaw.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_ForceYaw.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.checkBox_ForceYaw.Location = new System.Drawing.Point(209, 13);
            this.checkBox_ForceYaw.Name = "checkBox_ForceYaw";
            this.checkBox_ForceYaw.Size = new System.Drawing.Size(64, 24);
            this.checkBox_ForceYaw.TabIndex = 16;
            this.checkBox_ForceYaw.Text = "force";
            this.checkBox_ForceYaw.UseVisualStyleBackColor = true;
            // 
            // button_SetYingleYawParams
            // 
            this.button_SetYingleYawParams.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SetYingleYawParams.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_SetYingleYawParams.Location = new System.Drawing.Point(342, 76);
            this.button_SetYingleYawParams.Name = "button_SetYingleYawParams";
            this.button_SetYingleYawParams.Size = new System.Drawing.Size(80, 30);
            this.button_SetYingleYawParams.TabIndex = 15;
            this.button_SetYingleYawParams.Text = "Set";
            this.button_SetYingleYawParams.UseVisualStyleBackColor = true;
            this.button_SetYingleYawParams.Click += new System.EventHandler(this.button_SetYingleYawParams_Click);
            // 
            // textBox_Log
            // 
            this.textBox_Log.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Log.Location = new System.Drawing.Point(12, 112);
            this.textBox_Log.Multiline = true;
            this.textBox_Log.Name = "textBox_Log";
            this.textBox_Log.ReadOnly = true;
            this.textBox_Log.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Log.Size = new System.Drawing.Size(410, 261);
            this.textBox_Log.TabIndex = 16;
            this.textBox_Log.WordWrap = false;
            // 
            // SingleYawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 461);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button_SetYingleYawParams);
            this.Controls.Add(this.numericUpDown_Kp);
            this.Controls.Add(this.checkBox_ForceYaw);
            this.Controls.Add(this.textBox_Log);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numericUpDown_CameraYaw);
            this.Controls.Add(this.label_LastSent);
            this.Controls.Add(this.button_SwitchSingleYaw);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SingleYawForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Single-Yaw";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_CameraYaw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Kp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_SwitchSingleYaw;
        private System.Windows.Forms.Label label_LastSent;
        private System.Windows.Forms.NumericUpDown numericUpDown_CameraYaw;
        private System.Windows.Forms.NumericUpDown numericUpDown_Kp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button_SetYingleYawParams;
        private System.Windows.Forms.CheckBox checkBox_ForceYaw;
        private System.Windows.Forms.TextBox textBox_Log;
    }
}