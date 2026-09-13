namespace GUI_08YS.Admin
{
    partial class FormGestionAcceso_08YS
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGestionAcceso_08YS));
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.iconPictureBox = new FontAwesome.Sharp.IconPictureBox();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnEliminar = new FontAwesome.Sharp.IconButton();
            this.btnModificar = new FontAwesome.Sharp.IconButton();
            this.btnCrear = new FontAwesome.Sharp.IconButton();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.dgvEntidades = new System.Windows.Forms.DataGridView();
            this.colTipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.trvDetalle = new System.Windows.Forms.TreeView();
            this.lblDetalle = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.pnlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEntidades)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.Transparent;
            this.pnlHeader.Controls.Add(this.lblTitulo);
            this.pnlHeader.Controls.Add(this.iconPictureBox);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(25, 15, 20, 5);
            this.pnlHeader.Size = new System.Drawing.Size(1469, 150);
            this.pnlHeader.TabIndex = 58;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblTitulo.Location = new System.Drawing.Point(145, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(194, 62);
            this.lblTitulo.TabIndex = 1;
            this.lblTitulo.Tag = "lblGestion";
            this.lblTitulo.Text = "Gestion";
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.iconPictureBox.ForeColor = System.Drawing.Color.Goldenrod;
            this.iconPictureBox.IconChar = FontAwesome.Sharp.IconChar.None;
            this.iconPictureBox.IconColor = System.Drawing.Color.Goldenrod;
            this.iconPictureBox.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox.IconSize = 120;
            this.iconPictureBox.Location = new System.Drawing.Point(25, 15);
            this.iconPictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.iconPictureBox.Name = "iconPictureBox";
            this.iconPictureBox.Size = new System.Drawing.Size(120, 130);
            this.iconPictureBox.TabIndex = 0;
            this.iconPictureBox.TabStop = false;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.Transparent;
            this.pnlBottom.Controls.Add(this.btnEliminar);
            this.pnlBottom.Controls.Add(this.btnModificar);
            this.pnlBottom.Controls.Add(this.btnCrear);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 841);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Padding = new System.Windows.Forms.Padding(0, 10, 25, 10);
            this.pnlBottom.Size = new System.Drawing.Size(1469, 80);
            this.pnlBottom.TabIndex = 59;
            // 
            // btnEliminar
            // 
            this.btnEliminar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEliminar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEliminar.ForeColor = System.Drawing.Color.Goldenrod;
            this.btnEliminar.IconChar = FontAwesome.Sharp.IconChar.Trash;
            this.btnEliminar.IconColor = System.Drawing.Color.Gold;
            this.btnEliminar.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.btnEliminar.IconSize = 32;
            this.btnEliminar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEliminar.Location = new System.Drawing.Point(672, 10);
            this.btnEliminar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Padding = new System.Windows.Forms.Padding(5, 0, 20, 0);
            this.btnEliminar.Size = new System.Drawing.Size(251, 52);
            this.btnEliminar.TabIndex = 60;
            this.btnEliminar.Tag = "btnEliminar";
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEliminar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEliminar.UseVisualStyleBackColor = false;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // btnModificar
            // 
            this.btnModificar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModificar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.btnModificar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModificar.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModificar.ForeColor = System.Drawing.Color.Goldenrod;
            this.btnModificar.IconChar = FontAwesome.Sharp.IconChar.UserEdit;
            this.btnModificar.IconColor = System.Drawing.Color.Gold;
            this.btnModificar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnModificar.IconSize = 40;
            this.btnModificar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnModificar.Location = new System.Drawing.Point(939, 10);
            this.btnModificar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnModificar.Name = "btnModificar";
            this.btnModificar.Padding = new System.Windows.Forms.Padding(5, 0, 20, 0);
            this.btnModificar.Size = new System.Drawing.Size(251, 52);
            this.btnModificar.TabIndex = 60;
            this.btnModificar.Tag = "btnModificar";
            this.btnModificar.Text = "Modificar";
            this.btnModificar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnModificar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnModificar.UseVisualStyleBackColor = false;
            this.btnModificar.Click += new System.EventHandler(this.btnModificar_Click);
            // 
            // btnCrear
            // 
            this.btnCrear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCrear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.btnCrear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCrear.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCrear.ForeColor = System.Drawing.Color.Goldenrod;
            this.btnCrear.IconChar = FontAwesome.Sharp.IconChar.PlusSquare;
            this.btnCrear.IconColor = System.Drawing.Color.Gold;
            this.btnCrear.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnCrear.IconSize = 42;
            this.btnCrear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCrear.Location = new System.Drawing.Point(1203, 10);
            this.btnCrear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCrear.Name = "btnCrear";
            this.btnCrear.Padding = new System.Windows.Forms.Padding(5, 0, 20, 0);
            this.btnCrear.Size = new System.Drawing.Size(251, 52);
            this.btnCrear.TabIndex = 59;
            this.btnCrear.Tag = "btnCrear";
            this.btnCrear.Text = "Crear";
            this.btnCrear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCrear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCrear.UseVisualStyleBackColor = false;
            this.btnCrear.Click += new System.EventHandler(this.btnCrear_Click);
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.Transparent;
            this.pnlContent.Controls.Add(this.splitMain);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 150);
            this.pnlContent.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Padding = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.pnlContent.Size = new System.Drawing.Size(1469, 691);
            this.pnlContent.TabIndex = 60;
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(15, 5);
            this.splitMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.dgvEntidades);
            this.splitMain.Panel1.Controls.Add(this.label1);
            this.splitMain.Panel1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.splitMain.Panel1MinSize = 350;
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.trvDetalle);
            this.splitMain.Panel2.Controls.Add(this.lblDetalle);
            this.splitMain.Panel2.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.splitMain.Panel2MinSize = 350;
            this.splitMain.Size = new System.Drawing.Size(1439, 681);
            this.splitMain.SplitterDistance = 555;
            this.splitMain.TabIndex = 0;
            // 
            // dgvEntidades
            // 
            this.dgvEntidades.AllowUserToAddRows = false;
            this.dgvEntidades.AllowUserToDeleteRows = false;
            this.dgvEntidades.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEntidades.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(10)))), ((int)(((byte)(40)))));
            this.dgvEntidades.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvEntidades.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(25)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(25)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvEntidades.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvEntidades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEntidades.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTipo,
            this.colNombre});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvEntidades.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvEntidades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEntidades.EnableHeadersVisualStyles = false;
            this.dgvEntidades.GridColor = System.Drawing.Color.Goldenrod;
            this.dgvEntidades.Location = new System.Drawing.Point(0, 40);
            this.dgvEntidades.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvEntidades.MultiSelect = false;
            this.dgvEntidades.Name = "dgvEntidades";
            this.dgvEntidades.ReadOnly = true;
            this.dgvEntidades.RowHeadersVisible = false;
            this.dgvEntidades.RowHeadersWidth = 51;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(10)))), ((int)(((byte)(30)))));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(100)))));
            this.dgvEntidades.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvEntidades.RowTemplate.Height = 30;
            this.dgvEntidades.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEntidades.Size = new System.Drawing.Size(552, 641);
            this.dgvEntidades.TabIndex = 56;
            this.dgvEntidades.SelectionChanged += new System.EventHandler(this.dgvEntidades_SelectionChanged);
            // 
            // colTipo
            // 
            this.colTipo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colTipo.DataPropertyName = "TipoDisplay";
            this.colTipo.FillWeight = 106.9519F;
            this.colTipo.HeaderText = "Tipo";
            this.colTipo.MinimumWidth = 6;
            this.colTipo.Name = "colTipo";
            this.colTipo.ReadOnly = true;
            this.colTipo.Width = 125;
            // 
            // colNombre
            // 
            this.colNombre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colNombre.DataPropertyName = "Nombre";
            this.colNombre.FillWeight = 93.04813F;
            this.colNombre.HeaderText = "Nombre";
            this.colNombre.MinimumWidth = 6;
            this.colNombre.Name = "colNombre";
            this.colNombre.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Goldenrod;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(3, 0, 0, 2);
            this.label1.Size = new System.Drawing.Size(114, 40);
            this.label1.TabIndex = 2;
            this.label1.Tag = "lblListado";
            this.label1.Text = "Listado";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // trvDetalle
            // 
            this.trvDetalle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(10)))), ((int)(((byte)(40)))));
            this.trvDetalle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.trvDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvDetalle.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.trvDetalle.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trvDetalle.ForeColor = System.Drawing.Color.Goldenrod;
            this.trvDetalle.FullRowSelect = true;
            this.trvDetalle.HideSelection = false;
            this.trvDetalle.Indent = 40;
            this.trvDetalle.ItemHeight = 32;
            this.trvDetalle.LineColor = System.Drawing.Color.Goldenrod;
            this.trvDetalle.Location = new System.Drawing.Point(3, 40);
            this.trvDetalle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trvDetalle.Name = "trvDetalle";
            this.trvDetalle.Size = new System.Drawing.Size(877, 641);
            this.trvDetalle.TabIndex = 4;
            this.trvDetalle.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.trvDetalle_DrawNode);
            // 
            // lblDetalle
            // 
            this.lblDetalle.AutoSize = true;
            this.lblDetalle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDetalle.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetalle.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblDetalle.Location = new System.Drawing.Point(3, 0);
            this.lblDetalle.Name = "lblDetalle";
            this.lblDetalle.Padding = new System.Windows.Forms.Padding(3, 0, 0, 2);
            this.lblDetalle.Size = new System.Drawing.Size(113, 40);
            this.lblDetalle.TabIndex = 3;
            this.lblDetalle.Tag = "lblDetalle";
            this.lblDetalle.Text = "Detalle";
            this.lblDetalle.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // FormGestionAcceso_08YS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1469, 921);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormGestionAcceso_08YS";
            this.Text = "FormFamilia";
            this.Load += new System.EventHandler(this.FormGestionAcceso_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel1.PerformLayout();
            this.splitMain.Panel2.ResumeLayout(false);
            this.splitMain.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEntidades)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlBottom;
        private FontAwesome.Sharp.IconButton btnModificar;
        private FontAwesome.Sharp.IconButton btnCrear;
        private FontAwesome.Sharp.IconButton btnEliminar;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvEntidades;
        private System.Windows.Forms.Label lblDetalle;
        private System.Windows.Forms.TreeView trvDetalle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre;
    }
}