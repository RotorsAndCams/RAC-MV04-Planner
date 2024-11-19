namespace MissionPlanner.Controls
{
    partial class YesNoForm
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
            this.button_Yes = new System.Windows.Forms.Button();
            this.button_No = new System.Windows.Forms.Button();
            this.label_Text = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Yes
            // 
            this.button_Yes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Yes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.button_Yes.Location = new System.Drawing.Point(116, 78);
            this.button_Yes.Name = "button_Yes";
            this.button_Yes.Size = new System.Drawing.Size(75, 23);
            this.button_Yes.TabIndex = 1;
            this.button_Yes.Text = "Yes";
            this.button_Yes.UseVisualStyleBackColor = true;
            this.button_Yes.Click += new System.EventHandler(this.button_Click);
            // 
            // button_No
            // 
            this.button_No.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_No.DialogResult = System.Windows.Forms.DialogResult.No;
            this.button_No.Location = new System.Drawing.Point(197, 78);
            this.button_No.Name = "button_No";
            this.button_No.Size = new System.Drawing.Size(75, 23);
            this.button_No.TabIndex = 2;
            this.button_No.Text = "No";
            this.button_No.UseVisualStyleBackColor = true;
            this.button_No.Click += new System.EventHandler(this.button_Click);
            // 
            // label_Text
            // 
            this.label_Text.AutoSize = true;
            this.label_Text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Text.Location = new System.Drawing.Point(0, 0);
            this.label_Text.Name = "label_Text";
            this.label_Text.Size = new System.Drawing.Size(35, 13);
            this.label_Text.TabIndex = 3;
            this.label_Text.Text = "label1";
            this.label_Text.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label_Text);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(260, 60);
            this.panel1.TabIndex = 4;
            // 
            // YesNoForm
            // 
            this.AcceptButton = this.button_Yes;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_No;
            this.ClientSize = new System.Drawing.Size(284, 113);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button_No);
            this.Controls.Add(this.button_Yes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "YesNoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "YesNoForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_Yes;
        private System.Windows.Forms.Button button_No;
        private System.Windows.Forms.Label label_Text;
        private System.Windows.Forms.Panel panel1;
    }
}