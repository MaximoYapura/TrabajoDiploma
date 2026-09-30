namespace GUI_08YS.RF1
{
    partial class FormComprobanteReserva_790MY
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
            this.components = new System.ComponentModel.Container();
            this.pnlBanner_790MY = new System.Windows.Forms.Panel();
            this.lblEstadoEmail_790MY = new System.Windows.Forms.Label();
            this.lblBannerExito_790MY = new System.Windows.Forms.Label();
            this.tlpHost_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTicket_790MY = new GUI_08YS.RF1.TicketPreviewPanel_790MY();
            this.pnlFooter_790MY = new System.Windows.Forms.Panel();
            this.btnReenviar_790MY = new System.Windows.Forms.Button();
            this.btnGuardarPdf_790MY = new System.Windows.Forms.Button();
            this.btnCerrar_790MY = new System.Windows.Forms.Button();
            this.tipEstado_790MY = new System.Windows.Forms.ToolTip(this.components);
            this.pnlBanner_790MY.SuspendLayout();
            this.tlpHost_790MY.SuspendLayout();
            this.pnlFooter_790MY.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlBanner_790MY
            //
            this.pnlBanner_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(242)))), ((int)(((byte)(230)))));
            this.pnlBanner_790MY.Controls.Add(this.lblEstadoEmail_790MY);
            this.pnlBanner_790MY.Controls.Add(this.lblBannerExito_790MY);
            this.pnlBanner_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBanner_790MY.Location = new System.Drawing.Point(0, 0);
            this.pnlBanner_790MY.Name = "pnlBanner_790MY";
            this.pnlBanner_790MY.Size = new System.Drawing.Size(520, 56);
            this.pnlBanner_790MY.TabIndex = 2;
            //
            // lblEstadoEmail_790MY
            //
            this.lblEstadoEmail_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEstadoEmail_790MY.AutoEllipsis = true;
            this.lblEstadoEmail_790MY.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblEstadoEmail_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblEstadoEmail_790MY.Location = new System.Drawing.Point(19, 31);
            this.lblEstadoEmail_790MY.Name = "lblEstadoEmail_790MY";
            this.lblEstadoEmail_790MY.Size = new System.Drawing.Size(484, 18);
            this.lblEstadoEmail_790MY.TabIndex = 1;
            //
            // lblBannerExito_790MY
            //
            this.lblBannerExito_790MY.AutoSize = true;
            this.lblBannerExito_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.lblBannerExito_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(110)))), ((int)(((byte)(60)))));
            this.lblBannerExito_790MY.Location = new System.Drawing.Point(17, 9);
            this.lblBannerExito_790MY.Name = "lblBannerExito_790MY";
            this.lblBannerExito_790MY.Size = new System.Drawing.Size(290, 19);
            this.lblBannerExito_790MY.TabIndex = 0;
            this.lblBannerExito_790MY.Text = "✓  Reserva registrada correctamente";
            //
            // tlpHost_790MY
            //
            this.tlpHost_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(230)))), ((int)(((byte)(222)))));
            this.tlpHost_790MY.ColumnCount = 1;
            this.tlpHost_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHost_790MY.Controls.Add(this.pnlTicket_790MY, 0, 0);
            this.tlpHost_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpHost_790MY.Location = new System.Drawing.Point(0, 56);
            this.tlpHost_790MY.Name = "tlpHost_790MY";
            this.tlpHost_790MY.RowCount = 1;
            this.tlpHost_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHost_790MY.Size = new System.Drawing.Size(520, 553);
            this.tlpHost_790MY.TabIndex = 0;
            //
            // pnlTicket_790MY
            //
            this.pnlTicket_790MY.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlTicket_790MY.BackColor = System.Drawing.Color.White;
            this.pnlTicket_790MY.Location = new System.Drawing.Point(70, 8);
            this.pnlTicket_790MY.Name = "pnlTicket_790MY";
            this.pnlTicket_790MY.Size = new System.Drawing.Size(380, 537);
            this.pnlTicket_790MY.TabIndex = 0;
            this.pnlTicket_790MY.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlTicket_790MY_Paint);
            //
            // pnlFooter_790MY
            //
            this.pnlFooter_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.pnlFooter_790MY.Controls.Add(this.btnReenviar_790MY);
            this.pnlFooter_790MY.Controls.Add(this.btnGuardarPdf_790MY);
            this.pnlFooter_790MY.Controls.Add(this.btnCerrar_790MY);
            this.pnlFooter_790MY.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter_790MY.Location = new System.Drawing.Point(0, 609);
            this.pnlFooter_790MY.Name = "pnlFooter_790MY";
            this.pnlFooter_790MY.Size = new System.Drawing.Size(520, 60);
            this.pnlFooter_790MY.TabIndex = 1;
            this.pnlFooter_790MY.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlFooter_790MY_Paint);
            //
            // btnReenviar_790MY
            //
            this.btnReenviar_790MY.BackColor = System.Drawing.Color.White;
            this.btnReenviar_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReenviar_790MY.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnReenviar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReenviar_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnReenviar_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnReenviar_790MY.Location = new System.Drawing.Point(16, 12);
            this.btnReenviar_790MY.Name = "btnReenviar_790MY";
            this.btnReenviar_790MY.Size = new System.Drawing.Size(150, 36);
            this.btnReenviar_790MY.TabIndex = 2;
            this.btnReenviar_790MY.Tag = "CR_btnReenviar";
            this.btnReenviar_790MY.Text = "Reintentar envío";
            this.btnReenviar_790MY.UseVisualStyleBackColor = false;
            this.btnReenviar_790MY.Visible = false;
            this.btnReenviar_790MY.Click += new System.EventHandler(this.btnReenviar_790MY_Click);
            //
            // btnGuardarPdf_790MY
            //
            this.btnGuardarPdf_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGuardarPdf_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnGuardarPdf_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGuardarPdf_790MY.FlatAppearance.BorderSize = 0;
            this.btnGuardarPdf_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardarPdf_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnGuardarPdf_790MY.ForeColor = System.Drawing.Color.White;
            this.btnGuardarPdf_790MY.Location = new System.Drawing.Point(234, 12);
            this.btnGuardarPdf_790MY.Name = "btnGuardarPdf_790MY";
            this.btnGuardarPdf_790MY.Size = new System.Drawing.Size(150, 36);
            this.btnGuardarPdf_790MY.TabIndex = 0;
            this.btnGuardarPdf_790MY.Tag = "CR_btnGuardarPdf";
            this.btnGuardarPdf_790MY.Text = "Guardar PDF";
            this.btnGuardarPdf_790MY.UseVisualStyleBackColor = false;
            this.btnGuardarPdf_790MY.Click += new System.EventHandler(this.btnGuardarPdf_790MY_Click);
            //
            // btnCerrar_790MY
            //
            this.btnCerrar_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar_790MY.BackColor = System.Drawing.Color.White;
            this.btnCerrar_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrar_790MY.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCerrar_790MY.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnCerrar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnCerrar_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnCerrar_790MY.Location = new System.Drawing.Point(392, 12);
            this.btnCerrar_790MY.Name = "btnCerrar_790MY";
            this.btnCerrar_790MY.Size = new System.Drawing.Size(112, 36);
            this.btnCerrar_790MY.TabIndex = 1;
            this.btnCerrar_790MY.Tag = "CR_btnCerrar";
            this.btnCerrar_790MY.Text = "Cerrar";
            this.btnCerrar_790MY.UseVisualStyleBackColor = false;
            this.btnCerrar_790MY.Click += new System.EventHandler(this.btnCerrar_790MY_Click);
            //
            // FormComprobanteReserva_790MY
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.CancelButton = this.btnCerrar_790MY;
            this.ClientSize = new System.Drawing.Size(520, 669);
            this.Controls.Add(this.tlpHost_790MY);
            this.Controls.Add(this.pnlFooter_790MY);
            this.Controls.Add(this.pnlBanner_790MY);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormComprobanteReserva_790MY";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Comprobante de Reserva";
            this.Load += new System.EventHandler(this.FormComprobanteReserva_790MY_Load);
            this.Shown += new System.EventHandler(this.FormComprobanteReserva_790MY_Shown);
            this.pnlBanner_790MY.ResumeLayout(false);
            this.pnlBanner_790MY.PerformLayout();
            this.tlpHost_790MY.ResumeLayout(false);
            this.pnlFooter_790MY.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBanner_790MY;
        private System.Windows.Forms.Label lblBannerExito_790MY;
        private System.Windows.Forms.Label lblEstadoEmail_790MY;
        private System.Windows.Forms.TableLayoutPanel tlpHost_790MY;
        private GUI_08YS.RF1.TicketPreviewPanel_790MY pnlTicket_790MY;
        private System.Windows.Forms.Panel pnlFooter_790MY;
        private System.Windows.Forms.Button btnReenviar_790MY;
        private System.Windows.Forms.Button btnGuardarPdf_790MY;
        private System.Windows.Forms.Button btnCerrar_790MY;
        private System.Windows.Forms.ToolTip tipEstado_790MY;
    }
}
