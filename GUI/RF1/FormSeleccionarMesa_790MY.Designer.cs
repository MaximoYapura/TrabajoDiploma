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
            this.tableLayoutMain_790MY       = new System.Windows.Forms.TableLayoutPanel();
            this.panelPlano_790MY            = new System.Windows.Forms.Panel();
            this.lblPlanoTitulo_790MY        = new System.Windows.Forms.Label();
            this.pbPlano_790MY               = new System.Windows.Forms.PictureBox();
            this.panelDerecho_790MY          = new System.Windows.Forms.Panel();
            this.lblTituloSeleccionarMesa_790MY = new System.Windows.Forms.Label();
            this.lblSinResultados_790MY      = new System.Windows.Forms.Label();
            this.flowLayoutMesas_790MY       = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAceptar_790MY            = new System.Windows.Forms.Button();
            this.btnCancelar_790MY           = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.pbPlano_790MY)).BeginInit();
            this.tableLayoutMain_790MY.SuspendLayout();
            this.panelPlano_790MY.SuspendLayout();
            this.panelDerecho_790MY.SuspendLayout();
            this.SuspendLayout();

            // ── tableLayoutMain_790MY ──────────────────────────────────────────
            this.tableLayoutMain_790MY.ColumnCount = 2;
            this.tableLayoutMain_790MY.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutMain_790MY.ColumnStyles.Add(
                new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutMain_790MY.RowCount = 1;
            this.tableLayoutMain_790MY.RowStyles.Add(
                new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMain_790MY.Controls.Add(this.panelPlano_790MY,   0, 0);
            this.tableLayoutMain_790MY.Controls.Add(this.panelDerecho_790MY, 1, 0);
            this.tableLayoutMain_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutMain_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutMain_790MY.Name = "tableLayoutMain_790MY";

            // ── panelPlano_790MY (columna izquierda — fondo bordó oscuro) ─────
            this.panelPlano_790MY.BackColor = System.Drawing.Color.FromArgb(56, 17, 17);
            this.panelPlano_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPlano_790MY.Name = "panelPlano_790MY";
            this.panelPlano_790MY.Padding = new System.Windows.Forms.Padding(15, 15, 15, 15);
            this.panelPlano_790MY.Controls.Add(this.pbPlano_790MY);
            this.panelPlano_790MY.Controls.Add(this.lblPlanoTitulo_790MY);

            // ── lblPlanoTitulo_790MY ──────────────────────────────────────────
            this.lblPlanoTitulo_790MY.AutoSize = false;
            this.lblPlanoTitulo_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPlanoTitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblPlanoTitulo_790MY.ForeColor = System.Drawing.Color.FromArgb(253, 246, 227);
            this.lblPlanoTitulo_790MY.Height = 38;
            this.lblPlanoTitulo_790MY.Name = "lblPlanoTitulo_790MY";
            this.lblPlanoTitulo_790MY.Tag = "SM_lblPlano";
            this.lblPlanoTitulo_790MY.Text = "Plano del local";
            this.lblPlanoTitulo_790MY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ── pbPlano_790MY ──────────────────────────────────────────────────
            this.pbPlano_790MY.BackColor = System.Drawing.Color.FromArgb(45, 10, 10);
            this.pbPlano_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbPlano_790MY.Name = "pbPlano_790MY";
            this.pbPlano_790MY.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            // ── panelDerecho_790MY (columna derecha — crema pergamino) ─────────
            this.panelDerecho_790MY.BackColor = System.Drawing.Color.FromArgb(250, 248, 245);
            this.panelDerecho_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDerecho_790MY.Name = "panelDerecho_790MY";
            this.panelDerecho_790MY.Controls.Add(this.btnCancelar_790MY);
            this.panelDerecho_790MY.Controls.Add(this.btnAceptar_790MY);
            this.panelDerecho_790MY.Controls.Add(this.flowLayoutMesas_790MY);
            this.panelDerecho_790MY.Controls.Add(this.lblSinResultados_790MY);
            this.panelDerecho_790MY.Controls.Add(this.lblTituloSeleccionarMesa_790MY);

            // ── lblTituloSeleccionarMesa_790MY ────────────────────────────────
            this.lblTituloSeleccionarMesa_790MY.AutoSize = true;
            this.lblTituloSeleccionarMesa_790MY.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTituloSeleccionarMesa_790MY.ForeColor = System.Drawing.Color.FromArgb(45, 42, 40);
            this.lblTituloSeleccionarMesa_790MY.Location = new System.Drawing.Point(15, 15);
            this.lblTituloSeleccionarMesa_790MY.Name = "lblTituloSeleccionarMesa_790MY";
            this.lblTituloSeleccionarMesa_790MY.Size = new System.Drawing.Size(200, 21);
            this.lblTituloSeleccionarMesa_790MY.TabIndex = 4;
            this.lblTituloSeleccionarMesa_790MY.Tag = "SM_titulo";
            this.lblTituloSeleccionarMesa_790MY.Text = "Seleccionar Mesa";

            // ── lblSinResultados_790MY ────────────────────────────────────────
            this.lblSinResultados_790MY.AutoSize = true;
            this.lblSinResultados_790MY.ForeColor = System.Drawing.Color.FromArgb(120, 25, 25);
            this.lblSinResultados_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblSinResultados_790MY.Location = new System.Drawing.Point(15, 52);
            this.lblSinResultados_790MY.Name = "lblSinResultados_790MY";
            this.lblSinResultados_790MY.TabIndex = 3;
            this.lblSinResultados_790MY.Tag = "SM_lblSinResultados";
            this.lblSinResultados_790MY.Text = "No hay mesas disponibles para ese turno.";
            this.lblSinResultados_790MY.Visible = false;

            // ── flowLayoutMesas_790MY ─────────────────────────────────────────
            this.flowLayoutMesas_790MY.AutoScroll = true;
            this.flowLayoutMesas_790MY.BackColor = System.Drawing.Color.FromArgb(250, 248, 245);
            this.flowLayoutMesas_790MY.Location = new System.Drawing.Point(15, 78);
            this.flowLayoutMesas_790MY.Name = "flowLayoutMesas_790MY";
            this.flowLayoutMesas_790MY.Size = new System.Drawing.Size(388, 378);
            this.flowLayoutMesas_790MY.TabIndex = 0;

            // ── btnAceptar_790MY ──────────────────────────────────────────────
            this.btnAceptar_790MY.BackColor = System.Drawing.Color.FromArgb(120, 25, 25);
            this.btnAceptar_790MY.Enabled = false;
            this.btnAceptar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAceptar_790MY.FlatAppearance.BorderSize = 0;
            this.btnAceptar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnAceptar_790MY.Location = new System.Drawing.Point(205, 472);
            this.btnAceptar_790MY.Name = "btnAceptar_790MY";
            this.btnAceptar_790MY.Size = new System.Drawing.Size(90, 30);
            this.btnAceptar_790MY.TabIndex = 1;
            this.btnAceptar_790MY.Tag = "btn_aceptar";
            this.btnAceptar_790MY.Text = "Aceptar";
            this.btnAceptar_790MY.UseVisualStyleBackColor = false;
            this.btnAceptar_790MY.Click += new System.EventHandler(this.btnAceptar_790MY_Click);

            // ── btnCancelar_790MY ─────────────────────────────────────────────
            this.btnCancelar_790MY.BackColor = System.Drawing.Color.FromArgb(239, 235, 228);
            this.btnCancelar_790MY.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar_790MY.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(120, 25, 25);
            this.btnCancelar_790MY.ForeColor = System.Drawing.Color.FromArgb(45, 30, 20);
            this.btnCancelar_790MY.Location = new System.Drawing.Point(305, 472);
            this.btnCancelar_790MY.Name = "btnCancelar_790MY";
            this.btnCancelar_790MY.Size = new System.Drawing.Size(90, 30);
            this.btnCancelar_790MY.TabIndex = 2;
            this.btnCancelar_790MY.Tag = "btn_cancelar";
            this.btnCancelar_790MY.Text = "Cancelar";
            this.btnCancelar_790MY.UseVisualStyleBackColor = false;
            this.btnCancelar_790MY.Click += new System.EventHandler(this.btnCancelar_790MY_Click);

            // ── FormSeleccionarMesa_790MY ─────────────────────────────────────
            this.AcceptButton = this.btnAceptar_790MY;
            this.BackColor = System.Drawing.Color.FromArgb(250, 248, 245);
            this.CancelButton = this.btnCancelar_790MY;
            this.ClientSize = new System.Drawing.Size(820, 520);
            this.Controls.Add(this.tableLayoutMain_790MY);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSeleccionarMesa_790MY";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Seleccionar Mesa";
            this.Load += new System.EventHandler(this.FormSeleccionarMesa_790MY_Load);

            ((System.ComponentModel.ISupportInitialize)(this.pbPlano_790MY)).EndInit();
            this.tableLayoutMain_790MY.ResumeLayout(false);
            this.panelPlano_790MY.ResumeLayout(false);
            this.panelDerecho_790MY.ResumeLayout(false);
            this.panelDerecho_790MY.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel    tableLayoutMain_790MY;
        private System.Windows.Forms.Panel               panelPlano_790MY;
        private System.Windows.Forms.Label               lblPlanoTitulo_790MY;
        private System.Windows.Forms.PictureBox          pbPlano_790MY;
        private System.Windows.Forms.Panel               panelDerecho_790MY;
        private System.Windows.Forms.Label               lblTituloSeleccionarMesa_790MY;
        private System.Windows.Forms.Label               lblSinResultados_790MY;
        private System.Windows.Forms.FlowLayoutPanel     flowLayoutMesas_790MY;
        private System.Windows.Forms.Button              btnAceptar_790MY;
        private System.Windows.Forms.Button              btnCancelar_790MY;
    }
}
