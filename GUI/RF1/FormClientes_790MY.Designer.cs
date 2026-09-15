namespace GUI_08YS.RF1
{
    partial class FormClientes_790MY
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
            this.dgvClientes_790MY = new System.Windows.Forms.DataGridView();
            this.txtDni_790MY = new System.Windows.Forms.TextBox();
            this.txtNombre_790MY = new System.Windows.Forms.TextBox();
            this.txtApellido_790MY = new System.Windows.Forms.TextBox();
            this.txtEmail_790MY = new System.Windows.Forms.TextBox();
            this.txtTelefono_790MY = new System.Windows.Forms.TextBox();
            this.btnLimpiar_790MY = new System.Windows.Forms.Button();
            this.btnRegistrar_790MY = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes_790MY)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvClientes_790MY
            // 
            this.dgvClientes_790MY.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClientes_790MY.Location = new System.Drawing.Point(371, 107);
            this.dgvClientes_790MY.Name = "dgvClientes_790MY";
            this.dgvClientes_790MY.Size = new System.Drawing.Size(240, 150);
            this.dgvClientes_790MY.TabIndex = 0;
            // 
            // txtDni_790MY
            // 
            this.txtDni_790MY.Location = new System.Drawing.Point(82, 95);
            this.txtDni_790MY.Name = "txtDni_790MY";
            this.txtDni_790MY.Size = new System.Drawing.Size(197, 20);
            this.txtDni_790MY.TabIndex = 1;
            // 
            // txtNombre_790MY
            // 
            this.txtNombre_790MY.Location = new System.Drawing.Point(82, 151);
            this.txtNombre_790MY.Name = "txtNombre_790MY";
            this.txtNombre_790MY.Size = new System.Drawing.Size(197, 20);
            this.txtNombre_790MY.TabIndex = 2;
            // 
            // txtApellido_790MY
            // 
            this.txtApellido_790MY.Location = new System.Drawing.Point(82, 209);
            this.txtApellido_790MY.Name = "txtApellido_790MY";
            this.txtApellido_790MY.Size = new System.Drawing.Size(197, 20);
            this.txtApellido_790MY.TabIndex = 3;
            // 
            // txtEmail_790MY
            // 
            this.txtEmail_790MY.Location = new System.Drawing.Point(82, 272);
            this.txtEmail_790MY.Name = "txtEmail_790MY";
            this.txtEmail_790MY.Size = new System.Drawing.Size(197, 20);
            this.txtEmail_790MY.TabIndex = 4;
            // 
            // txtTelefono_790MY
            // 
            this.txtTelefono_790MY.Location = new System.Drawing.Point(82, 330);
            this.txtTelefono_790MY.Name = "txtTelefono_790MY";
            this.txtTelefono_790MY.Size = new System.Drawing.Size(197, 20);
            this.txtTelefono_790MY.TabIndex = 5;
            // 
            // btnLimpiar_790MY
            // 
            this.btnLimpiar_790MY.Location = new System.Drawing.Point(461, 330);
            this.btnLimpiar_790MY.Name = "btnLimpiar_790MY";
            this.btnLimpiar_790MY.Size = new System.Drawing.Size(91, 26);
            this.btnLimpiar_790MY.TabIndex = 6;
            this.btnLimpiar_790MY.Text = "button1";
            this.btnLimpiar_790MY.UseVisualStyleBackColor = true;
            this.btnLimpiar_790MY.Click += new System.EventHandler(this.btnLimpiar_790MY_Click);
            // 
            // btnRegistrar_790MY
            // 
            this.btnRegistrar_790MY.Location = new System.Drawing.Point(461, 384);
            this.btnRegistrar_790MY.Name = "btnRegistrar_790MY";
            this.btnRegistrar_790MY.Size = new System.Drawing.Size(91, 26);
            this.btnRegistrar_790MY.TabIndex = 7;
            this.btnRegistrar_790MY.Text = "button1";
            this.btnRegistrar_790MY.UseVisualStyleBackColor = true;
            this.btnRegistrar_790MY.Click += new System.EventHandler(this.btnRegistrar_790MY_Click);
            // 
            // FormClientes_790MY
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnRegistrar_790MY);
            this.Controls.Add(this.btnLimpiar_790MY);
            this.Controls.Add(this.txtTelefono_790MY);
            this.Controls.Add(this.txtEmail_790MY);
            this.Controls.Add(this.txtApellido_790MY);
            this.Controls.Add(this.txtNombre_790MY);
            this.Controls.Add(this.txtDni_790MY);
            this.Controls.Add(this.dgvClientes_790MY);
            this.Name = "FormClientes_790MY";
            this.Text = "FormClientes_790MY";
            this.Load += new System.EventHandler(this.FormClientes_790MY_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes_790MY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvClientes_790MY;
        private System.Windows.Forms.TextBox txtDni_790MY;
        private System.Windows.Forms.TextBox txtNombre_790MY;
        private System.Windows.Forms.TextBox txtApellido_790MY;
        private System.Windows.Forms.TextBox txtEmail_790MY;
        private System.Windows.Forms.TextBox txtTelefono_790MY;
        private System.Windows.Forms.Button btnLimpiar_790MY;
        private System.Windows.Forms.Button btnRegistrar_790MY;
    }
}