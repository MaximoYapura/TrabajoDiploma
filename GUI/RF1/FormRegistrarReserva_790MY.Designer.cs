namespace GUI_08YS.RF1
{
    partial class FormRegistrarReserva_790MY
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
            this.lblDniCliente_790MY = new System.Windows.Forms.Label();
            this.txtDniCliente_790MY = new System.Windows.Forms.TextBox();
            this.btnBuscarCliente_790MY = new System.Windows.Forms.Button();
            this.lblClienteInfo_790MY = new System.Windows.Forms.Label();
            this.lblFecha_790MY = new System.Windows.Forms.Label();
            this.dtpFecha_790MY = new System.Windows.Forms.DateTimePicker();
            this.lblHora_790MY = new System.Windows.Forms.Label();
            this.cmbHora_790MY = new System.Windows.Forms.ComboBox();
            this.lblComensales_790MY = new System.Windows.Forms.Label();
            this.nudComensales_790MY = new System.Windows.Forms.NumericUpDown();
            this.btnSeleccionarMesa_790MY = new System.Windows.Forms.Button();
            this.lblMesaSeleccionada_790MY = new System.Windows.Forms.Label();
            this.btnConfirmar_790MY = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudComensales_790MY)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo_790MY
            // 
            this.lblTitulo_790MY.AutoSize = true;
            this.lblTitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblTitulo_790MY.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo_790MY.Name = "lblTitulo_790MY";
            this.lblTitulo_790MY.Size = new System.Drawing.Size(142, 21);
            this.lblTitulo_790MY.TabIndex = 13;
            this.lblTitulo_790MY.Tag = "RR_titulo";
            this.lblTitulo_790MY.Text = "Registrar Reserva";
            // 
            // lblDniCliente_790MY
            // 
            this.lblDniCliente_790MY.AutoSize = true;
            this.lblDniCliente_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDniCliente_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblDniCliente_790MY.Location = new System.Drawing.Point(57, 65);
            this.lblDniCliente_790MY.Name = "lblDniCliente_790MY";
            this.lblDniCliente_790MY.Size = new System.Drawing.Size(94, 20);
            this.lblDniCliente_790MY.TabIndex = 12;
            this.lblDniCliente_790MY.Tag = "RR_lblDniCliente";
            this.lblDniCliente_790MY.Text = "DNI Cliente:";
            // 
            // txtDniCliente_790MY
            // 
            this.txtDniCliente_790MY.Location = new System.Drawing.Point(210, 65);
            this.txtDniCliente_790MY.Name = "txtDniCliente_790MY";
            this.txtDniCliente_790MY.Size = new System.Drawing.Size(555, 20);
            this.txtDniCliente_790MY.TabIndex = 11;
            // 
            // btnBuscarCliente_790MY
            // 
            this.btnBuscarCliente_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(235)))), ((int)(((byte)(228)))));
            this.btnBuscarCliente_790MY.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnBuscarCliente_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscarCliente_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuscarCliente_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(30)))), ((int)(((byte)(20)))));
            this.btnBuscarCliente_790MY.Location = new System.Drawing.Point(848, 57);
            this.btnBuscarCliente_790MY.Name = "btnBuscarCliente_790MY";
            this.btnBuscarCliente_790MY.Size = new System.Drawing.Size(171, 36);
            this.btnBuscarCliente_790MY.TabIndex = 10;
            this.btnBuscarCliente_790MY.Tag = "btnBuscar";
            this.btnBuscarCliente_790MY.Text = "Buscar";
            this.btnBuscarCliente_790MY.UseVisualStyleBackColor = false;
            this.btnBuscarCliente_790MY.Click += new System.EventHandler(this.btnBuscarCliente_790MY_Click);
            // 
            // lblClienteInfo_790MY
            // 
            this.lblClienteInfo_790MY.AutoSize = true;
            this.lblClienteInfo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.lblClienteInfo_790MY.Location = new System.Drawing.Point(365, 60);
            this.lblClienteInfo_790MY.Name = "lblClienteInfo_790MY";
            this.lblClienteInfo_790MY.Size = new System.Drawing.Size(0, 13);
            this.lblClienteInfo_790MY.TabIndex = 9;
            // 
            // lblFecha_790MY
            // 
            this.lblFecha_790MY.AutoSize = true;
            this.lblFecha_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFecha_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblFecha_790MY.Location = new System.Drawing.Point(150, 193);
            this.lblFecha_790MY.Name = "lblFecha_790MY";
            this.lblFecha_790MY.Size = new System.Drawing.Size(58, 20);
            this.lblFecha_790MY.TabIndex = 8;
            this.lblFecha_790MY.Tag = "RR_lblFecha";
            this.lblFecha_790MY.Text = "Fecha:";
            // 
            // dtpFecha_790MY
            // 
            this.dtpFecha_790MY.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFecha_790MY.Location = new System.Drawing.Point(232, 192);
            this.dtpFecha_790MY.Name = "dtpFecha_790MY";
            this.dtpFecha_790MY.Size = new System.Drawing.Size(160, 20);
            this.dtpFecha_790MY.TabIndex = 7;
            this.dtpFecha_790MY.ValueChanged += new System.EventHandler(this.InvalidarMesaSeleccionada);
            // 
            // lblHora_790MY
            // 
            this.lblHora_790MY.AutoSize = true;
            this.lblHora_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHora_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblHora_790MY.Location = new System.Drawing.Point(150, 282);
            this.lblHora_790MY.Name = "lblHora_790MY";
            this.lblHora_790MY.Size = new System.Drawing.Size(54, 20);
            this.lblHora_790MY.TabIndex = 6;
            this.lblHora_790MY.Tag = "RR_lblHora";
            this.lblHora_790MY.Text = "Turno:";
            // 
            // cmbHora_790MY
            // 
            this.cmbHora_790MY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHora_790MY.Location = new System.Drawing.Point(232, 281);
            this.cmbHora_790MY.Name = "cmbHora_790MY";
            this.cmbHora_790MY.Size = new System.Drawing.Size(160, 21);
            this.cmbHora_790MY.TabIndex = 5;
            this.cmbHora_790MY.SelectedIndexChanged += new System.EventHandler(this.InvalidarMesaSeleccionada);
            // 
            // lblComensales_790MY
            // 
            this.lblComensales_790MY.AutoSize = true;
            this.lblComensales_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComensales_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblComensales_790MY.Location = new System.Drawing.Point(506, 187);
            this.lblComensales_790MY.Name = "lblComensales_790MY";
            this.lblComensales_790MY.Size = new System.Drawing.Size(101, 20);
            this.lblComensales_790MY.TabIndex = 4;
            this.lblComensales_790MY.Tag = "RR_lblComensales";
            this.lblComensales_790MY.Text = "Comensales:";
            // 
            // nudComensales_790MY
            // 
            this.nudComensales_790MY.Location = new System.Drawing.Point(636, 187);
            this.nudComensales_790MY.Name = "nudComensales_790MY";
            this.nudComensales_790MY.Size = new System.Drawing.Size(148, 20);
            this.nudComensales_790MY.TabIndex = 3;
            this.nudComensales_790MY.ValueChanged += new System.EventHandler(this.InvalidarMesaSeleccionada);
            // 
            // btnSeleccionarMesa_790MY
            // 
            this.btnSeleccionarMesa_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(235)))), ((int)(((byte)(228)))));
            this.btnSeleccionarMesa_790MY.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnSeleccionarMesa_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSeleccionarMesa_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSeleccionarMesa_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(30)))), ((int)(((byte)(20)))));
            this.btnSeleccionarMesa_790MY.Location = new System.Drawing.Point(510, 274);
            this.btnSeleccionarMesa_790MY.Name = "btnSeleccionarMesa_790MY";
            this.btnSeleccionarMesa_790MY.Size = new System.Drawing.Size(171, 36);
            this.btnSeleccionarMesa_790MY.TabIndex = 2;
            this.btnSeleccionarMesa_790MY.Tag = "RR_btnSeleccionarMesa";
            this.btnSeleccionarMesa_790MY.Text = "Seleccionar Mesa";
            this.btnSeleccionarMesa_790MY.UseVisualStyleBackColor = false;
            this.btnSeleccionarMesa_790MY.Click += new System.EventHandler(this.btnSeleccionarMesa_790MY_Click);
            // 
            // lblMesaSeleccionada_790MY
            // 
            this.lblMesaSeleccionada_790MY.AutoSize = true;
            this.lblMesaSeleccionada_790MY.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMesaSeleccionada_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblMesaSeleccionada_790MY.Location = new System.Drawing.Point(734, 277);
            this.lblMesaSeleccionada_790MY.Name = "lblMesaSeleccionada_790MY";
            this.lblMesaSeleccionada_790MY.Size = new System.Drawing.Size(134, 21);
            this.lblMesaSeleccionada_790MY.TabIndex = 1;
            this.lblMesaSeleccionada_790MY.Tag = "RR_lblMesaSinSeleccionar";
            this.lblMesaSeleccionada_790MY.Text = "(sin seleccionar)";
            // 
            // btnConfirmar_790MY
            // 
            this.btnConfirmar_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnConfirmar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirmar_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirmar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnConfirmar_790MY.Location = new System.Drawing.Point(436, 415);
            this.btnConfirmar_790MY.Name = "btnConfirmar_790MY";
            this.btnConfirmar_790MY.Size = new System.Drawing.Size(171, 36);
            this.btnConfirmar_790MY.TabIndex = 0;
            this.btnConfirmar_790MY.Tag = "RR_btnConfirmar";
            this.btnConfirmar_790MY.Text = "Confirmar Reserva";
            this.btnConfirmar_790MY.UseVisualStyleBackColor = false;
            this.btnConfirmar_790MY.Click += new System.EventHandler(this.btnConfirmar_790MY_Click);
            // 
            // FormRegistrarReserva_790MY
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(1073, 704);
            this.Controls.Add(this.btnConfirmar_790MY);
            this.Controls.Add(this.lblMesaSeleccionada_790MY);
            this.Controls.Add(this.btnSeleccionarMesa_790MY);
            this.Controls.Add(this.nudComensales_790MY);
            this.Controls.Add(this.lblComensales_790MY);
            this.Controls.Add(this.cmbHora_790MY);
            this.Controls.Add(this.lblHora_790MY);
            this.Controls.Add(this.dtpFecha_790MY);
            this.Controls.Add(this.lblFecha_790MY);
            this.Controls.Add(this.lblClienteInfo_790MY);
            this.Controls.Add(this.btnBuscarCliente_790MY);
            this.Controls.Add(this.txtDniCliente_790MY);
            this.Controls.Add(this.lblDniCliente_790MY);
            this.Controls.Add(this.lblTitulo_790MY);
            this.Name = "FormRegistrarReserva_790MY";
            this.Text = "Registrar Reserva";
            this.Load += new System.EventHandler(this.FormRegistrarReserva_790MY_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudComensales_790MY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo_790MY;
        private System.Windows.Forms.Label lblDniCliente_790MY;
        private System.Windows.Forms.TextBox txtDniCliente_790MY;
        private System.Windows.Forms.Button btnBuscarCliente_790MY;
        private System.Windows.Forms.Label lblClienteInfo_790MY;
        private System.Windows.Forms.Label lblFecha_790MY;
        private System.Windows.Forms.DateTimePicker dtpFecha_790MY;
        private System.Windows.Forms.Label lblHora_790MY;
        private System.Windows.Forms.ComboBox cmbHora_790MY;
        private System.Windows.Forms.Label lblComensales_790MY;
        private System.Windows.Forms.NumericUpDown nudComensales_790MY;
        private System.Windows.Forms.Button btnSeleccionarMesa_790MY;
        private System.Windows.Forms.Label lblMesaSeleccionada_790MY;
        private System.Windows.Forms.Button btnConfirmar_790MY;
    }
}
