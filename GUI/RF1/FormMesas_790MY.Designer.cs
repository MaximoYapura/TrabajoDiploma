namespace GUI_08YS.RF1
{
    partial class FormMesas_790MY
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitulo_790MY = new System.Windows.Forms.Label();
            this.dgvMesas_790MY = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMesas_790MY)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitulo_790MY
            //
            this.lblTitulo_790MY.AutoSize = true;
            this.lblTitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo_790MY.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo_790MY.Name = "lblTitulo_790MY";
            this.lblTitulo_790MY.Size = new System.Drawing.Size(100, 25);
            this.lblTitulo_790MY.Text = "Mesas";
            //
            // dgvMesas_790MY
            //
            this.dgvMesas_790MY.AllowUserToAddRows = false;
            this.dgvMesas_790MY.AllowUserToDeleteRows = false;
            this.dgvMesas_790MY.ReadOnly = true;
            this.dgvMesas_790MY.Location = new System.Drawing.Point(20, 55);
            this.dgvMesas_790MY.Name = "dgvMesas_790MY";
            this.dgvMesas_790MY.Size = new System.Drawing.Size(500, 350);
            this.dgvMesas_790MY.RowHeadersVisible = false;
            //
            // FormMesas_790MY
            //
            this.ClientSize = new System.Drawing.Size(540, 430);
            this.Controls.Add(this.dgvMesas_790MY);
            this.Controls.Add(this.lblTitulo_790MY);
            this.Name = "FormMesas_790MY";
            this.Text = "Mesas";
            this.Load += new System.EventHandler(this.FormMesas_790MY_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMesas_790MY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo_790MY;
        private System.Windows.Forms.DataGridView dgvMesas_790MY;
    }
}
