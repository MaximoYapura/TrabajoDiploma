namespace GUI_08YS.RF1
{
    partial class FormRegistrarCliente_790MY
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
            this.pnlHeader_790MY = new System.Windows.Forms.Panel();
            this.lblSubtitulo_790MY = new System.Windows.Forms.Label();
            this.lblTitulo_790MY = new System.Windows.Forms.Label();
            this.pnlFooter_790MY = new System.Windows.Forms.Panel();
            this.flpBotones_790MY = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancelar_790MY = new System.Windows.Forms.Button();
            this.btnAplicar_790MY = new System.Windows.Forms.Button();
            this.pnlCuerpo_790MY = new System.Windows.Forms.Panel();
            this.grpDatos_790MY = new System.Windows.Forms.GroupBox();
            this.tlpCampos_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.lblDni_790MY = new System.Windows.Forms.Label();
            this.txtDni_790MY = new System.Windows.Forms.TextBox();
            this.lblApellidos_790MY = new System.Windows.Forms.Label();
            this.txtApellidos_790MY = new System.Windows.Forms.TextBox();
            this.lblNombres_790MY = new System.Windows.Forms.Label();
            this.txtNombres_790MY = new System.Windows.Forms.TextBox();
            this.lblEmail_790MY = new System.Windows.Forms.Label();
            this.txtEmail_790MY = new System.Windows.Forms.TextBox();
            this.btnEmailsAdicionales_790MY = new System.Windows.Forms.Button();
            this.lblCelular_790MY = new System.Windows.Forms.Label();
            this.txtCelular_790MY = new System.Windows.Forms.TextBox();
            this.btnTelefonosAdicionales_790MY = new System.Windows.Forms.Button();
            this.lblDireccion_790MY = new System.Windows.Forms.Label();
            this.txtDireccion_790MY = new System.Windows.Forms.TextBox();
            this.tipAyuda_790MY = new System.Windows.Forms.ToolTip(this.components);
            this.pnlHeader_790MY.SuspendLayout();
            this.pnlFooter_790MY.SuspendLayout();
            this.flpBotones_790MY.SuspendLayout();
            this.pnlCuerpo_790MY.SuspendLayout();
            this.grpDatos_790MY.SuspendLayout();
            this.tlpCampos_790MY.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlHeader_790MY
            //
            this.pnlHeader_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.pnlHeader_790MY.Controls.Add(this.lblSubtitulo_790MY);
            this.pnlHeader_790MY.Controls.Add(this.lblTitulo_790MY);
            this.pnlHeader_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader_790MY.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader_790MY.Name = "pnlHeader_790MY";
            this.pnlHeader_790MY.Size = new System.Drawing.Size(504, 56);
            this.pnlHeader_790MY.TabIndex = 2;
            //
            // lblSubtitulo_790MY
            //
            this.lblSubtitulo_790MY.AutoSize = true;
            this.lblSubtitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSubtitulo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(195)))), ((int)(((byte)(140)))));
            this.lblSubtitulo_790MY.Location = new System.Drawing.Point(19, 32);
            this.lblSubtitulo_790MY.Name = "lblSubtitulo_790MY";
            this.lblSubtitulo_790MY.Size = new System.Drawing.Size(290, 15);
            this.lblSubtitulo_790MY.TabIndex = 1;
            this.lblSubtitulo_790MY.Tag = "RC_subtitulo";
            this.lblSubtitulo_790MY.Text = "Completá los datos para dar de alta al nuevo cliente.";
            //
            // lblTitulo_790MY
            //
            this.lblTitulo_790MY.AutoSize = true;
            this.lblTitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitulo_790MY.ForeColor = System.Drawing.Color.White;
            this.lblTitulo_790MY.Location = new System.Drawing.Point(17, 8);
            this.lblTitulo_790MY.Name = "lblTitulo_790MY";
            this.lblTitulo_790MY.Size = new System.Drawing.Size(145, 25);
            this.lblTitulo_790MY.TabIndex = 0;
            this.lblTitulo_790MY.Tag = "RC_titulo";
            this.lblTitulo_790MY.Text = "Registrar Cliente";
            //
            // pnlFooter_790MY
            //
            this.pnlFooter_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(238)))), ((int)(((byte)(231)))));
            this.pnlFooter_790MY.Controls.Add(this.flpBotones_790MY);
            this.pnlFooter_790MY.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter_790MY.Location = new System.Drawing.Point(0, 327);
            this.pnlFooter_790MY.Name = "pnlFooter_790MY";
            this.pnlFooter_790MY.Padding = new System.Windows.Forms.Padding(0, 9, 16, 9);
            this.pnlFooter_790MY.Size = new System.Drawing.Size(504, 54);
            this.pnlFooter_790MY.TabIndex = 1;
            this.pnlFooter_790MY.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlFooter_790MY_Paint);
            //
            // flpBotones_790MY
            //
            this.flpBotones_790MY.AutoSize = true;
            this.flpBotones_790MY.Controls.Add(this.btnCancelar_790MY);
            this.flpBotones_790MY.Controls.Add(this.btnAplicar_790MY);
            this.flpBotones_790MY.Dock = System.Windows.Forms.DockStyle.Right;
            this.flpBotones_790MY.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpBotones_790MY.Location = new System.Drawing.Point(248, 9);
            this.flpBotones_790MY.Name = "flpBotones_790MY";
            this.flpBotones_790MY.Size = new System.Drawing.Size(240, 36);
            this.flpBotones_790MY.TabIndex = 0;
            this.flpBotones_790MY.WrapContents = false;
            //
            // btnCancelar_790MY
            //
            this.btnCancelar_790MY.BackColor = System.Drawing.Color.White;
            this.btnCancelar_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar_790MY.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar_790MY.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnCancelar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.btnCancelar_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnCancelar_790MY.Location = new System.Drawing.Point(128, 0);
            this.btnCancelar_790MY.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnCancelar_790MY.Name = "btnCancelar_790MY";
            this.btnCancelar_790MY.Size = new System.Drawing.Size(112, 36);
            this.btnCancelar_790MY.TabIndex = 1;
            this.btnCancelar_790MY.Tag = "btn_Cancelar";
            this.btnCancelar_790MY.Text = "Cancelar";
            this.btnCancelar_790MY.UseVisualStyleBackColor = false;
            this.btnCancelar_790MY.Click += new System.EventHandler(this.btnCancelar_790MY_Click);
            //
            // btnAplicar_790MY
            //
            this.btnAplicar_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnAplicar_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAplicar_790MY.FlatAppearance.BorderSize = 0;
            this.btnAplicar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAplicar_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnAplicar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnAplicar_790MY.Location = new System.Drawing.Point(8, 0);
            this.btnAplicar_790MY.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.btnAplicar_790MY.Name = "btnAplicar_790MY";
            this.btnAplicar_790MY.Size = new System.Drawing.Size(112, 36);
            this.btnAplicar_790MY.TabIndex = 0;
            this.btnAplicar_790MY.Tag = "btnAplicar";
            this.btnAplicar_790MY.Text = "Aplicar";
            this.btnAplicar_790MY.UseVisualStyleBackColor = false;
            this.btnAplicar_790MY.Click += new System.EventHandler(this.btnAplicar_790MY_Click);
            //
            // pnlCuerpo_790MY
            //
            this.pnlCuerpo_790MY.Controls.Add(this.grpDatos_790MY);
            this.pnlCuerpo_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCuerpo_790MY.Location = new System.Drawing.Point(0, 56);
            this.pnlCuerpo_790MY.Name = "pnlCuerpo_790MY";
            this.pnlCuerpo_790MY.Padding = new System.Windows.Forms.Padding(16, 10, 16, 10);
            this.pnlCuerpo_790MY.Size = new System.Drawing.Size(504, 271);
            this.pnlCuerpo_790MY.TabIndex = 0;
            //
            // grpDatos_790MY
            //
            this.grpDatos_790MY.Controls.Add(this.tlpCampos_790MY);
            this.grpDatos_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDatos_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.grpDatos_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.grpDatos_790MY.Location = new System.Drawing.Point(16, 10);
            this.grpDatos_790MY.Name = "grpDatos_790MY";
            this.grpDatos_790MY.Padding = new System.Windows.Forms.Padding(12, 6, 12, 6);
            this.grpDatos_790MY.Size = new System.Drawing.Size(472, 251);
            this.grpDatos_790MY.TabIndex = 0;
            this.grpDatos_790MY.TabStop = false;
            this.grpDatos_790MY.Tag = "RC_grpDatos";
            this.grpDatos_790MY.Text = "Datos del Cliente";
            //
            // tlpCampos_790MY
            //
            this.tlpCampos_790MY.ColumnCount = 3;
            this.tlpCampos_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 104F));
            this.tlpCampos_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCampos_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpCampos_790MY.Controls.Add(this.lblDni_790MY, 0, 0);
            this.tlpCampos_790MY.Controls.Add(this.txtDni_790MY, 1, 0);
            this.tlpCampos_790MY.Controls.Add(this.lblApellidos_790MY, 0, 1);
            this.tlpCampos_790MY.Controls.Add(this.txtApellidos_790MY, 1, 1);
            this.tlpCampos_790MY.Controls.Add(this.lblNombres_790MY, 0, 2);
            this.tlpCampos_790MY.Controls.Add(this.txtNombres_790MY, 1, 2);
            this.tlpCampos_790MY.Controls.Add(this.lblEmail_790MY, 0, 3);
            this.tlpCampos_790MY.Controls.Add(this.txtEmail_790MY, 1, 3);
            this.tlpCampos_790MY.Controls.Add(this.btnEmailsAdicionales_790MY, 2, 3);
            this.tlpCampos_790MY.Controls.Add(this.lblCelular_790MY, 0, 4);
            this.tlpCampos_790MY.Controls.Add(this.txtCelular_790MY, 1, 4);
            this.tlpCampos_790MY.Controls.Add(this.btnTelefonosAdicionales_790MY, 2, 4);
            this.tlpCampos_790MY.Controls.Add(this.lblDireccion_790MY, 0, 5);
            this.tlpCampos_790MY.Controls.Add(this.txtDireccion_790MY, 1, 5);
            this.tlpCampos_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCampos_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.tlpCampos_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.tlpCampos_790MY.Location = new System.Drawing.Point(12, 24);
            this.tlpCampos_790MY.Name = "tlpCampos_790MY";
            this.tlpCampos_790MY.RowCount = 7;
            this.tlpCampos_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tlpCampos_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tlpCampos_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tlpCampos_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tlpCampos_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tlpCampos_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tlpCampos_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCampos_790MY.Size = new System.Drawing.Size(448, 221);
            this.tlpCampos_790MY.TabIndex = 0;
            //
            // lblDni_790MY
            //
            this.lblDni_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDni_790MY.AutoSize = true;
            this.lblDni_790MY.Location = new System.Drawing.Point(3, 8);
            this.lblDni_790MY.Name = "lblDni_790MY";
            this.lblDni_790MY.Size = new System.Drawing.Size(34, 17);
            this.lblDni_790MY.TabIndex = 0;
            this.lblDni_790MY.Tag = "FC_lblDni";
            this.lblDni_790MY.Text = "DNI:";
            //
            // txtDni_790MY
            //
            this.txtDni_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDni_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(238)))), ((int)(((byte)(231)))));
            this.txtDni_790MY.Location = new System.Drawing.Point(107, 4);
            this.txtDni_790MY.Name = "txtDni_790MY";
            this.txtDni_790MY.ReadOnly = true;
            this.txtDni_790MY.Size = new System.Drawing.Size(298, 25);
            this.txtDni_790MY.TabIndex = 1;
            this.txtDni_790MY.TabStop = false;
            //
            // lblApellidos_790MY
            //
            this.lblApellidos_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblApellidos_790MY.AutoSize = true;
            this.lblApellidos_790MY.Location = new System.Drawing.Point(3, 44);
            this.lblApellidos_790MY.Name = "lblApellidos_790MY";
            this.lblApellidos_790MY.Size = new System.Drawing.Size(66, 17);
            this.lblApellidos_790MY.TabIndex = 2;
            this.lblApellidos_790MY.Tag = "FC_lblApellidos";
            this.lblApellidos_790MY.Text = "Apellidos:";
            //
            // txtApellidos_790MY
            //
            this.txtApellidos_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtApellidos_790MY.Location = new System.Drawing.Point(107, 39);
            this.txtApellidos_790MY.Name = "txtApellidos_790MY";
            this.txtApellidos_790MY.Size = new System.Drawing.Size(298, 25);
            this.txtApellidos_790MY.TabIndex = 3;
            //
            // lblNombres_790MY
            //
            this.lblNombres_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNombres_790MY.AutoSize = true;
            this.lblNombres_790MY.Location = new System.Drawing.Point(3, 79);
            this.lblNombres_790MY.Name = "lblNombres_790MY";
            this.lblNombres_790MY.Size = new System.Drawing.Size(65, 17);
            this.lblNombres_790MY.TabIndex = 4;
            this.lblNombres_790MY.Tag = "FC_lblNombres";
            this.lblNombres_790MY.Text = "Nombres:";
            //
            // txtNombres_790MY
            //
            this.txtNombres_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNombres_790MY.Location = new System.Drawing.Point(107, 74);
            this.txtNombres_790MY.Name = "txtNombres_790MY";
            this.txtNombres_790MY.Size = new System.Drawing.Size(298, 25);
            this.txtNombres_790MY.TabIndex = 5;
            //
            // lblEmail_790MY
            //
            this.lblEmail_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEmail_790MY.AutoSize = true;
            this.lblEmail_790MY.Location = new System.Drawing.Point(3, 114);
            this.lblEmail_790MY.Name = "lblEmail_790MY";
            this.lblEmail_790MY.Size = new System.Drawing.Size(42, 17);
            this.lblEmail_790MY.TabIndex = 6;
            this.lblEmail_790MY.Tag = "FC_lblEmail";
            this.lblEmail_790MY.Text = "Email:";
            //
            // txtEmail_790MY
            //
            this.txtEmail_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEmail_790MY.Location = new System.Drawing.Point(107, 109);
            this.txtEmail_790MY.Name = "txtEmail_790MY";
            this.txtEmail_790MY.Size = new System.Drawing.Size(298, 25);
            this.txtEmail_790MY.TabIndex = 7;
            //
            // btnEmailsAdicionales_790MY
            //
            this.btnEmailsAdicionales_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnEmailsAdicionales_790MY.BackColor = System.Drawing.Color.White;
            this.btnEmailsAdicionales_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEmailsAdicionales_790MY.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnEmailsAdicionales_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEmailsAdicionales_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnEmailsAdicionales_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnEmailsAdicionales_790MY.Location = new System.Drawing.Point(411, 108);
            this.btnEmailsAdicionales_790MY.Name = "btnEmailsAdicionales_790MY";
            this.btnEmailsAdicionales_790MY.Size = new System.Drawing.Size(34, 26);
            this.btnEmailsAdicionales_790MY.TabIndex = 8;
            this.btnEmailsAdicionales_790MY.Text = "+";
            this.btnEmailsAdicionales_790MY.UseVisualStyleBackColor = false;
            this.btnEmailsAdicionales_790MY.Click += new System.EventHandler(this.btnEmailsAdicionales_790MY_Click);
            //
            // lblCelular_790MY
            //
            this.lblCelular_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCelular_790MY.AutoSize = true;
            this.lblCelular_790MY.Location = new System.Drawing.Point(3, 149);
            this.lblCelular_790MY.Name = "lblCelular_790MY";
            this.lblCelular_790MY.Size = new System.Drawing.Size(53, 17);
            this.lblCelular_790MY.TabIndex = 9;
            this.lblCelular_790MY.Tag = "FC_lblCelular";
            this.lblCelular_790MY.Text = "Celular:";
            //
            // txtCelular_790MY
            //
            this.txtCelular_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCelular_790MY.Location = new System.Drawing.Point(107, 144);
            this.txtCelular_790MY.Name = "txtCelular_790MY";
            this.txtCelular_790MY.Size = new System.Drawing.Size(298, 25);
            this.txtCelular_790MY.TabIndex = 10;
            //
            // btnTelefonosAdicionales_790MY
            //
            this.btnTelefonosAdicionales_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnTelefonosAdicionales_790MY.BackColor = System.Drawing.Color.White;
            this.btnTelefonosAdicionales_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTelefonosAdicionales_790MY.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnTelefonosAdicionales_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTelefonosAdicionales_790MY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnTelefonosAdicionales_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnTelefonosAdicionales_790MY.Location = new System.Drawing.Point(411, 143);
            this.btnTelefonosAdicionales_790MY.Name = "btnTelefonosAdicionales_790MY";
            this.btnTelefonosAdicionales_790MY.Size = new System.Drawing.Size(34, 26);
            this.btnTelefonosAdicionales_790MY.TabIndex = 11;
            this.btnTelefonosAdicionales_790MY.Text = "+";
            this.btnTelefonosAdicionales_790MY.UseVisualStyleBackColor = false;
            this.btnTelefonosAdicionales_790MY.Click += new System.EventHandler(this.btnTelefonosAdicionales_790MY_Click);
            //
            // lblDireccion_790MY
            //
            this.lblDireccion_790MY.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDireccion_790MY.AutoSize = true;
            this.lblDireccion_790MY.Location = new System.Drawing.Point(3, 184);
            this.lblDireccion_790MY.Name = "lblDireccion_790MY";
            this.lblDireccion_790MY.Size = new System.Drawing.Size(67, 17);
            this.lblDireccion_790MY.TabIndex = 12;
            this.lblDireccion_790MY.Tag = "FC_lblDireccion";
            this.lblDireccion_790MY.Text = "Dirección:";
            //
            // txtDireccion_790MY
            //
            this.txtDireccion_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDireccion_790MY.Location = new System.Drawing.Point(107, 179);
            this.txtDireccion_790MY.Name = "txtDireccion_790MY";
            this.txtDireccion_790MY.Size = new System.Drawing.Size(298, 25);
            this.txtDireccion_790MY.TabIndex = 13;
            //
            // FormRegistrarCliente_790MY
            //
            this.AcceptButton = this.btnAplicar_790MY;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.CancelButton = this.btnCancelar_790MY;
            this.Controls.Add(this.pnlCuerpo_790MY);
            this.Controls.Add(this.pnlFooter_790MY);
            this.Controls.Add(this.pnlHeader_790MY);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRegistrarCliente_790MY";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Size = new System.Drawing.Size(520, 420);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Registrar Cliente";
            this.Load += new System.EventHandler(this.FormRegistrarCliente_790MY_Load);
            this.pnlHeader_790MY.ResumeLayout(false);
            this.pnlHeader_790MY.PerformLayout();
            this.pnlFooter_790MY.ResumeLayout(false);
            this.pnlFooter_790MY.PerformLayout();
            this.flpBotones_790MY.ResumeLayout(false);
            this.pnlCuerpo_790MY.ResumeLayout(false);
            this.grpDatos_790MY.ResumeLayout(false);
            this.tlpCampos_790MY.ResumeLayout(false);
            this.tlpCampos_790MY.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader_790MY;
        private System.Windows.Forms.Label lblTitulo_790MY;
        private System.Windows.Forms.Label lblSubtitulo_790MY;
        private System.Windows.Forms.Panel pnlFooter_790MY;
        private System.Windows.Forms.FlowLayoutPanel flpBotones_790MY;
        private System.Windows.Forms.Button btnAplicar_790MY;
        private System.Windows.Forms.Button btnCancelar_790MY;
        private System.Windows.Forms.Panel pnlCuerpo_790MY;
        private System.Windows.Forms.GroupBox grpDatos_790MY;
        private System.Windows.Forms.TableLayoutPanel tlpCampos_790MY;
        private System.Windows.Forms.Label lblDni_790MY;
        private System.Windows.Forms.TextBox txtDni_790MY;
        private System.Windows.Forms.Label lblApellidos_790MY;
        private System.Windows.Forms.TextBox txtApellidos_790MY;
        private System.Windows.Forms.Label lblNombres_790MY;
        private System.Windows.Forms.TextBox txtNombres_790MY;
        private System.Windows.Forms.Label lblEmail_790MY;
        private System.Windows.Forms.TextBox txtEmail_790MY;
        private System.Windows.Forms.Button btnEmailsAdicionales_790MY;
        private System.Windows.Forms.Label lblCelular_790MY;
        private System.Windows.Forms.TextBox txtCelular_790MY;
        private System.Windows.Forms.Button btnTelefonosAdicionales_790MY;
        private System.Windows.Forms.Label lblDireccion_790MY;
        private System.Windows.Forms.TextBox txtDireccion_790MY;
        private System.Windows.Forms.ToolTip tipAyuda_790MY;
    }
}
