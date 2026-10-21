using BE_08YS;
using BLL_08YS;
using BLL_08YS.Exceptions;
using FontAwesome.Sharp;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GUI_08YS.Estilo;
using E = GUI_08YS.RF2.EstiloRF2_790MY;

namespace GUI_08YS.RF2
{
    /// <summary>
    /// CUN-005 Registrar Pedido (Camarero). Muestra las mesas abiertas a cargo del
    /// camarero logueado, el catálogo por categoría con las porciones disponibles
    /// (stock físico - comprometido) y la comanda en curso. El envío a cocina lo
    /// valida y persiste PedidoBLL_790MY (stock, DV y bitácora).
    /// También consulta periódicamente los pedidos Listos de sus mesas (CUN-008 paso 8).
    /// </summary>
    public partial class FormRegistrarPedido_790MY : Form, IIdiomaObserver_08YS, IAyudaContextual_790MY
    {
        /// <summary>Renglón de la comanda todavía no enviada.</summary>
        private sealed class ItemComanda
        {
            public Producto_790MY Producto { get; set; }
            public int Cantidad { get; set; }
            public string Observaciones { get; set; }
            public decimal Subtotal => Producto.Precio * Cantidad;
        }

        private const int UmbralBajoStock = 3;

        private readonly PedidoBLL_790MY _pedidoBll;

        private static readonly Dictionary<string, Permisos> _mapaPermisos =
            new Dictionary<string, Permisos>
            {
                { nameof(btnEnviar_790MY), Permisos.RegistrarPedido },
            };

        private readonly Dictionary<Button, CategoriaProducto_790MY> _botonesCategoria;

        private List<Reserva_790MY> _mesas = new List<Reserva_790MY>();
        private List<Pedido_790MY> _pedidos = new List<Pedido_790MY>();
        private List<Producto_790MY> _catalogo = new List<Producto_790MY>();

        // Una comanda por mesa: cambiar de mesa no pierde lo que se estaba cargando.
        private readonly Dictionary<int, List<ItemComanda>> _comandas = new Dictionary<int, List<ItemComanda>>();
        // Avisos de "pedido listo" ya retirados en esta sesión de la pantalla.
        private readonly HashSet<int> _retirados = new HashSet<int>();

        private Reserva_790MY _mesaSel;
        private Producto_790MY _productoSel;
        private CategoriaProducto_790MY _categoriaSel = CategoriaProducto_790MY.PlatoPrincipal;
        private bool _puedeOperar = true;

        private readonly Font _fuenteMesa     = new Font("Segoe UI", 15F, FontStyle.Bold);
        private readonly Font _fuenteNombre   = new Font("Segoe UI", 11.25F);
        private readonly Font _fuenteNombreB  = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        private readonly Font _fuenteDetalle  = new Font("Segoe UI", 9.75F);
        private readonly Font _fuenteObs      = new Font("Segoe UI", 9.75F, FontStyle.Italic);
        private readonly Font _fuentePrecio   = new Font("Segoe UI", 13F, FontStyle.Bold);
        private readonly Font _fuenteChip     = new Font("Segoe UI", 9F, FontStyle.Bold);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        public FormRegistrarPedido_790MY()
        {
            InitializeComponent();

            _pedidoBll = BLLFactory_790MY.CreatePedidoBLL();

            _botonesCategoria = new Dictionary<Button, CategoriaProducto_790MY>
            {
                { btnCatEntrada_790MY,        CategoriaProducto_790MY.Entrada },
                { btnCatPlatoPrincipal_790MY, CategoriaProducto_790MY.PlatoPrincipal },
                { btnCatPostre_790MY,         CategoriaProducto_790MY.Postre },
                { btnCatBebida_790MY,         CategoriaProducto_790MY.Bebida },
            };

            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) =>
            {
                TraductorManager_08YS.Instance.Desuscribir(this);
                timerAvisos_790MY.Stop();
                foreach (var f in new[] { _fuenteMesa, _fuenteNombre, _fuenteNombreB, _fuenteDetalle, _fuenteObs, _fuentePrecio, _fuenteChip })
                    f.Dispose();
            };
        }

        private IWin32Window VentanaPropietaria => (this.TopLevelControl as Form) ?? (IWin32Window)this;

