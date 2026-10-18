namespace GUI_08YS.Maestros
{
    partial class FormEmpleados_790MY
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
            this.pnlBarra_790MY = new System.Windows.Forms.Panel();
            this.bnrAvisos_790MY = new GUI_08YS.Estilo.Banner_UC_790MY();
            this.errValidacion_790MY = new System.Windows.Forms.ErrorProvider(this.components);
            this.txtBuscar_790MY = new System.Windows.Forms.TextBox();
            this.lblFiltroCargo_790MY = new System.Windows.Forms.Label();
            this.cmbFiltroCargo_790MY = new System.Windows.Forms.ComboBox();
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
            this.lblDni_790MY = new System.Windows.Forms.Label();
            this.txtDni_790MY = new System.Windows.Forms.TextBox();
            this.lblNombre_790MY = new System.Windows.Forms.Label();
            this.lblApellido_790MY = new System.Windows.Forms.Label();
            this.txtNombre_790MY = new System.Windows.Forms.TextBox();
            this.txtApellido_790MY = new System.Windows.Forms.TextBox();
            this.lblCargo_790MY = new System.Windows.Forms.Label();
            this.cmbCargo_790MY = new System.Windows.Forms.ComboBox();
            this.chkActivo_790MY = new System.Windows.Forms.CheckBox();
            this.lblAviso_790MY = new System.Windows.Forms.Label();
            this.lblUsuario_790MY = new System.Windows.Forms.Label();
            this.cmbUsuario_790MY = new System.Windows.Forms.ComboBox();
            this.lblUsuarioAyuda_790MY = new System.Windows.Forms.Label();
            this.btnGuardar_790MY = new System.Windows.Forms.Button();
            this.btnCancelar_790MY = new System.Windows.Forms.Button();
            this.lblAyuda_790MY = new System.Windows.Forms.Label();
            this.dgvEmpleados_790MY = new System.Windows.Forms.DataGridView();
            this.colDni_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colApellido_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCargo_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUsuario_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstado_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlBarra_790MY.SuspendLayout();
            this.pnlEstado_790MY.SuspendLayout();
            this.pnlDetalle_790MY.SuspendLayout();
            this.tlpDetalle_790MY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmpleados_790MY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errValidacion_790MY)).BeginInit();
            this.SuspendLayout();
            //
            // pnlBarra_790MY
            //
            this.pnlBarra_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(240)))), ((int)(((byte)(233)))));
            this.pnlBarra_790MY.Controls.Add(this.txtBuscar_790MY);
            this.pnlBarra_790MY.Controls.Add(this.lblFiltroCargo_790MY);
            this.pnlBarra_790MY.Controls.Add(this.cmbFiltroCargo_790MY);
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
            // lblFiltroCargo_790MY
            //
            this.lblFiltroCargo_790MY.AutoSize = true;
            this.lblFiltroCargo_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblFiltroCargo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblFiltroCargo_790MY.Location = new System.Drawing.Point(322, 22);
            this.lblFiltroCargo_790MY.Name = "lblFiltroCargo_790MY";
            this.lblFiltroCargo_790MY.TabIndex = 1;
            this.lblFiltroCargo_790MY.Tag = "FE_lblFiltroCargo";
            this.lblFiltroCargo_790MY.Text = "Cargo";
            //
            // cmbFiltroCargo_790MY
            //
            this.cmbFiltroCargo_790MY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFiltroCargo_790MY.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.cmbFiltroCargo_790MY.Location = new System.Drawing.Point(384, 17);
            this.cmbFiltroCargo_790MY.Name = "cmbFiltroCargo_790MY";
            this.cmbFiltroCargo_790MY.Size = new System.Drawing.Size(180, 28);
            this.cmbFiltroCargo_790MY.TabIndex = 2;
            this.cmbFiltroCargo_790MY.SelectedIndexChanged += new System.EventHandler(this.Filtro_Changed);
            //
            // chkSoloActivos_790MY
            //
            this.chkSoloActivos_790MY.AutoSize = true;
            this.chkSoloActivos_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.chkSoloActivos_790MY.Location = new System.Drawing.Point(584, 21);
            this.chkSoloActivos_790MY.Name = "chkSoloActivos_790MY";
            this.chkSoloActivos_790MY.TabIndex = 3;
            this.chkSoloActivos_790MY.Tag = "FE_chkSoloActivos";
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
            this.btnAnadir_790MY.Tag = "FE_btnAnadir";
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
            this.btnModificar_790MY.Tag = "FE_btnModificar";
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
            this.btnBaja_790MY.Tag = "FE_btnBaja";
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
            this.tlpDetalle_790MY.ColumnCount = 2;
            this.tlpDetalle_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpDetalle_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpDetalle_790MY.Controls.Add(this.lblDetalleTitulo_790MY, 0, 0);
            this.tlpDetalle_790MY.Controls.Add(this.chipModo_790MY, 1, 0);
            this.tlpDetalle_790MY.Controls.Add(this.lblDni_790MY, 0, 1);
            this.tlpDetalle_790MY.Controls.Add(this.txtDni_790MY, 0, 2);
            this.tlpDetalle_790MY.Controls.Add(this.lblNombre_790MY, 0, 3);
            this.tlpDetalle_790MY.Controls.Add(this.lblApellido_790MY, 1, 3);
            this.tlpDetalle_790MY.Controls.Add(this.txtNombre_790MY, 0, 4);
            this.tlpDetalle_790MY.Controls.Add(this.txtApellido_790MY, 1, 4);
            this.tlpDetalle_790MY.Controls.Add(this.lblCargo_790MY, 0, 5);
            this.tlpDetalle_790MY.Controls.Add(this.cmbCargo_790MY, 0, 6);
            this.tlpDetalle_790MY.Controls.Add(this.chkActivo_790MY, 0, 7);
            this.tlpDetalle_790MY.Controls.Add(this.lblAviso_790MY, 0, 8);
            this.tlpDetalle_790MY.Controls.Add(this.lblUsuario_790MY, 0, 9);
            this.tlpDetalle_790MY.Controls.Add(this.cmbUsuario_790MY, 0, 10);
            this.tlpDetalle_790MY.Controls.Add(this.lblUsuarioAyuda_790MY, 0, 11);
            this.tlpDetalle_790MY.Controls.Add(this.btnGuardar_790MY, 0, 12);
            this.tlpDetalle_790MY.Controls.Add(this.btnCancelar_790MY, 1, 12);
            this.tlpDetalle_790MY.Controls.Add(this.lblAyuda_790MY, 0, 13);
            this.tlpDetalle_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpDetalle_790MY.AutoSize = true;
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
            this.tlpDetalle_790MY.SetColumnSpan(this.txtDni_790MY, 2);
            this.tlpDetalle_790MY.SetColumnSpan(this.lblDni_790MY, 2);
            this.tlpDetalle_790MY.SetColumnSpan(this.lblCargo_790MY, 2);
            this.tlpDetalle_790MY.SetColumnSpan(this.cmbCargo_790MY, 2);
            this.tlpDetalle_790MY.SetColumnSpan(this.chkActivo_790MY, 2);
            this.tlpDetalle_790MY.SetColumnSpan(this.lblAviso_790MY, 2);
            this.tlpDetalle_790MY.SetColumnSpan(this.lblUsuario_790MY, 2);
            this.tlpDetalle_790MY.SetColumnSpan(this.cmbUsuario_790MY, 2);
            this.tlpDetalle_790MY.SetColumnSpan(this.lblUsuarioAyuda_790MY, 2);
            this.tlpDetalle_790MY.SetColumnSpan(this.lblAyuda_790MY, 2);
            this.tlpDetalle_790MY.TabIndex = 0;
            //
            // lblDetalleTitulo_790MY
            //
            this.lblDetalleTitulo_790MY.AutoSize = true;
            this.lblDetalleTitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblDetalleTitulo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(26)))), ((int)(((byte)(46)))));
            this.lblDetalleTitulo_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.lblDetalleTitulo_790MY.Name = "lblDetalleTitulo_790MY";
            this.lblDetalleTitulo_790MY.TabIndex = 0;
            this.lblDetalleTitulo_790MY.Tag = "FE_detalle_titulo";
            this.lblDetalleTitulo_790MY.Text = "Datos del empleado";
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
            // lblDni_790MY
            //
            this.lblDni_790MY.AutoSize = true;
            this.lblDni_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDni_790MY.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblDni_790MY.Name = "lblDni_790MY";
            this.lblDni_790MY.TabIndex = 2;
            this.lblDni_790MY.Tag = "FE_lblDni";
            this.lblDni_790MY.Text = "DNI";
            //
            // txtDni_790MY
            //
            this.txtDni_790MY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDni_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDni_790MY.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtDni_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.txtDni_790MY.MaxLength = 9;
            this.txtDni_790MY.Name = "txtDni_790MY";
            this.txtDni_790MY.TabIndex = 3;
            this.txtDni_790MY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDni_790MY_KeyPress);
            //
            // lblNombre_790MY
            //
            this.lblNombre_790MY.AutoSize = true;
            this.lblNombre_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblNombre_790MY.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblNombre_790MY.Name = "lblNombre_790MY";
            this.lblNombre_790MY.TabIndex = 4;
            this.lblNombre_790MY.Tag = "FE_lblNombre";
            this.lblNombre_790MY.Text = "Nombre";
            //
            // lblApellido_790MY
            //
            this.lblApellido_790MY.AutoSize = true;
            this.lblApellido_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblApellido_790MY.Margin = new System.Windows.Forms.Padding(6, 4, 0, 4);
            this.lblApellido_790MY.Name = "lblApellido_790MY";
            this.lblApellido_790MY.TabIndex = 5;
            this.lblApellido_790MY.Tag = "FE_lblApellido";
            this.lblApellido_790MY.Text = "Apellido";
            //
            // txtNombre_790MY
            //
            this.txtNombre_790MY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNombre_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNombre_790MY.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtNombre_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 6, 10);
            this.txtNombre_790MY.MaxLength = 100;
            this.txtNombre_790MY.Name = "txtNombre_790MY";
            this.txtNombre_790MY.TabIndex = 6;
            //
            // txtApellido_790MY
            //
            this.txtApellido_790MY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtApellido_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtApellido_790MY.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtApellido_790MY.Margin = new System.Windows.Forms.Padding(6, 0, 0, 10);
            this.txtApellido_790MY.MaxLength = 100;
            this.txtApellido_790MY.Name = "txtApellido_790MY";
            this.txtApellido_790MY.TabIndex = 7;
            //
            // lblCargo_790MY
            //
            this.lblCargo_790MY.AutoSize = true;
            this.lblCargo_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCargo_790MY.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblCargo_790MY.Name = "lblCargo_790MY";
            this.lblCargo_790MY.TabIndex = 8;
            this.lblCargo_790MY.Tag = "FE_lblCargo";
            this.lblCargo_790MY.Text = "Cargo";
            //
            // cmbCargo_790MY
            //
            this.cmbCargo_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbCargo_790MY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCargo_790MY.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.cmbCargo_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.cmbCargo_790MY.Name = "cmbCargo_790MY";
            this.cmbCargo_790MY.TabIndex = 9;
            //
            // chkActivo_790MY
            //
            this.chkActivo_790MY.AutoSize = true;
            this.chkActivo_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.chkActivo_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.chkActivo_790MY.Name = "chkActivo_790MY";
            this.chkActivo_790MY.TabIndex = 10;
            this.chkActivo_790MY.Tag = "FE_chkActivo";
            this.chkActivo_790MY.Text = "Activo";
            //
            // lblAviso_790MY
            //
            this.lblAviso_790MY.AutoSize = true;
            this.lblAviso_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(236)))), ((int)(((byte)(214)))));
            this.lblAviso_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAviso_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblAviso_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(90)))), ((int)(((byte)(20)))));
            this.lblAviso_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 12);
            this.lblAviso_790MY.MaximumSize = new System.Drawing.Size(340, 0);
            this.lblAviso_790MY.Name = "lblAviso_790MY";
            this.lblAviso_790MY.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.lblAviso_790MY.TabIndex = 11;
            this.lblAviso_790MY.Visible = false;
            //
            // lblUsuario_790MY
            //
            this.lblUsuario_790MY.AutoSize = true;
            this.lblUsuario_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUsuario_790MY.Margin = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.lblUsuario_790MY.Name = "lblUsuario_790MY";
            this.lblUsuario_790MY.TabIndex = 12;
            this.lblUsuario_790MY.Tag = "FE_lblUsuario";
            this.lblUsuario_790MY.Text = "Usuario del sistema (opcional)";
            //
            // cmbUsuario_790MY
            //
            this.cmbUsuario_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbUsuario_790MY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUsuario_790MY.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.cmbUsuario_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.cmbUsuario_790MY.Name = "cmbUsuario_790MY";
            this.cmbUsuario_790MY.TabIndex = 13;
            //
            // lblUsuarioAyuda_790MY
            //
            this.lblUsuarioAyuda_790MY.AutoSize = true;
            this.lblUsuarioAyuda_790MY.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblUsuarioAyuda_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblUsuarioAyuda_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.lblUsuarioAyuda_790MY.MaximumSize = new System.Drawing.Size(340, 0);
            this.lblUsuarioAyuda_790MY.Name = "lblUsuarioAyuda_790MY";
            this.lblUsuarioAyuda_790MY.TabIndex = 14;
            this.lblUsuarioAyuda_790MY.Tag = "FE_lblUsuarioAyuda";
            this.lblUsuarioAyuda_790MY.Text = "Sin usuario vinculado, el empleado no podrá iniciar sesión.";
            //
            // btnGuardar_790MY
            //
            this.btnGuardar_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGuardar_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnGuardar_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 6, 10);
            this.btnGuardar_790MY.MinimumSize = new System.Drawing.Size(0, 40);
            this.btnGuardar_790MY.Name = "btnGuardar_790MY";
            this.btnGuardar_790MY.TabIndex = 15;
            this.btnGuardar_790MY.Tag = "FE_btnGuardar";
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
            this.btnCancelar_790MY.TabIndex = 16;
            this.btnCancelar_790MY.Tag = "FE_btnCancelar";
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
            this.lblAyuda_790MY.TabIndex = 17;
            this.lblAyuda_790MY.Tag = "FE_ayuda";
            this.lblAyuda_790MY.Text = "Seleccione un empleado de la lista o presione Añadir.";
            //
            // dgvEmpleados_790MY
            //
            this.dgvEmpleados_790MY.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDni_790MY,
            this.colNombre_790MY,
            this.colApellido_790MY,
            this.colCargo_790MY,
            this.colUsuario_790MY,
            this.colEstado_790MY});
            this.dgvEmpleados_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEmpleados_790MY.Name = "dgvEmpleados_790MY";
            this.dgvEmpleados_790MY.TabIndex = 1;
            this.dgvEmpleados_790MY.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvEmpleados_790MY_CellPainting);
            this.dgvEmpleados_790MY.SelectionChanged += new System.EventHandler(this.dgvEmpleados_790MY_SelectionChanged);
            //
            // colDni_790MY
            //
            this.colDni_790MY.FillWeight = 80F;
            this.colDni_790MY.HeaderText = "DNI";
            this.colDni_790MY.Name = "colDni_790MY";
            this.colDni_790MY.Tag = "FE_colDni";
            //
            // colNombre_790MY
            //
            this.colNombre_790MY.FillWeight = 100F;
            this.colNombre_790MY.HeaderText = "Nombre";
            this.colNombre_790MY.Name = "colNombre_790MY";
            this.colNombre_790MY.Tag = "FE_colNombre";
            //
            // colApellido_790MY
            //
            this.colApellido_790MY.FillWeight = 100F;
            this.colApellido_790MY.HeaderText = "Apellido";
            this.colApellido_790MY.Name = "colApellido_790MY";
            this.colApellido_790MY.Tag = "FE_colApellido";
            //
            // colCargo_790MY
            //
            this.colCargo_790MY.FillWeight = 90F;
            this.colCargo_790MY.HeaderText = "Cargo";
            this.colCargo_790MY.Name = "colCargo_790MY";
            this.colCargo_790MY.Tag = "FE_colCargo";
            //
            // colUsuario_790MY
            //
            this.colUsuario_790MY.FillWeight = 100F;
            this.colUsuario_790MY.HeaderText = "Usuario vinculado";
            this.colUsuario_790MY.Name = "colUsuario_790MY";
            this.colUsuario_790MY.Tag = "FE_colUsuario";
            //
            // colEstado_790MY
            //
            this.colEstado_790MY.FillWeight = 80F;
            this.colEstado_790MY.HeaderText = "Estado";
            this.colEstado_790MY.Name = "colEstado_790MY";
            this.colEstado_790MY.Tag = "FE_colEstado";
            //
            // bnrAvisos_790MY
            //
            this.bnrAvisos_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.bnrAvisos_790MY.Name = "bnrAvisos_790MY";
            this.bnrAvisos_790MY.TabIndex = 4;
            //
            // errValidacion_790MY
            //
            this.errValidacion_790MY.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errValidacion_790MY.ContainerControl = this;
            this.errValidacion_790MY.SetIconAlignment(this.txtDni_790MY, System.Windows.Forms.ErrorIconAlignment.MiddleRight);
            this.errValidacion_790MY.SetIconPadding(this.txtDni_790MY, -20);
            //
            // FormEmpleados_790MY
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(1200, 700);
            this.Controls.Add(this.dgvEmpleados_790MY);
            this.Controls.Add(this.pnlDetalle_790MY);
            this.Controls.Add(this.pnlEstado_790MY);
            this.Controls.Add(this.bnrAvisos_790MY);
            this.Controls.Add(this.pnlBarra_790MY);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormEmpleados_790MY";
            this.Text = "Gestión de empleados";
            this.Load += new System.EventHandler(this.FormEmpleados_790MY_Load);
            this.pnlBarra_790MY.ResumeLayout(false);
            this.pnlBarra_790MY.PerformLayout();
            this.pnlEstado_790MY.ResumeLayout(false);
            this.tlpDetalle_790MY.ResumeLayout(false);
            this.tlpDetalle_790MY.PerformLayout();
            this.pnlDetalle_790MY.ResumeLayout(false);
            this.pnlDetalle_790MY.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEmpleados_790MY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errValidacion_790MY)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBarra_790MY;
        private System.Windows.Forms.TextBox txtBuscar_790MY;
        private System.Windows.Forms.Label lblFiltroCargo_790MY;
        private System.Windows.Forms.ComboBox cmbFiltroCargo_790MY;
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
        private System.Windows.Forms.Label lblDni_790MY;
        private System.Windows.Forms.TextBox txtDni_790MY;
        private System.Windows.Forms.Label lblNombre_790MY;
        private System.Windows.Forms.Label lblApellido_790MY;
        private System.Windows.Forms.TextBox txtNombre_790MY;
        private System.Windows.Forms.TextBox txtApellido_790MY;
        private System.Windows.Forms.Label lblCargo_790MY;
        private System.Windows.Forms.ComboBox cmbCargo_790MY;
        private System.Windows.Forms.CheckBox chkActivo_790MY;
        private System.Windows.Forms.Label lblAviso_790MY;
        private System.Windows.Forms.Label lblUsuario_790MY;
        private System.Windows.Forms.ComboBox cmbUsuario_790MY;
        private System.Windows.Forms.Label lblUsuarioAyuda_790MY;
        private System.Windows.Forms.Button btnGuardar_790MY;
        private System.Windows.Forms.Button btnCancelar_790MY;
        private System.Windows.Forms.Label lblAyuda_790MY;
        private System.Windows.Forms.DataGridView dgvEmpleados_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDni_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colApellido_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCargo_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUsuario_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstado_790MY;
        private GUI_08YS.Estilo.Banner_UC_790MY bnrAvisos_790MY;
        private System.Windows.Forms.ErrorProvider errValidacion_790MY;
    }
}
