namespace GUI_08YS.Estilo
{
    partial class ValorCopiable_790MY
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.tlpValor_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitulo_790MY = new System.Windows.Forms.Label();
            this.lblDetalle_790MY = new System.Windows.Forms.Label();
            this.txtValor_790MY = new System.Windows.Forms.TextBox();
            this.btnCopiar_790MY = new System.Windows.Forms.Button();
            this.tlpValor_790MY.SuspendLayout();
            this.SuspendLayout();
            //
            // tlpValor_790MY  (título y detalle a la izquierda; valor y Copiar a la derecha)
            //
            this.tlpValor_790MY.ColumnCount = 3;
            this.tlpValor_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpValor_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 136F));
            this.tlpValor_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpValor_790MY.Controls.Add(this.lblTitulo_790MY, 0, 0);
            this.tlpValor_790MY.Controls.Add(this.lblDetalle_790MY, 0, 1);
            this.tlpValor_790MY.Controls.Add(this.txtValor_790MY, 1, 0);
            this.tlpValor_790MY.Controls.Add(this.btnCopiar_790MY, 2, 0);
            this.tlpValor_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpValor_790MY.Location = new System.Drawing.Point(0, 0);
            this.tlpValor_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.tlpValor_790MY.Name = "tlpValor_790MY";
            this.tlpValor_790MY.RowCount = 2;
            this.tlpValor_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpValor_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpValor_790MY.SetRowSpan(this.txtValor_790MY, 2);
            this.tlpValor_790MY.SetRowSpan(this.btnCopiar_790MY, 2);
            this.tlpValor_790MY.Size = new System.Drawing.Size(316, 44);
            this.tlpValor_790MY.TabIndex = 0;
            //
            // lblTitulo_790MY
            //
            this.lblTitulo_790MY.AutoEllipsis = true;
            this.lblTitulo_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTitulo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblTitulo_790MY.Location = new System.Drawing.Point(0, 0);
            this.lblTitulo_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.lblTitulo_790MY.Name = "lblTitulo_790MY";
            this.lblTitulo_790MY.Size = new System.Drawing.Size(112, 22);
            this.lblTitulo_790MY.TabIndex = 0;
            this.lblTitulo_790MY.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            //
            // lblDetalle_790MY
            //
            this.lblDetalle_790MY.AutoEllipsis = true;
            this.lblDetalle_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetalle_790MY.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblDetalle_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblDetalle_790MY.Location = new System.Drawing.Point(0, 22);
            this.lblDetalle_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.lblDetalle_790MY.Name = "lblDetalle_790MY";
            this.lblDetalle_790MY.Size = new System.Drawing.Size(112, 22);
            this.lblDetalle_790MY.TabIndex = 1;
            this.lblDetalle_790MY.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            //
            // txtValor_790MY  (solo lectura: se puede seleccionar y copiar con Ctrl+C)
            //
            this.txtValor_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtValor_790MY.BackColor = System.Drawing.Color.White;
            this.txtValor_790MY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtValor_790MY.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtValor_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.txtValor_790MY.Location = new System.Drawing.Point(116, 9);
            this.txtValor_790MY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.txtValor_790MY.Name = "txtValor_790MY";
            this.txtValor_790MY.ReadOnly = true;
            this.txtValor_790MY.Size = new System.Drawing.Size(128, 23);
            this.txtValor_790MY.TabIndex = 2;
            //
            // btnCopiar_790MY  (secundario: contorno bordó)
            //
            this.btnCopiar_790MY.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCopiar_790MY.AutoSize = true;
            this.btnCopiar_790MY.BackColor = System.Drawing.Color.White;
            this.btnCopiar_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCopiar_790MY.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(29)))), ((int)(((byte)(47)))));
            this.btnCopiar_790MY.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(236)))), ((int)(((byte)(238)))));
            this.btnCopiar_790MY.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(236)))), ((int)(((byte)(238)))));
            this.btnCopiar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopiar_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCopiar_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(29)))), ((int)(((byte)(47)))));
            this.btnCopiar_790MY.Location = new System.Drawing.Point(252, 7);
            this.btnCopiar_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.btnCopiar_790MY.MinimumSize = new System.Drawing.Size(64, 30);
            this.btnCopiar_790MY.Name = "btnCopiar_790MY";
            this.btnCopiar_790MY.Size = new System.Drawing.Size(64, 30);
            this.btnCopiar_790MY.TabIndex = 3;
            this.btnCopiar_790MY.Text = "Copiar";
            this.btnCopiar_790MY.UseVisualStyleBackColor = false;
            this.btnCopiar_790MY.Click += new System.EventHandler(this.btnCopiar_790MY_Click);
            //
            // ValorCopiable_790MY
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.Controls.Add(this.tlpValor_790MY);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.Name = "ValorCopiable_790MY";
            this.Size = new System.Drawing.Size(316, 44);
            this.tlpValor_790MY.ResumeLayout(false);
            this.tlpValor_790MY.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpValor_790MY;
        private System.Windows.Forms.Label lblTitulo_790MY;
        private System.Windows.Forms.Label lblDetalle_790MY;
        private System.Windows.Forms.TextBox txtValor_790MY;
        private System.Windows.Forms.Button btnCopiar_790MY;
    }
}
