namespace GUI_08YS.RF1
{
    partial class FormSeleccionarMesa_790MY
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.pnlHeader_790MY = new System.Windows.Forms.Panel();
            this.pbLogo_790MY = new System.Windows.Forms.PictureBox();
            this.pnlDivisor_790MY = new System.Windows.Forms.Panel();
            this._lblTituloHeader = new System.Windows.Forms.Label();
            this._lblSubtitulo = new System.Windows.Forms.Label();
            this._pnlFooter = new System.Windows.Forms.Panel();
            this.bnrAvisos_790MY = new GUI_08YS.Estilo.Banner_UC_790MY();
            this._lblFooterInfo = new System.Windows.Forms.Label();
            this.pnlBody_790MY = new System.Windows.Forms.Panel();
            this._pnlSalon = new DoubleBufferedPanel();
            this._pnlDetalle = new System.Windows.Forms.Panel();
            this._lblLeyendaTitulo = new System.Windows.Forms.Label();
            this.pnlDotRecomendada_790MY = new System.Windows.Forms.Panel();
            this._lblLeyendaRecomendada = new System.Windows.Forms.Label();
            this.pnlDotDisponible_790MY = new System.Windows.Forms.Panel();
            this._lblLeyendaDisponible = new System.Windows.Forms.Label();
            this.pnlDotOcupada_790MY = new System.Windows.Forms.Panel();
            this._lblLeyendaOcupada = new System.Windows.Forms.Label();
            this.pnlDotIncompatible_790MY = new System.Windows.Forms.Panel();
            this._lblLeyendaIncompatible = new System.Windows.Forms.Label();
            this.pnlDotSeleccionada_790MY = new System.Windows.Forms.Panel();
            this._lblLeyendaSeleccionada = new System.Windows.Forms.Label();
            this.pnlSep_790MY = new System.Windows.Forms.Panel();
            this._lblInfoTituloDetalle = new System.Windows.Forms.Label();
            this._lblInfoNumero = new System.Windows.Forms.Label();
            this._lblInfoCapacidad = new System.Windows.Forms.Label();
            this._lblInfoEstado = new System.Windows.Forms.Label();
            this._lblPista = new System.Windows.Forms.Label();
            this._btnAceptar = new System.Windows.Forms.Button();
            this._btnCancelar = new System.Windows.Forms.Button();
            this.pnlHeader_790MY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo_790MY)).BeginInit();
            this._pnlFooter.SuspendLayout();
            this.pnlBody_790MY.SuspendLayout();
            this._pnlDetalle.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlHeader_790MY  (header bordó corporativo #4A151B)
            //
            this.pnlHeader_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(21)))), ((int)(((byte)(27)))));
            this.pnlHeader_790MY.Controls.Add(this.pbLogo_790MY);
            this.pnlHeader_790MY.Controls.Add(this.pnlDivisor_790MY);
            this.pnlHeader_790MY.Controls.Add(this._lblTituloHeader);
            this.pnlHeader_790MY.Controls.Add(this._lblSubtitulo);
            this.pnlHeader_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader_790MY.Name = "pnlHeader_790MY";
            this.pnlHeader_790MY.Size = new System.Drawing.Size(1180, 60);
            //
            // pbLogo_790MY  (logo corporativo, izquierda, 62×62)
            //
            this.pbLogo_790MY.BackColor = System.Drawing.Color.Transparent;
            this.pbLogo_790MY.Image = global::GUI_08YS.Properties.Resources.LogoElFogon;
            this.pbLogo_790MY.Location = new System.Drawing.Point(10, 9);
            this.pbLogo_790MY.Name = "pbLogo_790MY";
            this.pbLogo_790MY.Size = new System.Drawing.Size(62, 62);
            this.pbLogo_790MY.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbLogo_790MY.TabStop = false;
            //
            // pnlDivisor_790MY  (divisor vertical semitransparente)
            //
            this.pnlDivisor_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlDivisor_790MY.Location = new System.Drawing.Point(78, 14);
            this.pnlDivisor_790MY.Name = "pnlDivisor_790MY";
            this.pnlDivisor_790MY.Size = new System.Drawing.Size(1, 52);
            //
            // _lblTituloHeader  (título, blanco)
            //
            this._lblTituloHeader.AutoSize = true;
            this._lblTituloHeader.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this._lblTituloHeader.ForeColor = System.Drawing.Color.White;
            this._lblTituloHeader.Location = new System.Drawing.Point(88, 12);
            this._lblTituloHeader.Name = "_lblTituloHeader";
            this._lblTituloHeader.Text = "Seleccionar Mesa";
            //
            // _lblSubtitulo  (subtítulo estático, dorado)
            //
            this._lblSubtitulo.AutoSize = true;
            this._lblSubtitulo.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this._lblSubtitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(215)))), ((int)(((byte)(128)))));
            this._lblSubtitulo.Location = new System.Drawing.Point(90, 44);
            this._lblSubtitulo.Name = "_lblSubtitulo";
            this._lblSubtitulo.Text = "Elegí la mesa que mejor se adapte a tu reserva";
            //
            // _pnlFooter  (barra inferior con datos del turno, #1A1A1A)
            //
            this._pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this._pnlFooter.Controls.Add(this._lblFooterInfo);
            this._pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._pnlFooter.Name = "_pnlFooter";
            this._pnlFooter.Size = new System.Drawing.Size(1180, 35);
            //
            // _lblFooterInfo  (el texto se arma en FormatarSubtitulo() / UpdateIdioma())
            //
            this._lblFooterInfo.AutoSize = true;
            this._lblFooterInfo.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this._lblFooterInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(200)))), ((int)(((byte)(170)))));
            this._lblFooterInfo.Location = new System.Drawing.Point(15, 8);
            this._lblFooterInfo.Name = "_lblFooterInfo";
            //
            // pnlBody_790MY  (Fill)
            //
            this.pnlBody_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.pnlBody_790MY.Controls.Add(this._pnlSalon);
            this.pnlBody_790MY.Controls.Add(this._pnlDetalle);
            this.pnlBody_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody_790MY.Name = "pnlBody_790MY";
            //
            // _pnlSalon  (Fill, con imagen de fondo)
            // AutoScroll = false: la grilla 4×3 cabe en 660 px (3 filas × 155 px + 40 px inicial).
            // Con AutoScroll activo, WinForms estira BackgroundImage sobre la DisplayRectangle
            // (área virtual del scroll) en lugar del ClientRectangle visible, provocando recorte.
            // No se asigna BackgroundImageLayout: la pintura manual de
            // DoubleBufferedPanel.OnPaintBackground reemplaza el escalado automático.
            //
            this._pnlSalon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this._pnlSalon.BackgroundImage = global::GUI_08YS.Properties.Resources.FondoRestauran;
            this._pnlSalon.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pnlSalon.Name = "_pnlSalon";
            //
            // _pnlDetalle  (panel lateral derecho)
            //
            this._pnlDetalle.BackColor = System.Drawing.Color.White;
            this._pnlDetalle.Controls.Add(this._lblLeyendaTitulo);
            this._pnlDetalle.Controls.Add(this.pnlDotRecomendada_790MY);
            this._pnlDetalle.Controls.Add(this._lblLeyendaRecomendada);
            this._pnlDetalle.Controls.Add(this.pnlDotDisponible_790MY);
            this._pnlDetalle.Controls.Add(this._lblLeyendaDisponible);
            this._pnlDetalle.Controls.Add(this.pnlDotOcupada_790MY);
            this._pnlDetalle.Controls.Add(this._lblLeyendaOcupada);
            this._pnlDetalle.Controls.Add(this.pnlDotIncompatible_790MY);
            this._pnlDetalle.Controls.Add(this._lblLeyendaIncompatible);
            this._pnlDetalle.Controls.Add(this.pnlDotSeleccionada_790MY);
            this._pnlDetalle.Controls.Add(this._lblLeyendaSeleccionada);
            this._pnlDetalle.Controls.Add(this.pnlSep_790MY);
            this._pnlDetalle.Controls.Add(this._lblInfoTituloDetalle);
            this._pnlDetalle.Controls.Add(this._lblInfoNumero);
            this._pnlDetalle.Controls.Add(this._lblInfoCapacidad);
            this._pnlDetalle.Controls.Add(this._lblInfoEstado);
            this._pnlDetalle.Controls.Add(this._lblPista);
            this._pnlDetalle.Controls.Add(this._btnAceptar);
            this._pnlDetalle.Controls.Add(this._btnCancelar);
            this._pnlDetalle.Dock = System.Windows.Forms.DockStyle.Right;
            this._pnlDetalle.Name = "_pnlDetalle";
            this._pnlDetalle.Padding = new System.Windows.Forms.Padding(10);
            this._pnlDetalle.Size = new System.Drawing.Size(240, 100);
            this._pnlDetalle.Paint += new System.Windows.Forms.PaintEventHandler(this._pnlDetalle_Paint);
            this._pnlDetalle.Resize += new System.EventHandler(this._pnlDetalle_Resize);
            //
            // _lblLeyendaTitulo
            //
            this._lblLeyendaTitulo.AutoSize = true;
            this._lblLeyendaTitulo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this._lblLeyendaTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this._lblLeyendaTitulo.Location = new System.Drawing.Point(15, 15);
            this._lblLeyendaTitulo.Name = "_lblLeyendaTitulo";
            this._lblLeyendaTitulo.Text = "Leyenda";
            //
            // pnlDotRecomendada_790MY / _lblLeyendaRecomendada  (y = 45)
            //
            this.pnlDotRecomendada_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.pnlDotRecomendada_790MY.Location = new System.Drawing.Point(15, 47);
            this.pnlDotRecomendada_790MY.Name = "pnlDotRecomendada_790MY";
            this.pnlDotRecomendada_790MY.Size = new System.Drawing.Size(10, 10);
            this._lblLeyendaRecomendada.AutoSize = true;
            this._lblLeyendaRecomendada.Font = new System.Drawing.Font("Segoe UI", 8F);
            this._lblLeyendaRecomendada.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this._lblLeyendaRecomendada.Location = new System.Drawing.Point(30, 45);
            this._lblLeyendaRecomendada.Name = "_lblLeyendaRecomendada";
            this._lblLeyendaRecomendada.Text = "⭐ Recomendada";
            //
            // pnlDotDisponible_790MY / _lblLeyendaDisponible  (y = 70)
            //
            this.pnlDotDisponible_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(140)))), ((int)(((byte)(70)))));
            this.pnlDotDisponible_790MY.Location = new System.Drawing.Point(15, 72);
            this.pnlDotDisponible_790MY.Name = "pnlDotDisponible_790MY";
            this.pnlDotDisponible_790MY.Size = new System.Drawing.Size(10, 10);
            this._lblLeyendaDisponible.AutoSize = true;
            this._lblLeyendaDisponible.Font = new System.Drawing.Font("Segoe UI", 8F);
            this._lblLeyendaDisponible.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this._lblLeyendaDisponible.Location = new System.Drawing.Point(30, 70);
            this._lblLeyendaDisponible.Name = "_lblLeyendaDisponible";
            this._lblLeyendaDisponible.Text = "Disponible";
            //
            // pnlDotOcupada_790MY / _lblLeyendaOcupada  (y = 95)
            //
            this.pnlDotOcupada_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.pnlDotOcupada_790MY.Location = new System.Drawing.Point(15, 97);
            this.pnlDotOcupada_790MY.Name = "pnlDotOcupada_790MY";
            this.pnlDotOcupada_790MY.Size = new System.Drawing.Size(10, 10);
            this._lblLeyendaOcupada.AutoSize = true;
            this._lblLeyendaOcupada.Font = new System.Drawing.Font("Segoe UI", 8F);
            this._lblLeyendaOcupada.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this._lblLeyendaOcupada.Location = new System.Drawing.Point(30, 95);
            this._lblLeyendaOcupada.Name = "_lblLeyendaOcupada";
            this._lblLeyendaOcupada.Text = "Ocupada / Reservada";
            //
            // pnlDotIncompatible_790MY / _lblLeyendaIncompatible  (y = 120)
            //
            this.pnlDotIncompatible_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(155)))), ((int)(((byte)(148)))));
            this.pnlDotIncompatible_790MY.Location = new System.Drawing.Point(15, 122);
            this.pnlDotIncompatible_790MY.Name = "pnlDotIncompatible_790MY";
            this.pnlDotIncompatible_790MY.Size = new System.Drawing.Size(10, 10);
            this._lblLeyendaIncompatible.AutoSize = true;
            this._lblLeyendaIncompatible.Font = new System.Drawing.Font("Segoe UI", 8F);
            this._lblLeyendaIncompatible.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this._lblLeyendaIncompatible.Location = new System.Drawing.Point(30, 120);
            this._lblLeyendaIncompatible.Name = "_lblLeyendaIncompatible";
            this._lblLeyendaIncompatible.Text = "Incompatible";
            //
            // pnlDotSeleccionada_790MY / _lblLeyendaSeleccionada  (y = 145)
            //
            this.pnlDotSeleccionada_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(84)))), ((int)(((byte)(0)))));
            this.pnlDotSeleccionada_790MY.Location = new System.Drawing.Point(15, 147);
            this.pnlDotSeleccionada_790MY.Name = "pnlDotSeleccionada_790MY";
            this.pnlDotSeleccionada_790MY.Size = new System.Drawing.Size(10, 10);
            this._lblLeyendaSeleccionada.AutoSize = true;
            this._lblLeyendaSeleccionada.Font = new System.Drawing.Font("Segoe UI", 8F);
            this._lblLeyendaSeleccionada.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this._lblLeyendaSeleccionada.Location = new System.Drawing.Point(30, 145);
            this._lblLeyendaSeleccionada.Name = "_lblLeyendaSeleccionada";
            this._lblLeyendaSeleccionada.Text = "Seleccionada";
            //
            // pnlSep_790MY
            //
            this.pnlSep_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(233)))), ((int)(((byte)(225)))));
            this.pnlSep_790MY.Location = new System.Drawing.Point(15, 170);
            this.pnlSep_790MY.Name = "pnlSep_790MY";
            this.pnlSep_790MY.Size = new System.Drawing.Size(200, 1);
            //
            // _lblInfoTituloDetalle
            //
            this._lblInfoTituloDetalle.AutoSize = true;
            this._lblInfoTituloDetalle.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this._lblInfoTituloDetalle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this._lblInfoTituloDetalle.Location = new System.Drawing.Point(15, 185);
            this._lblInfoTituloDetalle.Name = "_lblInfoTituloDetalle";
            this._lblInfoTituloDetalle.Text = "Mesa seleccionada:";
            //
            // _lblInfoNumero
            //
            this._lblInfoNumero.AutoSize = true;
            this._lblInfoNumero.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this._lblInfoNumero.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(84)))), ((int)(((byte)(0)))));
            this._lblInfoNumero.Location = new System.Drawing.Point(15, 203);
            this._lblInfoNumero.Name = "_lblInfoNumero";
            this._lblInfoNumero.Text = "—";
            //
            // _lblInfoCapacidad
            //
            this._lblInfoCapacidad.AutoSize = true;
            this._lblInfoCapacidad.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._lblInfoCapacidad.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this._lblInfoCapacidad.Location = new System.Drawing.Point(15, 233);
            this._lblInfoCapacidad.Name = "_lblInfoCapacidad";
            //
            // _lblInfoEstado
            //
            this._lblInfoEstado.AutoSize = true;
            this._lblInfoEstado.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic);
            this._lblInfoEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this._lblInfoEstado.Location = new System.Drawing.Point(15, 251);
            this._lblInfoEstado.Name = "_lblInfoEstado";
            //
            // _lblPista
            //
            this._lblPista.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Italic);
            this._lblPista.ForeColor = System.Drawing.Color.Gray;
            this._lblPista.Location = new System.Drawing.Point(15, 273);
            this._lblPista.Name = "_lblPista";
            this._lblPista.Size = new System.Drawing.Size(200, 23);
            this._lblPista.Text = "Seleccioná una mesa del plano";
            //
            // _btnAceptar  (la posición final la ajusta _pnlDetalle_Resize)
            //
            this._btnAceptar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this._btnAceptar.Enabled = false;
            this._btnAceptar.FlatAppearance.BorderSize = 0;
            this._btnAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btnAceptar.ForeColor = System.Drawing.Color.White;
            this._btnAceptar.Location = new System.Drawing.Point(15, 10);
            this._btnAceptar.Name = "_btnAceptar";
            this._btnAceptar.Size = new System.Drawing.Size(200, 36);
            this._btnAceptar.Tag = "btn_aceptar";
            this._btnAceptar.Text = "Confirmar Mesa";
            this._btnAceptar.UseVisualStyleBackColor = false;
            this._btnAceptar.Click += new System.EventHandler(this._btnAceptar_Click);
            //
            // _btnCancelar  (la posición final la ajusta _pnlDetalle_Resize)
            //
            this._btnCancelar.Location = new System.Drawing.Point(15, 60);
            this._btnCancelar.Name = "_btnCancelar";
            this._btnCancelar.Size = new System.Drawing.Size(200, 36);
            this._btnCancelar.Tag = "btn_cancelar";
            this._btnCancelar.Text = "Cancelar";
            this._btnCancelar.UseVisualStyleBackColor = true;
            this._btnCancelar.Click += new System.EventHandler(this._btnCancelar_Click);
            //
            // bnrAvisos_790MY
            // Flotante (sin Dock): se superpone al plano al mostrarse, sin mover sus controles.
            //
            this.bnrAvisos_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.bnrAvisos_790MY.Name = "bnrAvisos_790MY";
            this.bnrAvisos_790MY.TabStop = false;
            //
            // FormSeleccionarMesa_790MY
            //
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.ClientSize = new System.Drawing.Size(960, 640);
            // Mismo orden de agregado que la versión original (header, footer, body):
            // define el z-order y, por lo tanto, el orden de docking.
            this.Controls.Add(this.pnlHeader_790MY);
            this.Controls.Add(this._pnlFooter);
            this.Controls.Add(this.pnlBody_790MY);
            this.Controls.Add(this.bnrAvisos_790MY);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSeleccionarMesa_790MY";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Seleccionar Mesa";
            // Dimensiones del formulario modal (se asignan después de FormBorderStyle
            // para que el área cliente resultante sea la misma que en la versión original).
            this.Size = new System.Drawing.Size(1180, 820);
            this.MinimumSize = new System.Drawing.Size(1180, 820);
            this.Load += new System.EventHandler(this.FormSeleccionarMesa_790MY_Load);
            this.Shown += new System.EventHandler(this.FormSeleccionarMesa_790MY_Shown);
            this.pnlHeader_790MY.ResumeLayout(false);
            this.pnlHeader_790MY.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo_790MY)).EndInit();
            this._pnlFooter.ResumeLayout(false);
            this._pnlFooter.PerformLayout();
            this._pnlDetalle.ResumeLayout(false);
            this._pnlDetalle.PerformLayout();
            this.pnlBody_790MY.ResumeLayout(false);
            this.ResumeLayout(false);
            this.AplicarIconoCorporativo();
        }
        #endregion

        /// <summary>
        /// Icono corporativo (seguro: el recurso puede no existir en la Build actual).
        /// Invocado desde InitializeComponent().
        /// </summary>
        private void AplicarIconoCorporativo()
        {
            try
            {
                var icon = global::GUI_08YS.Properties.Resources.ResourceManager
                               .GetObject("FogonIcon") as System.Drawing.Icon;
                if (icon != null) this.Icon = icon;
            }
            catch { }
        }

        // ── Header ──────────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlHeader_790MY;
        private System.Windows.Forms.PictureBox pbLogo_790MY;
        private System.Windows.Forms.Panel pnlDivisor_790MY;
        private System.Windows.Forms.Label _lblTituloHeader;
        private System.Windows.Forms.Label _lblSubtitulo;

        // ── Footer ──────────────────────────────────────────────────────
        private System.Windows.Forms.Panel _pnlFooter;
        private System.Windows.Forms.Label _lblFooterInfo;

        // ── Body ────────────────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlBody_790MY;
        private DoubleBufferedPanel _pnlSalon;
        private System.Windows.Forms.Panel _pnlDetalle;

        // ── Leyenda ─────────────────────────────────────────────────────
        private System.Windows.Forms.Label _lblLeyendaTitulo;
        private System.Windows.Forms.Panel pnlDotRecomendada_790MY;
        private System.Windows.Forms.Label _lblLeyendaRecomendada;
        private System.Windows.Forms.Panel pnlDotDisponible_790MY;
        private System.Windows.Forms.Label _lblLeyendaDisponible;
        private System.Windows.Forms.Panel pnlDotOcupada_790MY;
        private System.Windows.Forms.Label _lblLeyendaOcupada;
        private System.Windows.Forms.Panel pnlDotIncompatible_790MY;
        private System.Windows.Forms.Label _lblLeyendaIncompatible;
        private System.Windows.Forms.Panel pnlDotSeleccionada_790MY;
        private System.Windows.Forms.Label _lblLeyendaSeleccionada;

        // ── Detalle de mesa ─────────────────────────────────────────────
        private System.Windows.Forms.Panel pnlSep_790MY;
        private System.Windows.Forms.Label _lblInfoTituloDetalle;
        private System.Windows.Forms.Label _lblInfoNumero;
        private System.Windows.Forms.Label _lblInfoCapacidad;
        private System.Windows.Forms.Label _lblInfoEstado;
        private System.Windows.Forms.Label _lblPista;
        private System.Windows.Forms.Button _btnAceptar;
        private System.Windows.Forms.Button _btnCancelar;
        private GUI_08YS.Estilo.Banner_UC_790MY bnrAvisos_790MY;
    }
}
