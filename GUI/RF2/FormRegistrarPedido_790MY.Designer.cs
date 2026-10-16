namespace GUI_08YS.RF2
{
    partial class FormRegistrarPedido_790MY
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
            this.pnlAviso_790MY = new System.Windows.Forms.Panel();
            this.lblAviso_790MY = new System.Windows.Forms.Label();
            this.btnRetirado_790MY = new System.Windows.Forms.Button();
            this.tlpMain_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.pnlMesas_790MY = new System.Windows.Forms.Panel();
            this.flpMesas_790MY = new System.Windows.Forms.FlowLayoutPanel();
            this.lblMisMesasSub_790MY = new System.Windows.Forms.Label();
            this.lblMisMesas_790MY = new System.Windows.Forms.Label();
            this.pnlCatalogo_790MY = new System.Windows.Forms.Panel();
            this.tlpCatalogo_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.tlpCatalogoHeader_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.lblPedidoTitulo_790MY = new System.Windows.Forms.Label();
            this.lblPedidoSub_790MY = new System.Windows.Forms.Label();
            this.txtBuscarProducto_790MY = new System.Windows.Forms.TextBox();
            this.flpCategorias_790MY = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCatEntrada_790MY = new System.Windows.Forms.Button();
            this.btnCatPlatoPrincipal_790MY = new System.Windows.Forms.Button();
            this.btnCatPostre_790MY = new System.Windows.Forms.Button();
            this.btnCatBebida_790MY = new System.Windows.Forms.Button();
            this.flpProductos_790MY = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlEditor_790MY = new GUI_08YS.RF2.PanelRedondeado_790MY();
            this.lblEditorVacio_790MY = new System.Windows.Forms.Label();
            this.tlpEditor_790MY = new System.Windows.Forms.TableLayoutPanel();
            this.lblItemNombre_790MY = new System.Windows.Forms.Label();
            this.lblCantidad_790MY = new System.Windows.Forms.Label();
            this.lblObservaciones_790MY = new System.Windows.Forms.Label();
            this.nudCantidad_790MY = new System.Windows.Forms.NumericUpDown();
            this.txtObservaciones_790MY = new System.Windows.Forms.TextBox();
            this.btnAgregarItem_790MY = new System.Windows.Forms.Button();
            this.pnlComanda_790MY = new System.Windows.Forms.Panel();
            this.flpItems_790MY = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlComandaFooter_790MY = new System.Windows.Forms.Panel();
            this.btnEnviar_790MY = new FontAwesome.Sharp.IconButton();
            this.lblTotal_790MY = new System.Windows.Forms.Label();
            this.lblTotalInfo_790MY = new System.Windows.Forms.Label();
            this.lblComandaSub_790MY = new System.Windows.Forms.Label();
            this.lblComandaTitulo_790MY = new System.Windows.Forms.Label();
            this.timerAvisos_790MY = new System.Windows.Forms.Timer(this.components);
            this.pnlAviso_790MY.SuspendLayout();
            this.tlpMain_790MY.SuspendLayout();
            this.pnlMesas_790MY.SuspendLayout();
            this.pnlCatalogo_790MY.SuspendLayout();
            this.tlpCatalogo_790MY.SuspendLayout();
            this.tlpCatalogoHeader_790MY.SuspendLayout();
            this.flpCategorias_790MY.SuspendLayout();
            this.pnlEditor_790MY.SuspendLayout();
            this.tlpEditor_790MY.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidad_790MY)).BeginInit();
            this.pnlComanda_790MY.SuspendLayout();
            this.pnlComandaFooter_790MY.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlAviso_790MY
            //
            this.pnlAviso_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(242)))), ((int)(((byte)(230)))));
            this.pnlAviso_790MY.Controls.Add(this.lblAviso_790MY);
            this.pnlAviso_790MY.Controls.Add(this.btnRetirado_790MY);
            this.pnlAviso_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAviso_790MY.Name = "pnlAviso_790MY";
            this.pnlAviso_790MY.Padding = new System.Windows.Forms.Padding(24, 8, 24, 8);
            this.pnlAviso_790MY.Size = new System.Drawing.Size(1280, 52);
            this.pnlAviso_790MY.TabIndex = 1;
            this.pnlAviso_790MY.Visible = false;
            this.pnlAviso_790MY.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlAviso_790MY_Paint);
            //
            // lblAviso_790MY
            //
            this.lblAviso_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAviso_790MY.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblAviso_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(110)))), ((int)(((byte)(60)))));
            this.lblAviso_790MY.Name = "lblAviso_790MY";
            this.lblAviso_790MY.TabIndex = 0;
            this.lblAviso_790MY.Text = "🔔 Pedido listo para su entrega";
            this.lblAviso_790MY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // btnRetirado_790MY
            //
            this.btnRetirado_790MY.BackColor = System.Drawing.Color.White;
            this.btnRetirado_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRetirado_790MY.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRetirado_790MY.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(110)))), ((int)(((byte)(60)))));
            this.btnRetirado_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRetirado_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnRetirado_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(110)))), ((int)(((byte)(60)))));
            this.btnRetirado_790MY.Name = "btnRetirado_790MY";
            this.btnRetirado_790MY.Size = new System.Drawing.Size(210, 36);
            this.btnRetirado_790MY.TabIndex = 1;
            this.btnRetirado_790MY.Tag = "RP_btnRetirado";
            this.btnRetirado_790MY.Text = "Marcar como retirado";
            this.btnRetirado_790MY.UseVisualStyleBackColor = false;
            this.btnRetirado_790MY.Click += new System.EventHandler(this.btnRetirado_790MY_Click);
            //
            // tlpMain_790MY
            //
            this.tlpMain_790MY.ColumnCount = 3;
            this.tlpMain_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.tlpMain_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28F));
            this.tlpMain_790MY.Controls.Add(this.pnlMesas_790MY, 0, 0);
            this.tlpMain_790MY.Controls.Add(this.pnlCatalogo_790MY, 1, 0);
            this.tlpMain_790MY.Controls.Add(this.pnlComanda_790MY, 2, 0);
            this.tlpMain_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain_790MY.Name = "tlpMain_790MY";
            this.tlpMain_790MY.RowCount = 1;
            this.tlpMain_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain_790MY.TabIndex = 0;
            this.tlpMain_790MY.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.tlpMain_790MY_CellPaint);
            //
            // pnlMesas_790MY
            //
            this.pnlMesas_790MY.Controls.Add(this.flpMesas_790MY);
            this.pnlMesas_790MY.Controls.Add(this.lblMisMesasSub_790MY);
            this.pnlMesas_790MY.Controls.Add(this.lblMisMesas_790MY);
            this.pnlMesas_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMesas_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.pnlMesas_790MY.Name = "pnlMesas_790MY";
            this.pnlMesas_790MY.Padding = new System.Windows.Forms.Padding(20, 18, 16, 16);
            this.pnlMesas_790MY.TabIndex = 0;
            //
            // flpMesas_790MY
            //
            this.flpMesas_790MY.AutoScroll = true;
            this.flpMesas_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpMesas_790MY.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpMesas_790MY.Name = "flpMesas_790MY";
            this.flpMesas_790MY.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.flpMesas_790MY.TabIndex = 2;
            this.flpMesas_790MY.WrapContents = false;
            this.flpMesas_790MY.Resize += new System.EventHandler(this.flpMesas_790MY_Resize);
            //
            // lblMisMesasSub_790MY
            //
            this.lblMisMesasSub_790MY.AutoSize = true;
            this.lblMisMesasSub_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMisMesasSub_790MY.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblMisMesasSub_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblMisMesasSub_790MY.Name = "lblMisMesasSub_790MY";
            this.lblMisMesasSub_790MY.TabIndex = 1;
            this.lblMisMesasSub_790MY.Tag = "RP_mis_mesas_sub";
            this.lblMisMesasSub_790MY.Text = "Ocupadas y a su cargo";
            //
            // lblMisMesas_790MY
            //
            this.lblMisMesas_790MY.AutoSize = true;
            this.lblMisMesas_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMisMesas_790MY.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblMisMesas_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblMisMesas_790MY.Name = "lblMisMesas_790MY";
            this.lblMisMesas_790MY.TabIndex = 0;
            this.lblMisMesas_790MY.Tag = "RP_mis_mesas";
            this.lblMisMesas_790MY.Text = "Mis mesas";
            //
            // pnlCatalogo_790MY
            //
            this.pnlCatalogo_790MY.Controls.Add(this.tlpCatalogo_790MY);
            this.pnlCatalogo_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCatalogo_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.pnlCatalogo_790MY.Name = "pnlCatalogo_790MY";
            this.pnlCatalogo_790MY.Padding = new System.Windows.Forms.Padding(24, 18, 24, 16);
            this.pnlCatalogo_790MY.TabIndex = 1;
            //
            // tlpCatalogo_790MY
            //
            this.tlpCatalogo_790MY.ColumnCount = 1;
            this.tlpCatalogo_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCatalogo_790MY.Controls.Add(this.tlpCatalogoHeader_790MY, 0, 0);
            this.tlpCatalogo_790MY.Controls.Add(this.flpCategorias_790MY, 0, 1);
            this.tlpCatalogo_790MY.Controls.Add(this.flpProductos_790MY, 0, 2);
            this.tlpCatalogo_790MY.Controls.Add(this.pnlEditor_790MY, 0, 3);
            this.tlpCatalogo_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCatalogo_790MY.Name = "tlpCatalogo_790MY";
            this.tlpCatalogo_790MY.RowCount = 4;
            this.tlpCatalogo_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpCatalogo_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpCatalogo_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCatalogo_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpCatalogo_790MY.TabIndex = 0;
            //
            // tlpCatalogoHeader_790MY
            //
            this.tlpCatalogoHeader_790MY.AutoSize = true;
            this.tlpCatalogoHeader_790MY.ColumnCount = 2;
            this.tlpCatalogoHeader_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCatalogoHeader_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            this.tlpCatalogoHeader_790MY.Controls.Add(this.lblPedidoTitulo_790MY, 0, 0);
            this.tlpCatalogoHeader_790MY.Controls.Add(this.lblPedidoSub_790MY, 0, 1);
            this.tlpCatalogoHeader_790MY.Controls.Add(this.txtBuscarProducto_790MY, 1, 0);
            this.tlpCatalogoHeader_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCatalogoHeader_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.tlpCatalogoHeader_790MY.Name = "tlpCatalogoHeader_790MY";
            this.tlpCatalogoHeader_790MY.RowCount = 2;
            this.tlpCatalogoHeader_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpCatalogoHeader_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpCatalogoHeader_790MY.SetRowSpan(this.txtBuscarProducto_790MY, 2);
            this.tlpCatalogoHeader_790MY.TabIndex = 0;
            //
            // lblPedidoTitulo_790MY
            //
            this.lblPedidoTitulo_790MY.AutoSize = true;
            this.lblPedidoTitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblPedidoTitulo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblPedidoTitulo_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.lblPedidoTitulo_790MY.Name = "lblPedidoTitulo_790MY";
            this.lblPedidoTitulo_790MY.TabIndex = 0;
            this.lblPedidoTitulo_790MY.Text = "Registrar pedido";
            //
            // lblPedidoSub_790MY
            //
            this.lblPedidoSub_790MY.AutoSize = true;
            this.lblPedidoSub_790MY.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPedidoSub_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblPedidoSub_790MY.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblPedidoSub_790MY.Name = "lblPedidoSub_790MY";
            this.lblPedidoSub_790MY.TabIndex = 1;
            this.lblPedidoSub_790MY.Text = "-";
            //
            // txtBuscarProducto_790MY
            //
            this.txtBuscarProducto_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBuscarProducto_790MY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBuscarProducto_790MY.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtBuscarProducto_790MY.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.txtBuscarProducto_790MY.Name = "txtBuscarProducto_790MY";
            this.txtBuscarProducto_790MY.TabIndex = 2;
            this.txtBuscarProducto_790MY.TextChanged += new System.EventHandler(this.txtBuscarProducto_790MY_TextChanged);
            //
            // flpCategorias_790MY
            //
            this.flpCategorias_790MY.Controls.Add(this.btnCatEntrada_790MY);
            this.flpCategorias_790MY.Controls.Add(this.btnCatPlatoPrincipal_790MY);
            this.flpCategorias_790MY.Controls.Add(this.btnCatPostre_790MY);
            this.flpCategorias_790MY.Controls.Add(this.btnCatBebida_790MY);
            this.flpCategorias_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpCategorias_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.flpCategorias_790MY.Name = "flpCategorias_790MY";
            this.flpCategorias_790MY.TabIndex = 1;
            this.flpCategorias_790MY.WrapContents = false;
            this.flpCategorias_790MY.Resize += new System.EventHandler(this.flpCategorias_790MY_Resize);
            //
            // btnCatEntrada_790MY
            //
            this.btnCatEntrada_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCatEntrada_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCatEntrada_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.btnCatEntrada_790MY.Margin = new System.Windows.Forms.Padding(0, 6, 8, 6);
            this.btnCatEntrada_790MY.Size = new System.Drawing.Size(110, 44);
            this.btnCatEntrada_790MY.Name = "btnCatEntrada_790MY";
            this.btnCatEntrada_790MY.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.btnCatEntrada_790MY.TabIndex = 0;
            this.btnCatEntrada_790MY.Tag = "RP_cat_Entrada";
            this.btnCatEntrada_790MY.Text = "Entradas";
            this.btnCatEntrada_790MY.UseVisualStyleBackColor = false;
            this.btnCatEntrada_790MY.Click += new System.EventHandler(this.btnCategoria_Click);
            //
            // btnCatPlatoPrincipal_790MY
            //
            this.btnCatPlatoPrincipal_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCatPlatoPrincipal_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCatPlatoPrincipal_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.btnCatPlatoPrincipal_790MY.Margin = new System.Windows.Forms.Padding(0, 6, 8, 6);
            this.btnCatPlatoPrincipal_790MY.Size = new System.Drawing.Size(110, 44);
            this.btnCatPlatoPrincipal_790MY.Name = "btnCatPlatoPrincipal_790MY";
            this.btnCatPlatoPrincipal_790MY.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.btnCatPlatoPrincipal_790MY.TabIndex = 1;
            this.btnCatPlatoPrincipal_790MY.Tag = "RP_cat_PlatoPrincipal";
            this.btnCatPlatoPrincipal_790MY.Text = "Platos principales";
            this.btnCatPlatoPrincipal_790MY.UseVisualStyleBackColor = false;
            this.btnCatPlatoPrincipal_790MY.Click += new System.EventHandler(this.btnCategoria_Click);
            //
            // btnCatPostre_790MY
            //
            this.btnCatPostre_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCatPostre_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCatPostre_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.btnCatPostre_790MY.Margin = new System.Windows.Forms.Padding(0, 6, 8, 6);
            this.btnCatPostre_790MY.Size = new System.Drawing.Size(110, 44);
            this.btnCatPostre_790MY.Name = "btnCatPostre_790MY";
            this.btnCatPostre_790MY.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.btnCatPostre_790MY.TabIndex = 2;
            this.btnCatPostre_790MY.Tag = "RP_cat_Postre";
            this.btnCatPostre_790MY.Text = "Postres";
            this.btnCatPostre_790MY.UseVisualStyleBackColor = false;
            this.btnCatPostre_790MY.Click += new System.EventHandler(this.btnCategoria_Click);
            //
            // btnCatBebida_790MY
            //
            this.btnCatBebida_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCatBebida_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCatBebida_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.btnCatBebida_790MY.Margin = new System.Windows.Forms.Padding(0, 6, 8, 6);
            this.btnCatBebida_790MY.Size = new System.Drawing.Size(110, 44);
            this.btnCatBebida_790MY.Name = "btnCatBebida_790MY";
            this.btnCatBebida_790MY.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.btnCatBebida_790MY.TabIndex = 3;
            this.btnCatBebida_790MY.Tag = "RP_cat_Bebida";
            this.btnCatBebida_790MY.Text = "Bebidas";
            this.btnCatBebida_790MY.UseVisualStyleBackColor = false;
            this.btnCatBebida_790MY.Click += new System.EventHandler(this.btnCategoria_Click);
            //
            // flpProductos_790MY
            //
            this.flpProductos_790MY.AutoScroll = true;
            this.flpProductos_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpProductos_790MY.Margin = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.flpProductos_790MY.Name = "flpProductos_790MY";
            this.flpProductos_790MY.TabIndex = 2;
            this.flpProductos_790MY.Resize += new System.EventHandler(this.flpProductos_790MY_Resize);
            //
            // pnlEditor_790MY
            //
            this.pnlEditor_790MY.BackColor = System.Drawing.Color.White;
            this.pnlEditor_790MY.Controls.Add(this.tlpEditor_790MY);
            this.pnlEditor_790MY.Controls.Add(this.lblEditorVacio_790MY);
            this.pnlEditor_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEditor_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.pnlEditor_790MY.Name = "pnlEditor_790MY";
            this.pnlEditor_790MY.Padding = new System.Windows.Forms.Padding(20, 14, 20, 14);
            this.pnlEditor_790MY.Radio = 12;
            this.pnlEditor_790MY.TabIndex = 3;
            //
            // lblEditorVacio_790MY
            //
            this.lblEditorVacio_790MY.BackColor = System.Drawing.Color.Transparent;
            this.lblEditorVacio_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblEditorVacio_790MY.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.lblEditorVacio_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblEditorVacio_790MY.Name = "lblEditorVacio_790MY";
            this.lblEditorVacio_790MY.TabIndex = 0;
            this.lblEditorVacio_790MY.Tag = "RP_editor_vacio";
            this.lblEditorVacio_790MY.Text = "+  Seleccione un producto del catálogo para indicar cantidad y observaciones.";
            this.lblEditorVacio_790MY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // tlpEditor_790MY
            //
            this.tlpEditor_790MY.BackColor = System.Drawing.Color.Transparent;
            this.tlpEditor_790MY.ColumnCount = 3;
            this.tlpEditor_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tlpEditor_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpEditor_790MY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpEditor_790MY.Controls.Add(this.lblItemNombre_790MY, 0, 0);
            this.tlpEditor_790MY.Controls.Add(this.lblCantidad_790MY, 0, 1);
            this.tlpEditor_790MY.Controls.Add(this.lblObservaciones_790MY, 1, 1);
            this.tlpEditor_790MY.Controls.Add(this.nudCantidad_790MY, 0, 2);
            this.tlpEditor_790MY.Controls.Add(this.txtObservaciones_790MY, 1, 2);
            this.tlpEditor_790MY.Controls.Add(this.btnAgregarItem_790MY, 2, 2);
            this.tlpEditor_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpEditor_790MY.Name = "tlpEditor_790MY";
            this.tlpEditor_790MY.RowCount = 3;
            this.tlpEditor_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpEditor_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tlpEditor_790MY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpEditor_790MY.SetColumnSpan(this.lblItemNombre_790MY, 3);
            this.tlpEditor_790MY.TabIndex = 1;
            this.tlpEditor_790MY.Visible = false;
            //
            // lblItemNombre_790MY
            //
            this.lblItemNombre_790MY.AutoSize = true;
            this.lblItemNombre_790MY.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblItemNombre_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.lblItemNombre_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.lblItemNombre_790MY.Name = "lblItemNombre_790MY";
            this.lblItemNombre_790MY.TabIndex = 0;
            this.lblItemNombre_790MY.Text = "-";
            //
            // lblCantidad_790MY
            //
            this.lblCantidad_790MY.AutoSize = true;
            this.lblCantidad_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblCantidad_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblCantidad_790MY.Margin = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.lblCantidad_790MY.Name = "lblCantidad_790MY";
            this.lblCantidad_790MY.TabIndex = 1;
            this.lblCantidad_790MY.Tag = "RP_lblCantidad";
            this.lblCantidad_790MY.Text = "Cantidad";
            //
            // lblObservaciones_790MY
            //
            this.lblObservaciones_790MY.AutoSize = true;
            this.lblObservaciones_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblObservaciones_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblObservaciones_790MY.Margin = new System.Windows.Forms.Padding(10, 0, 0, 2);
            this.lblObservaciones_790MY.Name = "lblObservaciones_790MY";
            this.lblObservaciones_790MY.TabIndex = 2;
            this.lblObservaciones_790MY.Tag = "RP_lblObservaciones";
            this.lblObservaciones_790MY.Text = "Observaciones";
            //
            // nudCantidad_790MY
            //
            this.nudCantidad_790MY.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.nudCantidad_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.nudCantidad_790MY.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudCantidad_790MY.Name = "nudCantidad_790MY";
            this.nudCantidad_790MY.Size = new System.Drawing.Size(100, 29);
            this.nudCantidad_790MY.TabIndex = 3;
            this.nudCantidad_790MY.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // txtObservaciones_790MY
            //
            this.txtObservaciones_790MY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtObservaciones_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtObservaciones_790MY.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtObservaciones_790MY.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.txtObservaciones_790MY.MaxLength = 200;
            this.txtObservaciones_790MY.Name = "txtObservaciones_790MY";
            this.txtObservaciones_790MY.TabIndex = 4;
            this.txtObservaciones_790MY.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtObservaciones_790MY_KeyDown);
            //
            // btnAgregarItem_790MY
            //
            this.btnAgregarItem_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnAgregarItem_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAgregarItem_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAgregarItem_790MY.FlatAppearance.BorderSize = 0;
            this.btnAgregarItem_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgregarItem_790MY.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnAgregarItem_790MY.ForeColor = System.Drawing.Color.White;
            this.btnAgregarItem_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.btnAgregarItem_790MY.Name = "btnAgregarItem_790MY";
            this.btnAgregarItem_790MY.Size = new System.Drawing.Size(150, 30);
            this.btnAgregarItem_790MY.TabIndex = 5;
            this.btnAgregarItem_790MY.Tag = "RP_btnAgregar";
            this.btnAgregarItem_790MY.Text = "Agregar";
            this.btnAgregarItem_790MY.UseVisualStyleBackColor = false;
            this.btnAgregarItem_790MY.Click += new System.EventHandler(this.btnAgregarItem_790MY_Click);
            //
            // pnlComanda_790MY
            //
            this.pnlComanda_790MY.BackColor = System.Drawing.Color.White;
            this.pnlComanda_790MY.Controls.Add(this.flpItems_790MY);
            this.pnlComanda_790MY.Controls.Add(this.pnlComandaFooter_790MY);
            this.pnlComanda_790MY.Controls.Add(this.lblComandaSub_790MY);
            this.pnlComanda_790MY.Controls.Add(this.lblComandaTitulo_790MY);
            this.pnlComanda_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlComanda_790MY.Margin = new System.Windows.Forms.Padding(0);
            this.pnlComanda_790MY.Name = "pnlComanda_790MY";
            this.pnlComanda_790MY.Padding = new System.Windows.Forms.Padding(22, 18, 22, 16);
            this.pnlComanda_790MY.TabIndex = 2;
            //
            // flpItems_790MY
            //
            this.flpItems_790MY.AutoScroll = true;
            this.flpItems_790MY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpItems_790MY.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpItems_790MY.Name = "flpItems_790MY";
            this.flpItems_790MY.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.flpItems_790MY.TabIndex = 2;
            this.flpItems_790MY.WrapContents = false;
            this.flpItems_790MY.Resize += new System.EventHandler(this.flpItems_790MY_Resize);
            //
            // pnlComandaFooter_790MY
            //
            this.pnlComandaFooter_790MY.Controls.Add(this.btnEnviar_790MY);
            this.pnlComandaFooter_790MY.Controls.Add(this.lblTotal_790MY);
            this.pnlComandaFooter_790MY.Controls.Add(this.lblTotalInfo_790MY);
            this.pnlComandaFooter_790MY.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlComandaFooter_790MY.Name = "pnlComandaFooter_790MY";
            this.pnlComandaFooter_790MY.Padding = new System.Windows.Forms.Padding(0, 14, 0, 0);
            this.pnlComandaFooter_790MY.Size = new System.Drawing.Size(360, 128);
            this.pnlComandaFooter_790MY.TabIndex = 3;
            this.pnlComandaFooter_790MY.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlComandaFooter_790MY_Paint);
            //
            // btnEnviar_790MY
            //
            this.btnEnviar_790MY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.btnEnviar_790MY.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEnviar_790MY.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnEnviar_790MY.FlatAppearance.BorderSize = 0;
            this.btnEnviar_790MY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnviar_790MY.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.btnEnviar_790MY.ForeColor = System.Drawing.Color.White;
            this.btnEnviar_790MY.IconChar = FontAwesome.Sharp.IconChar.PaperPlane;
            this.btnEnviar_790MY.IconColor = System.Drawing.Color.White;
            this.btnEnviar_790MY.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.btnEnviar_790MY.IconSize = 26;
            this.btnEnviar_790MY.Name = "btnEnviar_790MY";
            this.btnEnviar_790MY.Size = new System.Drawing.Size(360, 58);
            this.btnEnviar_790MY.TabIndex = 2;
            this.btnEnviar_790MY.Tag = "RP_btnEnviar";
            this.btnEnviar_790MY.Text = "  Enviar a cocina";
            this.btnEnviar_790MY.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEnviar_790MY.UseVisualStyleBackColor = false;
            this.btnEnviar_790MY.Click += new System.EventHandler(this.btnEnviar_790MY_Click);
            //
            // lblTotal_790MY
            //
            this.lblTotal_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotal_790MY.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTotal_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblTotal_790MY.Location = new System.Drawing.Point(180, 18);
            this.lblTotal_790MY.Name = "lblTotal_790MY";
            this.lblTotal_790MY.Size = new System.Drawing.Size(180, 36);
            this.lblTotal_790MY.TabIndex = 1;
            this.lblTotal_790MY.Text = "$0";
            this.lblTotal_790MY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // lblTotalInfo_790MY
            //
            this.lblTotalInfo_790MY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalInfo_790MY.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.lblTotalInfo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblTotalInfo_790MY.Location = new System.Drawing.Point(0, 18);
            this.lblTotalInfo_790MY.Name = "lblTotalInfo_790MY";
            this.lblTotalInfo_790MY.Size = new System.Drawing.Size(180, 36);
            this.lblTotalInfo_790MY.TabIndex = 0;
            this.lblTotalInfo_790MY.Text = "0 ítems";
            this.lblTotalInfo_790MY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblComandaSub_790MY
            //
            this.lblComandaSub_790MY.AutoSize = true;
            this.lblComandaSub_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblComandaSub_790MY.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblComandaSub_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(110)))), ((int)(((byte)(100)))), ((int)(((byte)(95)))));
            this.lblComandaSub_790MY.Name = "lblComandaSub_790MY";
            this.lblComandaSub_790MY.TabIndex = 1;
            this.lblComandaSub_790MY.Text = "-";
            //
            // lblComandaTitulo_790MY
            //
            this.lblComandaTitulo_790MY.AutoSize = true;
            this.lblComandaTitulo_790MY.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblComandaTitulo_790MY.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblComandaTitulo_790MY.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(42)))), ((int)(((byte)(40)))));
            this.lblComandaTitulo_790MY.Name = "lblComandaTitulo_790MY";
            this.lblComandaTitulo_790MY.TabIndex = 0;
            this.lblComandaTitulo_790MY.Tag = "RP_comanda_titulo";
            this.lblComandaTitulo_790MY.Text = "Comanda en curso";
            //
            // timerAvisos_790MY
            //
            this.timerAvisos_790MY.Interval = 20000;
            this.timerAvisos_790MY.Tick += new System.EventHandler(this.timerAvisos_790MY_Tick);
            //
            // FormRegistrarPedido_790MY
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(248)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(1280, 760);
            this.Controls.Add(this.tlpMain_790MY);
            this.Controls.Add(this.pnlAviso_790MY);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormRegistrarPedido_790MY";
            this.Text = "Registrar Pedido";
            this.Load += new System.EventHandler(this.FormRegistrarPedido_790MY_Load);
            this.pnlAviso_790MY.ResumeLayout(false);
            this.tlpMain_790MY.ResumeLayout(false);
            this.pnlMesas_790MY.ResumeLayout(false);
            this.pnlMesas_790MY.PerformLayout();
            this.pnlCatalogo_790MY.ResumeLayout(false);
            this.tlpCatalogo_790MY.ResumeLayout(false);
            this.tlpCatalogo_790MY.PerformLayout();
            this.tlpCatalogoHeader_790MY.ResumeLayout(false);
            this.tlpCatalogoHeader_790MY.PerformLayout();
            this.flpCategorias_790MY.ResumeLayout(false);
            this.flpCategorias_790MY.PerformLayout();
            this.pnlEditor_790MY.ResumeLayout(false);
            this.tlpEditor_790MY.ResumeLayout(false);
            this.tlpEditor_790MY.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCantidad_790MY)).EndInit();
            this.pnlComanda_790MY.ResumeLayout(false);
            this.pnlComanda_790MY.PerformLayout();
            this.pnlComandaFooter_790MY.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlAviso_790MY;
        private System.Windows.Forms.Label lblAviso_790MY;
        private System.Windows.Forms.Button btnRetirado_790MY;
        private System.Windows.Forms.TableLayoutPanel tlpMain_790MY;
        private System.Windows.Forms.Panel pnlMesas_790MY;
        private System.Windows.Forms.FlowLayoutPanel flpMesas_790MY;
        private System.Windows.Forms.Label lblMisMesasSub_790MY;
        private System.Windows.Forms.Label lblMisMesas_790MY;
        private System.Windows.Forms.Panel pnlCatalogo_790MY;
        private System.Windows.Forms.TableLayoutPanel tlpCatalogo_790MY;
        private System.Windows.Forms.TableLayoutPanel tlpCatalogoHeader_790MY;
        private System.Windows.Forms.Label lblPedidoTitulo_790MY;
        private System.Windows.Forms.Label lblPedidoSub_790MY;
        private System.Windows.Forms.TextBox txtBuscarProducto_790MY;
        private System.Windows.Forms.FlowLayoutPanel flpCategorias_790MY;
        private System.Windows.Forms.Button btnCatEntrada_790MY;
        private System.Windows.Forms.Button btnCatPlatoPrincipal_790MY;
        private System.Windows.Forms.Button btnCatPostre_790MY;
        private System.Windows.Forms.Button btnCatBebida_790MY;
        private System.Windows.Forms.FlowLayoutPanel flpProductos_790MY;
        private GUI_08YS.RF2.PanelRedondeado_790MY pnlEditor_790MY;
        private System.Windows.Forms.Label lblEditorVacio_790MY;
        private System.Windows.Forms.TableLayoutPanel tlpEditor_790MY;
        private System.Windows.Forms.Label lblItemNombre_790MY;
        private System.Windows.Forms.Label lblCantidad_790MY;
        private System.Windows.Forms.Label lblObservaciones_790MY;
        private System.Windows.Forms.NumericUpDown nudCantidad_790MY;
        private System.Windows.Forms.TextBox txtObservaciones_790MY;
        private System.Windows.Forms.Button btnAgregarItem_790MY;
        private System.Windows.Forms.Panel pnlComanda_790MY;
        private System.Windows.Forms.FlowLayoutPanel flpItems_790MY;
        private System.Windows.Forms.Panel pnlComandaFooter_790MY;
        private FontAwesome.Sharp.IconButton btnEnviar_790MY;
        private System.Windows.Forms.Label lblTotal_790MY;
        private System.Windows.Forms.Label lblTotalInfo_790MY;
        private System.Windows.Forms.Label lblComandaSub_790MY;
        private System.Windows.Forms.Label lblComandaTitulo_790MY;
        private System.Windows.Forms.Timer timerAvisos_790MY;
    }
}
