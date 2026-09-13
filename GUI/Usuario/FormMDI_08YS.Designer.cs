namespace GUI_08YS
{
    partial class FormMDI_08YS
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.crearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modificarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.desbloquearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelIdioma = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.Titulo = new System.Windows.Forms.Label();
            this.panelLateral = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPerfil = new FontAwesome.Sharp.IconButton();
            this.btnAdministrativo = new FontAwesome.Sharp.IconButton();
            this.btnReservar = new FontAwesome.Sharp.IconButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnCerrarSesion = new FontAwesome.Sharp.IconButton();
            this.panelLogo = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblNombreApellido = new System.Windows.Forms.Label();
            this.lblRolSistema = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.IdiomaCombobox = new CustomControls.IconComboBox();
            this.AdministrativoDropDownMenu = new GUI_08YS.DropdownMenuStrip_08YS(this.components);
            this.gestionUsuarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bitacoraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionAccesosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rolesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.familiasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gestionRespaldosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PerfilDropDownMenu = new GUI_08YS.DropdownMenuStrip_08YS(this.components);
            this.cambiarContraseñaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReLoginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cerrarSesionToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.panelIdioma.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelLateral.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panelLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.AdministrativoDropDownMenu.SuspendLayout();
            this.PerfilDropDownMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // crearToolStripMenuItem
            // 
            this.crearToolStripMenuItem.Name = "crearToolStripMenuItem";
            this.crearToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.crearToolStripMenuItem.Text = "Crear";
            // 
            // modificarToolStripMenuItem
            // 
            this.modificarToolStripMenuItem.Name = "modificarToolStripMenuItem";
            this.modificarToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.modificarToolStripMenuItem.Text = "Modificar";
            // 
            // desbloquearToolStripMenuItem
            // 
            this.desbloquearToolStripMenuItem.Name = "desbloquearToolStripMenuItem";
            this.desbloquearToolStripMenuItem.Size = new System.Drawing.Size(163, 24);
            this.desbloquearToolStripMenuItem.Text = "Desbloquear";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(20)))), ((int)(((byte)(38)))));
            this.panel1.Controls.Add(this.panelIdioma);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.Titulo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1800, 46);
            this.panel1.TabIndex = 2;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // panelIdioma
            // 
            this.panelIdioma.Controls.Add(this.IdiomaCombobox);
            this.panelIdioma.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelIdioma.Location = new System.Drawing.Point(1370, 0);
            this.panelIdioma.Name = "panelIdioma";
            this.panelIdioma.Size = new System.Drawing.Size(250, 46);
            this.panelIdioma.TabIndex = 8;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = global::GUI_08YS.Properties.Resources.HorizonLogoPuro1;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(83, 46);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.Dock = System.Windows.Forms.DockStyle.Right;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(20)))), ((int)(((byte)(90)))));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button3.Location = new System.Drawing.Point(1620, 0);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button3.Name = "button3";
            this.button3.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.button3.Size = new System.Drawing.Size(60, 46);
            this.button3.TabIndex = 3;
            this.button3.Text = "—";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.Dock = System.Windows.Forms.DockStyle.Right;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(20)))), ((int)(((byte)(90)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button2.Location = new System.Drawing.Point(1680, 0);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Padding = new System.Windows.Forms.Padding(3, 0, 0, 7);
            this.button2.Size = new System.Drawing.Size(60, 46);
            this.button2.TabIndex = 2;
            this.button2.Text = "□";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Maroon;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button1.Location = new System.Drawing.Point(1740, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Padding = new System.Windows.Forms.Padding(4, 1, 0, 0);
            this.button1.Size = new System.Drawing.Size(60, 46);
            this.button1.TabIndex = 1;
            this.button1.Text = "X";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Titulo
            // 
            this.Titulo.AutoSize = true;
            this.Titulo.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Titulo.Location = new System.Drawing.Point(89, 12);
            this.Titulo.Name = "Titulo";
            this.Titulo.Size = new System.Drawing.Size(279, 16);
            this.Titulo.TabIndex = 0;
            this.Titulo.Text = "Sistema de Hotelería - Horizon Hotel && Resort";
            // 
            // panelLateral
            // 
            this.panelLateral.AutoScroll = true;
            this.panelLateral.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(7)))), ((int)(((byte)(40)))));
            this.panelLateral.Controls.Add(this.flowLayoutPanel1);
            this.panelLateral.Controls.Add(this.panel3);
            this.panelLateral.Controls.Add(this.panelLogo);
            this.panelLateral.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLateral.Location = new System.Drawing.Point(0, 46);
            this.panelLateral.Margin = new System.Windows.Forms.Padding(4);
            this.panelLateral.Name = "panelLateral";
            this.panelLateral.Size = new System.Drawing.Size(331, 919);
            this.panelLateral.TabIndex = 4;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.flowLayoutPanel1.Controls.Add(this.btnPerfil);
            this.flowLayoutPanel1.Controls.Add(this.btnAdministrativo);
            this.flowLayoutPanel1.Controls.Add(this.btnReservar);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 70);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(331, 789);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // btnPerfil
            // 
            this.btnPerfil.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPerfil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPerfil.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPerfil.ForeColor = System.Drawing.Color.Gold;
            this.btnPerfil.IconChar = FontAwesome.Sharp.IconChar.User;
            this.btnPerfil.IconColor = System.Drawing.Color.Gold;
            this.btnPerfil.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnPerfil.IconSize = 30;
            this.btnPerfil.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPerfil.Location = new System.Drawing.Point(3, 2);
            this.btnPerfil.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPerfil.Name = "btnPerfil";
            this.btnPerfil.Padding = new System.Windows.Forms.Padding(5, 0, 20, 0);
            this.btnPerfil.Size = new System.Drawing.Size(325, 60);
            this.btnPerfil.TabIndex = 3;
            this.btnPerfil.Tag = "btnPerfil";
            this.btnPerfil.Text = "Perfil";
            this.btnPerfil.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPerfil.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPerfil.UseVisualStyleBackColor = true;
            this.btnPerfil.Click += new System.EventHandler(this.btnPerfil_Click);
            // 
            // btnAdministrativo
            // 
            this.btnAdministrativo.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAdministrativo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdministrativo.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdministrativo.ForeColor = System.Drawing.Color.Gold;
            this.btnAdministrativo.IconChar = FontAwesome.Sharp.IconChar.UserTag;
            this.btnAdministrativo.IconColor = System.Drawing.Color.Gold;
            this.btnAdministrativo.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnAdministrativo.IconSize = 30;
            this.btnAdministrativo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdministrativo.Location = new System.Drawing.Point(3, 66);
            this.btnAdministrativo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAdministrativo.Name = "btnAdministrativo";
            this.btnAdministrativo.Padding = new System.Windows.Forms.Padding(5, 0, 20, 0);
            this.btnAdministrativo.Size = new System.Drawing.Size(325, 60);
            this.btnAdministrativo.TabIndex = 4;
            this.btnAdministrativo.Tag = "btnAdministrativo";
            this.btnAdministrativo.Text = "Administrativo";
            this.btnAdministrativo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdministrativo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdministrativo.UseVisualStyleBackColor = true;
            this.btnAdministrativo.Click += new System.EventHandler(this.btnAdministrativo_Click);
            // 
            // btnReservar
            // 
            this.btnReservar.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnReservar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReservar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReservar.ForeColor = System.Drawing.Color.Gold;
            this.btnReservar.IconChar = FontAwesome.Sharp.IconChar.CalendarCheck;
            this.btnReservar.IconColor = System.Drawing.Color.Gold;
            this.btnReservar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnReservar.IconSize = 30;
            this.btnReservar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReservar.Location = new System.Drawing.Point(3, 130);
            this.btnReservar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnReservar.Name = "btnReservar";
            this.btnReservar.Padding = new System.Windows.Forms.Padding(5, 0, 20, 0);
            this.btnReservar.Size = new System.Drawing.Size(325, 60);
            this.btnReservar.TabIndex = 5;
            this.btnReservar.Tag = "btnReservar";
            this.btnReservar.Text = "Reservar";
            this.btnReservar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReservar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReservar.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnCerrarSesion);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 859);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.panel3.Size = new System.Drawing.Size(331, 60);
            this.panel3.TabIndex = 1;
            // 
            // btnCerrarSesion
            // 
            this.btnCerrarSesion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(7)))), ((int)(((byte)(50)))));
            this.btnCerrarSesion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCerrarSesion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarSesion.Flip = FontAwesome.Sharp.FlipOrientation.Horizontal;
            this.btnCerrarSesion.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrarSesion.ForeColor = System.Drawing.Color.Gold;
            this.btnCerrarSesion.IconChar = FontAwesome.Sharp.IconChar.SignOut;
            this.btnCerrarSesion.IconColor = System.Drawing.Color.Gold;
            this.btnCerrarSesion.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCerrarSesion.IconSize = 30;
            this.btnCerrarSesion.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCerrarSesion.Location = new System.Drawing.Point(2, 0);
            this.btnCerrarSesion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCerrarSesion.Name = "btnCerrarSesion";
            this.btnCerrarSesion.Padding = new System.Windows.Forms.Padding(5, 0, 20, 0);
            this.btnCerrarSesion.Size = new System.Drawing.Size(327, 60);
            this.btnCerrarSesion.TabIndex = 7;
            this.btnCerrarSesion.Tag = "btnCerrarSesion";
            this.btnCerrarSesion.Text = "  Cerrar Sesión";
            this.btnCerrarSesion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCerrarSesion.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCerrarSesion.UseVisualStyleBackColor = false;
            this.btnCerrarSesion.Click += new System.EventHandler(this.btnCerrarSesion_Click);
            // 
            // panelLogo
            // 
            this.panelLogo.Controls.Add(this.pictureBox2);
            this.panelLogo.Controls.Add(this.lblNombreApellido);
            this.panelLogo.Controls.Add(this.lblRolSistema);
            this.panelLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLogo.Location = new System.Drawing.Point(0, 0);
            this.panelLogo.Margin = new System.Windows.Forms.Padding(4);
            this.panelLogo.Name = "panelLogo";
            this.panelLogo.Size = new System.Drawing.Size(331, 70);
            this.panelLogo.TabIndex = 0;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(4, 5);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(83, 57);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // lblNombreApellido
            // 
            this.lblNombreApellido.AutoSize = true;
            this.lblNombreApellido.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombreApellido.ForeColor = System.Drawing.Color.LightGray;
            this.lblNombreApellido.Location = new System.Drawing.Point(87, 11);
            this.lblNombreApellido.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNombreApellido.Name = "lblNombreApellido";
            this.lblNombreApellido.Size = new System.Drawing.Size(132, 20);
            this.lblNombreApellido.TabIndex = 1;
            this.lblNombreApellido.Text = "Nombre Apellido";
            // 
            // lblRolSistema
            // 
            this.lblRolSistema.AutoSize = true;
            this.lblRolSistema.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRolSistema.ForeColor = System.Drawing.Color.LightGray;
            this.lblRolSistema.Location = new System.Drawing.Point(89, 39);
            this.lblRolSistema.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRolSistema.Name = "lblRolSistema";
            this.lblRolSistema.Size = new System.Drawing.Size(71, 15);
            this.lblRolSistema.TabIndex = 2;
            this.lblRolSistema.Text = "RolSistema";
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::GUI_08YS.Properties.Resources.ChatGPT_Image_19_may_2026__22_23_23;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(331, 46);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1469, 919);
            this.panel2.TabIndex = 5;
            // 
            // IdiomaCombobox
            // 
            this.IdiomaCombobox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(20)))), ((int)(((byte)(38)))));
            this.IdiomaCombobox.BorderColor = System.Drawing.Color.Goldenrod;
            this.IdiomaCombobox.BorderFocusColor = System.Drawing.Color.Goldenrod;
            this.IdiomaCombobox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.IdiomaCombobox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.IdiomaCombobox.DropDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(20)))), ((int)(((byte)(38)))));
            this.IdiomaCombobox.DropDownBorderColor = System.Drawing.Color.Goldenrod;
            this.IdiomaCombobox.DropDownForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.IdiomaCombobox.DropDownHighlightBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(20)))), ((int)(((byte)(100)))));
            this.IdiomaCombobox.DropDownHighlightForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.IdiomaCombobox.DropDownItemHeight = 35;
            this.IdiomaCombobox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.IdiomaCombobox.IconChar = FontAwesome.Sharp.IconChar.Earth;
            this.IdiomaCombobox.IconColor = System.Drawing.Color.Goldenrod;
            this.IdiomaCombobox.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.IdiomaCombobox.IconPadding = 10;
            this.IdiomaCombobox.IconSize = 30;
            this.IdiomaCombobox.Location = new System.Drawing.Point(0, 0);
            this.IdiomaCombobox.Margin = new System.Windows.Forms.Padding(4);
            this.IdiomaCombobox.Name = "IdiomaCombobox";
            this.IdiomaCombobox.SelectedItem = null;
            this.IdiomaCombobox.SelectedValue = null;
            this.IdiomaCombobox.Size = new System.Drawing.Size(250, 46);
            this.IdiomaCombobox.TabIndex = 7;
            this.IdiomaCombobox.SelectedIndexChanged += new System.EventHandler(this.IdiomaCombobox_SelectedIndexChanged);
            // 
            // AdministrativoDropDownMenu
            // 
            this.AdministrativoDropDownMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.AdministrativoDropDownMenu.IsMainMenu = false;
            this.AdministrativoDropDownMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gestionUsuarioToolStripMenuItem,
            this.bitacoraToolStripMenuItem,
            this.gestionAccesosToolStripMenuItem,
            this.gestionRespaldosToolStripMenuItem});
            this.AdministrativoDropDownMenu.MenuItemHeight = 25;
            this.AdministrativoDropDownMenu.MenuItemTextColor = System.Drawing.Color.Empty;
            this.AdministrativoDropDownMenu.Name = "dropdownMenuStrip_08YS1";
            this.AdministrativoDropDownMenu.PrimaryColor = System.Drawing.Color.Empty;
            this.AdministrativoDropDownMenu.Size = new System.Drawing.Size(201, 100);
            // 
            // gestionUsuarioToolStripMenuItem
            // 
            this.gestionUsuarioToolStripMenuItem.Name = "gestionUsuarioToolStripMenuItem";
            this.gestionUsuarioToolStripMenuItem.Size = new System.Drawing.Size(200, 24);
            this.gestionUsuarioToolStripMenuItem.Tag = "menuGestionUsuario";
            this.gestionUsuarioToolStripMenuItem.Text = "GestionUsuario";
            this.gestionUsuarioToolStripMenuItem.Click += new System.EventHandler(this.gestionUsuarioToolStripMenuItem_Click);
            // 
            // bitacoraToolStripMenuItem
            // 
            this.bitacoraToolStripMenuItem.Name = "bitacoraToolStripMenuItem";
            this.bitacoraToolStripMenuItem.Size = new System.Drawing.Size(200, 24);
            this.bitacoraToolStripMenuItem.Tag = "menuBitacora";
            this.bitacoraToolStripMenuItem.Text = "Bitacora";
            this.bitacoraToolStripMenuItem.Click += new System.EventHandler(this.bitacoraToolStripMenuItem_Click);
            // 
            // gestionAccesosToolStripMenuItem
            // 
            this.gestionAccesosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rolesToolStripMenuItem,
            this.familiasToolStripMenuItem});
            this.gestionAccesosToolStripMenuItem.Name = "gestionAccesosToolStripMenuItem";
            this.gestionAccesosToolStripMenuItem.Size = new System.Drawing.Size(200, 24);
            this.gestionAccesosToolStripMenuItem.Tag = "menuGestionAccesos";
            this.gestionAccesosToolStripMenuItem.Text = "Gestion Accesos";
            // 
            // rolesToolStripMenuItem
            // 
            this.rolesToolStripMenuItem.Name = "rolesToolStripMenuItem";
            this.rolesToolStripMenuItem.Size = new System.Drawing.Size(145, 26);
            this.rolesToolStripMenuItem.Tag = "menuRoles";
            this.rolesToolStripMenuItem.Text = "Roles";
            this.rolesToolStripMenuItem.Click += new System.EventHandler(this.rolesToolStripMenuItem_Click);
            // 
            // familiasToolStripMenuItem
            // 
            this.familiasToolStripMenuItem.Name = "familiasToolStripMenuItem";
            this.familiasToolStripMenuItem.Size = new System.Drawing.Size(145, 26);
            this.familiasToolStripMenuItem.Tag = "menuFamilias";
            this.familiasToolStripMenuItem.Text = "Familias";
            this.familiasToolStripMenuItem.Click += new System.EventHandler(this.familiasToolStripMenuItem_Click);
            // 
            // gestionRespaldosToolStripMenuItem
            // 
            this.gestionRespaldosToolStripMenuItem.Name = "gestionRespaldosToolStripMenuItem";
            this.gestionRespaldosToolStripMenuItem.Size = new System.Drawing.Size(200, 24);
            this.gestionRespaldosToolStripMenuItem.Tag = "menuRespaldos";
            this.gestionRespaldosToolStripMenuItem.Text = "Gestion Respaldos";
            this.gestionRespaldosToolStripMenuItem.Click += new System.EventHandler(this.gestionRespaldosToolStripMenuItem_Click);
            // 
            // PerfilDropDownMenu
            // 
            this.PerfilDropDownMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.PerfilDropDownMenu.IsMainMenu = false;
            this.PerfilDropDownMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cambiarContraseñaToolStripMenuItem,
            this.ReLoginToolStripMenuItem,
            this.cerrarSesionToolStripMenuItem1});
            this.PerfilDropDownMenu.MenuItemHeight = 25;
            this.PerfilDropDownMenu.MenuItemTextColor = System.Drawing.Color.Empty;
            this.PerfilDropDownMenu.Name = "dropdownMenuStrip_08YS2";
            this.PerfilDropDownMenu.PrimaryColor = System.Drawing.Color.Empty;
            this.PerfilDropDownMenu.Size = new System.Drawing.Size(213, 76);
            // 
            // cambiarContraseñaToolStripMenuItem
            // 
            this.cambiarContraseñaToolStripMenuItem.Name = "cambiarContraseñaToolStripMenuItem";
            this.cambiarContraseñaToolStripMenuItem.Size = new System.Drawing.Size(212, 24);
            this.cambiarContraseñaToolStripMenuItem.Tag = "menuCambiarContraseña";
            this.cambiarContraseñaToolStripMenuItem.Text = "Cambiar Contraseña";
            this.cambiarContraseñaToolStripMenuItem.Click += new System.EventHandler(this.cambiarContraseñaToolStripMenuItem_Click);
            // 
            // ReLoginToolStripMenuItem
            // 
            this.ReLoginToolStripMenuItem.Name = "ReLoginToolStripMenuItem";
            this.ReLoginToolStripMenuItem.Size = new System.Drawing.Size(212, 24);
            this.ReLoginToolStripMenuItem.Tag = "menuRelogin";
            this.ReLoginToolStripMenuItem.Text = "ReLogin";
            this.ReLoginToolStripMenuItem.Click += new System.EventHandler(this.ReLoginToolStripMenuItem_Click);
            // 
            // cerrarSesionToolStripMenuItem1
            // 
            this.cerrarSesionToolStripMenuItem1.Name = "cerrarSesionToolStripMenuItem1";
            this.cerrarSesionToolStripMenuItem1.Size = new System.Drawing.Size(212, 24);
            this.cerrarSesionToolStripMenuItem1.Tag = "menuCerrarSesion";
            this.cerrarSesionToolStripMenuItem1.Text = "Cerrar Sesion";
            this.cerrarSesionToolStripMenuItem1.Click += new System.EventHandler(this.cerrarSesionToolStripMenuItem1_Click);
            // 
            // FormMDI_08YS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1800, 965);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelLateral);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormMDI_08YS";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMDI_FormClosing);
            this.Load += new System.EventHandler(this.FormMDI_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelIdioma.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelLateral.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panelLogo.ResumeLayout(false);
            this.panelLogo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.AdministrativoDropDownMenu.ResumeLayout(false);
            this.PerfilDropDownMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolStripMenuItem crearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modificarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem desbloquearToolStripMenuItem;
        private DropdownMenuStrip_08YS AdministrativoDropDownMenu;
        private System.Windows.Forms.ToolStripMenuItem gestionUsuarioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bitacoraToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label Titulo;
        private System.Windows.Forms.Panel panelLateral;
        private System.Windows.Forms.Panel panelLogo;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblNombreApellido;
        private System.Windows.Forms.Label lblRolSistema;
        private DropdownMenuStrip_08YS PerfilDropDownMenu;
        private System.Windows.Forms.ToolStripMenuItem cambiarContraseñaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ReLoginToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cerrarSesionToolStripMenuItem1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripMenuItem gestionAccesosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rolesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem familiasToolStripMenuItem;
        private CustomControls.IconComboBox IdiomaCombobox;
        private System.Windows.Forms.Panel panelIdioma;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private FontAwesome.Sharp.IconButton btnPerfil;
        private FontAwesome.Sharp.IconButton btnAdministrativo;
        private FontAwesome.Sharp.IconButton btnReservar;
        private System.Windows.Forms.Panel panel3;
        private FontAwesome.Sharp.IconButton btnCerrarSesion;
        private System.Windows.Forms.ToolStripMenuItem gestionRespaldosToolStripMenuItem;
    }
}

