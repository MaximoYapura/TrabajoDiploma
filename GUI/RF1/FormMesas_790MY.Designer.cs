namespace GUI_08YS.RF1
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
            this.dgvMesas_790MY = new System.Windows.Forms.DataGridView();
            this.colNumero_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCapacidad_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstado_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAnadir_790MY = new System.Windows.Forms.Button();
            this.btnModificar_790MY = new System.Windows.Forms.Button();
            this.btnEliminar_790MY = new System.Windows.Forms.Button();
            this.btnAplicar_790MY = new System.Windows.Forms.Button();
            this.btnCancelar_790MY = new System.Windows.Forms.Button();
            this.btnSalir_790MY = new System.Windows.Forms.Button();
            this.lblNumero_790MY = new System.Windows.Forms.Label();
            this.txtNumero_790MY = new System.Windows.Forms.TextBox();
            this.lblCapacidad_790MY = new System.Windows.Forms.Label();
            this.txtCapacidad_790MY = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMesas_790MY)).BeginInit();
            this.SuspendLayout();
            //
            // dgvMesas_790MY
            //
            this.dgvMesas_790MY.AllowUserToAddRows = false;
            this.dgvMesas_790MY.AllowUserToDeleteRows = false;
            this.dgvMesas_790MY.ReadOnly = true;
            this.dgvMesas_790MY.RowHeadersVisible = false;
            this.dgvMesas_790MY.AutoGenerateColumns = false;
            this.dgvMesas_790MY.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMesas_790MY.MultiSelect = false;
            this.dgvMesas_790MY.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.dgvMesas_790MY.EnableHeadersVisualStyles = false;
            this.dgvMesas_790MY.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.dgvMesas_790MY.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(195)))), ((int)(((byte)(140)))));
            this.dgvMesas_790MY.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.dgvMesas_790MY.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.dgvMesas_790MY.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNumero_790MY,
            this.colCapacidad_790MY,
            this.colEstado_790MY});
            this.dgvMesas_790MY.Location = new System.Drawing.Point(20, 20);
            this.dgvMesas_790MY.Name = "dgvMesas_790MY";
            this.dgvMesas_790MY.Size = new System.Drawing.Size(450, 300);
            this.dgvMesas_790MY.SelectionChanged += new System.EventHandler(this.dgvMesas_790MY_SelectionChanged);
            //
            // colNumero_790MY
            //
            this.colNumero_790MY.DataPropertyName = "NroMesa";
            this.colNumero_790MY.HeaderText = "Número";
            this.colNumero_790MY.Name = "colNumero_790MY";
            this.colNumero_790MY.ReadOnly = true;
            //
            // colCapacidad_790MY
            //
            this.colCapacidad_790MY.DataPropertyName = "Capacidad";
            this.colCapacidad_790MY.HeaderText = "Capacidad";
            this.colCapacidad_790MY.Name = "colCapacidad_790MY";
            this.colCapacidad_790MY.ReadOnly = true;
            //
            // colEstado_790MY
            //
            this.colEstado_790MY.DataPropertyName = "Estado";
            this.colEstado_790MY.HeaderText = "Estado";
            this.colEstado_790MY.Name = "colEstado_790MY";
            this.colEstado_790MY.ReadOnly = true;
            //
            // Botones de acción (lado derecho de la grilla)
            //
            this.btnAnadir_790MY.Location = new System.Drawing.Point(485, 20);
            this.btnAnadir_790MY.Name = "btnAnadir_790MY";
            this.btnAnadir_790MY.Size = new System.Drawing.Size(110, 28);
            this.btnAnadir_790MY.Text = "Añadir";
            this.btnAnadir_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnadir_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnAnadir_790MY.ForeColor = System.Drawing.Color.White;
            this.btnAnadir_790MY.UseVisualStyleBackColor = false;
            this.btnAnadir_790MY.Click += new System.EventHandler(this.btnAnadir_790MY_Click);
            //
            this.btnModificar_790MY.Location = new System.Drawing.Point(485, 55);
            this.btnModificar_790MY.Name = "btnModificar_790MY";
            this.btnModificar_790MY.Size = new System.Drawing.Size(110, 28);
            this.btnModificar_790MY.Text = "Modificar";
            this.btnModificar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModificar_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnModificar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnModificar_790MY.UseVisualStyleBackColor = false;
            this.btnModificar_790MY.Click += new System.EventHandler(this.btnModificar_790MY_Click);
            //
            this.btnEliminar_790MY.Location = new System.Drawing.Point(485, 90);
            this.btnEliminar_790MY.Name = "btnEliminar_790MY";
            this.btnEliminar_790MY.Size = new System.Drawing.Size(110, 28);
            this.btnEliminar_790MY.Text = "Eliminar";
            this.btnEliminar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnEliminar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnEliminar_790MY.UseVisualStyleBackColor = false;
            this.btnEliminar_790MY.Click += new System.EventHandler(this.btnEliminar_790MY_Click);
            //
            this.btnAplicar_790MY.Location = new System.Drawing.Point(485, 140);
            this.btnAplicar_790MY.Name = "btnAplicar_790MY";
            this.btnAplicar_790MY.Size = new System.Drawing.Size(110, 28);
            this.btnAplicar_790MY.Text = "Aplicar";
            this.btnAplicar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAplicar_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnAplicar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnAplicar_790MY.UseVisualStyleBackColor = false;
            this.btnAplicar_790MY.Click += new System.EventHandler(this.btnAplicar_790MY_Click);
            //
            this.btnCancelar_790MY.Location = new System.Drawing.Point(485, 175);
            this.btnCancelar_790MY.Name = "btnCancelar_790MY";
            this.btnCancelar_790MY.Size = new System.Drawing.Size(110, 28);
            this.btnCancelar_790MY.Text = "Cancelar";
            this.btnCancelar_790MY.UseVisualStyleBackColor = true;
            this.btnCancelar_790MY.Click += new System.EventHandler(this.btnCancelar_790MY_Click);
            //
            this.btnSalir_790MY.Location = new System.Drawing.Point(485, 212);
            this.btnSalir_790MY.Name = "btnSalir_790MY";
            this.btnSalir_790MY.Size = new System.Drawing.Size(110, 28);
            this.btnSalir_790MY.Text = "Salir";
            this.btnSalir_790MY.UseVisualStyleBackColor = true;
            this.btnSalir_790MY.Click += new System.EventHandler(this.btnSalir_790MY_Click);
            //
            // Formulario inferior de edición
            //
            this.lblNumero_790MY.AutoSize = true;
            this.lblNumero_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblNumero_790MY.Location = new System.Drawing.Point(20, 340);
            this.lblNumero_790MY.Name = "lblNumero_790MY";
            this.lblNumero_790MY.Text = "Número:";
            //
            this.txtNumero_790MY.Location = new System.Drawing.Point(110, 337);
            this.txtNumero_790MY.Name = "txtNumero_790MY";
            this.txtNumero_790MY.Size = new System.Drawing.Size(120, 20);
            //
            this.lblCapacidad_790MY.AutoSize = true;
            this.lblCapacidad_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblCapacidad_790MY.Location = new System.Drawing.Point(20, 372);
            this.lblCapacidad_790MY.Name = "lblCapacidad_790MY";
            this.lblCapacidad_790MY.Text = "Capacidad:";
            //
            this.txtCapacidad_790MY.Location = new System.Drawing.Point(110, 369);
            this.txtCapacidad_790MY.Name = "txtCapacidad_790MY";
            this.txtCapacidad_790MY.Size = new System.Drawing.Size(120, 20);
            //
            // FormMesas_790MY
            //
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(630, 420);
            this.Controls.Add(this.dgvMesas_790MY);
            this.Controls.Add(this.btnAnadir_790MY);
            this.Controls.Add(this.btnModificar_790MY);
            this.Controls.Add(this.btnEliminar_790MY);
            this.Controls.Add(this.btnAplicar_790MY);
            this.Controls.Add(this.btnCancelar_790MY);
            this.Controls.Add(this.btnSalir_790MY);
            this.Controls.Add(this.lblNumero_790MY);
            this.Controls.Add(this.txtNumero_790MY);
            this.Controls.Add(this.lblCapacidad_790MY);
            this.Controls.Add(this.txtCapacidad_790MY);
            this.Name = "FormMesas_790MY";
            this.Text = "Mesas";
            this.Load += new System.EventHandler(this.FormMesas_790MY_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMesas_790MY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMesas_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumero_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCapacidad_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstado_790MY;
        private System.Windows.Forms.Button btnAnadir_790MY;
        private System.Windows.Forms.Button btnModificar_790MY;
        private System.Windows.Forms.Button btnEliminar_790MY;
        private System.Windows.Forms.Button btnAplicar_790MY;
        private System.Windows.Forms.Button btnCancelar_790MY;
        private System.Windows.Forms.Button btnSalir_790MY;
        private System.Windows.Forms.Label lblNumero_790MY;
        private System.Windows.Forms.TextBox txtNumero_790MY;
        private System.Windows.Forms.Label lblCapacidad_790MY;
        private System.Windows.Forms.TextBox txtCapacidad_790MY;
    }
}
