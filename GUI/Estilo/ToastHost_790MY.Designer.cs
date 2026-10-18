namespace GUI_08YS.Estilo
{
    partial class ToastHost_790MY
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblMensaje_790MY = new System.Windows.Forms.Label();
            this.tmrCierre_790MY = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            //
            // lblMensaje_790MY
            //
            this.lblMensaje_790MY.AutoSize = true;
            this.lblMensaje_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMensaje_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(110)))), ((int)(((byte)(60)))));
            this.lblMensaje_790MY.Location = new System.Drawing.Point(16, 12);
            this.lblMensaje_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.lblMensaje_790MY.MaximumSize = new System.Drawing.Size(360, 0);
            this.lblMensaje_790MY.Name = "lblMensaje_790MY";
            this.lblMensaje_790MY.TabIndex = 0;
            this.lblMensaje_790MY.Click += new System.EventHandler(this.Toast_Click);
            //
            // tmrCierre_790MY
            //
            this.tmrCierre_790MY.Interval = 4000;
            this.tmrCierre_790MY.Tick += new System.EventHandler(this.tmrCierre_790MY_Tick);
            //
            // ToastHost_790MY
            //
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Alert;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(242)))));
            this.Controls.Add(this.lblMensaje_790MY);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ToastHost_790MY";
            this.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            this.Size = new System.Drawing.Size(200, 44);
            this.TabStop = false;
            this.Click += new System.EventHandler(this.Toast_Click);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblMensaje_790MY;
        private System.Windows.Forms.Timer tmrCierre_790MY;
    }
}
