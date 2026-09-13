namespace GUI_08YS.Admin
{
    partial class FormGestionRespaldos_08YS
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRealizarBackUp = new FontAwesome.Sharp.IconButton();
            this.lblNombreArchivo = new System.Windows.Forms.Label();
            this.lblNombreLabel = new System.Windows.Forms.Label();
            this.pnlCarpeta = new System.Windows.Forms.Panel();
            this.txtCarpetaDestino = new CustomControls.IconPlaceholderTextBox();
            this.btnSeleccionarCarpetaBackup = new FontAwesome.Sharp.IconButton();
            this.lblBackupDesc = new System.Windows.Forms.Label();
            this.lblBackupTitulo = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnRealizarRestore = new FontAwesome.Sharp.IconButton();
            this.pnlInfoArchivo = new System.Windows.Forms.Panel();
            this.lblInfoArchivo = new System.Windows.Forms.Label();
            this.iconPictureBox3 = new FontAwesome.Sharp.IconPictureBox();
            this.pnlArchivo = new System.Windows.Forms.Panel();
            this.txtArchivoRestore = new CustomControls.IconPlaceholderTextBox();
            this.btnSeleccionarArchivoRestore = new FontAwesome.Sharp.IconButton();
            this.lblRestoreLabel = new System.Windows.Forms.Label();
            this.pnlWarning = new System.Windows.Forms.Panel();
            this.lblWarning = new System.Windows.Forms.Label();
            this.iconPictureBox2 = new FontAwesome.Sharp.IconPictureBox();
            this.lblRestoreTitulo = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlCarpeta.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnlInfoArchivo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox3)).BeginInit();
            this.pnlArchivo.SuspendLayout();
            this.pnlWarning.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.Transparent;
            this.pnlHeader.Controls.Add(this.lblTitulo);
            this.pnlHeader.Controls.Add(this.iconPictureBox1);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(20, 15, 20, 5);
            this.pnlHeader.Size = new System.Drawing.Size(1470, 120);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblTitulo.Location = new System.Drawing.Point(120, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(496, 62);
            this.lblTitulo.TabIndex = 1;
            this.lblTitulo.Tag = "backup_titulo";
            this.lblTitulo.Text = "Gestión de Respaldos";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // iconPictureBox1
            // 
            this.iconPictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.iconPictureBox1.ForeColor = System.Drawing.Color.Goldenrod;
            this.iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.Database;
            this.iconPictureBox1.IconColor = System.Drawing.Color.Goldenrod;
            this.iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox1.IconSize = 100;
            this.iconPictureBox1.Location = new System.Drawing.Point(20, 15);
            this.iconPictureBox1.Name = "iconPictureBox1";
            this.iconPictureBox1.Size = new System.Drawing.Size(100, 100);
            this.iconPictureBox1.TabIndex = 0;
            this.iconPictureBox1.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 120);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.lblNombreArchivo);
            this.splitContainer1.Panel1.Controls.Add(this.lblNombreLabel);
            this.splitContainer1.Panel1.Controls.Add(this.pnlCarpeta);
            this.splitContainer1.Panel1.Controls.Add(this.lblBackupDesc);
            this.splitContainer1.Panel1.Controls.Add(this.lblBackupTitulo);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(30, 20, 20, 20);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel3);
            this.splitContainer1.Panel2.Controls.Add(this.pnlInfoArchivo);
            this.splitContainer1.Panel2.Controls.Add(this.pnlArchivo);
            this.splitContainer1.Panel2.Controls.Add(this.lblRestoreLabel);
            this.splitContainer1.Panel2.Controls.Add(this.pnlWarning);
            this.splitContainer1.Panel2.Controls.Add(this.lblRestoreTitulo);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(20, 20, 30, 20);
            this.splitContainer1.Size = new System.Drawing.Size(1470, 800);
            this.splitContainer1.SplitterDistance = 720;
            this.splitContainer1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRealizarBackUp);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(30, 248);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.panel1.Size = new System.Drawing.Size(670, 80);
            this.panel1.TabIndex = 5;
            // 
            // btnRealizarBackUp
            // 
            this.btnRealizarBackUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.btnRealizarBackUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRealizarBackUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRealizarBackUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRealizarBackUp.ForeColor = System.Drawing.Color.Goldenrod;
            this.btnRealizarBackUp.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.btnRealizarBackUp.IconColor = System.Drawing.Color.Gold;
            this.btnRealizarBackUp.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnRealizarBackUp.IconSize = 38;
            this.btnRealizarBackUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRealizarBackUp.Location = new System.Drawing.Point(0, 30);
            this.btnRealizarBackUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRealizarBackUp.Name = "btnRealizarBackUp";
            this.btnRealizarBackUp.Padding = new System.Windows.Forms.Padding(5, 0, 10, 0);
            this.btnRealizarBackUp.Size = new System.Drawing.Size(670, 50);
            this.btnRealizarBackUp.TabIndex = 61;
            this.btnRealizarBackUp.Tag = "backup_btn_realizar";
            this.btnRealizarBackUp.Text = "Realizar BackUp";
            this.btnRealizarBackUp.UseVisualStyleBackColor = false;
            this.btnRealizarBackUp.Click += new System.EventHandler(this.btnRealizarBackUp_Click);
            // 
            // lblNombreArchivo
            // 
            this.lblNombreArchivo.AutoSize = true;
            this.lblNombreArchivo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNombreArchivo.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombreArchivo.ForeColor = System.Drawing.Color.LightGoldenrodYellow;
            this.lblNombreArchivo.Location = new System.Drawing.Point(30, 210);
            this.lblNombreArchivo.Name = "lblNombreArchivo";
            this.lblNombreArchivo.Padding = new System.Windows.Forms.Padding(10, 10, 0, 0);
            this.lblNombreArchivo.Size = new System.Drawing.Size(165, 38);
            this.lblNombreArchivo.TabIndex = 4;
            this.lblNombreArchivo.Text = "nombre archivo";
            // 
            // lblNombreLabel
            // 
            this.lblNombreLabel.AutoSize = true;
            this.lblNombreLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblNombreLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombreLabel.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblNombreLabel.Location = new System.Drawing.Point(30, 153);
            this.lblNombreLabel.Name = "lblNombreLabel";
            this.lblNombreLabel.Padding = new System.Windows.Forms.Padding(0, 25, 0, 0);
            this.lblNombreLabel.Size = new System.Drawing.Size(308, 57);
            this.lblNombreLabel.TabIndex = 3;
            this.lblNombreLabel.Tag = "backup_nombre_label";
            this.lblNombreLabel.Text = "El nombre del archivo será:";
            // 
            // pnlCarpeta
            // 
            this.pnlCarpeta.Controls.Add(this.txtCarpetaDestino);
            this.pnlCarpeta.Controls.Add(this.btnSeleccionarCarpetaBackup);
            this.pnlCarpeta.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCarpeta.Location = new System.Drawing.Point(30, 98);
            this.pnlCarpeta.Name = "pnlCarpeta";
            this.pnlCarpeta.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnlCarpeta.Size = new System.Drawing.Size(670, 55);
            this.pnlCarpeta.TabIndex = 2;
            // 
            // txtCarpetaDestino
            // 
            this.txtCarpetaDestino.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.txtCarpetaDestino.BorderColor = System.Drawing.Color.Goldenrod;
            this.txtCarpetaDestino.BorderFocusColor = System.Drawing.Color.Goldenrod;
            this.txtCarpetaDestino.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCarpetaDestino.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCarpetaDestino.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtCarpetaDestino.IconAlignment = CustomControls.IconTextBoxAlignment.Right;
            this.txtCarpetaDestino.IconColor = System.Drawing.Color.Goldenrod;
            this.txtCarpetaDestino.IconColorRight = System.Drawing.Color.DimGray;
            this.txtCarpetaDestino.IconPadding = 10;
            this.txtCarpetaDestino.IconSize = 30;
            this.txtCarpetaDestino.Location = new System.Drawing.Point(0, 10);
            this.txtCarpetaDestino.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCarpetaDestino.Name = "txtCarpetaDestino";
            this.txtCarpetaDestino.PlaceholderColor = System.Drawing.Color.LightGray;
            this.txtCarpetaDestino.ReadOnly = true;
            this.txtCarpetaDestino.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCarpetaDestino.Size = new System.Drawing.Size(615, 45);
            this.txtCarpetaDestino.TabIndex = 70;
            this.txtCarpetaDestino.TextLeftPadding = 20;
            // 
            // btnSeleccionarCarpetaBackup
            // 
            this.btnSeleccionarCarpetaBackup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.btnSeleccionarCarpetaBackup.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSeleccionarCarpetaBackup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSeleccionarCarpetaBackup.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSeleccionarCarpetaBackup.ForeColor = System.Drawing.Color.Goldenrod;
            this.btnSeleccionarCarpetaBackup.IconChar = FontAwesome.Sharp.IconChar.FolderOpen;
            this.btnSeleccionarCarpetaBackup.IconColor = System.Drawing.Color.Gold;
            this.btnSeleccionarCarpetaBackup.IconFont = FontAwesome.Sharp.IconFont.Regular;
            this.btnSeleccionarCarpetaBackup.IconSize = 38;
            this.btnSeleccionarCarpetaBackup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSeleccionarCarpetaBackup.Location = new System.Drawing.Point(615, 10);
            this.btnSeleccionarCarpetaBackup.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSeleccionarCarpetaBackup.Name = "btnSeleccionarCarpetaBackup";
            this.btnSeleccionarCarpetaBackup.Padding = new System.Windows.Forms.Padding(5, 0, 20, 0);
            this.btnSeleccionarCarpetaBackup.Size = new System.Drawing.Size(55, 45);
            this.btnSeleccionarCarpetaBackup.TabIndex = 62;
            this.btnSeleccionarCarpetaBackup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSeleccionarCarpetaBackup.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSeleccionarCarpetaBackup.UseVisualStyleBackColor = false;
            this.btnSeleccionarCarpetaBackup.Click += new System.EventHandler(this.btnSeleccionarCarpetaBackup_Click);
            // 
            // lblBackupDesc
            // 
            this.lblBackupDesc.AutoSize = true;
            this.lblBackupDesc.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBackupDesc.Font = new System.Drawing.Font("Segoe UI Semibold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBackupDesc.ForeColor = System.Drawing.Color.LightGoldenrodYellow;
            this.lblBackupDesc.Location = new System.Drawing.Point(30, 58);
            this.lblBackupDesc.Name = "lblBackupDesc";
            this.lblBackupDesc.Padding = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.lblBackupDesc.Size = new System.Drawing.Size(519, 40);
            this.lblBackupDesc.TabIndex = 1;
            this.lblBackupDesc.Tag = "backup_crear_desc";
            this.lblBackupDesc.Text = "Seleccioná la carpeta donde se guardará el archivo de backup";
            // 
            // lblBackupTitulo
            // 
            this.lblBackupTitulo.AutoSize = true;
            this.lblBackupTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBackupTitulo.Font = new System.Drawing.Font("Segoe UI Semibold", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBackupTitulo.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblBackupTitulo.Location = new System.Drawing.Point(30, 20);
            this.lblBackupTitulo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 50);
            this.lblBackupTitulo.Name = "lblBackupTitulo";
            this.lblBackupTitulo.Size = new System.Drawing.Size(202, 38);
            this.lblBackupTitulo.TabIndex = 0;
            this.lblBackupTitulo.Tag = "backup_crear_titulo";
            this.lblBackupTitulo.Text = "Crear respaldo";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnRealizarRestore);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(20, 355);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.panel3.Size = new System.Drawing.Size(696, 80);
            this.panel3.TabIndex = 7;
            // 
            // btnRealizarRestore
            // 
            this.btnRealizarRestore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.btnRealizarRestore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRealizarRestore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRealizarRestore.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRealizarRestore.ForeColor = System.Drawing.Color.Goldenrod;
            this.btnRealizarRestore.IconChar = FontAwesome.Sharp.IconChar.History;
            this.btnRealizarRestore.IconColor = System.Drawing.Color.Gold;
            this.btnRealizarRestore.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnRealizarRestore.IconSize = 38;
            this.btnRealizarRestore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRealizarRestore.Location = new System.Drawing.Point(0, 30);
            this.btnRealizarRestore.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRealizarRestore.Name = "btnRealizarRestore";
            this.btnRealizarRestore.Padding = new System.Windows.Forms.Padding(5, 0, 10, 0);
            this.btnRealizarRestore.Size = new System.Drawing.Size(696, 50);
            this.btnRealizarRestore.TabIndex = 61;
            this.btnRealizarRestore.Tag = "backup_btn_restaurar";
            this.btnRealizarRestore.Text = "Restaurar Backup";
            this.btnRealizarRestore.UseVisualStyleBackColor = false;
            this.btnRealizarRestore.Click += new System.EventHandler(this.btnRealizarRestore_Click);
            // 
            // pnlInfoArchivo
            // 
            this.pnlInfoArchivo.Controls.Add(this.lblInfoArchivo);
            this.pnlInfoArchivo.Controls.Add(this.iconPictureBox3);
            this.pnlInfoArchivo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfoArchivo.Location = new System.Drawing.Point(20, 280);
            this.pnlInfoArchivo.Name = "pnlInfoArchivo";
            this.pnlInfoArchivo.Padding = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.pnlInfoArchivo.Size = new System.Drawing.Size(696, 75);
            this.pnlInfoArchivo.TabIndex = 6;
            this.pnlInfoArchivo.Visible = false;
            // 
            // lblInfoArchivo
            // 
            this.lblInfoArchivo.AutoSize = true;
            this.lblInfoArchivo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInfoArchivo.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoArchivo.ForeColor = System.Drawing.SystemColors.ScrollBar;
            this.lblInfoArchivo.Location = new System.Drawing.Point(60, 15);
            this.lblInfoArchivo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 50);
            this.lblInfoArchivo.Name = "lblInfoArchivo";
            this.lblInfoArchivo.Size = new System.Drawing.Size(119, 28);
            this.lblInfoArchivo.TabIndex = 2;
            this.lblInfoArchivo.Text = "info archivo";
            // 
            // iconPictureBox3
            // 
            this.iconPictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.iconPictureBox3.ForeColor = System.Drawing.Color.Goldenrod;
            this.iconPictureBox3.IconChar = FontAwesome.Sharp.IconChar.CircleInfo;
            this.iconPictureBox3.IconColor = System.Drawing.Color.Goldenrod;
            this.iconPictureBox3.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox3.IconSize = 60;
            this.iconPictureBox3.Location = new System.Drawing.Point(0, 15);
            this.iconPictureBox3.Name = "iconPictureBox3";
            this.iconPictureBox3.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.iconPictureBox3.Size = new System.Drawing.Size(60, 60);
            this.iconPictureBox3.TabIndex = 1;
            this.iconPictureBox3.TabStop = false;
            // 
            // pnlArchivo
            // 
            this.pnlArchivo.Controls.Add(this.txtArchivoRestore);
            this.pnlArchivo.Controls.Add(this.btnSeleccionarArchivoRestore);
            this.pnlArchivo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlArchivo.Location = new System.Drawing.Point(20, 225);
            this.pnlArchivo.Name = "pnlArchivo";
            this.pnlArchivo.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnlArchivo.Size = new System.Drawing.Size(696, 55);
            this.pnlArchivo.TabIndex = 5;
            // 
            // txtArchivoRestore
            // 
            this.txtArchivoRestore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.txtArchivoRestore.BorderColor = System.Drawing.Color.Goldenrod;
            this.txtArchivoRestore.BorderFocusColor = System.Drawing.Color.Goldenrod;
            this.txtArchivoRestore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtArchivoRestore.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtArchivoRestore.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtArchivoRestore.IconAlignment = CustomControls.IconTextBoxAlignment.Right;
            this.txtArchivoRestore.IconColor = System.Drawing.Color.Goldenrod;
            this.txtArchivoRestore.IconColorRight = System.Drawing.Color.DimGray;
            this.txtArchivoRestore.IconPadding = 10;
            this.txtArchivoRestore.IconSize = 30;
            this.txtArchivoRestore.Location = new System.Drawing.Point(0, 10);
            this.txtArchivoRestore.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtArchivoRestore.Name = "txtArchivoRestore";
            this.txtArchivoRestore.PlaceholderColor = System.Drawing.Color.LightGray;
            this.txtArchivoRestore.ReadOnly = true;
            this.txtArchivoRestore.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtArchivoRestore.Size = new System.Drawing.Size(641, 45);
            this.txtArchivoRestore.TabIndex = 70;
            this.txtArchivoRestore.TextLeftPadding = 20;
            // 
            // btnSeleccionarArchivoRestore
            // 
            this.btnSeleccionarArchivoRestore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.btnSeleccionarArchivoRestore.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSeleccionarArchivoRestore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSeleccionarArchivoRestore.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSeleccionarArchivoRestore.ForeColor = System.Drawing.Color.Goldenrod;
            this.btnSeleccionarArchivoRestore.IconChar = FontAwesome.Sharp.IconChar.FolderOpen;
            this.btnSeleccionarArchivoRestore.IconColor = System.Drawing.Color.Gold;
            this.btnSeleccionarArchivoRestore.IconFont = FontAwesome.Sharp.IconFont.Regular;
            this.btnSeleccionarArchivoRestore.IconSize = 38;
            this.btnSeleccionarArchivoRestore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSeleccionarArchivoRestore.Location = new System.Drawing.Point(641, 10);
            this.btnSeleccionarArchivoRestore.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSeleccionarArchivoRestore.Name = "btnSeleccionarArchivoRestore";
            this.btnSeleccionarArchivoRestore.Padding = new System.Windows.Forms.Padding(5, 0, 20, 0);
            this.btnSeleccionarArchivoRestore.Size = new System.Drawing.Size(55, 45);
            this.btnSeleccionarArchivoRestore.TabIndex = 62;
            this.btnSeleccionarArchivoRestore.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSeleccionarArchivoRestore.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSeleccionarArchivoRestore.UseVisualStyleBackColor = false;
            this.btnSeleccionarArchivoRestore.Click += new System.EventHandler(this.btnSeleccionarArchivoRestore_Click);
            // 
            // lblRestoreLabel
            // 
            this.lblRestoreLabel.AutoSize = true;
            this.lblRestoreLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRestoreLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRestoreLabel.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblRestoreLabel.Location = new System.Drawing.Point(20, 168);
            this.lblRestoreLabel.Name = "lblRestoreLabel";
            this.lblRestoreLabel.Padding = new System.Windows.Forms.Padding(0, 25, 0, 0);
            this.lblRestoreLabel.Size = new System.Drawing.Size(429, 57);
            this.lblRestoreLabel.TabIndex = 4;
            this.lblRestoreLabel.Tag = "backup_restore_label";
            this.lblRestoreLabel.Text = "Seleccioná el archivo de backup (.bak)";
            // 
            // pnlWarning
            // 
            this.pnlWarning.Controls.Add(this.lblWarning);
            this.pnlWarning.Controls.Add(this.iconPictureBox2);
            this.pnlWarning.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlWarning.Location = new System.Drawing.Point(20, 58);
            this.pnlWarning.Name = "pnlWarning";
            this.pnlWarning.Padding = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.pnlWarning.Size = new System.Drawing.Size(696, 110);
            this.pnlWarning.TabIndex = 2;
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblWarning.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarning.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblWarning.Location = new System.Drawing.Point(60, 15);
            this.lblWarning.Margin = new System.Windows.Forms.Padding(3, 0, 3, 50);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(635, 84);
            this.lblWarning.TabIndex = 2;
            this.lblWarning.Tag = "backup_warning";
            this.lblWarning.Text = "Restaurar reemplazará completamente la base de datos actual con el\r\ncontenido del" +
    " backup seleccionado. Esta operación no puede \r\ndeshacerse. El sistema se reinic" +
    "iará automáticamente al finalizar.";
            // 
            // iconPictureBox2
            // 
            this.iconPictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.iconPictureBox2.ForeColor = System.Drawing.Color.Goldenrod;
            this.iconPictureBox2.IconChar = FontAwesome.Sharp.IconChar.ExclamationTriangle;
            this.iconPictureBox2.IconColor = System.Drawing.Color.Goldenrod;
            this.iconPictureBox2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox2.IconSize = 60;
            this.iconPictureBox2.Location = new System.Drawing.Point(0, 15);
            this.iconPictureBox2.Name = "iconPictureBox2";
            this.iconPictureBox2.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.iconPictureBox2.Size = new System.Drawing.Size(60, 95);
            this.iconPictureBox2.TabIndex = 1;
            this.iconPictureBox2.TabStop = false;
            // 
            // lblRestoreTitulo
            // 
            this.lblRestoreTitulo.AutoSize = true;
            this.lblRestoreTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRestoreTitulo.Font = new System.Drawing.Font("Segoe UI Semibold", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRestoreTitulo.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblRestoreTitulo.Location = new System.Drawing.Point(20, 20);
            this.lblRestoreTitulo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 50);
            this.lblRestoreTitulo.Name = "lblRestoreTitulo";
            this.lblRestoreTitulo.Size = new System.Drawing.Size(254, 38);
            this.lblRestoreTitulo.TabIndex = 1;
            this.lblRestoreTitulo.Tag = "backup_restore_titulo";
            this.lblRestoreTitulo.Text = "Restaurar respaldo";
            // 
            // FormGestionRespaldos_08YS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::GUI_08YS.Properties.Resources.BackGroundHorizon;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1470, 920);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormGestionRespaldos_08YS";
            this.Text = "FormGestionRespaldos_08YS";
            this.Load += new System.EventHandler(this.FormGestionRespaldos_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlCarpeta.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.pnlInfoArchivo.ResumeLayout(false);
            this.pnlInfoArchivo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox3)).EndInit();
            this.pnlArchivo.ResumeLayout(false);
            this.pnlWarning.ResumeLayout(false);
            this.pnlWarning.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitulo;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lblBackupTitulo;
        private System.Windows.Forms.Label lblBackupDesc;
        private System.Windows.Forms.Panel pnlCarpeta;
        private FontAwesome.Sharp.IconButton btnSeleccionarCarpetaBackup;
        private CustomControls.IconPlaceholderTextBox txtCarpetaDestino;
        private System.Windows.Forms.Label lblNombreLabel;
        private System.Windows.Forms.Label lblNombreArchivo;
        private System.Windows.Forms.Panel panel1;
        private FontAwesome.Sharp.IconButton btnRealizarBackUp;
        private System.Windows.Forms.Label lblRestoreTitulo;
        private System.Windows.Forms.Panel pnlWarning;
        private System.Windows.Forms.Label lblWarning;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox2;
        private System.Windows.Forms.Label lblRestoreLabel;
        private System.Windows.Forms.Panel pnlArchivo;
        private CustomControls.IconPlaceholderTextBox txtArchivoRestore;
        private FontAwesome.Sharp.IconButton btnSeleccionarArchivoRestore;
        private System.Windows.Forms.Panel pnlInfoArchivo;
        private System.Windows.Forms.Label lblInfoArchivo;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox3;
        private System.Windows.Forms.Panel panel3;
        private FontAwesome.Sharp.IconButton btnRealizarRestore;
    }
}