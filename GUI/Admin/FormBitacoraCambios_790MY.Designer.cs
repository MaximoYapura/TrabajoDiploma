namespace GUI_08YS.Admin
{
    partial class FormBitacoraCambios_790MY
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
            this.tlpMain_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.bnrAvisos_790MY = new GUI_08YS.Estilo.Banner_UC_790MY();
            this.errValidacion_790MY = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errValidacion_790MY)).BeginInit();
            this.pnlCabecera_790MY = new GUI_08YS.RF2.PanelRedondeado_790MY();
            this.tlpCabecera_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitulo_790MY = new System.Windows.Forms.Label();
            this.lblSubtitulo_790MY = new System.Windows.Forms.Label();
            this.lblSesion_790MY = new System.Windows.Forms.Label();
            this.lblPermiso_790MY = new System.Windows.Forms.Label();
            this.pnlFiltros_790MY = new GUI_08YS.RF2.PanelRedondeado_790MY();
            this.tlpFiltros_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.lblCodigo_790MY = new System.Windows.Forms.Label();
            this.lblNombre_790MY = new System.Windows.Forms.Label();
            this.lblFechaDesde_790MY = new System.Windows.Forms.Label();
            this.lblFechaHasta_790MY = new System.Windows.Forms.Label();
            this.txtCodigo_790MY = new System.Windows.Forms.TextBox();
            this.txtNombre_790MY = new System.Windows.Forms.TextBox();
            this.dtpDesde_790MY = new System.Windows.Forms.DateTimePicker();
            this.dtpHasta_790MY = new System.Windows.Forms.DateTimePicker();
            this.btnLimpiar_790MY = new System.Windows.Forms.Button();
            this.btnAplicar_790MY = new System.Windows.Forms.Button();
            this.tlpCentro_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.tlpHistorial_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.lblHistorial_790MY = new System.Windows.Forms.Label();
            this.lblLeyenda_790MY = new System.Windows.Forms.Label();
            this.pnlGrilla_790MY = new GUI_08YS.RF2.PanelRedondeado_790MY();
            this.dgvHistorial_790MY = new System.Windows.Forms.DataGridView();
            this.colID_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCodigo_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFecha_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHora_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategoria_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrecio_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStock_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActivo_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVigente_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblSinRegistros_790MY = new System.Windows.Forms.Label();
            this.pnlDetalle_790MY = new GUI_08YS.RF2.PanelRedondeado_790MY();
            this.tlpDetalle_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.lblDetalleTitulo_790MY = new System.Windows.Forms.Label();
            this.pnlSeparador_790MY = new System.Windows.Forms.Panel();
            this.lblDetalleProducto_790MY = new System.Windows.Forms.Label();
            this.lblDetalleVersion_790MY = new System.Windows.Forms.Label();
            this.lblDetalleResumen_790MY = new System.Windows.Forms.Label();
            this.tlpDiff_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.lblNota_790MY = new System.Windows.Forms.Label();
            this.pnlPie_790MY = new GUI_08YS.RF2.PanelRedondeado_790MY();
            this.tlpPie_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.lblEstado_790MY = new System.Windows.Forms.Label();
            this.lblSeleccion_790MY = new System.Windows.Forms.Label();
            this.btnRestaurar_790MY = new FontAwesome.Sharp.IconButton();
            this.btnSalir_790MY = new System.Windows.Forms.Button();
            this.tlpMain_790MY.SuspendLayout();
            this.pnlCabecera_790MY.SuspendLayout();
            this.tlpCabecera_790MY.SuspendLayout();
            this.pnlFiltros_790MY.SuspendLayout();
            this.tlpFiltros_790MY.SuspendLayout();
            this.tlpCentro_790MY.SuspendLayout();
            this.tlpHistorial_790MY.SuspendLayout();
            this.pnlGrilla_790MY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial_790MY)).BeginInit();
            this.pnlDetalle_790MY.SuspendLayout();
            this.tlpDetalle_790MY.SuspendLayout();
            this.pnlPie_790MY.SuspendLayout();
            this.tlpPie_790MY.SuspendLayout();
            this.SuspendLayout();
            //
            // tlpMain_790MY
            //
            this.tlpMain_790MY.ColumnCount = 1;
            this.tlpMain_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain_790MY.Controls.Add(this.pnlCabecera_790MY, 0, 0);
            this.tlpMain_790MY.Controls.Add(this.pnlFiltros_790MY, 0, 1);
            this.tlpMain_790MY.Controls.Add(this.tlpCentro_790MY, 0, 2);
            this.tlpMain_790MY.Controls.Add(this.pnlPie_790MY, 0, 3);
            this.tlpMain_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain_790MY.Name = "tlpMain_790MY";
            this.tlpMain_790MY.Padding = new System.Windows.Forms.Padding(24, 14, 24, 16);
            this.tlpMain_790MY.RowCount = 4;
            this.tlpMain_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tlpMain_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpMain_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tlpMain_790MY.TabIndex = 0;
            //
            // pnlCabecera_790MY
            //
            this.pnlCabecera_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(29)))), ((int)(((byte)(47)))));
            this.pnlCabecera_790MY.ColorBorde = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(29)))), ((int)(((byte)(47)))));
            this.pnlCabecera_790MY.Controls.Add(this.tlpCabecera_790MY);
            this.pnlCabecera_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCabecera_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.pnlCabecera_790MY.Name = "pnlCabecera_790MY";
            this.pnlCabecera_790MY.Padding = new System.Windows.Forms.Padding(22, 12, 22, 12);
            this.pnlCabecera_790MY.Radio = 8;
            this.pnlCabecera_790MY.TabIndex = 0;
            //
            // tlpCabecera_790MY
            //
            this.tlpCabecera_790MY.BackColor = System.Drawing.Color.Transparent;
            this.tlpCabecera_790MY.ColumnCount = 2;
            this.tlpCabecera_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCabecera_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpCabecera_790MY.Controls.Add(this.lblTitulo_790MY, 0, 0);
            this.tlpCabecera_790MY.Controls.Add(this.lblSubtitulo_790MY, 0, 1);
            this.tlpCabecera_790MY.Controls.Add(this.lblSesion_790MY, 1, 0);
            this.tlpCabecera_790MY.Controls.Add(this.lblPermiso_790MY, 1, 1);
            this.tlpCabecera_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCabecera_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.tlpCabecera_790MY.Name = "tlpCabecera_790MY";
            this.tlpCabecera_790MY.RowCount = 2;
            this.tlpCabecera_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tlpCabecera_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpCabecera_790MY.TabIndex = 0;
            //
            // lblTitulo_790MY
            //
            this.lblTitulo_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom;
            this.lblTitulo_790MY.AutoSize = true;
            this.lblTitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitulo_790MY.ForeColor = System.Drawing.Color.White;
            this.lblTitulo_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.lblTitulo_790MY.Name = "lblTitulo_790MY";
            this.lblTitulo_790MY.TabIndex = 0;
            this.lblTitulo_790MY.Tag = "BC_titulo";
            this.lblTitulo_790MY.Text = "Bitácora de Cambios - Auditoría de Productos";
            //
            // lblSubtitulo_790MY
            //
            this.lblSubtitulo_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top;
            this.lblSubtitulo_790MY.AutoSize = true;
            this.lblSubtitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblSubtitulo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(224)))), ((int)(((byte)(228)))));
            this.lblSubtitulo_790MY.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblSubtitulo_790MY.Name = "lblSubtitulo_790MY";
            this.lblSubtitulo_790MY.TabIndex = 1;
            this.lblSubtitulo_790MY.Tag = "BC_subtitulo";
            this.lblSubtitulo_790MY.Text = "Historial de modificaciones, altas y bajas lógicas.";
            //
            // lblSesion_790MY
            //
            this.lblSesion_790MY.Anchor = System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom;
            this.lblSesion_790MY.AutoSize = true;
            this.lblSesion_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblSesion_790MY.ForeColor = System.Drawing.Color.White;
            this.lblSesion_790MY.Margin = new System.Windows.Forms.Padding(16, 0, 0, 2);
            this.lblSesion_790MY.Name = "lblSesion_790MY";
            this.lblSesion_790MY.TabIndex = 2;
            this.lblSesion_790MY.Tag = "BC_sesion";
            this.lblSesion_790MY.Text = "Sesión de Administrador";
            //
            // lblPermiso_790MY
            //
            this.lblPermiso_790MY.Anchor = System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Top;
            this.lblPermiso_790MY.AutoSize = true;
            this.lblPermiso_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblPermiso_790MY.ForeColor = System.Drawing.Color.White;
            this.lblPermiso_790MY.Margin = new System.Windows.Forms.Padding(16, 2, 0, 0);
            this.lblPermiso_790MY.Name = "lblPermiso_790MY";
            this.lblPermiso_790MY.TabIndex = 3;
            this.lblPermiso_790MY.Tag = "BC_permiso_activo";
            this.lblPermiso_790MY.Text = "Permiso AuditarProductos activo";
            //
            // pnlFiltros_790MY
            //
            this.pnlFiltros_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(251)))), ((int)(((byte)(247)))));
            this.pnlFiltros_790MY.ColorBorde = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(216)))), ((int)(((byte)(202)))));
            this.pnlFiltros_790MY.Controls.Add(this.tlpFiltros_790MY);
            this.pnlFiltros_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFiltros_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 14);
            this.pnlFiltros_790MY.Name = "pnlFiltros_790MY";
            this.pnlFiltros_790MY.Padding = new System.Windows.Forms.Padding(16, 10, 16, 10);
            this.pnlFiltros_790MY.Radio = 8;
            this.pnlFiltros_790MY.TabIndex = 1;
            //
            // tlpFiltros_790MY
            //
            this.tlpFiltros_790MY.BackColor = System.Drawing.Color.Transparent;
            this.tlpFiltros_790MY.ColumnCount = 6;
            this.tlpFiltros_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 166F));
            this.tlpFiltros_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFiltros_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 186F));
            this.tlpFiltros_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 186F));
            this.tlpFiltros_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 104F));
            this.tlpFiltros_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 144F));
            this.tlpFiltros_790MY.Controls.Add(this.lblCodigo_790MY, 0, 0);
            this.tlpFiltros_790MY.Controls.Add(this.lblNombre_790MY, 1, 0);
            this.tlpFiltros_790MY.Controls.Add(this.lblFechaDesde_790MY, 2, 0);
            this.tlpFiltros_790MY.Controls.Add(this.lblFechaHasta_790MY, 3, 0);
            this.tlpFiltros_790MY.Controls.Add(this.txtCodigo_790MY, 0, 1);
            this.tlpFiltros_790MY.Controls.Add(this.txtNombre_790MY, 1, 1);
            this.tlpFiltros_790MY.Controls.Add(this.dtpDesde_790MY, 2, 1);
            this.tlpFiltros_790MY.Controls.Add(this.dtpHasta_790MY, 3, 1);
            this.tlpFiltros_790MY.Controls.Add(this.btnLimpiar_790MY, 4, 0);
            this.tlpFiltros_790MY.Controls.Add(this.btnAplicar_790MY, 5, 0);
            this.tlpFiltros_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFiltros_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFiltros_790MY.Name = "tlpFiltros_790MY";
            this.tlpFiltros_790MY.RowCount = 2;
            this.tlpFiltros_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpFiltros_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFiltros_790MY.SetRowSpan(this.btnLimpiar_790MY, 2);
            this.tlpFiltros_790MY.SetRowSpan(this.btnAplicar_790MY, 2);
            this.tlpFiltros_790MY.TabIndex = 0;
            //
            // lblCodigo_790MY
            //
            this.lblCodigo_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom;
            this.lblCodigo_790MY.AutoSize = true;
            this.lblCodigo_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblCodigo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblCodigo_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblCodigo_790MY.Name = "lblCodigo_790MY";
            this.lblCodigo_790MY.Tag = "BC_lbl_codigo";
            this.lblCodigo_790MY.Text = "Código (ProductoID)";
            //
            // lblNombre_790MY
            //
            this.lblNombre_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom;
            this.lblNombre_790MY.AutoSize = true;
            this.lblNombre_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblNombre_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblNombre_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblNombre_790MY.Name = "lblNombre_790MY";
            this.lblNombre_790MY.Tag = "BC_lbl_nombre";
            this.lblNombre_790MY.Text = "Nombre del Producto";
            //
            // lblFechaDesde_790MY
            //
            this.lblFechaDesde_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom;
            this.lblFechaDesde_790MY.AutoSize = true;
            this.lblFechaDesde_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblFechaDesde_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblFechaDesde_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblFechaDesde_790MY.Name = "lblFechaDesde_790MY";
            this.lblFechaDesde_790MY.Tag = "BC_lbl_desde";
            this.lblFechaDesde_790MY.Text = "Fecha Desde";
            //
            // lblFechaHasta_790MY
            //
            this.lblFechaHasta_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom;
            this.lblFechaHasta_790MY.AutoSize = true;
            this.lblFechaHasta_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblFechaHasta_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblFechaHasta_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblFechaHasta_790MY.Name = "lblFechaHasta_790MY";
            this.lblFechaHasta_790MY.Tag = "BC_lbl_hasta";
            this.lblFechaHasta_790MY.Text = "Fecha Hasta";
            //
            // txtCodigo_790MY
            //
            this.txtCodigo_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.txtCodigo_790MY.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtCodigo_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.txtCodigo_790MY.MaxLength = 9;
            this.txtCodigo_790MY.Name = "txtCodigo_790MY";
            this.txtCodigo_790MY.TabIndex = 0;
            this.txtCodigo_790MY.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Filtro_KeyDown);
            this.txtCodigo_790MY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCodigo_790MY_KeyPress);
            //
            // txtNombre_790MY
            //
            this.txtNombre_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.txtNombre_790MY.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtNombre_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.txtNombre_790MY.MaxLength = 100;
            this.txtNombre_790MY.Name = "txtNombre_790MY";
            this.txtNombre_790MY.TabIndex = 1;
            this.txtNombre_790MY.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Filtro_KeyDown);
            //
            // dtpDesde_790MY
            //
            this.dtpDesde_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dtpDesde_790MY.Checked = false;
            this.dtpDesde_790MY.CustomFormat = "dd/MM/yyyy";
            this.dtpDesde_790MY.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.dtpDesde_790MY.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDesde_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.dtpDesde_790MY.Name = "dtpDesde_790MY";
            this.dtpDesde_790MY.ShowCheckBox = true;
            this.dtpDesde_790MY.TabIndex = 2;
            this.dtpDesde_790MY.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Filtro_KeyDown);
            //
            // dtpHasta_790MY
            //
            this.dtpHasta_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dtpHasta_790MY.Checked = false;
            this.dtpHasta_790MY.CustomFormat = "dd/MM/yyyy";
            this.dtpHasta_790MY.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.dtpHasta_790MY.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpHasta_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.dtpHasta_790MY.Name = "dtpHasta_790MY";
            this.dtpHasta_790MY.ShowCheckBox = true;
            this.dtpHasta_790MY.TabIndex = 3;
            this.dtpHasta_790MY.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Filtro_KeyDown);
            //
            // btnLimpiar_790MY
            //
            this.btnLimpiar_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.btnLimpiar_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLimpiar_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.btnLimpiar_790MY.Name = "btnLimpiar_790MY";
            this.btnLimpiar_790MY.Size = new System.Drawing.Size(94, 44);
            this.btnLimpiar_790MY.TabIndex = 4;
            this.btnLimpiar_790MY.Tag = "BC_btn_limpiar";
            this.btnLimpiar_790MY.Text = "Limpiar";
            this.btnLimpiar_790MY.UseVisualStyleBackColor = false;
            this.btnLimpiar_790MY.Click += new System.EventHandler(this.btnLimpiar_790MY_Click);
            //
            // btnAplicar_790MY
            //
            this.btnAplicar_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.btnAplicar_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAplicar_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.btnAplicar_790MY.Name = "btnAplicar_790MY";
            this.btnAplicar_790MY.Size = new System.Drawing.Size(144, 44);
            this.btnAplicar_790MY.TabIndex = 5;
            this.btnAplicar_790MY.Tag = "BC_btn_aplicar";
            this.btnAplicar_790MY.Text = "Aplicar Filtros";
            this.btnAplicar_790MY.UseVisualStyleBackColor = false;
            this.btnAplicar_790MY.Click += new System.EventHandler(this.btnAplicar_790MY_Click);
            //
            // tlpCentro_790MY
            //
            this.tlpCentro_790MY.ColumnCount = 2;
            this.tlpCentro_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCentro_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350F));
            this.tlpCentro_790MY.Controls.Add(this.tlpHistorial_790MY, 0, 0);
            this.tlpCentro_790MY.Controls.Add(this.pnlDetalle_790MY, 1, 0);
            this.tlpCentro_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCentro_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 14);
            this.tlpCentro_790MY.Name = "tlpCentro_790MY";
            this.tlpCentro_790MY.RowCount = 1;
            this.tlpCentro_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCentro_790MY.TabIndex = 2;
            //
            // tlpHistorial_790MY
            //
            this.tlpHistorial_790MY.ColumnCount = 2;
            this.tlpHistorial_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpHistorial_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHistorial_790MY.Controls.Add(this.lblHistorial_790MY, 0, 0);
            this.tlpHistorial_790MY.Controls.Add(this.lblLeyenda_790MY, 1, 0);
            this.tlpHistorial_790MY.Controls.Add(this.pnlGrilla_790MY, 0, 1);
            this.tlpHistorial_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpHistorial_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.tlpHistorial_790MY.Name = "tlpHistorial_790MY";
            this.tlpHistorial_790MY.RowCount = 2;
            this.tlpHistorial_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpHistorial_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHistorial_790MY.SetColumnSpan(this.pnlGrilla_790MY, 2);
            this.tlpHistorial_790MY.TabIndex = 0;
            //
            // lblHistorial_790MY
            //
            this.lblHistorial_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblHistorial_790MY.AutoSize = true;
            this.lblHistorial_790MY.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblHistorial_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblHistorial_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.lblHistorial_790MY.Name = "lblHistorial_790MY";
            this.lblHistorial_790MY.Tag = "BC_historial";
            this.lblHistorial_790MY.Text = "Historial de versiones";
            //
            // lblLeyenda_790MY
            //
            this.lblLeyenda_790MY.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblLeyenda_790MY.AutoSize = true;
            this.lblLeyenda_790MY.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblLeyenda_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblLeyenda_790MY.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblLeyenda_790MY.Name = "lblLeyenda_790MY";
            this.lblLeyenda_790MY.Tag = "BC_leyenda";
            this.lblLeyenda_790MY.Text = "Vigente (Act = 1) con fondo verde. Histórico (Act = 0) con fondo claro.";
            //
            // pnlGrilla_790MY
            //
            this.pnlGrilla_790MY.BackColor = System.Drawing.Color.White;
            this.pnlGrilla_790MY.ColorBorde = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(216)))), ((int)(((byte)(202)))));
            this.pnlGrilla_790MY.Controls.Add(this.lblSinRegistros_790MY);
            this.pnlGrilla_790MY.Controls.Add(this.dgvHistorial_790MY);
            this.pnlGrilla_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrilla_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.pnlGrilla_790MY.Name = "pnlGrilla_790MY";
            this.pnlGrilla_790MY.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.pnlGrilla_790MY.Radio = 8;
            this.pnlGrilla_790MY.TabIndex = 2;
            //
            // dgvHistorial_790MY
            //
            this.dgvHistorial_790MY.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID_790MY,
            this.colCodigo_790MY,
            this.colFecha_790MY,
            this.colHora_790MY,
            this.colNombre_790MY,
            this.colCategoria_790MY,
            this.colPrecio_790MY,
            this.colStock_790MY,
            this.colActivo_790MY,
            this.colVigente_790MY});
            this.dgvHistorial_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHistorial_790MY.Name = "dgvHistorial_790MY";
            this.dgvHistorial_790MY.TabIndex = 6;
            this.dgvHistorial_790MY.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvHistorial_790MY_CellPainting);
            this.dgvHistorial_790MY.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvHistorial_790MY_RowPostPaint);
            this.dgvHistorial_790MY.SelectionChanged += new System.EventHandler(this.dgvHistorial_790MY_SelectionChanged);
            this.dgvHistorial_790MY.Resize += new System.EventHandler(this.dgvHistorial_790MY_Resize);
            //
            // colID_790MY
            //
            this.colID_790MY.FillWeight = 6F;
            this.colID_790MY.HeaderText = "ID";
            this.colID_790MY.Name = "colID_790MY";
            //
            // colCodigo_790MY
            //
            this.colCodigo_790MY.FillWeight = 7F;
            this.colCodigo_790MY.HeaderText = "Código";
            this.colCodigo_790MY.Name = "colCodigo_790MY";
            //
            // colFecha_790MY
            //
            this.colFecha_790MY.FillWeight = 10F;
            this.colFecha_790MY.HeaderText = "Fecha";
            this.colFecha_790MY.Name = "colFecha_790MY";
            //
            // colHora_790MY
            //
            this.colHora_790MY.FillWeight = 8F;
            this.colHora_790MY.HeaderText = "Hora";
            this.colHora_790MY.Name = "colHora_790MY";
            //
            // colNombre_790MY
            //
            this.colNombre_790MY.FillWeight = 16F;
            this.colNombre_790MY.HeaderText = "Nombre";
            this.colNombre_790MY.Name = "colNombre_790MY";
            //
            // colCategoria_790MY
            //
            this.colCategoria_790MY.FillWeight = 14F;
            this.colCategoria_790MY.HeaderText = "Categoría";
            this.colCategoria_790MY.Name = "colCategoria_790MY";
            //
            // colPrecio_790MY
            //
            this.colPrecio_790MY.FillWeight = 11F;
            this.colPrecio_790MY.HeaderText = "Precio";
            this.colPrecio_790MY.Name = "colPrecio_790MY";
            //
            // colStock_790MY
            //
            this.colStock_790MY.FillWeight = 6F;
            this.colStock_790MY.HeaderText = "Stock";
            this.colStock_790MY.Name = "colStock_790MY";
            //
            // colActivo_790MY
            //
            this.colActivo_790MY.FillWeight = 11F;
            this.colActivo_790MY.HeaderText = "Activo (negocio)";
            this.colActivo_790MY.Name = "colActivo_790MY";
            //
            // colVigente_790MY
            //
            this.colVigente_790MY.FillWeight = 10F;
            this.colVigente_790MY.HeaderText = "Vigente (Act)";
            this.colVigente_790MY.Name = "colVigente_790MY";
            //
            // lblSinRegistros_790MY
            //
            this.lblSinRegistros_790MY.BackColor = System.Drawing.Color.White;
            this.lblSinRegistros_790MY.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.lblSinRegistros_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblSinRegistros_790MY.Name = "lblSinRegistros_790MY";
            this.lblSinRegistros_790MY.Tag = "BC_sin_registros";
            this.lblSinRegistros_790MY.Text = "No hay versiones para los filtros indicados.";
            this.lblSinRegistros_790MY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSinRegistros_790MY.Visible = false;
            //
            // pnlDetalle_790MY
            //
            this.pnlDetalle_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(251)))), ((int)(((byte)(247)))));
            this.pnlDetalle_790MY.ColorBorde = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(216)))), ((int)(((byte)(202)))));
            this.pnlDetalle_790MY.Controls.Add(this.tlpDetalle_790MY);
            this.pnlDetalle_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetalle_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.pnlDetalle_790MY.Name = "pnlDetalle_790MY";
            this.pnlDetalle_790MY.Padding = new System.Windows.Forms.Padding(16, 12, 16, 14);
            this.pnlDetalle_790MY.Radio = 8;
            this.pnlDetalle_790MY.TabIndex = 1;
            //
            // tlpDetalle_790MY
            //
            this.tlpDetalle_790MY.BackColor = System.Drawing.Color.Transparent;
            this.tlpDetalle_790MY.ColumnCount = 1;
            this.tlpDetalle_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDetalle_790MY.Controls.Add(this.lblDetalleTitulo_790MY, 0, 0);
            this.tlpDetalle_790MY.Controls.Add(this.pnlSeparador_790MY, 0, 1);
            this.tlpDetalle_790MY.Controls.Add(this.lblDetalleProducto_790MY, 0, 2);
            this.tlpDetalle_790MY.Controls.Add(this.lblDetalleVersion_790MY, 0, 3);
            this.tlpDetalle_790MY.Controls.Add(this.lblDetalleResumen_790MY, 0, 4);
            this.tlpDetalle_790MY.Controls.Add(this.tlpDiff_790MY, 0, 5);
            this.tlpDetalle_790MY.Controls.Add(this.lblNota_790MY, 0, 6);
            this.tlpDetalle_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDetalle_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.tlpDetalle_790MY.Name = "tlpDetalle_790MY";
            this.tlpDetalle_790MY.RowCount = 7;
            this.tlpDetalle_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetalle_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tlpDetalle_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetalle_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetalle_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetalle_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDetalle_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetalle_790MY.TabIndex = 0;
            //
            // lblDetalleTitulo_790MY
            //
            this.lblDetalleTitulo_790MY.AutoSize = true;
            this.lblDetalleTitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 12.5F, System.Drawing.FontStyle.Bold);
            this.lblDetalleTitulo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(29)))), ((int)(((byte)(47)))));
            this.lblDetalleTitulo_790MY.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblDetalleTitulo_790MY.Name = "lblDetalleTitulo_790MY";
            this.lblDetalleTitulo_790MY.Tag = "BC_detalle_titulo";
            this.lblDetalleTitulo_790MY.Text = "Detalle de la versión";
            //
            // pnlSeparador_790MY
            //
            this.pnlSeparador_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(216)))), ((int)(((byte)(202)))));
            this.pnlSeparador_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSeparador_790MY.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.pnlSeparador_790MY.Name = "pnlSeparador_790MY";
            //
            // lblDetalleProducto_790MY
            //
            this.lblDetalleProducto_790MY.AutoEllipsis = true;
            this.lblDetalleProducto_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetalleProducto_790MY.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblDetalleProducto_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblDetalleProducto_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.lblDetalleProducto_790MY.Name = "lblDetalleProducto_790MY";
            this.lblDetalleProducto_790MY.Size = new System.Drawing.Size(318, 30);
            this.lblDetalleProducto_790MY.Text = "-";
            //
            // lblDetalleVersion_790MY
            //
            this.lblDetalleVersion_790MY.AutoSize = true;
            this.lblDetalleVersion_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblDetalleVersion_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblDetalleVersion_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.lblDetalleVersion_790MY.MaximumSize = new System.Drawing.Size(318, 0);
            this.lblDetalleVersion_790MY.Name = "lblDetalleVersion_790MY";
            this.lblDetalleVersion_790MY.Text = "";
            //
            // lblDetalleResumen_790MY
            //
            this.lblDetalleResumen_790MY.AutoSize = true;
            this.lblDetalleResumen_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblDetalleResumen_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblDetalleResumen_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.lblDetalleResumen_790MY.MaximumSize = new System.Drawing.Size(318, 0);
            this.lblDetalleResumen_790MY.Name = "lblDetalleResumen_790MY";
            this.lblDetalleResumen_790MY.Text = "";
            //
            // tlpDiff_790MY  (las filas se arman en código)
            //
            this.tlpDiff_790MY.AutoScroll = true;
            this.tlpDiff_790MY.BackColor = System.Drawing.Color.Transparent;
            this.tlpDiff_790MY.ColumnCount = 3;
            this.tlpDiff_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27F));
            this.tlpDiff_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36F));
            this.tlpDiff_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37F));
            this.tlpDiff_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDiff_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.tlpDiff_790MY.Name = "tlpDiff_790MY";
            this.tlpDiff_790MY.RowCount = 1;
            this.tlpDiff_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            //
            // lblNota_790MY
            //
            this.lblNota_790MY.AutoSize = true;
            this.lblNota_790MY.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNota_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblNota_790MY.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.lblNota_790MY.MaximumSize = new System.Drawing.Size(318, 0);
            this.lblNota_790MY.Name = "lblNota_790MY";
            this.lblNota_790MY.Tag = "BC_nota";
            this.lblNota_790MY.Text = "Ningún registro del historial se elimina. Podés deshacer el rollback justo después.";
            //
            // pnlPie_790MY
            //
            this.pnlPie_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(251)))), ((int)(((byte)(247)))));
            this.pnlPie_790MY.ColorBorde = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(216)))), ((int)(((byte)(202)))));
            this.pnlPie_790MY.Controls.Add(this.tlpPie_790MY);
            this.pnlPie_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPie_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.pnlPie_790MY.Name = "pnlPie_790MY";
            this.pnlPie_790MY.Padding = new System.Windows.Forms.Padding(16, 10, 16, 10);
            this.pnlPie_790MY.Radio = 8;
            this.pnlPie_790MY.TabIndex = 3;
            //
            // tlpPie_790MY
            //
            this.tlpPie_790MY.BackColor = System.Drawing.Color.Transparent;
            this.tlpPie_790MY.ColumnCount = 3;
            this.tlpPie_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPie_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 270F));
            this.tlpPie_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.tlpPie_790MY.Controls.Add(this.lblEstado_790MY, 0, 0);
            this.tlpPie_790MY.Controls.Add(this.lblSeleccion_790MY, 0, 1);
            this.tlpPie_790MY.Controls.Add(this.btnRestaurar_790MY, 1, 0);
            this.tlpPie_790MY.Controls.Add(this.btnSalir_790MY, 2, 0);
            this.tlpPie_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPie_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPie_790MY.Name = "tlpPie_790MY";
            this.tlpPie_790MY.RowCount = 2;
            this.tlpPie_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tlpPie_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tlpPie_790MY.SetRowSpan(this.btnRestaurar_790MY, 2);
            this.tlpPie_790MY.SetRowSpan(this.btnSalir_790MY, 2);
            this.tlpPie_790MY.TabIndex = 0;
            //
            // lblEstado_790MY
            //
            this.lblEstado_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom;
            this.lblEstado_790MY.AutoSize = true;
            this.lblEstado_790MY.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblEstado_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblEstado_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.lblEstado_790MY.Name = "lblEstado_790MY";
            this.lblEstado_790MY.Text = "-";
            //
            // lblSeleccion_790MY
            //
            this.lblSeleccion_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top;
            this.lblSeleccion_790MY.AutoSize = true;
            this.lblSeleccion_790MY.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSeleccion_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblSeleccion_790MY.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblSeleccion_790MY.Name = "lblSeleccion_790MY";
            this.lblSeleccion_790MY.Text = "";
            //
            // btnRestaurar_790MY
            //
            this.btnRestaurar_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.btnRestaurar_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(84)))), ((int)(((byte)(20)))));
            this.btnRestaurar_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRestaurar_790MY.FlatAppearance.BorderSize = 0;
            this.btnRestaurar_790MY.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(70)))), ((int)(((byte)(16)))));
            this.btnRestaurar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestaurar_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnRestaurar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnRestaurar_790MY.IconChar = FontAwesome.Sharp.IconChar.RotateLeft;
            this.btnRestaurar_790MY.IconColor = System.Drawing.Color.White;
            this.btnRestaurar_790MY.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnRestaurar_790MY.IconSize = 16;
            this.btnRestaurar_790MY.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRestaurar_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.btnRestaurar_790MY.Name = "btnRestaurar_790MY";
            this.btnRestaurar_790MY.Padding = new System.Windows.Forms.Padding(14, 0, 10, 0);
            this.btnRestaurar_790MY.Size = new System.Drawing.Size(260, 44);
            this.btnRestaurar_790MY.TabIndex = 7;
            this.btnRestaurar_790MY.Tag = "BC_btn_restaurar";
            this.btnRestaurar_790MY.Text = "Restaurar Versión (Rollback)";
            this.btnRestaurar_790MY.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRestaurar_790MY.UseVisualStyleBackColor = false;
            this.btnRestaurar_790MY.Click += new System.EventHandler(this.btnRestaurar_790MY_Click);
            //
            // btnSalir_790MY
            //
            this.btnSalir_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.btnSalir_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSalir_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.btnSalir_790MY.Name = "btnSalir_790MY";
            this.btnSalir_790MY.Size = new System.Drawing.Size(86, 44);
            this.btnSalir_790MY.TabIndex = 8;
            this.btnSalir_790MY.Tag = "BC_btn_salir";
            this.btnSalir_790MY.Text = "Salir";
            this.btnSalir_790MY.UseVisualStyleBackColor = false;
            this.btnSalir_790MY.Click += new System.EventHandler(this.btnSalir_790MY_Click);
            //
            // bnrAvisos_790MY
            //
            this.bnrAvisos_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.bnrAvisos_790MY.Name = "bnrAvisos_790MY";
            this.bnrAvisos_790MY.TabIndex = 1;
            //
            // errValidacion_790MY
            //
            this.errValidacion_790MY.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errValidacion_790MY.ContainerControl = this;
            this.errValidacion_790MY.SetIconAlignment(this.txtCodigo_790MY, System.Windows.Forms.ErrorIconAlignment.MiddleRight);
            this.errValidacion_790MY.SetIconPadding(this.txtCodigo_790MY, -20);
            //
            // FormBitacoraCambios_790MY
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(251)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(1400, 860);
            this.Controls.Add(this.tlpMain_790MY);
            this.Controls.Add(this.bnrAvisos_790MY);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "FormBitacoraCambios_790MY";
            this.Tag = "BC_titulo";
            this.Text = "Bitácora de Cambios - Auditoría de Productos";
            this.Load += new System.EventHandler(this.FormBitacoraCambios_790MY_Load);
            this.tlpMain_790MY.ResumeLayout(false);
            this.pnlCabecera_790MY.ResumeLayout(false);
            this.tlpCabecera_790MY.ResumeLayout(false);
            this.tlpCabecera_790MY.PerformLayout();
            this.pnlFiltros_790MY.ResumeLayout(false);
            this.tlpFiltros_790MY.ResumeLayout(false);
            this.tlpFiltros_790MY.PerformLayout();
            this.tlpCentro_790MY.ResumeLayout(false);
            this.tlpHistorial_790MY.ResumeLayout(false);
            this.tlpHistorial_790MY.PerformLayout();
            this.pnlGrilla_790MY.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial_790MY)).EndInit();
            this.pnlDetalle_790MY.ResumeLayout(false);
            this.tlpDetalle_790MY.ResumeLayout(false);
            this.tlpDetalle_790MY.PerformLayout();
            this.pnlPie_790MY.ResumeLayout(false);
            this.tlpPie_790MY.ResumeLayout(false);
            this.tlpPie_790MY.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errValidacion_790MY)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain_790MY;
        private GUI_08YS.RF2.PanelRedondeado_790MY pnlCabecera_790MY;
        private System.Windows.Forms.TableLayoutPanel tlpCabecera_790MY;
        private System.Windows.Forms.Label lblTitulo_790MY;
        private System.Windows.Forms.Label lblSubtitulo_790MY;
        private System.Windows.Forms.Label lblSesion_790MY;
        private System.Windows.Forms.Label lblPermiso_790MY;
        private GUI_08YS.RF2.PanelRedondeado_790MY pnlFiltros_790MY;
        private System.Windows.Forms.TableLayoutPanel tlpFiltros_790MY;
        private System.Windows.Forms.Label lblCodigo_790MY;
        private System.Windows.Forms.Label lblNombre_790MY;
        private System.Windows.Forms.Label lblFechaDesde_790MY;
        private System.Windows.Forms.Label lblFechaHasta_790MY;
        private System.Windows.Forms.TextBox txtCodigo_790MY;
        private System.Windows.Forms.TextBox txtNombre_790MY;
        private System.Windows.Forms.DateTimePicker dtpDesde_790MY;
        private System.Windows.Forms.DateTimePicker dtpHasta_790MY;
        private System.Windows.Forms.Button btnLimpiar_790MY;
        private System.Windows.Forms.Button btnAplicar_790MY;
        private System.Windows.Forms.TableLayoutPanel tlpCentro_790MY;
        private System.Windows.Forms.TableLayoutPanel tlpHistorial_790MY;
        private System.Windows.Forms.Label lblHistorial_790MY;
        private System.Windows.Forms.Label lblLeyenda_790MY;
        private GUI_08YS.RF2.PanelRedondeado_790MY pnlGrilla_790MY;
        private System.Windows.Forms.DataGridView dgvHistorial_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCodigo_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFecha_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHora_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategoria_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrecio_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStock_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colActivo_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVigente_790MY;
        private System.Windows.Forms.Label lblSinRegistros_790MY;
        private GUI_08YS.RF2.PanelRedondeado_790MY pnlDetalle_790MY;
        private System.Windows.Forms.TableLayoutPanel tlpDetalle_790MY;
        private System.Windows.Forms.Label lblDetalleTitulo_790MY;
        private System.Windows.Forms.Panel pnlSeparador_790MY;
        private System.Windows.Forms.Label lblDetalleProducto_790MY;
        private System.Windows.Forms.Label lblDetalleVersion_790MY;
        private System.Windows.Forms.Label lblDetalleResumen_790MY;
        private System.Windows.Forms.TableLayoutPanel tlpDiff_790MY;
        private System.Windows.Forms.Label lblNota_790MY;
        private GUI_08YS.RF2.PanelRedondeado_790MY pnlPie_790MY;
        private System.Windows.Forms.TableLayoutPanel tlpPie_790MY;
        private System.Windows.Forms.Label lblEstado_790MY;
        private System.Windows.Forms.Label lblSeleccion_790MY;
        private FontAwesome.Sharp.IconButton btnRestaurar_790MY;
        private System.Windows.Forms.Button btnSalir_790MY;
        private GUI_08YS.Estilo.Banner_UC_790MY bnrAvisos_790MY;
        private System.Windows.Forms.ErrorProvider errValidacion_790MY;
    }
}
