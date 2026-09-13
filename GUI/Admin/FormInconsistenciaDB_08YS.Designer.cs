namespace GUI_08YS.Admin
{
    partial class FormInconsistenciaDB_08YS
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
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSalir = new FontAwesome.Sharp.IconButton();
            this.btnRestore = new FontAwesome.Sharp.IconButton();
            this.btnRecalcular = new FontAwesome.Sharp.IconButton();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblTitulo);
            this.pnlHeader.Controls.Add(this.iconPictureBox1);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(670, 110);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblTitulo.Location = new System.Drawing.Point(111, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Padding = new System.Windows.Forms.Padding(0, 25, 0, 0);
            this.lblTitulo.Size = new System.Drawing.Size(544, 70);
            this.lblTitulo.TabIndex = 1;
            this.lblTitulo.Tag = "dv_titulo";
            this.lblTitulo.Text = "Inconsistencia en la base de datos";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // iconPictureBox1
            // 
            this.iconPictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.iconPictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.iconPictureBox1.ForeColor = System.Drawing.Color.DarkOrange;
            this.iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.ExclamationTriangle;
            this.iconPictureBox1.IconColor = System.Drawing.Color.DarkOrange;
            this.iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox1.IconSize = 110;
            this.iconPictureBox1.Location = new System.Drawing.Point(0, 0);
            this.iconPictureBox1.Name = "iconPictureBox1";
            this.iconPictureBox1.Size = new System.Drawing.Size(111, 110);
            this.iconPictureBox1.TabIndex = 0;
            this.iconPictureBox1.TabStop = false;
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDescripcion.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescripcion.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblDescripcion.Location = new System.Drawing.Point(0, 110);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Padding = new System.Windows.Forms.Padding(15);
            this.lblDescripcion.Size = new System.Drawing.Size(685, 53);
            this.lblDescripcion.TabIndex = 1;
            this.lblDescripcion.Tag = "dv_descripcion";
            this.lblDescripcion.Text = "Se detectaron modificaciones directas sobre la base de datos. Seleccione una acci" +
    "ón:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.btnSalir, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnRestore, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnRecalcular, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 163);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(670, 67);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // btnSalir
            // 
            this.btnSalir.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalir.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalir.ForeColor = System.Drawing.Color.Gold;
            this.btnSalir.IconChar = FontAwesome.Sharp.IconChar.SignOut;
            this.btnSalir.IconColor = System.Drawing.Color.Gold;
            this.btnSalir.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnSalir.IconSize = 30;
            this.btnSalir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalir.Location = new System.Drawing.Point(451, 2);
            this.btnSalir.Margin = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Padding = new System.Windows.Forms.Padding(5, 0, 20, 0);
            this.btnSalir.Size = new System.Drawing.Size(214, 60);
            this.btnSalir.TabIndex = 6;
            this.btnSalir.Tag = "dv_btn_salir";
            this.btnSalir.Text = "Salir";
            this.btnSalir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSalir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnRestore
            // 
            this.btnRestore.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRestore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestore.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRestore.ForeColor = System.Drawing.Color.Gold;
            this.btnRestore.IconChar = FontAwesome.Sharp.IconChar.Database;
            this.btnRestore.IconColor = System.Drawing.Color.Gold;
            this.btnRestore.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnRestore.IconSize = 30;
            this.btnRestore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRestore.Location = new System.Drawing.Point(228, 2);
            this.btnRestore.Margin = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Padding = new System.Windows.Forms.Padding(5, 0, 20, 0);
            this.btnRestore.Size = new System.Drawing.Size(213, 60);
            this.btnRestore.TabIndex = 5;
            this.btnRestore.Tag = "dv_btn_restaurar";
            this.btnRestore.Text = "Restaurar";
            this.btnRestore.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRestore.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestaurar_Click);
            // 
            // btnRecalcular
            // 
            this.btnRecalcular.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRecalcular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRecalcular.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRecalcular.ForeColor = System.Drawing.Color.Gold;
            this.btnRecalcular.IconChar = FontAwesome.Sharp.IconChar.Sync;
            this.btnRecalcular.IconColor = System.Drawing.Color.Gold;
            this.btnRecalcular.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnRecalcular.IconSize = 30;
            this.btnRecalcular.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRecalcular.Location = new System.Drawing.Point(5, 2);
            this.btnRecalcular.Margin = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.btnRecalcular.Name = "btnRecalcular";
            this.btnRecalcular.Padding = new System.Windows.Forms.Padding(5, 0, 20, 0);
            this.btnRecalcular.Size = new System.Drawing.Size(213, 60);
            this.btnRecalcular.TabIndex = 4;
            this.btnRecalcular.Tag = "dv_btn_recalcular";
            this.btnRecalcular.Text = "Recalcular";
            this.btnRecalcular.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRecalcular.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRecalcular.UseVisualStyleBackColor = true;
            this.btnRecalcular.Click += new System.EventHandler(this.btnRecalcular_Click);
            // 
            // FormInconsistenciaDB_08YS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.ClientSize = new System.Drawing.Size(670, 230);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.lblDescripcion);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormInconsistenciaDB_08YS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inconsistencia detectada";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormInconsistenciaDB_08YS_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private FontAwesome.Sharp.IconButton btnSalir;
        private FontAwesome.Sharp.IconButton btnRestore;
        private FontAwesome.Sharp.IconButton btnRecalcular;
    }
}