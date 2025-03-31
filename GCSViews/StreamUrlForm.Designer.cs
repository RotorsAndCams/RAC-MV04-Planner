namespace MissionPlanner.GCSViews
{
    partial class StreamUrlForm
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
            this.tb_Url = new System.Windows.Forms.TextBox();
            this.btn_SetUrl = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tb_Url
            // 
            this.tb_Url.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.tb_Url.Location = new System.Drawing.Point(12, 30);
            this.tb_Url.Name = "tb_Url";
            this.tb_Url.Size = new System.Drawing.Size(332, 31);
            this.tb_Url.TabIndex = 0;
            this.tb_Url.Text = "rtsp://192.168.70.203:554/live0";
            // 
            // btn_SetUrl
            // 
            this.btn_SetUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btn_SetUrl.Location = new System.Drawing.Point(350, 12);
            this.btn_SetUrl.Name = "btn_SetUrl";
            this.btn_SetUrl.Size = new System.Drawing.Size(129, 65);
            this.btn_SetUrl.TabIndex = 1;
            this.btn_SetUrl.Text = "Set URL";
            this.btn_SetUrl.UseVisualStyleBackColor = true;
            this.btn_SetUrl.Click += new System.EventHandler(this.btn_SetUrl_Click);
            // 
            // CameraTesterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 86);
            this.Controls.Add(this.btn_SetUrl);
            this.Controls.Add(this.tb_Url);
            this.Name = "StreamUrlForm";
            this.Text = "Stream Url";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_Url;
        private System.Windows.Forms.Button btn_SetUrl;
    }
}