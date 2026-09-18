namespace GUI_08YS.RF1
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes_790MY)).BeginInit();
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
            this.btnAnadir_790MY.UseVisualStyleBackColor = true;
            this.btnAnadir_790MY.Click += new System.EventHandler(this.btnAnadir_790MY_Click);
            //
            this.btnModificar_790MY.Location = new System.Drawing.Point(635, 55);
            this.btnModificar_790MY.Name = "btnModificar_790MY";
            this.btnModificar_790MY.Size = new System.Drawing.Size(110, 28);
            this.btnModificar_790MY.Text = "Modificar";
            this.btnModificar_790MY.UseVisualStyleBackColor = true;
            this.btnModificar_790MY.Click += new System.EventHandler(this.btnModificar_790MY_Click);
            //
            this.btnEliminar_790MY.Location = new System.Drawing.Point(635, 90);
            this.btnEliminar_790MY.Name = "btnEliminar_790MY";
            this.btnEliminar_790MY.Size = new System.Drawing.Size(110, 28);
            this.btnEliminar_790MY.Text = "Eliminar";
            this.btnEliminar_790MY.UseVisualStyleBackColor = true;
            this.btnEliminar_790MY.Click += new System.EventHandler(this.btnEliminar_790MY_Click);
            //
            this.btnAplicar_790MY.Location = new System.Drawing.Point(635, 140);
            this.btnAplicar_790MY.Name = "btnAplicar_790MY";
            this.btnAplicar_790MY.Size = new System.Drawing.Size(110, 28);
            this.btnAplicar_790MY.Text = "Aplicar";
            this.btnAplicar_790MY.UseVisualStyleBackColor = true;
            this.btnAplicar_790MY.Click += new System.EventHandler(this.btnAplicar_790MY_Click);
            //
            this.btnCancelar_790MY.Location = new System.Drawing.Point(635, 175);
            this.btnCancelar_790MY.Name = "btnCancelar_790MY";
            this.btnCancelar_790MY.Size = new System.Drawing.Size(110, 28);
            this.btnCancelar_790MY.Text = "Cancelar";
            this.btnCancelar_790MY.UseVisualStyleBackColor = true;
            this.btnCancelar_790MY.Click += new System.EventHandler(this.btnCancelar_790MY_Click);
            //
            this.btnSalir_790MY.Location = new System.Drawing.Point(635, 212);
            this.btnSalir_790MY.Name = "btnSalir_790MY";
            this.btnSalir_790MY.Size = new System.Drawing.Size(110, 28);
            this.btnSalir_790MY.Text = "Salir";
            this.btnSalir_790MY.UseVisualStyleBackColor = true;
            this.btnSalir_790MY.Click += new System.EventHandler(this.btnSalir_790MY_Click);
            //
            // Formulario inferior de edición
            //
            this.lblDni_790MY.AutoSize = true;
            this.lblDni_790MY.Location = new System.Drawing.Point(20, 260);
            this.lblDni_790MY.Name = "lblDni_790MY";
            this.lblDni_790MY.Text = "DNI:";
            //
            this.txtDni_790MY.Location = new System.Drawing.Point(110, 257);
            this.txtDni_790MY.Name = "txtDni_790MY";
            this.txtDni_790MY.Size = new System.Drawing.Size(150, 20);
            //
            this.lblApellidos_790MY.AutoSize = true;
            this.lblApellidos_790MY.Location = new System.Drawing.Point(20, 292);
            this.lblApellidos_790MY.Name = "lblApellidos_790MY";
            this.lblApellidos_790MY.Text = "Apellidos:";
            //
            this.txtApellidos_790MY.Location = new System.Drawing.Point(110, 289);
            this.txtApellidos_790MY.Name = "txtApellidos_790MY";
            this.txtApellidos_790MY.Size = new System.Drawing.Size(280, 20);
            //
            this.lblNombres_790MY.AutoSize = true;
            this.lblNombres_790MY.Location = new System.Drawing.Point(20, 324);
            this.lblNombres_790MY.Name = "lblNombres_790MY";
            this.lblNombres_790MY.Text = "Nombres:";
            //
            this.txtNombres_790MY.Location = new System.Drawing.Point(110, 321);
            this.txtNombres_790MY.Name = "txtNombres_790MY";
            this.txtNombres_790MY.Size = new System.Drawing.Size(280, 20);
            //
            this.lblEmail_790MY.AutoSize = true;
            this.lblEmail_790MY.Location = new System.Drawing.Point(20, 356);
            this.lblEmail_790MY.Name = "lblEmail_790MY";
            this.lblEmail_790MY.Text = "Email:";
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
            this.lblCelular_790MY.Location = new System.Drawing.Point(20, 388);
            this.lblCelular_790MY.Name = "lblCelular_790MY";
            this.lblCelular_790MY.Text = "Celular:";
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
            this.lblDireccion_790MY.Location = new System.Drawing.Point(20, 420);
            this.lblDireccion_790MY.Name = "lblDireccion_790MY";
            this.lblDireccion_790MY.Text = "Dirección:";
            //
            this.txtDireccion_790MY.Location = new System.Drawing.Point(110, 417);
            this.txtDireccion_790MY.Name = "txtDireccion_790MY";
            this.txtDireccion_790MY.Size = new System.Drawing.Size(280, 20);
            //
            // FormClientes_790MY
            //
            this.ClientSize = new System.Drawing.Size(780, 460);
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
    }
}
