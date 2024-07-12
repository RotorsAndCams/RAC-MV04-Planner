namespace MV04.Camera
{
    partial class GCSModeTesterForm
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
            this.cb_SelectMode = new System.Windows.Forms.ComboBox();
            this.btn_SetMode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cb_SelectMode
            // 
            this.cb_SelectMode.FormattingEnabled = true;
            this.cb_SelectMode.Location = new System.Drawing.Point(12, 12);
            this.cb_SelectMode.Name = "cb_SelectMode";
            this.cb_SelectMode.Size = new System.Drawing.Size(222, 21);
            this.cb_SelectMode.TabIndex = 0;
            this.cb_SelectMode.SelectedIndexChanged += new System.EventHandler(this.cb_SelectMode_SelectedIndexChanged);
            // 
            // btn_SetMode
            // 
            this.btn_SetMode.Location = new System.Drawing.Point(159, 107);
            this.btn_SetMode.Name = "btn_SetMode";
            this.btn_SetMode.Size = new System.Drawing.Size(75, 23);
            this.btn_SetMode.TabIndex = 1;
            this.btn_SetMode.Text = "Set Mode";
            this.btn_SetMode.UseVisualStyleBackColor = true;
            this.btn_SetMode.Click += new System.EventHandler(this.btn_SetMode_Click);
            // 
            // GCSModeTesterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(246, 142);
            this.Controls.Add(this.btn_SetMode);
            this.Controls.Add(this.cb_SelectMode);
            this.Name = "GCSModeTesterForm";
            this.Text = "GCSModeTesterForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_SelectMode;
        private System.Windows.Forms.Button btn_SetMode;
    }
}