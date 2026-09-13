namespace GUI_08YS
{
    partial class FormCambiarContraseña_08YS
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
            this.lblContraseñaActual = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblConfirmarContraseña = new System.Windows.Forms.Label();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.btnCambiarContraseña = new FontAwesome.Sharp.IconButton();
            this.txtConfirmarContraseña = new CustomControls.IconPlaceholderTextBox();
            this.txtNuevaContraseña = new CustomControls.IconPlaceholderTextBox();
            this.txtContraseñaActual = new CustomControls.IconPlaceholderTextBox();
            this.SuspendLayout();
            // 
            // lblContraseñaActual
            // 
            this.lblContraseñaActual.AutoSize = true;
            this.lblContraseñaActual.BackColor = System.Drawing.Color.Transparent;
            this.lblContraseñaActual.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContraseñaActual.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblContraseñaActual.Location = new System.Drawing.Point(92, 72);
            this.lblContraseñaActual.Name = "lblContraseñaActual";
            this.lblContraseñaActual.Size = new System.Drawing.Size(104, 15);
            this.lblContraseñaActual.TabIndex = 3;
            this.lblContraseñaActual.Tag = "lblPasswordActual";
            this.lblContraseñaActual.Text = "Contraseña Actual";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Goldenrod;
            this.label2.Location = new System.Drawing.Point(92, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 15);
            this.label2.TabIndex = 4;
            this.label2.Tag = "lblPasswordNueva";
            this.label2.Text = "Nueva Contraseña";
            // 
            // lblConfirmarContraseña
            // 
            this.lblConfirmarContraseña.AutoSize = true;
            this.lblConfirmarContraseña.BackColor = System.Drawing.Color.Transparent;
            this.lblConfirmarContraseña.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfirmarContraseña.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblConfirmarContraseña.Location = new System.Drawing.Point(92, 261);
            this.lblConfirmarContraseña.Name = "lblConfirmarContraseña";
            this.lblConfirmarContraseña.Size = new System.Drawing.Size(124, 15);
            this.lblConfirmarContraseña.TabIndex = 5;
            this.lblConfirmarContraseña.Tag = "lblConfirmarPassword";
            this.lblConfirmarContraseña.Text = "Confirmar Contraseña";
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.BackColor = System.Drawing.Color.Transparent;
            this.lblUsuario.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuario.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblUsuario.Location = new System.Drawing.Point(92, 41);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(50, 15);
            this.lblUsuario.TabIndex = 6;
            this.lblUsuario.Tag = "lblUsuario";
            this.lblUsuario.Text = "Usuario:";
            // 
            // btnCambiarContraseña
            // 
            this.btnCambiarContraseña.BackColor = System.Drawing.Color.Transparent;
            this.btnCambiarContraseña.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCambiarContraseña.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCambiarContraseña.ForeColor = System.Drawing.Color.Goldenrod;
            this.btnCambiarContraseña.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.btnCambiarContraseña.IconColor = System.Drawing.Color.Gold;
            this.btnCambiarContraseña.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCambiarContraseña.IconSize = 30;
            this.btnCambiarContraseña.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCambiarContraseña.Location = new System.Drawing.Point(95, 338);
            this.btnCambiarContraseña.Margin = new System.Windows.Forms.Padding(2);
            this.btnCambiarContraseña.Name = "btnCambiarContraseña";
            this.btnCambiarContraseña.Padding = new System.Windows.Forms.Padding(4, 0, 15, 0);
            this.btnCambiarContraseña.Size = new System.Drawing.Size(199, 46);
            this.btnCambiarContraseña.TabIndex = 60;
            this.btnCambiarContraseña.Tag = "btnCambiarContraseña";
            this.btnCambiarContraseña.Text = "Cambiar contraseña";
            this.btnCambiarContraseña.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCambiarContraseña.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCambiarContraseña.UseVisualStyleBackColor = false;
            this.btnCambiarContraseña.Click += new System.EventHandler(this.btnCambiarContraseña_Click);
            // 
            // txtConfirmarContraseña
            // 
            this.txtConfirmarContraseña.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.txtConfirmarContraseña.BorderColor = System.Drawing.Color.Goldenrod;
            this.txtConfirmarContraseña.BorderFocusColor = System.Drawing.Color.Goldenrod;
            this.txtConfirmarContraseña.BorderWidth = 2;
            this.txtConfirmarContraseña.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtConfirmarContraseña.IconAlignment = CustomControls.IconTextBoxAlignment.Right;
            this.txtConfirmarContraseña.IconChar = FontAwesome.Sharp.IconChar.EyeSlash;
            this.txtConfirmarContraseña.IconColor = System.Drawing.Color.Goldenrod;
            this.txtConfirmarContraseña.IconColorRight = System.Drawing.Color.DimGray;
            this.txtConfirmarContraseña.IconSize = 25;
            this.txtConfirmarContraseña.Location = new System.Drawing.Point(95, 280);
            this.txtConfirmarContraseña.Margin = new System.Windows.Forms.Padding(2);
            this.txtConfirmarContraseña.MaskedInput = true;
            this.txtConfirmarContraseña.Name = "txtConfirmarContraseña";
            this.txtConfirmarContraseña.PlaceholderColor = System.Drawing.Color.LightGray;
            this.txtConfirmarContraseña.PlaceholderText = "Ingrese su nueva contraseña";
            this.txtConfirmarContraseña.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtConfirmarContraseña.Size = new System.Drawing.Size(199, 32);
            this.txtConfirmarContraseña.TabIndex = 10;
            this.txtConfirmarContraseña.Tag = "txtPwdConfirmar";
            // 
            // txtNuevaContraseña
            // 
            this.txtNuevaContraseña.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.txtNuevaContraseña.BorderColor = System.Drawing.Color.Goldenrod;
            this.txtNuevaContraseña.BorderFocusColor = System.Drawing.Color.Goldenrod;
            this.txtNuevaContraseña.BorderWidth = 2;
            this.txtNuevaContraseña.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtNuevaContraseña.IconAlignment = CustomControls.IconTextBoxAlignment.Right;
            this.txtNuevaContraseña.IconChar = FontAwesome.Sharp.IconChar.EyeSlash;
            this.txtNuevaContraseña.IconColor = System.Drawing.Color.Goldenrod;
            this.txtNuevaContraseña.IconColorRight = System.Drawing.Color.DimGray;
            this.txtNuevaContraseña.IconSize = 25;
            this.txtNuevaContraseña.Location = new System.Drawing.Point(95, 185);
            this.txtNuevaContraseña.Margin = new System.Windows.Forms.Padding(2);
            this.txtNuevaContraseña.MaskedInput = true;
            this.txtNuevaContraseña.Name = "txtNuevaContraseña";
            this.txtNuevaContraseña.PlaceholderColor = System.Drawing.Color.LightGray;
            this.txtNuevaContraseña.PlaceholderText = "Ingrese su nueva contraseña";
            this.txtNuevaContraseña.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtNuevaContraseña.Size = new System.Drawing.Size(199, 32);
            this.txtNuevaContraseña.TabIndex = 9;
            this.txtNuevaContraseña.Tag = "txtPwdNueva";
            // 
            // txtContraseñaActual
            // 
            this.txtContraseñaActual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.txtContraseñaActual.BorderColor = System.Drawing.Color.Goldenrod;
            this.txtContraseñaActual.BorderFocusColor = System.Drawing.Color.Goldenrod;
            this.txtContraseñaActual.BorderWidth = 2;
            this.txtContraseñaActual.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtContraseñaActual.IconAlignment = CustomControls.IconTextBoxAlignment.Right;
            this.txtContraseñaActual.IconChar = FontAwesome.Sharp.IconChar.EyeSlash;
            this.txtContraseñaActual.IconColor = System.Drawing.Color.Goldenrod;
            this.txtContraseñaActual.IconColorRight = System.Drawing.Color.DimGray;
            this.txtContraseñaActual.IconSize = 25;
            this.txtContraseñaActual.Location = new System.Drawing.Point(95, 91);
            this.txtContraseñaActual.Margin = new System.Windows.Forms.Padding(2);
            this.txtContraseñaActual.MaskedInput = true;
            this.txtContraseñaActual.Name = "txtContraseñaActual";
            this.txtContraseñaActual.PlaceholderColor = System.Drawing.Color.LightGray;
            this.txtContraseñaActual.PlaceholderText = "Ingrese su contraseña actual";
            this.txtContraseñaActual.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtContraseñaActual.Size = new System.Drawing.Size(199, 32);
            this.txtContraseñaActual.TabIndex = 8;
            this.txtContraseñaActual.Tag = "txtPwdActual";
            // 
            // FormCambiarContraseña_08YS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::GUI_08YS.Properties.Resources.ChatGPT_Image_19_may_2026__22_23_23;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(347, 414);
            this.Controls.Add(this.btnCambiarContraseña);
            this.Controls.Add(this.txtConfirmarContraseña);
            this.Controls.Add(this.txtNuevaContraseña);
            this.Controls.Add(this.txtContraseñaActual);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.lblConfirmarContraseña);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblContraseñaActual);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormCambiarContraseña_08YS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormCambiarContraseña";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblContraseñaActual;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblConfirmarContraseña;
        private System.Windows.Forms.Label lblUsuario;
        private CustomControls.IconPlaceholderTextBox txtContraseñaActual;
        private CustomControls.IconPlaceholderTextBox txtNuevaContraseña;
        private CustomControls.IconPlaceholderTextBox txtConfirmarContraseña;
        private FontAwesome.Sharp.IconButton btnCambiarContraseña;
    }
}