using BE_08YS;
using BLL_08YS.Exceptions;
using DAL_08YS.Interfaces_Repositories;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using Service_08YS.Entities.Bitacora;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;

namespace BLL_08YS
{
    /// <summary>
    /// CUN-007 Cobrar Venta (registro del pago) y reporte de Arqueo / Cierre de Caja.
    ///
    /// El pago, el paso de la factura a Pagada, la reserva a Finalizada y la mesa a Libre
    /// ocurren en una única transacción del repositorio. Un rechazo bancario no toca la
    /// base de negocio: solo deja constancia en la bitácora (escenario 7.1).
    /// </summary>
    public class PagoBLL_790MY
    {
        /// <summary>
        /// Terminación de tarjeta que la entidad bancaria simulada siempre rechaza,
        /// para poder probar el escenario 7.1 (p. ej. 4000 0000 0000 0002, Luhn válido).
        /// </summary>
        public const string TerminacionRechazoSimulado = "0002";

        private static readonly CultureInfo CulturaMoneda = CultureInfo.GetCultureInfo("es-AR");

        private readonly IPagoRepository_790MY _pagoRepository;
        private readonly FacturaBLL_790MY _facturaBll;
        private readonly ICierreCajaRepository_790MY _cierreCajaRepository;
        private readonly BitacoraBLL_08YS _bitacoraBll;

        public PagoBLL_790MY(IPagoRepository_790MY pagoRepository, FacturaBLL_790MY facturaBll,
                             ICierreCajaRepository_790MY cierreCajaRepository, BitacoraBLL_08YS bitacoraBll)
        {
            _pagoRepository = pagoRepository ?? throw new ArgumentNullException(nameof(pagoRepository));
            _facturaBll = facturaBll ?? throw new ArgumentNullException(nameof(facturaBll));
            _cierreCajaRepository = cierreCajaRepository ?? throw new ArgumentNullException(nameof(cierreCajaRepository));
            _bitacoraBll = bitacoraBll ?? throw new ArgumentNullException(nameof(bitacoraBll));
        }

        // ═══════════════════════════════════════════════════════════════════
        //  Validaciones de tarjeta (sintácticas, sin acceso a datos)
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Algoritmo de Luhn (módulo 10). Acepta espacios o guiones como separadores;
        /// exige entre 13 y 19 dígitos.
        /// </summary>
        public static bool ValidarTarjetaLuhn(string nroTarjeta)
        {
            string digitos = SoloDigitos(nroTarjeta);
            if (digitos == null || digitos.Length < 13 || digitos.Length > 19)
                return false;

            int suma = 0;
            bool duplicar = false;
            for (int i = digitos.Length - 1; i >= 0; i--)
            {
                int d = digitos[i] - '0';
                if (duplicar)
                {
                    d *= 2;
                    if (d > 9) d -= 9;
                }
                suma += d;
                duplicar = !duplicar;
            }
            return suma % 10 == 0;
        }

        /// <summary>Dígitos del número ingresado, o null si contiene otros caracteres.</summary>
        public static string SoloDigitos(string nroTarjeta)
        {
            if (string.IsNullOrWhiteSpace(nroTarjeta)) return null;
            string limpio = nroTarjeta.Replace(" ", "").Replace("-", "");
            return limpio.All(char.IsDigit) ? limpio : null;
        }

        /// <summary>Marca deducida del prefijo (IIN): VISA, MASTERCARD, AMEX u OTRA.</summary>
        public static string DetectarMarca(string nroTarjeta)
        {
            string d = SoloDigitos(nroTarjeta);
            if (string.IsNullOrEmpty(d)) return string.Empty;
            if (d.StartsWith("4")) return "VISA";
            if (d.StartsWith("34") || d.StartsWith("37")) return "AMEX";
            if (d.Length >= 2)
            {
                int p2 = int.Parse(d.Substring(0, 2));
                if (p2 >= 51 && p2 <= 55) return "MASTERCARD";
            }
            if (d.Length >= 4)
            {
                int p4 = int.Parse(d.Substring(0, 4));
                if (p4 >= 2221 && p4 <= 2720) return "MASTERCARD";
            }
            return "OTRA";
        }

        /// <summary>"**** **** **** 1111": nunca se persiste ni se muestra el número completo.</summary>
        public static string EnmascararTarjeta(string nroTarjeta)
        {
            string d = SoloDigitos(nroTarjeta) ?? string.Empty;
            string ultimos = d.Length >= 4 ? d.Substring(d.Length - 4) : d;
            return "**** **** **** " + ultimos;
        }

        /// <summary>Vuelto a entregar; negativo si el efectivo no alcanza.</summary>
        public static decimal CalcularCambio(decimal montoAbonado, decimal montoRecibido)
            => montoRecibido - montoAbonado;

