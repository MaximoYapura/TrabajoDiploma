namespace GUI_08YS
{
    partial class FormLogin_08YS
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnMinimizar = new System.Windows.Forms.Button();
            this.btnMaximizar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.Titulo = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblDescripcionLogin = new System.Windows.Forms.Label();
            this.iconPictureBox2 = new FontAwesome.Sharp.IconPictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btn_Acceder = new FontAwesome.Sharp.IconButton();
            this.lblLogin = new System.Windows.Forms.Label();
            this.IdiomaCombobox = new CustomControls.IconComboBox();
            this.txtPassword = new CustomControls.IconPlaceholderTextBox();
            this.txtUsername = new CustomControls.IconPlaceholderTextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox2)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(20)))), ((int)(((byte)(38)))));
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.btnMinimizar);
            this.panel1.Controls.Add(this.btnMaximizar);
            this.panel1.Controls.Add(this.btnCerrar);
            this.panel1.Controls.Add(this.Titulo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(789, 46);
            this.panel1.TabIndex = 8;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
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
            // btnMinimizar
            // 
            this.btnMinimizar.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimizar.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMinimizar.FlatAppearance.BorderSize = 0;
            this.btnMinimizar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(20)))), ((int)(((byte)(90)))));
            this.btnMinimizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimizar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimizar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnMinimizar.Location = new System.Drawing.Point(672, 0);
            this.btnMinimizar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnMinimizar.Name = "btnMinimizar";
            this.btnMinimizar.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.btnMinimizar.Size = new System.Drawing.Size(39, 46);
            this.btnMinimizar.TabIndex = 7;
            this.btnMinimizar.Text = "—";
            this.btnMinimizar.UseVisualStyleBackColor = false;
            this.btnMinimizar.Visible = false;
            this.btnMinimizar.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnMaximizar
            // 
            this.btnMaximizar.BackColor = System.Drawing.Color.Transparent;
            this.btnMaximizar.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMaximizar.FlatAppearance.BorderSize = 0;
            this.btnMaximizar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(20)))), ((int)(((byte)(90)))));
            this.btnMaximizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximizar.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMaximizar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnMaximizar.Location = new System.Drawing.Point(711, 0);
            this.btnMaximizar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnMaximizar.Name = "btnMaximizar";
            this.btnMaximizar.Padding = new System.Windows.Forms.Padding(3, 0, 0, 7);
            this.btnMaximizar.Size = new System.Drawing.Size(39, 46);
            this.btnMaximizar.TabIndex = 6;
            this.btnMaximizar.Text = "□";
            this.btnMaximizar.UseVisualStyleBackColor = false;
            this.btnMaximizar.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackColor = System.Drawing.Color.Transparent;
            this.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCerrar.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCerrar.FlatAppearance.BorderSize = 0;
            this.btnCerrar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Maroon;
            this.btnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCerrar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnCerrar.Location = new System.Drawing.Point(750, 0);
            this.btnCerrar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Padding = new System.Windows.Forms.Padding(4, 1, 0, 0);
            this.btnCerrar.Size = new System.Drawing.Size(39, 46);
            this.btnCerrar.TabIndex = 5;
            this.btnCerrar.Text = "X";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Titulo
            // 
            this.Titulo.AutoSize = true;
            this.Titulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Titulo.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Titulo.Location = new System.Drawing.Point(88, 14);
            this.Titulo.Name = "Titulo";
            this.Titulo.Size = new System.Drawing.Size(173, 15);
            this.Titulo.TabIndex = 0;
            this.Titulo.Text = "Login - Horizon Hotel && Resort";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.WindowText;
            this.panel3.Controls.Add(this.lblDescripcionLogin);
            this.panel3.Controls.Add(this.iconPictureBox2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 388);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(789, 62);
            this.panel3.TabIndex = 10;
            // 
            // lblDescripcionLogin
            // 
            this.lblDescripcionLogin.AutoSize = true;
            this.lblDescripcionLogin.BackColor = System.Drawing.Color.Transparent;
            this.lblDescripcionLogin.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescripcionLogin.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblDescripcionLogin.Location = new System.Drawing.Point(301, 39);
            this.lblDescripcionLogin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDescripcionLogin.Name = "lblDescripcionLogin";
            this.lblDescripcionLogin.Size = new System.Drawing.Size(186, 20);
            this.lblDescripcionLogin.TabIndex = 4;
            this.lblDescripcionLogin.Tag = "lblDescripcionLogin";
            this.lblDescripcionLogin.Text = "Excelencia en cada estadía";
            // 
            // iconPictureBox2
            // 
            this.iconPictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox2.ForeColor = System.Drawing.Color.Goldenrod;
            this.iconPictureBox2.IconChar = FontAwesome.Sharp.IconChar.ConciergeBell;
            this.iconPictureBox2.IconColor = System.Drawing.Color.Goldenrod;
            this.iconPictureBox2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox2.IconSize = 48;
            this.iconPictureBox2.Location = new System.Drawing.Point(371, 0);
            this.iconPictureBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.iconPictureBox2.Name = "iconPictureBox2";
            this.iconPictureBox2.Size = new System.Drawing.Size(48, 48);
            this.iconPictureBox2.TabIndex = 0;
            this.iconPictureBox2.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.iconPictureBox1);
            this.panel2.Controls.Add(this.pictureBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.ForeColor = System.Drawing.Color.Goldenrod;
            this.panel2.Location = new System.Drawing.Point(0, 46);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(264, 342);
            this.panel2.TabIndex = 11;
            // 
            // iconPictureBox1
            // 
            this.iconPictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(18)))), ((int)(((byte)(46)))));
            this.iconPictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.iconPictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iconPictureBox1.ForeColor = System.Drawing.Color.Goldenrod;
            this.iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.UserCircle;
            this.iconPictureBox1.IconColor = System.Drawing.Color.Goldenrod;
            this.iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.iconPictureBox1.IconSize = 209;
            this.iconPictureBox1.Location = new System.Drawing.Point(0, 133);
            this.iconPictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.iconPictureBox1.Name = "iconPictureBox1";
            this.iconPictureBox1.Size = new System.Drawing.Size(264, 209);
            this.iconPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.iconPictureBox1.TabIndex = 14;
            this.iconPictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(18)))), ((int)(((byte)(46)))));
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox2.Image = global::GUI_08YS.Properties.Resources.HorizonLogoPuro1;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(264, 133);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 13;
            this.pictureBox2.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.IdiomaCombobox);
            this.panel4.Controls.Add(this.btn_Acceder);
            this.panel4.Controls.Add(this.lblLogin);
            this.panel4.Controls.Add(this.txtPassword);
            this.panel4.Controls.Add(this.txtUsername);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(264, 46);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(525, 342);
            this.panel4.TabIndex = 12;
            // 
            // btn_Acceder
            // 
            this.btn_Acceder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Acceder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(7)))), ((int)(((byte)(40)))));
            this.btn_Acceder.FlatAppearance.BorderSize = 2;
            this.btn_Acceder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Acceder.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Acceder.ForeColor = System.Drawing.Color.Goldenrod;
            this.btn_Acceder.IconChar = FontAwesome.Sharp.IconChar.SignIn;
            this.btn_Acceder.IconColor = System.Drawing.Color.Goldenrod;
            this.btn_Acceder.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btn_Acceder.IconSize = 40;
            this.btn_Acceder.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_Acceder.Location = new System.Drawing.Point(103, 270);
            this.btn_Acceder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Acceder.Name = "btn_Acceder";
            this.btn_Acceder.Padding = new System.Windows.Forms.Padding(55, 0, 20, 0);
            this.btn_Acceder.Size = new System.Drawing.Size(320, 55);
            this.btn_Acceder.TabIndex = 3;
            this.btn_Acceder.Tag = "btn_Acceder";
            this.btn_Acceder.Text = "Acceder";
            this.btn_Acceder.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btn_Acceder.UseVisualStyleBackColor = false;
            this.btn_Acceder.Click += new System.EventHandler(this.btnAcceder_Click);
            // 
            // lblLogin
            // 
            this.lblLogin.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblLogin.AutoSize = true;
            this.lblLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogin.ForeColor = System.Drawing.Color.Gold;
            this.lblLogin.Location = new System.Drawing.Point(192, 30);
            this.lblLogin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(142, 55);
            this.lblLogin.TabIndex = 7;
            this.lblLogin.Text = "Login";
            // 
            // IdiomaCombobox
            // 
            this.IdiomaCombobox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.IdiomaCombobox.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.IdiomaCombobox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.IdiomaCombobox.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(149)))), ((int)(((byte)(237)))));
            this.IdiomaCombobox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.IdiomaCombobox.DropDownBackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.IdiomaCombobox.DropDownBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(140)))), ((int)(((byte)(140)))), ((int)(((byte)(140)))));
            this.IdiomaCombobox.DropDownForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.IdiomaCombobox.DropDownHighlightBackColor = System.Drawing.Color.Goldenrod;
            this.IdiomaCombobox.DropDownHighlightForeColor = System.Drawing.Color.Black;
            this.IdiomaCombobox.DropDownItemHeight = 30;
            this.IdiomaCombobox.ForeColor = System.Drawing.Color.Gold;
            this.IdiomaCombobox.IconChar = FontAwesome.Sharp.IconChar.Earth;
            this.IdiomaCombobox.IconColor = System.Drawing.Color.Goldenrod;
            this.IdiomaCombobox.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.IdiomaCombobox.IconPadding = 5;
            this.IdiomaCombobox.IconSize = 24;
            this.IdiomaCombobox.Location = new System.Drawing.Point(387, 6);
            this.IdiomaCombobox.Margin = new System.Windows.Forms.Padding(4);
            this.IdiomaCombobox.Name = "IdiomaCombobox";
            this.IdiomaCombobox.SelectedItem = null;
            this.IdiomaCombobox.SelectedValue = null;
            this.IdiomaCombobox.Size = new System.Drawing.Size(135, 30);
            this.IdiomaCombobox.TabIndex = 4;
            this.IdiomaCombobox.SelectedIndexChanged += new System.EventHandler(this.IdiomaComboBox_SelectedIndexChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.txtPassword.BorderColor = System.Drawing.Color.Goldenrod;
            this.txtPassword.BorderFocusColor = System.Drawing.Color.Goldenrod;
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtPassword.IconChar = FontAwesome.Sharp.IconChar.Lock;
            this.txtPassword.IconCharRight = FontAwesome.Sharp.IconChar.EyeSlash;
            this.txtPassword.IconColor = System.Drawing.Color.Goldenrod;
            this.txtPassword.IconColorRight = System.Drawing.Color.Goldenrod;
            this.txtPassword.IconSize = 24;
            this.txtPassword.IconSizeRight = 30;
            this.txtPassword.Location = new System.Drawing.Point(103, 194);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPassword.MaskedInput = true;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtPassword.PlaceholderText = "Contraseña";
            this.txtPassword.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtPassword.Size = new System.Drawing.Size(320, 41);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.Tag = "txtPassword";
            this.txtPassword.IconRightClick += new System.EventHandler(this.txtPassword_IconRightClick);
            // 
            // txtUsername
            // 
            this.txtUsername.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUsername.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.txtUsername.BorderColor = System.Drawing.Color.Goldenrod;
            this.txtUsername.BorderFocusColor = System.Drawing.Color.Goldenrod;
            this.txtUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtUsername.IconChar = FontAwesome.Sharp.IconChar.User;
            this.txtUsername.IconColor = System.Drawing.Color.Goldenrod;
            this.txtUsername.IconColorRight = System.Drawing.Color.DimGray;
            this.txtUsername.IconSize = 24;
            this.txtUsername.Location = new System.Drawing.Point(103, 119);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.PlaceholderColor = System.Drawing.Color.Gray;
            this.txtUsername.PlaceholderText = "Username";
            this.txtUsername.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtUsername.Size = new System.Drawing.Size(320, 41);
            this.txtUsername.TabIndex = 1;
            this.txtUsername.Tag = "txtUsername";
            // 
            // FormLogin_08YS
            // 
            this.AcceptButton = this.btn_Acceder;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
            this.CancelButton = this.btnCerrar;
            this.ClientSize = new System.Drawing.Size(789, 450);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormLogin_08YS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormLogin";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox2)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnMinimizar;
        private System.Windows.Forms.Button btnMaximizar;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Label Titulo;
        private System.Windows.Forms.Panel panel3;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox2;
        private System.Windows.Forms.Panel panel2;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lblDescripcionLogin;
        private System.Windows.Forms.Panel panel4;
        private CustomControls.IconComboBox IdiomaCombobox;
        private FontAwesome.Sharp.IconButton btn_Acceder;
        private System.Windows.Forms.Label lblLogin;
        private CustomControls.IconPlaceholderTextBox txtPassword;
        private CustomControls.IconPlaceholderTextBox txtUsername;
    }
}