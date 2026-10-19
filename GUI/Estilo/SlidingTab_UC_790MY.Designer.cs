namespace GUI_08YS.Estilo
{
    partial class SlidingTab_UC_790MY
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                tmrDeslizar_790MY?.Stop();
                if (components != null) components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tmrDeslizar_790MY = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            //
            // tmrDeslizar_790MY  (solo corre mientras el indicador se desliza)
            //
            this.tmrDeslizar_790MY.Interval = 15;
            this.tmrDeslizar_790MY.Tick += new System.EventHandler(this.tmrDeslizar_790MY_Tick);
            //
            // SlidingTab_UC_790MY
            //
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.PageTabList;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MinimumSize = new System.Drawing.Size(120, 40);
            this.Name = "SlidingTab_UC_790MY";
            this.Size = new System.Drawing.Size(376, 54);
            this.TabStop = true;
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer tmrDeslizar_790MY;
    }
}
