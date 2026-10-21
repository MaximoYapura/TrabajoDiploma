using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BE_08YS;
using BLL_08YS;
using Service_08YS;

namespace GUI_08YS.Estilo
{
    /// <summary>
    /// Ficha de Ingredientes (Etapa 5, Fase 3): datos de prueba para el jurado leídos en vivo
    /// de la BLL (A5, C4), solo lectura y sin contraseñas. Solo existe con Modo Demostración
    /// y rol SuperAdmin (lo decide PanelAyuda_UC_790MY). Lo único constante son las reglas
    /// del cobro simulado: Luhn, terminación de rechazo y cuotas (PagoBLL_790MY, Pago_790MY).
    /// </summary>
    public partial class FichaIngredientes_UC_790MY : UserControl, IIdiomaObserver_08YS
    {
        private const int Maximo = 5;

        // Ejemplos de tarjeta (reglas, no datos): VISA válida por Luhn y otra válida que
        // termina en la terminación de rechazo simulado. Se verifican al mostrarlas.
        private const string TarjetaAprobada = "4111 1111 1111 1111";
        private const string TarjetaRechazada = "4000 0000 0000 0002";

        private ValorCopiable_790MY[] _clientes;
        private ValorCopiable_790MY[] _empleados;
        private ValorCopiable_790MY[] _mesas;
        private readonly List<string> _seccionesConError = new List<string>();
        private string _detalleError;

        public FichaIngredientes_UC_790MY()
        {
            InitializeComponent();
            _clientes = new[] { vcCliente1_790MY, vcCliente2_790MY, vcCliente3_790MY, vcCliente4_790MY, vcCliente5_790MY };
            _empleados = new[] { vcEmpleado1_790MY, vcEmpleado2_790MY, vcEmpleado3_790MY, vcEmpleado4_790MY, vcEmpleado5_790MY };
            _mesas = new[] { vcMesa1_790MY, vcMesa2_790MY, vcMesa3_790MY, vcMesa4_790MY, vcMesa5_790MY };
        }

        private static string T(string clave) => TraductorManager_08YS.Instance.GetTexto(clave);

        private static string T(string clave, params object[] args)
        {
            string plantilla = T(clave);
            try { return string.Format(plantilla, args); }
            catch (FormatException) { return plantilla; }
        }

        /// <summary>Lee los datos en vivo (se llama cada vez que se abre el panel).</summary>
        public void Cargar()
        {
            _seccionesConError.Clear();
            _detalleError = null;
            bnrFicha_790MY.Ocultar();
            flpFicha_790MY.SuspendLayout();
            try
            {
                CargarClientes();
                CargarEmpleados();
                CargarMesas();
                CargarTurno();
                CargarTarjetas();
            }
            finally
            {
                flpFicha_790MY.ResumeLayout(true);
            }
            if (_seccionesConError.Count > 0)
                bnrFicha_790MY.Mostrar(T("AY_ficha_error_lectura", string.Join(", ", _seccionesConError), _detalleError),
                                       TipoBanner_790MY.Error);
        }

        private void CargarClientes()
        {
            List<Cliente_790MY> lista = Leer("AY_ficha_sec_clientes",
                () => BLLFactory_790MY.CreateClienteBLL().GetAll().Where(c => c.Activo).Take(Maximo).ToList());
            Llenar(_clientes, lblVacioClientes_790MY, lista,
                   c => (c.Nombre + " " + c.Apellido).Trim(), c => c.Email, c => c.DNI.ToString());
        }

        private void CargarEmpleados()
        {
            List<Empleado_790MY> lista = Leer("AY_ficha_sec_empleados",
                () => BLLFactory_790MY.CreateEmpleadoBLL().GetAll().Where(e => e.Activo).Take(Maximo).ToList());
            Llenar(_empleados, lblVacioEmpleados_790MY, lista,
                   e => e.NombreCompleto, e => T("FE_puesto_" + e.Puesto), e => e.DNI.ToString());
        }

        /// <summary>
        /// «Mesas libres» no existe como dato único (depende de fecha y turno): se muestran
        /// las primeras mesas en servicio con número y capacidad, sin llamarlas libres (C4).
        /// </summary>
        private void CargarMesas()
        {
            List<Mesa_790MY> lista = Leer("AY_ficha_sec_mesas",
                () => BLLFactory_790MY.CreateMesaBLL().GetAll()
                        .Where(m => m.Activo && m.Estado != EstadoMesa_790MY.FueraDeServicio)
                        .OrderBy(m => m.NroMesa).Take(Maximo).ToList());
            Llenar(_mesas, lblVacioMesas_790MY, lista,
                   m => T("AY_ficha_mesa", m.NroMesa), m => T("AY_ficha_capacidad", m.Capacidad), m => m.NroMesa.ToString());
        }

