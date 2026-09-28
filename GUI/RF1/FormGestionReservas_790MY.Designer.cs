namespace GUI_08YS.RF1
{
    partial class FormGestionReservas_790MY
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblTituloConsultaReservas = new System.Windows.Forms.Label();
            this.lblDesde_790MY = new System.Windows.Forms.Label();
            this.dtpDesde_790MY = new System.Windows.Forms.DateTimePicker();
            this.lblHasta_790MY = new System.Windows.Forms.Label();
            this.dtpHasta_790MY = new System.Windows.Forms.DateTimePicker();
            this.lblDni_790MY = new System.Windows.Forms.Label();
            this.txtDni_790MY = new System.Windows.Forms.TextBox();
            this.lblEstado_790MY = new System.Windows.Forms.Label();
            this.cmbEstado_790MY = new System.Windows.Forms.ComboBox();
            this.btnBuscar_790MY = new System.Windows.Forms.Button();
            this.dgvReservas_790MY = new System.Windows.Forms.DataGridView();
            this.btnLimpiar_790MY = new System.Windows.Forms.Button();
            this.btnCancelarReserva_790MY = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservas_790MY)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTituloConsultaReservas
            // 
            this.lblTituloConsultaReservas.AutoSize = true;
            this.lblTituloConsultaReservas.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTituloConsultaReservas.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblTituloConsultaReservas.Location = new System.Drawing.Point(20, 15);
            this.lblTituloConsultaReservas.Name = "lblTituloConsultaReservas";
            this.lblTituloConsultaReservas.Size = new System.Drawing.Size(235, 21);
            this.lblTituloConsultaReservas.TabIndex = 12;
            this.lblTituloConsultaReservas.Tag = "GR_titulo";
            this.lblTituloConsultaReservas.Text = "Consultar / Cancelar Reservas";
            // 
            // lblDesde_790MY
            // 
            this.lblDesde_790MY.AutoSize = true;
            this.lblDesde_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDesde_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblDesde_790MY.Location = new System.Drawing.Point(254, 56);
            this.lblDesde_790MY.Name = "lblDesde_790MY";
            this.lblDesde_790MY.Size = new System.Drawing.Size(60, 20);
            this.lblDesde_790MY.TabIndex = 11;
            this.lblDesde_790MY.Tag = "GR_lblDesde";
            this.lblDesde_790MY.Text = "Desde:";
            // 
            // dtpDesde_790MY
            // 
            this.dtpDesde_790MY.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde_790MY.Location = new System.Drawing.Point(330, 56);
            this.dtpDesde_790MY.Name = "dtpDesde_790MY";
            this.dtpDesde_790MY.ShowCheckBox = true;
            this.dtpDesde_790MY.Size = new System.Drawing.Size(235, 20);
            this.dtpDesde_790MY.TabIndex = 10;
            // 
            // lblHasta_790MY
            // 
            this.lblHasta_790MY.AutoSize = true;
            this.lblHasta_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHasta_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblHasta_790MY.Location = new System.Drawing.Point(259, 120);
            this.lblHasta_790MY.Name = "lblHasta_790MY";
            this.lblHasta_790MY.Size = new System.Drawing.Size(56, 20);
            this.lblHasta_790MY.TabIndex = 9;
            this.lblHasta_790MY.Tag = "GR_lblHasta";
            this.lblHasta_790MY.Text = "Hasta:";
            // 
            // dtpHasta_790MY
            // 
            this.dtpHasta_790MY.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta_790MY.Location = new System.Drawing.Point(330, 120);
            this.dtpHasta_790MY.Name = "dtpHasta_790MY";
            this.dtpHasta_790MY.ShowCheckBox = true;
            this.dtpHasta_790MY.Size = new System.Drawing.Size(235, 20);
            this.dtpHasta_790MY.TabIndex = 8;
            // 
            // lblDni_790MY
            // 
            this.lblDni_790MY.AutoSize = true;
            this.lblDni_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDni_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblDni_790MY.Location = new System.Drawing.Point(594, 55);
            this.lblDni_790MY.Name = "lblDni_790MY";
            this.lblDni_790MY.Size = new System.Drawing.Size(91, 20);
            this.lblDni_790MY.TabIndex = 7;
            this.lblDni_790MY.Tag = "GR_lblDni";
            this.lblDni_790MY.Text = "DNI cliente:";
            // 
            // txtDni_790MY
            // 
            this.txtDni_790MY.Location = new System.Drawing.Point(718, 57);
            this.txtDni_790MY.Name = "txtDni_790MY";
            this.txtDni_790MY.Size = new System.Drawing.Size(343, 20);
            this.txtDni_790MY.TabIndex = 6;
            // 
            // lblEstado_790MY
            // 
            this.lblEstado_790MY.AutoSize = true;
            this.lblEstado_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEstado_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblEstado_790MY.Location = new System.Drawing.Point(98, 57);
            this.lblEstado_790MY.Name = "lblEstado_790MY";
            this.lblEstado_790MY.Size = new System.Drawing.Size(64, 20);
            this.lblEstado_790MY.TabIndex = 5;
            this.lblEstado_790MY.Tag = "GR_lblEstado";
            this.lblEstado_790MY.Text = "Estado:";
            // 
            // cmbEstado_790MY
            // 
            this.cmbEstado_790MY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstado_790MY.Location = new System.Drawing.Point(24, 95);
            this.cmbEstado_790MY.Name = "cmbEstado_790MY";
            this.cmbEstado_790MY.Size = new System.Drawing.Size(213, 21);
            this.cmbEstado_790MY.TabIndex = 4;
            // 
            // btnBuscar_790MY
            // 
            this.btnBuscar_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnBuscar_790MY.FlatAppearance.BorderSize = 0;
            this.btnBuscar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuscar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnBuscar_790MY.Location = new System.Drawing.Point(598, 95);
            this.btnBuscar_790MY.Name = "btnBuscar_790MY";
            this.btnBuscar_790MY.Size = new System.Drawing.Size(215, 45);
            this.btnBuscar_790MY.TabIndex = 3;
            this.btnBuscar_790MY.Tag = "btnBuscar";
            this.btnBuscar_790MY.Text = "Buscar";
            this.btnBuscar_790MY.UseVisualStyleBackColor = false;
            this.btnBuscar_790MY.Click += new System.EventHandler(this.btnBuscar_790MY_Click);
            // 
            // dgvReservas_790MY
            // 
            this.dgvReservas_790MY.AllowUserToAddRows = false;
            this.dgvReservas_790MY.AllowUserToDeleteRows = false;
            this.dgvReservas_790MY.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvReservas_790MY.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvReservas_790MY.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(195)))), ((int)(((byte)(140)))));
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReservas_790MY.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReservas_790MY.DefaultCellStyle = dataGridViewCellStyle14;
            this.dgvReservas_790MY.EnableHeadersVisualStyles = false;
            this.dgvReservas_790MY.Location = new System.Drawing.Point(20, 160);
            this.dgvReservas_790MY.MultiSelect = false;
            this.dgvReservas_790MY.Name = "dgvReservas_790MY";
            this.dgvReservas_790MY.ReadOnly = true;
            this.dgvReservas_790MY.RowHeadersVisible = false;
            this.dgvReservas_790MY.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvReservas_790MY.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReservas_790MY.Size = new System.Drawing.Size(1041, 481);
            this.dgvReservas_790MY.TabIndex = 1;
            this.dgvReservas_790MY.SelectionChanged += new System.EventHandler(this.dgvReservas_790MY_SelectionChanged);
            // 
            // btnLimpiar_790MY
            // 
            this.btnLimpiar_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnLimpiar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiar_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpiar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnLimpiar_790MY.Location = new System.Drawing.Point(846, 95);
            this.btnLimpiar_790MY.Name = "btnLimpiar_790MY";
            this.btnLimpiar_790MY.Size = new System.Drawing.Size(215, 45);
            this.btnLimpiar_790MY.TabIndex = 2;
            this.btnLimpiar_790MY.Tag = "btnLimpiar";
            this.btnLimpiar_790MY.Text = "Limpiar";
            this.btnLimpiar_790MY.UseVisualStyleBackColor = false;
            this.btnLimpiar_790MY.Click += new System.EventHandler(this.btnLimpiar_790MY_Click);
            // 
            // btnCancelarReserva_790MY
            // 
            this.btnCancelarReserva_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnCancelarReserva_790MY.Enabled = false;
            this.btnCancelarReserva_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelarReserva_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelarReserva_790MY.ForeColor = System.Drawing.Color.White;
            this.btnCancelarReserva_790MY.Location = new System.Drawing.Point(846, 647);
            this.btnCancelarReserva_790MY.Name = "btnCancelarReserva_790MY";
            this.btnCancelarReserva_790MY.Size = new System.Drawing.Size(215, 45);
            this.btnCancelarReserva_790MY.TabIndex = 0;
            this.btnCancelarReserva_790MY.Tag = "btnCancelarReserva";
            this.btnCancelarReserva_790MY.Text = "Cancelar Reserva";
            this.btnCancelarReserva_790MY.UseVisualStyleBackColor = false;
            this.btnCancelarReserva_790MY.Click += new System.EventHandler(this.btnCancelarReserva_790MY_Click);
            // 
            // FormGestionReservas_790MY
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(1073, 704);
            this.Controls.Add(this.btnCancelarReserva_790MY);
            this.Controls.Add(this.dgvReservas_790MY);
            this.Controls.Add(this.btnLimpiar_790MY);
            this.Controls.Add(this.btnBuscar_790MY);
            this.Controls.Add(this.cmbEstado_790MY);
            this.Controls.Add(this.lblEstado_790MY);
            this.Controls.Add(this.txtDni_790MY);
            this.Controls.Add(this.lblDni_790MY);
            this.Controls.Add(this.dtpHasta_790MY);
            this.Controls.Add(this.lblHasta_790MY);
            this.Controls.Add(this.dtpDesde_790MY);
            this.Controls.Add(this.lblDesde_790MY);
            this.Controls.Add(this.lblTituloConsultaReservas);
            this.Name = "FormGestionReservas_790MY";
            this.Text = "Consultar / Cancelar Reservas";
            this.Load += new System.EventHandler(this.FormGestionReservas_790MY_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservas_790MY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTituloConsultaReservas;
        private System.Windows.Forms.Label lblDesde_790MY;
        private System.Windows.Forms.DateTimePicker dtpDesde_790MY;
        private System.Windows.Forms.Label lblHasta_790MY;
        private System.Windows.Forms.DateTimePicker dtpHasta_790MY;
        private System.Windows.Forms.Label lblDni_790MY;
        private System.Windows.Forms.TextBox txtDni_790MY;
        private System.Windows.Forms.Label lblEstado_790MY;
        private System.Windows.Forms.ComboBox cmbEstado_790MY;
        private System.Windows.Forms.Button btnBuscar_790MY;
        private System.Windows.Forms.DataGridView dgvReservas_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReservaID_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDniCliente_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCliente_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMesa_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFecha_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHora_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComensales_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEstado_790MY;
        private System.Windows.Forms.Button btnLimpiar_790MY;
        private System.Windows.Forms.Button btnCancelarReserva_790MY;
    }
}