        private List<ItemComanda> ComandaActual
        {
            get
            {
                if (_mesaSel == null) return new List<ItemComanda>();
                if (!_comandas.TryGetValue(_mesaSel.ReservaID, out var lista))
                    _comandas[_mesaSel.ReservaID] = lista = new List<ItemComanda>();
                return lista;
            }
        }

        private void FormRegistrarPedido_790MY_Load(object sender, EventArgs e)
        {
            PermissionFilter_08YS.Aplicar(this, _mapaPermisos);

            UpdateIdioma();
            flpCategorias_790MY_Resize(this, EventArgs.Empty);
            CargarTodo();
            timerAvisos_790MY.Start();
        }

        #region Carga de datos

        private void CargarTodo()
        {
            // El catálogo solo depende de la patente: se carga aunque el usuario no
            // esté vinculado a un camarero, para que la carta siempre sea visible.
            try
            {
                _catalogo = _pedidoBll.ObtenerCatalogo();
            }
            catch (Exception ex)
            {
                _catalogo = new List<Producto_790MY>();
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
            }

            try
            {
                _mesas = _pedidoBll.ObtenerMesasAsignadas();
                _pedidos = _pedidoBll.ObtenerPedidosDeMisMesas();
                _puedeOperar = true;
            }
            catch (Exception ex)
            {
                // P. ej. el usuario no está vinculado a un camarero activo.
                _puedeOperar = false;
                _mesas = new List<Reserva_790MY>();
                _pedidos = new List<Pedido_790MY>();
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
            }

            // Se descartan las comandas de mesas que ya no están a cargo (cerradas o reasignadas).
            var abiertas = new HashSet<int>(_mesas.Select(m => m.ReservaID));
            foreach (int id in _comandas.Keys.Where(k => !abiertas.Contains(k)).ToList())
                _comandas.Remove(id);

            _mesaSel = _mesaSel != null ? _mesas.FirstOrDefault(m => m.ReservaID == _mesaSel.ReservaID) : null;
            if (_mesaSel == null && _mesas.Count > 0)
                _mesaSel = _mesas[0];

            RenderizarMesas();
            RenderizarCategorias();
            RenderizarProductos();
            RenderizarComanda();
            ActualizarAviso();
        }

        private void timerAvisos_790MY_Tick(object sender, EventArgs e)
        {
            if (!_puedeOperar) return;
            try
            {
                _pedidos = _pedidoBll.ObtenerPedidosDeMisMesas();
                RenderizarMesas();
                ActualizarAviso();
            }
            catch (Exception ex)
            {
                // Un fallo transitorio del sondeo no debe interrumpir la carga del pedido.
                System.Diagnostics.Trace.TraceWarning("Sondeo de pedidos listos: " + ex.Message);
            }
        }

        #endregion

        #region Mis mesas

        private void RenderizarMesas()
        {
            flpMesas_790MY.SuspendLayout();
            foreach (Control c in flpMesas_790MY.Controls.Cast<Control>().ToList())
                c.Dispose();
            flpMesas_790MY.Controls.Clear();

            int ancho = Math.Max(160, flpMesas_790MY.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 2);

            if (_mesas.Count == 0)
            {
                var vacio = E.CrearLabel(E.T("msg_rf2_sin_mesas_asignadas"), _fuenteDetalle, E.Gris, autoSize: false);
                vacio.Size = new Size(ancho, 80);
                vacio.TextAlign = ContentAlignment.MiddleCenter;
                flpMesas_790MY.Controls.Add(vacio);
            }

            foreach (var mesa in _mesas)
                flpMesas_790MY.Controls.Add(CrearTarjetaMesa(mesa, ancho));

            flpMesas_790MY.ResumeLayout();
        }

