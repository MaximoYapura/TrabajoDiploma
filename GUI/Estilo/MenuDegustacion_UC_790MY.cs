using System;
using System.Linq;
using System.Windows.Forms;
using BLL_08YS;
using Service_08YS;
using Service_08YS.Entities.Acceso;

namespace GUI_08YS.Estilo
{
    // Menú Degustación: guía del servicio completo en seis pasos (cliente, reserva, abrir mesa,
    // pedido, cocina y caja). "Ir" le pide a FormMDI_08YS que abra la pantalla haciendo clic
    // en la opción real del menú, así respeta los mismos permisos. Sin permiso, el paso queda
    // deshabilitado y dice por qué. Las casillas "Hecho" no se guardan.
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
            public string Receta;           // PDF del Manual de Recetas (carpeta Manual)
        }

        private readonly Paso[] _pasos;

        // Además del permiso, la pantalla tiene que estar en el menú del usuario (por ejemplo,
        // Recepción tiene VerClientes pero no ve el menú Maestros).
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public Func<DestinoAyuda_790MY, bool> PantallaDisponible { get; set; }

        private bool Permitido(Paso paso)
            => paso.Permisos.Any(SessionManager_08YS.Instance.HasPermission)
               && (PantallaDisponible?.Invoke(paso.Destino) ?? true);

        public MenuDegustacion_UC_790MY()
        {
            InitializeComponent();
            _pasos = new[]
            {
                new Paso { Control = paso1_790MY, Destino = DestinoAyuda_790MY.Clientes,
                           Permisos = new[] { Permisos.VerClientes }, ClaveTitulo = "AY_paso1_titulo", Receta = "Receta_Cliente.pdf",
                           Ruta = () => Ruta("btnMaestros", "menuClientes"),
                           Pista = () => T("AY_paso1_pista") },
                new Paso { Control = paso2_790MY, Destino = DestinoAyuda_790MY.RegistrarReserva,
                           Permisos = new[] { Permisos.RegistrarReserva }, ClaveTitulo = "AY_paso2_titulo", Receta = "Receta_Reserva.pdf",
                           Ruta = () => Ruta("btnReservas", "menuRegistrarReserva"),
                           Pista = PistaReserva },
                new Paso { Control = paso3_790MY, Destino = DestinoAyuda_790MY.AbrirMesa,
                           Permisos = new[] { Permisos.AbrirMesa }, ClaveTitulo = "AY_paso3_titulo", Receta = "Receta_AbrirMesa.pdf",
                           Ruta = () => Ruta("menuAtencion", "menuAbrirMesa"),
                           Pista = () => T("AY_paso3_pista", Boton("AM_modo_reserva"), Boton("AM_btnConfirmar")) },
                new Paso { Control = paso4_790MY, Destino = DestinoAyuda_790MY.RegistrarPedido,
                           Permisos = new[] { Permisos.RegistrarPedido }, ClaveTitulo = "AY_paso4_titulo", Receta = "Receta_RegistrarPedido.pdf",
                           Ruta = () => Ruta("menuAtencion", "menuRegistrarPedido"),
                           Pista = () => T("AY_paso4_pista", Boton("RP_btnAgregar"), Boton("RP_btnEnviar")) },
                new Paso { Control = paso5_790MY, Destino = DestinoAyuda_790MY.Cocina,
                           Permisos = new[] { Permisos.DespacharPedido }, ClaveTitulo = "AY_paso5_titulo", Receta = "Receta_DespacharPedido.pdf",
                           Ruta = () => Ruta("menuAtencion", "menuDespacharPedido"),
                           Pista = () => T("AY_paso5_pista", Boton("KDS_btn_iniciar"), Boton("KDS_btn_listo")) },
                new Paso { Control = paso6_790MY, Destino = DestinoAyuda_790MY.CajaCobro,
                           Permisos = new[] { Permisos.RegistrarCobro, Permisos.GenerarFactura }, ClaveTitulo = "AY_paso6_titulo", Receta = "Receta_CajaYCobro.pdf",
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

        // Texto de un botón tal como se ve en su pantalla (sin los espacios del ícono).
        private static string Boton(string clave) => T(clave).Trim();

        private static string Ruta(string claveBoton, string claveItem) => $"{T(claveBoton).Trim()} › {T(claveItem).Trim()}";

        // Con turno en curso (real o de prueba), la pista muestra la hora en vivo.
        private static string PistaReserva()
        {
            TimeSpan? turno = MesaBLL_790MY.ObtenerTurnoVigente(DateTime.Now);
            return turno.HasValue
                ? T("AY_paso2_pista", turno.Value.ToString(@"hh\:mm"), Boton("RR_btnConfirmar"))
                : T("AY_paso2_pista_sin_turno", Boton("RR_btnConfirmar"));
        }

        // Permisos y textos en vivo: se llama cada vez que se abre el panel.
        public void Refrescar()
        {
            lblIntro_790MY.Text = T("AY_deg_intro");
            string ir = T("AY_btn_ir"), hecho = T("AY_chk_hecho"), motivo = T("AY_paso_sin_permiso"), receta = T("AY_btn_receta");
            foreach (var p in _pasos)
                p.Control.Configurar(T(p.ClaveTitulo), p.Ruta() + Environment.NewLine + p.Pista(),
                                     Permitido(p), motivo, ir, hecho, receta);
        }

        public void EnfocarPrimero()
        {
            if (!_pasos.Any(p => p.Control.EnfocarIr())) Focus();
        }

        public void UpdateIdioma() => Refrescar();

        // Muestra un aviso arriba de los pasos (lo usa también el submenú de Ayuda).
        public void MostrarAviso(string mensaje) => bnrDegustacion_790MY.Mostrar(mensaje, TipoBanner_790MY.Error);

        private void Paso_RecetaSolicitada(object sender, EventArgs e)
        {
            var paso = _pasos.FirstOrDefault(p => p.Control == sender);
            if (paso == null) return;
            string error = ManualRecetas_790MY.AbrirManual(paso.Receta);
            if (error == null) bnrDegustacion_790MY.Ocultar();
            else MostrarAviso(error);
        }

        private void Paso_IrSolicitado(object sender, EventArgs e)
        {
            var paso = _pasos.FirstOrDefault(p => p.Control == sender);
            if (paso != null && Permitido(paso))
                NavegacionSolicitada?.Invoke(this, new DestinoAyudaEventArgs_790MY(paso.Destino));
        }
    }
}
