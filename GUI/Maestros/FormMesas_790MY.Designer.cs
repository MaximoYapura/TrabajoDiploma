namespace GUI_08YS.Maestros
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
            this.components = new System.ComponentModel.Container();
            this.pnlEncabezado_790MY = new System.Windows.Forms.Panel();
            this.lblTitulo_790MY = new System.Windows.Forms.Label();
            this.flpLeyenda_790MY = new System.Windows.Forms.FlowLayoutPanel();
            this.chipLeyendaDisponible_790MY = new GUI_08YS.RF2.EtiquetaChip_790MY();
            this.chipLeyendaFuera_790MY = new GUI_08YS.RF2.EtiquetaChip_790MY();
            this.btnAnadir_790MY = new System.Windows.Forms.Button();
            this.btnSalir_790MY = new System.Windows.Forms.Button();
            this.pnlDetalle_790MY = new System.Windows.Forms.Panel();
            this.lblDetalleTitulo_790MY = new System.Windows.Forms.Label();
            this.lblDetalleAyuda_790MY = new System.Windows.Forms.Label();
            this.lblNumero_790MY = new System.Windows.Forms.Label();
            this.txtNumero_790MY = new System.Windows.Forms.TextBox();
            this.lblNumeroAyuda_790MY = new System.Windows.Forms.Label();
            this.lblCapacidad_790MY = new System.Windows.Forms.Label();
            this.nudCapacidad_790MY = new System.Windows.Forms.NumericUpDown();
            this.lblEstado_790MY = new System.Windows.Forms.Label();
            this.rdoDisponible_790MY = new System.Windows.Forms.RadioButton();
            this.rdoFueraServicio_790MY = new System.Windows.Forms.RadioButton();
            this.lblEstadoAyuda_790MY = new System.Windows.Forms.Label();
            this.btnModificar_790MY = new System.Windows.Forms.Button();
            this.btnEliminar_790MY = new System.Windows.Forms.Button();
            this.btnAplicar_790MY = new System.Windows.Forms.Button();
            this.btnCancelar_790MY = new System.Windows.Forms.Button();
            this.flpTablero_790MY = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSinMesas_790MY = new System.Windows.Forms.Label();
            this.bnrAvisos_790MY = new GUI_08YS.Estilo.Banner_UC_790MY();
            this.errValidacion_790MY = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlEncabezado_790MY.SuspendLayout();
            this.flpLeyenda_790MY.SuspendLayout();
            this.pnlDetalle_790MY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCapacidad_790MY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errValidacion_790MY)).BeginInit();
            this.flpTablero_790MY.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlEncabezado_790MY
            //
            this.pnlEncabezado_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.pnlEncabezado_790MY.Controls.Add(this.lblTitulo_790MY);
            this.pnlEncabezado_790MY.Controls.Add(this.flpLeyenda_790MY);
            this.pnlEncabezado_790MY.Controls.Add(this.btnAnadir_790MY);
            this.pnlEncabezado_790MY.Controls.Add(this.btnSalir_790MY);
            this.pnlEncabezado_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEncabezado_790MY.Name = "pnlEncabezado_790MY";
            this.pnlEncabezado_790MY.Size = new System.Drawing.Size(1073, 88);
            this.pnlEncabezado_790MY.TabIndex = 0;
            //
            // lblTitulo_790MY
            //
            this.lblTitulo_790MY.AutoSize = true;
            this.lblTitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblTitulo_790MY.Location = new System.Drawing.Point(16, 12);
            this.lblTitulo_790MY.Name = "lblTitulo_790MY";
            this.lblTitulo_790MY.TabIndex = 0;
            this.lblTitulo_790MY.Tag = "FM_lblTitulo";
            this.lblTitulo_790MY.Text = "Mesas del salón";
            //
            // flpLeyenda_790MY
            //
            this.flpLeyenda_790MY.AutoSize = true;
            this.flpLeyenda_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.flpLeyenda_790MY.Controls.Add(this.chipLeyendaDisponible_790MY);
            this.flpLeyenda_790MY.Controls.Add(this.chipLeyendaFuera_790MY);
            this.flpLeyenda_790MY.Location = new System.Drawing.Point(16, 52);
            this.flpLeyenda_790MY.Name = "flpLeyenda_790MY";
            this.flpLeyenda_790MY.TabIndex = 1;
            this.flpLeyenda_790MY.WrapContents = false;
            //
            // chipLeyendaDisponible_790MY
            //
            this.chipLeyendaDisponible_790MY.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.chipLeyendaDisponible_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.chipLeyendaDisponible_790MY.Name = "chipLeyendaDisponible_790MY";
            this.chipLeyendaDisponible_790MY.Tag = "FM_estado_Disponible";
            this.chipLeyendaDisponible_790MY.Text = "Disponible";
            //
            // chipLeyendaFuera_790MY
            //
            this.chipLeyendaFuera_790MY.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.chipLeyendaFuera_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.chipLeyendaFuera_790MY.Name = "chipLeyendaFuera_790MY";
            this.chipLeyendaFuera_790MY.Tag = "FM_estado_FueraDeServicio";
            this.chipLeyendaFuera_790MY.Text = "Fuera de servicio";
            //
            // btnAnadir_790MY
            //
            this.btnAnadir_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnadir_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAnadir_790MY.Location = new System.Drawing.Point(817, 20);
            this.btnAnadir_790MY.Name = "btnAnadir_790MY";
            this.btnAnadir_790MY.Size = new System.Drawing.Size(124, 40);
            this.btnAnadir_790MY.TabIndex = 2;
            this.btnAnadir_790MY.Tag = "btnAnadir";
            this.btnAnadir_790MY.Text = "Añadir";
            this.btnAnadir_790MY.Click += new System.EventHandler(this.btnAnadir_790MY_Click);
            //
            // btnSalir_790MY
            //
            this.btnSalir_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSalir_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSalir_790MY.Location = new System.Drawing.Point(953, 20);
            this.btnSalir_790MY.Name = "btnSalir_790MY";
            this.btnSalir_790MY.Size = new System.Drawing.Size(104, 40);
            this.btnSalir_790MY.TabIndex = 3;
            this.btnSalir_790MY.Tag = "btnSalir";
            this.btnSalir_790MY.Text = "Salir";
            this.btnSalir_790MY.Click += new System.EventHandler(this.btnSalir_790MY_Click);
            //
            // pnlDetalle_790MY
            //
            this.pnlDetalle_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(240)))), ((int)(((byte)(233)))));
            this.pnlDetalle_790MY.Controls.Add(this.lblDetalleTitulo_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.lblDetalleAyuda_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.lblNumero_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.txtNumero_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.lblNumeroAyuda_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.lblCapacidad_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.nudCapacidad_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.lblEstado_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.rdoDisponible_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.rdoFueraServicio_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.lblEstadoAyuda_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.btnModificar_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.btnEliminar_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.btnAplicar_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.btnCancelar_790MY);
            this.pnlDetalle_790MY.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlDetalle_790MY.Location = new System.Drawing.Point(753, 88);
            this.pnlDetalle_790MY.Name = "pnlDetalle_790MY";
            this.pnlDetalle_790MY.Size = new System.Drawing.Size(320, 616);
            this.pnlDetalle_790MY.TabIndex = 2;
            this.pnlDetalle_790MY.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDetalle_790MY_Paint);
            //
            // lblDetalleTitulo_790MY
            //
            this.lblDetalleTitulo_790MY.AutoEllipsis = true;
            this.lblDetalleTitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblDetalleTitulo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblDetalleTitulo_790MY.Location = new System.Drawing.Point(16, 16);
            this.lblDetalleTitulo_790MY.Name = "lblDetalleTitulo_790MY";
            this.lblDetalleTitulo_790MY.Size = new System.Drawing.Size(288, 28);
            this.lblDetalleTitulo_790MY.TabIndex = 0;
            this.lblDetalleTitulo_790MY.Text = "Detalle de la mesa";
            //
            // lblDetalleAyuda_790MY
            //
            this.lblDetalleAyuda_790MY.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblDetalleAyuda_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblDetalleAyuda_790MY.Location = new System.Drawing.Point(16, 44);
            this.lblDetalleAyuda_790MY.Name = "lblDetalleAyuda_790MY";
            this.lblDetalleAyuda_790MY.Size = new System.Drawing.Size(288, 36);
            this.lblDetalleAyuda_790MY.TabIndex = 1;
            this.lblDetalleAyuda_790MY.Text = "Elegí una mesa del tablero para ver su detalle.";
            //
            // lblNumero_790MY
            //
            this.lblNumero_790MY.AutoSize = true;
            this.lblNumero_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNumero_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblNumero_790MY.Location = new System.Drawing.Point(16, 88);
            this.lblNumero_790MY.Name = "lblNumero_790MY";
            this.lblNumero_790MY.TabIndex = 2;
            this.lblNumero_790MY.Tag = "FM_lblNumero";
            this.lblNumero_790MY.Text = "Número:";
            //
            // txtNumero_790MY
            //
            this.txtNumero_790MY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNumero_790MY.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtNumero_790MY.Location = new System.Drawing.Point(16, 110);
            this.txtNumero_790MY.Name = "txtNumero_790MY";
            this.txtNumero_790MY.ReadOnly = true;
            this.txtNumero_790MY.Size = new System.Drawing.Size(288, 25);
            this.txtNumero_790MY.TabIndex = 3;
            this.txtNumero_790MY.TabStop = false;
            //
            // lblNumeroAyuda_790MY
            //
            this.lblNumeroAyuda_790MY.AutoSize = true;
            this.lblNumeroAyuda_790MY.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblNumeroAyuda_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblNumeroAyuda_790MY.Location = new System.Drawing.Point(16, 138);
            this.lblNumeroAyuda_790MY.Name = "lblNumeroAyuda_790MY";
            this.lblNumeroAyuda_790MY.TabIndex = 4;
            this.lblNumeroAyuda_790MY.Tag = "FM_ayuda_numero";
            this.lblNumeroAyuda_790MY.Text = "El número se asigna automáticamente.";
            //
            // lblCapacidad_790MY
            //
            this.lblCapacidad_790MY.AutoSize = true;
            this.lblCapacidad_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCapacidad_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblCapacidad_790MY.Location = new System.Drawing.Point(16, 168);
            this.lblCapacidad_790MY.Name = "lblCapacidad_790MY";
            this.lblCapacidad_790MY.TabIndex = 5;
            this.lblCapacidad_790MY.Tag = "FM_lblCapacidad";
            this.lblCapacidad_790MY.Text = "Capacidad:";
            //
            // nudCapacidad_790MY
            //
            this.nudCapacidad_790MY.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.nudCapacidad_790MY.Location = new System.Drawing.Point(16, 190);
            this.nudCapacidad_790MY.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nudCapacidad_790MY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudCapacidad_790MY.Name = "nudCapacidad_790MY";
            this.nudCapacidad_790MY.Size = new System.Drawing.Size(288, 25);
            this.nudCapacidad_790MY.TabIndex = 6;
            this.nudCapacidad_790MY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            //
            // lblEstado_790MY
            //
            this.lblEstado_790MY.AutoSize = true;
            this.lblEstado_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblEstado_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblEstado_790MY.Location = new System.Drawing.Point(16, 232);
            this.lblEstado_790MY.Name = "lblEstado_790MY";
            this.lblEstado_790MY.TabIndex = 7;
            this.lblEstado_790MY.Tag = "FM_lblEstado";
            this.lblEstado_790MY.Text = "Estado:";
            //
            // rdoDisponible_790MY
            //
            this.rdoDisponible_790MY.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoDisponible_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdoDisponible_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.rdoDisponible_790MY.Location = new System.Drawing.Point(16, 254);
            this.rdoDisponible_790MY.Name = "rdoDisponible_790MY";
            this.rdoDisponible_790MY.Size = new System.Drawing.Size(140, 36);
            this.rdoDisponible_790MY.TabIndex = 8;
            this.rdoDisponible_790MY.Tag = "FM_estado_Disponible";
            this.rdoDisponible_790MY.Text = "Disponible";
            this.rdoDisponible_790MY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // rdoFueraServicio_790MY
            //
            this.rdoFueraServicio_790MY.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdoFueraServicio_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdoFueraServicio_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.rdoFueraServicio_790MY.Location = new System.Drawing.Point(164, 254);
            this.rdoFueraServicio_790MY.Name = "rdoFueraServicio_790MY";
            this.rdoFueraServicio_790MY.Size = new System.Drawing.Size(140, 36);
            this.rdoFueraServicio_790MY.TabIndex = 9;
            this.rdoFueraServicio_790MY.Tag = "FM_estado_FueraDeServicio";
            this.rdoFueraServicio_790MY.Text = "Fuera de servicio";
            this.rdoFueraServicio_790MY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblEstadoAyuda_790MY
            //
            this.lblEstadoAyuda_790MY.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblEstadoAyuda_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblEstadoAyuda_790MY.Location = new System.Drawing.Point(16, 296);
            this.lblEstadoAyuda_790MY.Name = "lblEstadoAyuda_790MY";
            this.lblEstadoAyuda_790MY.Size = new System.Drawing.Size(288, 34);
            this.lblEstadoAyuda_790MY.TabIndex = 10;
            this.lblEstadoAyuda_790MY.Tag = "FM_ayuda_estado";
            this.lblEstadoAyuda_790MY.Text = "Una mesa fuera de servicio no se ofrece para nuevas reservas.";
            //
            // btnModificar_790MY
            //
            this.btnModificar_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnModificar_790MY.Location = new System.Drawing.Point(16, 336);
            this.btnModificar_790MY.Name = "btnModificar_790MY";
            this.btnModificar_790MY.Size = new System.Drawing.Size(140, 40);
            this.btnModificar_790MY.TabIndex = 11;
            this.btnModificar_790MY.Tag = "btnModificar";
            this.btnModificar_790MY.Text = "Modificar";
            this.btnModificar_790MY.Click += new System.EventHandler(this.btnModificar_790MY_Click);
            //
            // btnEliminar_790MY
            //
            this.btnEliminar_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnEliminar_790MY.Location = new System.Drawing.Point(164, 336);
            this.btnEliminar_790MY.Name = "btnEliminar_790MY";
            this.btnEliminar_790MY.Size = new System.Drawing.Size(140, 40);
            this.btnEliminar_790MY.TabIndex = 12;
            this.btnEliminar_790MY.Tag = "btnEliminar";
            this.btnEliminar_790MY.Text = "Eliminar";
            this.btnEliminar_790MY.Click += new System.EventHandler(this.btnEliminar_790MY_Click);
            //
            // btnAplicar_790MY
            //
            this.btnAplicar_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAplicar_790MY.Location = new System.Drawing.Point(16, 388);
            this.btnAplicar_790MY.Name = "btnAplicar_790MY";
            this.btnAplicar_790MY.Size = new System.Drawing.Size(140, 40);
            this.btnAplicar_790MY.TabIndex = 13;
            this.btnAplicar_790MY.Tag = "btnAplicar";
            this.btnAplicar_790MY.Text = "Aplicar";
            this.btnAplicar_790MY.Click += new System.EventHandler(this.btnAplicar_790MY_Click);
            //
            // btnCancelar_790MY
            //
            this.btnCancelar_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancelar_790MY.Location = new System.Drawing.Point(164, 388);
            this.btnCancelar_790MY.Name = "btnCancelar_790MY";
            this.btnCancelar_790MY.Size = new System.Drawing.Size(140, 40);
            this.btnCancelar_790MY.TabIndex = 14;
            this.btnCancelar_790MY.Tag = "btn_Cancelar";
            this.btnCancelar_790MY.Text = "Cancelar";
            this.btnCancelar_790MY.Click += new System.EventHandler(this.btnCancelar_790MY_Click);
            //
            // flpTablero_790MY
            //
            this.flpTablero_790MY.AutoScroll = true;
            this.flpTablero_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.flpTablero_790MY.Controls.Add(this.lblSinMesas_790MY);
            this.flpTablero_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTablero_790MY.Location = new System.Drawing.Point(0, 88);
            this.flpTablero_790MY.Name = "flpTablero_790MY";
            this.flpTablero_790MY.Padding = new System.Windows.Forms.Padding(16, 4, 16, 16);
            this.flpTablero_790MY.Size = new System.Drawing.Size(753, 616);
            this.flpTablero_790MY.TabIndex = 1;
            //
            // lblSinMesas_790MY
            //
            this.lblSinMesas_790MY.AutoSize = true;
            this.lblSinMesas_790MY.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSinMesas_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblSinMesas_790MY.Margin = new System.Windows.Forms.Padding(0, 16, 0, 0);
            this.lblSinMesas_790MY.Name = "lblSinMesas_790MY";
            this.lblSinMesas_790MY.TabIndex = 0;
            this.lblSinMesas_790MY.Tag = "FM_sin_mesas";
            this.lblSinMesas_790MY.Text = "Todavía no hay mesas cargadas. Usá «Añadir» para crear la primera.";
            this.lblSinMesas_790MY.Visible = false;
            //
            // bnrAvisos_790MY
            //
            this.bnrAvisos_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.bnrAvisos_790MY.Name = "bnrAvisos_790MY";
            this.bnrAvisos_790MY.TabIndex = 3;
            //
            // errValidacion_790MY
            //
            this.errValidacion_790MY.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errValidacion_790MY.ContainerControl = this;
            //
            // FormMesas_790MY
            //
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(1073, 704);
            this.Controls.Add(this.flpTablero_790MY);
            this.Controls.Add(this.pnlDetalle_790MY);
            this.Controls.Add(this.bnrAvisos_790MY);
            this.Controls.Add(this.pnlEncabezado_790MY);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "FormMesas_790MY";
            this.Text = "Mesas";
            this.Load += new System.EventHandler(this.FormMesas_790MY_Load);
            this.pnlEncabezado_790MY.ResumeLayout(false);
            this.pnlEncabezado_790MY.PerformLayout();
            this.flpLeyenda_790MY.ResumeLayout(false);
            this.flpLeyenda_790MY.PerformLayout();
            this.pnlDetalle_790MY.ResumeLayout(false);
            this.pnlDetalle_790MY.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCapacidad_790MY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errValidacion_790MY)).EndInit();
            this.flpTablero_790MY.ResumeLayout(false);
            this.flpTablero_790MY.PerformLayout();
            this.ResumeLayout(false);
            this.AplicarEstiloVisual();
            this.AplicarIconoCorporativo();

        }

        #endregion

        /// <summary>
        /// Tokens de TemaFogon_790MY sobre botones y chips de leyenda (solo apariencia).
        /// Invocado desde InitializeComponent().
        /// </summary>
        private void AplicarEstiloVisual()
        {
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarBoton(this.btnAnadir_790MY, true);
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarBoton(this.btnSalir_790MY, false);
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarBoton(this.btnModificar_790MY, false);
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarBoton(this.btnEliminar_790MY, false);
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarBoton(this.btnAplicar_790MY, true);
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarBoton(this.btnCancelar_790MY, false);
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarOpcion(this.rdoDisponible_790MY);
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarOpcion(this.rdoFueraServicio_790MY);

            this.chipLeyendaDisponible_790MY.BackColor = GUI_08YS.Estilo.TemaFogon_790MY.OkFondo;
            this.chipLeyendaDisponible_790MY.ForeColor = GUI_08YS.Estilo.TemaFogon_790MY.OkTexto;
            this.chipLeyendaFuera_790MY.BackColor = GUI_08YS.Estilo.TemaFogon_790MY.FueraFondo;
            this.chipLeyendaFuera_790MY.ForeColor = GUI_08YS.Estilo.TemaFogon_790MY.FueraTexto;
            this.chipLeyendaFuera_790MY.ColorBorde = GUI_08YS.Estilo.TemaFogon_790MY.FueraBorde;

            // Ícono de error dentro del borde derecho: el campo ocupa todo el ancho del panel.
            this.errValidacion_790MY.SetIconAlignment(this.txtNumero_790MY, System.Windows.Forms.ErrorIconAlignment.MiddleRight);
            this.errValidacion_790MY.SetIconPadding(this.txtNumero_790MY, -20);
        }

        /// <summary>
        /// Icono corporativo (seguro: el recurso puede no existir aún en la Build actual).
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

        private System.Windows.Forms.Panel pnlEncabezado_790MY;
        private System.Windows.Forms.Label lblTitulo_790MY;
        private System.Windows.Forms.FlowLayoutPanel flpLeyenda_790MY;
        private GUI_08YS.RF2.EtiquetaChip_790MY chipLeyendaDisponible_790MY;
        private GUI_08YS.RF2.EtiquetaChip_790MY chipLeyendaFuera_790MY;
        private System.Windows.Forms.Panel pnlDetalle_790MY;
        private System.Windows.Forms.Label lblDetalleTitulo_790MY;
        private System.Windows.Forms.Label lblDetalleAyuda_790MY;
        private System.Windows.Forms.Label lblNumeroAyuda_790MY;
        private System.Windows.Forms.Label lblEstado_790MY;
        private System.Windows.Forms.RadioButton rdoDisponible_790MY;
        private System.Windows.Forms.RadioButton rdoFueraServicio_790MY;
        private System.Windows.Forms.Label lblEstadoAyuda_790MY;
        private System.Windows.Forms.FlowLayoutPanel flpTablero_790MY;
        private System.Windows.Forms.Label lblSinMesas_790MY;
        private System.Windows.Forms.Button btnAnadir_790MY;
        private System.Windows.Forms.Button btnModificar_790MY;
        private System.Windows.Forms.Button btnEliminar_790MY;
        private System.Windows.Forms.Button btnAplicar_790MY;
        private System.Windows.Forms.Button btnCancelar_790MY;
        private System.Windows.Forms.Button btnSalir_790MY;
        private System.Windows.Forms.Label lblNumero_790MY;
        private System.Windows.Forms.TextBox txtNumero_790MY;
        private System.Windows.Forms.Label lblCapacidad_790MY;
        private System.Windows.Forms.NumericUpDown nudCapacidad_790MY;
        private GUI_08YS.Estilo.Banner_UC_790MY bnrAvisos_790MY;
        private System.Windows.Forms.ErrorProvider errValidacion_790MY;
    }
}
