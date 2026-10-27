namespace GUI_08YS.Estilo
{
    partial class AsistenteMaitre_UC_790MY
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.picPersonaje_790MY = new System.Windows.Forms.PictureBox();
            this.pnlGlobo_790MY = new System.Windows.Forms.Panel();
            this.tlpGlobo_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitulo_790MY = new System.Windows.Forms.Label();
            this.lblMensaje_790MY = new System.Windows.Forms.Label();
            this.pnlSeparador_790MY = new System.Windows.Forms.Panel();
            this.lnkSiguiente_790MY = new System.Windows.Forms.LinkLabel();
            this.picPersonajeCompacto_790MY = new System.Windows.Forms.PictureBox();
            this.pnlAviso_790MY = new System.Windows.Forms.Panel();
            this.lblAviso_790MY = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picPersonaje_790MY)).BeginInit();
            this.pnlGlobo_790MY.SuspendLayout();
            this.tlpGlobo_790MY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPersonajeCompacto_790MY)).BeginInit();
            this.pnlAviso_790MY.SuspendLayout();
            this.SuspendLayout();
            //
            // picPersonaje_790MY  (modo completo: 250 x 432, abajo a la derecha del control.
            // La pose la dibuja Personaje_Paint con escala Zoom apoyada en el borde inferior)
            //
            this.picPersonaje_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.picPersonaje_790MY.Location = new System.Drawing.Point(350, 0);
            this.picPersonaje_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.picPersonaje_790MY.Name = "picPersonaje_790MY";
            this.picPersonaje_790MY.Size = new System.Drawing.Size(250, 432);
            this.picPersonaje_790MY.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPersonaje_790MY.TabIndex = 0;
            this.picPersonaje_790MY.TabStop = false;
            this.picPersonaje_790MY.Paint += new System.Windows.Forms.PaintEventHandler(this.Personaje_Paint);
            //
            // pnlGlobo_790MY  (336 px de cuerpo + 12 px de cola; alto automático 72 a 180.
            // Queda 14 px a la izquierda del personaje y 156 px más abajo que su borde superior)
            //
            this.pnlGlobo_790MY.AutoSize = true;
            this.pnlGlobo_790MY.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlGlobo_790MY.Controls.Add(this.tlpGlobo_790MY);
            this.pnlGlobo_790MY.Location = new System.Drawing.Point(0, 156);
            this.pnlGlobo_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.pnlGlobo_790MY.MaximumSize = new System.Drawing.Size(348, 180);
            this.pnlGlobo_790MY.MinimumSize = new System.Drawing.Size(348, 72);
            this.pnlGlobo_790MY.Name = "pnlGlobo_790MY";
            this.pnlGlobo_790MY.Padding = new System.Windows.Forms.Padding(16, 12, 28, 10);
            this.pnlGlobo_790MY.Size = new System.Drawing.Size(348, 120);
            this.pnlGlobo_790MY.TabIndex = 1;
            this.pnlGlobo_790MY.SizeChanged += new System.EventHandler(this.pnlGlobo_790MY_SizeChanged);
            this.pnlGlobo_790MY.Paint += new System.Windows.Forms.PaintEventHandler(this.Globo_Paint);
            //
            // tlpGlobo_790MY  (título; mensaje; separador; «Siguiente consejo →»)
            //
            this.tlpGlobo_790MY.AutoSize = true;
            this.tlpGlobo_790MY.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpGlobo_790MY.BackColor = GUI_08YS.Estilo.TemaFogon_790MY.MaitreGlobo;
            this.tlpGlobo_790MY.ColumnCount = 1;
            this.tlpGlobo_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpGlobo_790MY.Controls.Add(this.lblTitulo_790MY, 0, 0);
            this.tlpGlobo_790MY.Controls.Add(this.lblMensaje_790MY, 0, 1);
            this.tlpGlobo_790MY.Controls.Add(this.pnlSeparador_790MY, 0, 2);
            this.tlpGlobo_790MY.Controls.Add(this.lnkSiguiente_790MY, 0, 3);
            this.tlpGlobo_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpGlobo_790MY.Location = new System.Drawing.Point(16, 12);
            this.tlpGlobo_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.tlpGlobo_790MY.Name = "tlpGlobo_790MY";
            this.tlpGlobo_790MY.RowCount = 4;
            this.tlpGlobo_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpGlobo_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpGlobo_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpGlobo_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpGlobo_790MY.Size = new System.Drawing.Size(304, 98);
            this.tlpGlobo_790MY.TabIndex = 0;
            //
            // lblTitulo_790MY
            //
            this.lblTitulo_790MY.AutoSize = true;
            this.lblTitulo_790MY.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo_790MY.ForeColor = GUI_08YS.Estilo.TemaFogon_790MY.Crema;
            this.lblTitulo_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblTitulo_790MY.MaximumSize = new System.Drawing.Size(304, 0);
            this.lblTitulo_790MY.Name = "lblTitulo_790MY";
            this.lblTitulo_790MY.Size = new System.Drawing.Size(0, 20);
            this.lblTitulo_790MY.TabIndex = 0;
            this.lblTitulo_790MY.UseMnemonic = false;
            //
            // lblMensaje_790MY  (hasta 4 líneas; el .cs corta con «…» si el texto no entra)
            //
            this.lblMensaje_790MY.AutoSize = true;
            this.lblMensaje_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensaje_790MY.ForeColor = GUI_08YS.Estilo.TemaFogon_790MY.MaitreTexto;
            this.lblMensaje_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 9);
            this.lblMensaje_790MY.MaximumSize = new System.Drawing.Size(304, 0);
            this.lblMensaje_790MY.Name = "lblMensaje_790MY";
            this.lblMensaje_790MY.Size = new System.Drawing.Size(0, 17);
            this.lblMensaje_790MY.TabIndex = 1;
            this.lblMensaje_790MY.UseMnemonic = false;
            //
            // pnlSeparador_790MY  (línea de 1 px entre el mensaje y el enlace)
            //
            this.pnlSeparador_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSeparador_790MY.BackColor = GUI_08YS.Estilo.TemaFogon_790MY.MaitreSeparador;
            this.pnlSeparador_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 7);
            this.pnlSeparador_790MY.Name = "pnlSeparador_790MY";
            this.pnlSeparador_790MY.Size = new System.Drawing.Size(304, 1);
            this.pnlSeparador_790MY.TabIndex = 2;
            //
            // lnkSiguiente_790MY
            //
            this.lnkSiguiente_790MY.ActiveLinkColor = GUI_08YS.Estilo.TemaFogon_790MY.Oro;
            this.lnkSiguiente_790MY.AutoSize = true;
            this.lnkSiguiente_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkSiguiente_790MY.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkSiguiente_790MY.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkSiguiente_790MY.LinkColor = GUI_08YS.Estilo.TemaFogon_790MY.Oro;
            this.lnkSiguiente_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.lnkSiguiente_790MY.Name = "lnkSiguiente_790MY";
            this.lnkSiguiente_790MY.Size = new System.Drawing.Size(0, 15);
            this.lnkSiguiente_790MY.TabIndex = 3;
            this.lnkSiguiente_790MY.TabStop = true;
            this.lnkSiguiente_790MY.UseMnemonic = false;
            this.lnkSiguiente_790MY.VisitedLinkColor = GUI_08YS.Estilo.TemaFogon_790MY.Oro;
            this.lnkSiguiente_790MY.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSiguiente_790MY_LinkClicked);
            //
            // picPersonajeCompacto_790MY  (modo compacto: ~200 px de ancho, misma base)
            //
            this.picPersonajeCompacto_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.picPersonajeCompacto_790MY.Location = new System.Drawing.Point(400, 86);
            this.picPersonajeCompacto_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.picPersonajeCompacto_790MY.Name = "picPersonajeCompacto_790MY";
            this.picPersonajeCompacto_790MY.Size = new System.Drawing.Size(200, 346);
            this.picPersonajeCompacto_790MY.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPersonajeCompacto_790MY.TabIndex = 2;
            this.picPersonajeCompacto_790MY.TabStop = false;
            this.picPersonajeCompacto_790MY.Visible = false;
            this.picPersonajeCompacto_790MY.Paint += new System.Windows.Forms.PaintEventHandler(this.Personaje_Paint);
            //
            // pnlAviso_790MY  (modo compacto: aviso de una línea, 300 px + 12 px de cola;
            // un clic pasa al siguiente consejo)
            //
            this.pnlAviso_790MY.Controls.Add(this.lblAviso_790MY);
            this.pnlAviso_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlAviso_790MY.Location = new System.Drawing.Point(86, 211);
            this.pnlAviso_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.pnlAviso_790MY.Name = "pnlAviso_790MY";
            this.pnlAviso_790MY.Padding = new System.Windows.Forms.Padding(14, 10, 26, 10);
            this.pnlAviso_790MY.Size = new System.Drawing.Size(312, 40);
            this.pnlAviso_790MY.TabIndex = 3;
            this.pnlAviso_790MY.Visible = false;
            this.pnlAviso_790MY.Click += new System.EventHandler(this.Aviso_Click);
            this.pnlAviso_790MY.Paint += new System.Windows.Forms.PaintEventHandler(this.Globo_Paint);
            //
            // lblAviso_790MY
            //
            this.lblAviso_790MY.AutoEllipsis = true;
            this.lblAviso_790MY.BackColor = GUI_08YS.Estilo.TemaFogon_790MY.MaitreGlobo;
            this.lblAviso_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblAviso_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAviso_790MY.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAviso_790MY.ForeColor = GUI_08YS.Estilo.TemaFogon_790MY.Crema;
            this.lblAviso_790MY.Location = new System.Drawing.Point(14, 10);
            this.lblAviso_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.lblAviso_790MY.Name = "lblAviso_790MY";
            this.lblAviso_790MY.Size = new System.Drawing.Size(272, 20);
            this.lblAviso_790MY.TabIndex = 0;
            this.lblAviso_790MY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblAviso_790MY.UseMnemonic = false;
            this.lblAviso_790MY.Click += new System.EventHandler(this.Aviso_Click);
            //
            // AsistenteMaitre_UC_790MY  (sin bordes; fondo sólido = TemaFogon.Crema, el mismo
            // color de panel2. La forma visible la recorta Region: personaje ∪ globo)
            //
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.HelpBalloon;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = GUI_08YS.Estilo.TemaFogon_790MY.Crema;
            this.Controls.Add(this.pnlAviso_790MY);
            this.Controls.Add(this.picPersonajeCompacto_790MY);
            this.Controls.Add(this.pnlGlobo_790MY);
            this.Controls.Add(this.picPersonaje_790MY);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "AsistenteMaitre_UC_790MY";
            this.Size = new System.Drawing.Size(600, 432);
            ((System.ComponentModel.ISupportInitialize)(this.picPersonaje_790MY)).EndInit();
            this.tlpGlobo_790MY.ResumeLayout(false);
            this.tlpGlobo_790MY.PerformLayout();
            this.pnlGlobo_790MY.ResumeLayout(false);
            this.pnlGlobo_790MY.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPersonajeCompacto_790MY)).EndInit();
            this.pnlAviso_790MY.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PictureBox picPersonaje_790MY;
        private System.Windows.Forms.Panel pnlGlobo_790MY;
        private System.Windows.Forms.TableLayoutPanel tlpGlobo_790MY;
        private System.Windows.Forms.Label lblTitulo_790MY;
        private System.Windows.Forms.Label lblMensaje_790MY;
        private System.Windows.Forms.Panel pnlSeparador_790MY;
        private System.Windows.Forms.LinkLabel lnkSiguiente_790MY;
        private System.Windows.Forms.PictureBox picPersonajeCompacto_790MY;
        private System.Windows.Forms.Panel pnlAviso_790MY;
        private System.Windows.Forms.Label lblAviso_790MY;
    }
}
