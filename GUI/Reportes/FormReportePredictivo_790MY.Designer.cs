namespace GUI_08YS.Reportes
{
    partial class FormReportePredictivo_790MY
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
            this.tlpMain_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.bnrAvisos_790MY = new GUI_08YS.Estilo.Banner_UC_790MY();
            this.tlpEncabezado_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitulo_790MY = new System.Windows.Forms.Label();
            this.lblSubtitulo_790MY = new System.Windows.Forms.Label();
            this.pnlConfianza_790MY = new System.Windows.Forms.Panel();
            this.lblConfianza_790MY = new System.Windows.Forms.Label();
            this.sldConfianza_790MY = new GUI_08YS.Reportes.SliderBordo_790MY();
            this.pnlOrden_790MY = new GUI_08YS.RF2.PanelRedondeado_790MY();
            this.btnPorConfianza_790MY = new System.Windows.Forms.Button();
            this.btnPorSoporte_790MY = new System.Windows.Forms.Button();
            this.dgvReglas_790MY = new System.Windows.Forms.DataGridView();
            this.colNumero_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCombinacion_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoporte_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colConfianza_790MY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblSinReglas_790MY = new System.Windows.Forms.Label();
            this.flpTituloSugerencias_790MY = new System.Windows.Forms.FlowLayoutPanel();
            this.icoSugerencias_790MY = new FontAwesome.Sharp.IconPictureBox();
            this.lblSugerencias_790MY = new System.Windows.Forms.Label();
            this.tlpSugerencias_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.tlpMain_790MY.SuspendLayout();
            this.tlpEncabezado_790MY.SuspendLayout();
            this.pnlConfianza_790MY.SuspendLayout();
            this.pnlOrden_790MY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReglas_790MY)).BeginInit();
            this.flpTituloSugerencias_790MY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.icoSugerencias_790MY)).BeginInit();
            this.SuspendLayout();
            //
            // tlpMain_790MY
            //
            this.tlpMain_790MY.ColumnCount = 1;
            this.tlpMain_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain_790MY.Controls.Add(this.tlpEncabezado_790MY, 0, 0);
            this.tlpMain_790MY.Controls.Add(this.dgvReglas_790MY, 0, 1);
            this.tlpMain_790MY.Controls.Add(this.flpTituloSugerencias_790MY, 0, 2);
            this.tlpMain_790MY.Controls.Add(this.tlpSugerencias_790MY, 0, 3);
            this.tlpMain_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain_790MY.Name = "tlpMain_790MY";
            this.tlpMain_790MY.Padding = new System.Windows.Forms.Padding(24, 18, 24, 20);
            this.tlpMain_790MY.RowCount = 4;
            this.tlpMain_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpMain_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpMain_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 190F));
            this.tlpMain_790MY.TabIndex = 0;
            //
            // tlpEncabezado_790MY
            //
            this.tlpEncabezado_790MY.AutoSize = true;
            this.tlpEncabezado_790MY.ColumnCount = 3;
            this.tlpEncabezado_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpEncabezado_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 210F));
            this.tlpEncabezado_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpEncabezado_790MY.Controls.Add(this.lblTitulo_790MY, 0, 0);
            this.tlpEncabezado_790MY.Controls.Add(this.lblSubtitulo_790MY, 0, 1);
            this.tlpEncabezado_790MY.Controls.Add(this.pnlConfianza_790MY, 1, 0);
            this.tlpEncabezado_790MY.Controls.Add(this.pnlOrden_790MY, 2, 0);
            this.tlpEncabezado_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpEncabezado_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 14);
            this.tlpEncabezado_790MY.Name = "tlpEncabezado_790MY";
            this.tlpEncabezado_790MY.RowCount = 2;
            this.tlpEncabezado_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpEncabezado_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpEncabezado_790MY.SetRowSpan(this.pnlConfianza_790MY, 2);
            this.tlpEncabezado_790MY.SetRowSpan(this.pnlOrden_790MY, 2);
            this.tlpEncabezado_790MY.TabIndex = 0;
            //
            // lblTitulo_790MY
            //
            this.lblTitulo_790MY.AutoSize = true;
            this.lblTitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitulo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblTitulo_790MY.Margin = new System.Windows.Forms.Padding(0, 6, 0, 2);
            this.lblTitulo_790MY.Name = "lblTitulo_790MY";
            this.lblTitulo_790MY.TabIndex = 0;
            this.lblTitulo_790MY.Tag = "RI_titulo_reglas";
            this.lblTitulo_790MY.Text = "Reglas de asociación";
            //
            // lblSubtitulo_790MY
            //
            this.lblSubtitulo_790MY.AutoSize = true;
            this.lblSubtitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblSubtitulo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblSubtitulo_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.lblSubtitulo_790MY.Name = "lblSubtitulo_790MY";
            this.lblSubtitulo_790MY.TabIndex = 1;
            this.lblSubtitulo_790MY.Text = "-";
            //
            // pnlConfianza_790MY
            //
            this.pnlConfianza_790MY.Controls.Add(this.lblConfianza_790MY);
            this.pnlConfianza_790MY.Controls.Add(this.sldConfianza_790MY);
            this.pnlConfianza_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlConfianza_790MY.Margin = new System.Windows.Forms.Padding(12, 0, 16, 0);
            this.pnlConfianza_790MY.Name = "pnlConfianza_790MY";
            this.pnlConfianza_790MY.Size = new System.Drawing.Size(182, 56);
            this.pnlConfianza_790MY.TabIndex = 2;
            //
            // lblConfianza_790MY
            //
            this.lblConfianza_790MY.AutoSize = true;
            this.lblConfianza_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblConfianza_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblConfianza_790MY.Location = new System.Drawing.Point(0, 4);
            this.lblConfianza_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.lblConfianza_790MY.Name = "lblConfianza_790MY";
            this.lblConfianza_790MY.TabIndex = 0;
            this.lblConfianza_790MY.Text = "Confianza mínima: 40 %";
            //
            // sldConfianza_790MY
            //
            this.sldConfianza_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sldConfianza_790MY.Location = new System.Drawing.Point(0, 28);
            this.sldConfianza_790MY.Maximum = 95;
            this.sldConfianza_790MY.Minimum = 10;
            this.sldConfianza_790MY.Name = "sldConfianza_790MY";
            this.sldConfianza_790MY.Size = new System.Drawing.Size(182, 24);
            this.sldConfianza_790MY.Step = 5;
            this.sldConfianza_790MY.TabIndex = 1;
            this.sldConfianza_790MY.Value = 40;
            this.sldConfianza_790MY.ValueChanged += new System.EventHandler(this.sldConfianza_790MY_ValueChanged);
            //
            // pnlOrden_790MY
            //
            this.pnlOrden_790MY.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.pnlOrden_790MY.BackColor = System.Drawing.Color.White;
            this.pnlOrden_790MY.ColorBorde = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(29)))), ((int)(((byte)(47)))));
            this.pnlOrden_790MY.Controls.Add(this.btnPorSoporte_790MY);
            this.pnlOrden_790MY.Controls.Add(this.btnPorConfianza_790MY);
            this.pnlOrden_790MY.GrosorBorde = 1.5F;
            this.pnlOrden_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.pnlOrden_790MY.Name = "pnlOrden_790MY";
            this.pnlOrden_790MY.Padding = new System.Windows.Forms.Padding(3);
            this.pnlOrden_790MY.Radio = 8;
            this.pnlOrden_790MY.Size = new System.Drawing.Size(242, 46);
            this.pnlOrden_790MY.TabIndex = 3;
            //
            // btnPorConfianza_790MY
            //
            this.btnPorConfianza_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPorConfianza_790MY.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnPorConfianza_790MY.FlatAppearance.BorderSize = 0;
            this.btnPorConfianza_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPorConfianza_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnPorConfianza_790MY.Name = "btnPorConfianza_790MY";
            this.btnPorConfianza_790MY.Size = new System.Drawing.Size(118, 40);
            this.btnPorConfianza_790MY.TabIndex = 0;
            this.btnPorConfianza_790MY.Tag = "RI_btn_por_confianza";
            this.btnPorConfianza_790MY.Text = "Por confianza";
            this.btnPorConfianza_790MY.UseVisualStyleBackColor = false;
            this.btnPorConfianza_790MY.Click += new System.EventHandler(this.btnPorConfianza_790MY_Click);
            //
            // btnPorSoporte_790MY
            //
            this.btnPorSoporte_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPorSoporte_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPorSoporte_790MY.FlatAppearance.BorderSize = 0;
            this.btnPorSoporte_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPorSoporte_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnPorSoporte_790MY.Name = "btnPorSoporte_790MY";
            this.btnPorSoporte_790MY.TabIndex = 1;
            this.btnPorSoporte_790MY.Tag = "RI_btn_por_soporte";
            this.btnPorSoporte_790MY.Text = "Por soporte";
            this.btnPorSoporte_790MY.UseVisualStyleBackColor = false;
            this.btnPorSoporte_790MY.Click += new System.EventHandler(this.btnPorSoporte_790MY_Click);
            //
            // dgvReglas_790MY
            //
            this.dgvReglas_790MY.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNumero_790MY,
            this.colCombinacion_790MY,
            this.colSoporte_790MY,
            this.colConfianza_790MY});
            this.dgvReglas_790MY.Controls.Add(this.lblSinReglas_790MY);
            this.dgvReglas_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReglas_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.dgvReglas_790MY.Name = "dgvReglas_790MY";
            this.dgvReglas_790MY.TabIndex = 1;
            this.dgvReglas_790MY.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvReglas_790MY_CellPainting);
            this.dgvReglas_790MY.Resize += new System.EventHandler(this.dgvReglas_790MY_Resize);
            //
            // colNumero_790MY
            //
            this.colNumero_790MY.FillWeight = 6F;
            this.colNumero_790MY.HeaderText = "N°";
            this.colNumero_790MY.Name = "colNumero_790MY";
            //
            // colCombinacion_790MY
            //
            this.colCombinacion_790MY.FillWeight = 40F;
            this.colCombinacion_790MY.HeaderText = "Combinación (si pide → también pide)";
            this.colCombinacion_790MY.Name = "colCombinacion_790MY";
            //
            // colSoporte_790MY
            //
            this.colSoporte_790MY.FillWeight = 27F;
            this.colSoporte_790MY.HeaderText = "Soporte %";
            this.colSoporte_790MY.Name = "colSoporte_790MY";
            //
            // colConfianza_790MY
            //
            this.colConfianza_790MY.FillWeight = 27F;
            this.colConfianza_790MY.HeaderText = "Confianza %";
            this.colConfianza_790MY.Name = "colConfianza_790MY";
            //
            // lblSinReglas_790MY
            //
            this.lblSinReglas_790MY.BackColor = System.Drawing.Color.White;
            this.lblSinReglas_790MY.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.lblSinReglas_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblSinReglas_790MY.Name = "lblSinReglas_790MY";
            this.lblSinReglas_790MY.TabIndex = 0;
            this.lblSinReglas_790MY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSinReglas_790MY.Visible = false;
            //
            // flpTituloSugerencias_790MY
            //
            this.flpTituloSugerencias_790MY.AutoSize = true;
            this.flpTituloSugerencias_790MY.Controls.Add(this.icoSugerencias_790MY);
            this.flpTituloSugerencias_790MY.Controls.Add(this.lblSugerencias_790MY);
            this.flpTituloSugerencias_790MY.Margin = new System.Windows.Forms.Padding(0, 16, 0, 10);
            this.flpTituloSugerencias_790MY.Name = "flpTituloSugerencias_790MY";
            this.flpTituloSugerencias_790MY.TabIndex = 2;
            this.flpTituloSugerencias_790MY.WrapContents = false;
            //
            // icoSugerencias_790MY
            //
            this.icoSugerencias_790MY.BackColor = System.Drawing.Color.Transparent;
            this.icoSugerencias_790MY.IconChar = FontAwesome.Sharp.IconChar.Lightbulb;
            this.icoSugerencias_790MY.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(29)))), ((int)(((byte)(47)))));
            this.icoSugerencias_790MY.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.icoSugerencias_790MY.IconSize = 22;
            this.icoSugerencias_790MY.Margin = new System.Windows.Forms.Padding(0, 2, 8, 0);
            this.icoSugerencias_790MY.Name = "icoSugerencias_790MY";
            this.icoSugerencias_790MY.Size = new System.Drawing.Size(22, 22);
            this.icoSugerencias_790MY.TabIndex = 0;
            this.icoSugerencias_790MY.TabStop = false;
            //
            // lblSugerencias_790MY
            //
            this.lblSugerencias_790MY.AutoSize = true;
            this.lblSugerencias_790MY.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblSugerencias_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblSugerencias_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.lblSugerencias_790MY.Name = "lblSugerencias_790MY";
            this.lblSugerencias_790MY.TabIndex = 1;
            this.lblSugerencias_790MY.Tag = "RI_sugerencias";
            this.lblSugerencias_790MY.Text = "Sugerencias Inteligentes";
            //
            // tlpSugerencias_790MY  (las tarjetas se arman en código)
            //
            this.tlpSugerencias_790MY.ColumnCount = 3;
            this.tlpSugerencias_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpSugerencias_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tlpSugerencias_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tlpSugerencias_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSugerencias_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSugerencias_790MY.Name = "tlpSugerencias_790MY";
            this.tlpSugerencias_790MY.RowCount = 1;
            this.tlpSugerencias_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSugerencias_790MY.TabIndex = 3;
            //
            // bnrAvisos_790MY
            //
            this.bnrAvisos_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.bnrAvisos_790MY.Name = "bnrAvisos_790MY";
            this.bnrAvisos_790MY.TabIndex = 1;
            //
            // FormReportePredictivo_790MY
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(251)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(1400, 820);
            this.Controls.Add(this.tlpMain_790MY);
            this.Controls.Add(this.bnrAvisos_790MY);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormReportePredictivo_790MY";
            this.Tag = "RI_titulo";
            this.Text = "Análisis predictivo de canasta";
            this.Load += new System.EventHandler(this.FormReportePredictivo_790MY_Load);
            this.tlpMain_790MY.ResumeLayout(false);
            this.tlpMain_790MY.PerformLayout();
            this.tlpEncabezado_790MY.ResumeLayout(false);
            this.tlpEncabezado_790MY.PerformLayout();
            this.pnlConfianza_790MY.ResumeLayout(false);
            this.pnlConfianza_790MY.PerformLayout();
            this.pnlOrden_790MY.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReglas_790MY)).EndInit();
            this.flpTituloSugerencias_790MY.ResumeLayout(false);
            this.flpTituloSugerencias_790MY.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.icoSugerencias_790MY)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain_790MY;
        private System.Windows.Forms.TableLayoutPanel tlpEncabezado_790MY;
        private System.Windows.Forms.Label lblTitulo_790MY;
        private System.Windows.Forms.Label lblSubtitulo_790MY;
        private System.Windows.Forms.Panel pnlConfianza_790MY;
        private System.Windows.Forms.Label lblConfianza_790MY;
        private GUI_08YS.Reportes.SliderBordo_790MY sldConfianza_790MY;
        private GUI_08YS.RF2.PanelRedondeado_790MY pnlOrden_790MY;
        private System.Windows.Forms.Button btnPorConfianza_790MY;
        private System.Windows.Forms.Button btnPorSoporte_790MY;
        private System.Windows.Forms.DataGridView dgvReglas_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumero_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCombinacion_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoporte_790MY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colConfianza_790MY;
        private System.Windows.Forms.Label lblSinReglas_790MY;
        private System.Windows.Forms.FlowLayoutPanel flpTituloSugerencias_790MY;
        private FontAwesome.Sharp.IconPictureBox icoSugerencias_790MY;
        private System.Windows.Forms.Label lblSugerencias_790MY;
        private System.Windows.Forms.TableLayoutPanel tlpSugerencias_790MY;
        private GUI_08YS.Estilo.Banner_UC_790MY bnrAvisos_790MY;
    }
}
