namespace GUI_08YS.RF1
{
    partial class FormSeleccionarMesa_790MY
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
            this.lblSinResultados_790MY = new System.Windows.Forms.Label();
            this.flowLayoutMesas_790MY = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAceptar_790MY = new System.Windows.Forms.Button();
            this.btnCancelar_790MY = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblTitulo_790MY
            //
            this.lblTitulo_790MY.AutoSize = true;
            this.lblTitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo_790MY.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo_790MY.Name = "lblTitulo_790MY";
            this.lblTitulo_790MY.Size = new System.Drawing.Size(160, 25);
            this.lblTitulo_790MY.Text = "Seleccionar Mesa";
            //
            // lblSinResultados_790MY
            //
            this.lblSinResultados_790MY.AutoSize = true;
            this.lblSinResultados_790MY.ForeColor = System.Drawing.Color.Firebrick;
            this.lblSinResultados_790MY.Location = new System.Drawing.Point(20, 60);
            this.lblSinResultados_790MY.Name = "lblSinResultados_790MY";
            this.lblSinResultados_790MY.Size = new System.Drawing.Size(280, 15);
            this.lblSinResultados_790MY.Text = "No hay mesas disponibles para ese turno.";
            this.lblSinResultados_790MY.Visible = false;
            //
            // flowLayoutMesas_790MY
            //
            this.flowLayoutMesas_790MY.AutoScroll = true;
            this.flowLayoutMesas_790MY.Location = new System.Drawing.Point(20, 90);
            this.flowLayoutMesas_790MY.Name = "flowLayoutMesas_790MY";
            this.flowLayoutMesas_790MY.Size = new System.Drawing.Size(560, 320);
            this.flowLayoutMesas_790MY.TabIndex = 0;
            //
            // btnAceptar_790MY
            //
            this.btnAceptar_790MY.Enabled = false;
            this.btnAceptar_790MY.Location = new System.Drawing.Point(390, 430);
            this.btnAceptar_790MY.Name = "btnAceptar_790MY";
            this.btnAceptar_790MY.Size = new System.Drawing.Size(90, 30);
            this.btnAceptar_790MY.TabIndex = 1;
            this.btnAceptar_790MY.Text = "Aceptar";
            this.btnAceptar_790MY.UseVisualStyleBackColor = true;
            this.btnAceptar_790MY.Click += new System.EventHandler(this.btnAceptar_790MY_Click);
            //
            // btnCancelar_790MY
            //
            this.btnCancelar_790MY.Location = new System.Drawing.Point(490, 430);
            this.btnCancelar_790MY.Name = "btnCancelar_790MY";
            this.btnCancelar_790MY.Size = new System.Drawing.Size(90, 30);
            this.btnCancelar_790MY.TabIndex = 2;
            this.btnCancelar_790MY.Text = "Cancelar";
            this.btnCancelar_790MY.UseVisualStyleBackColor = true;
            this.btnCancelar_790MY.Click += new System.EventHandler(this.btnCancelar_790MY_Click);
            //
            // FormSeleccionarMesa_790MY
            //
            this.AcceptButton = this.btnAceptar_790MY;
            this.CancelButton = this.btnCancelar_790MY;
            this.ClientSize = new System.Drawing.Size(600, 480);
            this.Controls.Add(this.btnCancelar_790MY);
            this.Controls.Add(this.btnAceptar_790MY);
            this.Controls.Add(this.flowLayoutMesas_790MY);
            this.Controls.Add(this.lblSinResultados_790MY);
            this.Controls.Add(this.lblTitulo_790MY);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSeleccionarMesa_790MY";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Seleccionar Mesa";
            this.Load += new System.EventHandler(this.FormSeleccionarMesa_790MY_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo_790MY;
        private System.Windows.Forms.Label lblSinResultados_790MY;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutMesas_790MY;
        private System.Windows.Forms.Button btnAceptar_790MY;
        private System.Windows.Forms.Button btnCancelar_790MY;
    }
}
