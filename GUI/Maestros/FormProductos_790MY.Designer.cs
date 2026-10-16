namespace GUI_08YS.Maestros
{
    partial class FormProductos_790MY
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
            this.pnlBarra_790MY = new System.Windows.Forms.Panel();
            this.txtBuscar_790MY = new System.Windows.Forms.TextBox();
            this.lblFiltroCategoria_790MY = new System.Windows.Forms.Label();
            this.cmbFiltroCategoria_790MY = new System.Windows.Forms.ComboBox();
            this.chkSoloActivos_790MY = new System.Windows.Forms.CheckBox();
            this.btnAnadir_790MY = new System.Windows.Forms.Button();
            this.btnModificar_790MY = new System.Windows.Forms.Button();
            this.btnBaja_790MY = new System.Windows.Forms.Button();
            this.pnlEstado_790MY = new System.Windows.Forms.Panel();
            this.lblResumen_790MY = new System.Windows.Forms.Label();
            this.lblModo_790MY = new System.Windows.Forms.Label();
            this.pnlDetalle_790MY = new System.Windows.Forms.Panel();
            this.tlpDetalle_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.lblDetalleTitulo_790MY = new System.Windows.Forms.Label();
            this.chipModo_790MY = new GUI_08YS.RF2.EtiquetaChip_790MY();
            this.lblId_790MY = new System.Windows.Forms.Label();
            this.lblNombre_790MY = new System.Windows.Forms.Label();
            this.txtNombre_790MY = new System.Windows.Forms.TextBox();
            this.lblCategoria_790MY = new System.Windows.Forms.Label();
            this.lblPrecio_790MY = new System.Windows.Forms.Label();
            this.cmbCategoria_790MY = new System.Windows.Forms.ComboBox();
            this.nudPrecio_790MY = new System.Windows.Forms.NumericUpDown();
            this.lblStock_790MY = new System.Windows.Forms.Label();
            this.lblComprometido_790MY = new System.Windows.Forms.Label();
            this.nudStock_790MY = new System.Windows.Forms.NumericUpDown();
            this.txtComprometido_790MY = new System.Windows.Forms.TextBox();
            this.lblStockAyuda_790MY = new System.Windows.Forms.Label();
            this.chkControlaStock_790MY = new System.Windows.Forms.CheckBox();
            this.chkActivo_790MY = new System.Windows.Forms.CheckBox();
            this.lblActivoAyuda_790MY = new System.Windows.Forms.Label();
            this.btnGuardar_790MY = new System.Windows.Forms.Button();
            this.btnCancelar_790MY = new System.Windows.Forms.Button();
            this.lblAyuda_790MY = new System.Windows.Forms.Label();
            this.dgvProductos_790MY = new System.Windows.Forms.DataGridView();
            this.colId_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCategoria_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrecio_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStock_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colControlaStock_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstado_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlBarra_790MY.SuspendLayout();
            this.pnlEstado_790MY.SuspendLayout();
            this.pnlDetalle_790MY.SuspendLayout();
            this.tlpDetalle_790MY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPrecio_790MY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStock_790MY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos_790MY)).BeginInit();
            this.SuspendLayout();
            //
            // pnlBarra_790MY
            //
            this.pnlBarra_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(240)))), ((int)(((byte)(233)))));
            this.pnlBarra_790MY.Controls.Add(this.txtBuscar_790MY);
            this.pnlBarra_790MY.Controls.Add(this.lblFiltroCategoria_790MY);
            this.pnlBarra_790MY.Controls.Add(this.cmbFiltroCategoria_790MY);
            this.pnlBarra_790MY.Controls.Add(this.chkSoloActivos_790MY);
            this.pnlBarra_790MY.Controls.Add(this.btnAnadir_790MY);
            this.pnlBarra_790MY.Controls.Add(this.btnModificar_790MY);
            this.pnlBarra_790MY.Controls.Add(this.btnBaja_790MY);
            this.pnlBarra_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBarra_790MY.Name = "pnlBarra_790MY";
            this.pnlBarra_790MY.Size = new System.Drawing.Size(1200, 68);
            this.pnlBarra_790MY.TabIndex = 0;
            //
            // txtBuscar_790MY
            //
            this.txtBuscar_790MY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBuscar_790MY.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtBuscar_790MY.Location = new System.Drawing.Point(16, 18);
            this.txtBuscar_790MY.Name = "txtBuscar_790MY";
            this.txtBuscar_790MY.Size = new System.Drawing.Size(290, 27);
            this.txtBuscar_790MY.TabIndex = 0;
            this.txtBuscar_790MY.TextChanged += new System.EventHandler(this.Filtro_Changed);
            //
            // lblFiltroCategoria_790MY
            //
            this.lblFiltroCategoria_790MY.AutoSize = true;
            this.lblFiltroCategoria_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblFiltroCategoria_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblFiltroCategoria_790MY.Location = new System.Drawing.Point(322, 22);
            this.lblFiltroCategoria_790MY.Name = "lblFiltroCategoria_790MY";
            this.lblFiltroCategoria_790MY.TabIndex = 1;
            this.lblFiltroCategoria_790MY.Tag = "FP_lblFiltroCategoria";
            this.lblFiltroCategoria_790MY.Text = "Categoría";
            //
            // cmbFiltroCategoria_790MY
            //
            this.cmbFiltroCategoria_790MY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiltroCategoria_790MY.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.cmbFiltroCategoria_790MY.Location = new System.Drawing.Point(410, 17);
            this.cmbFiltroCategoria_790MY.Name = "cmbFiltroCategoria_790MY";
            this.cmbFiltroCategoria_790MY.Size = new System.Drawing.Size(190, 28);
            this.cmbFiltroCategoria_790MY.TabIndex = 2;
            this.cmbFiltroCategoria_790MY.SelectedIndexChanged += new System.EventHandler(this.Filtro_Changed);
            //
            // chkSoloActivos_790MY
            //
            this.chkSoloActivos_790MY.AutoSize = true;
            this.chkSoloActivos_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.chkSoloActivos_790MY.Location = new System.Drawing.Point(620, 21);
            this.chkSoloActivos_790MY.Name = "chkSoloActivos_790MY";
            this.chkSoloActivos_790MY.TabIndex = 3;
            this.chkSoloActivos_790MY.Tag = "FP_chkSoloActivos";
            this.chkSoloActivos_790MY.Text = "Solo activos";
            this.chkSoloActivos_790MY.CheckedChanged += new System.EventHandler(this.Filtro_Changed);
            //
            // btnAnadir_790MY
            //
            this.btnAnadir_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnadir_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnAnadir_790MY.Location = new System.Drawing.Point(808, 14);
            this.btnAnadir_790MY.Name = "btnAnadir_790MY";
            this.btnAnadir_790MY.Size = new System.Drawing.Size(120, 40);
            this.btnAnadir_790MY.TabIndex = 4;
            this.btnAnadir_790MY.Tag = "FP_btnAnadir";
            this.btnAnadir_790MY.Text = "+  Añadir";
            this.btnAnadir_790MY.UseVisualStyleBackColor = false;
            this.btnAnadir_790MY.Click += new System.EventHandler(this.btnAnadir_790MY_Click);
            //
            // btnModificar_790MY
            //
            this.btnModificar_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModificar_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnModificar_790MY.Location = new System.Drawing.Point(938, 14);
            this.btnModificar_790MY.Name = "btnModificar_790MY";
            this.btnModificar_790MY.Size = new System.Drawing.Size(120, 40);
            this.btnModificar_790MY.TabIndex = 5;
            this.btnModificar_790MY.Tag = "FP_btnModificar";
            this.btnModificar_790MY.Text = "Modificar";
            this.btnModificar_790MY.UseVisualStyleBackColor = false;
            this.btnModificar_790MY.Click += new System.EventHandler(this.btnModificar_790MY_Click);
            //
            // btnBaja_790MY
            //
            this.btnBaja_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBaja_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnBaja_790MY.Location = new System.Drawing.Point(1068, 14);
            this.btnBaja_790MY.Name = "btnBaja_790MY";
            this.btnBaja_790MY.Size = new System.Drawing.Size(120, 40);
            this.btnBaja_790MY.TabIndex = 6;
            this.btnBaja_790MY.Tag = "FP_btnBaja";
            this.btnBaja_790MY.Text = "Dar de baja";
            this.btnBaja_790MY.UseVisualStyleBackColor = false;
            this.btnBaja_790MY.Click += new System.EventHandler(this.btnBaja_790MY_Click);
            //
            // pnlEstado_790MY
            //
            this.pnlEstado_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(240)))), ((int)(((byte)(233)))));
            this.pnlEstado_790MY.Controls.Add(this.lblResumen_790MY);
            this.pnlEstado_790MY.Controls.Add(this.lblModo_790MY);
            this.pnlEstado_790MY.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlEstado_790MY.Name = "pnlEstado_790MY";
            this.pnlEstado_790MY.Padding = new System.Windows.Forms.Padding(16, 0, 16, 0);
            this.pnlEstado_790MY.Size = new System.Drawing.Size(1200, 30);
            this.pnlEstado_790MY.TabIndex = 3;
            //
            // lblResumen_790MY
            //
            this.lblResumen_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResumen_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblResumen_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblResumen_790MY.Name = "lblResumen_790MY";
            this.lblResumen_790MY.TabIndex = 0;
            this.lblResumen_790MY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblModo_790MY
            //
            this.lblModo_790MY.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblModo_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblModo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblModo_790MY.Name = "lblModo_790MY";
            this.lblModo_790MY.Size = new System.Drawing.Size(220, 30);
            this.lblModo_790MY.TabIndex = 1;
            this.lblModo_790MY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // pnlDetalle_790MY
            //
            this.pnlDetalle_790MY.AutoScroll = true;
            this.pnlDetalle_790MY.BackColor = System.Drawing.Color.White;
            this.pnlDetalle_790MY.Controls.Add(this.tlpDetalle_790MY);
            this.pnlDetalle_790MY.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlDetalle_790MY.Name = "pnlDetalle_790MY";
            this.pnlDetalle_790MY.Padding = new System.Windows.Forms.Padding(20, 16, 20, 16);
            this.pnlDetalle_790MY.Size = new System.Drawing.Size(380, 600);
            this.pnlDetalle_790MY.TabIndex = 2;
            this.pnlDetalle_790MY.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDetalle_790MY_Paint);
            //
            // tlpDetalle_790MY
            //
            this.tlpDetalle_790MY.AutoSize = true;
            this.tlpDetalle_790MY.ColumnCount = 2;
            this.tlpDetalle_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpDetalle_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpDetalle_790MY.Controls.Add(this.lblDetalleTitulo_790MY, 0, 0);
            this.tlpDetalle_790MY.Controls.Add(this.chipModo_790MY, 1, 0);
            this.tlpDetalle_790MY.Controls.Add(this.lblId_790MY, 0, 1);
            this.tlpDetalle_790MY.Controls.Add(this.lblNombre_790MY, 0, 2);
            this.tlpDetalle_790MY.Controls.Add(this.txtNombre_790MY, 0, 3);
            this.tlpDetalle_790MY.Controls.Add(this.lblCategoria_790MY, 0, 4);
            this.tlpDetalle_790MY.Controls.Add(this.lblPrecio_790MY, 1, 4);
            this.tlpDetalle_790MY.Controls.Add(this.cmbCategoria_790MY, 0, 5);
            this.tlpDetalle_790MY.Controls.Add(this.nudPrecio_790MY, 1, 5);
            this.tlpDetalle_790MY.Controls.Add(this.lblStock_790MY, 0, 6);
            this.tlpDetalle_790MY.Controls.Add(this.lblComprometido_790MY, 1, 6);
            this.tlpDetalle_790MY.Controls.Add(this.nudStock_790MY, 0, 7);
            this.tlpDetalle_790MY.Controls.Add(this.txtComprometido_790MY, 1, 7);
            this.tlpDetalle_790MY.Controls.Add(this.lblStockAyuda_790MY, 0, 8);
            this.tlpDetalle_790MY.Controls.Add(this.chkControlaStock_790MY, 0, 9);
            this.tlpDetalle_790MY.Controls.Add(this.chkActivo_790MY, 0, 10);
            this.tlpDetalle_790MY.Controls.Add(this.lblActivoAyuda_790MY, 0, 11);
            this.tlpDetalle_790MY.Controls.Add(this.btnGuardar_790MY, 0, 12);
            this.tlpDetalle_790MY.Controls.Add(this.btnCancelar_790MY, 1, 12);
            this.tlpDetalle_790MY.Controls.Add(this.lblAyuda_790MY, 0, 13);
            this.tlpDetalle_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpDetalle_790MY.Name = "tlpDetalle_790MY";
            this.tlpDetalle_790MY.RowCount = 14;
            this.tlpDetalle_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetalle_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetalle_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetalle_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetalle_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetalle_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetalle_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetalle_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetalle_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetalle_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetalle_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetalle_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetalle_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetalle_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpDetalle_790MY.SetColumnSpan(this.lblId_790MY, 2);
            this.tlpDetalle_790MY.SetColumnSpan(this.lblNombre_790MY, 2);
            this.tlpDetalle_790MY.SetColumnSpan(this.txtNombre_790MY, 2);
            this.tlpDetalle_790MY.SetColumnSpan(this.lblStockAyuda_790MY, 2);
            this.tlpDetalle_790MY.SetColumnSpan(this.chkControlaStock_790MY, 2);
            this.tlpDetalle_790MY.SetColumnSpan(this.chkActivo_790MY, 2);
            this.tlpDetalle_790MY.SetColumnSpan(this.lblActivoAyuda_790MY, 2);
            this.tlpDetalle_790MY.SetColumnSpan(this.lblAyuda_790MY, 2);
            this.tlpDetalle_790MY.TabIndex = 0;
            //
            // lblDetalleTitulo_790MY
            //
            this.lblDetalleTitulo_790MY.AutoSize = true;
            this.lblDetalleTitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblDetalleTitulo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(26)))), ((int)(((byte)(46)))));
            this.lblDetalleTitulo_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblDetalleTitulo_790MY.Name = "lblDetalleTitulo_790MY";
            this.lblDetalleTitulo_790MY.TabIndex = 0;
            this.lblDetalleTitulo_790MY.Tag = "FP_detalle_titulo";
            this.lblDetalleTitulo_790MY.Text = "Datos del producto";
            //
            // chipModo_790MY
            //
            this.chipModo_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chipModo_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(232)))), ((int)(((byte)(222)))));
            this.chipModo_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.chipModo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(72)))), ((int)(((byte)(66)))));
            this.chipModo_790MY.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.chipModo_790MY.Name = "chipModo_790MY";
            this.chipModo_790MY.TabIndex = 1;
            this.chipModo_790MY.Text = "Consulta";
            //
            // lblId_790MY
            //
            this.lblId_790MY.AutoSize = true;
            this.lblId_790MY.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblId_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblId_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblId_790MY.Name = "lblId_790MY";
            this.lblId_790MY.TabIndex = 2;
            this.lblId_790MY.Text = "ID: -";
            //
            // lblNombre_790MY
            //
            this.lblNombre_790MY.AutoSize = true;
            this.lblNombre_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblNombre_790MY.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblNombre_790MY.Name = "lblNombre_790MY";
            this.lblNombre_790MY.TabIndex = 3;
            this.lblNombre_790MY.Tag = "FP_lblNombre";
            this.lblNombre_790MY.Text = "Nombre";
            //
            // txtNombre_790MY
            //
            this.txtNombre_790MY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNombre_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNombre_790MY.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtNombre_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.txtNombre_790MY.MaxLength = 100;
            this.txtNombre_790MY.Name = "txtNombre_790MY";
            this.txtNombre_790MY.TabIndex = 4;
            //
            // lblCategoria_790MY
            //
            this.lblCategoria_790MY.AutoSize = true;
            this.lblCategoria_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCategoria_790MY.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblCategoria_790MY.Name = "lblCategoria_790MY";
            this.lblCategoria_790MY.TabIndex = 5;
            this.lblCategoria_790MY.Tag = "FP_lblCategoria";
            this.lblCategoria_790MY.Text = "Categoría";
            //
            // lblPrecio_790MY
            //
            this.lblPrecio_790MY.AutoSize = true;
            this.lblPrecio_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPrecio_790MY.Margin = new System.Windows.Forms.Padding(6, 4, 0, 4);
            this.lblPrecio_790MY.Name = "lblPrecio_790MY";
            this.lblPrecio_790MY.TabIndex = 6;
            this.lblPrecio_790MY.Tag = "FP_lblPrecio";
            this.lblPrecio_790MY.Text = "Precio ($)";
            //
            // cmbCategoria_790MY
            //
            this.cmbCategoria_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbCategoria_790MY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategoria_790MY.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.cmbCategoria_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 6, 10);
            this.cmbCategoria_790MY.Name = "cmbCategoria_790MY";
            this.cmbCategoria_790MY.TabIndex = 7;
            //
            // nudPrecio_790MY
            //
            this.nudPrecio_790MY.DecimalPlaces = 2;
            this.nudPrecio_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudPrecio_790MY.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.nudPrecio_790MY.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            this.nudPrecio_790MY.Margin = new System.Windows.Forms.Padding(6, 0, 0, 10);
            this.nudPrecio_790MY.Maximum = new decimal(new int[] { 99999999, 0, 0, 0 });
            this.nudPrecio_790MY.Name = "nudPrecio_790MY";
            this.nudPrecio_790MY.TabIndex = 8;
            this.nudPrecio_790MY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudPrecio_790MY.ThousandsSeparator = true;
            //
            // lblStock_790MY
            //
            this.lblStock_790MY.AutoSize = true;
            this.lblStock_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStock_790MY.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblStock_790MY.Name = "lblStock_790MY";
            this.lblStock_790MY.TabIndex = 9;
            this.lblStock_790MY.Tag = "FP_lblStock";
            this.lblStock_790MY.Text = "Stock físico";
            //
            // lblComprometido_790MY
            //
            this.lblComprometido_790MY.AutoSize = true;
            this.lblComprometido_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblComprometido_790MY.Margin = new System.Windows.Forms.Padding(6, 4, 0, 4);
            this.lblComprometido_790MY.Name = "lblComprometido_790MY";
            this.lblComprometido_790MY.TabIndex = 10;
            this.lblComprometido_790MY.Tag = "FP_lblComprometido";
            this.lblComprometido_790MY.Text = "Comprometido";
            //
            // nudStock_790MY
            //
            this.nudStock_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudStock_790MY.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.nudStock_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 6, 4);
            this.nudStock_790MY.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            this.nudStock_790MY.Name = "nudStock_790MY";
            this.nudStock_790MY.TabIndex = 11;
            this.nudStock_790MY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            //
            // txtComprometido_790MY
            //
            this.txtComprometido_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(240)))), ((int)(((byte)(233)))));
            this.txtComprometido_790MY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtComprometido_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtComprometido_790MY.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtComprometido_790MY.Margin = new System.Windows.Forms.Padding(6, 0, 0, 4);
            this.txtComprometido_790MY.Name = "txtComprometido_790MY";
            this.txtComprometido_790MY.ReadOnly = true;
            this.txtComprometido_790MY.TabIndex = 12;
            this.txtComprometido_790MY.TabStop = false;
            this.txtComprometido_790MY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            //
            // lblStockAyuda_790MY
            //
            this.lblStockAyuda_790MY.AutoSize = true;
            this.lblStockAyuda_790MY.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStockAyuda_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblStockAyuda_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.lblStockAyuda_790MY.MaximumSize = new System.Drawing.Size(340, 0);
            this.lblStockAyuda_790MY.Name = "lblStockAyuda_790MY";
            this.lblStockAyuda_790MY.TabIndex = 13;
            this.lblStockAyuda_790MY.Tag = "FP_lblStockAyuda";
            this.lblStockAyuda_790MY.Text = "Porciones físicas. Lo comprometido por pedidos sin despachar no se puede quitar.";
            //
            // chkControlaStock_790MY
            //
            this.chkControlaStock_790MY.AutoSize = true;
            this.chkControlaStock_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.chkControlaStock_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.chkControlaStock_790MY.Name = "chkControlaStock_790MY";
            this.chkControlaStock_790MY.TabIndex = 14;
            this.chkControlaStock_790MY.Tag = "FP_chkControlaStock";
            this.chkControlaStock_790MY.Text = "Controla stock";
            this.chkControlaStock_790MY.CheckedChanged += new System.EventHandler(this.chkControlaStock_790MY_CheckedChanged);
            //
            // chkActivo_790MY
            //
            this.chkActivo_790MY.AutoSize = true;
            this.chkActivo_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.chkActivo_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.chkActivo_790MY.Name = "chkActivo_790MY";
            this.chkActivo_790MY.TabIndex = 15;
            this.chkActivo_790MY.Tag = "FP_chkActivo";
            this.chkActivo_790MY.Text = "Activo";
            //
            // lblActivoAyuda_790MY
            //
            this.lblActivoAyuda_790MY.AutoSize = true;
            this.lblActivoAyuda_790MY.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblActivoAyuda_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblActivoAyuda_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.lblActivoAyuda_790MY.MaximumSize = new System.Drawing.Size(340, 0);
            this.lblActivoAyuda_790MY.Name = "lblActivoAyuda_790MY";
            this.lblActivoAyuda_790MY.TabIndex = 16;
            this.lblActivoAyuda_790MY.Tag = "FP_lblActivoAyuda";
            this.lblActivoAyuda_790MY.Text = "Los productos inactivos no aparecen en la carta de Registrar Pedido.";
            //
            // btnGuardar_790MY
            //
            this.btnGuardar_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGuardar_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnGuardar_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 6, 10);
            this.btnGuardar_790MY.MinimumSize = new System.Drawing.Size(0, 40);
            this.btnGuardar_790MY.Name = "btnGuardar_790MY";
            this.btnGuardar_790MY.TabIndex = 17;
            this.btnGuardar_790MY.Tag = "FP_btnGuardar";
            this.btnGuardar_790MY.Text = "Guardar";
            this.btnGuardar_790MY.UseVisualStyleBackColor = false;
            this.btnGuardar_790MY.Click += new System.EventHandler(this.btnGuardar_790MY_Click);
            //
            // btnCancelar_790MY
            //
            this.btnCancelar_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancelar_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnCancelar_790MY.Margin = new System.Windows.Forms.Padding(6, 0, 0, 10);
            this.btnCancelar_790MY.MinimumSize = new System.Drawing.Size(0, 40);
            this.btnCancelar_790MY.Name = "btnCancelar_790MY";
            this.btnCancelar_790MY.TabIndex = 18;
            this.btnCancelar_790MY.Tag = "FP_btnCancelar";
            this.btnCancelar_790MY.Text = "Cancelar";
            this.btnCancelar_790MY.UseVisualStyleBackColor = false;
            this.btnCancelar_790MY.Click += new System.EventHandler(this.btnCancelar_790MY_Click);
            //
            // lblAyuda_790MY
            //
            this.lblAyuda_790MY.AutoSize = true;
            this.lblAyuda_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblAyuda_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblAyuda_790MY.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.lblAyuda_790MY.MaximumSize = new System.Drawing.Size(340, 0);
            this.lblAyuda_790MY.Name = "lblAyuda_790MY";
            this.lblAyuda_790MY.TabIndex = 19;
            this.lblAyuda_790MY.Tag = "FP_ayuda";
            this.lblAyuda_790MY.Text = "Seleccione un producto de la lista o presione Añadir.";
            //
            // dgvProductos_790MY
            //
            this.dgvProductos_790MY.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId_790MY,
            this.colNombre_790MY,
            this.colCategoria_790MY,
            this.colPrecio_790MY,
            this.colStock_790MY,
            this.colControlaStock_790MY,
            this.colEstado_790MY});
            this.dgvProductos_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProductos_790MY.Name = "dgvProductos_790MY";
            this.dgvProductos_790MY.TabIndex = 1;
            this.dgvProductos_790MY.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvProductos_790MY_CellPainting);
            this.dgvProductos_790MY.SelectionChanged += new System.EventHandler(this.dgvProductos_790MY_SelectionChanged);
            //
            // colId_790MY
            //
            this.colId_790MY.FillWeight = 45F;
            this.colId_790MY.HeaderText = "ID";
            this.colId_790MY.Name = "colId_790MY";
            this.colId_790MY.Tag = "FP_colId";
            //
            // colNombre_790MY
            //
            this.colNombre_790MY.FillWeight = 170F;
            this.colNombre_790MY.HeaderText = "Nombre";
            this.colNombre_790MY.Name = "colNombre_790MY";
            this.colNombre_790MY.Tag = "FP_colNombre";
            //
            // colCategoria_790MY
            //
            this.colCategoria_790MY.FillWeight = 100F;
            this.colCategoria_790MY.HeaderText = "Categoría";
            this.colCategoria_790MY.Name = "colCategoria_790MY";
            this.colCategoria_790MY.Tag = "FP_colCategoria";
            //
            // colPrecio_790MY
            //
            this.colPrecio_790MY.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colPrecio_790MY.FillWeight = 80F;
            this.colPrecio_790MY.HeaderText = "Precio vigente";
            this.colPrecio_790MY.Name = "colPrecio_790MY";
            this.colPrecio_790MY.Tag = "FP_colPrecio";
            //
            // colStock_790MY
            //
            this.colStock_790MY.FillWeight = 95F;
            this.colStock_790MY.HeaderText = "Stock disponible";
            this.colStock_790MY.Name = "colStock_790MY";
            this.colStock_790MY.Tag = "FP_colStock";
            //
            // colControlaStock_790MY
            //
            this.colControlaStock_790MY.FillWeight = 70F;
            this.colControlaStock_790MY.HeaderText = "Controla stock";
            this.colControlaStock_790MY.Name = "colControlaStock_790MY";
            this.colControlaStock_790MY.Tag = "FP_colControlaStock";
            //
            // colEstado_790MY
            //
            this.colEstado_790MY.FillWeight = 75F;
            this.colEstado_790MY.HeaderText = "Estado";
            this.colEstado_790MY.Name = "colEstado_790MY";
            this.colEstado_790MY.Tag = "FP_colEstado";
            //
            // FormProductos_790MY
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.dgvProductos_790MY);
            this.Controls.Add(this.pnlDetalle_790MY);
            this.Controls.Add(this.pnlEstado_790MY);
            this.Controls.Add(this.pnlBarra_790MY);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormProductos_790MY";
            this.Text = "Gestión de productos";
            this.Load += new System.EventHandler(this.FormProductos_790MY_Load);
            this.pnlBarra_790MY.ResumeLayout(false);
            this.pnlBarra_790MY.PerformLayout();
            this.pnlEstado_790MY.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudPrecio_790MY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudStock_790MY)).EndInit();
            this.tlpDetalle_790MY.ResumeLayout(false);
            this.tlpDetalle_790MY.PerformLayout();
            this.pnlDetalle_790MY.ResumeLayout(false);
            this.pnlDetalle_790MY.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductos_790MY)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBarra_790MY;
        private System.Windows.Forms.TextBox txtBuscar_790MY;
        private System.Windows.Forms.Label lblFiltroCategoria_790MY;
        private System.Windows.Forms.ComboBox cmbFiltroCategoria_790MY;
        private System.Windows.Forms.CheckBox chkSoloActivos_790MY;
        private System.Windows.Forms.Button btnAnadir_790MY;
        private System.Windows.Forms.Button btnModificar_790MY;
        private System.Windows.Forms.Button btnBaja_790MY;
        private System.Windows.Forms.Panel pnlEstado_790MY;
        private System.Windows.Forms.Label lblResumen_790MY;
        private System.Windows.Forms.Label lblModo_790MY;
        private System.Windows.Forms.Panel pnlDetalle_790MY;
        private System.Windows.Forms.TableLayoutPanel tlpDetalle_790MY;
        private System.Windows.Forms.Label lblDetalleTitulo_790MY;
        private GUI_08YS.RF2.EtiquetaChip_790MY chipModo_790MY;
        private System.Windows.Forms.Label lblId_790MY;
        private System.Windows.Forms.Label lblNombre_790MY;
        private System.Windows.Forms.TextBox txtNombre_790MY;
        private System.Windows.Forms.Label lblCategoria_790MY;
        private System.Windows.Forms.Label lblPrecio_790MY;
        private System.Windows.Forms.ComboBox cmbCategoria_790MY;
        private System.Windows.Forms.NumericUpDown nudPrecio_790MY;
        private System.Windows.Forms.Label lblStock_790MY;
        private System.Windows.Forms.Label lblComprometido_790MY;
        private System.Windows.Forms.NumericUpDown nudStock_790MY;
        private System.Windows.Forms.TextBox txtComprometido_790MY;
        private System.Windows.Forms.Label lblStockAyuda_790MY;
        private System.Windows.Forms.CheckBox chkControlaStock_790MY;
        private System.Windows.Forms.CheckBox chkActivo_790MY;
        private System.Windows.Forms.Label lblActivoAyuda_790MY;
        private System.Windows.Forms.Button btnGuardar_790MY;
        private System.Windows.Forms.Button btnCancelar_790MY;
        private System.Windows.Forms.Label lblAyuda_790MY;
        private System.Windows.Forms.DataGridView dgvProductos_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategoria_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrecio_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStock_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colControlaStock_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstado_790MY;
    }
}
