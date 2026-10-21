namespace GUI_08YS.Estilo
{
    partial class FilaAyuda_790MY
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _imagenAtenuada?.Dispose();
                if (components != null) components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tlpFila_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.picSombrero_790MY = new System.Windows.Forms.PictureBox();
            this.lblTexto_790MY = new System.Windows.Forms.Label();
            this.lblTecla_790MY = new GUI_08YS.Estilo.TeclaEtiqueta_790MY();
            this.ttAyuda_790MY = new System.Windows.Forms.ToolTip(this.components);
            this.tlpFila_790MY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSombrero_790MY)).BeginInit();
            this.SuspendLayout();
            //
            // tlpFila_790MY  (imagen fija | texto Fill | tecla AutoSize): el centrado vertical
            // sobrevive a cualquier escala porque ningún hijo se ancla arriba ni abajo.
            //
            this.tlpFila_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.tlpFila_790MY.ColumnCount = 3;
            this.tlpFila_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tlpFila_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFila_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpFila_790MY.Controls.Add(this.picSombrero_790MY, 0, 0);
            this.tlpFila_790MY.Controls.Add(this.lblTexto_790MY, 1, 0);
            this.tlpFila_790MY.Controls.Add(this.lblTecla_790MY, 2, 0);
            this.tlpFila_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFila_790MY.Location = new System.Drawing.Point(5, 3);
            this.tlpFila_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFila_790MY.Name = "tlpFila_790MY";
            this.tlpFila_790MY.RowCount = 1;
            this.tlpFila_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFila_790MY.Size = new System.Drawing.Size(224, 43);
            this.tlpFila_790MY.TabIndex = 0;
            this.tlpFila_790MY.Click += new System.EventHandler(this.Hijo_Click);
            this.tlpFila_790MY.MouseEnter += new System.EventHandler(this.Hijo_MouseEnter);
            this.tlpFila_790MY.MouseLeave += new System.EventHandler(this.Hijo_MouseLeave);
            this.tlpFila_790MY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Hijo_MouseDown);
            this.tlpFila_790MY.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Hijo_MouseUp);
            //
            // picSombrero_790MY  (PNG con transparencia real; el tamaño se elige por escala)
            //
            this.picSombrero_790MY.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picSombrero_790MY.BackColor = System.Drawing.Color.Transparent;
            this.picSombrero_790MY.Image = global::GUI_08YS.Properties.Resources.SombreroAyuda32;
            this.picSombrero_790MY.Location = new System.Drawing.Point(0, 5);
            this.picSombrero_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.picSombrero_790MY.Name = "picSombrero_790MY";
            this.picSombrero_790MY.Size = new System.Drawing.Size(32, 32);
            this.picSombrero_790MY.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSombrero_790MY.TabIndex = 0;
            this.picSombrero_790MY.TabStop = false;
            this.picSombrero_790MY.Click += new System.EventHandler(this.Hijo_Click);
            this.picSombrero_790MY.MouseEnter += new System.EventHandler(this.Hijo_MouseEnter);
            this.picSombrero_790MY.MouseLeave += new System.EventHandler(this.Hijo_MouseLeave);
            this.picSombrero_790MY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Hijo_MouseDown);
            this.picSombrero_790MY.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Hijo_MouseUp);
            //
            // lblTexto_790MY
            //
            this.lblTexto_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTexto_790MY.AutoSize = true;
            this.lblTexto_790MY.BackColor = System.Drawing.Color.Transparent;
            this.lblTexto_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTexto_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(195)))), ((int)(((byte)(140)))));
            this.lblTexto_790MY.Location = new System.Drawing.Point(34, 15);
            this.lblTexto_790MY.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.lblTexto_790MY.Name = "lblTexto_790MY";
            this.lblTexto_790MY.Size = new System.Drawing.Size(39, 13);
            this.lblTexto_790MY.TabIndex = 1;
            this.lblTexto_790MY.Text = "Ayuda";
            this.lblTexto_790MY.Click += new System.EventHandler(this.Hijo_Click);
            this.lblTexto_790MY.MouseEnter += new System.EventHandler(this.Hijo_MouseEnter);
            this.lblTexto_790MY.MouseLeave += new System.EventHandler(this.Hijo_MouseLeave);
            this.lblTexto_790MY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Hijo_MouseDown);
            this.lblTexto_790MY.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Hijo_MouseUp);
            //
            // lblTecla_790MY  («F1» dibujada como tecla física en TeclaEtiqueta_790MY.OnPaint)
            //
            this.lblTecla_790MY.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTecla_790MY.AutoSize = true;
            this.lblTecla_790MY.BackColor = System.Drawing.Color.Transparent;
            this.lblTecla_790MY.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTecla_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(195)))), ((int)(((byte)(140)))));
            this.lblTecla_790MY.Location = new System.Drawing.Point(199, 13);
            this.lblTecla_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.lblTecla_790MY.Name = "lblTecla_790MY";
            this.lblTecla_790MY.Padding = new System.Windows.Forms.Padding(5, 3, 4, 3);
            this.lblTecla_790MY.Size = new System.Drawing.Size(25, 18);
            this.lblTecla_790MY.TabIndex = 2;
            this.lblTecla_790MY.Text = "F1";
            this.lblTecla_790MY.Click += new System.EventHandler(this.Hijo_Click);
            this.lblTecla_790MY.MouseEnter += new System.EventHandler(this.Hijo_MouseEnter);
            this.lblTecla_790MY.MouseLeave += new System.EventHandler(this.Hijo_MouseLeave);
            this.lblTecla_790MY.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Hijo_MouseDown);
            this.lblTecla_790MY.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Hijo_MouseUp);
            //
            // FilaAyuda_790MY  (misma geometría que los IconButton de la barra: 49 px,
            // borde dorado de 1 px y Padding(4,0,15,0) + 1 px de borde a la izquierda)
            //
            this.AccessibleName = "Ayuda (F1)";
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.Controls.Add(this.tlpFila_790MY);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(195)))), ((int)(((byte)(140)))));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "FilaAyuda_790MY";
            this.Padding = new System.Windows.Forms.Padding(5, 3, 15, 3);
            this.Size = new System.Drawing.Size(244, 49);
            this.TabStop = true;
            this.tlpFila_790MY.ResumeLayout(false);
            this.tlpFila_790MY.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSombrero_790MY)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpFila_790MY;
        private System.Windows.Forms.PictureBox picSombrero_790MY;
        private System.Windows.Forms.Label lblTexto_790MY;
        private GUI_08YS.Estilo.TeclaEtiqueta_790MY lblTecla_790MY;
        private System.Windows.Forms.ToolTip ttAyuda_790MY;
    }
}
