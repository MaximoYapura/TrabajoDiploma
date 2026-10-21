using System;
using System.Linq;
using System.Windows.Forms;
using BLL_08YS;
using Service_08YS;
using Service_08YS.Entities.Acceso;

namespace GUI_08YS.Estilo
{
    /// <summary>
    /// Menú Degustación (Etapa 5, Fase 4, G5): la ruta de prueba completa con el flujo real
    /// de FormMDI_08YS (Cliente → Reserva → Abrir mesa → Registrar pedido → Cocina → Caja y
    /// cobro). Visible para todos los roles. «Ir» pide la navegación a FormMDI_08YS, que hace
    /// PerformClick del ítem del menú (hereda ValidatePermission). Sin permiso, «Ir» queda
    /// deshabilitado con el motivo. Las casillas «Hecho» no se guardan.
    /// </summary>
    public partial class MenuDegustacion_UC_790MY : UserControl, IIdiomaObserver_08YS
    {
        public event EventHandler<DestinoAyudaEventArgs_790MY> NavegacionSolicitada;

        private sealed class Paso
        {
            public PasoDegustacion_790MY Control;
            public DestinoAyuda_790MY Destino;
            public Permisos[] Permisos;     // alcanza con uno (Caja y cobro: RegistrarCobro o GenerarFactura)
            public string ClaveTitulo;
            public Func<string> Ruta;       // con los textos reales del menú lateral
            public Func<string> Pista;
        }

        private readonly Paso[] _pasos;

        public MenuDegustacion_UC_790MY()
        {
            InitializeComponent();
            _pasos = new[]
            {
                new Paso { Control = paso1_790MY, Destino = DestinoAyuda_790MY.Clientes,
                           Permisos = new[] { Permisos.VerClientes }, ClaveTitulo = "AY_paso1_titulo",
                           Ruta = () => Ruta("btnMaestros", "menuClientes"),
                           Pista = () => T("AY_paso1_pista") },
                new Paso { Control = paso2_790MY, Destino = DestinoAyuda_790MY.RegistrarReserva,
                           Permisos = new[] { Permisos.RegistrarReserva }, ClaveTitulo = "AY_paso2_titulo",
                           Ruta = () => Ruta("btnReservas", "menuRegistrarReserva"),
                           Pista = PistaReserva },
                new Paso { Control = paso3_790MY, Destino = DestinoAyuda_790MY.AbrirMesa,
                           Permisos = new[] { Permisos.AbrirMesa }, ClaveTitulo = "AY_paso3_titulo",
                           Ruta = () => Ruta("menuAtencion", "menuAbrirMesa"),
                           Pista = () => T("AY_paso3_pista", Boton("AM_modo_reserva"), Boton("AM_btnConfirmar")) },
                new Paso { Control = paso4_790MY, Destino = DestinoAyuda_790MY.RegistrarPedido,
                           Permisos = new[] { Permisos.RegistrarPedido }, ClaveTitulo = "AY_paso4_titulo",
                           Ruta = () => Ruta("menuAtencion", "menuRegistrarPedido"),
                           Pista = () => T("AY_paso4_pista", Boton("RP_btnAgregar"), Boton("RP_btnEnviar")) },
                new Paso { Control = paso5_790MY, Destino = DestinoAyuda_790MY.Cocina,
                           Permisos = new[] { Permisos.DespacharPedido }, ClaveTitulo = "AY_paso5_titulo",
                           Ruta = () => Ruta("menuAtencion", "menuDespacharPedido"),
                           Pista = () => T("AY_paso5_pista", Boton("KDS_btn_iniciar"), Boton("KDS_btn_listo")) },
                new Paso { Control = paso6_790MY, Destino = DestinoAyuda_790MY.CajaCobro,
                           Permisos = new[] { Permisos.RegistrarCobro, Permisos.GenerarFactura }, ClaveTitulo = "AY_paso6_titulo",
                           Ruta = () => Ruta("menuAtencion", "menuCajaCobro"),
                           Pista = () => T("AY_paso6_pista", PagoBLL_790MY.TerminacionRechazoSimulado) },
            };
        }

        private static string T(string clave) => TraductorManager_08YS.Instance.GetTexto(clave);

        private static string T(string clave, params object[] args)
        {
            string plantilla = T(clave);
            try { return string.Format(plantilla, args); }
            catch (FormatException) { return plantilla; }
        }

        /// <summary>Texto de un botón tal como se ve en su pantalla (sin los espacios del ícono).</summary>
        private static string Boton(string clave) => T(clave).Trim();

        private static string Ruta(string claveBoton, string claveItem) => $"{T(claveBoton).Trim()} › {T(claveItem).Trim()}";

        /// <summary>Con turno en curso (real o de prueba), la pista muestra la hora en vivo.</summary>
        private static string PistaReserva()
        {
            TimeSpan? turno = MesaBLL_790MY.ObtenerTurnoVigente(DateTime.Now);
            return turno.HasValue
                ? T("AY_paso2_pista", turno.Value.ToString(@"hh\:mm"), Boton("RR_btnConfirmar"))
                : T("AY_paso2_pista_sin_turno", Boton("RR_btnConfirmar"));
        }

        /// <summary>Permisos y textos en vivo: se llama cada vez que se abre el panel.</summary>
        public void Refrescar()
        {
            lblIntro_790MY.Text = T("AY_deg_intro");
            string ir = T("AY_btn_ir"), hecho = T("AY_chk_hecho"), motivo = T("AY_paso_sin_permiso");
            var sesion = SessionManager_08YS.Instance;
            foreach (var p in _pasos)
            {
                bool permitido = p.Permisos.Any(sesion.HasPermission);
                p.Control.Configurar(T(p.ClaveTitulo), p.Ruta() + Environment.NewLine + p.Pista(),
                                     permitido, motivo, ir, hecho);
            }
        }

        public void EnfocarPrimero()
        {
            if (!_pasos.Any(p => p.Control.EnfocarIr())) Focus();
        }

        public void UpdateIdioma() => Refrescar();

        private void Paso_IrSolicitado(object sender, EventArgs e)
        {
            var paso = _pasos.FirstOrDefault(p => p.Control == sender);
            if (paso != null)
                NavegacionSolicitada?.Invoke(this, new DestinoAyudaEventArgs_790MY(paso.Destino));
        }
    }
}