        // ═══════════════════════════════════════════════════════════════════
        //  CUN-007 Cobrar Venta — registro del pago
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Procesa el cobro de una factura Abierta. Efectivo: el monto recibido debe cubrir
        /// total + propina y se calcula el vuelto. Tarjeta: valida Luhn y cuotas (1, 3, 6, 12)
        /// y solicita la autorización a la entidad bancaria simulada.
        /// Lanza PagoRechazadoException_790MY si el banco rechaza (sin cambios en la base).
        /// </summary>
        public ResultadoCobroDTO_790MY RegistrarCobro(SolicitudCobro_790MY solicitud)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.RegistrarCobro);
            if (solicitud == null) throw new ArgumentNullException(nameof(solicitud));

            // Pasos 1-2: factura Abierta. RegistrarCobro ya habilita el acceso a Caja
            // que exige FacturaBLL_790MY.ObtenerFactura.
            Factura_790MY factura = _facturaBll.ObtenerFactura(solicitud.NroFactura);
            if (factura == null || factura.Estado != EstadoFactura_790MY.Abierta)
                throw MensajesBLL_790MY.Regla("msg_fc_factura_no_abierta", solicitud.NroFactura);

            if (solicitud.MontoPropina < 0)
                throw MensajesBLL_790MY.Regla("msg_fc_propina_invalida");

            decimal propina = FacturaBLL_790MY.Redondear(solicitud.MontoPropina);
            var pago = new Pago_790MY
            {
                NroFactura = factura.NroFactura,
                MetodoPago = solicitud.MetodoPago,
                MontoPropina = propina,
                MontoAbonado = factura.MontoTotal + propina
            };

            if (solicitud.MetodoPago == MetodoPago_790MY.Efectivo)
            {
                // Escenarios 4.1.1, 4.1.2 y 4.2.
                if (!solicitud.MontoRecibido.HasValue || solicitud.MontoRecibido.Value < pago.MontoAbonado)
                    throw MensajesBLL_790MY.Regla("msg_fc_efectivo_insuficiente");

                pago.MontoRecibido = FacturaBLL_790MY.Redondear(solicitud.MontoRecibido.Value);
                pago.Cambio = CalcularCambio(pago.MontoAbonado, pago.MontoRecibido.Value);
            }
            else
            {
                // Paso 5 / escenarios 5.1-5.2: datos del medio de pago.
                if (!ValidarTarjetaLuhn(solicitud.NroTarjeta))
                    throw MensajesBLL_790MY.Regla("msg_fc_tarjeta_invalida");
                if (!Pago_790MY.CuotasPermitidas.Contains(solicitud.CantidadCuotas))
                    throw MensajesBLL_790MY.Regla("msg_fc_cuotas_invalidas", string.Join(", ", Pago_790MY.CuotasPermitidas));

                pago.NroTarjeta = EnmascararTarjeta(solicitud.NroTarjeta);
                pago.MarcaTarjeta = DetectarMarca(solicitud.NroTarjeta);
                pago.CantidadCuotas = solicitud.CantidadCuotas;

                // Pasos 6-7 / escenario 7.1: autorización de la entidad bancaria simulada.
                if (!SolicitarAutorizacionBancaria(solicitud.NroTarjeta, pago.MontoAbonado, out string autorizacion))
                {
                    _bitacoraBll.RegistrarEventoPostCommit(Evento.PagoRechazado,
                        $"Factura {factura.NroFactura} - {pago.MarcaTarjeta} {pago.NroTarjeta.Substring(pago.NroTarjeta.Length - 4)}");
                    throw new PagoRechazadoException_790MY("FONDOS_INSUFICIENTES",
                        MensajesBLL_790MY.Texto("msg_fc_pago_rechazado"));
                }
                pago.NroAutorizacion = autorizacion;
            }

