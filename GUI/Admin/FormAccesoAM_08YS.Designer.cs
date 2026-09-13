namespace GUI_08YS.Admin
{
    partial class FormAccesoAM_08YS
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAccesoAM_08YS));
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.iconPictureBox = new FontAwesome.Sharp.IconPictureBox();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnCancelar = new FontAwesome.Sharp.IconButton();
            this.btnGuardar = new FontAwesome.Sharp.IconButton();
            this.pnlNombre = new System.Windows.Forms.Panel();
            this.txtNombre = new CustomControls.IconPlaceholderTextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.splitContenido = new System.Windows.Forms.SplitContainer();
            this.pnlDerecha = new System.Windows.Forms.Panel();
            this.dgvDisponibles = new System.Windows.Forms.DataGridView();
            this.colTipoDisp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombreDisp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblDisponibles = new System.Windows.Forms.Label();
            this.pnlCentro = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAgregar = new FontAwesome.Sharp.IconButton();
            this.btnEliminar = new FontAwesome.Sharp.IconButton();
            this.pnlIzquierda = new System.Windows.Forms.Panel();
            this.dgvSeleccionados = new System.Windows.Forms.DataGridView();
            this.ColTipoCel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombreSel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblAsignados = new System.Windows.Forms.Label();
            this.trvDetalle = new System.Windows.Forms.TreeView();
            this.lblDescripcion = new System.Windows.Forms.Label();
            this.lblDetalle = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.pnlNombre.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContenido)).BeginInit();
            this.splitContenido.Panel1.SuspendLayout();
            this.splitContenido.Panel2.SuspendLayout();
            this.splitContenido.SuspendLayout();
            this.pnlDerecha.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisponibles)).BeginInit();
            this.pnlCentro.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.pnlIzquierda.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSeleccionados)).BeginInit();
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
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(20, 15, 20, 5);
            this.pnlHeader.Size = new System.Drawing.Size(1472, 121);
            this.pnlHeader.TabIndex = 1;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblTitulo.Location = new System.Drawing.Point(120, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(451, 62);
            this.lblTitulo.TabIndex = 2;
            this.lblTitulo.Tag = "lblAltaModificacion";
            this.lblTitulo.Text = "Alta / Modificacion";
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
            this.iconPictureBox.IconSize = 100;
            this.iconPictureBox.Location = new System.Drawing.Point(20, 15);
            this.iconPictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.iconPictureBox.Name = "iconPictureBox";
            this.iconPictureBox.Size = new System.Drawing.Size(100, 101);
            this.iconPictureBox.TabIndex = 1;
            this.iconPictureBox.TabStop = false;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.Transparent;
            this.pnlBottom.Controls.Add(this.btnCancelar);
            this.pnlBottom.Controls.Add(this.btnGuardar);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 841);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Padding = new System.Windows.Forms.Padding(0, 12, 25, 12);
            this.pnlBottom.Size = new System.Drawing.Size(1472, 80);
            this.pnlBottom.TabIndex = 2;
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.ForeColor = System.Drawing.Color.Goldenrod;
            this.btnCancelar.IconChar = FontAwesome.Sharp.IconChar.CircleXmark;
            this.btnCancelar.IconColor = System.Drawing.Color.Gold;
            this.btnCancelar.IconFont = FontAwesome.Sharp.IconFont.Regular;
            this.btnCancelar.IconSize = 40;
            this.btnCancelar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancelar.Location = new System.Drawing.Point(947, 14);
            this.btnCancelar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Padding = new System.Windows.Forms.Padding(5, 0, 20, 0);
            this.btnCancelar.Size = new System.Drawing.Size(251, 52);
            this.btnCancelar.TabIndex = 61;
            this.btnCancelar.Tag = "btnCancelar";
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.btnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGuardar.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuardar.ForeColor = System.Drawing.Color.Goldenrod;
            this.btnGuardar.IconChar = FontAwesome.Sharp.IconChar.FloppyDisk;
            this.btnGuardar.IconColor = System.Drawing.Color.Gold;
            this.btnGuardar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnGuardar.IconSize = 40;
            this.btnGuardar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGuardar.Location = new System.Drawing.Point(1211, 14);
            this.btnGuardar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Padding = new System.Windows.Forms.Padding(5, 0, 20, 0);
            this.btnGuardar.Size = new System.Drawing.Size(251, 52);
            this.btnGuardar.TabIndex = 60;
            this.btnGuardar.Tag = "btnGuardar";
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGuardar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // pnlNombre
            // 
            this.pnlNombre.BackColor = System.Drawing.Color.Transparent;
            this.pnlNombre.Controls.Add(this.txtNombre);
            this.pnlNombre.Controls.Add(this.lblNombre);
            this.pnlNombre.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlNombre.Location = new System.Drawing.Point(0, 121);
            this.pnlNombre.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlNombre.Name = "pnlNombre";
            this.pnlNombre.Padding = new System.Windows.Forms.Padding(20, 10, 20, 5);
            this.pnlNombre.Size = new System.Drawing.Size(1472, 65);
            this.pnlNombre.TabIndex = 3;
            // 
            // txtNombre
            // 
            this.txtNombre.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.txtNombre.BorderColor = System.Drawing.Color.Goldenrod;
            this.txtNombre.BorderFocusColor = System.Drawing.Color.Goldenrod;
            this.txtNombre.BorderWidth = 2;
            this.txtNombre.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNombre.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtNombre.IconAlignment = CustomControls.IconTextBoxAlignment.Right;
            this.txtNombre.IconChar = FontAwesome.Sharp.IconChar.Edit;
            this.txtNombre.IconColor = System.Drawing.Color.Goldenrod;
            this.txtNombre.IconColorRight = System.Drawing.Color.DimGray;
            this.txtNombre.IconPadding = 10;
            this.txtNombre.IconSize = 30;
            this.txtNombre.Location = new System.Drawing.Point(145, 10);
            this.txtNombre.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.PlaceholderColor = System.Drawing.Color.LightGray;
            this.txtNombre.PlaceholderText = "Ingrese el nombre";
            this.txtNombre.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtNombre.Size = new System.Drawing.Size(1307, 50);
            this.txtNombre.TabIndex = 69;
            this.txtNombre.TextLeftPadding = 20;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombre.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblNombre.Location = new System.Drawing.Point(20, 10);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(125, 38);
            this.lblNombre.TabIndex = 3;
            this.lblNombre.Tag = "lblNombre";
            this.lblNombre.Text = "Nombre";
            this.lblNombre.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitContenido
            // 
            this.splitContenido.BackColor = System.Drawing.Color.Transparent;
            this.splitContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContenido.Location = new System.Drawing.Point(0, 186);
            this.splitContenido.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContenido.Name = "splitContenido";
            this.splitContenido.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContenido.Panel1
            // 
            this.splitContenido.Panel1.Controls.Add(this.pnlDerecha);
            this.splitContenido.Panel1.Controls.Add(this.pnlCentro);
            this.splitContenido.Panel1.Controls.Add(this.pnlIzquierda);
            this.splitContenido.Panel1MinSize = 300;
            // 
            // splitContenido.Panel2
            // 
            this.splitContenido.Panel2.Controls.Add(this.trvDetalle);
            this.splitContenido.Panel2.Controls.Add(this.lblDescripcion);
            this.splitContenido.Panel2.Controls.Add(this.lblDetalle);
            this.splitContenido.Panel2.Padding = new System.Windows.Forms.Padding(15, 5, 15, 5);
            this.splitContenido.Panel2MinSize = 150;
            this.splitContenido.Size = new System.Drawing.Size(1472, 655);
            this.splitContenido.SplitterDistance = 395;
            this.splitContenido.TabIndex = 4;
            // 
            // pnlDerecha
            // 
            this.pnlDerecha.Controls.Add(this.dgvDisponibles);
            this.pnlDerecha.Controls.Add(this.lblDisponibles);
            this.pnlDerecha.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDerecha.Location = new System.Drawing.Point(729, 0);
            this.pnlDerecha.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlDerecha.Name = "pnlDerecha";
            this.pnlDerecha.Padding = new System.Windows.Forms.Padding(8, 5, 15, 5);
            this.pnlDerecha.Size = new System.Drawing.Size(743, 395);
            this.pnlDerecha.TabIndex = 2;
            // 
            // dgvDisponibles
            // 
            this.dgvDisponibles.AllowUserToAddRows = false;
            this.dgvDisponibles.AllowUserToDeleteRows = false;
            this.dgvDisponibles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDisponibles.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(10)))), ((int)(((byte)(40)))));
            this.dgvDisponibles.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvDisponibles.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(25)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(25)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDisponibles.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDisponibles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDisponibles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTipoDisp,
            this.colNombreDisp});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDisponibles.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvDisponibles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDisponibles.EnableHeadersVisualStyles = false;
            this.dgvDisponibles.GridColor = System.Drawing.Color.Goldenrod;
            this.dgvDisponibles.Location = new System.Drawing.Point(8, 44);
            this.dgvDisponibles.Margin = new System.Windows.Forms.Padding(4);
            this.dgvDisponibles.MultiSelect = false;
            this.dgvDisponibles.Name = "dgvDisponibles";
            this.dgvDisponibles.ReadOnly = true;
            this.dgvDisponibles.RowHeadersVisible = false;
            this.dgvDisponibles.RowHeadersWidth = 51;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(10)))), ((int)(((byte)(30)))));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(100)))));
            this.dgvDisponibles.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvDisponibles.RowTemplate.Height = 30;
            this.dgvDisponibles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDisponibles.Size = new System.Drawing.Size(720, 346);
            this.dgvDisponibles.TabIndex = 58;
            this.dgvDisponibles.SelectionChanged += new System.EventHandler(this.dgvDisponibles_SelectionChanged);
            // 
            // colTipoDisp
            // 
            this.colTipoDisp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colTipoDisp.DataPropertyName = "TipoDisplay";
            this.colTipoDisp.FillWeight = 106.9519F;
            this.colTipoDisp.HeaderText = "Tipo";
            this.colTipoDisp.MinimumWidth = 6;
            this.colTipoDisp.Name = "colTipoDisp";
            this.colTipoDisp.ReadOnly = true;
            this.colTipoDisp.Width = 125;
            // 
            // colNombreDisp
            // 
            this.colNombreDisp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colNombreDisp.DataPropertyName = "Nombre";
            this.colNombreDisp.FillWeight = 93.04813F;
            this.colNombreDisp.HeaderText = "Nombre";
            this.colNombreDisp.MinimumWidth = 6;
            this.colNombreDisp.Name = "colNombreDisp";
            this.colNombreDisp.ReadOnly = true;
            // 
            // lblDisponibles
            // 
            this.lblDisponibles.AutoSize = true;
            this.lblDisponibles.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDisponibles.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDisponibles.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblDisponibles.Location = new System.Drawing.Point(8, 5);
            this.lblDisponibles.Name = "lblDisponibles";
            this.lblDisponibles.Padding = new System.Windows.Forms.Padding(3, 0, 0, 2);
            this.lblDisponibles.Size = new System.Drawing.Size(170, 39);
            this.lblDisponibles.TabIndex = 4;
            this.lblDisponibles.Tag = "lblDisponibles";
            this.lblDisponibles.Text = "Disponibles";
            this.lblDisponibles.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // pnlCentro
            // 
            this.pnlCentro.Controls.Add(this.tableLayoutPanel1);
            this.pnlCentro.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlCentro.Location = new System.Drawing.Point(549, 0);
            this.pnlCentro.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlCentro.Name = "pnlCentro";
            this.pnlCentro.Size = new System.Drawing.Size(180, 395);
            this.pnlCentro.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(180, 395);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.btnAgregar);
            this.flowLayoutPanel1.Controls.Add(this.btnEliminar);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 139);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(174, 116);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btnAgregar
            // 
            this.btnAgregar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.btnAgregar.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAgregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgregar.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAgregar.ForeColor = System.Drawing.Color.Goldenrod;
            this.btnAgregar.IconChar = FontAwesome.Sharp.IconChar.ChevronLeft;
            this.btnAgregar.IconColor = System.Drawing.Color.Gold;
            this.btnAgregar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnAgregar.IconSize = 30;
            this.btnAgregar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgregar.Location = new System.Drawing.Point(3, 2);
            this.btnAgregar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Padding = new System.Windows.Forms.Padding(5, 0, 20, 0);
            this.btnAgregar.Size = new System.Drawing.Size(171, 50);
            this.btnAgregar.TabIndex = 61;
            this.btnAgregar.Tag = "btnAgregar";
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgregar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAgregar.UseVisualStyleBackColor = false;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEliminar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(15)))), ((int)(((byte)(45)))));
            this.btnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEliminar.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEliminar.ForeColor = System.Drawing.Color.Goldenrod;
            this.btnEliminar.IconChar = FontAwesome.Sharp.IconChar.ChevronRight;
            this.btnEliminar.IconColor = System.Drawing.Color.Gold;
            this.btnEliminar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnEliminar.IconSize = 30;
            this.btnEliminar.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnEliminar.Location = new System.Drawing.Point(0, 66);
            this.btnEliminar.Margin = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnEliminar.Size = new System.Drawing.Size(171, 50);
            this.btnEliminar.TabIndex = 62;
            this.btnEliminar.Tag = "btnEliminar";
            this.btnEliminar.Text = "  Eliminar";
            this.btnEliminar.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.btnEliminar.UseVisualStyleBackColor = false;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // pnlIzquierda
            // 
            this.pnlIzquierda.Controls.Add(this.dgvSeleccionados);
            this.pnlIzquierda.Controls.Add(this.lblAsignados);
            this.pnlIzquierda.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlIzquierda.Location = new System.Drawing.Point(0, 0);
            this.pnlIzquierda.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlIzquierda.Name = "pnlIzquierda";
            this.pnlIzquierda.Padding = new System.Windows.Forms.Padding(15, 5, 8, 5);
            this.pnlIzquierda.Size = new System.Drawing.Size(549, 395);
            this.pnlIzquierda.TabIndex = 0;
            // 
            // dgvSeleccionados
            // 
            this.dgvSeleccionados.AllowUserToAddRows = false;
            this.dgvSeleccionados.AllowUserToDeleteRows = false;
            this.dgvSeleccionados.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSeleccionados.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(10)))), ((int)(((byte)(40)))));
            this.dgvSeleccionados.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSeleccionados.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(25)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(25)))), ((int)(((byte)(50)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSeleccionados.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvSeleccionados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSeleccionados.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColTipoCel,
            this.colNombreSel});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSeleccionados.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvSeleccionados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSeleccionados.EnableHeadersVisualStyles = false;
            this.dgvSeleccionados.GridColor = System.Drawing.Color.Goldenrod;
            this.dgvSeleccionados.Location = new System.Drawing.Point(15, 44);
            this.dgvSeleccionados.Margin = new System.Windows.Forms.Padding(4);
            this.dgvSeleccionados.MultiSelect = false;
            this.dgvSeleccionados.Name = "dgvSeleccionados";
            this.dgvSeleccionados.ReadOnly = true;
            this.dgvSeleccionados.RowHeadersVisible = false;
            this.dgvSeleccionados.RowHeadersWidth = 51;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(10)))), ((int)(((byte)(30)))));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(100)))));
            this.dgvSeleccionados.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvSeleccionados.RowTemplate.Height = 30;
            this.dgvSeleccionados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSeleccionados.Size = new System.Drawing.Size(526, 346);
            this.dgvSeleccionados.TabIndex = 57;
            this.dgvSeleccionados.SelectionChanged += new System.EventHandler(this.dgvSeleccionados_SelectionChanged);
            // 
            // ColTipoCel
            // 
            this.ColTipoCel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColTipoCel.DataPropertyName = "TipoDisplay";
            this.ColTipoCel.FillWeight = 106.9519F;
            this.ColTipoCel.HeaderText = "Tipo";
            this.ColTipoCel.MinimumWidth = 6;
            this.ColTipoCel.Name = "ColTipoCel";
            this.ColTipoCel.ReadOnly = true;
            this.ColTipoCel.Width = 125;
            // 
            // colNombreSel
            // 
            this.colNombreSel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colNombreSel.DataPropertyName = "Nombre";
            this.colNombreSel.FillWeight = 93.04813F;
            this.colNombreSel.HeaderText = "Nombre";
            this.colNombreSel.MinimumWidth = 6;
            this.colNombreSel.Name = "colNombreSel";
            this.colNombreSel.ReadOnly = true;
            // 
            // lblAsignados
            // 
            this.lblAsignados.AutoSize = true;
            this.lblAsignados.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAsignados.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAsignados.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblAsignados.Location = new System.Drawing.Point(15, 5);
            this.lblAsignados.Name = "lblAsignados";
            this.lblAsignados.Padding = new System.Windows.Forms.Padding(4, 0, 0, 2);
            this.lblAsignados.Size = new System.Drawing.Size(154, 39);
            this.lblAsignados.TabIndex = 3;
            this.lblAsignados.Tag = "lblAsignados";
            this.lblAsignados.Text = "Asignados";
            this.lblAsignados.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // trvDetalle
            // 
            this.trvDetalle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(10)))), ((int)(((byte)(40)))));
            this.trvDetalle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.trvDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvDetalle.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.trvDetalle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trvDetalle.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.trvDetalle.FullRowSelect = true;
            this.trvDetalle.HideSelection = false;
            this.trvDetalle.Indent = 100;
            this.trvDetalle.ItemHeight = 40;
            this.trvDetalle.LineColor = System.Drawing.Color.Goldenrod;
            this.trvDetalle.Location = new System.Drawing.Point(15, 83);
            this.trvDetalle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.trvDetalle.Name = "trvDetalle";
            this.trvDetalle.Size = new System.Drawing.Size(1442, 168);
            this.trvDetalle.TabIndex = 60;
            this.trvDetalle.Visible = false;
            this.trvDetalle.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.trvDetalle_DrawNode);
            // 
            // lblDescripcion
            // 
            this.lblDescripcion.AutoSize = true;
            this.lblDescripcion.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDescripcion.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescripcion.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblDescripcion.Location = new System.Drawing.Point(15, 42);
            this.lblDescripcion.Name = "lblDescripcion";
            this.lblDescripcion.Padding = new System.Windows.Forms.Padding(5);
            this.lblDescripcion.Size = new System.Drawing.Size(150, 41);
            this.lblDescripcion.TabIndex = 8;
            this.lblDescripcion.Text = "Descripcion";
            this.lblDescripcion.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblDescripcion.Visible = false;
            // 
            // lblDetalle
            // 
            this.lblDetalle.AutoSize = true;
            this.lblDetalle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDetalle.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDetalle.ForeColor = System.Drawing.Color.Goldenrod;
            this.lblDetalle.Location = new System.Drawing.Point(15, 5);
            this.lblDetalle.Name = "lblDetalle";
            this.lblDetalle.Padding = new System.Windows.Forms.Padding(3, 0, 0, 2);
            this.lblDetalle.Size = new System.Drawing.Size(301, 37);
            this.lblDetalle.TabIndex = 5;
            this.lblDetalle.Tag = "lblDetalleSeleccionado";
            this.lblDetalle.Text = "Detalle del seleccionado";
            this.lblDetalle.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // FormAccesoAM_08YS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1472, 921);
            this.Controls.Add(this.splitContenido);
            this.Controls.Add(this.pnlNombre);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormAccesoAM_08YS";
            this.Text = "FormAccesoAM_08YS";
            this.Load += new System.EventHandler(this.FormAccesoAM_08YS_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.pnlNombre.ResumeLayout(false);
            this.pnlNombre.PerformLayout();
            this.splitContenido.Panel1.ResumeLayout(false);
            this.splitContenido.Panel2.ResumeLayout(false);
            this.splitContenido.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContenido)).EndInit();
            this.splitContenido.ResumeLayout(false);
            this.pnlDerecha.ResumeLayout(false);
            this.pnlDerecha.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDisponibles)).EndInit();
            this.pnlCentro.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.pnlIzquierda.ResumeLayout(false);
            this.pnlIzquierda.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSeleccionados)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel pnlBottom;
        private FontAwesome.Sharp.IconButton btnGuardar;
        private FontAwesome.Sharp.IconButton btnCancelar;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox;
        private System.Windows.Forms.Panel pnlNombre;
        private System.Windows.Forms.Label lblNombre;
        private CustomControls.IconPlaceholderTextBox txtNombre;
        private System.Windows.Forms.SplitContainer splitContenido;
        private System.Windows.Forms.Panel pnlIzquierda;
        private System.Windows.Forms.Label lblAsignados;
        private System.Windows.Forms.DataGridView dgvSeleccionados;
        private System.Windows.Forms.Panel pnlCentro;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private FontAwesome.Sharp.IconButton btnAgregar;
        private FontAwesome.Sharp.IconButton btnEliminar;
        private System.Windows.Forms.Panel pnlDerecha;
        private System.Windows.Forms.DataGridView dgvDisponibles;
        private System.Windows.Forms.Label lblDisponibles;
        private System.Windows.Forms.Label lblDetalle;
        private System.Windows.Forms.TreeView trvDetalle;
        private System.Windows.Forms.Label lblDescripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTipoDisp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombreDisp;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTipoCel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombreSel;
    }
}