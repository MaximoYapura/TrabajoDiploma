using BE_08YS;
using BLL_08YS;
using FontAwesome.Sharp;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using E = GUI_08YS.RF2.EstiloRF2_790MY;

namespace GUI_08YS.RF2
{
    /// <summary>
    /// CUN-008 Despachar Pedido (Chef). Pantalla KDS en columnas: Pendientes,
    /// En preparación y Listas para retirar. "Marcar como listo" invoca
    /// PedidoBLL_790MY.MarcarPedidoListo, que en una misma transacción cambia el estado
    /// y descuenta el stock (con DV y bitácora). El camarero lo ve en Registrar Pedido.
    /// </summary>
    public partial class FormCocinaKDS_790MY : Form, IIdiomaObserver_08YS
    {
        /// <summary>Minutos desde la solicitud a partir de los cuales la comanda se marca demorada.</summary>
        private const int MinutosDemora = 20;

        private readonly PedidoBLL_790MY _pedidoBll;

        private static readonly Dictionary<string, Permisos> _mapaPermisos =
            new Dictionary<string, Permisos>
            {
                { nameof(btnActualizar_790MY), Permisos.DespacharPedido },
            };

        private List<Pedido_790MY> _comandas = new List<Pedido_790MY>();
        private List<Pedido_790MY> _listos = new List<Pedido_790MY>();
        private Dictionary<int, Empleado_790MY> _camareros = new Dictionary<int, Empleado_790MY>();

        private readonly Font _fuentePedido  = new Font("Segoe UI", 15F, FontStyle.Bold);
        private readonly Font _fuenteMesa    = new Font("Segoe UI", 11.25F, FontStyle.Bold);
        private readonly Font _fuenteDetalle = new Font("Segoe UI", 9.75F);
        private readonly Font _fuenteItem    = new Font("Segoe UI", 12.5F);
        private readonly Font _fuenteCant    = new Font("Segoe UI", 12.5F, FontStyle.Bold);
        private readonly Font _fuenteChip    = new Font("Segoe UI", 9.75F, FontStyle.Bold);
        private readonly Font _fuenteBoton   = new Font("Segoe UI", 12F, FontStyle.Bold);

        public FormCocinaKDS_790MY()
        {
            InitializeComponent();

            _pedidoBll = BLLFactory_790MY.CreatePedidoBLL();

            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) =>
            {
                TraductorManager_08YS.Instance.Desuscribir(this);
                timerKds_790MY.Stop();
                foreach (var f in new[] { _fuentePedido, _fuenteMesa, _fuenteDetalle, _fuenteItem, _fuenteCant, _fuenteChip, _fuenteBoton })
                    f.Dispose();
            };
        }

        private IWin32Window VentanaPropietaria => (this.TopLevelControl as Form) ?? (IWin32Window)this;

        private void FormCocinaKDS_790MY_Load(object sender, EventArgs e)
        {
            PermissionFilter_08YS.Aplicar(this, _mapaPermisos);

            UpdateIdioma();
            try
            {
                _camareros = _pedidoBll.ObtenerCamarerosPorDni();
            }
            catch (Exception ex)
            {
                E.MostrarError(VentanaPropietaria, ex);
            }
            Recargar(mostrarErrores: true);
            timerKds_790MY.Start();
        }

        #region Datos

        private void Recargar(bool mostrarErrores)
        {
            try
            {
                _comandas = _pedidoBll.ObtenerComandasCocina();
                _listos = _pedidoBll.ObtenerListosDelDia();
            }
            catch (Exception ex)
            {
                if (mostrarErrores)
                    E.MostrarError(VentanaPropietaria, ex);
                else
                    System.Diagnostics.Trace.TraceWarning("Actualización KDS: " + ex.Message);
            }
            Renderizar();
        }

        private void timerKds_790MY_Tick(object sender, EventArgs e) => Recargar(mostrarErrores: false);

        private void btnActualizar_790MY_Click(object sender, EventArgs e) => Recargar(mostrarErrores: true);

        private void flpColumna_Resize(object sender, EventArgs e)
        {
            var flp = (FlowLayoutPanel)sender;
            if (flp.Controls.Count > 0) Renderizar();
        }

        #endregion

        #region Render

