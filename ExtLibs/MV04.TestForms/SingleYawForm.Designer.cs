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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button_SendYaw = new System.Windows.Forms.Button();
            this.button_SwitchSingleYaw = new System.Windows.Forms.Button();
            this.numericUpDown_Deg = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_Speed = new System.Windows.Forms.NumericUpDown();
            this.comboBox_Dir = new System.Windows.Forms.ComboBox();
            this.comboBox_Frame = new System.Windows.Forms.ComboBox();
            this.label_LastSent = new System.Windows.Forms.Label();
            this.button_SendSetPos = new System.Windows.Forms.Button();
            this.numericUpDown_CameraYaw = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_Kp = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_SetYingleYawParams = new System.Windows.Forms.Button();
            this.checkBox_ForceYaw = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Deg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Speed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_CameraYaw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Kp)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Deg";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Speed (deg/s)";
            this.label2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(12, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Dir";
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(12, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "Frame";
            this.label4.Visible = false;
            // 
            // button_SendYaw
            // 
            this.button_SendYaw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SendYaw.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_SendYaw.Location = new System.Drawing.Point(12, 273);
            this.button_SendYaw.Name = "button_SendYaw";
            this.button_SendYaw.Size = new System.Drawing.Size(347, 50);
            this.button_SendYaw.TabIndex = 3;
            this.button_SendYaw.Text = "Send CONDITION_YAW";
            this.button_SendYaw.UseVisualStyleBackColor = true;
            this.button_SendYaw.Visible = false;
            this.button_SendYaw.Click += new System.EventHandler(this.button_SendYaw_Click);
            // 
            // button_SwitchSingleYaw
            // 
            this.button_SwitchSingleYaw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SwitchSingleYaw.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_SwitchSingleYaw.Location = new System.Drawing.Point(12, 385);
            this.button_SwitchSingleYaw.Name = "button_SwitchSingleYaw";
            this.button_SwitchSingleYaw.Size = new System.Drawing.Size(347, 50);
            this.button_SwitchSingleYaw.TabIndex = 4;
            this.button_SwitchSingleYaw.Text = "Single-Yaw ON";
            this.button_SwitchSingleYaw.UseVisualStyleBackColor = true;
            this.button_SwitchSingleYaw.Click += new System.EventHandler(this.button_SwitchSingleYaw_Click);
            // 
            // numericUpDown_Deg
            // 
            this.numericUpDown_Deg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown_Deg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.numericUpDown_Deg.Location = new System.Drawing.Point(279, 10);
            this.numericUpDown_Deg.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numericUpDown_Deg.Name = "numericUpDown_Deg";
            this.numericUpDown_Deg.Size = new System.Drawing.Size(80, 26);
            this.numericUpDown_Deg.TabIndex = 5;
            this.numericUpDown_Deg.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.numericUpDown_Deg.Visible = false;
            // 
            // numericUpDown_Speed
            // 
            this.numericUpDown_Speed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown_Speed.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.numericUpDown_Speed.Location = new System.Drawing.Point(279, 42);
            this.numericUpDown_Speed.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numericUpDown_Speed.Name = "numericUpDown_Speed";
            this.numericUpDown_Speed.Size = new System.Drawing.Size(80, 26);
            this.numericUpDown_Speed.TabIndex = 6;
            this.numericUpDown_Speed.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown_Speed.Visible = false;
            // 
            // comboBox_Dir
            // 
            this.comboBox_Dir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_Dir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Dir.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.comboBox_Dir.FormattingEnabled = true;
            this.comboBox_Dir.Items.AddRange(new object[] {
            "CW",
            "CCW",
            "Auto"});
            this.comboBox_Dir.Location = new System.Drawing.Point(279, 74);
            this.comboBox_Dir.Name = "comboBox_Dir";
            this.comboBox_Dir.Size = new System.Drawing.Size(80, 28);
            this.comboBox_Dir.TabIndex = 7;
            this.comboBox_Dir.Visible = false;
            // 
            // comboBox_Frame
            // 
            this.comboBox_Frame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_Frame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Frame.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.comboBox_Frame.FormattingEnabled = true;
            this.comboBox_Frame.Items.AddRange(new object[] {
            "Abs",
            "Rel"});
            this.comboBox_Frame.Location = new System.Drawing.Point(279, 108);
            this.comboBox_Frame.Name = "comboBox_Frame";
            this.comboBox_Frame.Size = new System.Drawing.Size(80, 28);
            this.comboBox_Frame.TabIndex = 8;
            this.comboBox_Frame.Visible = false;
            // 
            // label_LastSent
            // 
            this.label_LastSent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_LastSent.AutoSize = true;
            this.label_LastSent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_LastSent.Location = new System.Drawing.Point(12, 438);
            this.label_LastSent.Name = "label_LastSent";
            this.label_LastSent.Size = new System.Drawing.Size(51, 20);
            this.label_LastSent.TabIndex = 9;
            this.label_LastSent.Text = "label5";
            // 
            // button_SendSetPos
            // 
            this.button_SendSetPos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SendSetPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_SendSetPos.Location = new System.Drawing.Point(12, 329);
            this.button_SendSetPos.Name = "button_SendSetPos";
            this.button_SendSetPos.Size = new System.Drawing.Size(347, 50);
            this.button_SendSetPos.TabIndex = 10;
            this.button_SendSetPos.Text = "Send SET_POSITION";
            this.button_SendSetPos.UseVisualStyleBackColor = true;
            this.button_SendSetPos.Visible = false;
            this.button_SendSetPos.Click += new System.EventHandler(this.button_SendSetPos_Click);
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
            this.numericUpDown_CameraYaw.Location = new System.Drawing.Point(261, 16);
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
            this.numericUpDown_Kp.Location = new System.Drawing.Point(261, 48);
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
            this.label5.Location = new System.Drawing.Point(6, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 20);
            this.label5.TabIndex = 13;
            this.label5.Text = "Camera Yaw";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label6.Location = new System.Drawing.Point(6, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 20);
            this.label6.TabIndex = 14;
            this.label6.Text = "Single-Yaw Kp";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox_ForceYaw);
            this.groupBox1.Controls.Add(this.button_SetYingleYawParams);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.numericUpDown_CameraYaw);
            this.groupBox1.Controls.Add(this.numericUpDown_Kp);
            this.groupBox1.Location = new System.Drawing.Point(12, 142);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(347, 117);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Aingle-Yaw Params";
            // 
            // button_SetYingleYawParams
            // 
            this.button_SetYingleYawParams.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SetYingleYawParams.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_SetYingleYawParams.Location = new System.Drawing.Point(266, 81);
            this.button_SetYingleYawParams.Name = "button_SetYingleYawParams";
            this.button_SetYingleYawParams.Size = new System.Drawing.Size(75, 30);
            this.button_SetYingleYawParams.TabIndex = 15;
            this.button_SetYingleYawParams.Text = "Set";
            this.button_SetYingleYawParams.UseVisualStyleBackColor = true;
            this.button_SetYingleYawParams.Click += new System.EventHandler(this.button_SetYingleYawParams_Click);
            // 
            // checkBox_ForceYaw
            // 
            this.checkBox_ForceYaw.AutoSize = true;
            this.checkBox_ForceYaw.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_ForceYaw.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.checkBox_ForceYaw.Location = new System.Drawing.Point(191, 17);
            this.checkBox_ForceYaw.Name = "checkBox_ForceYaw";
            this.checkBox_ForceYaw.Size = new System.Drawing.Size(64, 24);
            this.checkBox_ForceYaw.TabIndex = 16;
            this.checkBox_ForceYaw.Text = "force";
            this.checkBox_ForceYaw.UseVisualStyleBackColor = true;
            // 
            // SingleYawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 467);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_SendSetPos);
            this.Controls.Add(this.label_LastSent);
            this.Controls.Add(this.comboBox_Frame);
            this.Controls.Add(this.comboBox_Dir);
            this.Controls.Add(this.numericUpDown_Speed);
            this.Controls.Add(this.numericUpDown_Deg);
            this.Controls.Add(this.button_SwitchSingleYaw);
            this.Controls.Add(this.button_SendYaw);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SingleYawForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Single-Yaw";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Deg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Speed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_CameraYaw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Kp)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_SendYaw;
        private System.Windows.Forms.Button button_SwitchSingleYaw;
        private System.Windows.Forms.NumericUpDown numericUpDown_Deg;
        private System.Windows.Forms.NumericUpDown numericUpDown_Speed;
        private System.Windows.Forms.ComboBox comboBox_Dir;
        private System.Windows.Forms.ComboBox comboBox_Frame;
        private System.Windows.Forms.Label label_LastSent;
        private System.Windows.Forms.Button button_SendSetPos;
        private System.Windows.Forms.NumericUpDown numericUpDown_CameraYaw;
        private System.Windows.Forms.NumericUpDown numericUpDown_Kp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button_SetYingleYawParams;
        private System.Windows.Forms.CheckBox checkBox_ForceYaw;
    }
}