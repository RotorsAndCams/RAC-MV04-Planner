﻿namespace MissionPlanner.GCSViews
{
    partial class uc_CameraSettings
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
            this.tlp_Base = new System.Windows.Forms.TableLayoutPanel();
            this.tlp_SYAutoStart = new System.Windows.Forms.TableLayoutPanel();
            this.rb_NoSY = new System.Windows.Forms.RadioButton();
            this.rb_YesSY = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rb_AutoRecordNo = new System.Windows.Forms.RadioButton();
            this.rb_AutoRecordYes = new System.Windows.Forms.RadioButton();
            this.comboBox_IrColorMode = new System.Windows.Forms.ComboBox();
            this.textBox_cameraControlPort = new System.Windows.Forms.TextBox();
            this.comboBox_coordFormat = new System.Windows.Forms.ComboBox();
            this.comboBox_altFormat = new System.Windows.Forms.ComboBox();
            this.comboBox_distFormat = new System.Windows.Forms.ComboBox();
            this.comboBox_speedFormat = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_cameraIp = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label13 = new System.Windows.Forms.Label();
            this.numericUpDown_VideoSegmentLength = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButton_AutoConnect_No = new System.Windows.Forms.RadioButton();
            this.radioButton_AutoConnect_Yes = new System.Windows.Forms.RadioButton();
            this.lb_AutoRecord = new System.Windows.Forms.Label();
            this.comboBox_cameraStreamChannel = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_Save = new System.Windows.Forms.Button();
            this.lb_AutoStartSingleYaw = new System.Windows.Forms.Label();
            this.lb_AutoStartCameraStream = new System.Windows.Forms.Label();
            this.tlp_AutoStartCameraStream = new System.Windows.Forms.TableLayoutPanel();
            this.rb_AutoStartCameraStream_No = new System.Windows.Forms.RadioButton();
            this.rb_AutoStartCameraStream_Yes = new System.Windows.Forms.RadioButton();
            this.tlp_Base.SuspendLayout();
            this.tlp_SYAutoStart.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_VideoSegmentLength)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tlp_AutoStartCameraStream.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlp_Base
            // 
            this.tlp_Base.BackColor = System.Drawing.Color.Black;
            this.tlp_Base.ColumnCount = 2;
            this.tlp_Base.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlp_Base.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_Base.Controls.Add(this.tlp_AutoStartCameraStream, 1, 12);
            this.tlp_Base.Controls.Add(this.lb_AutoStartCameraStream, 0, 12);
            this.tlp_Base.Controls.Add(this.tlp_SYAutoStart, 1, 11);
            this.tlp_Base.Controls.Add(this.tableLayoutPanel1, 1, 10);
            this.tlp_Base.Controls.Add(this.comboBox_IrColorMode, 1, 5);
            this.tlp_Base.Controls.Add(this.textBox_cameraControlPort, 1, 2);
            this.tlp_Base.Controls.Add(this.comboBox_coordFormat, 1, 6);
            this.tlp_Base.Controls.Add(this.comboBox_altFormat, 1, 7);
            this.tlp_Base.Controls.Add(this.comboBox_distFormat, 1, 8);
            this.tlp_Base.Controls.Add(this.comboBox_speedFormat, 1, 9);
            this.tlp_Base.Controls.Add(this.label5, 0, 0);
            this.tlp_Base.Controls.Add(this.textBox_cameraIp, 1, 0);
            this.tlp_Base.Controls.Add(this.label11, 0, 4);
            this.tlp_Base.Controls.Add(this.label6, 0, 1);
            this.tlp_Base.Controls.Add(this.label7, 0, 5);
            this.tlp_Base.Controls.Add(this.label4, 0, 9);
            this.tlp_Base.Controls.Add(this.label10, 0, 3);
            this.tlp_Base.Controls.Add(this.label3, 0, 8);
            this.tlp_Base.Controls.Add(this.label1, 0, 6);
            this.tlp_Base.Controls.Add(this.label8, 0, 2);
            this.tlp_Base.Controls.Add(this.label2, 0, 7);
            this.tlp_Base.Controls.Add(this.tableLayoutPanel2, 1, 4);
            this.tlp_Base.Controls.Add(this.tableLayoutPanel4, 1, 3);
            this.tlp_Base.Controls.Add(this.lb_AutoRecord, 0, 10);
            this.tlp_Base.Controls.Add(this.comboBox_cameraStreamChannel, 1, 1);
            this.tlp_Base.Controls.Add(this.lb_AutoStartSingleYaw, 0, 11);
            this.tlp_Base.Controls.Add(this.tableLayoutPanel3, 1, 13);
            this.tlp_Base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_Base.ForeColor = System.Drawing.Color.White;
            this.tlp_Base.Location = new System.Drawing.Point(0, 0);
            this.tlp_Base.Name = "tlp_Base";
            this.tlp_Base.RowCount = 14;
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.90268F));
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.90268F));
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.90268F));
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.90268F));
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.90268F));
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.90268F));
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.90268F));
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.90268F));
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.90268F));
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.90268F));
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.90268F));
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.90268F));
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.900461F));
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.26737F));
            this.tlp_Base.Size = new System.Drawing.Size(400, 422);
            this.tlp_Base.TabIndex = 26;
            // 
            // tlp_SYAutoStart
            // 
            this.tlp_SYAutoStart.ColumnCount = 2;
            this.tlp_SYAutoStart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlp_SYAutoStart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_SYAutoStart.Controls.Add(this.rb_NoSY, 1, 0);
            this.tlp_SYAutoStart.Controls.Add(this.rb_YesSY, 0, 0);
            this.tlp_SYAutoStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_SYAutoStart.Location = new System.Drawing.Point(153, 322);
            this.tlp_SYAutoStart.Name = "tlp_SYAutoStart";
            this.tlp_SYAutoStart.RowCount = 1;
            this.tlp_SYAutoStart.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_SYAutoStart.Size = new System.Drawing.Size(244, 23);
            this.tlp_SYAutoStart.TabIndex = 36;
            // 
            // rb_NoSY
            // 
            this.rb_NoSY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rb_NoSY.AutoSize = true;
            this.rb_NoSY.Location = new System.Drawing.Point(55, 3);
            this.rb_NoSY.Name = "rb_NoSY";
            this.rb_NoSY.Size = new System.Drawing.Size(47, 17);
            this.rb_NoSY.TabIndex = 22;
            this.rb_NoSY.TabStop = true;
            this.rb_NoSY.Text = "Nem";
            this.rb_NoSY.UseVisualStyleBackColor = true;
            // 
            // rb_YesSY
            // 
            this.rb_YesSY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rb_YesSY.AutoSize = true;
            this.rb_YesSY.Location = new System.Drawing.Point(3, 3);
            this.rb_YesSY.Name = "rb_YesSY";
            this.rb_YesSY.Size = new System.Drawing.Size(46, 17);
            this.rb_YesSY.TabIndex = 21;
            this.rb_YesSY.TabStop = true;
            this.rb_YesSY.Text = "Igen";
            this.rb_YesSY.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.rb_AutoRecordNo, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.rb_AutoRecordYes, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(153, 293);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(244, 23);
            this.tableLayoutPanel1.TabIndex = 33;
            // 
            // rb_AutoRecordNo
            // 
            this.rb_AutoRecordNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rb_AutoRecordNo.AutoSize = true;
            this.rb_AutoRecordNo.Location = new System.Drawing.Point(55, 3);
            this.rb_AutoRecordNo.Name = "rb_AutoRecordNo";
            this.rb_AutoRecordNo.Size = new System.Drawing.Size(47, 17);
            this.rb_AutoRecordNo.TabIndex = 22;
            this.rb_AutoRecordNo.TabStop = true;
            this.rb_AutoRecordNo.Text = "Nem";
            this.rb_AutoRecordNo.UseVisualStyleBackColor = true;
            // 
            // rb_AutoRecordYes
            // 
            this.rb_AutoRecordYes.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rb_AutoRecordYes.AutoSize = true;
            this.rb_AutoRecordYes.Location = new System.Drawing.Point(3, 3);
            this.rb_AutoRecordYes.Name = "rb_AutoRecordYes";
            this.rb_AutoRecordYes.Size = new System.Drawing.Size(46, 17);
            this.rb_AutoRecordYes.TabIndex = 21;
            this.rb_AutoRecordYes.TabStop = true;
            this.rb_AutoRecordYes.Text = "Igen";
            this.rb_AutoRecordYes.UseVisualStyleBackColor = true;
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
            this.comboBox_IrColorMode.Location = new System.Drawing.Point(153, 149);
            this.comboBox_IrColorMode.Name = "comboBox_IrColorMode";
            this.comboBox_IrColorMode.Size = new System.Drawing.Size(244, 21);
            this.comboBox_IrColorMode.TabIndex = 15;
            // 
            // textBox_cameraControlPort
            // 
            this.textBox_cameraControlPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_cameraControlPort.Location = new System.Drawing.Point(153, 62);
            this.textBox_cameraControlPort.Name = "textBox_cameraControlPort";
            this.textBox_cameraControlPort.Size = new System.Drawing.Size(244, 20);
            this.textBox_cameraControlPort.TabIndex = 18;
            // 
            // comboBox_coordFormat
            // 
            this.comboBox_coordFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_coordFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_coordFormat.FormattingEnabled = true;
            this.comboBox_coordFormat.Items.AddRange(new object[] {
            "WGS84",
            "MGRS"});
            this.comboBox_coordFormat.Location = new System.Drawing.Point(153, 178);
            this.comboBox_coordFormat.Name = "comboBox_coordFormat";
            this.comboBox_coordFormat.Size = new System.Drawing.Size(244, 21);
            this.comboBox_coordFormat.TabIndex = 6;
            // 
            // comboBox_altFormat
            // 
            this.comboBox_altFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_altFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_altFormat.FormattingEnabled = true;
            this.comboBox_altFormat.Items.AddRange(new object[] {
            "m",
            "ft"});
            this.comboBox_altFormat.Location = new System.Drawing.Point(153, 207);
            this.comboBox_altFormat.Name = "comboBox_altFormat";
            this.comboBox_altFormat.Size = new System.Drawing.Size(244, 21);
            this.comboBox_altFormat.TabIndex = 7;
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
            this.comboBox_distFormat.Location = new System.Drawing.Point(153, 236);
            this.comboBox_distFormat.Name = "comboBox_distFormat";
            this.comboBox_distFormat.Size = new System.Drawing.Size(244, 21);
            this.comboBox_distFormat.TabIndex = 8;
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
            this.comboBox_speedFormat.Location = new System.Drawing.Point(153, 265);
            this.comboBox_speedFormat.Name = "comboBox_speedFormat";
            this.comboBox_speedFormat.Size = new System.Drawing.Size(244, 21);
            this.comboBox_speedFormat.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Kamera IP";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_cameraIp
            // 
            this.textBox_cameraIp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_cameraIp.Location = new System.Drawing.Point(153, 4);
            this.textBox_cameraIp.Name = "textBox_cameraIp";
            this.textBox_cameraIp.Size = new System.Drawing.Size(244, 20);
            this.textBox_cameraIp.TabIndex = 13;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 124);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(128, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Videó szegmens hossz (s)";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Kamera stream Csatorna";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 153);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "IR szín mód";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 269);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Sebesség mértékegység";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 95);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "Auto kapcsolódás";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 240);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Távolság mértékegység";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 182);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Koordináta formátum";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 66);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Kamera vezérlés Port";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 211);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Magasság mértékegység";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.label13, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.numericUpDown_VideoSegmentLength, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(153, 119);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(244, 23);
            this.tableLayoutPanel2.TabIndex = 30;
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(229, 5);
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
            this.numericUpDown_VideoSegmentLength.Size = new System.Drawing.Size(220, 20);
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
            this.tableLayoutPanel4.Location = new System.Drawing.Point(153, 90);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(244, 23);
            this.tableLayoutPanel4.TabIndex = 31;
            // 
            // radioButton_AutoConnect_No
            // 
            this.radioButton_AutoConnect_No.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButton_AutoConnect_No.AutoSize = true;
            this.radioButton_AutoConnect_No.Location = new System.Drawing.Point(55, 3);
            this.radioButton_AutoConnect_No.Name = "radioButton_AutoConnect_No";
            this.radioButton_AutoConnect_No.Size = new System.Drawing.Size(47, 17);
            this.radioButton_AutoConnect_No.TabIndex = 22;
            this.radioButton_AutoConnect_No.TabStop = true;
            this.radioButton_AutoConnect_No.Text = "Nem";
            this.radioButton_AutoConnect_No.UseVisualStyleBackColor = true;
            // 
            // radioButton_AutoConnect_Yes
            // 
            this.radioButton_AutoConnect_Yes.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.radioButton_AutoConnect_Yes.AutoSize = true;
            this.radioButton_AutoConnect_Yes.Location = new System.Drawing.Point(3, 3);
            this.radioButton_AutoConnect_Yes.Name = "radioButton_AutoConnect_Yes";
            this.radioButton_AutoConnect_Yes.Size = new System.Drawing.Size(46, 17);
            this.radioButton_AutoConnect_Yes.TabIndex = 21;
            this.radioButton_AutoConnect_Yes.TabStop = true;
            this.radioButton_AutoConnect_Yes.Text = "Igen";
            this.radioButton_AutoConnect_Yes.UseVisualStyleBackColor = true;
            // 
            // lb_AutoRecord
            // 
            this.lb_AutoRecord.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lb_AutoRecord.AutoSize = true;
            this.lb_AutoRecord.Location = new System.Drawing.Point(3, 298);
            this.lb_AutoRecord.Name = "lb_AutoRecord";
            this.lb_AutoRecord.Size = new System.Drawing.Size(134, 13);
            this.lb_AutoRecord.TabIndex = 32;
            this.lb_AutoRecord.Text = "AutoRecordCameraScreen";
            // 
            // comboBox_cameraStreamChannel
            // 
            this.comboBox_cameraStreamChannel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_cameraStreamChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_cameraStreamChannel.FormattingEnabled = true;
            this.comboBox_cameraStreamChannel.Items.AddRange(new object[] {
            "0",
            "1"});
            this.comboBox_cameraStreamChannel.Location = new System.Drawing.Point(153, 33);
            this.comboBox_cameraStreamChannel.Name = "comboBox_cameraStreamChannel";
            this.comboBox_cameraStreamChannel.Size = new System.Drawing.Size(244, 21);
            this.comboBox_cameraStreamChannel.TabIndex = 34;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.button_Cancel, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.button_Save, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(153, 380);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(244, 39);
            this.tableLayoutPanel3.TabIndex = 29;
            // 
            // button_Cancel
            // 
            this.button_Cancel.BackColor = System.Drawing.Color.Black;
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Cancel.Location = new System.Drawing.Point(125, 3);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(116, 33);
            this.button_Cancel.TabIndex = 1;
            this.button_Cancel.Text = "Reset";
            this.button_Cancel.UseVisualStyleBackColor = false;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // button_Save
            // 
            this.button_Save.BackColor = System.Drawing.Color.Black;
            this.button_Save.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button_Save.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Save.Location = new System.Drawing.Point(3, 3);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(116, 33);
            this.button_Save.TabIndex = 0;
            this.button_Save.Text = "Save";
            this.button_Save.UseVisualStyleBackColor = false;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // lb_AutoStartSingleYaw
            // 
            this.lb_AutoStartSingleYaw.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lb_AutoStartSingleYaw.AutoSize = true;
            this.lb_AutoStartSingleYaw.Location = new System.Drawing.Point(3, 327);
            this.lb_AutoStartSingleYaw.Name = "lb_AutoStartSingleYaw";
            this.lb_AutoStartSingleYaw.Size = new System.Drawing.Size(101, 13);
            this.lb_AutoStartSingleYaw.TabIndex = 35;
            this.lb_AutoStartSingleYaw.Text = "AutoStartSingleYaw";
            // 
            // lb_AutoStartCameraStream
            // 
            this.lb_AutoStartCameraStream.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lb_AutoStartCameraStream.AutoSize = true;
            this.lb_AutoStartCameraStream.Location = new System.Drawing.Point(3, 356);
            this.lb_AutoStartCameraStream.Name = "lb_AutoStartCameraStream";
            this.lb_AutoStartCameraStream.Size = new System.Drawing.Size(120, 13);
            this.lb_AutoStartCameraStream.TabIndex = 37;
            this.lb_AutoStartCameraStream.Text = "AutoStartCameraStream";
            // 
            // tlp_AutoStartCameraStream
            // 
            this.tlp_AutoStartCameraStream.ColumnCount = 2;
            this.tlp_AutoStartCameraStream.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlp_AutoStartCameraStream.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_AutoStartCameraStream.Controls.Add(this.rb_AutoStartCameraStream_No, 1, 0);
            this.tlp_AutoStartCameraStream.Controls.Add(this.rb_AutoStartCameraStream_Yes, 0, 0);
            this.tlp_AutoStartCameraStream.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_AutoStartCameraStream.Location = new System.Drawing.Point(153, 351);
            this.tlp_AutoStartCameraStream.Name = "tlp_AutoStartCameraStream";
            this.tlp_AutoStartCameraStream.RowCount = 1;
            this.tlp_AutoStartCameraStream.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_AutoStartCameraStream.Size = new System.Drawing.Size(244, 23);
            this.tlp_AutoStartCameraStream.TabIndex = 38;
            // 
            // rb_AutoStartCameraStream_No
            // 
            this.rb_AutoStartCameraStream_No.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rb_AutoStartCameraStream_No.AutoSize = true;
            this.rb_AutoStartCameraStream_No.Location = new System.Drawing.Point(55, 3);
            this.rb_AutoStartCameraStream_No.Name = "rb_AutoStartCameraStream_No";
            this.rb_AutoStartCameraStream_No.Size = new System.Drawing.Size(47, 17);
            this.rb_AutoStartCameraStream_No.TabIndex = 22;
            this.rb_AutoStartCameraStream_No.TabStop = true;
            this.rb_AutoStartCameraStream_No.Text = "Nem";
            this.rb_AutoStartCameraStream_No.UseVisualStyleBackColor = true;
            // 
            // rb_AutoStartCameraStream_Yes
            // 
            this.rb_AutoStartCameraStream_Yes.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rb_AutoStartCameraStream_Yes.AutoSize = true;
            this.rb_AutoStartCameraStream_Yes.Location = new System.Drawing.Point(3, 3);
            this.rb_AutoStartCameraStream_Yes.Name = "rb_AutoStartCameraStream_Yes";
            this.rb_AutoStartCameraStream_Yes.Size = new System.Drawing.Size(46, 17);
            this.rb_AutoStartCameraStream_Yes.TabIndex = 21;
            this.rb_AutoStartCameraStream_Yes.TabStop = true;
            this.rb_AutoStartCameraStream_Yes.Text = "Igen";
            this.rb_AutoStartCameraStream_Yes.UseVisualStyleBackColor = true;
            // 
            // uc_CameraSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlp_Base);
            this.Name = "uc_CameraSettings";
            this.Size = new System.Drawing.Size(400, 422);
            this.tlp_Base.ResumeLayout(false);
            this.tlp_Base.PerformLayout();
            this.tlp_SYAutoStart.ResumeLayout(false);
            this.tlp_SYAutoStart.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_VideoSegmentLength)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tlp_AutoStartCameraStream.ResumeLayout(false);
            this.tlp_AutoStartCameraStream.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlp_Base;
        private System.Windows.Forms.ComboBox comboBox_IrColorMode;
        private System.Windows.Forms.TextBox textBox_cameraControlPort;
        private System.Windows.Forms.ComboBox comboBox_coordFormat;
        private System.Windows.Forms.ComboBox comboBox_altFormat;
        private System.Windows.Forms.ComboBox comboBox_distFormat;
        private System.Windows.Forms.ComboBox comboBox_speedFormat;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_cameraIp;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown numericUpDown_VideoSegmentLength;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.RadioButton radioButton_AutoConnect_No;
        private System.Windows.Forms.RadioButton radioButton_AutoConnect_Yes;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton rb_AutoRecordNo;
        private System.Windows.Forms.RadioButton rb_AutoRecordYes;
        private System.Windows.Forms.Label lb_AutoRecord;
        private System.Windows.Forms.ComboBox comboBox_cameraStreamChannel;
        private System.Windows.Forms.TableLayoutPanel tlp_SYAutoStart;
        private System.Windows.Forms.RadioButton rb_NoSY;
        private System.Windows.Forms.RadioButton rb_YesSY;
        private System.Windows.Forms.Label lb_AutoStartSingleYaw;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.TableLayoutPanel tlp_AutoStartCameraStream;
        private System.Windows.Forms.RadioButton rb_AutoStartCameraStream_No;
        private System.Windows.Forms.RadioButton rb_AutoStartCameraStream_Yes;
        private System.Windows.Forms.Label lb_AutoStartCameraStream;
    }
}