        private void Renderizar()
        {
            var pendientes = _comandas.Where(p => p.Estado == EstadoPedido_790MY.Pendiente).ToList();
            var enPreparacion = _comandas.Where(p => p.Estado == EstadoPedido_790MY.EnPreparacion).ToList();

            chipPendientes_790MY.Text = E.T("KDS_chip_pendientes", pendientes.Count);
            chipPreparacion_790MY.Text = E.T("KDS_chip_preparacion", enPreparacion.Count);
            chipListas_790MY.Text = E.T("KDS_chip_listas", _listos.Count);
            chipCntPendientes_790MY.Text = pendientes.Count.ToString();
            chipCntPreparacion_790MY.Text = enPreparacion.Count.ToString();
            chipCntListas_790MY.Text = _listos.Count.ToString();
            lblReloj_790MY.Text = DateTime.Now.ToString("HH:mm");

            LlenarColumna(flpPendientes_790MY, pendientes, "msg_rf2_sin_comandas");
            LlenarColumna(flpPreparacion_790MY, enPreparacion, "KDS_sin_preparacion");
            LlenarColumna(flpListas_790MY, _listos, "KDS_sin_listas");
        }

        private void LlenarColumna(FlowLayoutPanel flp, List<Pedido_790MY> pedidos, string claveVacio)
        {
            // Se conserva la posición de scroll al refrescar periódicamente.
            int scroll = flp.VerticalScroll.Value;

            flp.SuspendLayout();
            foreach (Control c in flp.Controls.Cast<Control>().ToList())
                c.Dispose();
            flp.Controls.Clear();

            int ancho = Math.Max(220, flp.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 4);

            if (pedidos.Count == 0)
            {
                var vacio = E.CrearLabel(E.T(claveVacio), _fuenteDetalle, E.KdsGris, autoSize: false);
                vacio.Size = new Size(ancho, 80);
                vacio.TextAlign = ContentAlignment.MiddleCenter;
                flp.Controls.Add(vacio);
            }

            foreach (var pedido in pedidos)
                flp.Controls.Add(CrearTicket(pedido, ancho));

            flp.ResumeLayout();
            if (scroll > 0 && flp.VerticalScroll.Visible)
                flp.AutoScrollPosition = new Point(0, scroll);
        }

