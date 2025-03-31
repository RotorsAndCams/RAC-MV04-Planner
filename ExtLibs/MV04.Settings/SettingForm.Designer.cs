namespace MV04.Settings
{
    partial class SettingForm
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
            this.components = new System.ComponentModel.Container();
            this.button_Save = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_speedFormat = new System.Windows.Forms.ComboBox();
            this.comboBox_distFormat = new System.Windows.Forms.ComboBox();
            this.comboBox_altFormat = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_coordFormat = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_cameraIp = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox_IrColorMode = new System.Windows.Forms.ComboBox();
            this.textBox_cameraControlPort = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.radioButton_AutoConnect_Yes = new System.Windows.Forms.RadioButton();
            this.radioButton_AutoConnect_No = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label13 = new System.Windows.Forms.Label();
            this.numericUpDown_VideoSegmentLength = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.textBox_StreamUrl = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_VideoSegmentLength)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_Save
            // 
            this.button_Save.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_Save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Save.Location = new System.Drawing.Point(69, 3);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(75, 19);
            this.button_Save.TabIndex = 0;
            this.button_Save.Text = "Save";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Location = new System.Drawing.Point(150, 3);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(75, 19);
            this.button_Cancel.TabIndex = 1;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 208);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Koordináta formátum";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox_speedFormat
            // 
            this.comboBox_speedFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_speedFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_speedFormat.FormattingEnabled = true;
            this.comboBox_speedFormat.Items.AddRange(new object[] {
            "mps",
            "kmph",
            "knots"});
            this.comboBox_speedFormat.Location = new System.Drawing.Point(153, 303);
            this.comboBox_speedFormat.Name = "comboBox_speedFormat";
            this.comboBox_speedFormat.Size = new System.Drawing.Size(228, 21);
            this.comboBox_speedFormat.TabIndex = 9;
            // 
            // comboBox_distFormat
            // 
            this.comboBox_distFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_distFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_distFormat.FormattingEnabled = true;
            this.comboBox_distFormat.Items.AddRange(new object[] {
            "m",
            "km",
            "ft"});
            this.comboBox_distFormat.Location = new System.Drawing.Point(153, 270);
            this.comboBox_distFormat.Name = "comboBox_distFormat";
            this.comboBox_distFormat.Size = new System.Drawing.Size(228, 21);
            this.comboBox_distFormat.TabIndex = 8;
            // 
            // comboBox_altFormat
            // 
            this.comboBox_altFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_altFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_altFormat.FormattingEnabled = true;
            this.comboBox_altFormat.Items.AddRange(new object[] {
            "m",
            "ft"});
            this.comboBox_altFormat.Location = new System.Drawing.Point(153, 237);
            this.comboBox_altFormat.Name = "comboBox_altFormat";
            this.comboBox_altFormat.Size = new System.Drawing.Size(228, 21);
            this.comboBox_altFormat.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 241);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Magasság mértékegység";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 274);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Távolság mértékegység";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 307);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Sebesség mértékegység";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox_coordFormat
            // 
            this.comboBox_coordFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_coordFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_coordFormat.FormattingEnabled = true;
            this.comboBox_coordFormat.Items.AddRange(new object[] {
            "WGS84",
            "MGRS"});
            this.comboBox_coordFormat.Location = new System.Drawing.Point(153, 204);
            this.comboBox_coordFormat.Name = "comboBox_coordFormat";
            this.comboBox_coordFormat.Size = new System.Drawing.Size(228, 21);
            this.comboBox_coordFormat.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(91, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Kamera IP";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(45, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Kamera stream URL";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_cameraIp
            // 
            this.textBox_cameraIp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_cameraIp.Location = new System.Drawing.Point(153, 6);
            this.textBox_cameraIp.Name = "textBox_cameraIp";
            this.textBox_cameraIp.Size = new System.Drawing.Size(228, 20);
            this.textBox_cameraIp.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(83, 175);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "IR szín mód";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox_IrColorMode
            // 
            this.comboBox_IrColorMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_IrColorMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_IrColorMode.FormattingEnabled = true;
            this.comboBox_IrColorMode.Items.AddRange(new object[] {
            "WhiteHot",
            "BlackHot",
            "Color",
            "ColorInverse"});
            this.comboBox_IrColorMode.Location = new System.Drawing.Point(153, 171);
            this.comboBox_IrColorMode.Name = "comboBox_IrColorMode";
            this.comboBox_IrColorMode.Size = new System.Drawing.Size(228, 21);
            this.comboBox_IrColorMode.TabIndex = 15;
            // 
            // textBox_cameraControlPort
            // 
            this.textBox_cameraControlPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_cameraControlPort.Location = new System.Drawing.Point(153, 39);
            this.textBox_cameraControlPort.Name = "textBox_cameraControlPort";
            this.textBox_cameraControlPort.Size = new System.Drawing.Size(228, 20);
            this.textBox_cameraControlPort.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(40, 43);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Kamera vezérlés Port";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(55, 109);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Auto kapcsolódás";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radioButton_AutoConnect_Yes
            // 
            this.radioButton_AutoConnect_Yes.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButton_AutoConnect_Yes.AutoSize = true;
            this.radioButton_AutoConnect_Yes.Location = new System.Drawing.Point(3, 5);
            this.radioButton_AutoConnect_Yes.Name = "radioButton_AutoConnect_Yes";
            this.radioButton_AutoConnect_Yes.Size = new System.Drawing.Size(46, 17);
            this.radioButton_AutoConnect_Yes.TabIndex = 21;
            this.radioButton_AutoConnect_Yes.TabStop = true;
            this.radioButton_AutoConnect_Yes.Text = "Igen";
            this.radioButton_AutoConnect_Yes.UseVisualStyleBackColor = true;
            // 
            // radioButton_AutoConnect_No
            // 
            this.radioButton_AutoConnect_No.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButton_AutoConnect_No.AutoSize = true;
            this.radioButton_AutoConnect_No.Location = new System.Drawing.Point(55, 5);
            this.radioButton_AutoConnect_No.Name = "radioButton_AutoConnect_No";
            this.radioButton_AutoConnect_No.Size = new System.Drawing.Size(47, 17);
            this.radioButton_AutoConnect_No.TabIndex = 22;
            this.radioButton_AutoConnect_No.TabStop = true;
            this.radioButton_AutoConnect_No.Text = "Nem";
            this.radioButton_AutoConnect_No.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(19, 142);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(128, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Videó szegmens hossz (s)";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.textBox_StreamUrl, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.comboBox_IrColorMode, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.textBox_cameraControlPort, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.comboBox_coordFormat, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.comboBox_altFormat, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.comboBox_distFormat, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.comboBox_speedFormat, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBox_cameraIp, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 11;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.999999F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(384, 361);
            this.tableLayoutPanel1.TabIndex = 25;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.button_Save, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.button_Cancel, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(153, 333);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(228, 25);
            this.tableLayoutPanel3.TabIndex = 29;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.label13, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.numericUpDown_VideoSegmentLength, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(153, 135);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(228, 27);
            this.tableLayoutPanel2.TabIndex = 30;
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(213, 7);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(12, 13);
            this.label13.TabIndex = 26;
            this.label13.Text = "s";
            // 
            // numericUpDown_VideoSegmentLength
            // 
            this.numericUpDown_VideoSegmentLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown_VideoSegmentLength.Location = new System.Drawing.Point(3, 3);
            this.numericUpDown_VideoSegmentLength.Name = "numericUpDown_VideoSegmentLength";
            this.numericUpDown_VideoSegmentLength.Size = new System.Drawing.Size(204, 20);
            this.numericUpDown_VideoSegmentLength.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.radioButton_AutoConnect_No, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.radioButton_AutoConnect_Yes, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(153, 102);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(228, 27);
            this.tableLayoutPanel4.TabIndex = 31;
            // 
            // textBox_StreamUrl
            // 
            this.textBox_StreamUrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_StreamUrl.Location = new System.Drawing.Point(153, 72);
            this.textBox_StreamUrl.Name = "textBox_StreamUrl";
            this.textBox_StreamUrl.Size = new System.Drawing.Size(228, 20);
            this.textBox_StreamUrl.TabIndex = 32;
            // 
            // SettingForm
            // 
            this.AcceptButton = this.button_Save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button_Cancel;
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MV04 Settings";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_VideoSegmentLength)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox_coordFormat;
        private System.Windows.Forms.ComboBox comboBox_speedFormat;
        private System.Windows.Forms.ComboBox comboBox_distFormat;
        private System.Windows.Forms.ComboBox comboBox_altFormat;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_cameraIp;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox_IrColorMode;
        private System.Windows.Forms.TextBox textBox_cameraControlPort;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RadioButton radioButton_AutoConnect_Yes;
        private System.Windows.Forms.RadioButton radioButton_AutoConnect_No;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NumericUpDown numericUpDown_VideoSegmentLength;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox textBox_StreamUrl;
    }
}