            // Pasos 8-9: confirmación transaccional.
            pago.FechaHoraPago = Truncar(DateTime.Now);
            ResultadoRegistroCobro_790MY resultado = _pagoRepository.RegistrarCobro(pago, out int reservaId, out int mesaNumero);
            switch (resultado)
            {
                case ResultadoRegistroCobro_790MY.Ok:
                    break;
                case ResultadoRegistroCobro_790MY.FacturaNoAbierta:
                    throw MensajesBLL_790MY.Regla("msg_fc_factura_no_abierta", factura.NroFactura);
                case ResultadoRegistroCobro_790MY.ReservaNoAsistida:
                    throw MensajesBLL_790MY.Regla("msg_rf2_reserva_no_asistida", factura.NroReserva);
                case ResultadoRegistroCobro_790MY.PedidosSinDespachar:
                    throw MensajesBLL_790MY.Regla("msg_fc_pedidos_sin_despachar", factura.MesaNumero);
                default:
                    throw new InvalidOperationException(resultado.ToString());
            }

            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.PagoRegistrado,
                $"Factura {factura.NroFactura} - {pago.MetodoPago} {pago.MontoAbonado.ToString("N2", CulturaMoneda)}");
            _bitacoraBll.RegistrarEventoPostCommit(Evento.ReservaFinalizada, $"Reserva {reservaId}");
            _bitacoraBll.RegistrarEventoPostCommit(Evento.MesaEstadoActualizado, $"Mesa {mesaNumero} Libre");

            return new ResultadoCobroDTO_790MY
            {
                Factura = _facturaBll.ObtenerFactura(factura.NroFactura) ?? factura,
                Pago = pago
            };
        }

        // ═══════════════════════════════════════════════════════════════════
        //  Arqueo y cierre de caja
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Totales de la jornada (efectivo, tarjetas, propinas, descuentos y recaudación
        /// neta = efectivo + tarjetas) con el detalle de cuentas cobradas.
        /// </summary>
        public ReporteArqueoCaja_790MY GenerarReporteArqueoCaja(DateTime fecha)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.GestionCaja);
            return new ReporteArqueoCaja_790MY
            {
                Fecha = fecha.Date,
                Cobros = _pagoRepository.ObtenerCobrosDelDia(fecha.Date)
            };
        }

        /// <summary>
        /// Cierre de caja del día: recalcula el arqueo, lo registra en la tabla CierresCaja
        /// y deja constancia en la bitácora (fecha y recaudación neta). No modifica los
        /// pagos registrados.
        /// </summary>
        public ReporteArqueoCaja_790MY CerrarCaja(DateTime fecha)
        {
            ReporteArqueoCaja_790MY reporte = GenerarReporteArqueoCaja(fecha);
            if (reporte.CantidadCuentas == 0)
                throw MensajesBLL_790MY.Regla("msg_ac_sin_cobros", fecha.ToString("dd/MM/yyyy"));

            // Una sola vez por día. La consulta previa da el mensaje sin intentar el INSERT;
            // la garantía real es la PK (Fecha) de CierresCaja: si otra terminal cerró la
            // misma jornada en el medio, el repositorio devuelve false.
            if (EstaCajaCerrada(fecha))
                throw MensajesBLL_790MY.Regla("msg_ac_caja_ya_cerrada", fecha.ToString("dd/MM/yyyy"));

            string usuario = SessionManager_08YS.Instance.Current?.Username ?? "Desconocido";
            if (!_cierreCajaRepository.Registrar(fecha.Date, reporte.RecaudacionNeta, reporte.CantidadCuentas, usuario))
                throw MensajesBLL_790MY.Regla("msg_ac_caja_ya_cerrada", fecha.ToString("dd/MM/yyyy"));

            // Texto de la bitácora sin cambios ("Caja dd/MM/yyyy - importe"); cultura
            // invariante para que la barra no dependa del idioma activo.
            _bitacoraBll.RegistrarEventoPostCommit(Evento.CierreCajaRealizado,
                "Caja " + fecha.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " - " +
                reporte.RecaudacionNeta.ToString("N2", CulturaMoneda));
            return reporte;
        }

        /// <summary>True si la caja de <paramref name="fecha"/> ya se cerró (tabla CierresCaja).</summary>
        public bool EstaCajaCerrada(DateTime fecha)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.GestionCaja);
            return _cierreCajaRepository.Existe(fecha.Date);
        }

        // ─── Helpers ─────────────────────────────────────────────────────────

        /// <summary>
        /// Entidad bancaria simulada (actor secundario de CUN-007). Aprueba toda tarjeta
        /// válida salvo la terminación de prueba <see cref="TerminacionRechazoSimulado"/>,
        /// y devuelve un código de autorización de 6 dígitos.
        /// </summary>
        private static bool SolicitarAutorizacionBancaria(string nroTarjeta, decimal monto, out string codigoAutorizacion)
        {
            codigoAutorizacion = null;
            string digitos = SoloDigitos(nroTarjeta);
            if (digitos == null || monto <= 0 || digitos.EndsWith(TerminacionRechazoSimulado))
                return false;

            byte[] buffer = new byte[4];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(buffer);
            codigoAutorizacion = (BitConverter.ToUInt32(buffer, 0) % 900000 + 100000).ToString(CultureInfo.InvariantCulture);
            return true;
        }

        private static DateTime Truncar(DateTime momento)
            => new DateTime(momento.Year, momento.Month, momento.Day, momento.Hour, momento.Minute, momento.Second);
    }
}
