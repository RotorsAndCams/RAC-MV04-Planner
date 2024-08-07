namespace MissionPlanner.GCSViews
{
    partial class CameraFullScreenForm
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
            this.btn_StopTracking = new System.Windows.Forms.Button();
            this.btn_Close = new System.Windows.Forms.Button();
            this.pb_Stream = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Stream)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_StopTracking
            // 
            this.btn_StopTracking.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_StopTracking.BackColor = System.Drawing.Color.Black;
            this.btn_StopTracking.Image = global::MissionPlanner.Properties.Resources.icons8_stop_sign_50;
            this.btn_StopTracking.Location = new System.Drawing.Point(1239, 12);
            this.btn_StopTracking.Name = "btn_StopTracking";
            this.btn_StopTracking.Size = new System.Drawing.Size(85, 80);
            this.btn_StopTracking.TabIndex = 1;
            this.btn_StopTracking.UseVisualStyleBackColor = false;
            this.btn_StopTracking.Click += new System.EventHandler(this.btn_StopTracking_Click);
            // 
            // btn_Close
            // 
            this.btn_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Close.BackColor = System.Drawing.Color.Black;
            this.btn_Close.Image = global::MissionPlanner.Properties.Resources.close_50;
            this.btn_Close.Location = new System.Drawing.Point(1264, 107);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(60, 61);
            this.btn_Close.TabIndex = 0;
            this.btn_Close.UseVisualStyleBackColor = false;
            // 
            // pb_Stream
            // 
            this.pb_Stream.BackColor = System.Drawing.Color.RosyBrown;
            this.pb_Stream.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pb_Stream.Location = new System.Drawing.Point(0, 0);
            this.pb_Stream.Name = "pb_Stream";
            this.pb_Stream.Size = new System.Drawing.Size(1216, 673);
            this.pb_Stream.TabIndex = 2;
            this.pb_Stream.TabStop = false;
            // 
            // CameraFullScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1520, 841);
            this.Controls.Add(this.btn_StopTracking);
            this.Controls.Add(this.pb_Stream);
            this.Controls.Add(this.btn_Close);
            this.DoubleBuffered = true;
            this.Name = "CameraFullScreenForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CameraFullScreenForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.pb_Stream)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.Button btn_StopTracking;
        private System.Windows.Forms.PictureBox pb_Stream;
    }
}