namespace MissionPlanner.Controls
{
    partial class FlightSafetyForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_Buttons = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel_Buttons = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage_ParamSet = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel_ParamSet = new System.Windows.Forms.FlowLayoutPanel();
            this.tabControl1.SuspendLayout();
            this.tabPage_Buttons.SuspendLayout();
            this.tabPage_ParamSet.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_Buttons);
            this.tabControl1.Controls.Add(this.tabPage_ParamSet);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(804, 461);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage_Buttons
            // 
            this.tabPage_Buttons.Controls.Add(this.flowLayoutPanel_Buttons);
            this.tabPage_Buttons.Location = new System.Drawing.Point(4, 29);
            this.tabPage_Buttons.Name = "tabPage_Buttons";
            this.tabPage_Buttons.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Buttons.Size = new System.Drawing.Size(796, 428);
            this.tabPage_Buttons.TabIndex = 0;
            this.tabPage_Buttons.Text = "Buttons";
            this.tabPage_Buttons.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel_Buttons
            // 
            this.flowLayoutPanel_Buttons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel_Buttons.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel_Buttons.Name = "flowLayoutPanel_Buttons";
            this.flowLayoutPanel_Buttons.Size = new System.Drawing.Size(790, 422);
            this.flowLayoutPanel_Buttons.TabIndex = 0;
            // 
            // tabPage_ParamSet
            // 
            this.tabPage_ParamSet.Controls.Add(this.flowLayoutPanel_ParamSet);
            this.tabPage_ParamSet.Location = new System.Drawing.Point(4, 29);
            this.tabPage_ParamSet.Name = "tabPage_ParamSet";
            this.tabPage_ParamSet.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_ParamSet.Size = new System.Drawing.Size(796, 428);
            this.tabPage_ParamSet.TabIndex = 1;
            this.tabPage_ParamSet.Text = "Parameters";
            this.tabPage_ParamSet.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel_ParamSet
            // 
            this.flowLayoutPanel_ParamSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel_ParamSet.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel_ParamSet.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel_ParamSet.Name = "flowLayoutPanel_ParamSet";
            this.flowLayoutPanel_ParamSet.Size = new System.Drawing.Size(790, 422);
            this.flowLayoutPanel_ParamSet.TabIndex = 0;
            // 
            // FlightSafetyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 461);
            this.Controls.Add(this.tabControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FlightSafetyForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Flight Safety Config";
            this.tabControl1.ResumeLayout(false);
            this.tabPage_Buttons.ResumeLayout(false);
            this.tabPage_ParamSet.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_Buttons;
        private System.Windows.Forms.TabPage tabPage_ParamSet;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_ParamSet;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_Buttons;
    }
}