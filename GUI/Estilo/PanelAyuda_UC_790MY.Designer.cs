namespace GUI_08YS.Estilo
{
    partial class PanelAyuda_UC_790MY
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
            this.pnlEncabezado_790MY = new System.Windows.Forms.Panel();
            this.lblTitulo_790MY = new System.Windows.Forms.Label();
            this.btnCerrar_790MY = new System.Windows.Forms.Button();
            this.tabAyuda_790MY = new GUI_08YS.Estilo.SlidingTab_UC_790MY();
            this.pnlTabs_790MY = new System.Windows.Forms.Panel();
            this.pnlContenido_790MY = new System.Windows.Forms.Panel();
            this.ucFicha_790MY = new GUI_08YS.Estilo.FichaIngredientes_UC_790MY();
            this.ucDegustacion_790MY = new GUI_08YS.Estilo.MenuDegustacion_UC_790MY();
            this.pnlEncabezado_790MY.SuspendLayout();
            this.pnlTabs_790MY.SuspendLayout();
            this.pnlContenido_790MY.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlEncabezado_790MY
            //
            this.pnlEncabezado_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(240)))), ((int)(((byte)(233)))));
            this.pnlEncabezado_790MY.Controls.Add(this.lblTitulo_790MY);
            this.pnlEncabezado_790MY.Controls.Add(this.btnCerrar_790MY);
            this.pnlEncabezado_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEncabezado_790MY.Location = new System.Drawing.Point(1, 0);
            this.pnlEncabezado_790MY.Name = "pnlEncabezado_790MY";
            this.pnlEncabezado_790MY.Padding = new System.Windows.Forms.Padding(16, 8, 8, 8);
            this.pnlEncabezado_790MY.Size = new System.Drawing.Size(359, 52);
            this.pnlEncabezado_790MY.TabIndex = 0;
            //
            // lblTitulo_790MY
            //
            this.lblTitulo_790MY.AutoEllipsis = true;
            this.lblTitulo_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitulo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblTitulo_790MY.Location = new System.Drawing.Point(16, 8);
            this.lblTitulo_790MY.Name = "lblTitulo_790MY";
            this.lblTitulo_790MY.Size = new System.Drawing.Size(299, 36);
            this.lblTitulo_790MY.TabIndex = 0;
            this.lblTitulo_790MY.Text = "Ayuda del Chef";
            this.lblTitulo_790MY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // btnCerrar_790MY
            //
            this.btnCerrar_790MY.AccessibleName = "Cerrar ayuda";
            this.btnCerrar_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrar_790MY.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCerrar_790MY.FlatAppearance.BorderSize = 0;
            this.btnCerrar_790MY.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(236)))), ((int)(((byte)(238)))));
            this.btnCerrar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar_790MY.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnCerrar_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(29)))), ((int)(((byte)(47)))));
            this.btnCerrar_790MY.Location = new System.Drawing.Point(315, 8);
            this.btnCerrar_790MY.Name = "btnCerrar_790MY";
            this.btnCerrar_790MY.Size = new System.Drawing.Size(36, 36);
            this.btnCerrar_790MY.TabIndex = 1;
            this.btnCerrar_790MY.Text = "✕";
            this.btnCerrar_790MY.UseVisualStyleBackColor = false;
            this.btnCerrar_790MY.Click += new System.EventHandler(this.btnCerrar_790MY_Click);
            //
            // pnlTabs_790MY
            //
            this.pnlTabs_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(240)))), ((int)(((byte)(233)))));
            this.pnlTabs_790MY.Controls.Add(this.tabAyuda_790MY);
            this.pnlTabs_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTabs_790MY.Location = new System.Drawing.Point(1, 52);
            this.pnlTabs_790MY.Name = "pnlTabs_790MY";
            this.pnlTabs_790MY.Padding = new System.Windows.Forms.Padding(12, 0, 12, 12);
            this.pnlTabs_790MY.Size = new System.Drawing.Size(359, 56);
            this.pnlTabs_790MY.TabIndex = 1;
            //
            // tabAyuda_790MY
            //
            this.tabAyuda_790MY.ClavesIdioma = new string[] {
        "AY_tab_ficha",
        "AY_tab_degustacion"};
            this.tabAyuda_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabAyuda_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.tabAyuda_790MY.Location = new System.Drawing.Point(12, 0);
            this.tabAyuda_790MY.MinimumSize = new System.Drawing.Size(120, 40);
            this.tabAyuda_790MY.Name = "tabAyuda_790MY";
            this.tabAyuda_790MY.Size = new System.Drawing.Size(335, 44);
            this.tabAyuda_790MY.TabIndex = 0;
            this.tabAyuda_790MY.SeleccionCambiada += new System.EventHandler(this.tabAyuda_790MY_SeleccionCambiada);
            //
            // pnlContenido_790MY
            //
            this.pnlContenido_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.pnlContenido_790MY.Controls.Add(this.ucFicha_790MY);
            this.pnlContenido_790MY.Controls.Add(this.ucDegustacion_790MY);
            this.pnlContenido_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContenido_790MY.Location = new System.Drawing.Point(1, 108);
            this.pnlContenido_790MY.Name = "pnlContenido_790MY";
            this.pnlContenido_790MY.Size = new System.Drawing.Size(359, 592);
            this.pnlContenido_790MY.TabIndex = 2;
            //
            // ucFicha_790MY
            //
            this.ucFicha_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucFicha_790MY.Location = new System.Drawing.Point(0, 0);
            this.ucFicha_790MY.Name = "ucFicha_790MY";
            this.ucFicha_790MY.Size = new System.Drawing.Size(359, 592);
            this.ucFicha_790MY.TabIndex = 0;
            //
            // ucDegustacion_790MY
            //
            this.ucDegustacion_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucDegustacion_790MY.Location = new System.Drawing.Point(0, 0);
            this.ucDegustacion_790MY.Name = "ucDegustacion_790MY";
            this.ucDegustacion_790MY.Size = new System.Drawing.Size(359, 592);
            this.ucDegustacion_790MY.TabIndex = 1;
            this.ucDegustacion_790MY.Visible = false;
            this.ucDegustacion_790MY.NavegacionSolicitada += new System.EventHandler<GUI_08YS.Estilo.DestinoAyudaEventArgs_790MY>(this.ucDegustacion_790MY_NavegacionSolicitada);
            //
            // PanelAyuda_UC_790MY  (borde izquierdo de 1 px en OnPaint, sobre el Padding)
            //
            this.AccessibleName = "Ayuda del Chef";
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Pane;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.Controls.Add(this.pnlContenido_790MY);
            this.Controls.Add(this.pnlTabs_790MY);
            this.Controls.Add(this.pnlEncabezado_790MY);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "PanelAyuda_UC_790MY";
            this.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.Size = new System.Drawing.Size(360, 700);
            this.pnlEncabezado_790MY.ResumeLayout(false);
            this.pnlTabs_790MY.ResumeLayout(false);
            this.pnlContenido_790MY.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlEncabezado_790MY;
        private System.Windows.Forms.Label lblTitulo_790MY;
        private System.Windows.Forms.Button btnCerrar_790MY;
        private System.Windows.Forms.Panel pnlTabs_790MY;
        private GUI_08YS.Estilo.SlidingTab_UC_790MY tabAyuda_790MY;
        private System.Windows.Forms.Panel pnlContenido_790MY;
        private GUI_08YS.Estilo.FichaIngredientes_UC_790MY ucFicha_790MY;
        private GUI_08YS.Estilo.MenuDegustacion_UC_790MY ucDegustacion_790MY;
    }
}
