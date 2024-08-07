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
            this.pnl_CameraView = new System.Windows.Forms.Panel();
            this.btn_StopTracking = new System.Windows.Forms.Button();
            this.btn_Close = new System.Windows.Forms.Button();
            this.pnl_CameraView.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl_CameraView
            // 
            this.pnl_CameraView.BackColor = System.Drawing.Color.DimGray;
            this.pnl_CameraView.Controls.Add(this.btn_StopTracking);
            this.pnl_CameraView.Controls.Add(this.btn_Close);
            this.pnl_CameraView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_CameraView.Location = new System.Drawing.Point(0, 0);
            this.pnl_CameraView.Name = "pnl_CameraView";
            this.pnl_CameraView.Size = new System.Drawing.Size(1904, 1061);
            this.pnl_CameraView.TabIndex = 0;
            // 
            // btn_StopTracking
            // 
            this.btn_StopTracking.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_StopTracking.BackColor = System.Drawing.Color.Black;
            this.btn_StopTracking.Image = global::MissionPlanner.Properties.Resources.icons8_stop_sign_50;
            this.btn_StopTracking.Location = new System.Drawing.Point(1819, 981);
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
            this.btn_Close.Location = new System.Drawing.Point(1844, 0);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(60, 61);
            this.btn_Close.TabIndex = 0;
            this.btn_Close.UseVisualStyleBackColor = false;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // CameraFullScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1904, 1061);
            this.Controls.Add(this.pnl_CameraView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CameraFullScreenForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CameraFullScreenForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnl_CameraView.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_CameraView;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.Button btn_StopTracking;
    }
}