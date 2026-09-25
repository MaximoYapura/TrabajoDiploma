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
            this.lblTitulo_790MY = new System.Windows.Forms.Label();
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
            this.colReservaID_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDniCliente_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCliente_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMesa_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFecha_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHora_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colComensales_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEstado_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCancelarReserva_790MY = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservas_790MY)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitulo_790MY
            //
            this.lblTitulo_790MY.AutoSize = true;
            this.lblTitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblTitulo_790MY.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo_790MY.Name = "lblTitulo_790MY";
            this.lblTitulo_790MY.Size = new System.Drawing.Size(260, 25);
            this.lblTitulo_790MY.Text = "Consultar / Cancelar Reservas";
            this.lblTitulo_790MY.Tag = "GR_titulo";
            //
            // lblDesde_790MY
            //
            this.lblDesde_790MY.AutoSize = true;
            this.lblDesde_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblDesde_790MY.Location = new System.Drawing.Point(20, 60);
            this.lblDesde_790MY.Name = "lblDesde_790MY";
            this.lblDesde_790MY.Text = "Desde:";
            this.lblDesde_790MY.Tag = "GR_lblDesde";
            //
            // dtpDesde_790MY
            //
            this.dtpDesde_790MY.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde_790MY.Location = new System.Drawing.Point(80, 57);
            this.dtpDesde_790MY.Name = "dtpDesde_790MY";
            this.dtpDesde_790MY.ShowCheckBox = true;
            this.dtpDesde_790MY.Size = new System.Drawing.Size(140, 20);
            //
            // lblHasta_790MY
            //
            this.lblHasta_790MY.AutoSize = true;
            this.lblHasta_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblHasta_790MY.Location = new System.Drawing.Point(235, 60);
            this.lblHasta_790MY.Name = "lblHasta_790MY";
            this.lblHasta_790MY.Text = "Hasta:";
            this.lblHasta_790MY.Tag = "GR_lblHasta";
            //
            // dtpHasta_790MY
            //
            this.dtpHasta_790MY.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta_790MY.Location = new System.Drawing.Point(290, 57);
            this.dtpHasta_790MY.Name = "dtpHasta_790MY";
            this.dtpHasta_790MY.ShowCheckBox = true;
            this.dtpHasta_790MY.Size = new System.Drawing.Size(140, 20);
            //
            // lblDni_790MY
            //
            this.lblDni_790MY.AutoSize = true;
            this.lblDni_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblDni_790MY.Location = new System.Drawing.Point(20, 95);
            this.lblDni_790MY.Name = "lblDni_790MY";
            this.lblDni_790MY.Text = "DNI cliente:";
            this.lblDni_790MY.Tag = "GR_lblDni";
            //
            // txtDni_790MY
            //
            this.txtDni_790MY.Location = new System.Drawing.Point(100, 92);
            this.txtDni_790MY.Name = "txtDni_790MY";
            this.txtDni_790MY.Size = new System.Drawing.Size(120, 20);
            //
            // lblEstado_790MY
            //
            this.lblEstado_790MY.AutoSize = true;
            this.lblEstado_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblEstado_790MY.Location = new System.Drawing.Point(240, 95);
            this.lblEstado_790MY.Name = "lblEstado_790MY";
            this.lblEstado_790MY.Text = "Estado:";
            this.lblEstado_790MY.Tag = "GR_lblEstado";
            //
            // cmbEstado_790MY
            //
            this.cmbEstado_790MY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstado_790MY.Location = new System.Drawing.Point(300, 91);
            this.cmbEstado_790MY.Name = "cmbEstado_790MY";
            this.cmbEstado_790MY.Size = new System.Drawing.Size(130, 21);
            //
            // btnBuscar_790MY
            //
            this.btnBuscar_790MY.Location = new System.Drawing.Point(450, 89);
            this.btnBuscar_790MY.Name = "btnBuscar_790MY";
            this.btnBuscar_790MY.Size = new System.Drawing.Size(90, 26);
            this.btnBuscar_790MY.Text = "Buscar";
            this.btnBuscar_790MY.Tag = "btnBuscar";
            this.btnBuscar_790MY.UseVisualStyleBackColor = true;
            this.btnBuscar_790MY.Click += new System.EventHandler(this.btnBuscar_790MY_Click);
            //
            // dgvReservas_790MY
            //
            this.dgvReservas_790MY.AllowUserToAddRows = false;
            this.dgvReservas_790MY.AllowUserToDeleteRows = false;
            this.dgvReservas_790MY.ReadOnly = true;
            this.dgvReservas_790MY.RowHeadersVisible = false;
            this.dgvReservas_790MY.AutoGenerateColumns = false;
            this.dgvReservas_790MY.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReservas_790MY.MultiSelect = false;
            this.dgvReservas_790MY.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.dgvReservas_790MY.EnableHeadersVisualStyles = false;
            this.dgvReservas_790MY.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.dgvReservas_790MY.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(195)))), ((int)(((byte)(140)))));
            this.dgvReservas_790MY.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.dgvReservas_790MY.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.dgvReservas_790MY.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colReservaID_790MY,
            this.colDniCliente_790MY,
            this.colCliente_790MY,
            this.colMesa_790MY,
            this.colFecha_790MY,
            this.colHora_790MY,
            this.colComensales_790MY,
            this.colEstado_790MY});
            this.dgvReservas_790MY.Location = new System.Drawing.Point(20, 130);
            this.dgvReservas_790MY.Name = "dgvReservas_790MY";
            this.dgvReservas_790MY.Size = new System.Drawing.Size(720, 280);
            this.dgvReservas_790MY.SelectionChanged += new System.EventHandler(this.dgvReservas_790MY_SelectionChanged);
            //
            // colReservaID_790MY
            //
            this.colReservaID_790MY.DataPropertyName = "ReservaID";
            this.colReservaID_790MY.HeaderText = "Nro.";
            this.colReservaID_790MY.Name = "colReservaID_790MY";
            this.colReservaID_790MY.ReadOnly = true;
            this.colReservaID_790MY.Width = 50;
            //
            // colDniCliente_790MY
            //
            this.colDniCliente_790MY.DataPropertyName = "ClienteDNI";
            this.colDniCliente_790MY.HeaderText = "DNI";
            this.colDniCliente_790MY.Name = "colDniCliente_790MY";
            this.colDniCliente_790MY.ReadOnly = true;
            this.colDniCliente_790MY.Width = 80;
            //
            // colCliente_790MY
            //
            this.colCliente_790MY.DataPropertyName = "ClienteNombreCompleto";
            this.colCliente_790MY.HeaderText = "Cliente";
            this.colCliente_790MY.Name = "colCliente_790MY";
            this.colCliente_790MY.ReadOnly = true;
            this.colCliente_790MY.Width = 160;
            //
            // colMesa_790MY
            //
            this.colMesa_790MY.DataPropertyName = "MesaNumero";
            this.colMesa_790MY.HeaderText = "Mesa";
            this.colMesa_790MY.Name = "colMesa_790MY";
            this.colMesa_790MY.ReadOnly = true;
            this.colMesa_790MY.Width = 60;
            //
            // colFecha_790MY
            //
            this.colFecha_790MY.DataPropertyName = "Fecha";
            this.colFecha_790MY.HeaderText = "Fecha";
            this.colFecha_790MY.Name = "colFecha_790MY";
            this.colFecha_790MY.ReadOnly = true;
            this.colFecha_790MY.DefaultCellStyle.Format = "dd/MM/yyyy";
            //
            // colHora_790MY
            //
            this.colHora_790MY.DataPropertyName = "Hora";
            this.colHora_790MY.HeaderText = "Hora";
            this.colHora_790MY.Name = "colHora_790MY";
            this.colHora_790MY.ReadOnly = true;
            this.colHora_790MY.DefaultCellStyle.Format = "hh\\:mm";
            //
            // colComensales_790MY
            //
            this.colComensales_790MY.DataPropertyName = "CantidadComensales";
            this.colComensales_790MY.HeaderText = "Comensales";
            this.colComensales_790MY.Name = "colComensales_790MY";
            this.colComensales_790MY.ReadOnly = true;
            //
            // colEstado_790MY
            //
            this.colEstado_790MY.DataPropertyName = "Estado";
            this.colEstado_790MY.HeaderText = "Estado";
            this.colEstado_790MY.Name = "colEstado_790MY";
            this.colEstado_790MY.ReadOnly = true;
            //
            // btnCancelarReserva_790MY
            //
            this.btnCancelarReserva_790MY.Enabled = false;
            this.btnCancelarReserva_790MY.Location = new System.Drawing.Point(20, 425);
            this.btnCancelarReserva_790MY.Name = "btnCancelarReserva_790MY";
            this.btnCancelarReserva_790MY.Size = new System.Drawing.Size(160, 32);
            this.btnCancelarReserva_790MY.Text = "Cancelar Reserva";
            this.btnCancelarReserva_790MY.Tag = "GR_btnCancelarReserva";
            this.btnCancelarReserva_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelarReserva_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnCancelarReserva_790MY.ForeColor = System.Drawing.Color.White;
            this.btnCancelarReserva_790MY.UseVisualStyleBackColor = false;
            this.btnCancelarReserva_790MY.Click += new System.EventHandler(this.btnCancelarReserva_790MY_Click);
            //
            // FormGestionReservas_790MY
            //
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(760, 480);
            this.Controls.Add(this.btnCancelarReserva_790MY);
            this.Controls.Add(this.dgvReservas_790MY);
            this.Controls.Add(this.btnBuscar_790MY);
            this.Controls.Add(this.cmbEstado_790MY);
            this.Controls.Add(this.lblEstado_790MY);
            this.Controls.Add(this.txtDni_790MY);
            this.Controls.Add(this.lblDni_790MY);
            this.Controls.Add(this.dtpHasta_790MY);
            this.Controls.Add(this.lblHasta_790MY);
            this.Controls.Add(this.dtpDesde_790MY);
            this.Controls.Add(this.lblDesde_790MY);
            this.Controls.Add(this.lblTitulo_790MY);
            this.Name = "FormGestionReservas_790MY";
            this.Text = "Consultar / Cancelar Reservas";
            this.Load += new System.EventHandler(this.FormGestionReservas_790MY_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReservas_790MY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo_790MY;
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
        private System.Windows.Forms.Button btnCancelarReserva_790MY;
    }
}
