namespace GUI_08YS.Estilo
{
    partial class Banner_UC_790MY
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.tlpBanner_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.lblIcono_790MY = new System.Windows.Forms.Label();
            this.lblMensaje_790MY = new System.Windows.Forms.Label();
            this.btnCerrar_790MY = new System.Windows.Forms.Button();
            this.tlpBanner_790MY.SuspendLayout();
            this.SuspendLayout();
            //
            // tlpBanner_790MY
            //
            this.tlpBanner_790MY.AutoSize = true;
            this.tlpBanner_790MY.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpBanner_790MY.ColumnCount = 3;
            this.tlpBanner_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpBanner_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBanner_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpBanner_790MY.Controls.Add(this.lblIcono_790MY, 0, 0);
            this.tlpBanner_790MY.Controls.Add(this.lblMensaje_790MY, 1, 0);
            this.tlpBanner_790MY.Controls.Add(this.btnCerrar_790MY, 2, 0);
            this.tlpBanner_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpBanner_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.tlpBanner_790MY.Name = "tlpBanner_790MY";
            this.tlpBanner_790MY.RowCount = 1;
            this.tlpBanner_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBanner_790MY.TabIndex = 0;
            this.tlpBanner_790MY.SizeChanged += new System.EventHandler(this.tlpBanner_790MY_SizeChanged);
            //
            // lblIcono_790MY
            //
            this.lblIcono_790MY.AutoSize = true;
            this.lblIcono_790MY.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblIcono_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 8, 0);
            this.lblIcono_790MY.Name = "lblIcono_790MY";
            this.lblIcono_790MY.TabIndex = 0;
            this.lblIcono_790MY.Text = "⚠";
            //
            // lblMensaje_790MY
            //
            this.lblMensaje_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMensaje_790MY.AutoSize = true;
            this.lblMensaje_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblMensaje_790MY.Margin = new System.Windows.Forms.Padding(0, 2, 8, 0);
            this.lblMensaje_790MY.Name = "lblMensaje_790MY";
            this.lblMensaje_790MY.TabIndex = 1;
            //
            // btnCerrar_790MY
            //
            this.btnCerrar_790MY.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnCerrar_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrar_790MY.FlatAppearance.BorderSize = 0;
            this.btnCerrar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCerrar_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.btnCerrar_790MY.Name = "btnCerrar_790MY";
            this.btnCerrar_790MY.Size = new System.Drawing.Size(28, 24);
            this.btnCerrar_790MY.TabIndex = 2;
            this.btnCerrar_790MY.Text = "✕";
            this.btnCerrar_790MY.UseVisualStyleBackColor = false;
            this.btnCerrar_790MY.Click += new System.EventHandler(this.btnCerrar_790MY_Click);
            //
            // Banner_UC_790MY
            //
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Alert;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            this.Controls.Add(this.tlpBanner_790MY);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "Banner_UC_790MY";
            this.Padding = new System.Windows.Forms.Padding(16, 10, 12, 10);
            this.Size = new System.Drawing.Size(600, 44);
            this.Visible = false;
            this.tlpBanner_790MY.ResumeLayout(false);
            this.tlpBanner_790MY.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpBanner_790MY;
        private System.Windows.Forms.Label lblIcono_790MY;
        private System.Windows.Forms.Label lblMensaje_790MY;
        private System.Windows.Forms.Button btnCerrar_790MY;
    }
}
