using System.Drawing;

namespace GUI
{
    partial class FormGestionUsuarios_08YS
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            this.cmbRol = new System.Windows.Forms.ComboBox();
            this.rbBloqueados = new System.Windows.Forms.RadioButton();
            this.lblActivo = new System.Windows.Forms.Label();
            this.lblBloqueado = new System.Windows.Forms.Label();
            this.lblLogin = new System.Windows.Forms.Label();
            this.lblRol = new System.Windows.Forms.Label();
            this.lblNombres = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblApellidos = new System.Windows.Forms.Label();
            this.lblDNI = new System.Windows.Forms.Label();
            this.txtActivo = new System.Windows.Forms.TextBox();
            this.txtBloqueado = new System.Windows.Forms.TextBox();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtNombres = new System.Windows.Forms.TextBox();
            this.txtApellidos = new System.Windows.Forms.TextBox();
            this.txtDNI = new System.Windows.Forms.TextBox();
            this.txtMensaje = new System.Windows.Forms.TextBox();
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnAplicar = new System.Windows.Forms.Button();
            this.btnActDes = new System.Windows.Forms.Button();
            this.btnModificar = new System.Windows.Forms.Button();
            this.btnDesbloquear = new System.Windows.Forms.Button();
            this.btnCrear = new System.Windows.Forms.Button();
            this.rbActivos = new System.Windows.Forms.RadioButton();
            this.rbTodos = new System.Windows.Forms.RadioButton();
            this.dgvUsuarios = new System.Windows.Forms.DataGridView();
            this.lblCantUsuarios = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbRol
            // 
            this.cmbRol.FormattingEnabled = true;
            this.cmbRol.Location = new System.Drawing.Point(112, 570);
            this.cmbRol.Name = "cmbRol";
            this.cmbRol.Size = new System.Drawing.Size(182, 21);
            this.cmbRol.TabIndex = 61;
            // 
            // rbBloqueados
            // 
            this.rbBloqueados.AutoSize = true;
            this.rbBloqueados.BackColor = System.Drawing.Color.Transparent;
            this.rbBloqueados.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbBloqueados.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.rbBloqueados.Location = new System.Drawing.Point(540, 28);
            this.rbBloqueados.Name = "rbBloqueados";
            this.rbBloqueados.Size = new System.Drawing.Size(99, 20);
            this.rbBloqueados.TabIndex = 60;
            this.rbBloqueados.TabStop = true;
            this.rbBloqueados.Tag = "rbBloqueados";
            this.rbBloqueados.Text = "Bloqueados";
            this.rbBloqueados.UseVisualStyleBackColor = false;
            this.rbBloqueados.CheckedChanged += new System.EventHandler(this.rbBloqueados_CheckedChanged);
            // 
            // lblActivo
            // 
            this.lblActivo.AutoSize = true;
            this.lblActivo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.lblActivo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActivo.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblActivo.Location = new System.Drawing.Point(16, 682);
            this.lblActivo.Name = "lblActivo";
            this.lblActivo.Size = new System.Drawing.Size(44, 16);
            this.lblActivo.TabIndex = 59;
            this.lblActivo.Tag = "lblActivo";
            this.lblActivo.Text = "Activo";
            // 
            // lblBloqueado
            // 
            this.lblBloqueado.AutoSize = true;
            this.lblBloqueado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.lblBloqueado.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBloqueado.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblBloqueado.Location = new System.Drawing.Point(16, 646);
            this.lblBloqueado.Name = "lblBloqueado";
            this.lblBloqueado.Size = new System.Drawing.Size(74, 16);
            this.lblBloqueado.TabIndex = 58;
            this.lblBloqueado.Tag = "lblBloqueado";
            this.lblBloqueado.Text = "Bloqueado";
            // 
            // lblLogin
            // 
            this.lblLogin.AutoSize = true;
            this.lblLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.lblLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogin.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblLogin.Location = new System.Drawing.Point(16, 609);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(70, 16);
            this.lblLogin.TabIndex = 57;
            this.lblLogin.Tag = "";
            this.lblLogin.Text = "Username";
            // 
            // lblRol
            // 
            this.lblRol.AutoSize = true;
            this.lblRol.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.lblRol.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRol.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblRol.Location = new System.Drawing.Point(16, 570);
            this.lblRol.Name = "lblRol";
            this.lblRol.Size = new System.Drawing.Size(28, 16);
            this.lblRol.TabIndex = 56;
            this.lblRol.Tag = "lblRol";
            this.lblRol.Text = "Rol";
            // 
            // lblNombres
            // 
            this.lblNombres.AutoSize = true;
            this.lblNombres.BackColor = System.Drawing.Color.Transparent;
            this.lblNombres.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombres.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblNombres.Location = new System.Drawing.Point(16, 496);
            this.lblNombres.Name = "lblNombres";
            this.lblNombres.Size = new System.Drawing.Size(63, 16);
            this.lblNombres.TabIndex = 55;
            this.lblNombres.Tag = "lblNombres";
            this.lblNombres.Text = "Nombres";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.lblEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmail.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblEmail.Location = new System.Drawing.Point(16, 531);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(41, 16);
            this.lblEmail.TabIndex = 54;
            this.lblEmail.Text = "Email";
            // 
            // lblApellidos
            // 
            this.lblApellidos.AutoSize = true;
            this.lblApellidos.BackColor = System.Drawing.Color.Transparent;
            this.lblApellidos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApellidos.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblApellidos.Location = new System.Drawing.Point(16, 456);
            this.lblApellidos.Name = "lblApellidos";
            this.lblApellidos.Size = new System.Drawing.Size(64, 16);
            this.lblApellidos.TabIndex = 53;
            this.lblApellidos.Tag = "lblApellidos";
            this.lblApellidos.Text = "Apellidos";
            // 
            // lblDNI
            // 
            this.lblDNI.AutoSize = true;
            this.lblDNI.BackColor = System.Drawing.Color.Transparent;
            this.lblDNI.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDNI.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblDNI.Location = new System.Drawing.Point(16, 410);
            this.lblDNI.Name = "lblDNI";
            this.lblDNI.Size = new System.Drawing.Size(30, 16);
            this.lblDNI.TabIndex = 52;
            this.lblDNI.Text = "DNI";
            // 
            // txtActivo
            // 
            this.txtActivo.Location = new System.Drawing.Point(112, 674);
            this.txtActivo.Name = "txtActivo";
            this.txtActivo.Size = new System.Drawing.Size(182, 20);
            this.txtActivo.TabIndex = 51;
            // 
            // txtBloqueado
            // 
            this.txtBloqueado.Location = new System.Drawing.Point(112, 639);
            this.txtBloqueado.Name = "txtBloqueado";
            this.txtBloqueado.Size = new System.Drawing.Size(182, 20);
            this.txtBloqueado.TabIndex = 50;
            // 
            // txtLogin
            // 
            this.txtLogin.Location = new System.Drawing.Point(112, 603);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(182, 20);
            this.txtLogin.TabIndex = 49;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(112, 525);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(182, 20);
            this.txtEmail.TabIndex = 48;
            // 
            // txtNombres
            // 
            this.txtNombres.Location = new System.Drawing.Point(112, 488);
            this.txtNombres.Name = "txtNombres";
            this.txtNombres.Size = new System.Drawing.Size(182, 20);
            this.txtNombres.TabIndex = 47;
            // 
            // txtApellidos
            // 
            this.txtApellidos.Location = new System.Drawing.Point(112, 453);
            this.txtApellidos.Name = "txtApellidos";
            this.txtApellidos.Size = new System.Drawing.Size(182, 20);
            this.txtApellidos.TabIndex = 46;
            // 
            // txtDNI
            // 
            this.txtDNI.Location = new System.Drawing.Point(112, 410);
            this.txtDNI.Name = "txtDNI";
            this.txtDNI.Size = new System.Drawing.Size(182, 20);
            this.txtDNI.TabIndex = 45;
            // 
            // txtMensaje
            // 
            this.txtMensaje.Location = new System.Drawing.Point(540, 421);
            this.txtMensaje.Multiline = true;
            this.txtMensaje.Name = "txtMensaje";
            this.txtMensaje.Size = new System.Drawing.Size(227, 185);
            this.txtMensaje.TabIndex = 44;
            this.txtMensaje.Text = "                            Mensaje:";
            // 
            // btnSalir
            // 
            this.btnSalir.BackColor = System.Drawing.SystemColors.Control;
            this.btnSalir.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalir.Location = new System.Drawing.Point(868, 601);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(98, 46);
            this.btnSalir.TabIndex = 43;
            this.btnSalir.Tag = "btnSalir";
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = false;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Location = new System.Drawing.Point(868, 513);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(98, 46);
            this.btnCancelar.TabIndex = 42;
            this.btnCancelar.Tag = "btnCancelar";
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnAplicar
            // 
            this.btnAplicar.BackColor = System.Drawing.SystemColors.Control;
            this.btnAplicar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAplicar.Location = new System.Drawing.Point(868, 421);
            this.btnAplicar.Name = "btnAplicar";
            this.btnAplicar.Size = new System.Drawing.Size(98, 46);
            this.btnAplicar.TabIndex = 41;
            this.btnAplicar.Tag = "btnAplicar";
            this.btnAplicar.Text = "Aplicar";
            this.btnAplicar.UseVisualStyleBackColor = false;
            this.btnAplicar.Click += new System.EventHandler(this.btnAplicar_Click);
            // 
            // btnActDes
            // 
            this.btnActDes.BackColor = System.Drawing.SystemColors.Control;
            this.btnActDes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActDes.Location = new System.Drawing.Point(868, 328);
            this.btnActDes.Name = "btnActDes";
            this.btnActDes.Size = new System.Drawing.Size(98, 46);
            this.btnActDes.TabIndex = 40;
            this.btnActDes.Tag = "btnActivarDesactivar";
            this.btnActDes.Text = "Act./Desact.";
            this.btnActDes.UseVisualStyleBackColor = false;
            this.btnActDes.Click += new System.EventHandler(this.btnActDes_Click);
            // 
            // btnModificar
            // 
            this.btnModificar.BackColor = System.Drawing.SystemColors.Control;
            this.btnModificar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModificar.Location = new System.Drawing.Point(868, 237);
            this.btnModificar.Name = "btnModificar";
            this.btnModificar.Size = new System.Drawing.Size(98, 46);
            this.btnModificar.TabIndex = 39;
            this.btnModificar.Tag = "btnModificar";
            this.btnModificar.Text = "Modificar";
            this.btnModificar.UseVisualStyleBackColor = false;
            this.btnModificar.Click += new System.EventHandler(this.btnModificar_Click);
            // 
            // btnDesbloquear
            // 
            this.btnDesbloquear.BackColor = System.Drawing.SystemColors.Control;
            this.btnDesbloquear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDesbloquear.Location = new System.Drawing.Point(868, 145);
            this.btnDesbloquear.Name = "btnDesbloquear";
            this.btnDesbloquear.Size = new System.Drawing.Size(98, 46);
            this.btnDesbloquear.TabIndex = 38;
            this.btnDesbloquear.Tag = "btnDesbloquear";
            this.btnDesbloquear.Text = "Desbloquear";
            this.btnDesbloquear.UseVisualStyleBackColor = false;
            this.btnDesbloquear.Click += new System.EventHandler(this.btnDesbloquear_Click);
            // 
            // btnCrear
            // 
            this.btnCrear.BackColor = System.Drawing.SystemColors.Control;
            this.btnCrear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCrear.Location = new System.Drawing.Point(868, 61);
            this.btnCrear.Name = "btnCrear";
            this.btnCrear.Size = new System.Drawing.Size(98, 46);
            this.btnCrear.TabIndex = 37;
            this.btnCrear.Tag = "btnCrear";
            this.btnCrear.Text = "Crear";
            this.btnCrear.UseVisualStyleBackColor = false;
            this.btnCrear.Click += new System.EventHandler(this.btnCrear_Click);
            // 
            // rbActivos
            // 
            this.rbActivos.AutoSize = true;
            this.rbActivos.BackColor = System.Drawing.Color.Transparent;
            this.rbActivos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbActivos.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.rbActivos.Location = new System.Drawing.Point(442, 28);
            this.rbActivos.Name = "rbActivos";
            this.rbActivos.Size = new System.Drawing.Size(69, 20);
            this.rbActivos.TabIndex = 36;
            this.rbActivos.TabStop = true;
            this.rbActivos.Tag = "rbActivos";
            this.rbActivos.Text = "Activos";
            this.rbActivos.UseVisualStyleBackColor = false;
            this.rbActivos.CheckedChanged += new System.EventHandler(this.rbActivos_CheckedChanged);
            // 
            // rbTodos
            // 
            this.rbTodos.AutoSize = true;
            this.rbTodos.BackColor = System.Drawing.Color.Transparent;
            this.rbTodos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbTodos.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.rbTodos.Location = new System.Drawing.Point(340, 28);
            this.rbTodos.Name = "rbTodos";
            this.rbTodos.Size = new System.Drawing.Size(65, 20);
            this.rbTodos.TabIndex = 35;
            this.rbTodos.TabStop = true;
            this.rbTodos.Tag = "rbTodos";
            this.rbTodos.Text = "Todos";
            this.rbTodos.UseVisualStyleBackColor = false;
            this.rbTodos.CheckedChanged += new System.EventHandler(this.rbTodos_CheckedChanged);
            // 
            // dgvUsuarios
            // 
            this.dgvUsuarios.AllowUserToAddRows = false;
            this.dgvUsuarios.AllowUserToDeleteRows = false;
            this.dgvUsuarios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsuarios.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(10)))), ((int)(((byte)(40)))));
            this.dgvUsuarios.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvUsuarios.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(25)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(25)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvUsuarios.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvUsuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsuarios.EnableHeadersVisualStyles = false;
            this.dgvUsuarios.GridColor = System.Drawing.Color.Goldenrod;
            this.dgvUsuarios.Location = new System.Drawing.Point(10, 79);
            this.dgvUsuarios.MultiSelect = false;
            this.dgvUsuarios.Name = "dgvUsuarios";
            this.dgvUsuarios.ReadOnly = true;
            this.dgvUsuarios.RowHeadersVisible = false;
            this.dgvUsuarios.RowHeadersWidth = 51;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(10)))), ((int)(((byte)(30)))));
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(100)))));
            this.dgvUsuarios.RowsDefaultCellStyle = dataGridViewCellStyle14;
            this.dgvUsuarios.RowTemplate.Height = 30;
            this.dgvUsuarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsuarios.Size = new System.Drawing.Size(758, 307);
            this.dgvUsuarios.TabIndex = 34;
            this.dgvUsuarios.SelectionChanged += new System.EventHandler(this.dgvUsuarios_SelectionChanged);
            // 
            // lblCantUsuarios
            // 
            this.lblCantUsuarios.AutoSize = true;
            this.lblCantUsuarios.BackColor = System.Drawing.Color.Transparent;
            this.lblCantUsuarios.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCantUsuarios.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblCantUsuarios.Location = new System.Drawing.Point(658, 28);
            this.lblCantUsuarios.Name = "lblCantUsuarios";
            this.lblCantUsuarios.Size = new System.Drawing.Size(137, 16);
            this.lblCantUsuarios.TabIndex = 33;
            this.lblCantUsuarios.Tag = "lblNumeroUsuarios";
            this.lblCantUsuarios.Text = "Numero de Usuarios: ";
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.BackColor = System.Drawing.Color.Transparent;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 22.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblTitulo.Location = new System.Drawing.Point(72, 17);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(134, 36);
            this.lblTitulo.TabIndex = 32;
            this.lblTitulo.Tag = "lblUsuarios";
            this.lblTitulo.Text = "Usuarios";
            // 
            // iconPictureBox1
            // 
            this.iconPictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox1.ForeColor = System.Drawing.Color.Goldenrod;
            this.iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.UsersViewfinder;
            this.iconPictureBox1.IconColor = System.Drawing.Color.Goldenrod;
            this.iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox1.IconSize = 58;
            this.iconPictureBox1.Location = new System.Drawing.Point(9, 17);
            this.iconPictureBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.iconPictureBox1.Name = "iconPictureBox1";
            this.iconPictureBox1.Size = new System.Drawing.Size(58, 62);
            this.iconPictureBox1.TabIndex = 62;
            this.iconPictureBox1.TabStop = false;
            // 
            // FormGestionUsuarios_08YS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::GUI_08YS.Properties.Resources.ChatGPT_Image_19_may_2026__22_23_23;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1089, 743);
            this.Controls.Add(this.iconPictureBox1);
            this.Controls.Add(this.cmbRol);
            this.Controls.Add(this.rbBloqueados);
            this.Controls.Add(this.lblActivo);
            this.Controls.Add(this.lblBloqueado);
            this.Controls.Add(this.lblLogin);
            this.Controls.Add(this.lblRol);
            this.Controls.Add(this.lblNombres);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.lblApellidos);
            this.Controls.Add(this.lblDNI);
            this.Controls.Add(this.txtActivo);
            this.Controls.Add(this.txtBloqueado);
            this.Controls.Add(this.txtLogin);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.txtNombres);
            this.Controls.Add(this.txtApellidos);
            this.Controls.Add(this.txtDNI);
            this.Controls.Add(this.txtMensaje);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAplicar);
            this.Controls.Add(this.btnActDes);
            this.Controls.Add(this.btnModificar);
            this.Controls.Add(this.btnDesbloquear);
            this.Controls.Add(this.btnCrear);
            this.Controls.Add(this.rbActivos);
            this.Controls.Add(this.rbTodos);
            this.Controls.Add(this.dgvUsuarios);
            this.Controls.Add(this.lblCantUsuarios);
            this.Controls.Add(this.lblTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormGestionUsuarios_08YS";
            this.Text = "FormGestionUsuarios";
            this.Load += new System.EventHandler(this.FormGestionUsuarios_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbRol;
        private System.Windows.Forms.RadioButton rbBloqueados;
        private System.Windows.Forms.Label lblActivo;
        private System.Windows.Forms.Label lblBloqueado;
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.Label lblRol;
        private System.Windows.Forms.Label lblNombres;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblApellidos;
        private System.Windows.Forms.Label lblDNI;
        private System.Windows.Forms.TextBox txtActivo;
        private System.Windows.Forms.TextBox txtBloqueado;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtNombres;
        private System.Windows.Forms.TextBox txtApellidos;
        private System.Windows.Forms.TextBox txtDNI;
        private System.Windows.Forms.TextBox txtMensaje;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnAplicar;
        private System.Windows.Forms.Button btnActDes;
        private System.Windows.Forms.Button btnModificar;
        private System.Windows.Forms.Button btnDesbloquear;
        private System.Windows.Forms.Button btnCrear;
        private System.Windows.Forms.RadioButton rbActivos;
        private System.Windows.Forms.RadioButton rbTodos;
        private System.Windows.Forms.DataGridView dgvUsuarios;
        private System.Windows.Forms.Label lblCantUsuarios;
        private System.Windows.Forms.Label lblTitulo;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
    }
}