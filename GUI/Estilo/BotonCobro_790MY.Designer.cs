namespace GUI_08YS.Estilo
{
    partial class BotonCobro_790MY
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                tmrGiro_790MY?.Stop();
                if (components != null) components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tmrGiro_790MY = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            //
            // tmrGiro_790MY  (solo corre en el estado Procesando)
            //
            this.tmrGiro_790MY.Interval = 40;
            this.tmrGiro_790MY.Tick += new System.EventHandler(this.tmrGiro_790MY_Tick);
            //
            // BotonCobro_790MY
            //
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FlatAppearance.BorderSize = 0;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.ForeColor = System.Drawing.Color.White;
            this.UseVisualStyleBackColor = false;
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer tmrGiro_790MY;
    }
}
