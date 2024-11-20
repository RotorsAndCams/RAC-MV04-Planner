namespace MissionPlanner.Controls
{
    partial class ParamSet
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
            this.components = new System.ComponentModel.Container();
            this.label_ParamName = new System.Windows.Forms.Label();
            this.toolTip_ValueLimits = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label_Unit1 = new System.Windows.Forms.Label();
            this.label_Unit2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown_SetValue = new System.Windows.Forms.NumericUpDown();
            this.button_SetParam = new System.Windows.Forms.Button();
            this.label_CurrentValue = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_SetValue)).BeginInit();
            this.SuspendLayout();
            // 
            // label_ParamName
            // 
            this.label_ParamName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label_ParamName.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label_ParamName, 3);
            this.label_ParamName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_ParamName.Location = new System.Drawing.Point(3, 3);
            this.label_ParamName.Margin = new System.Windows.Forms.Padding(3);
            this.label_ParamName.Name = "label_ParamName";
            this.label_ParamName.Size = new System.Drawing.Size(129, 13);
            this.label_ParamName.TabIndex = 0;
            this.label_ParamName.Text = "label1";
            this.label_ParamName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label_Unit1, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_Unit2, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label_ParamName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDown_SetValue, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.button_SetParam, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label_CurrentValue, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(135, 93);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label_Unit1
            // 
            this.label_Unit1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Unit1.AutoSize = true;
            this.label_Unit1.Location = new System.Drawing.Point(97, 22);
            this.label_Unit1.Margin = new System.Windows.Forms.Padding(3);
            this.label_Unit1.Name = "label_Unit1";
            this.label_Unit1.Size = new System.Drawing.Size(35, 13);
            this.label_Unit1.TabIndex = 0;
            this.label_Unit1.Text = "label3";
            this.label_Unit1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Unit2
            // 
            this.label_Unit2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label_Unit2.AutoSize = true;
            this.label_Unit2.Location = new System.Drawing.Point(97, 44);
            this.label_Unit2.Margin = new System.Windows.Forms.Padding(3);
            this.label_Unit2.Name = "label_Unit2";
            this.label_Unit2.Size = new System.Drawing.Size(35, 13);
            this.label_Unit2.TabIndex = 0;
            this.label_Unit2.Text = "label4";
            this.label_Unit2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Current";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 44);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "New";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numericUpDown_SetValue
            // 
            this.numericUpDown_SetValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown_SetValue.AutoSize = true;
            this.numericUpDown_SetValue.Location = new System.Drawing.Point(50, 41);
            this.numericUpDown_SetValue.Name = "numericUpDown_SetValue";
            this.numericUpDown_SetValue.Size = new System.Drawing.Size(41, 20);
            this.numericUpDown_SetValue.TabIndex = 5;
            // 
            // button_SetParam
            // 
            this.button_SetParam.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tableLayoutPanel1.SetColumnSpan(this.button_SetParam, 2);
            this.button_SetParam.Location = new System.Drawing.Point(57, 67);
            this.button_SetParam.Name = "button_SetParam";
            this.button_SetParam.Size = new System.Drawing.Size(75, 23);
            this.button_SetParam.TabIndex = 3;
            this.button_SetParam.Text = "Set";
            this.button_SetParam.UseVisualStyleBackColor = true;
            this.button_SetParam.Click += new System.EventHandler(this.button_SetParam_Click);
            // 
            // label_CurrentValue
            // 
            this.label_CurrentValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label_CurrentValue.AutoSize = true;
            this.label_CurrentValue.Location = new System.Drawing.Point(50, 22);
            this.label_CurrentValue.Margin = new System.Windows.Forms.Padding(3);
            this.label_CurrentValue.Name = "label_CurrentValue";
            this.label_CurrentValue.Size = new System.Drawing.Size(41, 13);
            this.label_CurrentValue.TabIndex = 6;
            this.label_CurrentValue.Text = "label3";
            this.label_CurrentValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ParamSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.Name = "ParamSet";
            this.Size = new System.Drawing.Size(135, 93);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_SetValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_ParamName;
        private System.Windows.Forms.ToolTip toolTip_ValueLimits;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_SetParam;
        private System.Windows.Forms.NumericUpDown numericUpDown_SetValue;
        private System.Windows.Forms.Label label_Unit1;
        private System.Windows.Forms.Label label_Unit2;
        private System.Windows.Forms.Label label_CurrentValue;
    }
}
