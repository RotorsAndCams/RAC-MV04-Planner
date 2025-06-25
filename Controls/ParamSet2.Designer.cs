namespace MissionPlanner.Controls
{
    partial class ParamSet2
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label_ParamName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label_ParamValue = new System.Windows.Forms.Label();
            this.numericUpDown_ParamValue = new System.Windows.Forms.NumericUpDown();
            this.button_Set = new System.Windows.Forms.Button();
            this.toolTip_ParamLimits = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_ParamValue)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label_ParamName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label_ParamValue, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDown_ParamValue, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.button_Set, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(290, 100);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label_ParamName
            // 
            this.label_ParamName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_ParamName.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label_ParamName, 3);
            this.label_ParamName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_ParamName.Location = new System.Drawing.Point(3, 6);
            this.label_ParamName.Name = "label_ParamName";
            this.label_ParamName.Size = new System.Drawing.Size(121, 20);
            this.label_ParamName.TabIndex = 0;
            this.label_ParamName.Text = "PARAM_NAME";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(3, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Current";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(3, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "New";
            // 
            // label_ParamValue
            // 
            this.label_ParamValue.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_ParamValue.AutoSize = true;
            this.label_ParamValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label_ParamValue.Location = new System.Drawing.Point(71, 39);
            this.label_ParamValue.Name = "label_ParamValue";
            this.label_ParamValue.Size = new System.Drawing.Size(18, 20);
            this.label_ParamValue.TabIndex = 3;
            this.label_ParamValue.Text = "0";
            // 
            // numericUpDown_ParamValue
            // 
            this.numericUpDown_ParamValue.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numericUpDown_ParamValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.numericUpDown_ParamValue.Location = new System.Drawing.Point(71, 70);
            this.numericUpDown_ParamValue.Name = "numericUpDown_ParamValue";
            this.numericUpDown_ParamValue.Size = new System.Drawing.Size(120, 26);
            this.numericUpDown_ParamValue.TabIndex = 4;
            // 
            // button_Set
            // 
            this.button_Set.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Set.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button_Set.Location = new System.Drawing.Point(197, 57);
            this.button_Set.Name = "button_Set";
            this.tableLayoutPanel1.SetRowSpan(this.button_Set, 2);
            this.button_Set.Size = new System.Drawing.Size(90, 40);
            this.button_Set.TabIndex = 5;
            this.button_Set.Text = "Set";
            this.button_Set.UseVisualStyleBackColor = true;
            this.button_Set.Click += new System.EventHandler(this.button_Set_Click);
            // 
            // ParamSet2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximumSize = new System.Drawing.Size(290, 100);
            this.Name = "ParamSet2";
            this.Size = new System.Drawing.Size(290, 100);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_ParamValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label_ParamName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_ParamValue;
        private System.Windows.Forms.NumericUpDown numericUpDown_ParamValue;
        private System.Windows.Forms.Button button_Set;
        private System.Windows.Forms.ToolTip toolTip_ParamLimits;
    }
}
