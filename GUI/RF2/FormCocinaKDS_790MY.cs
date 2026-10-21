using BE_08YS;
using BLL_08YS;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI_08YS.Estilo;
using E = GUI_08YS.RF2.EstiloRF2_790MY;
using NivelSemaforo = GUI_08YS.RF2.TicketKds_790MY.NivelSemaforo;

namespace GUI_08YS.RF2
{
    /// <summary>
    /// CUN-008 Despachar Pedido (Chef). Pantalla KDS en columnas: Pendientes,
    /// En preparación y Listas para retirar. "Marcar como listo" invoca
    /// PedidoBLL_790MY.MarcarPedidoListo, que en una misma transacción cambia el estado
    /// y descuenta el stock (con DV y bitácora). El camarero lo ve en Registrar Pedido.
    /// </summary>
    public partial class FormCocinaKDS_790MY : Form, IIdiomaObserver_08YS, IAyudaContextual_790MY
    {
        private readonly PedidoBLL_790MY _pedidoBll;

        private static readonly Dictionary<string, Permisos> _mapaPermisos =
            new Dictionary<string, Permisos>
            {
                { nameof(btnActualizar_790MY), Permisos.DespacharPedido },
            };

        private List<Pedido_790MY> _comandas = new List<Pedido_790MY>();
        private List<Pedido_790MY> _listos = new List<Pedido_790MY>();
        private Dictionary<int, Empleado_790MY> _camareros = new Dictionary<int, Empleado_790MY>();

        /// <summary>
        /// Etapa 3 (Opción C): cada pedido en pantalla tiene un único ticket, que se
        /// actualiza en el lugar y se mueve de columna; solo se crean los pedidos nuevos
        /// y solo se destruyen los que ya no vienen en la consulta.
        /// </summary>
        private readonly Dictionary<int, TicketKds_790MY> _tickets = new Dictionary<int, TicketKds_790MY>();

        /// <summary>Aviso «sin comandas» de cada columna; se muestra u oculta, no se recrea.</summary>
        private readonly Dictionary<FlowLayoutPanel, Label> _avisosVacio = new Dictionary<FlowLayoutPanel, Label>();

        private readonly FuentesKds_790MY _fuentes = new FuentesKds_790MY();

        public FormCocinaKDS_790MY()
        {
            InitializeComponent();

            _pedidoBll = BLLFactory_790MY.CreatePedidoBLL();

            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) =>
            {
                TraductorManager_08YS.Instance.Desuscribir(this);
                timerKds_790MY.Stop();
                _fuentes.Dispose();
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
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
            }
            Recargar(mostrarErrores: true);
            timerKds_790MY.Start();
        }

        #region Datos

        private void Recargar(bool mostrarErrores)
        {
            // Tiempos de consulta y redibujado (criterio E3-D5: redibujado < 50 ms),
            // para poder medir en el equipo real de la cocina.
            var reloj = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                _comandas = _pedidoBll.ObtenerComandasCocina();
                _listos = _pedidoBll.ObtenerListosDelDia();
            }
            catch (Exception ex)
            {
                if (mostrarErrores)
                    E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
                else
                    System.Diagnostics.Trace.TraceWarning("Actualización KDS: " + ex.Message);
            }
            long msConsulta = reloj.ElapsedMilliseconds;
            reloj.Restart();
            Renderizar();
            System.Diagnostics.Trace.WriteLine(
                $"consulta {msConsulta} ms, redibujado {reloj.ElapsedMilliseconds} ms ({_comandas.Count + _listos.Count} tickets)", "KDS");
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

            var niveles = _comandas.Select(p => TicketKds_790MY.Nivel(TicketKds_790MY.MinutosEspera(p))).ToList();
            chipATiempo_790MY.Text = E.T("KDS_sem_a_tiempo", niveles.Count(n => n == NivelSemaforo.ATiempo));
            chipAtencion_790MY.Text = E.T("KDS_sem_atencion", niveles.Count(n => n == NivelSemaforo.Atencion));
            chipDemorado_790MY.Text = E.T("KDS_sem_demorado", niveles.Count(n => n == NivelSemaforo.Demorado));

            var columnas = new[] { flpPendientes_790MY, flpPreparacion_790MY, flpListas_790MY };
            foreach (var flp in columnas) flp.SuspendLayout();

            var vigentes = new HashSet<int>();
            SincronizarColumna(flpPendientes_790MY, pendientes, "msg_rf2_sin_comandas", vigentes);
            SincronizarColumna(flpPreparacion_790MY, enPreparacion, "KDS_sin_preparacion", vigentes);
            SincronizarColumna(flpListas_790MY, _listos, "KDS_sin_listas", vigentes);

            // Bajas: solo se destruyen los tickets de pedidos que ya no vienen en la consulta.
            foreach (int nro in _tickets.Keys.Where(k => !vigentes.Contains(k)).ToList())
            {
                _tickets[nro].Dispose();
                _tickets.Remove(nro);
            }