        private Control CrearTarjetaMesa(Reserva_790MY mesa, int ancho)
        {
            bool sel = _mesaSel != null && _mesaSel.ReservaID == mesa.ReservaID;

            var card = new PanelRedondeado_790MY
            {
                Width = ancho,
                Margin = new Padding(0, 0, 0, 12),
                BackColor = sel ? E.BordoSuave : Color.White,
                ColorBorde = sel ? E.Bordo : E.BordeCard,
                GrosorBorde = sel ? 2.5f : 1f,
                Radio = 12
            };

            // Grilla interna: título y comensales en la primera fila (izquierda / derecha),
            // debajo detalle, cliente y estados alineados al mismo margen izquierdo.
            const int margen = 16;
            int interior = ancho - margen * 2;
            var grilla = new TableLayoutPanel
            {
                Location = new Point(margen, 12),
                Width = interior,
                ColumnCount = 2,
                BackColor = Color.Transparent,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            grilla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            grilla.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            var lblMesa = E.CrearLabel(E.T("AM_card_mesa", mesa.MesaNumero), _fuenteMesa, E.Texto);
            lblMesa.Anchor = AnchorStyles.Left;
            var lblPax = E.CrearLabel(E.T("RP_pax", mesa.CantidadComensales), _fuenteNombre, E.Gris);
            lblPax.Anchor = AnchorStyles.Right;
            var lblDetalle = FilaTexto(E.T("RP_mesa_detalle", mesa.ReservaID, E.Hora(mesa.Hora)), _fuenteDetalle, E.Gris, interior);
            lblDetalle.Margin = new Padding(0, 4, 0, 0);
            var lblCliente = FilaTexto(mesa.ClienteNombreCompleto ?? "", _fuenteNombreB, E.Texto, interior);
            lblCliente.Margin = new Padding(0, 2, 0, 0);

            grilla.Controls.Add(lblMesa, 0, 0);
            grilla.Controls.Add(lblPax, 1, 0);
            grilla.Controls.Add(lblDetalle, 0, 1);
            grilla.SetColumnSpan(lblDetalle, 2);
            grilla.Controls.Add(lblCliente, 0, 2);
            grilla.SetColumnSpan(lblCliente, 2);

            // Etiquetas de estado de los pedidos de esa mesa (los más recientes al final).
            var chips = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                MaximumSize = new Size(interior, 0),
                BackColor = Color.Transparent,
                Margin = new Padding(0, 8, 0, 0),
                Padding = Padding.Empty
            };
            foreach (var pedido in _pedidos.Where(p => p.ReservaID == mesa.ReservaID).OrderBy(p => p.NroPedido))
            {
                Color fondo = pedido.Estado == EstadoPedido_790MY.Listo ? E.ListoFondo : E.PendFondo;
                Color texto = pedido.Estado == EstadoPedido_790MY.Listo ? E.ListoTexto : E.PendTexto;
                Color borde = pedido.Estado == EstadoPedido_790MY.Listo ? E.ListoBorde : E.PendBorde;
                var chip = E.CrearChip($"# {pedido.NroPedido} · {E.T("estado_pedido_" + pedido.Estado)}", fondo, texto, _fuenteChip, borde);
                chip.Margin = new Padding(0, 0, 6, 6);
                chips.Controls.Add(chip);
            }
            if (chips.Controls.Count > 0)
            {
                grilla.Controls.Add(chips, 0, 3);
                grilla.SetColumnSpan(chips, 2);
            }

            grilla.Height = grilla.GetPreferredSize(new Size(interior, 0)).Height;
            card.Controls.Add(grilla);
            card.Height = grilla.Height + 12 + (chips.Controls.Count > 0 ? 6 : 14);

            E.PropagarClick(card, (s, e) =>
            {
                _mesaSel = mesa;
                bnrAvisos_790MY.Ocultar();   // «Elegí primero la mesa» ya quedó resuelto.
                _productoSel = null;
                RenderizarMesas();
                RenderizarProductos();
                RenderizarComanda();
            });
            return card;
        }

        /// <summary>Renglón de texto de ancho fijo que corta con "…" en lugar de desbordar la tarjeta.</summary>
        private static Label FilaTexto(string texto, Font fuente, Color color, int ancho)
        {
            var lbl = E.CrearLabel(texto, fuente, color, autoSize: false);
            lbl.AutoEllipsis = true;
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            lbl.Size = new Size(ancho, TextRenderer.MeasureText("Ag", fuente).Height + 2);
            return lbl;
        }

        private void flpMesas_790MY_Resize(object sender, EventArgs e)
        {
            if (flpMesas_790MY.Controls.Count > 0) RenderizarMesas();
        }

        #endregion

        #region Catálogo

        private void btnCategoria_Click(object sender, EventArgs e)
        {
            _categoriaSel = _botonesCategoria[(Button)sender];
            txtBuscarProducto_790MY.Clear();
            RenderizarCategorias();
            RenderizarProductos();
        }

