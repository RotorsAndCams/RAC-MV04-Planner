namespace MissionPlanner.GCSViews
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
            this.tb_StreamUrl = new System.Windows.Forms.TextBox();
            this.tlp_AutoStartCameraStream = new System.Windows.Forms.TableLayoutPanel();
            this.rb_AutoStartCameraStream_No = new System.Windows.Forms.RadioButton();
            this.rb_AutoStartCameraStream_Yes = new System.Windows.Forms.RadioButton();
            this.numericUpDown_VideoSegmentLength = new System.Windows.Forms.NumericUpDown();
            this.lb_AutoStartCameraStream = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label13 = new System.Windows.Forms.Label();
            this.comboBox_distFormat = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.button_Save = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox_IrColorMode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_coordFormat = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_altFormat = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_speedFormat = new System.Windows.Forms.ComboBox();
            this.lb_AutoRecord = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rb_AutoRecordNo = new System.Windows.Forms.RadioButton();
            this.rb_AutoRecordYes = new System.Windows.Forms.RadioButton();
            this.lb_AutoConnectCam = new System.Windows.Forms.Label();
            this.tlp_AutoConnCam = new System.Windows.Forms.TableLayoutPanel();
            this.radioButton_AutoConnect_No = new System.Windows.Forms.RadioButton();
            this.radioButton_AutoConnect_Yes = new System.Windows.Forms.RadioButton();
            this.tb_CamPort = new System.Windows.Forms.TextBox();
            this.lb_CamPort = new System.Windows.Forms.Label();
            this.lb_CamStream = new System.Windows.Forms.Label();
            this.lb_CamIP = new System.Windows.Forms.Label();
            this.tb_CamIP = new System.Windows.Forms.TextBox();
            this.tlp_Base.SuspendLayout();
            this.tlp_AutoStartCameraStream.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_VideoSegmentLength)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tlp_AutoConnCam.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlp_Base
            // 
            this.tlp_Base.BackColor = System.Drawing.Color.Black;
            this.tlp_Base.ColumnCount = 2;
            this.tlp_Base.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlp_Base.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_Base.Controls.Add(this.tb_StreamUrl, 1, 9);
            this.tlp_Base.Controls.Add(this.tlp_AutoStartCameraStream, 1, 11);
            this.tlp_Base.Controls.Add(this.numericUpDown_VideoSegmentLength, 1, 0);
            this.tlp_Base.Controls.Add(this.lb_AutoStartCameraStream, 0, 11);
            this.tlp_Base.Controls.Add(this.tableLayoutPanel2, 1, 4);
            this.tlp_Base.Controls.Add(this.tableLayoutPanel3, 1, 12);
            this.tlp_Base.Controls.Add(this.label11, 0, 0);
            this.tlp_Base.Controls.Add(this.label7, 0, 1);
            this.tlp_Base.Controls.Add(this.comboBox_IrColorMode, 1, 1);
            this.tlp_Base.Controls.Add(this.label1, 0, 2);
            this.tlp_Base.Controls.Add(this.comboBox_coordFormat, 1, 2);
            this.tlp_Base.Controls.Add(this.label2, 0, 3);
            this.tlp_Base.Controls.Add(this.comboBox_altFormat, 1, 3);
            this.tlp_Base.Controls.Add(this.label3, 0, 4);
            this.tlp_Base.Controls.Add(this.label4, 0, 5);
            this.tlp_Base.Controls.Add(this.comboBox_speedFormat, 1, 5);
            this.tlp_Base.Controls.Add(this.lb_AutoRecord, 0, 6);
            this.tlp_Base.Controls.Add(this.tableLayoutPanel1, 1, 6);
            this.tlp_Base.Controls.Add(this.lb_AutoConnectCam, 0, 10);
            this.tlp_Base.Controls.Add(this.tlp_AutoConnCam, 1, 10);
            this.tlp_Base.Controls.Add(this.tb_CamPort, 1, 8);
            this.tlp_Base.Controls.Add(this.lb_CamPort, 0, 8);
            this.tlp_Base.Controls.Add(this.lb_CamStream, 0, 9);
            this.tlp_Base.Controls.Add(this.lb_CamIP, 0, 7);
            this.tlp_Base.Controls.Add(this.tb_CamIP, 1, 7);
            this.tlp_Base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_Base.ForeColor = System.Drawing.Color.White;
            this.tlp_Base.Location = new System.Drawing.Point(0, 0);
            this.tlp_Base.Name = "tlp_Base";
            this.tlp_Base.RowCount = 13;
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
            // tb_StreamUrl
            // 
            this.tb_StreamUrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_StreamUrl.Location = new System.Drawing.Point(153, 284);
            this.tb_StreamUrl.Name = "tb_StreamUrl";
            this.tb_StreamUrl.Size = new System.Drawing.Size(244, 20);
            this.tb_StreamUrl.TabIndex = 39;
            this.tb_StreamUrl.Visible = false;
            // 
            // tlp_AutoStartCameraStream
            // 
            this.tlp_AutoStartCameraStream.ColumnCount = 2;
            this.tlp_AutoStartCameraStream.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlp_AutoStartCameraStream.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_AutoStartCameraStream.Controls.Add(this.rb_AutoStartCameraStream_No, 1, 0);
            this.tlp_AutoStartCameraStream.Controls.Add(this.rb_AutoStartCameraStream_Yes, 0, 0);
            this.tlp_AutoStartCameraStream.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_AutoStartCameraStream.Location = new System.Drawing.Point(153, 344);
            this.tlp_AutoStartCameraStream.Name = "tlp_AutoStartCameraStream";
            this.tlp_AutoStartCameraStream.RowCount = 1;
            this.tlp_AutoStartCameraStream.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_AutoStartCameraStream.Size = new System.Drawing.Size(244, 25);
            this.tlp_AutoStartCameraStream.TabIndex = 38;
            this.tlp_AutoStartCameraStream.Visible = false;
            // 
            // rb_AutoStartCameraStream_No
            // 
            this.rb_AutoStartCameraStream_No.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rb_AutoStartCameraStream_No.AutoSize = true;
            this.rb_AutoStartCameraStream_No.Location = new System.Drawing.Point(55, 4);
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
            this.rb_AutoStartCameraStream_Yes.Location = new System.Drawing.Point(3, 4);
            this.rb_AutoStartCameraStream_Yes.Name = "rb_AutoStartCameraStream_Yes";
            this.rb_AutoStartCameraStream_Yes.Size = new System.Drawing.Size(46, 17);
            this.rb_AutoStartCameraStream_Yes.TabIndex = 21;
            this.rb_AutoStartCameraStream_Yes.TabStop = true;
            this.rb_AutoStartCameraStream_Yes.Text = "Igen";
            this.rb_AutoStartCameraStream_Yes.UseVisualStyleBackColor = true;
            // 
            // numericUpDown_VideoSegmentLength
            // 
            this.numericUpDown_VideoSegmentLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown_VideoSegmentLength.Location = new System.Drawing.Point(153, 5);
            this.numericUpDown_VideoSegmentLength.Name = "numericUpDown_VideoSegmentLength";
            this.numericUpDown_VideoSegmentLength.Size = new System.Drawing.Size(244, 20);
            this.numericUpDown_VideoSegmentLength.TabIndex = 0;
            // 
            // lb_AutoStartCameraStream
            // 
            this.lb_AutoStartCameraStream.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lb_AutoStartCameraStream.AutoSize = true;
            this.lb_AutoStartCameraStream.Location = new System.Drawing.Point(3, 350);
            this.lb_AutoStartCameraStream.Name = "lb_AutoStartCameraStream";
            this.lb_AutoStartCameraStream.Size = new System.Drawing.Size(120, 13);
            this.lb_AutoStartCameraStream.TabIndex = 37;
            this.lb_AutoStartCameraStream.Text = "AutoStartCameraStream";
            this.lb_AutoStartCameraStream.Visible = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.label13, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.comboBox_distFormat, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(153, 127);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(244, 25);
            this.tableLayoutPanel2.TabIndex = 30;
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(229, 6);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(12, 13);
            this.label13.TabIndex = 26;
            this.label13.Text = "s";
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
            this.comboBox_distFormat.Location = new System.Drawing.Point(3, 3);
            this.comboBox_distFormat.Name = "comboBox_distFormat";
            this.comboBox_distFormat.Size = new System.Drawing.Size(220, 21);
            this.comboBox_distFormat.TabIndex = 8;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.button_Cancel, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.button_Save, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(153, 375);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(244, 44);
            this.tableLayoutPanel3.TabIndex = 29;
            // 
            // button_Cancel
            // 
            this.button_Cancel.BackColor = System.Drawing.Color.Black;
            this.button_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button_Cancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_Cancel.Location = new System.Drawing.Point(125, 3);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(116, 38);
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
            this.button_Save.Size = new System.Drawing.Size(116, 38);
            this.button_Save.TabIndex = 0;
            this.button_Save.Text = "Save";
            this.button_Save.UseVisualStyleBackColor = false;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(128, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Videó szegmens hossz (s)";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 40);
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
            this.comboBox_IrColorMode.Location = new System.Drawing.Point(153, 36);
            this.comboBox_IrColorMode.Name = "comboBox_IrColorMode";
            this.comboBox_IrColorMode.Size = new System.Drawing.Size(244, 21);
            this.comboBox_IrColorMode.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Koordináta formátum";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox_coordFormat
            // 
            this.comboBox_coordFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_coordFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_coordFormat.FormattingEnabled = true;
            this.comboBox_coordFormat.Items.AddRange(new object[] {
            "WGS84",
            "MGRS"});
            this.comboBox_coordFormat.Location = new System.Drawing.Point(153, 67);
            this.comboBox_coordFormat.Name = "comboBox_coordFormat";
            this.comboBox_coordFormat.Size = new System.Drawing.Size(244, 21);
            this.comboBox_coordFormat.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Magasság mértékegység";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox_altFormat
            // 
            this.comboBox_altFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_altFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_altFormat.FormattingEnabled = true;
            this.comboBox_altFormat.Items.AddRange(new object[] {
            "m",
            "ft"});
            this.comboBox_altFormat.Location = new System.Drawing.Point(153, 98);
            this.comboBox_altFormat.Name = "comboBox_altFormat";
            this.comboBox_altFormat.Size = new System.Drawing.Size(244, 21);
            this.comboBox_altFormat.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Távolság mértékegység";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 164);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Sebesség mértékegység";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            this.comboBox_speedFormat.Location = new System.Drawing.Point(153, 160);
            this.comboBox_speedFormat.Name = "comboBox_speedFormat";
            this.comboBox_speedFormat.Size = new System.Drawing.Size(244, 21);
            this.comboBox_speedFormat.TabIndex = 9;
            // 
            // lb_AutoRecord
            // 
            this.lb_AutoRecord.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lb_AutoRecord.AutoSize = true;
            this.lb_AutoRecord.Location = new System.Drawing.Point(3, 195);
            this.lb_AutoRecord.Name = "lb_AutoRecord";
            this.lb_AutoRecord.Size = new System.Drawing.Size(134, 13);
            this.lb_AutoRecord.TabIndex = 32;
            this.lb_AutoRecord.Text = "AutoRecordCameraScreen";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.rb_AutoRecordNo, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.rb_AutoRecordYes, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(153, 189);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(244, 25);
            this.tableLayoutPanel1.TabIndex = 33;
            // 
            // rb_AutoRecordNo
            // 
            this.rb_AutoRecordNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.rb_AutoRecordNo.AutoSize = true;
            this.rb_AutoRecordNo.Location = new System.Drawing.Point(55, 4);
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
            this.rb_AutoRecordYes.Location = new System.Drawing.Point(3, 4);
            this.rb_AutoRecordYes.Name = "rb_AutoRecordYes";
            this.rb_AutoRecordYes.Size = new System.Drawing.Size(46, 17);
            this.rb_AutoRecordYes.TabIndex = 21;
            this.rb_AutoRecordYes.TabStop = true;
            this.rb_AutoRecordYes.Text = "Igen";
            this.rb_AutoRecordYes.UseVisualStyleBackColor = true;
            // 
            // lb_AutoConnectCam
            // 
            this.lb_AutoConnectCam.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lb_AutoConnectCam.AutoSize = true;
            this.lb_AutoConnectCam.Location = new System.Drawing.Point(3, 319);
            this.lb_AutoConnectCam.Name = "lb_AutoConnectCam";
            this.lb_AutoConnectCam.Size = new System.Drawing.Size(92, 13);
            this.lb_AutoConnectCam.TabIndex = 20;
            this.lb_AutoConnectCam.Text = "Auto kapcsolódás";
            this.lb_AutoConnectCam.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lb_AutoConnectCam.Visible = false;
            // 
            // tlp_AutoConnCam
            // 
            this.tlp_AutoConnCam.ColumnCount = 2;
            this.tlp_AutoConnCam.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlp_AutoConnCam.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_AutoConnCam.Controls.Add(this.radioButton_AutoConnect_No, 1, 0);
            this.tlp_AutoConnCam.Controls.Add(this.radioButton_AutoConnect_Yes, 0, 0);
            this.tlp_AutoConnCam.Location = new System.Drawing.Point(153, 313);
            this.tlp_AutoConnCam.Name = "tlp_AutoConnCam";
            this.tlp_AutoConnCam.RowCount = 1;
            this.tlp_AutoConnCam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_AutoConnCam.Size = new System.Drawing.Size(244, 23);
            this.tlp_AutoConnCam.TabIndex = 31;
            this.tlp_AutoConnCam.Visible = false;
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
            // tb_CamPort
            // 
            this.tb_CamPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_CamPort.Location = new System.Drawing.Point(153, 253);
            this.tb_CamPort.Name = "tb_CamPort";
            this.tb_CamPort.Size = new System.Drawing.Size(244, 20);
            this.tb_CamPort.TabIndex = 18;
            this.tb_CamPort.Visible = false;
            // 
            // lb_CamPort
            // 
            this.lb_CamPort.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lb_CamPort.AutoSize = true;
            this.lb_CamPort.Location = new System.Drawing.Point(3, 257);
            this.lb_CamPort.Name = "lb_CamPort";
            this.lb_CamPort.Size = new System.Drawing.Size(107, 13);
            this.lb_CamPort.TabIndex = 17;
            this.lb_CamPort.Text = "Kamera vezérlés Port";
            this.lb_CamPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lb_CamPort.Visible = false;
            // 
            // lb_CamStream
            // 
            this.lb_CamStream.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lb_CamStream.AutoSize = true;
            this.lb_CamStream.Location = new System.Drawing.Point(3, 288);
            this.lb_CamStream.Name = "lb_CamStream";
            this.lb_CamStream.Size = new System.Drawing.Size(99, 13);
            this.lb_CamStream.TabIndex = 11;
            this.lb_CamStream.Text = "Kamera streamURL";
            this.lb_CamStream.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lb_CamStream.Visible = false;
            // 
            // lb_CamIP
            // 
            this.lb_CamIP.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lb_CamIP.AutoSize = true;
            this.lb_CamIP.Location = new System.Drawing.Point(3, 226);
            this.lb_CamIP.Name = "lb_CamIP";
            this.lb_CamIP.Size = new System.Drawing.Size(56, 13);
            this.lb_CamIP.TabIndex = 10;
            this.lb_CamIP.Text = "Kamera IP";
            this.lb_CamIP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lb_CamIP.Visible = false;
            // 
            // tb_CamIP
            // 
            this.tb_CamIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_CamIP.Location = new System.Drawing.Point(153, 222);
            this.tb_CamIP.Name = "tb_CamIP";
            this.tb_CamIP.Size = new System.Drawing.Size(244, 20);
            this.tb_CamIP.TabIndex = 13;
            this.tb_CamIP.Visible = false;
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
            this.tlp_AutoStartCameraStream.ResumeLayout(false);
            this.tlp_AutoStartCameraStream.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_VideoSegmentLength)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tlp_AutoConnCam.ResumeLayout(false);
            this.tlp_AutoConnCam.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlp_Base;
        private System.Windows.Forms.ComboBox comboBox_IrColorMode;
        private System.Windows.Forms.TextBox tb_CamPort;
        private System.Windows.Forms.ComboBox comboBox_coordFormat;
        private System.Windows.Forms.ComboBox comboBox_altFormat;
        private System.Windows.Forms.ComboBox comboBox_distFormat;
        private System.Windows.Forms.ComboBox comboBox_speedFormat;
        private System.Windows.Forms.Label lb_CamIP;
        private System.Windows.Forms.TextBox tb_CamIP;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lb_CamStream;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lb_AutoConnectCam;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lb_CamPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown numericUpDown_VideoSegmentLength;
        private System.Windows.Forms.TableLayoutPanel tlp_AutoConnCam;
        private System.Windows.Forms.RadioButton radioButton_AutoConnect_No;
        private System.Windows.Forms.RadioButton radioButton_AutoConnect_Yes;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton rb_AutoRecordNo;
        private System.Windows.Forms.RadioButton rb_AutoRecordYes;
        private System.Windows.Forms.Label lb_AutoRecord;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.TableLayoutPanel tlp_AutoStartCameraStream;
        private System.Windows.Forms.RadioButton rb_AutoStartCameraStream_No;
        private System.Windows.Forms.RadioButton rb_AutoStartCameraStream_Yes;
        private System.Windows.Forms.Label lb_AutoStartCameraStream;
        private System.Windows.Forms.TextBox tb_StreamUrl;
    }
}