        private Control CrearTicket(Pedido_790MY pedido, int ancho)
        {
            bool listo = pedido.Estado == EstadoPedido_790MY.Listo;

            var card = new PanelRedondeado_790MY
            {
                Width = ancho,
                Margin = new Padding(0, 0, 0, 14),
                BackColor = listo ? Color.FromArgb(38, 56, 44) : E.KdsTarjeta,
                ColorBorde = listo ? Color.FromArgb(70, 120, 85) : E.KdsBorde,
                Radio = 12
            };

            int x = 18, y = 14, interior = ancho - 36;

            // Encabezado: número de pedido + mesa
            var lblPedido = E.CrearLabel(E.T("KDS_pedido_nro", pedido.NroPedido), _fuentePedido, E.KdsTexto);
            lblPedido.Location = new Point(x, y);
            var chipMesa = E.CrearChip(E.T("KDS_mesa", pedido.MesaNumero).ToUpper(), E.KdsTexto, E.KdsFondo, _fuenteMesa);
            chipMesa.Padding = new Padding(10, 4, 10, 4);
            card.Controls.Add(lblPedido);
            card.Controls.Add(chipMesa);
            chipMesa.Location = new Point(ancho - chipMesa.PreferredWidth - 18, y + 2);
            y += 40;

            if (!listo)
            {
                // Camarero, hora de solicitud y tiempo transcurrido
                string camarero = pedido.CamareroDNI.HasValue && _camareros.TryGetValue(pedido.CamareroDNI.Value, out var emp)
                    ? E.NombreCorto(emp) : "-";
                var lblDetalle = E.CrearLabel(E.T("KDS_solicitado", camarero, E.Hora(pedido.Hora)), _fuenteDetalle, E.KdsGris);
                lblDetalle.Location = new Point(x, y + 4);
                card.Controls.Add(lblDetalle);

                int minutos = Math.Max(0, (int)(DateTime.Now - (pedido.Fecha.Date + pedido.Hora)).TotalMinutes);
                bool demorado = minutos >= MinutosDemora;
                var chipTiempo = demorado
                    ? E.CrearChip(E.T("KDS_demorado", minutos), E.KdsRojoFondo, E.KdsRojoTexto, _fuenteChip)
                    : E.CrearChip(E.T("KDS_minutos", minutos), E.KdsColumna, E.KdsTexto, _fuenteChip, E.KdsBorde);
                card.Controls.Add(chipTiempo);
                chipTiempo.Location = new Point(ancho - chipTiempo.PreferredWidth - 18, y);
                y += 38;
            }

            // Ítems con sus observaciones
            foreach (var item in pedido.Items.OrderBy(i => i.NroItem))
            {
                var lblCant = E.CrearLabel(item.Cantidad + "×", _fuenteCant, listo ? E.KdsVerde : ColorAcento(pedido.Estado));
                lblCant.Location = new Point(x, y);
                var lblItem = E.CrearLabel(item.ProductoNombre, _fuenteItem, E.KdsTexto, autoSize: false);
                lblItem.Location = new Point(x + 46, y);
                lblItem.Size = new Size(interior - 46, 26);
                card.Controls.Add(lblCant);
                card.Controls.Add(lblItem);
                y += 30;

                if (!string.IsNullOrWhiteSpace(item.Observaciones))
                {
                    var chipObs = E.CrearChip("⚠ " + item.Observaciones, Color.FromArgb(70, 56, 30), E.KdsAmbar, _fuenteChip);
                    chipObs.MaximumSize = new Size(interior - 46, 0);
                    chipObs.Location = new Point(x + 46, y);
                    card.Controls.Add(chipObs);
                    y += chipObs.PreferredHeight + 6;
                }
            }
            y += 6;

            if (listo)
            {
                string camarero = pedido.CamareroDNI.HasValue && _camareros.TryGetValue(pedido.CamareroDNI.Value, out var emp)
                    ? E.NombreCorto(emp) : "-";
                var barra = new Label
                {
                    Text = E.T("KDS_finalizado", pedido.HoraFin.HasValue ? E.Hora(pedido.HoraFin.Value) : "-", camarero),
                    Font = _fuenteChip,
                    ForeColor = E.KdsVerde,
                    BackColor = Color.FromArgb(28, 72, 40),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(10, 0, 10, 0),
                    Location = new Point(x, y),
                    Size = new Size(interior, 34)
                };
                card.Controls.Add(barra);
                y += 42;

                string descontado = string.Join(" · ", pedido.Items
                    .GroupBy(i => i.ProductoNombre)
                    .Select(g => $"{g.Key} −{g.Sum(i => i.Cantidad)}"));
                var lblStock = E.CrearLabel(E.T("KDS_stock_descontado", descontado), _fuenteDetalle, E.KdsGris, autoSize: false);
                lblStock.Location = new Point(x, y);
                lblStock.Size = new Size(interior, TextRenderer.MeasureText(lblStock.Text, _fuenteDetalle,
                    new Size(interior, 0), TextFormatFlags.WordBreak).Height + 4);
                card.Controls.Add(lblStock);
                y += lblStock.Height + 10;
            }
            else
            {
                bool pendiente = pedido.Estado == EstadoPedido_790MY.Pendiente;
                var boton = new IconButton
                {
                    Text = pendiente ? E.T("KDS_btn_iniciar") : E.T("KDS_btn_listo"),
                    IconChar = pendiente ? IconChar.Play : IconChar.Check,
                    IconColor = E.KdsFondo,
                    IconSize = 22,
                    TextImageRelation = TextImageRelation.ImageBeforeText,
                    ImageAlign = ContentAlignment.MiddleCenter,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = _fuenteBoton,
                    ForeColor = E.KdsFondo,
                    BackColor = pendiente ? E.KdsAzul : E.KdsVerde,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Location = new Point(x, y),
                    Size = new Size(interior, 52),
                    Enabled = SessionManager_08YS.Instance.HasPermission(Permisos.DespacharPedido)
                };
                boton.FlatAppearance.BorderSize = 0;
                boton.Click += (s, e) =>
                {
                    if (pendiente) IniciarPreparacion(pedido);
                    else MarcarListo(pedido);
                };
                card.Controls.Add(boton);
                y += 66;
            }

            card.Height = y;
            return card;
        }

        private static Color ColorAcento(EstadoPedido_790MY estado)
            => estado == EstadoPedido_790MY.Pendiente ? Color.FromArgb(240, 140, 120) : E.KdsAzul;

        #endregion

        #region Acciones

        private void IniciarPreparacion(Pedido_790MY pedido)
        {
            try
            {
                _pedidoBll.IniciarPreparacion(pedido.NroPedido);
            }
            catch (Exception ex)
            {
                E.MostrarError(VentanaPropietaria, ex);
            }
            Recargar(mostrarErrores: true);
        }

        private void MarcarListo(Pedido_790MY pedido)
        {
            try
            {
                // Cambio de estado + descuento efectivo del inventario en una transacción.
                Pedido_790MY despachado = _pedidoBll.MarcarPedidoListo(pedido.NroPedido);
                E.MostrarExito(VentanaPropietaria, E.T("msg_rf2_pedido_listo_ok", despachado?.NroPedido ?? pedido.NroPedido));
            }
            catch (Exception ex)
            {
                E.MostrarError(VentanaPropietaria, ex);
            }
            Recargar(mostrarErrores: true);
        }

        #endregion

        #region i18n

        public void UpdateIdioma()
        {
            E.TraducirTags(this);
            if (_pedidoBll == null) return; // aún en construcción
            Renderizar();
        }

        #endregion
    }
}