        private void CargarTurno()
        {
            DateTime ahora = DateTime.Now;
            TimeSpan? turno = MesaBLL_790MY.ObtenerTurnoVigente(ahora);
            if (!turno.HasValue)
            {
                vcTurno_790MY.Mostrar(T("AY_ficha_turno_titulo"), T("AY_ficha_turno_fuera"), "—");
                return;
            }
            string detalle = MesaBLL_790MY.EsTurnoDePrueba(ahora)
                ? T("AY_ficha_turno_prueba")
                : T("AY_ficha_turno_real");
            vcTurno_790MY.Mostrar(T("AY_ficha_turno_titulo"), detalle, turno.Value.ToString(@"hh\:mm"));
        }

        private void CargarTarjetas()
        {
            string terminacion = PagoBLL_790MY.TerminacionRechazoSimulado;

            if (PagoBLL_790MY.ValidarTarjetaLuhn(TarjetaAprobada)
                && !PagoBLL_790MY.SoloDigitos(TarjetaAprobada).EndsWith(terminacion))
                vcTarjetaAprobada_790MY.Mostrar(T("AY_ficha_tarjeta_aprobada"),
                    PagoBLL_790MY.DetectarMarca(TarjetaAprobada), TarjetaAprobada);
            else
                vcTarjetaAprobada_790MY.Visible = false;

            if (PagoBLL_790MY.ValidarTarjetaLuhn(TarjetaRechazada)
                && PagoBLL_790MY.SoloDigitos(TarjetaRechazada).EndsWith(terminacion))
                vcTarjetaRechazada_790MY.Mostrar(T("AY_ficha_tarjeta_rechazada"),
                    T("AY_ficha_tarjeta_rechazada_det", terminacion), TarjetaRechazada);
            else
                vcTarjetaRechazada_790MY.Visible = false;

            lblCuotas_790MY.Text = T("AY_ficha_cuotas", string.Join(", ", Pago_790MY.CuotasPermitidas));
        }

        private List<TItem> Leer<TItem>(string claveSeccion, Func<List<TItem>> lectura)
        {
            try
            {
                return lectura();
            }
            catch (Exception ex)
            {
                _seccionesConError.Add(T(claveSeccion));
                if (_detalleError == null) _detalleError = ex.Message;
                return null;
            }
        }

        /// <summary>Completa las casillas fijas del Designer; sin datos, muestra el estado vacío.</summary>
        private static void Llenar<TItem>(ValorCopiable_790MY[] casillas, Label vacio, List<TItem> datos,
                                          Func<TItem, string> titulo, Func<TItem, string> detalle, Func<TItem, string> valor)
        {
            int n = datos?.Count ?? 0;
            for (int i = 0; i < casillas.Length; i++)
            {
                if (i < n) casillas[i].Mostrar(titulo(datos[i]), detalle(datos[i]), valor(datos[i]));
                else casillas[i].Visible = false;
            }
            vacio.Text = datos == null ? T("AY_ficha_no_disponible") : T("AY_ficha_vacio");
            vacio.Visible = n == 0;
        }

        public void UpdateIdioma()
        {
            lblIntro_790MY.Text = T("AY_ficha_intro");
            lblSecClientes_790MY.Text = T("AY_ficha_sec_clientes");
            lblSecEmpleados_790MY.Text = T("AY_ficha_sec_empleados");
            lblSecMesas_790MY.Text = T("AY_ficha_sec_mesas");
            lblSecTurno_790MY.Text = T("AY_ficha_sec_turno");
            lblSecTarjetas_790MY.Text = T("AY_ficha_sec_tarjetas");
            string copiar = T("AY_btn_copiar");
            foreach (var vc in flpFicha_790MY.Controls.OfType<ValorCopiable_790MY>())
                vc.ActualizarTextos(copiar);
            // Detalles traducidos (puesto, capacidad, turno): se releen solo si ya hay datos.
            if (vcTurno_790MY.Visible) Cargar();
        }

        private void Valor_CopiaIntentada(object sender, bool ok)
        {
            var vc = (ValorCopiable_790MY)sender;
            if (ok)
            {
                bnrFicha_790MY.Ocultar();
                ToastHost_790MY.Mostrar(this, T("AY_ficha_copiado", vc.Valor));
            }
            else
            {
                bnrFicha_790MY.Mostrar(T("AY_ficha_copia_fallo"), TipoBanner_790MY.Aviso);
                vc.SeleccionarValor();
            }
        }
    }
}
