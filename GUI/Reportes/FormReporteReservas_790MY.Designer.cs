namespace GUI_08YS.Reportes
{
    partial class FormReporteReservas_790MY
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle headerCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle altRowStyle      = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle fechaCellStyle   = new System.Windows.Forms.DataGridViewCellStyle();

            // Panels
            this.pnlFiltros  = new System.Windows.Forms.Panel();
            this.pnlBotones  = new System.Windows.Forms.Panel();

            // Filter labels
            this.lblDesde    = new System.Windows.Forms.Label();
            this.lblHasta    = new System.Windows.Forms.Label();
            this.lblDni      = new System.Windows.Forms.Label();
            this.lblEstado   = new System.Windows.Forms.Label();

            // Filter controls
            this.dtpDesde_790MY       = new System.Windows.Forms.DateTimePicker();
            this.dtpHasta_790MY       = new System.Windows.Forms.DateTimePicker();
            this.txtDniCliente_790MY  = new System.Windows.Forms.TextBox();
            this.cboEstado_790MY      = new System.Windows.Forms.ComboBox();
            this.btnBuscar_790MY      = new System.Windows.Forms.Button();

            // DataGridView and columns
            this.dgvReporteReservas_790MY = new System.Windows.Forms.DataGridView();
            this.colRepID_790MY           = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRepDni_790MY          = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRepCliente_790MY      = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRepMesa_790MY         = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRepFecha_790MY        = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRepHora_790MY         = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRepComensales_790MY   = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRepEstado_790MY       = new System.Windows.Forms.DataGridViewTextBoxColumn();

            // Bottom buttons
            this.btnLimpiar_790MY    = new System.Windows.Forms.Button();
            this.btnGenerarPDF_790MY = new System.Windows.Forms.Button();
            this.btnCerrar_790MY     = new System.Windows.Forms.Button();

            this.pnlFiltros.SuspendLayout();
            this.pnlBotones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReporteReservas_790MY)).BeginInit();
            this.SuspendLayout();

            // ── pnlFiltros ──────────────────────────────────────────────────────────────
            this.pnlFiltros.BackColor = System.Drawing.Color.FromArgb(250, 248, 245);
            this.pnlFiltros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFiltros.Controls.Add(this.btnBuscar_790MY);
            this.pnlFiltros.Controls.Add(this.cboEstado_790MY);
            this.pnlFiltros.Controls.Add(this.lblEstado);
            this.pnlFiltros.Controls.Add(this.txtDniCliente_790MY);
            this.pnlFiltros.Controls.Add(this.lblDni);
            this.pnlFiltros.Controls.Add(this.dtpHasta_790MY);
            this.pnlFiltros.Controls.Add(this.lblHasta);
            this.pnlFiltros.Controls.Add(this.dtpDesde_790MY);
            this.pnlFiltros.Controls.Add(this.lblDesde);
            this.pnlFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFiltros.Location = new System.Drawing.Point(0, 0);
            this.pnlFiltros.Name = "pnlFiltros";
            this.pnlFiltros.Padding = new System.Windows.Forms.Padding(6);
            this.pnlFiltros.Size = new System.Drawing.Size(1000, 75);
            this.pnlFiltros.TabIndex = 0;

            // ── lblDesde ──
            this.lblDesde.AutoSize = true;
            this.lblDesde.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblDesde.Location = new System.Drawing.Point(10, 28);
            this.lblDesde.Name = "lblDesde";
            this.lblDesde.Size = new System.Drawing.Size(47, 13);
            this.lblDesde.TabIndex = 0;
            this.lblDesde.Tag = "REP_lblDesde";
            this.lblDesde.Text = "Desde:";

            // ── dtpDesde_790MY ──
            this.dtpDesde_790MY.Checked = false;
            this.dtpDesde_790MY.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde_790MY.Location = new System.Drawing.Point(63, 24);
            this.dtpDesde_790MY.Name = "dtpDesde_790MY";
            this.dtpDesde_790MY.ShowCheckBox = true;
            this.dtpDesde_790MY.Size = new System.Drawing.Size(165, 20);
            this.dtpDesde_790MY.TabIndex = 1;

            // ── lblHasta ──
            this.lblHasta.AutoSize = true;
            this.lblHasta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblHasta.Location = new System.Drawing.Point(240, 28);
            this.lblHasta.Name = "lblHasta";
            this.lblHasta.Size = new System.Drawing.Size(43, 13);
            this.lblHasta.TabIndex = 2;
            this.lblHasta.Tag = "REP_lblHasta";
            this.lblHasta.Text = "Hasta:";

            // ── dtpHasta_790MY ──
            this.dtpHasta_790MY.Checked = false;
            this.dtpHasta_790MY.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta_790MY.Location = new System.Drawing.Point(291, 24);
            this.dtpHasta_790MY.Name = "dtpHasta_790MY";
            this.dtpHasta_790MY.ShowCheckBox = true;
            this.dtpHasta_790MY.Size = new System.Drawing.Size(165, 20);
            this.dtpHasta_790MY.TabIndex = 3;

            // ── lblDni ──
            this.lblDni.AutoSize = true;
            this.lblDni.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblDni.Location = new System.Drawing.Point(470, 28);
            this.lblDni.Name = "lblDni";
            this.lblDni.Size = new System.Drawing.Size(65, 13);
            this.lblDni.TabIndex = 4;
            this.lblDni.Tag = "REP_lblDni";
            this.lblDni.Text = "DNI cliente:";

            // ── txtDniCliente_790MY ──
            this.txtDniCliente_790MY.Location = new System.Drawing.Point(542, 24);
            this.txtDniCliente_790MY.MaxLength = 10;
            this.txtDniCliente_790MY.Name = "txtDniCliente_790MY";
            this.txtDniCliente_790MY.Size = new System.Drawing.Size(100, 20);
            this.txtDniCliente_790MY.TabIndex = 5;

            // ── lblEstado ──
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblEstado.Location = new System.Drawing.Point(655, 28);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(44, 13);
            this.lblEstado.TabIndex = 6;
            this.lblEstado.Tag = "REP_lblEstado";
            this.lblEstado.Text = "Estado:";

            // ── cboEstado_790MY ──
            this.cboEstado_790MY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEstado_790MY.Location = new System.Drawing.Point(706, 24);
            this.cboEstado_790MY.Name = "cboEstado_790MY";
            this.cboEstado_790MY.Size = new System.Drawing.Size(155, 21);
            this.cboEstado_790MY.TabIndex = 7;

            // ── btnBuscar_790MY ──
            this.btnBuscar_790MY.BackColor = System.Drawing.Color.FromArgb(120, 25, 25);
            this.btnBuscar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscar_790MY.FlatAppearance.BorderSize = 0;
            this.btnBuscar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnBuscar_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnBuscar_790MY.Location = new System.Drawing.Point(878, 16);
            this.btnBuscar_790MY.Name = "btnBuscar_790MY";
            this.btnBuscar_790MY.Size = new System.Drawing.Size(110, 35);
            this.btnBuscar_790MY.TabIndex = 8;
            this.btnBuscar_790MY.Tag = "btnBuscar";
            this.btnBuscar_790MY.Text = "Buscar";
            this.btnBuscar_790MY.UseVisualStyleBackColor = false;
            this.btnBuscar_790MY.Click += new System.EventHandler(this.btnBuscar_790MY_Click);

            // ── pnlBotones ──────────────────────────────────────────────────────────────
            this.pnlBotones.BackColor = System.Drawing.Color.FromArgb(250, 248, 245);
            this.pnlBotones.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBotones.Controls.Add(this.btnCerrar_790MY);
            this.pnlBotones.Controls.Add(this.btnGenerarPDF_790MY);
            this.pnlBotones.Controls.Add(this.btnLimpiar_790MY);
            this.pnlBotones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBotones.Location = new System.Drawing.Point(0, 570);
            this.pnlBotones.Name = "pnlBotones";
            this.pnlBotones.Size = new System.Drawing.Size(1000, 55);
            this.pnlBotones.TabIndex = 2;

            // ── btnLimpiar_790MY ──
            this.btnLimpiar_790MY.BackColor = System.Drawing.Color.FromArgb(120, 25, 25);
            this.btnLimpiar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLimpiar_790MY.FlatAppearance.BorderSize = 0;
            this.btnLimpiar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnLimpiar_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnLimpiar_790MY.Location = new System.Drawing.Point(550, 12);
            this.btnLimpiar_790MY.Name = "btnLimpiar_790MY";
            this.btnLimpiar_790MY.Size = new System.Drawing.Size(150, 30);
            this.btnLimpiar_790MY.TabIndex = 2;
            this.btnLimpiar_790MY.Tag = "btnLimpiar";
            this.btnLimpiar_790MY.Text = "Limpiar";
            this.btnLimpiar_790MY.UseVisualStyleBackColor = false;
            this.btnLimpiar_790MY.Click += new System.EventHandler(this.btnLimpiar_790MY_Click);

            // ── btnCerrar_790MY ──
            this.btnCerrar_790MY.BackColor = System.Drawing.Color.FromArgb(120, 25, 25);
            this.btnCerrar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar_790MY.FlatAppearance.BorderSize = 0;
            this.btnCerrar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnCerrar_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnCerrar_790MY.Location = new System.Drawing.Point(890, 12);
            this.btnCerrar_790MY.Name = "btnCerrar_790MY";
            this.btnCerrar_790MY.Size = new System.Drawing.Size(100, 30);
            this.btnCerrar_790MY.TabIndex = 1;
            this.btnCerrar_790MY.Tag = "REP_btnCerrar";
            this.btnCerrar_790MY.Text = "Cerrar";
            this.btnCerrar_790MY.UseVisualStyleBackColor = false;
            this.btnCerrar_790MY.Click += new System.EventHandler(this.btnCerrar_790MY_Click);

            // ── btnGenerarPDF_790MY ──
            this.btnGenerarPDF_790MY.BackColor = System.Drawing.Color.FromArgb(120, 25, 25);
            this.btnGenerarPDF_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerarPDF_790MY.FlatAppearance.BorderSize = 0;
            this.btnGenerarPDF_790MY.ForeColor = System.Drawing.Color.White;
            this.btnGenerarPDF_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnGenerarPDF_790MY.Enabled = false;
            this.btnGenerarPDF_790MY.Location = new System.Drawing.Point(720, 12);
            this.btnGenerarPDF_790MY.Name = "btnGenerarPDF_790MY";
            this.btnGenerarPDF_790MY.Size = new System.Drawing.Size(160, 30);
            this.btnGenerarPDF_790MY.TabIndex = 0;
            this.btnGenerarPDF_790MY.Tag = "REP_btnGenerarPDF";
            this.btnGenerarPDF_790MY.Text = "Exportar PDF";
            this.btnGenerarPDF_790MY.UseVisualStyleBackColor = false;
            this.btnGenerarPDF_790MY.Click += new System.EventHandler(this.btnGenerarPDF_790MY_Click);

            // ── dgvReporteReservas_790MY ────────────────────────────────────────────────
            headerCellStyle.BackColor            = System.Drawing.Color.FromArgb(120, 25, 25);
            headerCellStyle.ForeColor            = System.Drawing.Color.White;
            headerCellStyle.SelectionBackColor   = System.Drawing.Color.FromArgb(100, 20, 20);
            headerCellStyle.SelectionForeColor   = System.Drawing.Color.White;
            headerCellStyle.Font                 = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);

            altRowStyle.BackColor = System.Drawing.Color.FromArgb(250, 248, 245);

            this.dgvReporteReservas_790MY.AllowUserToAddRows    = false;
            this.dgvReporteReservas_790MY.AllowUserToDeleteRows = false;
            this.dgvReporteReservas_790MY.AutoGenerateColumns   = false;
            this.dgvReporteReservas_790MY.AlternatingRowsDefaultCellStyle = altRowStyle;
            this.dgvReporteReservas_790MY.BackgroundColor       = System.Drawing.Color.White;
            this.dgvReporteReservas_790MY.BorderStyle           = System.Windows.Forms.BorderStyle.None;
            this.dgvReporteReservas_790MY.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvReporteReservas_790MY.ColumnHeadersDefaultCellStyle = headerCellStyle;
            this.dgvReporteReservas_790MY.ColumnHeadersHeight   = 30;
            this.dgvReporteReservas_790MY.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[]
            {
                this.colRepID_790MY,
                this.colRepDni_790MY,
                this.colRepCliente_790MY,
                this.colRepMesa_790MY,
                this.colRepFecha_790MY,
                this.colRepHora_790MY,
                this.colRepComensales_790MY,
                this.colRepEstado_790MY
            });
            this.dgvReporteReservas_790MY.Dock              = System.Windows.Forms.DockStyle.Fill;
            this.dgvReporteReservas_790MY.EnableHeadersVisualStyles = false;
            this.dgvReporteReservas_790MY.Location          = new System.Drawing.Point(0, 75);
            this.dgvReporteReservas_790MY.MultiSelect       = false;
            this.dgvReporteReservas_790MY.Name              = "dgvReporteReservas_790MY";
            this.dgvReporteReservas_790MY.ReadOnly          = true;
            this.dgvReporteReservas_790MY.RowHeadersVisible = false;
            this.dgvReporteReservas_790MY.RowTemplate.Height = 24;
            this.dgvReporteReservas_790MY.SelectionMode     = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReporteReservas_790MY.Size              = new System.Drawing.Size(1000, 495);
            this.dgvReporteReservas_790MY.TabIndex          = 1;
            this.dgvReporteReservas_790MY.CellFormatting   += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvReporteReservas_790MY_CellFormatting);

            // ── Columnas del DGV ────────────────────────────────────────────────────────

            // colRepID_790MY
            this.colRepID_790MY.DataPropertyName    = "ReservaID";
            this.colRepID_790MY.HeaderText           = "Nro.";
            this.colRepID_790MY.Name                 = "colRepID_790MY";
            this.colRepID_790MY.ReadOnly             = true;
            this.colRepID_790MY.Width                = 55;

            // colRepDni_790MY
            this.colRepDni_790MY.DataPropertyName   = "ClienteDNI";
            this.colRepDni_790MY.HeaderText          = "DNI";
            this.colRepDni_790MY.Name                = "colRepDni_790MY";
            this.colRepDni_790MY.ReadOnly            = true;
            this.colRepDni_790MY.Width               = 90;

            // colRepCliente_790MY (auto-fill remaining space)
            this.colRepCliente_790MY.AutoSizeMode    = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colRepCliente_790MY.DataPropertyName = "ClienteNombreCompleto";
            this.colRepCliente_790MY.HeaderText       = "Cliente";
            this.colRepCliente_790MY.Name             = "colRepCliente_790MY";
            this.colRepCliente_790MY.ReadOnly         = true;

            // colRepMesa_790MY
            this.colRepMesa_790MY.DataPropertyName  = "MesaNumero";
            this.colRepMesa_790MY.HeaderText         = "Mesa";
            this.colRepMesa_790MY.Name               = "colRepMesa_790MY";
            this.colRepMesa_790MY.ReadOnly           = true;
            this.colRepMesa_790MY.Width              = 55;

            // colRepFecha_790MY
            fechaCellStyle.Format                    = "dd/MM/yyyy";
            this.colRepFecha_790MY.DataPropertyName  = "Fecha";
            this.colRepFecha_790MY.DefaultCellStyle  = fechaCellStyle;
            this.colRepFecha_790MY.HeaderText        = "Fecha";
            this.colRepFecha_790MY.Name              = "colRepFecha_790MY";
            this.colRepFecha_790MY.ReadOnly          = true;
            this.colRepFecha_790MY.Width             = 95;

            // colRepHora_790MY
            this.colRepHora_790MY.DataPropertyName  = "Hora";
            this.colRepHora_790MY.HeaderText         = "Hora";
            this.colRepHora_790MY.Name               = "colRepHora_790MY";
            this.colRepHora_790MY.ReadOnly           = true;
            this.colRepHora_790MY.Width              = 70;

            // colRepComensales_790MY
            this.colRepComensales_790MY.DataPropertyName = "CantidadComensales";
            this.colRepComensales_790MY.HeaderText        = "Comensales";
            this.colRepComensales_790MY.Name              = "colRepComensales_790MY";
            this.colRepComensales_790MY.ReadOnly          = true;
            this.colRepComensales_790MY.Width             = 95;

            // colRepEstado_790MY
            this.colRepEstado_790MY.DataPropertyName = "Estado";
            this.colRepEstado_790MY.HeaderText        = "Estado";
            this.colRepEstado_790MY.Name              = "colRepEstado_790MY";
            this.colRepEstado_790MY.ReadOnly          = true;
            this.colRepEstado_790MY.Width             = 100;

            // ── FormReporteReservas_790MY ────────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor           = System.Drawing.Color.FromArgb(250, 248, 245);
            this.ClientSize          = new System.Drawing.Size(1000, 625);
            this.Controls.Add(this.dgvReporteReservas_790MY);
            this.Controls.Add(this.pnlBotones);
            this.Controls.Add(this.pnlFiltros);
            this.Name            = "FormReporteReservas_790MY";
            this.Text            = "Reporte de Reservas";
            this.Load           += new System.EventHandler(this.FormReporteReservas_790MY_Load);

            this.pnlFiltros.ResumeLayout(false);
            this.pnlFiltros.PerformLayout();
            this.pnlBotones.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReporteReservas_790MY)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel                       pnlFiltros;
        private System.Windows.Forms.Panel                       pnlBotones;
        private System.Windows.Forms.Label                       lblDesde;
        private System.Windows.Forms.Label                       lblHasta;
        private System.Windows.Forms.Label                       lblDni;
        private System.Windows.Forms.Label                       lblEstado;
        private System.Windows.Forms.DateTimePicker              dtpDesde_790MY;
        private System.Windows.Forms.DateTimePicker              dtpHasta_790MY;
        private System.Windows.Forms.TextBox                     txtDniCliente_790MY;
        private System.Windows.Forms.ComboBox                    cboEstado_790MY;
        private System.Windows.Forms.Button                      btnBuscar_790MY;
        private System.Windows.Forms.DataGridView                dgvReporteReservas_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn   colRepID_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn   colRepDni_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn   colRepCliente_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn   colRepMesa_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn   colRepFecha_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn   colRepHora_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn   colRepComensales_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn   colRepEstado_790MY;
        private System.Windows.Forms.Button                      btnLimpiar_790MY;
        private System.Windows.Forms.Button                      btnGenerarPDF_790MY;
        private System.Windows.Forms.Button                      btnCerrar_790MY;
    }
}
