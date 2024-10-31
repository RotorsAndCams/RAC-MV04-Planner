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
            this.button_Set = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_Modes = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // button_Set
            // 
            this.button_Set.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Set.Location = new System.Drawing.Point(98, 46);
            this.button_Set.Name = "button_Set";
            this.button_Set.Size = new System.Drawing.Size(75, 23);
            this.button_Set.TabIndex = 6;
            this.button_Set.Text = "Set";
            this.button_Set.UseVisualStyleBackColor = true;
            this.button_Set.Click += new System.EventHandler(this.button_Set_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Mode";
            // 
            // comboBox_Modes
            // 
            this.comboBox_Modes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Modes.FormattingEnabled = true;
            this.comboBox_Modes.Location = new System.Drawing.Point(53, 10);
            this.comboBox_Modes.Name = "comboBox_Modes";
            this.comboBox_Modes.Size = new System.Drawing.Size(121, 21);
            this.comboBox_Modes.TabIndex = 8;
            // 
            // JoystickAxisSwitcherForm
            // 
            this.AcceptButton = this.button_Set;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(185, 81);
            this.Controls.Add(this.comboBox_Modes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_Set);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "JoystickAxisSwitcherForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Joystick Axis Switcher Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_Set;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox_Modes;
    }
}