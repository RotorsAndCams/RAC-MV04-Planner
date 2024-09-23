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
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Deg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Speed)).BeginInit();
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
            // 
            // button_SendYaw
            // 
            this.button_SendYaw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SendYaw.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_SendYaw.Location = new System.Drawing.Point(12, 149);
            this.button_SendYaw.Name = "button_SendYaw";
            this.button_SendYaw.Size = new System.Drawing.Size(347, 50);
            this.button_SendYaw.TabIndex = 3;
            this.button_SendYaw.Text = "Send CONDITION_YAW";
            this.button_SendYaw.UseVisualStyleBackColor = true;
            this.button_SendYaw.Click += new System.EventHandler(this.button_SendYaw_Click);
            // 
            // button_SwitchSingleYaw
            // 
            this.button_SwitchSingleYaw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_SwitchSingleYaw.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_SwitchSingleYaw.Location = new System.Drawing.Point(12, 205);
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
            // 
            // label_LastSent
            // 
            this.label_LastSent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label_LastSent.AutoSize = true;
            this.label_LastSent.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_LastSent.Location = new System.Drawing.Point(12, 258);
            this.label_LastSent.Name = "label_LastSent";
            this.label_LastSent.Size = new System.Drawing.Size(51, 20);
            this.label_LastSent.TabIndex = 9;
            this.label_LastSent.Text = "label5";
            // 
            // SingleYawForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 287);
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
    }
}