        private void RenderizarCategorias()
        {
            bool buscando = txtBuscarProducto_790MY.Text.Trim().Length > 0;
            foreach (var kv in _botonesCategoria)
            {
                bool activa = !buscando && kv.Value == _categoriaSel;
                kv.Key.BackColor = activa ? E.Bordo : Color.White;
                kv.Key.ForeColor = activa ? Color.White : E.Texto;
                kv.Key.FlatAppearance.BorderColor = activa ? E.Bordo : E.BordeCard;
                kv.Key.FlatAppearance.MouseOverBackColor = activa ? E.BordoHover : E.FondoPanel;
            }
        }

        // Las cuatro pestañas comparten el ancho disponible en partes iguales: así ninguna
        // queda recortada aunque el texto traducido sea largo o la columna se angoste.
        private void flpCategorias_790MY_Resize(object sender, EventArgs e)
        {
            var botones = _botonesCategoria?.Keys.ToList();
            if (botones == null || botones.Count == 0) return;

            int separacion = botones[0].Margin.Horizontal;
            int ancho = (flpCategorias_790MY.ClientSize.Width - separacion * botones.Count) / botones.Count;
            if (ancho < 60) return;

            flpCategorias_790MY.SuspendLayout();
            foreach (var b in botones)
                b.Width = ancho;
            flpCategorias_790MY.ResumeLayout();
        }

        private void txtBuscarProducto_790MY_TextChanged(object sender, EventArgs e)
        {
            RenderizarCategorias();
            RenderizarProductos();
        }

        /// <summary>Porciones que todavía pueden agregarse descontando lo ya cargado en la comanda.</summary>
        private const int MaximoSinControl = 99;

        private int DisponibleParaAgregar(Producto_790MY producto)
            => (producto.ControlaStock ? producto.PorcionesDisponibles : MaximoSinControl) - ComandaActual.Where(i => i.Producto.Equals(producto)).Sum(i => i.Cantidad);