            foreach (var flp in columnas) flp.ResumeLayout();
        }

        private void SincronizarColumna(FlowLayoutPanel flp, List<Pedido_790MY> pedidos, string claveVacio, HashSet<int> vigentes)
        {
            int ancho = Math.Max(220, flp.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 4);
            int scroll = flp.VerticalScroll.Value;

            var vacio = AvisoVacio(flp);
            vacio.Text = E.T(claveVacio);
            vacio.Width = ancho;
            vacio.Visible = pedidos.Count == 0;

            // El aviso ocupa el índice 0; los tickets van detrás, en el orden de la consulta.
            for (int i = 0; i < pedidos.Count; i++)
            {
                var pedido = pedidos[i];
                vigentes.Add(pedido.NroPedido);

                if (!_tickets.TryGetValue(pedido.NroPedido, out var ticket))
                {
                    // Alta: solo se instancia el ticket del pedido nuevo.
                    ticket = new TicketKds_790MY(_fuentes, NombreCamarero,
                        SessionManager_08YS.Instance.HasPermission(Permisos.DespacharPedido),
                        IniciarPreparacion, MarcarListo);
                    _tickets.Add(pedido.NroPedido, ticket);
                }
                ticket.Aplicar(pedido, ancho);

                // Cambio de columna: el mismo control pasa de panel, sin destruirse.
                if (ticket.Parent != flp)
                    ticket.Parent = flp;
                if (flp.Controls.GetChildIndex(ticket) != i + 1)
                    flp.Controls.SetChildIndex(ticket, i + 1);
            }

            // Se conserva la posición de scroll al refrescar periódicamente.
            if (scroll > 0 && flp.VerticalScroll.Visible && flp.VerticalScroll.Value != scroll)
                flp.AutoScrollPosition = new Point(0, scroll);
        }

        private Label AvisoVacio(FlowLayoutPanel flp)
        {
            if (!_avisosVacio.TryGetValue(flp, out var aviso))
            {
                aviso = E.CrearLabel("", _fuentes.Detalle, E.KdsGris, autoSize: false);
                aviso.Height = 80;
                aviso.TextAlign = ContentAlignment.MiddleCenter;
                flp.Controls.Add(aviso);
                flp.Controls.SetChildIndex(aviso, 0);
                _avisosVacio.Add(flp, aviso);
            }
            return aviso;
        }

        private string NombreCamarero(int? dni)
            => dni.HasValue && _camareros.TryGetValue(dni.Value, out var emp) ? E.NombreCorto(emp) : "-";

        #endregion

        #region Acciones

        private void IniciarPreparacion(Pedido_790MY pedido)
        {
            bnrAvisos_790MY.Ocultar();
            try
            {
                _pedidoBll.IniciarPreparacion(pedido.NroPedido);
            }
            catch (Exception ex)
            {
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
            }
            Recargar(mostrarErrores: true);
        }

        private void MarcarListo(Pedido_790MY pedido)
        {
            bnrAvisos_790MY.Ocultar();
            try
            {
                // Cambio de estado + descuento efectivo del inventario en una transacción.
                Pedido_790MY despachado = _pedidoBll.MarcarPedidoListo(pedido.NroPedido);
                E.MostrarExitoToast(this, E.T("msg_rf2_pedido_listo_ok", despachado?.NroPedido ?? pedido.NroPedido));
            }
            catch (Exception ex)
            {
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
            }
            Recargar(mostrarErrores: true);
        }

        #endregion

        #region i18n

        public void UpdateIdioma()
        {
            E.TraducirTags(this);
            if (_pedidoBll == null) return; // aún en construcción
            foreach (var ticket in _tickets.Values)
                ticket.Invalidar(); // los textos cambian de idioma: se rearma cada ticket
            Renderizar();
        }

        #endregion

        #region Ayuda contextual (Etapa 5)
        /// <summary>Sugerencias del Chef (F1): solo datos; el globo y el resalte los dibuja AyudaSugerencias_790MY.</summary>
        public IReadOnlyList<PistaAyuda_790MY> ObtenerPistas() => new[]
        {
            new PistaAyuda_790MY(nameof(flpPendientes_790MY), "AY_sug_kds1_t", "AY_sug_kds1_x"),
            new PistaAyuda_790MY(nameof(flpPreparacion_790MY), "AY_sug_kds2_t", "KDS_subtitulo"),
            new PistaAyuda_790MY(nameof(flpSemaforo_790MY), "AY_sug_kds3_t", "AY_sug_kds3_x"),
            new PistaAyuda_790MY(nameof(btnActualizar_790MY), "AY_sug_kds4_t", "AY_sug_kds4_x"),
        };
        #endregion
    }
}
