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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvMesas_790MY = new System.Windows.Forms.DataGridView();
            this.btnAnadir_790MY = new System.Windows.Forms.Button();
            this.btnModificar_790MY = new System.Windows.Forms.Button();
            this.btnEliminar_790MY = new System.Windows.Forms.Button();
            this.btnAplicar_790MY = new System.Windows.Forms.Button();
            this.btnCancelar_790MY = new System.Windows.Forms.Button();
            this.btnSalir_790MY = new System.Windows.Forms.Button();
            this.lblNumero_790MY = new System.Windows.Forms.Label();
            this.txtNumero_790MY = new System.Windows.Forms.TextBox();
            this.lblCapacidad_790MY = new System.Windows.Forms.Label();
            this.nudCapacidad_790MY = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMesas_790MY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCapacidad_790MY)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMesas_790MY
            // 
            this.dgvMesas_790MY.AllowUserToAddRows = false;
            this.dgvMesas_790MY.AllowUserToDeleteRows = false;
            this.dgvMesas_790MY.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMesas_790MY.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(195)))), ((int)(((byte)(140)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMesas_790MY.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMesas_790MY.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvMesas_790MY.EnableHeadersVisualStyles = false;
            this.dgvMesas_790MY.Location = new System.Drawing.Point(20, 20);
            this.dgvMesas_790MY.MultiSelect = false;
            this.dgvMesas_790MY.Name = "dgvMesas_790MY";
            this.dgvMesas_790MY.ReadOnly = true;
            this.dgvMesas_790MY.RowHeadersVisible = false;
            this.dgvMesas_790MY.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMesas_790MY.Size = new System.Drawing.Size(674, 662);
            this.dgvMesas_790MY.TabIndex = 0;
            this.dgvMesas_790MY.SelectionChanged += new System.EventHandler(this.dgvMesas_790MY_SelectionChanged);
            // 
            // btnAnadir_790MY
            // 
            this.btnAnadir_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnAnadir_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnadir_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAnadir_790MY.ForeColor = System.Drawing.Color.White;
            this.btnAnadir_790MY.Location = new System.Drawing.Point(785, 294);
            this.btnAnadir_790MY.Name = "btnAnadir_790MY";
            this.btnAnadir_790MY.Size = new System.Drawing.Size(165, 35);
            this.btnAnadir_790MY.TabIndex = 1;
            this.btnAnadir_790MY.Tag = "btnAnadir";
            this.btnAnadir_790MY.Text = "Añadir";
            this.btnAnadir_790MY.UseVisualStyleBackColor = false;
            this.btnAnadir_790MY.Click += new System.EventHandler(this.btnAnadir_790MY_Click);
            // 
            // btnModificar_790MY
            // 
            this.btnModificar_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnModificar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModificar_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModificar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnModificar_790MY.Location = new System.Drawing.Point(785, 351);
            this.btnModificar_790MY.Name = "btnModificar_790MY";
            this.btnModificar_790MY.Size = new System.Drawing.Size(165, 35);
            this.btnModificar_790MY.TabIndex = 2;
            this.btnModificar_790MY.Tag = "btnModificar";
            this.btnModificar_790MY.Text = "Modificar";
            this.btnModificar_790MY.UseVisualStyleBackColor = false;
            this.btnModificar_790MY.Click += new System.EventHandler(this.btnModificar_790MY_Click);
            // 
            // btnEliminar_790MY
            // 
            this.btnEliminar_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnEliminar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEliminar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnEliminar_790MY.Location = new System.Drawing.Point(785, 402);
            this.btnEliminar_790MY.Name = "btnEliminar_790MY";
            this.btnEliminar_790MY.Size = new System.Drawing.Size(165, 35);
            this.btnEliminar_790MY.TabIndex = 3;
            this.btnEliminar_790MY.Tag = "btnEliminar";
            this.btnEliminar_790MY.Text = "Eliminar";
            this.btnEliminar_790MY.UseVisualStyleBackColor = false;
            this.btnEliminar_790MY.Click += new System.EventHandler(this.btnEliminar_790MY_Click);
            // 
            // btnAplicar_790MY
            // 
            this.btnAplicar_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnAplicar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAplicar_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAplicar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnAplicar_790MY.Location = new System.Drawing.Point(785, 519);
            this.btnAplicar_790MY.Name = "btnAplicar_790MY";
            this.btnAplicar_790MY.Size = new System.Drawing.Size(165, 35);
            this.btnAplicar_790MY.TabIndex = 4;
            this.btnAplicar_790MY.Tag = "btnAplicar";
            this.btnAplicar_790MY.Text = "Aplicar";
            this.btnAplicar_790MY.UseVisualStyleBackColor = false;
            this.btnAplicar_790MY.Click += new System.EventHandler(this.btnAplicar_790MY_Click);
            // 
            // btnCancelar_790MY
            // 
            this.btnCancelar_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar_790MY.Location = new System.Drawing.Point(785, 570);
            this.btnCancelar_790MY.Name = "btnCancelar_790MY";
            this.btnCancelar_790MY.Size = new System.Drawing.Size(165, 35);
            this.btnCancelar_790MY.TabIndex = 5;
            this.btnCancelar_790MY.Tag = "btn_Cancelar";
            this.btnCancelar_790MY.Text = "Cancelar";
            this.btnCancelar_790MY.UseVisualStyleBackColor = true;
            this.btnCancelar_790MY.Click += new System.EventHandler(this.btnCancelar_790MY_Click);
            // 
            // btnSalir_790MY
            // 
            this.btnSalir_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalir_790MY.Location = new System.Drawing.Point(785, 611);
            this.btnSalir_790MY.Name = "btnSalir_790MY";
            this.btnSalir_790MY.Size = new System.Drawing.Size(165, 35);
            this.btnSalir_790MY.TabIndex = 6;
            this.btnSalir_790MY.Tag = "btnSalir";
            this.btnSalir_790MY.Text = "Salir";
            this.btnSalir_790MY.UseVisualStyleBackColor = true;
            this.btnSalir_790MY.Click += new System.EventHandler(this.btnSalir_790MY_Click);
            // 
            // lblNumero_790MY
            // 
            this.lblNumero_790MY.AutoSize = true;
            this.lblNumero_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumero_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblNumero_790MY.Location = new System.Drawing.Point(841, 112);
            this.lblNumero_790MY.Name = "lblNumero_790MY";
            this.lblNumero_790MY.Size = new System.Drawing.Size(58, 16);
            this.lblNumero_790MY.TabIndex = 7;
            this.lblNumero_790MY.Tag = "FM_lblNumero";
            this.lblNumero_790MY.Text = "Número:";
            // 
            // txtNumero_790MY
            // 
            this.txtNumero_790MY.Location = new System.Drawing.Point(785, 142);
            this.txtNumero_790MY.Name = "txtNumero_790MY";
            this.txtNumero_790MY.Size = new System.Drawing.Size(165, 20);
            this.txtNumero_790MY.TabIndex = 8;
            // 
            // lblCapacidad_790MY
            // 
            this.lblCapacidad_790MY.AutoSize = true;
            this.lblCapacidad_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCapacidad_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblCapacidad_790MY.Location = new System.Drawing.Point(831, 198);
            this.lblCapacidad_790MY.Name = "lblCapacidad_790MY";
            this.lblCapacidad_790MY.Size = new System.Drawing.Size(77, 16);
            this.lblCapacidad_790MY.TabIndex = 9;
            this.lblCapacidad_790MY.Tag = "FM_lblCapacidad";
            this.lblCapacidad_790MY.Text = "Capacidad:";
            // 
            // nudCapacidad_790MY
            // 
            this.nudCapacidad_790MY.Location = new System.Drawing.Point(785, 228);
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
            this.nudCapacidad_790MY.Size = new System.Drawing.Size(165, 20);
            this.nudCapacidad_790MY.TabIndex = 10;
            this.nudCapacidad_790MY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // FormMesas_790MY
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(1073, 704);
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
            this.Controls.Add(this.nudCapacidad_790MY);
            this.Name = "FormMesas_790MY";
            this.Text = "Mesas";
            this.Load += new System.EventHandler(this.FormMesas_790MY_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMesas_790MY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCapacidad_790MY)).EndInit();
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
        private System.Windows.Forms.NumericUpDown nudCapacidad_790MY;
    }
}
