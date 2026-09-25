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
            this.dgvClientes_790MY = new System.Windows.Forms.DataGridView();
            this.colDni_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colApellidos_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombres_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmail_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCelular_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDireccion_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAnadir_790MY = new System.Windows.Forms.Button();
            this.btnModificar_790MY = new System.Windows.Forms.Button();
            this.btnEliminar_790MY = new System.Windows.Forms.Button();
            this.btnAplicar_790MY = new System.Windows.Forms.Button();
            this.btnCancelar_790MY = new System.Windows.Forms.Button();
            this.btnSalir_790MY = new System.Windows.Forms.Button();
            this.lblDni_790MY = new System.Windows.Forms.Label();
            this.txtDni_790MY = new System.Windows.Forms.TextBox();
            this.lblApellidos_790MY = new System.Windows.Forms.Label();
            this.txtApellidos_790MY = new System.Windows.Forms.TextBox();
            this.lblNombres_790MY = new System.Windows.Forms.Label();
            this.txtNombres_790MY = new System.Windows.Forms.TextBox();
            this.lblEmail_790MY = new System.Windows.Forms.Label();
            this.txtEmail_790MY = new System.Windows.Forms.TextBox();
            this.btnEmailsAdicionales_790MY = new System.Windows.Forms.Button();
            this.lblCelular_790MY = new System.Windows.Forms.Label();
            this.txtCelular_790MY = new System.Windows.Forms.TextBox();
            this.btnTelefonosAdicionales_790MY = new System.Windows.Forms.Button();
            this.lblDireccion_790MY = new System.Windows.Forms.Label();
            this.txtDireccion_790MY = new System.Windows.Forms.TextBox();
            this.grpSerializacion_790MY = new System.Windows.Forms.GroupBox();
            this.lblRutaSerializar_790MY = new System.Windows.Forms.Label();
            this.txtRutaSerializar_790MY = new System.Windows.Forms.TextBox();
            this.btnExaminarSerializar_790MY = new System.Windows.Forms.Button();
            this.lblRutaDeserializar_790MY = new System.Windows.Forms.Label();
            this.txtRutaDeserializar_790MY = new System.Windows.Forms.TextBox();
            this.btnExaminarDeserializar_790MY = new System.Windows.Forms.Button();
            this.btnSerializar_790MY = new System.Windows.Forms.Button();
            this.btnDeserializar_790MY = new System.Windows.Forms.Button();
            this.btnLimpiarSerializacion_790MY = new System.Windows.Forms.Button();
            this.lstDeserializados_790MY = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes_790MY)).BeginInit();
            this.grpSerializacion_790MY.SuspendLayout();
            this.SuspendLayout();
            //
            // dgvClientes_790MY
            //
            this.dgvClientes_790MY.AllowUserToAddRows = false;
            this.dgvClientes_790MY.AllowUserToDeleteRows = false;
            this.dgvClientes_790MY.ReadOnly = true;
            this.dgvClientes_790MY.RowHeadersVisible = false;
            this.dgvClientes_790MY.AutoGenerateColumns = false;
            this.dgvClientes_790MY.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvClientes_790MY.MultiSelect = false;
            this.dgvClientes_790MY.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.dgvClientes_790MY.EnableHeadersVisualStyles = false;
            this.dgvClientes_790MY.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.dgvClientes_790MY.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(195)))), ((int)(((byte)(140)))));
            this.dgvClientes_790MY.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.dgvClientes_790MY.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.dgvClientes_790MY.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDni_790MY,
            this.colApellidos_790MY,
            this.colNombres_790MY,
            this.colEmail_790MY,
            this.colCelular_790MY,
            this.colDireccion_790MY});
            this.dgvClientes_790MY.Location = new System.Drawing.Point(20, 20);
            this.dgvClientes_790MY.Name = "dgvClientes_790MY";
            this.dgvClientes_790MY.Size = new System.Drawing.Size(600, 220);
            this.dgvClientes_790MY.SelectionChanged += new System.EventHandler(this.dgvClientes_790MY_SelectionChanged);
            //
            // colDni_790MY
            //
            this.colDni_790MY.DataPropertyName = "DNI";
            this.colDni_790MY.HeaderText = "DNI";
            this.colDni_790MY.Name = "colDni_790MY";
            this.colDni_790MY.ReadOnly = true;
            this.colDni_790MY.Width = 80;
            //
            // colApellidos_790MY
            //
            this.colApellidos_790MY.DataPropertyName = "Apellido";
            this.colApellidos_790MY.HeaderText = "Apellidos";
            this.colApellidos_790MY.Name = "colApellidos_790MY";
            this.colApellidos_790MY.ReadOnly = true;
            //
            // colNombres_790MY
            //
            this.colNombres_790MY.DataPropertyName = "Nombre";
            this.colNombres_790MY.HeaderText = "Nombres";
            this.colNombres_790MY.Name = "colNombres_790MY";
            this.colNombres_790MY.ReadOnly = true;
            //
            // colEmail_790MY
            //
            this.colEmail_790MY.DataPropertyName = "Email";
            this.colEmail_790MY.HeaderText = "Email";
            this.colEmail_790MY.Name = "colEmail_790MY";
            this.colEmail_790MY.ReadOnly = true;
            //
            // colCelular_790MY
            //
            this.colCelular_790MY.DataPropertyName = "Telefono";
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
            //
            // Botones de acción (lado derecho de la grilla)
            //
            this.btnAnadir_790MY.Location = new System.Drawing.Point(635, 20);
            this.btnAnadir_790MY.Name = "btnAnadir_790MY";
            this.btnAnadir_790MY.Size = new System.Drawing.Size(110, 28);
            this.btnAnadir_790MY.Text = "Añadir";
            this.btnAnadir_790MY.Tag = "btnAnadir";
            this.btnAnadir_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnadir_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnAnadir_790MY.ForeColor = System.Drawing.Color.White;
            this.btnAnadir_790MY.UseVisualStyleBackColor = false;
            this.btnAnadir_790MY.Click += new System.EventHandler(this.btnAnadir_790MY_Click);
            //
            this.btnModificar_790MY.Location = new System.Drawing.Point(635, 55);
            this.btnModificar_790MY.Name = "btnModificar_790MY";
            this.btnModificar_790MY.Size = new System.Drawing.Size(110, 28);
            this.btnModificar_790MY.Text = "Modificar";
            this.btnModificar_790MY.Tag = "btnModificar";
            this.btnModificar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModificar_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnModificar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnModificar_790MY.UseVisualStyleBackColor = false;
            this.btnModificar_790MY.Click += new System.EventHandler(this.btnModificar_790MY_Click);
            //
            this.btnEliminar_790MY.Location = new System.Drawing.Point(635, 90);
            this.btnEliminar_790MY.Name = "btnEliminar_790MY";
            this.btnEliminar_790MY.Size = new System.Drawing.Size(110, 28);
            this.btnEliminar_790MY.Text = "Eliminar";
            this.btnEliminar_790MY.Tag = "btnEliminar";
            this.btnEliminar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnEliminar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnEliminar_790MY.UseVisualStyleBackColor = false;
            this.btnEliminar_790MY.Click += new System.EventHandler(this.btnEliminar_790MY_Click);
            //
            this.btnAplicar_790MY.Location = new System.Drawing.Point(635, 140);
            this.btnAplicar_790MY.Name = "btnAplicar_790MY";
            this.btnAplicar_790MY.Size = new System.Drawing.Size(110, 28);
            this.btnAplicar_790MY.Text = "Aplicar";
            this.btnAplicar_790MY.Tag = "btnAplicar";
            this.btnAplicar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAplicar_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnAplicar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnAplicar_790MY.UseVisualStyleBackColor = false;
            this.btnAplicar_790MY.Click += new System.EventHandler(this.btnAplicar_790MY_Click);
            //
            this.btnCancelar_790MY.Location = new System.Drawing.Point(635, 175);
            this.btnCancelar_790MY.Name = "btnCancelar_790MY";
            this.btnCancelar_790MY.Size = new System.Drawing.Size(110, 28);
            this.btnCancelar_790MY.Text = "Cancelar";
            this.btnCancelar_790MY.Tag = "btn_Cancelar";
            this.btnCancelar_790MY.UseVisualStyleBackColor = true;
            this.btnCancelar_790MY.Click += new System.EventHandler(this.btnCancelar_790MY_Click);
            //
            this.btnSalir_790MY.Location = new System.Drawing.Point(635, 212);
            this.btnSalir_790MY.Name = "btnSalir_790MY";
            this.btnSalir_790MY.Size = new System.Drawing.Size(110, 28);
            this.btnSalir_790MY.Text = "Salir";
            this.btnSalir_790MY.Tag = "btnSalir";
            this.btnSalir_790MY.UseVisualStyleBackColor = true;
            this.btnSalir_790MY.Click += new System.EventHandler(this.btnSalir_790MY_Click);
            //
            // Formulario inferior de edición
            //
            this.lblDni_790MY.AutoSize = true;
            this.lblDni_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblDni_790MY.Location = new System.Drawing.Point(20, 260);
            this.lblDni_790MY.Name = "lblDni_790MY";
            this.lblDni_790MY.Text = "DNI:";
            this.lblDni_790MY.Tag = "FC_lblDni";
            //
            this.txtDni_790MY.Location = new System.Drawing.Point(110, 257);
            this.txtDni_790MY.Name = "txtDni_790MY";
            this.txtDni_790MY.Size = new System.Drawing.Size(150, 20);
            //
            this.lblApellidos_790MY.AutoSize = true;
            this.lblApellidos_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblApellidos_790MY.Location = new System.Drawing.Point(20, 292);
            this.lblApellidos_790MY.Name = "lblApellidos_790MY";
            this.lblApellidos_790MY.Text = "Apellidos:";
            this.lblApellidos_790MY.Tag = "FC_lblApellidos";
            //
            this.txtApellidos_790MY.Location = new System.Drawing.Point(110, 289);
            this.txtApellidos_790MY.Name = "txtApellidos_790MY";
            this.txtApellidos_790MY.Size = new System.Drawing.Size(280, 20);
            //
            this.lblNombres_790MY.AutoSize = true;
            this.lblNombres_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblNombres_790MY.Location = new System.Drawing.Point(20, 324);
            this.lblNombres_790MY.Name = "lblNombres_790MY";
            this.lblNombres_790MY.Text = "Nombres:";
            this.lblNombres_790MY.Tag = "FC_lblNombres";
            //
            this.txtNombres_790MY.Location = new System.Drawing.Point(110, 321);
            this.txtNombres_790MY.Name = "txtNombres_790MY";
            this.txtNombres_790MY.Size = new System.Drawing.Size(280, 20);
            //
            this.lblEmail_790MY.AutoSize = true;
            this.lblEmail_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblEmail_790MY.Location = new System.Drawing.Point(20, 356);
            this.lblEmail_790MY.Name = "lblEmail_790MY";
            this.lblEmail_790MY.Text = "Email:";
            this.lblEmail_790MY.Tag = "FC_lblEmail";
            //
            this.txtEmail_790MY.Location = new System.Drawing.Point(110, 353);
            this.txtEmail_790MY.Name = "txtEmail_790MY";
            this.txtEmail_790MY.Size = new System.Drawing.Size(240, 20);
            //
            this.btnEmailsAdicionales_790MY.Location = new System.Drawing.Point(355, 351);
            this.btnEmailsAdicionales_790MY.Name = "btnEmailsAdicionales_790MY";
            this.btnEmailsAdicionales_790MY.Size = new System.Drawing.Size(35, 24);
            this.btnEmailsAdicionales_790MY.Text = "...";
            this.btnEmailsAdicionales_790MY.UseVisualStyleBackColor = true;
            this.btnEmailsAdicionales_790MY.Click += new System.EventHandler(this.btnEmailsAdicionales_790MY_Click);
            //
            this.lblCelular_790MY.AutoSize = true;
            this.lblCelular_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblCelular_790MY.Location = new System.Drawing.Point(20, 388);
            this.lblCelular_790MY.Name = "lblCelular_790MY";
            this.lblCelular_790MY.Text = "Celular:";
            this.lblCelular_790MY.Tag = "FC_lblCelular";
            //
            this.txtCelular_790MY.Location = new System.Drawing.Point(110, 385);
            this.txtCelular_790MY.Name = "txtCelular_790MY";
            this.txtCelular_790MY.Size = new System.Drawing.Size(240, 20);
            //
            this.btnTelefonosAdicionales_790MY.Location = new System.Drawing.Point(355, 383);
            this.btnTelefonosAdicionales_790MY.Name = "btnTelefonosAdicionales_790MY";
            this.btnTelefonosAdicionales_790MY.Size = new System.Drawing.Size(35, 24);
            this.btnTelefonosAdicionales_790MY.Text = "...";
            this.btnTelefonosAdicionales_790MY.UseVisualStyleBackColor = true;
            this.btnTelefonosAdicionales_790MY.Click += new System.EventHandler(this.btnTelefonosAdicionales_790MY_Click);
            //
            this.lblDireccion_790MY.AutoSize = true;
            this.lblDireccion_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblDireccion_790MY.Location = new System.Drawing.Point(20, 420);
            this.lblDireccion_790MY.Name = "lblDireccion_790MY";
            this.lblDireccion_790MY.Text = "Dirección:";
            this.lblDireccion_790MY.Tag = "FC_lblDireccion";
            //
            this.txtDireccion_790MY.Location = new System.Drawing.Point(110, 417);
            this.txtDireccion_790MY.Name = "txtDireccion_790MY";
            this.txtDireccion_790MY.Size = new System.Drawing.Size(280, 20);
            //
            // grpSerializacion_790MY
            //
            this.grpSerializacion_790MY.Controls.Add(this.lblRutaSerializar_790MY);
            this.grpSerializacion_790MY.Controls.Add(this.txtRutaSerializar_790MY);
            this.grpSerializacion_790MY.Controls.Add(this.btnExaminarSerializar_790MY);
            this.grpSerializacion_790MY.Controls.Add(this.lblRutaDeserializar_790MY);
            this.grpSerializacion_790MY.Controls.Add(this.txtRutaDeserializar_790MY);
            this.grpSerializacion_790MY.Controls.Add(this.btnExaminarDeserializar_790MY);
            this.grpSerializacion_790MY.Controls.Add(this.btnSerializar_790MY);
            this.grpSerializacion_790MY.Controls.Add(this.btnDeserializar_790MY);
            this.grpSerializacion_790MY.Controls.Add(this.btnLimpiarSerializacion_790MY);
            this.grpSerializacion_790MY.Controls.Add(this.lstDeserializados_790MY);
            this.grpSerializacion_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.grpSerializacion_790MY.Location = new System.Drawing.Point(20, 450);
            this.grpSerializacion_790MY.Name = "grpSerializacion_790MY";
            this.grpSerializacion_790MY.Size = new System.Drawing.Size(715, 235);
            this.grpSerializacion_790MY.TabStop = false;
            this.grpSerializacion_790MY.Text = "Serialización";
            this.grpSerializacion_790MY.Tag = "FC_grpSerializacion";
            //
            // lblRutaSerializar_790MY
            //
            this.lblRutaSerializar_790MY.AutoSize = true;
            this.lblRutaSerializar_790MY.Location = new System.Drawing.Point(15, 28);
            this.lblRutaSerializar_790MY.Name = "lblRutaSerializar_790MY";
            this.lblRutaSerializar_790MY.Text = "Archivo a serializar:";
            this.lblRutaSerializar_790MY.Tag = "FC_lblRutaSerializar";
            //
            this.txtRutaSerializar_790MY.Location = new System.Drawing.Point(160, 25);
            this.txtRutaSerializar_790MY.Name = "txtRutaSerializar_790MY";
            this.txtRutaSerializar_790MY.ReadOnly = true;
            this.txtRutaSerializar_790MY.Size = new System.Drawing.Size(430, 20);
            //
            this.btnExaminarSerializar_790MY.Location = new System.Drawing.Point(600, 23);
            this.btnExaminarSerializar_790MY.Name = "btnExaminarSerializar_790MY";
            this.btnExaminarSerializar_790MY.Size = new System.Drawing.Size(40, 24);
            this.btnExaminarSerializar_790MY.Text = "📁";
            this.btnExaminarSerializar_790MY.UseVisualStyleBackColor = true;
            this.btnExaminarSerializar_790MY.Click += new System.EventHandler(this.btnExaminarSerializar_790MY_Click);
            //
            // lblRutaDeserializar_790MY
            //
            this.lblRutaDeserializar_790MY.AutoSize = true;
            this.lblRutaDeserializar_790MY.Location = new System.Drawing.Point(15, 58);
            this.lblRutaDeserializar_790MY.Name = "lblRutaDeserializar_790MY";
            this.lblRutaDeserializar_790MY.Text = "Archivo a deserializar:";
            this.lblRutaDeserializar_790MY.Tag = "FC_lblRutaDeserializar";
            //
            this.txtRutaDeserializar_790MY.Location = new System.Drawing.Point(160, 55);
            this.txtRutaDeserializar_790MY.Name = "txtRutaDeserializar_790MY";
            this.txtRutaDeserializar_790MY.ReadOnly = true;
            this.txtRutaDeserializar_790MY.Size = new System.Drawing.Size(430, 20);
            //
            this.btnExaminarDeserializar_790MY.Location = new System.Drawing.Point(600, 53);
            this.btnExaminarDeserializar_790MY.Name = "btnExaminarDeserializar_790MY";
            this.btnExaminarDeserializar_790MY.Size = new System.Drawing.Size(40, 24);
            this.btnExaminarDeserializar_790MY.Text = "📁";
            this.btnExaminarDeserializar_790MY.UseVisualStyleBackColor = true;
            this.btnExaminarDeserializar_790MY.Click += new System.EventHandler(this.btnExaminarDeserializar_790MY_Click);
            //
            // btnSerializar_790MY
            //
            this.btnSerializar_790MY.Location = new System.Drawing.Point(15, 95);
            this.btnSerializar_790MY.Name = "btnSerializar_790MY";
            this.btnSerializar_790MY.Size = new System.Drawing.Size(140, 32);
            this.btnSerializar_790MY.Text = "SERIALIZAR";
            this.btnSerializar_790MY.Tag = "FC_btnSerializar";
            this.btnSerializar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSerializar_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnSerializar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnSerializar_790MY.UseVisualStyleBackColor = false;
            this.btnSerializar_790MY.Click += new System.EventHandler(this.btnSerializar_790MY_Click);
            //
            // btnDeserializar_790MY
            //
            this.btnDeserializar_790MY.Location = new System.Drawing.Point(165, 95);
            this.btnDeserializar_790MY.Name = "btnDeserializar_790MY";
            this.btnDeserializar_790MY.Size = new System.Drawing.Size(140, 32);
            this.btnDeserializar_790MY.Text = "DES-SERIALIZAR";
            this.btnDeserializar_790MY.Tag = "FC_btnDeserializar";
            this.btnDeserializar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeserializar_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnDeserializar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnDeserializar_790MY.UseVisualStyleBackColor = false;
            this.btnDeserializar_790MY.Click += new System.EventHandler(this.btnDeserializar_790MY_Click);
            //
            // btnLimpiarSerializacion_790MY
            //
            this.btnLimpiarSerializacion_790MY.Location = new System.Drawing.Point(315, 95);
            this.btnLimpiarSerializacion_790MY.Name = "btnLimpiarSerializacion_790MY";
            this.btnLimpiarSerializacion_790MY.Size = new System.Drawing.Size(100, 32);
            this.btnLimpiarSerializacion_790MY.Text = "LIMPIAR";
            this.btnLimpiarSerializacion_790MY.Tag = "FC_btnLimpiar";
            this.btnLimpiarSerializacion_790MY.UseVisualStyleBackColor = true;
            this.btnLimpiarSerializacion_790MY.Click += new System.EventHandler(this.btnLimpiarSerializacion_790MY_Click);
            //
            // lstDeserializados_790MY
            //
            this.lstDeserializados_790MY.FormattingEnabled = true;
            this.lstDeserializados_790MY.Location = new System.Drawing.Point(15, 140);
            this.lstDeserializados_790MY.Name = "lstDeserializados_790MY";
            this.lstDeserializados_790MY.Size = new System.Drawing.Size(685, 82);
            //
            // FormClientes_790MY
            //
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(780, 710);
            this.Controls.Add(this.grpSerializacion_790MY);
            this.Controls.Add(this.dgvClientes_790MY);
            this.Controls.Add(this.btnAnadir_790MY);
            this.Controls.Add(this.btnModificar_790MY);
            this.Controls.Add(this.btnEliminar_790MY);
            this.Controls.Add(this.btnAplicar_790MY);
            this.Controls.Add(this.btnCancelar_790MY);
            this.Controls.Add(this.btnSalir_790MY);
            this.Controls.Add(this.lblDni_790MY);
            this.Controls.Add(this.txtDni_790MY);
            this.Controls.Add(this.lblApellidos_790MY);
            this.Controls.Add(this.txtApellidos_790MY);
            this.Controls.Add(this.lblNombres_790MY);
            this.Controls.Add(this.txtNombres_790MY);
            this.Controls.Add(this.lblEmail_790MY);
            this.Controls.Add(this.txtEmail_790MY);
            this.Controls.Add(this.btnEmailsAdicionales_790MY);
            this.Controls.Add(this.lblCelular_790MY);
            this.Controls.Add(this.txtCelular_790MY);
            this.Controls.Add(this.btnTelefonosAdicionales_790MY);
            this.Controls.Add(this.lblDireccion_790MY);
            this.Controls.Add(this.txtDireccion_790MY);
            this.Name = "FormClientes_790MY";
            this.Text = "Clientes";
            this.Load += new System.EventHandler(this.FormClientes_790MY_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes_790MY)).EndInit();
            this.grpSerializacion_790MY.ResumeLayout(false);
            this.grpSerializacion_790MY.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

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
    }
}
