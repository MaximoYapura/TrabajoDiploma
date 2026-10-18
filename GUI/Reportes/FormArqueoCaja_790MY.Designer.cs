namespace GUI_08YS.Reportes
{
    partial class FormArqueoCaja_790MY
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
            System.Windows.Forms.DataGridViewCellStyle estiloDerecha = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpMain_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.bnrAvisos_790MY = new GUI_08YS.Estilo.Banner_UC_790MY();
            this.tlpFiltro_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.lblFecha_790MY = new System.Windows.Forms.Label();
            this.btnAnterior_790MY = new FontAwesome.Sharp.IconButton();
            this.dtpFecha_790MY = new System.Windows.Forms.DateTimePicker();
            this.btnSiguiente_790MY = new FontAwesome.Sharp.IconButton();
            this.lblResumen_790MY = new System.Windows.Forms.Label();
            this.btnCerrarCaja_790MY = new FontAwesome.Sharp.IconButton();
            this.tlpKpis_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.lblNota_790MY = new System.Windows.Forms.Label();
            this.dgvCobros_790MY = new System.Windows.Forms.DataGridView();
            this.colHora_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMesa_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMedio_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescuento_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPropina_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colImporte_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblSinCobros_790MY = new System.Windows.Forms.Label();
            this.tlpMain_790MY.SuspendLayout();
            this.tlpFiltro_790MY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCobros_790MY)).BeginInit();
            this.SuspendLayout();
            //
            // tlpMain_790MY
            //
            this.tlpMain_790MY.ColumnCount = 1;
            this.tlpMain_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain_790MY.Controls.Add(this.tlpFiltro_790MY, 0, 0);
            this.tlpMain_790MY.Controls.Add(this.tlpKpis_790MY, 0, 1);
            this.tlpMain_790MY.Controls.Add(this.lblNota_790MY, 0, 2);
            this.tlpMain_790MY.Controls.Add(this.dgvCobros_790MY, 0, 3);
            this.tlpMain_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain_790MY.Name = "tlpMain_790MY";
            this.tlpMain_790MY.Padding = new System.Windows.Forms.Padding(24, 18, 24, 20);
            this.tlpMain_790MY.RowCount = 4;
            this.tlpMain_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpMain_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 132F));
            this.tlpMain_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpMain_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain_790MY.TabIndex = 0;
            //
            // tlpFiltro_790MY
            //
            this.tlpFiltro_790MY.AutoSize = true;
            this.tlpFiltro_790MY.ColumnCount = 5;
            this.tlpFiltro_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tlpFiltro_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tlpFiltro_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tlpFiltro_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFiltro_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltro_790MY.Controls.Add(this.lblFecha_790MY, 1, 0);
            this.tlpFiltro_790MY.Controls.Add(this.btnAnterior_790MY, 0, 1);
            this.tlpFiltro_790MY.Controls.Add(this.dtpFecha_790MY, 1, 1);
            this.tlpFiltro_790MY.Controls.Add(this.btnSiguiente_790MY, 2, 1);
            this.tlpFiltro_790MY.Controls.Add(this.lblResumen_790MY, 3, 1);
            this.tlpFiltro_790MY.Controls.Add(this.btnCerrarCaja_790MY, 4, 0);
            this.tlpFiltro_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFiltro_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 16);
            this.tlpFiltro_790MY.Name = "tlpFiltro_790MY";
            this.tlpFiltro_790MY.RowCount = 2;
            this.tlpFiltro_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFiltro_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tlpFiltro_790MY.SetRowSpan(this.btnCerrarCaja_790MY, 2);
            this.tlpFiltro_790MY.TabIndex = 0;
            //
            // lblFecha_790MY
            //
            this.lblFecha_790MY.AutoSize = true;
            this.lblFecha_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblFecha_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblFecha_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.lblFecha_790MY.Name = "lblFecha_790MY";
            this.lblFecha_790MY.TabIndex = 0;
            this.lblFecha_790MY.Tag = "AC_fecha";
            this.lblFecha_790MY.Text = "Fecha del arqueo";
            //
            // btnAnterior_790MY
            //
            this.btnAnterior_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(240)))), ((int)(((byte)(233)))));
            this.btnAnterior_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAnterior_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAnterior_790MY.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(216)))), ((int)(((byte)(202)))));
            this.btnAnterior_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnterior_790MY.IconChar = FontAwesome.Sharp.IconChar.ChevronLeft;
            this.btnAnterior_790MY.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.btnAnterior_790MY.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnAnterior_790MY.IconSize = 18;
            this.btnAnterior_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.btnAnterior_790MY.Name = "btnAnterior_790MY";
            this.btnAnterior_790MY.TabIndex = 1;
            this.btnAnterior_790MY.UseVisualStyleBackColor = false;
            this.btnAnterior_790MY.Click += new System.EventHandler(this.btnAnterior_790MY_Click);
            //
            // dtpFecha_790MY
            //
            this.dtpFecha_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpFecha_790MY.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.dtpFecha_790MY.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFecha_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.dtpFecha_790MY.Name = "dtpFecha_790MY";
            this.dtpFecha_790MY.TabIndex = 2;
            this.dtpFecha_790MY.ValueChanged += new System.EventHandler(this.dtpFecha_790MY_ValueChanged);
            //
            // btnSiguiente_790MY
            //
            this.btnSiguiente_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(240)))), ((int)(((byte)(233)))));
            this.btnSiguiente_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSiguiente_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSiguiente_790MY.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(216)))), ((int)(((byte)(202)))));
            this.btnSiguiente_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSiguiente_790MY.IconChar = FontAwesome.Sharp.IconChar.ChevronRight;
            this.btnSiguiente_790MY.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.btnSiguiente_790MY.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnSiguiente_790MY.IconSize = 18;
            this.btnSiguiente_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 18, 0);
            this.btnSiguiente_790MY.Name = "btnSiguiente_790MY";
            this.btnSiguiente_790MY.TabIndex = 3;
            this.btnSiguiente_790MY.UseVisualStyleBackColor = false;
            this.btnSiguiente_790MY.Click += new System.EventHandler(this.btnSiguiente_790MY_Click);
            //
            // lblResumen_790MY
            //
            this.lblResumen_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblResumen_790MY.AutoSize = true;
            this.lblResumen_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.lblResumen_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblResumen_790MY.Name = "lblResumen_790MY";
            this.lblResumen_790MY.TabIndex = 4;
            this.lblResumen_790MY.Text = "-";
            //
            // btnCerrarCaja_790MY
            //
            this.btnCerrarCaja_790MY.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCerrarCaja_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnCerrarCaja_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrarCaja_790MY.FlatAppearance.BorderSize = 0;
            this.btnCerrarCaja_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarCaja_790MY.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnCerrarCaja_790MY.ForeColor = System.Drawing.Color.White;
            this.btnCerrarCaja_790MY.IconChar = FontAwesome.Sharp.IconChar.CashRegister;
            this.btnCerrarCaja_790MY.IconColor = System.Drawing.Color.White;
            this.btnCerrarCaja_790MY.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCerrarCaja_790MY.IconSize = 22;
            this.btnCerrarCaja_790MY.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.btnCerrarCaja_790MY.Name = "btnCerrarCaja_790MY";
            this.btnCerrarCaja_790MY.Size = new System.Drawing.Size(230, 56);
            this.btnCerrarCaja_790MY.TabIndex = 5;
            this.btnCerrarCaja_790MY.Tag = "AC_btn_cerrar_caja";
            this.btnCerrarCaja_790MY.Text = " Cerrar caja del día";
            this.btnCerrarCaja_790MY.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCerrarCaja_790MY.UseVisualStyleBackColor = false;
            this.btnCerrarCaja_790MY.Click += new System.EventHandler(this.btnCerrarCaja_790MY_Click);
            //
            // tlpKpis_790MY  (las tarjetas KPI se arman en código)
            //
            this.tlpKpis_790MY.ColumnCount = 5;
            this.tlpKpis_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpKpis_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpKpis_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpKpis_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpKpis_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpKpis_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpKpis_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.tlpKpis_790MY.Name = "tlpKpis_790MY";
            this.tlpKpis_790MY.RowCount = 1;
            this.tlpKpis_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpKpis_790MY.TabIndex = 1;
            //
            // lblNota_790MY
            //
            this.lblNota_790MY.AutoSize = true;
            this.lblNota_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblNota_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblNota_790MY.Margin = new System.Windows.Forms.Padding(0, 10, 0, 14);
            this.lblNota_790MY.Name = "lblNota_790MY";
            this.lblNota_790MY.TabIndex = 2;
            this.lblNota_790MY.Tag = "AC_nota";
            this.lblNota_790MY.Text = "Recaudación neta = Efectivo + Tarjetas, con descuentos ya deducidos e IVA incluido. Las propinas se informan aparte.";
            //
            // dgvCobros_790MY
            //
            this.dgvCobros_790MY.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colHora_790MY,
            this.colMesa_790MY,
            this.colMedio_790MY,
            this.colDescuento_790MY,
            this.colPropina_790MY,
            this.colImporte_790MY});
            this.dgvCobros_790MY.Controls.Add(this.lblSinCobros_790MY);
            this.dgvCobros_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCobros_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.dgvCobros_790MY.Name = "dgvCobros_790MY";
            this.dgvCobros_790MY.TabIndex = 3;
            this.dgvCobros_790MY.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvCobros_790MY_CellPainting);
            this.dgvCobros_790MY.Resize += new System.EventHandler(this.dgvCobros_790MY_Resize);
            //
            // colHora_790MY
            //
            this.colHora_790MY.FillWeight = 10F;
            this.colHora_790MY.HeaderText = "Hora";
            this.colHora_790MY.Name = "colHora_790MY";
            //
            // colMesa_790MY
            //
            this.colMesa_790MY.FillWeight = 10F;
            this.colMesa_790MY.HeaderText = "Mesa";
            this.colMesa_790MY.Name = "colMesa_790MY";
            //
            // colMedio_790MY
            //
            this.colMedio_790MY.FillWeight = 22F;
            this.colMedio_790MY.HeaderText = "Medio";
            this.colMedio_790MY.Name = "colMedio_790MY";
            //
            // colDescuento_790MY
            //
            estiloDerecha.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            estiloDerecha.Padding = new System.Windows.Forms.Padding(0, 0, 22, 0);
            this.colDescuento_790MY.DefaultCellStyle = estiloDerecha;
            this.colDescuento_790MY.FillWeight = 20F;
            this.colDescuento_790MY.HeaderText = "Descuento";
            this.colDescuento_790MY.Name = "colDescuento_790MY";
            //
            // colPropina_790MY
            //
            this.colPropina_790MY.DefaultCellStyle = estiloDerecha;
            this.colPropina_790MY.FillWeight = 18F;
            this.colPropina_790MY.HeaderText = "Propina";
            this.colPropina_790MY.Name = "colPropina_790MY";
            //
            // colImporte_790MY
            //
            this.colImporte_790MY.DefaultCellStyle = estiloDerecha;
            this.colImporte_790MY.FillWeight = 20F;
            this.colImporte_790MY.HeaderText = "Importe cobrado";
            this.colImporte_790MY.Name = "colImporte_790MY";
            //
            // lblSinCobros_790MY
            //
            this.lblSinCobros_790MY.BackColor = System.Drawing.Color.White;
            this.lblSinCobros_790MY.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.lblSinCobros_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblSinCobros_790MY.Name = "lblSinCobros_790MY";
            this.lblSinCobros_790MY.TabIndex = 0;
            this.lblSinCobros_790MY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSinCobros_790MY.Visible = false;
            //
            // bnrAvisos_790MY
            //
            this.bnrAvisos_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.bnrAvisos_790MY.Name = "bnrAvisos_790MY";
            this.bnrAvisos_790MY.TabIndex = 1;
            //
            // FormArqueoCaja_790MY
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(1400, 820);
            this.Controls.Add(this.tlpMain_790MY);
            this.Controls.Add(this.bnrAvisos_790MY);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormArqueoCaja_790MY";
            this.Tag = "AC_titulo";
            this.Text = "Arqueo y cierre de caja";
            this.Load += new System.EventHandler(this.FormArqueoCaja_790MY_Load);
            this.tlpMain_790MY.ResumeLayout(false);
            this.tlpMain_790MY.PerformLayout();
            this.tlpFiltro_790MY.ResumeLayout(false);
            this.tlpFiltro_790MY.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCobros_790MY)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain_790MY;
        private System.Windows.Forms.TableLayoutPanel tlpFiltro_790MY;
        private System.Windows.Forms.Label lblFecha_790MY;
        private FontAwesome.Sharp.IconButton btnAnterior_790MY;
        private System.Windows.Forms.DateTimePicker dtpFecha_790MY;
        private FontAwesome.Sharp.IconButton btnSiguiente_790MY;
        private System.Windows.Forms.Label lblResumen_790MY;
        private FontAwesome.Sharp.IconButton btnCerrarCaja_790MY;
        private System.Windows.Forms.TableLayoutPanel tlpKpis_790MY;
        private System.Windows.Forms.Label lblNota_790MY;
        private System.Windows.Forms.DataGridView dgvCobros_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHora_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMesa_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMedio_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescuento_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPropina_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colImporte_790MY;
        private System.Windows.Forms.Label lblSinCobros_790MY;
        private GUI_08YS.Estilo.Banner_UC_790MY bnrAvisos_790MY;
    }
}
