namespace GUI_08YS.Maestros
{
    partial class FormListaSimple_790MY
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
            this.lstValores_790MY = new System.Windows.Forms.ListBox();
            this.txtNuevoValor_790MY = new System.Windows.Forms.TextBox();
            this.btnAgregar_790MY = new System.Windows.Forms.Button();
            this.btnQuitar_790MY = new System.Windows.Forms.Button();
            this.btnAceptar_790MY = new System.Windows.Forms.Button();
            this.btnCancelar_790MY = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblTitulo_790MY
            //
            this.lblTitulo_790MY.AutoSize = true;
            this.lblTitulo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblTitulo_790MY.Location = new System.Drawing.Point(15, 15);
            this.lblTitulo_790MY.Name = "lblTitulo_790MY";
            this.lblTitulo_790MY.Size = new System.Drawing.Size(100, 15);
            this.lblTitulo_790MY.Text = "Editar valores";
            //
            // lstValores_790MY
            //
            this.lstValores_790MY.FormattingEnabled = true;
            this.lstValores_790MY.Location = new System.Drawing.Point(15, 40);
            this.lstValores_790MY.Name = "lstValores_790MY";
            this.lstValores_790MY.Size = new System.Drawing.Size(280, 121);
            //
            // txtNuevoValor_790MY
            //
            this.txtNuevoValor_790MY.Location = new System.Drawing.Point(15, 170);
            this.txtNuevoValor_790MY.Name = "txtNuevoValor_790MY";
            this.txtNuevoValor_790MY.Size = new System.Drawing.Size(200, 20);
            //
            // btnAgregar_790MY
            //
            this.btnAgregar_790MY.Location = new System.Drawing.Point(220, 168);
            this.btnAgregar_790MY.Name = "btnAgregar_790MY";
            this.btnAgregar_790MY.Size = new System.Drawing.Size(75, 24);
            this.btnAgregar_790MY.Text = "Agregar";
            this.btnAgregar_790MY.Tag = "btn_agregar";
            this.btnAgregar_790MY.UseVisualStyleBackColor = true;
            this.btnAgregar_790MY.Click += new System.EventHandler(this.btnAgregar_790MY_Click);
            //
            // btnQuitar_790MY
            //
            this.btnQuitar_790MY.Location = new System.Drawing.Point(15, 200);
            this.btnQuitar_790MY.Name = "btnQuitar_790MY";
            this.btnQuitar_790MY.Size = new System.Drawing.Size(90, 24);
            this.btnQuitar_790MY.Text = "Quitar";
            this.btnQuitar_790MY.Tag = "btn_quitar";
            this.btnQuitar_790MY.UseVisualStyleBackColor = true;
            this.btnQuitar_790MY.Click += new System.EventHandler(this.btnQuitar_790MY_Click);
            //
            // btnAceptar_790MY
            //
            this.btnAceptar_790MY.Location = new System.Drawing.Point(140, 230);
            this.btnAceptar_790MY.Name = "btnAceptar_790MY";
            this.btnAceptar_790MY.Size = new System.Drawing.Size(75, 26);
            this.btnAceptar_790MY.Text = "Aceptar";
            this.btnAceptar_790MY.Tag = "btn_aceptar";
            this.btnAceptar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAceptar_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnAceptar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnAceptar_790MY.UseVisualStyleBackColor = false;
            this.btnAceptar_790MY.Click += new System.EventHandler(this.btnAceptar_790MY_Click);
            //
            // btnCancelar_790MY
            //
            this.btnCancelar_790MY.Location = new System.Drawing.Point(220, 230);
            this.btnCancelar_790MY.Name = "btnCancelar_790MY";
            this.btnCancelar_790MY.Size = new System.Drawing.Size(75, 26);
            this.btnCancelar_790MY.Text = "Cancelar";
            this.btnCancelar_790MY.Tag = "btn_cancelar";
            this.btnCancelar_790MY.UseVisualStyleBackColor = true;
            this.btnCancelar_790MY.Click += new System.EventHandler(this.btnCancelar_790MY_Click);
            //
            // FormListaSimple_790MY
            //
            this.AcceptButton = this.btnAceptar_790MY;
            this.CancelButton = this.btnCancelar_790MY;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(310, 270);
            this.Controls.Add(this.btnCancelar_790MY);
            this.Controls.Add(this.btnAceptar_790MY);
            this.Controls.Add(this.btnQuitar_790MY);
            this.Controls.Add(this.btnAgregar_790MY);
            this.Controls.Add(this.txtNuevoValor_790MY);
            this.Controls.Add(this.lstValores_790MY);
            this.Controls.Add(this.lblTitulo_790MY);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormListaSimple_790MY";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Editar valores";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo_790MY;
        private System.Windows.Forms.ListBox lstValores_790MY;
        private System.Windows.Forms.TextBox txtNuevoValor_790MY;
        private System.Windows.Forms.Button btnAgregar_790MY;
        private System.Windows.Forms.Button btnQuitar_790MY;
        private System.Windows.Forms.Button btnAceptar_790MY;
        private System.Windows.Forms.Button btnCancelar_790MY;
    }
}