        private void RenderizarProductos()
        {
            string filtro = txtBuscarProducto_790MY.Text.Trim();
            var visibles = (filtro.Length > 0
                    ? _catalogo.Where(p => p.Nombre.IndexOf(filtro, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    : _catalogo.Where(p => p.Categoria == _categoriaSel))
                .OrderBy(p => p.Nombre)
                .ToList();

            flpProductos_790MY.SuspendLayout();
            foreach (Control c in flpProductos_790MY.Controls.Cast<Control>().ToList())
                c.Dispose();
            flpProductos_790MY.Controls.Clear();

            int disponibleAncho = flpProductos_790MY.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 4;
            int columnas = Math.Max(1, disponibleAncho / 230);
            int ancho = Math.Max(180, disponibleAncho / columnas - 14);

            if (visibles.Count == 0)
            {
                var vacio = E.CrearLabel(E.T("RP_sin_productos"), _fuenteDetalle, E.Gris);
                vacio.Margin = new Padding(4, 12, 0, 0);
                flpProductos_790MY.Controls.Add(vacio);
            }

            foreach (var producto in visibles)
                flpProductos_790MY.Controls.Add(CrearTarjetaProducto(producto, ancho));

            flpProductos_790MY.ResumeLayout();
            ActualizarEditor();
        }

        private Control CrearTarjetaProducto(Producto_790MY producto, int ancho)
        {
            int disponible = DisponibleParaAgregar(producto);
            bool agotado = disponible <= 0;
            bool sel = producto.Equals(_productoSel);

            var card = new PanelRedondeado_790MY
            {
                Size = new Size(ancho, 128),
                Margin = new Padding(0, 0, 14, 14),
                BackColor = agotado ? E.FondoPanel : (sel ? E.BordoSuave : Color.White),
                ColorBorde = sel ? E.Bordo : E.BordeCard,
                GrosorBorde = sel ? 2f : 1f,
                EstiloBorde = agotado ? DashStyle.Dash : DashStyle.Solid,
                Radio = 12
            };

            var lblNombre = E.CrearLabel(producto.Nombre, _fuenteNombre, agotado ? E.Gris : E.Texto, autoSize: false);
            lblNombre.Location = new Point(16, 14);
            lblNombre.Size = new Size(ancho - 32, 52);

            var lblPrecio = E.CrearLabel(E.Moneda(producto.Precio), _fuentePrecio, agotado ? E.Gris : E.Texto);
            lblPrecio.Location = new Point(16, 88);

            EtiquetaChip_790MY chip;
            if (agotado)
                chip = E.CrearChip(E.T("RP_agotado"), E.Deshabilitado, E.Texto, _fuenteChip, E.Gris);
            else if (!producto.ControlaStock)
                chip = E.CrearChip(E.T("RP_sin_control_stock"), E.ListoFondo, E.ListoTexto, _fuenteChip, E.ListoBorde);
            else if (disponible <= UmbralBajoStock)
                chip = E.CrearChip(E.T("RP_bajo_stock", disponible), E.PendFondo, E.PendTexto, _fuenteChip, E.PendBorde);
            else
                chip = E.CrearChip(E.T("RP_disponible", disponible), E.ListoFondo, E.ListoTexto, _fuenteChip, E.ListoBorde);

            card.Controls.Add(lblNombre);
            card.Controls.Add(lblPrecio);
            card.Controls.Add(chip);
            chip.Location = new Point(ancho - chip.PreferredWidth - 16, 90);

            if (!agotado && _puedeOperar)
                E.PropagarClick(card, (s, e) => SeleccionarProducto(producto));

            return card;
        }

        private void SeleccionarProducto(Producto_790MY producto)
        {
            if (_mesaSel == null)
            {
                bnrAvisos_790MY.Mostrar(E.T("RP_msg_falta_mesa"), TipoBanner_790MY.Aviso);
                return;
            }

            _productoSel = producto;
            txtObservaciones_790MY.Clear();
            nudCantidad_790MY.Value = 1;
            RenderizarProductos();
            txtObservaciones_790MY.Focus();
        }

        private void flpProductos_790MY_Resize(object sender, EventArgs e)
        {
            if (flpProductos_790MY.Controls.Count > 0) RenderizarProductos();
        }

        #endregion

        #region Editor de ítem

        private void ActualizarEditor()
        {
            int disponible = _productoSel != null ? DisponibleParaAgregar(_productoSel) : 0;
            if (_productoSel != null && disponible <= 0)
                _productoSel = null;

            bool hay = _productoSel != null;
            tlpEditor_790MY.Visible = hay;
            lblEditorVacio_790MY.Visible = !hay;

            if (!hay) return;

            lblItemNombre_790MY.Text = $"{_productoSel.Nombre} · {E.Moneda(_productoSel.Precio)}";
            nudCantidad_790MY.Maximum = disponible;
            if (nudCantidad_790MY.Value > disponible) nudCantidad_790MY.Value = disponible;
        }

        private void txtObservaciones_790MY_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            btnAgregarItem_790MY.PerformClick();
        }

        private void btnAgregarItem_790MY_Click(object sender, EventArgs e)
        {
            if (_productoSel == null || _mesaSel == null) return;

            int cantidad = (int)nudCantidad_790MY.Value;
            string obs = txtObservaciones_790MY.Text.Trim();
            string obsNormalizada = obs.Length == 0 ? null : obs;

            // Mismo producto con la misma observación: se suma la cantidad al renglón existente.
            var existente = ComandaActual.FirstOrDefault(i => i.Producto.Equals(_productoSel)
                && string.Equals(i.Observaciones, obsNormalizada, StringComparison.CurrentCultureIgnoreCase));
            if (existente != null)
                existente.Cantidad += cantidad;
            else
                ComandaActual.Add(new ItemComanda { Producto = _productoSel, Cantidad = cantidad, Observaciones = obsNormalizada });

            _productoSel = null;
            RenderizarProductos();
            RenderizarComanda();
        }

        #endregion

        #region Comanda en curso

        private void RenderizarComanda()
        {
            lblPedidoTitulo_790MY.Text = _mesaSel != null
                ? E.T("RP_titulo_mesa", _mesaSel.MesaNumero)
                : E.T("RP_titulo");
            lblPedidoSub_790MY.Text = _mesaSel != null
                ? E.T("RP_sub_mesa", _mesaSel.ReservaID, _mesaSel.CantidadComensales, _mesaSel.ClienteNombreCompleto)
                : E.T("RP_sub_sin_mesa");
            lblComandaSub_790MY.Text = _mesaSel != null
                ? E.T("RP_comanda_sub", _mesaSel.MesaNumero, E.T("estado_pedido_Pendiente"))
                : E.T("RP_sub_sin_mesa");

            flpItems_790MY.SuspendLayout();
            foreach (Control c in flpItems_790MY.Controls.Cast<Control>().ToList())
                c.Dispose();
            flpItems_790MY.Controls.Clear();

            int ancho = Math.Max(200, flpItems_790MY.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 2);
            var items = ComandaActual;

            if (items.Count == 0)
            {
                var vacio = E.CrearLabel(E.T("RP_comanda_vacia"), _fuenteDetalle, E.Gris, autoSize: false);
                vacio.Size = new Size(ancho, 60);
                vacio.TextAlign = ContentAlignment.MiddleCenter;
                flpItems_790MY.Controls.Add(vacio);
            }

            foreach (var item in items)
                flpItems_790MY.Controls.Add(CrearRenglon(item, ancho));

            flpItems_790MY.ResumeLayout();

            int unidades = items.Sum(i => i.Cantidad);
            lblTotalInfo_790MY.Text = E.T("RP_total_info", unidades, DateTime.Today.ToString("dd/MM/yyyy"));
            lblTotal_790MY.Text = E.Moneda(items.Sum(i => i.Subtotal));

            bool habilitado = _puedeOperar && _mesaSel != null && items.Count > 0;
            btnEnviar_790MY.Enabled = habilitado;
            btnEnviar_790MY.BackColor = habilitado ? E.Bordo : E.Deshabilitado;
            btnEnviar_790MY.ForeColor = btnEnviar_790MY.IconColor = habilitado ? Color.White : E.Gris;
        }

        private Control CrearRenglon(ItemComanda item, int ancho)
        {
            var card = new PanelRedondeado_790MY
            {
                Size = new Size(ancho, item.Observaciones != null ? 98 : 78),
                Margin = new Padding(0, 0, 0, 10),
                BackColor = E.Fondo,
                ColorBorde = E.BordeCard,
                Radio = 10
            };

            var cantidad = new EtiquetaChip_790MY
            {
                AutoSize = false,
                Size = new Size(46, 46),
                Location = new Point(14, 14),
                Padding = Padding.Empty,
                Text = item.Cantidad + "×",
                Font = _fuenteNombreB,
                BackColor = E.Bordo,
                ForeColor = Color.White
            };

            var lblNombre = E.CrearLabel(item.Producto.Nombre, _fuenteNombreB, E.Texto, autoSize: false);
            lblNombre.Location = new Point(72, 12);
            lblNombre.Size = new Size(ancho - 72 - 100, 22);

            var lblSubtotal = E.CrearLabel(E.Moneda(item.Subtotal), _fuenteNombreB, E.Texto);

            int y = 36;
            if (item.Observaciones != null)
            {
                var lblObs = E.CrearLabel(E.T("RP_obs", item.Observaciones), _fuenteObs, E.PendTexto, autoSize: false);
                lblObs.Location = new Point(72, y);
                lblObs.Size = new Size(ancho - 72 - 60, 20);
                card.Controls.Add(lblObs);
                y += 22;
            }
            var lblUnit = E.CrearLabel(E.T("RP_precio_unitario", E.Moneda(item.Producto.Precio)), _fuenteDetalle, E.Gris);
            lblUnit.Location = new Point(72, y);

            var btnQuitar = new IconButton
            {
                IconChar = IconChar.TrashAlt,
                IconColor = E.Bordo,
                IconSize = 18,
                Size = new Size(34, 34),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                Cursor = Cursors.Hand
            };
            btnQuitar.FlatAppearance.BorderColor = E.BordeCard;
            btnQuitar.Click += (s, e) =>
            {
                ComandaActual.Remove(item);
                RenderizarProductos();
                RenderizarComanda();
            };

            card.Controls.Add(cantidad);
            card.Controls.Add(lblNombre);
            card.Controls.Add(lblSubtotal);
            card.Controls.Add(lblUnit);
            card.Controls.Add(btnQuitar);
            lblSubtotal.Location = new Point(ancho - lblSubtotal.PreferredWidth - 14, 12);
            btnQuitar.Location = new Point(ancho - btnQuitar.Width - 14, card.Height - btnQuitar.Height - 10);

            return card;
        }

        private void flpItems_790MY_Resize(object sender, EventArgs e)
        {
            if (flpItems_790MY.Controls.Count > 0) RenderizarComanda();
        }

        private void btnEnviar_790MY_Click(object sender, EventArgs e)
        {
            var items = ComandaActual;
            if (_mesaSel == null || items.Count == 0) return;

            Pedido_790MY pedido;
            try
            {
                // El precio vigente lo toma la DAL dentro de la transacción; aquí solo
                // viajan producto, cantidad y observaciones.
                var renglones = items
                    .Select(i => new ItemPedido_790MY(i.Producto.ProductoID, i.Cantidad, i.Observaciones))
                    .ToList();
                pedido = _pedidoBll.RegistrarPedido(_mesaSel.ReservaID, renglones);
            }
            catch (StockInsuficienteException_790MY ex)
            {
                // Escenario alternativo 4.1: no se generó el pedido. Se refresca el catálogo
                // para que el camarero vea las porciones reales y ajuste la comanda.
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
                RecargarCatalogo();
                return;
            }
            catch (Exception ex)
            {
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
                CargarTodo();
                return;
            }

            _comandas.Remove(_mesaSel.ReservaID);
            bnrAvisos_790MY.Ocultar();
            CargarTodo();
            E.MostrarExitoToast(this, E.T("msg_rf2_pedido_registrado_ok", pedido.NroPedido));
        }

        private void RecargarCatalogo()
        {
            try { _catalogo = _pedidoBll.ObtenerCatalogo(); }
            catch (Exception ex) { E.MostrarErrorEnBanner(bnrAvisos_790MY, ex); }
            RenderizarProductos();
            RenderizarComanda();
        }

        #endregion

        #region Aviso de pedidos listos

        private Pedido_790MY ProximoListo()
            => _pedidos
                .Where(p => p.Estado == EstadoPedido_790MY.Listo && !_retirados.Contains(p.NroPedido))
                .OrderBy(p => p.HoraFin ?? p.Hora)
                .FirstOrDefault();

        private void ActualizarAviso()
        {
            var listo = ProximoListo();
            pnlAviso_790MY.Visible = listo != null;
            if (listo != null)
                lblAviso_790MY.Text = E.T("RP_aviso_listo", listo.NroPedido, listo.MesaNumero);
        }

        private void btnRetirado_790MY_Click(object sender, EventArgs e)
        {
            var listo = ProximoListo();
            if (listo != null) _retirados.Add(listo.NroPedido);
            ActualizarAviso();
        }

        #endregion

        #region Estética

        private void tlpMain_790MY_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            if (e.Column == 0) return;
            using (var linea = new Pen(E.BordeCard))
                e.Graphics.DrawLine(linea, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);
        }

