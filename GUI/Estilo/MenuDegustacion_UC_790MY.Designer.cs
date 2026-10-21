namespace GUI_08YS.Estilo
{
    partial class MenuDegustacion_UC_790MY
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
            this.bnrDegustacion_790MY = new GUI_08YS.Estilo.Banner_UC_790MY();
            this.flpPasos_790MY = new System.Windows.Forms.FlowLayoutPanel();
            this.lblIntro_790MY = new System.Windows.Forms.Label();
            this.paso1_790MY = new GUI_08YS.Estilo.PasoDegustacion_790MY();
            this.paso2_790MY = new GUI_08YS.Estilo.PasoDegustacion_790MY();
            this.paso3_790MY = new GUI_08YS.Estilo.PasoDegustacion_790MY();
            this.paso4_790MY = new GUI_08YS.Estilo.PasoDegustacion_790MY();
            this.paso5_790MY = new GUI_08YS.Estilo.PasoDegustacion_790MY();
            this.paso6_790MY = new GUI_08YS.Estilo.PasoDegustacion_790MY();
            this.flpPasos_790MY.SuspendLayout();
            this.SuspendLayout();
            //
            // flpPasos_790MY
            //
            this.flpPasos_790MY.AutoScroll = true;
            this.flpPasos_790MY.Controls.Add(this.lblIntro_790MY);
            this.flpPasos_790MY.Controls.Add(this.paso1_790MY);
            this.flpPasos_790MY.Controls.Add(this.paso2_790MY);
            this.flpPasos_790MY.Controls.Add(this.paso3_790MY);
            this.flpPasos_790MY.Controls.Add(this.paso4_790MY);
            this.flpPasos_790MY.Controls.Add(this.paso5_790MY);
            this.flpPasos_790MY.Controls.Add(this.paso6_790MY);
            this.flpPasos_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpPasos_790MY.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpPasos_790MY.Location = new System.Drawing.Point(0, 0);
            this.flpPasos_790MY.Name = "flpPasos_790MY";
            this.flpPasos_790MY.Padding = new System.Windows.Forms.Padding(12, 4, 0, 16);
            this.flpPasos_790MY.Size = new System.Drawing.Size(319, 592);
            this.flpPasos_790MY.TabIndex = 0;
            this.flpPasos_790MY.WrapContents = false;
            //
            // lblIntro_790MY
            //
            this.lblIntro_790MY.AutoSize = true;
            this.lblIntro_790MY.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblIntro_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblIntro_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.lblIntro_790MY.MaximumSize = new System.Drawing.Size(280, 0);
            this.lblIntro_790MY.Name = "lblIntro_790MY";
            this.lblIntro_790MY.TabIndex = 0;
            //
            // paso1_790MY
            //
            this.paso1_790MY.Name = "paso1_790MY";
            this.paso1_790MY.TabIndex = 1;
            this.paso1_790MY.IrSolicitado += new System.EventHandler(this.Paso_IrSolicitado);
            this.paso1_790MY.RecetaSolicitada += new System.EventHandler(this.Paso_RecetaSolicitada);
            //
            // paso2_790MY
            //
            this.paso2_790MY.Name = "paso2_790MY";
            this.paso2_790MY.TabIndex = 2;
            this.paso2_790MY.IrSolicitado += new System.EventHandler(this.Paso_IrSolicitado);
            this.paso2_790MY.RecetaSolicitada += new System.EventHandler(this.Paso_RecetaSolicitada);
            //
            // paso3_790MY
            //
            this.paso3_790MY.Name = "paso3_790MY";
            this.paso3_790MY.TabIndex = 3;
            this.paso3_790MY.IrSolicitado += new System.EventHandler(this.Paso_IrSolicitado);
            this.paso3_790MY.RecetaSolicitada += new System.EventHandler(this.Paso_RecetaSolicitada);
            //
            // paso4_790MY
            //
            this.paso4_790MY.Name = "paso4_790MY";
            this.paso4_790MY.TabIndex = 4;
            this.paso4_790MY.IrSolicitado += new System.EventHandler(this.Paso_IrSolicitado);
            this.paso4_790MY.RecetaSolicitada += new System.EventHandler(this.Paso_RecetaSolicitada);
            //
            // paso5_790MY
            //
            this.paso5_790MY.Name = "paso5_790MY";
            this.paso5_790MY.TabIndex = 5;
            this.paso5_790MY.IrSolicitado += new System.EventHandler(this.Paso_IrSolicitado);
            this.paso5_790MY.RecetaSolicitada += new System.EventHandler(this.Paso_RecetaSolicitada);
            //
            // paso6_790MY
            //
            this.paso6_790MY.Name = "paso6_790MY";
            this.paso6_790MY.TabIndex = 6;
            this.paso6_790MY.IrSolicitado += new System.EventHandler(this.Paso_IrSolicitado);
            this.paso6_790MY.RecetaSolicitada += new System.EventHandler(this.Paso_RecetaSolicitada);
            //
            // bnrDegustacion_790MY  (errores al abrir una receta: nunca MessageBox)
            //
            this.bnrDegustacion_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.bnrDegustacion_790MY.Name = "bnrDegustacion_790MY";
            this.bnrDegustacion_790MY.TabIndex = 1;
            this.bnrDegustacion_790MY.Visible = false;
            //
            // MenuDegustacion_UC_790MY
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.Controls.Add(this.flpPasos_790MY);
            this.Controls.Add(this.bnrDegustacion_790MY);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "MenuDegustacion_UC_790MY";
            this.Size = new System.Drawing.Size(319, 592);
            this.flpPasos_790MY.ResumeLayout(false);
            this.flpPasos_790MY.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flpPasos_790MY;
        private GUI_08YS.Estilo.Banner_UC_790MY bnrDegustacion_790MY;
        private System.Windows.Forms.Label lblIntro_790MY;
        private GUI_08YS.Estilo.PasoDegustacion_790MY paso1_790MY;
        private GUI_08YS.Estilo.PasoDegustacion_790MY paso2_790MY;
        private GUI_08YS.Estilo.PasoDegustacion_790MY paso3_790MY;
        private GUI_08YS.Estilo.PasoDegustacion_790MY paso4_790MY;
        private GUI_08YS.Estilo.PasoDegustacion_790MY paso5_790MY;
        private GUI_08YS.Estilo.PasoDegustacion_790MY paso6_790MY;
    }
}
