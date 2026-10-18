namespace GUI_08YS.Maestros
{
    partial class FormClientes_790MY
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
            this.tmrBusqueda_790MY = new System.Windows.Forms.Timer(this.components);
            this.pnlEncabezado_790MY = new System.Windows.Forms.Panel();
            this.lblTitulo_790MY = new System.Windows.Forms.Label();
            this.txtBuscar_790MY = new System.Windows.Forms.TextBox();
            this.lblResumen_790MY = new System.Windows.Forms.Label();
            this.btnAnadir_790MY = new System.Windows.Forms.Button();
            this.btnModificar_790MY = new System.Windows.Forms.Button();
            this.btnEliminar_790MY = new System.Windows.Forms.Button();
            this.btnSalir_790MY = new System.Windows.Forms.Button();
            this.pnlLista_790MY = new System.Windows.Forms.Panel();
            this.lblSinResultados_790MY = new System.Windows.Forms.Label();
            this.dgvClientes_790MY = new System.Windows.Forms.DataGridView();
            this.colDni_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colApellidos_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombres_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmail_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCelular_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDireccion_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlAvisoCambios_790MY = new System.Windows.Forms.Panel();
            this.lblAvisoCambios_790MY = new System.Windows.Forms.Label();
            this.btnGuardarCambios_790MY = new System.Windows.Forms.Button();
            this.btnDescartarCambios_790MY = new System.Windows.Forms.Button();
            this.pnlSerializacion_790MY = new System.Windows.Forms.Panel();
            this.grpSerializacion_790MY = new System.Windows.Forms.GroupBox();
            this.lblRutaSerializar_790MY = new System.Windows.Forms.Label();
            this.txtRutaSerializar_790MY = new System.Windows.Forms.TextBox();
            this.btnExaminarSerializar_790MY = new System.Windows.Forms.Button();
            this.btnSerializar_790MY = new System.Windows.Forms.Button();
            this.lblRutaDeserializar_790MY = new System.Windows.Forms.Label();
            this.txtRutaDeserializar_790MY = new System.Windows.Forms.TextBox();
            this.btnExaminarDeserializar_790MY = new System.Windows.Forms.Button();
            this.btnDeserializar_790MY = new System.Windows.Forms.Button();
            this.lstDeserializados_790MY = new System.Windows.Forms.ListBox();
            this.btnLimpiarSerializacion_790MY = new System.Windows.Forms.Button();
            this.btnPlegarSerializacion_790MY = new System.Windows.Forms.Button();
            this.pnlDetalle_790MY = new System.Windows.Forms.Panel();
            this.lblDetalleTitulo_790MY = new System.Windows.Forms.Label();
            this.lblDetalleAyuda_790MY = new System.Windows.Forms.Label();
            this.lblDni_790MY = new System.Windows.Forms.Label();
            this.txtDni_790MY = new System.Windows.Forms.TextBox();
            this.lblApellidos_790MY = new System.Windows.Forms.Label();
            this.txtApellidos_790MY = new System.Windows.Forms.TextBox();
            this.lblNombres_790MY = new System.Windows.Forms.Label();
            this.txtNombres_790MY = new System.Windows.Forms.TextBox();
            this.lblEmail_790MY = new System.Windows.Forms.Label();
            this.txtEmail_790MY = new System.Windows.Forms.TextBox();
            this.txtEmailAdicional_790MY = new System.Windows.Forms.TextBox();
            this.btnEmailsAdicionales_790MY = new System.Windows.Forms.Button();
            this.flpEmailsAdicionales_790MY = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCelular_790MY = new System.Windows.Forms.Label();
            this.txtCelular_790MY = new System.Windows.Forms.TextBox();
            this.txtCelularAdicional_790MY = new System.Windows.Forms.TextBox();
            this.btnTelefonosAdicionales_790MY = new System.Windows.Forms.Button();
            this.flpTelefonosAdicionales_790MY = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDireccion_790MY = new System.Windows.Forms.Label();
            this.txtDireccion_790MY = new System.Windows.Forms.TextBox();
            this.btnAplicar_790MY = new System.Windows.Forms.Button();
            this.btnCancelar_790MY = new System.Windows.Forms.Button();
            this.bnrAvisos_790MY = new GUI_08YS.Estilo.Banner_UC_790MY();
            this.errValidacion_790MY = new System.Windows.Forms.ErrorProvider(this.components);
            this.pnlEncabezado_790MY.SuspendLayout();
            this.pnlLista_790MY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes_790MY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errValidacion_790MY)).BeginInit();
            this.pnlAvisoCambios_790MY.SuspendLayout();
            this.pnlSerializacion_790MY.SuspendLayout();
            this.grpSerializacion_790MY.SuspendLayout();
            this.pnlDetalle_790MY.SuspendLayout();
            this.SuspendLayout();
            //
            // tmrBusqueda_790MY
            //
            this.tmrBusqueda_790MY.Interval = 250;
            this.tmrBusqueda_790MY.Tick += new System.EventHandler(this.tmrBusqueda_790MY_Tick);
            //
            // pnlEncabezado_790MY
            //
            this.pnlEncabezado_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.pnlEncabezado_790MY.Controls.Add(this.lblTitulo_790MY);
            this.pnlEncabezado_790MY.Controls.Add(this.txtBuscar_790MY);
            this.pnlEncabezado_790MY.Controls.Add(this.lblResumen_790MY);
            this.pnlEncabezado_790MY.Controls.Add(this.btnAnadir_790MY);
            this.pnlEncabezado_790MY.Controls.Add(this.btnModificar_790MY);
            this.pnlEncabezado_790MY.Controls.Add(this.btnEliminar_790MY);
            this.pnlEncabezado_790MY.Controls.Add(this.btnSalir_790MY);
            this.pnlEncabezado_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEncabezado_790MY.Name = "pnlEncabezado_790MY";
            this.pnlEncabezado_790MY.Size = new System.Drawing.Size(1073, 92);
            this.pnlEncabezado_790MY.TabIndex = 0;
            //
            // lblTitulo_790MY
            //
            this.lblTitulo_790MY.AutoSize = true;
            this.lblTitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblTitulo_790MY.Location = new System.Drawing.Point(16, 10);
            this.lblTitulo_790MY.Name = "lblTitulo_790MY";
            this.lblTitulo_790MY.TabIndex = 0;
            this.lblTitulo_790MY.Tag = "FC_titulo";
            this.lblTitulo_790MY.Text = "Clientes";
            //
            // txtBuscar_790MY
            //
            this.txtBuscar_790MY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBuscar_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.txtBuscar_790MY.Location = new System.Drawing.Point(16, 52);
            this.txtBuscar_790MY.Name = "txtBuscar_790MY";
            this.txtBuscar_790MY.Size = new System.Drawing.Size(360, 26);
            this.txtBuscar_790MY.TabIndex = 1;
            this.txtBuscar_790MY.TextChanged += new System.EventHandler(this.txtBuscar_790MY_TextChanged);
            //
            // lblResumen_790MY
            //
            this.lblResumen_790MY.AutoSize = true;
            this.lblResumen_790MY.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblResumen_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblResumen_790MY.Location = new System.Drawing.Point(388, 57);
            this.lblResumen_790MY.Name = "lblResumen_790MY";
            this.lblResumen_790MY.TabIndex = 2;
            //
            // btnAnadir_790MY
            //
            this.btnAnadir_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnadir_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAnadir_790MY.Location = new System.Drawing.Point(561, 44);
            this.btnAnadir_790MY.Name = "btnAnadir_790MY";
            this.btnAnadir_790MY.Size = new System.Drawing.Size(120, 40);
            this.btnAnadir_790MY.TabIndex = 3;
            this.btnAnadir_790MY.Tag = "btnAnadir";
            this.btnAnadir_790MY.Text = "Añadir";
            this.btnAnadir_790MY.Click += new System.EventHandler(this.btnAnadir_790MY_Click);
            //
            // btnModificar_790MY
            //
            this.btnModificar_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModificar_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnModificar_790MY.Location = new System.Drawing.Point(689, 44);
            this.btnModificar_790MY.Name = "btnModificar_790MY";
            this.btnModificar_790MY.Size = new System.Drawing.Size(120, 40);
            this.btnModificar_790MY.TabIndex = 4;
            this.btnModificar_790MY.Tag = "btnModificar";
            this.btnModificar_790MY.Text = "Modificar";
            this.btnModificar_790MY.Click += new System.EventHandler(this.btnModificar_790MY_Click);
            //
            // btnEliminar_790MY
            //
            this.btnEliminar_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEliminar_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnEliminar_790MY.Location = new System.Drawing.Point(817, 44);
            this.btnEliminar_790MY.Name = "btnEliminar_790MY";
            this.btnEliminar_790MY.Size = new System.Drawing.Size(120, 40);
            this.btnEliminar_790MY.TabIndex = 5;
            this.btnEliminar_790MY.Tag = "btnEliminar";
            this.btnEliminar_790MY.Text = "Eliminar";
            this.btnEliminar_790MY.Click += new System.EventHandler(this.btnEliminar_790MY_Click);
            //
            // btnSalir_790MY
            //
            this.btnSalir_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSalir_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSalir_790MY.Location = new System.Drawing.Point(953, 44);
            this.btnSalir_790MY.Name = "btnSalir_790MY";
            this.btnSalir_790MY.Size = new System.Drawing.Size(104, 40);
            this.btnSalir_790MY.TabIndex = 6;
            this.btnSalir_790MY.Tag = "btnSalir";
            this.btnSalir_790MY.Text = "Salir";
            this.btnSalir_790MY.Click += new System.EventHandler(this.btnSalir_790MY_Click);
            //
            // pnlLista_790MY
            //
            this.pnlLista_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.pnlLista_790MY.Controls.Add(this.lblSinResultados_790MY);
            this.pnlLista_790MY.Controls.Add(this.dgvClientes_790MY);
            this.pnlLista_790MY.Controls.Add(this.pnlAvisoCambios_790MY);
            this.pnlLista_790MY.Controls.Add(this.pnlSerializacion_790MY);
            this.pnlLista_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLista_790MY.Location = new System.Drawing.Point(0, 92);
            this.pnlLista_790MY.Name = "pnlLista_790MY";
            this.pnlLista_790MY.Padding = new System.Windows.Forms.Padding(16, 0, 16, 12);
            this.pnlLista_790MY.Size = new System.Drawing.Size(673, 612);
            this.pnlLista_790MY.TabIndex = 1;
            //
            // lblSinResultados_790MY
            //
            this.lblSinResultados_790MY.AutoSize = true;
            this.lblSinResultados_790MY.BackColor = System.Drawing.Color.White;
            this.lblSinResultados_790MY.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSinResultados_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblSinResultados_790MY.Location = new System.Drawing.Point(28, 52);
            this.lblSinResultados_790MY.Name = "lblSinResultados_790MY";
            this.lblSinResultados_790MY.TabIndex = 0;
            this.lblSinResultados_790MY.Visible = false;
            //
            // dgvClientes_790MY
            //
            this.dgvClientes_790MY.AllowUserToAddRows = false;
            this.dgvClientes_790MY.AllowUserToDeleteRows = false;
            this.dgvClientes_790MY.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvClientes_790MY.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDni_790MY,
            this.colApellidos_790MY,
            this.colNombres_790MY,
            this.colEmail_790MY,
            this.colCelular_790MY,
            this.colDireccion_790MY});
            this.dgvClientes_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvClientes_790MY.MultiSelect = true;
            this.dgvClientes_790MY.Name = "dgvClientes_790MY";
            this.dgvClientes_790MY.ReadOnly = true;
            this.dgvClientes_790MY.RowHeadersVisible = false;
            this.dgvClientes_790MY.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvClientes_790MY.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvClientes_790MY.TabIndex = 1;
            this.dgvClientes_790MY.SelectionChanged += new System.EventHandler(this.dgvClientes_790MY_SelectionChanged);
            //
            // colDni_790MY
            //
            this.colDni_790MY.DataPropertyName = "DNI";
            this.colDni_790MY.FillWeight = 70F;
            this.colDni_790MY.HeaderText = "DNI";
            this.colDni_790MY.Name = "colDni_790MY";
            this.colDni_790MY.ReadOnly = true;
            //
            // colApellidos_790MY
            //
            this.colApellidos_790MY.DataPropertyName = "Apellido";
            this.colApellidos_790MY.FillWeight = 90F;
            this.colApellidos_790MY.HeaderText = "Apellidos";
            this.colApellidos_790MY.Name = "colApellidos_790MY";
            this.colApellidos_790MY.ReadOnly = true;
            //
            // colNombres_790MY
            //
            this.colNombres_790MY.DataPropertyName = "Nombre";
            this.colNombres_790MY.FillWeight = 90F;
            this.colNombres_790MY.HeaderText = "Nombres";
            this.colNombres_790MY.Name = "colNombres_790MY";
            this.colNombres_790MY.ReadOnly = true;
            //
            // colEmail_790MY
            //
            this.colEmail_790MY.DataPropertyName = "Email";
            this.colEmail_790MY.FillWeight = 130F;
            this.colEmail_790MY.HeaderText = "Email";
            this.colEmail_790MY.Name = "colEmail_790MY";
            this.colEmail_790MY.ReadOnly = true;
            //
            // colCelular_790MY
            //
            this.colCelular_790MY.DataPropertyName = "Telefono";
            this.colCelular_790MY.FillWeight = 80F;
            this.colCelular_790MY.HeaderText = "Celular";
            this.colCelular_790MY.Name = "colCelular_790MY";
            this.colCelular_790MY.ReadOnly = true;
            //
            // colDireccion_790MY
            //
            this.colDireccion_790MY.DataPropertyName = "Direccion";
            this.colDireccion_790MY.HeaderText = "Dirección";
            this.colDireccion_790MY.Name = "colDireccion_790MY";
            this.colDireccion_790MY.ReadOnly = true;
            this.colDireccion_790MY.Visible = false;
            //
            // pnlAvisoCambios_790MY
            //
            this.pnlAvisoCambios_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(236)))), ((int)(((byte)(214)))));
            this.pnlAvisoCambios_790MY.Controls.Add(this.lblAvisoCambios_790MY);
            this.pnlAvisoCambios_790MY.Controls.Add(this.btnGuardarCambios_790MY);
            this.pnlAvisoCambios_790MY.Controls.Add(this.btnDescartarCambios_790MY);
            this.pnlAvisoCambios_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAvisoCambios_790MY.Name = "pnlAvisoCambios_790MY";
            this.pnlAvisoCambios_790MY.Size = new System.Drawing.Size(641, 56);
            this.pnlAvisoCambios_790MY.TabIndex = 2;
            this.pnlAvisoCambios_790MY.Visible = false;
            //
            // lblAvisoCambios_790MY
            //
            this.lblAvisoCambios_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAvisoCambios_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblAvisoCambios_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(90)))), ((int)(((byte)(20)))));
            this.lblAvisoCambios_790MY.Location = new System.Drawing.Point(12, 8);
            this.lblAvisoCambios_790MY.Name = "lblAvisoCambios_790MY";
            this.lblAvisoCambios_790MY.Size = new System.Drawing.Size(405, 40);
            this.lblAvisoCambios_790MY.TabIndex = 0;
            this.lblAvisoCambios_790MY.Tag = "FC_aviso_cambios";
            this.lblAvisoCambios_790MY.Text = "⚠ Hay cambios sin guardar en la ficha. ¿Guardarlos antes de cambiar de cliente?";
            this.lblAvisoCambios_790MY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // btnGuardarCambios_790MY
            //
            this.btnGuardarCambios_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGuardarCambios_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnGuardarCambios_790MY.Location = new System.Drawing.Point(429, 12);
            this.btnGuardarCambios_790MY.Name = "btnGuardarCambios_790MY";
            this.btnGuardarCambios_790MY.Size = new System.Drawing.Size(96, 32);
            this.btnGuardarCambios_790MY.TabIndex = 1;
            this.btnGuardarCambios_790MY.Tag = "FC_btnGuardarCambios";
            this.btnGuardarCambios_790MY.Text = "Guardar";
            this.btnGuardarCambios_790MY.Click += new System.EventHandler(this.btnGuardarCambios_790MY_Click);
            //
            // btnDescartarCambios_790MY
            //
            this.btnDescartarCambios_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDescartarCambios_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDescartarCambios_790MY.Location = new System.Drawing.Point(533, 12);
            this.btnDescartarCambios_790MY.Name = "btnDescartarCambios_790MY";
            this.btnDescartarCambios_790MY.Size = new System.Drawing.Size(96, 32);
            this.btnDescartarCambios_790MY.TabIndex = 2;
            this.btnDescartarCambios_790MY.Tag = "FC_btnDescartarCambios";
            this.btnDescartarCambios_790MY.Text = "Descartar";
            this.btnDescartarCambios_790MY.Click += new System.EventHandler(this.btnDescartarCambios_790MY_Click);
            //
            // pnlSerializacion_790MY
            //
            this.pnlSerializacion_790MY.Controls.Add(this.grpSerializacion_790MY);
            this.pnlSerializacion_790MY.Controls.Add(this.btnPlegarSerializacion_790MY);
            this.pnlSerializacion_790MY.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSerializacion_790MY.Name = "pnlSerializacion_790MY";
            this.pnlSerializacion_790MY.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlSerializacion_790MY.Size = new System.Drawing.Size(641, 44);
            this.pnlSerializacion_790MY.TabIndex = 3;
            //
            // btnPlegarSerializacion_790MY
            //
            this.btnPlegarSerializacion_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPlegarSerializacion_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlegarSerializacion_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnPlegarSerializacion_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.btnPlegarSerializacion_790MY.Name = "btnPlegarSerializacion_790MY";
            this.btnPlegarSerializacion_790MY.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnPlegarSerializacion_790MY.Size = new System.Drawing.Size(641, 36);
            this.btnPlegarSerializacion_790MY.TabIndex = 0;
            this.btnPlegarSerializacion_790MY.Text = "▸  Serialización XML";
            this.btnPlegarSerializacion_790MY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPlegarSerializacion_790MY.Click += new System.EventHandler(this.btnPlegarSerializacion_790MY_Click);
            //
            // grpSerializacion_790MY
            //
            this.grpSerializacion_790MY.Controls.Add(this.lblRutaSerializar_790MY);
            this.grpSerializacion_790MY.Controls.Add(this.txtRutaSerializar_790MY);
            this.grpSerializacion_790MY.Controls.Add(this.btnExaminarSerializar_790MY);
            this.grpSerializacion_790MY.Controls.Add(this.btnSerializar_790MY);
            this.grpSerializacion_790MY.Controls.Add(this.lblRutaDeserializar_790MY);
            this.grpSerializacion_790MY.Controls.Add(this.txtRutaDeserializar_790MY);
            this.grpSerializacion_790MY.Controls.Add(this.btnExaminarDeserializar_790MY);
            this.grpSerializacion_790MY.Controls.Add(this.btnDeserializar_790MY);
            this.grpSerializacion_790MY.Controls.Add(this.lstDeserializados_790MY);
            this.grpSerializacion_790MY.Controls.Add(this.btnLimpiarSerializacion_790MY);
            this.grpSerializacion_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSerializacion_790MY.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grpSerializacion_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.grpSerializacion_790MY.Name = "grpSerializacion_790MY";
            this.grpSerializacion_790MY.Size = new System.Drawing.Size(641, 236);
            this.grpSerializacion_790MY.TabIndex = 1;
            this.grpSerializacion_790MY.TabStop = false;
            this.grpSerializacion_790MY.Tag = "FC_grpSerializacion";
            this.grpSerializacion_790MY.Text = "Serialización";
            this.grpSerializacion_790MY.Visible = false;
            //
            // lblRutaSerializar_790MY
            //
            this.lblRutaSerializar_790MY.AutoSize = true;
            this.lblRutaSerializar_790MY.Location = new System.Drawing.Point(12, 24);
            this.lblRutaSerializar_790MY.Name = "lblRutaSerializar_790MY";
            this.lblRutaSerializar_790MY.TabIndex = 0;
            this.lblRutaSerializar_790MY.Tag = "FC_lblRutaSerializar";
            this.lblRutaSerializar_790MY.Text = "Archivo a serializar:";
            //
            // txtRutaSerializar_790MY
            //
            this.txtRutaSerializar_790MY.Location = new System.Drawing.Point(12, 44);
            this.txtRutaSerializar_790MY.Name = "txtRutaSerializar_790MY";
            this.txtRutaSerializar_790MY.ReadOnly = true;
            this.txtRutaSerializar_790MY.Size = new System.Drawing.Size(424, 23);
            this.txtRutaSerializar_790MY.TabIndex = 1;
            //
            // btnExaminarSerializar_790MY
            //
            this.btnExaminarSerializar_790MY.Font = new System.Drawing.Font("Segoe UI Emoji", 9F);
            this.btnExaminarSerializar_790MY.Location = new System.Drawing.Point(442, 43);
            this.btnExaminarSerializar_790MY.Name = "btnExaminarSerializar_790MY";
            this.btnExaminarSerializar_790MY.Size = new System.Drawing.Size(40, 26);
            this.btnExaminarSerializar_790MY.TabIndex = 2;
            this.btnExaminarSerializar_790MY.Text = "📁";
            this.btnExaminarSerializar_790MY.Click += new System.EventHandler(this.btnExaminarSerializar_790MY_Click);
            //
            // btnSerializar_790MY
            //
            this.btnSerializar_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSerializar_790MY.Location = new System.Drawing.Point(492, 40);
            this.btnSerializar_790MY.Name = "btnSerializar_790MY";
            this.btnSerializar_790MY.Size = new System.Drawing.Size(136, 32);
            this.btnSerializar_790MY.TabIndex = 3;
            this.btnSerializar_790MY.Tag = "FC_btnSerializar";
            this.btnSerializar_790MY.Text = "SERIALIZAR";
            this.btnSerializar_790MY.Click += new System.EventHandler(this.btnSerializar_790MY_Click);
            //
            // lblRutaDeserializar_790MY
            //
            this.lblRutaDeserializar_790MY.AutoSize = true;
            this.lblRutaDeserializar_790MY.Location = new System.Drawing.Point(12, 78);
            this.lblRutaDeserializar_790MY.Name = "lblRutaDeserializar_790MY";
            this.lblRutaDeserializar_790MY.TabIndex = 4;
            this.lblRutaDeserializar_790MY.Tag = "FC_lblRutaDeserializar";
            this.lblRutaDeserializar_790MY.Text = "Archivo a deserializar:";
            //
            // txtRutaDeserializar_790MY
            //
            this.txtRutaDeserializar_790MY.Location = new System.Drawing.Point(12, 98);
            this.txtRutaDeserializar_790MY.Name = "txtRutaDeserializar_790MY";
            this.txtRutaDeserializar_790MY.ReadOnly = true;
            this.txtRutaDeserializar_790MY.Size = new System.Drawing.Size(424, 23);
            this.txtRutaDeserializar_790MY.TabIndex = 5;
            //
            // btnExaminarDeserializar_790MY
            //
            this.btnExaminarDeserializar_790MY.Font = new System.Drawing.Font("Segoe UI Emoji", 9F);
            this.btnExaminarDeserializar_790MY.Location = new System.Drawing.Point(442, 97);
            this.btnExaminarDeserializar_790MY.Name = "btnExaminarDeserializar_790MY";
            this.btnExaminarDeserializar_790MY.Size = new System.Drawing.Size(40, 26);
            this.btnExaminarDeserializar_790MY.TabIndex = 6;
            this.btnExaminarDeserializar_790MY.Text = "📁";
            this.btnExaminarDeserializar_790MY.Click += new System.EventHandler(this.btnExaminarDeserializar_790MY_Click);
            //
            // btnDeserializar_790MY
            //
            this.btnDeserializar_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnDeserializar_790MY.Location = new System.Drawing.Point(492, 94);
            this.btnDeserializar_790MY.Name = "btnDeserializar_790MY";
            this.btnDeserializar_790MY.Size = new System.Drawing.Size(136, 32);
            this.btnDeserializar_790MY.TabIndex = 7;
            this.btnDeserializar_790MY.Tag = "FC_btnDeserializar";
            this.btnDeserializar_790MY.Text = "DES-SERIALIZAR";
            this.btnDeserializar_790MY.Click += new System.EventHandler(this.btnDeserializar_790MY_Click);
            //
            // lstDeserializados_790MY
            //
            this.lstDeserializados_790MY.FormattingEnabled = true;
            this.lstDeserializados_790MY.HorizontalScrollbar = true;
            this.lstDeserializados_790MY.ItemHeight = 15;
            this.lstDeserializados_790MY.Location = new System.Drawing.Point(12, 134);
            this.lstDeserializados_790MY.Name = "lstDeserializados_790MY";
            this.lstDeserializados_790MY.Size = new System.Drawing.Size(470, 94);
            this.lstDeserializados_790MY.TabIndex = 8;
            //
            // btnLimpiarSerializacion_790MY
            //
            this.btnLimpiarSerializacion_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLimpiarSerializacion_790MY.Location = new System.Drawing.Point(492, 134);
            this.btnLimpiarSerializacion_790MY.Name = "btnLimpiarSerializacion_790MY";
            this.btnLimpiarSerializacion_790MY.Size = new System.Drawing.Size(136, 32);
            this.btnLimpiarSerializacion_790MY.TabIndex = 9;
            this.btnLimpiarSerializacion_790MY.Tag = "FC_btnLimpiar";
            this.btnLimpiarSerializacion_790MY.Text = "LIMPIAR";
            this.btnLimpiarSerializacion_790MY.Click += new System.EventHandler(this.btnLimpiarSerializacion_790MY_Click);
            //
            // pnlDetalle_790MY
            //
            this.pnlDetalle_790MY.AutoScroll = true;
            this.pnlDetalle_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(240)))), ((int)(((byte)(233)))));
            this.pnlDetalle_790MY.Controls.Add(this.lblDetalleTitulo_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.lblDetalleAyuda_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.lblDni_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.txtDni_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.lblApellidos_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.txtApellidos_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.lblNombres_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.txtNombres_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.lblEmail_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.txtEmail_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.txtEmailAdicional_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.btnEmailsAdicionales_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.flpEmailsAdicionales_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.lblCelular_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.txtCelular_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.txtCelularAdicional_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.btnTelefonosAdicionales_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.flpTelefonosAdicionales_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.lblDireccion_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.txtDireccion_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.btnAplicar_790MY);
            this.pnlDetalle_790MY.Controls.Add(this.btnCancelar_790MY);
            this.pnlDetalle_790MY.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlDetalle_790MY.Location = new System.Drawing.Point(673, 92);
            this.pnlDetalle_790MY.Name = "pnlDetalle_790MY";
            this.pnlDetalle_790MY.Size = new System.Drawing.Size(400, 612);
            this.pnlDetalle_790MY.TabIndex = 2;
            this.pnlDetalle_790MY.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlDetalle_790MY_Paint);
            //
            // lblDetalleTitulo_790MY
            //
            this.lblDetalleTitulo_790MY.AutoEllipsis = true;
            this.lblDetalleTitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblDetalleTitulo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblDetalleTitulo_790MY.Location = new System.Drawing.Point(16, 12);
            this.lblDetalleTitulo_790MY.Name = "lblDetalleTitulo_790MY";
            this.lblDetalleTitulo_790MY.Size = new System.Drawing.Size(368, 28);
            this.lblDetalleTitulo_790MY.TabIndex = 0;
            this.lblDetalleTitulo_790MY.Text = "Ficha del cliente";
            //
            // lblDetalleAyuda_790MY
            //
            this.lblDetalleAyuda_790MY.AutoEllipsis = true;
            this.lblDetalleAyuda_790MY.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblDetalleAyuda_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblDetalleAyuda_790MY.Location = new System.Drawing.Point(16, 40);
            this.lblDetalleAyuda_790MY.Name = "lblDetalleAyuda_790MY";
            this.lblDetalleAyuda_790MY.Size = new System.Drawing.Size(368, 30);
            this.lblDetalleAyuda_790MY.TabIndex = 1;
            //
            // lblDni_790MY
            //
            this.lblDni_790MY.AutoSize = true;
            this.lblDni_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDni_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblDni_790MY.Location = new System.Drawing.Point(16, 72);
            this.lblDni_790MY.Name = "lblDni_790MY";
            this.lblDni_790MY.TabIndex = 2;
            this.lblDni_790MY.Tag = "FC_lblDni";
            this.lblDni_790MY.Text = "DNI:";
            //
            // txtDni_790MY
            //
            this.txtDni_790MY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDni_790MY.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtDni_790MY.Location = new System.Drawing.Point(16, 92);
            this.txtDni_790MY.Name = "txtDni_790MY";
            this.txtDni_790MY.Size = new System.Drawing.Size(176, 25);
            this.txtDni_790MY.TabIndex = 3;
            this.txtDni_790MY.TextChanged += new System.EventHandler(this.CampoFicha_TextChanged);
            this.txtDni_790MY.Leave += new System.EventHandler(this.CampoFicha_Leave);
            //
            // lblApellidos_790MY
            //
            this.lblApellidos_790MY.AutoSize = true;
            this.lblApellidos_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblApellidos_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblApellidos_790MY.Location = new System.Drawing.Point(16, 136);
            this.lblApellidos_790MY.Name = "lblApellidos_790MY";
            this.lblApellidos_790MY.TabIndex = 5;
            this.lblApellidos_790MY.Tag = "FC_lblApellidos";
            this.lblApellidos_790MY.Text = "Apellidos:";
            //
            // txtApellidos_790MY
            //
            this.txtApellidos_790MY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtApellidos_790MY.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtApellidos_790MY.Location = new System.Drawing.Point(16, 156);
            this.txtApellidos_790MY.Name = "txtApellidos_790MY";
            this.txtApellidos_790MY.Size = new System.Drawing.Size(176, 25);
            this.txtApellidos_790MY.TabIndex = 6;
            this.txtApellidos_790MY.TextChanged += new System.EventHandler(this.CampoFicha_TextChanged);
            this.txtApellidos_790MY.Leave += new System.EventHandler(this.CampoFicha_Leave);
            //
            // lblNombres_790MY
            //
            this.lblNombres_790MY.AutoSize = true;
            this.lblNombres_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNombres_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblNombres_790MY.Location = new System.Drawing.Point(200, 136);
            this.lblNombres_790MY.Name = "lblNombres_790MY";
            this.lblNombres_790MY.TabIndex = 8;
            this.lblNombres_790MY.Tag = "FC_lblNombres";
            this.lblNombres_790MY.Text = "Nombres:";
            //
            // txtNombres_790MY
            //
            this.txtNombres_790MY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtNombres_790MY.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtNombres_790MY.Location = new System.Drawing.Point(200, 156);
            this.txtNombres_790MY.Name = "txtNombres_790MY";
            this.txtNombres_790MY.Size = new System.Drawing.Size(184, 25);
            this.txtNombres_790MY.TabIndex = 9;
            this.txtNombres_790MY.TextChanged += new System.EventHandler(this.CampoFicha_TextChanged);
            this.txtNombres_790MY.Leave += new System.EventHandler(this.CampoFicha_Leave);
            //
            // lblEmail_790MY
            //
            this.lblEmail_790MY.AutoSize = true;
            this.lblEmail_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblEmail_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblEmail_790MY.Location = new System.Drawing.Point(16, 200);
            this.lblEmail_790MY.Name = "lblEmail_790MY";
            this.lblEmail_790MY.TabIndex = 11;
            this.lblEmail_790MY.Tag = "FC_lblEmail";
            this.lblEmail_790MY.Text = "Email:";
            //
            // txtEmail_790MY
            //
            this.txtEmail_790MY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmail_790MY.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtEmail_790MY.Location = new System.Drawing.Point(16, 220);
            this.txtEmail_790MY.Name = "txtEmail_790MY";
            this.txtEmail_790MY.Size = new System.Drawing.Size(368, 25);
            this.txtEmail_790MY.TabIndex = 12;
            this.txtEmail_790MY.TextChanged += new System.EventHandler(this.CampoFicha_TextChanged);
            this.txtEmail_790MY.Leave += new System.EventHandler(this.CampoFicha_Leave);
            //
            // txtEmailAdicional_790MY
            //
            this.txtEmailAdicional_790MY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmailAdicional_790MY.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtEmailAdicional_790MY.Location = new System.Drawing.Point(16, 264);
            this.txtEmailAdicional_790MY.Name = "txtEmailAdicional_790MY";
            this.txtEmailAdicional_790MY.Size = new System.Drawing.Size(328, 23);
            this.txtEmailAdicional_790MY.TabIndex = 14;
            this.txtEmailAdicional_790MY.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAdicional_790MY_KeyDown);
            //
            // btnEmailsAdicionales_790MY
            //
            this.btnEmailsAdicionales_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnEmailsAdicionales_790MY.Location = new System.Drawing.Point(350, 263);
            this.btnEmailsAdicionales_790MY.Name = "btnEmailsAdicionales_790MY";
            this.btnEmailsAdicionales_790MY.Size = new System.Drawing.Size(34, 25);
            this.btnEmailsAdicionales_790MY.TabIndex = 15;
            this.btnEmailsAdicionales_790MY.Text = "+";
            this.btnEmailsAdicionales_790MY.Click += new System.EventHandler(this.btnEmailsAdicionales_790MY_Click);
            //
            // flpEmailsAdicionales_790MY
            //
            this.flpEmailsAdicionales_790MY.AutoScroll = true;
            this.flpEmailsAdicionales_790MY.Location = new System.Drawing.Point(16, 292);
            this.flpEmailsAdicionales_790MY.Name = "flpEmailsAdicionales_790MY";
            this.flpEmailsAdicionales_790MY.Size = new System.Drawing.Size(368, 50);
            this.flpEmailsAdicionales_790MY.TabIndex = 16;
            //
            // lblCelular_790MY
            //
            this.lblCelular_790MY.AutoSize = true;
            this.lblCelular_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCelular_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblCelular_790MY.Location = new System.Drawing.Point(16, 344);
            this.lblCelular_790MY.Name = "lblCelular_790MY";
            this.lblCelular_790MY.TabIndex = 17;
            this.lblCelular_790MY.Tag = "FC_lblCelular";
            this.lblCelular_790MY.Text = "Celular:";
            //
            // txtCelular_790MY
            //
            this.txtCelular_790MY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCelular_790MY.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtCelular_790MY.Location = new System.Drawing.Point(16, 364);
            this.txtCelular_790MY.Name = "txtCelular_790MY";
            this.txtCelular_790MY.Size = new System.Drawing.Size(368, 25);
            this.txtCelular_790MY.TabIndex = 18;
            this.txtCelular_790MY.TextChanged += new System.EventHandler(this.CampoFicha_TextChanged);
            this.txtCelular_790MY.Leave += new System.EventHandler(this.CampoFicha_Leave);
            //
            // txtCelularAdicional_790MY
            //
            this.txtCelularAdicional_790MY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCelularAdicional_790MY.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtCelularAdicional_790MY.Location = new System.Drawing.Point(16, 408);
            this.txtCelularAdicional_790MY.Name = "txtCelularAdicional_790MY";
            this.txtCelularAdicional_790MY.Size = new System.Drawing.Size(328, 23);
            this.txtCelularAdicional_790MY.TabIndex = 20;
            this.txtCelularAdicional_790MY.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtAdicional_790MY_KeyDown);
            //
            // btnTelefonosAdicionales_790MY
            //
            this.btnTelefonosAdicionales_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnTelefonosAdicionales_790MY.Location = new System.Drawing.Point(350, 407);
            this.btnTelefonosAdicionales_790MY.Name = "btnTelefonosAdicionales_790MY";
            this.btnTelefonosAdicionales_790MY.Size = new System.Drawing.Size(34, 25);
            this.btnTelefonosAdicionales_790MY.TabIndex = 21;
            this.btnTelefonosAdicionales_790MY.Text = "+";
            this.btnTelefonosAdicionales_790MY.Click += new System.EventHandler(this.btnTelefonosAdicionales_790MY_Click);
            //
            // flpTelefonosAdicionales_790MY
            //
            this.flpTelefonosAdicionales_790MY.AutoScroll = true;
            this.flpTelefonosAdicionales_790MY.Location = new System.Drawing.Point(16, 436);
            this.flpTelefonosAdicionales_790MY.Name = "flpTelefonosAdicionales_790MY";
            this.flpTelefonosAdicionales_790MY.Size = new System.Drawing.Size(368, 50);
            this.flpTelefonosAdicionales_790MY.TabIndex = 22;
            //
            // lblDireccion_790MY
            //
            this.lblDireccion_790MY.AutoSize = true;
            this.lblDireccion_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDireccion_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblDireccion_790MY.Location = new System.Drawing.Point(16, 488);
            this.lblDireccion_790MY.Name = "lblDireccion_790MY";
            this.lblDireccion_790MY.TabIndex = 23;
            this.lblDireccion_790MY.Tag = "FC_lblDireccion";
            this.lblDireccion_790MY.Text = "Dirección:";
            //
            // txtDireccion_790MY
            //
            this.txtDireccion_790MY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDireccion_790MY.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtDireccion_790MY.Location = new System.Drawing.Point(16, 508);
            this.txtDireccion_790MY.Name = "txtDireccion_790MY";
            this.txtDireccion_790MY.Size = new System.Drawing.Size(368, 25);
            this.txtDireccion_790MY.TabIndex = 24;
            this.txtDireccion_790MY.TextChanged += new System.EventHandler(this.CampoFicha_TextChanged);
            this.txtDireccion_790MY.Leave += new System.EventHandler(this.CampoFicha_Leave);
            //
            // btnAplicar_790MY
            //
            this.btnAplicar_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAplicar_790MY.Location = new System.Drawing.Point(16, 554);
            this.btnAplicar_790MY.Name = "btnAplicar_790MY";
            this.btnAplicar_790MY.Size = new System.Drawing.Size(180, 36);
            this.btnAplicar_790MY.TabIndex = 26;
            this.btnAplicar_790MY.Tag = "btnAplicar";
            this.btnAplicar_790MY.Text = "Aplicar";
            this.btnAplicar_790MY.Click += new System.EventHandler(this.btnAplicar_790MY_Click);
            //
            // btnCancelar_790MY
            //
            this.btnCancelar_790MY.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancelar_790MY.Location = new System.Drawing.Point(204, 554);
            this.btnCancelar_790MY.Name = "btnCancelar_790MY";
            this.btnCancelar_790MY.Size = new System.Drawing.Size(180, 36);
            this.btnCancelar_790MY.TabIndex = 27;
            this.btnCancelar_790MY.Tag = "btn_Cancelar";
            this.btnCancelar_790MY.Text = "Cancelar";
            this.btnCancelar_790MY.Click += new System.EventHandler(this.btnCancelar_790MY_Click);
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
            // FormClientes_790MY
            //
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(1073, 704);
            this.Controls.Add(this.pnlLista_790MY);
            this.Controls.Add(this.pnlDetalle_790MY);
            this.Controls.Add(this.bnrAvisos_790MY);
            this.Controls.Add(this.pnlEncabezado_790MY);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "FormClientes_790MY";
            this.Text = "Clientes";
            this.Load += new System.EventHandler(this.FormClientes_790MY_Load);
            this.pnlEncabezado_790MY.ResumeLayout(false);
            this.pnlEncabezado_790MY.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes_790MY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errValidacion_790MY)).EndInit();
            this.pnlAvisoCambios_790MY.ResumeLayout(false);
            this.grpSerializacion_790MY.ResumeLayout(false);
            this.grpSerializacion_790MY.PerformLayout();
            this.pnlSerializacion_790MY.ResumeLayout(false);
            this.pnlLista_790MY.ResumeLayout(false);
            this.pnlLista_790MY.PerformLayout();
            this.pnlDetalle_790MY.ResumeLayout(false);
            this.pnlDetalle_790MY.PerformLayout();
            this.ResumeLayout(false);
            this.AplicarEstiloVisual();
            this.AplicarIconoCorporativo();

        }

        #endregion

        /// <summary>
        /// Tokens de TemaFogon_790MY sobre grilla y botones (solo apariencia).
        /// Invocado desde InitializeComponent().
        /// </summary>
        private void AplicarEstiloVisual()
        {
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarGrilla(this.dgvClientes_790MY);
            this.dgvClientes_790MY.AutoGenerateColumns = false;

            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarBoton(this.btnAnadir_790MY, true);
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarBoton(this.btnModificar_790MY, false);
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarBoton(this.btnEliminar_790MY, false);
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarBoton(this.btnSalir_790MY, false);
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarBoton(this.btnAplicar_790MY, true);
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarBoton(this.btnCancelar_790MY, false);
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarBoton(this.btnGuardarCambios_790MY, true);
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarBoton(this.btnDescartarCambios_790MY, false);
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarBoton(this.btnEmailsAdicionales_790MY, false);
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarBoton(this.btnTelefonosAdicionales_790MY, false);
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarBoton(this.btnSerializar_790MY, false);
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarBoton(this.btnDeserializar_790MY, false);
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarBoton(this.btnLimpiarSerializacion_790MY, false);
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarBoton(this.btnExaminarSerializar_790MY, false);
            GUI_08YS.Estilo.TemaFogon_790MY.EstilizarBoton(this.btnExaminarDeserializar_790MY, false);

            this.btnPlegarSerializacion_790MY.FlatAppearance.BorderColor = GUI_08YS.Estilo.TemaFogon_790MY.Linea;
            this.btnPlegarSerializacion_790MY.FlatAppearance.MouseOverBackColor = GUI_08YS.Estilo.TemaFogon_790MY.Panel;
            this.btnPlegarSerializacion_790MY.BackColor = GUI_08YS.Estilo.TemaFogon_790MY.Tarjeta;
            this.btnPlegarSerializacion_790MY.Cursor = System.Windows.Forms.Cursors.Hand;

            // Ícono de error dentro del borde derecho de cada campo: la ficha no tiene
            // margen libre a la derecha y DNI/Apellidos quedan pegados a otro campo.
            foreach (System.Windows.Forms.Control campo in new System.Windows.Forms.Control[]
            {
                this.txtDni_790MY, this.txtApellidos_790MY, this.txtNombres_790MY,
                this.txtEmail_790MY, this.txtCelular_790MY, this.txtDireccion_790MY,
                this.txtEmailAdicional_790MY, this.txtCelularAdicional_790MY,
                this.txtRutaSerializar_790MY, this.txtRutaDeserializar_790MY
            })
            {
                this.errValidacion_790MY.SetIconAlignment(campo, System.Windows.Forms.ErrorIconAlignment.MiddleRight);
                this.errValidacion_790MY.SetIconPadding(campo, -20);
            }
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

        private System.Windows.Forms.Timer tmrBusqueda_790MY;
        private System.Windows.Forms.Panel pnlEncabezado_790MY;
        private System.Windows.Forms.Label lblTitulo_790MY;
        private System.Windows.Forms.TextBox txtBuscar_790MY;
        private System.Windows.Forms.Label lblResumen_790MY;
        private System.Windows.Forms.Panel pnlLista_790MY;
        private System.Windows.Forms.Label lblSinResultados_790MY;
        private System.Windows.Forms.Panel pnlAvisoCambios_790MY;
        private System.Windows.Forms.Label lblAvisoCambios_790MY;
        private System.Windows.Forms.Button btnGuardarCambios_790MY;
        private System.Windows.Forms.Button btnDescartarCambios_790MY;
        private System.Windows.Forms.Panel pnlSerializacion_790MY;
        private System.Windows.Forms.Button btnPlegarSerializacion_790MY;
        private System.Windows.Forms.Panel pnlDetalle_790MY;
        private System.Windows.Forms.Label lblDetalleTitulo_790MY;
        private System.Windows.Forms.Label lblDetalleAyuda_790MY;
        private System.Windows.Forms.TextBox txtEmailAdicional_790MY;
        private System.Windows.Forms.FlowLayoutPanel flpEmailsAdicionales_790MY;
        private System.Windows.Forms.TextBox txtCelularAdicional_790MY;
        private System.Windows.Forms.FlowLayoutPanel flpTelefonosAdicionales_790MY;
        private System.Windows.Forms.DataGridView dgvClientes_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDni_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colApellidos_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombres_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmail_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCelular_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDireccion_790MY;
        private System.Windows.Forms.Button btnAnadir_790MY;
        private System.Windows.Forms.Button btnModificar_790MY;
        private System.Windows.Forms.Button btnEliminar_790MY;
        private System.Windows.Forms.Button btnAplicar_790MY;
        private System.Windows.Forms.Button btnCancelar_790MY;
        private System.Windows.Forms.Button btnSalir_790MY;
        private System.Windows.Forms.Label lblDni_790MY;
        private System.Windows.Forms.TextBox txtDni_790MY;
        private System.Windows.Forms.Label lblApellidos_790MY;
        private System.Windows.Forms.TextBox txtApellidos_790MY;
        private System.Windows.Forms.Label lblNombres_790MY;
        private System.Windows.Forms.TextBox txtNombres_790MY;
        private System.Windows.Forms.Label lblEmail_790MY;
        private System.Windows.Forms.TextBox txtEmail_790MY;
        private System.Windows.Forms.Button btnEmailsAdicionales_790MY;
        private System.Windows.Forms.Label lblCelular_790MY;
        private System.Windows.Forms.TextBox txtCelular_790MY;
        private System.Windows.Forms.Button btnTelefonosAdicionales_790MY;
        private System.Windows.Forms.Label lblDireccion_790MY;
        private System.Windows.Forms.TextBox txtDireccion_790MY;
        private System.Windows.Forms.GroupBox grpSerializacion_790MY;
        private System.Windows.Forms.Label lblRutaSerializar_790MY;
        private System.Windows.Forms.TextBox txtRutaSerializar_790MY;
        private System.Windows.Forms.Button btnExaminarSerializar_790MY;
        private System.Windows.Forms.Label lblRutaDeserializar_790MY;
        private System.Windows.Forms.TextBox txtRutaDeserializar_790MY;
        private System.Windows.Forms.Button btnExaminarDeserializar_790MY;
        private System.Windows.Forms.Button btnSerializar_790MY;
        private System.Windows.Forms.Button btnDeserializar_790MY;
        private System.Windows.Forms.Button btnLimpiarSerializacion_790MY;
        private System.Windows.Forms.ListBox lstDeserializados_790MY;
        private GUI_08YS.Estilo.Banner_UC_790MY bnrAvisos_790MY;
        private System.Windows.Forms.ErrorProvider errValidacion_790MY;
    }
}