        private void pnlAviso_790MY_Paint(object sender, PaintEventArgs e)
        {
            using (var linea = new Pen(E.ListoBorde))
                e.Graphics.DrawLine(linea, 0, pnlAviso_790MY.Height - 1, pnlAviso_790MY.Width, pnlAviso_790MY.Height - 1);
        }

        private void pnlComandaFooter_790MY_Paint(object sender, PaintEventArgs e)
        {
            using (var linea = new Pen(E.BordeCard))
                e.Graphics.DrawLine(linea, 0, 0, pnlComandaFooter_790MY.Width, 0);
        }

        #endregion

        #region i18n

        public void UpdateIdioma()
        {
            E.TraducirTags(this);
            SendMessage(txtBuscarProducto_790MY.Handle, EM_SETCUEBANNER, (IntPtr)1, E.T("RP_buscar_placeholder"));
            SendMessage(txtObservaciones_790MY.Handle, EM_SETCUEBANNER, (IntPtr)1, E.T("RP_obs_placeholder"));

            if (_pedidoBll == null) return; // aún en construcción
            RenderizarMesas();
            RenderizarCategorias();
            RenderizarProductos();
            RenderizarComanda();
            ActualizarAviso();
        }

        #endregion

        #region Ayuda contextual (Etapa 5)
        /// <summary>Sugerencias del Chef (F1): solo datos; el globo y el resalte los dibuja AyudaSugerencias_790MY.</summary>
        public IReadOnlyList<PistaAyuda_790MY> ObtenerPistas() => new[]
        {
            new PistaAyuda_790MY(nameof(flpMesas_790MY), "AY_sug_rp1_t", "RP_mis_mesas_sub"),
            new PistaAyuda_790MY(nameof(txtBuscarProducto_790MY), "AY_sug_rp2_t", "AY_sug_rp2_x"),
            new PistaAyuda_790MY(nameof(btnAgregarItem_790MY), "AY_sug_rp3_t", "AY_sug_rp3_x"),
            new PistaAyuda_790MY(nameof(btnEnviar_790MY), "AY_sug_rp4_t", "AY_sug_rp4_x"),
        };
        #endregion
    }
}
