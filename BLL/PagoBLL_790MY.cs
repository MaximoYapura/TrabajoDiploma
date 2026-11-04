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
    // Caja y cobro, segunda parte (CUN-007): el cobro. También el arqueo y el cierre de caja.
    // El pago, la factura Pagada, la reserva Finalizada y la mesa Libre se graban en una
    // sola transacción. Un rechazo del banco no cambia nada; solo queda en la bitácora.
    public class PagoBLL_790MY
    {
        // El banco simulado rechaza las tarjetas que terminan así (por ejemplo 4000 0000 0000 0002).
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
        //  Validaciones de la tarjeta (no usan la base)
        // ═══════════════════════════════════════════════════════════════════

        // Algoritmo de Luhn. Acepta espacios o guiones y pide entre 13 y 19 dígitos.
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

        // Dígitos del número ingresado, o null si contiene otros caracteres.
        public static string SoloDigitos(string nroTarjeta)
        {
            if (string.IsNullOrWhiteSpace(nroTarjeta)) return null;
            string limpio = nroTarjeta.Replace(" ", "").Replace("-", "");
            return limpio.All(char.IsDigit) ? limpio : null;
        }

        // Marca según cómo empieza el número: VISA, MASTERCARD, AMEX u OTRA.
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

        // "**** **** **** 1111". El número completo nunca se guarda ni se muestra.
        public static string EnmascararTarjeta(string nroTarjeta)
        {
            string d = SoloDigitos(nroTarjeta) ?? string.Empty;
            string ultimos = d.Length >= 4 ? d.Substring(d.Length - 4) : d;
            return "**** **** **** " + ultimos;
        }

        // Vuelto a entregar; negativo si el efectivo no alcanza.
        public static decimal CalcularCambio(decimal montoAbonado, decimal montoRecibido)
            => montoRecibido - montoAbonado;

        // ═══════════════════════════════════════════════════════════════════
        //  Cobro
        // ═══════════════════════════════════════════════════════════════════

        // Cobra una factura Abierta. En efectivo, lo recibido tiene que cubrir el total más la
        // propina y se calcula el vuelto. Con tarjeta se valida el número y las cuotas y se
        // pide autorización al banco simulado; si la rechaza, lanza PagoRechazadoException_790MY.
        public ResultadoCobroDTO_790MY RegistrarCobro(SolicitudCobro_790MY solicitud)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.RegistrarCobro);
            if (solicitud == null) throw new ArgumentNullException(nameof(solicitud));

            // La factura tiene que estar Abierta.
            Factura_790MY factura = _facturaBll.ObtenerFactura(solicitud.NroFactura);
            if (factura == null || factura.Estado != EstadoFactura_790MY.Abierta)
                throw MensajesBLL_790MY.Regla("msg_fc_factura_no_abierta", solicitud.NroFactura);

            // Antes de pedir la autorización se controla la mesa, para no autorizar una tarjeta
            // en un cobro que después falla. La transacción del repositorio lo vuelve a controlar.
            // Primero: que la cuenta no haya cambiado desde que se emitió la factura.
            if (!_facturaBll.FacturaVigente(factura))
                throw MensajesBLL_790MY.Regla("msg_fc_factura_desactualizada", factura.MesaNumero);

            if (_cierreCajaRepository.Existe(DateTime.Today))
                throw MensajesBLL_790MY.Regla("msg_fc_caja_cerrada", DateTime.Today.ToString("dd/MM/yyyy"));

            if (_facturaBll.ContarPedidosNoDespachados(factura.NroReserva) > 0)
                throw MensajesBLL_790MY.Regla("msg_fc_pedidos_sin_despachar", factura.MesaNumero);

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
                // Efectivo: lo recibido tiene que alcanzar.
                if (!solicitud.MontoRecibido.HasValue || solicitud.MontoRecibido.Value < pago.MontoAbonado)
                    throw MensajesBLL_790MY.Regla("msg_fc_efectivo_insuficiente");

                pago.MontoRecibido = FacturaBLL_790MY.Redondear(solicitud.MontoRecibido.Value);
                pago.Cambio = CalcularCambio(pago.MontoAbonado, pago.MontoRecibido.Value);
            }
            else
            {
                // Tarjeta: número y cuotas.
                if (!ValidarTarjetaLuhn(solicitud.NroTarjeta))
                    throw MensajesBLL_790MY.Regla("msg_fc_tarjeta_invalida");
                if (!Pago_790MY.CuotasPermitidas.Contains(solicitud.CantidadCuotas))
                    throw MensajesBLL_790MY.Regla("msg_fc_cuotas_invalidas", string.Join(", ", Pago_790MY.CuotasPermitidas));

                pago.NroTarjeta = EnmascararTarjeta(solicitud.NroTarjeta);
                pago.MarcaTarjeta = DetectarMarca(solicitud.NroTarjeta);
                pago.CantidadCuotas = solicitud.CantidadCuotas;

                // Autorización del banco simulado.
                if (!SolicitarAutorizacionBancaria(solicitud.NroTarjeta, pago.MontoAbonado, out string autorizacion))
                {
                    _bitacoraBll.RegistrarEventoPostCommit(Evento.PagoRechazado,
                        $"Factura {factura.NroFactura} - {pago.MarcaTarjeta} {pago.NroTarjeta.Substring(pago.NroTarjeta.Length - 4)}");
                    throw new PagoRechazadoException_790MY("FONDOS_INSUFICIENTES",
                        MensajesBLL_790MY.Texto("msg_fc_pago_rechazado"));
                }
                pago.NroAutorizacion = autorizacion;
            }

            // Se graba todo en una transacción.
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
                case ResultadoRegistroCobro_790MY.FacturaDesactualizada:
                    throw MensajesBLL_790MY.Regla("msg_fc_factura_desactualizada", factura.MesaNumero);
                case ResultadoRegistroCobro_790MY.CajaCerrada:
                    throw MensajesBLL_790MY.Regla("msg_fc_caja_cerrada", pago.FechaHoraPago.ToString("dd/MM/yyyy"));
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

        // Arqueo del día: los cobros con sus totales.
        public ReporteArqueoCaja_790MY GenerarReporteArqueoCaja(DateTime fecha)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.GestionCaja);
            return new ReporteArqueoCaja_790MY
            {
                Fecha = fecha.Date,
                Cobros = _pagoRepository.ObtenerCobrosDelDia(fecha.Date)
            };
        }

        // Cierra la caja del día: lo graba en CierresCaja y en la bitácora. No toca los pagos.
        public ReporteArqueoCaja_790MY CerrarCaja(DateTime fecha)
        {
            ReporteArqueoCaja_790MY reporte = GenerarReporteArqueoCaja(fecha);
            if (reporte.CantidadCuentas == 0)
                throw MensajesBLL_790MY.Regla("msg_ac_sin_cobros", fecha.ToString("dd/MM/yyyy"));

            // Una sola vez por día. Esta consulta es para avisar antes; lo que de verdad lo
            // impide es la clave primaria de CierresCaja (si otra terminal cerró en el medio,
            // el repositorio devuelve false).
            if (EstaCajaCerrada(fecha))
                throw MensajesBLL_790MY.Regla("msg_ac_caja_ya_cerrada", fecha.ToString("dd/MM/yyyy"));

            // Con mesas abiertas, esos cobros quedarían afuera del arqueo.
            int mesasAbiertas = _facturaBll.ContarMesasAbiertas();
            if (mesasAbiertas > 0)
                throw MensajesBLL_790MY.Regla("msg_ac_mesas_abiertas", mesasAbiertas);

            string usuario = SessionManager_08YS.Instance.Current?.Username ?? "Desconocido";
            if (!_cierreCajaRepository.Registrar(fecha.Date, reporte.RecaudacionNeta, reporte.CantidadCuentas, usuario))
                throw MensajesBLL_790MY.Regla("msg_ac_caja_ya_cerrada", fecha.ToString("dd/MM/yyyy"));

            // En la bitácora queda "Caja dd/MM/yyyy - importe", con la fecha en cultura invariante.
            _bitacoraBll.RegistrarEventoPostCommit(Evento.CierreCajaRealizado,
                "Caja " + fecha.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " - " +
                reporte.RecaudacionNeta.ToString("N2", CulturaMoneda));
            return reporte;
        }

        // PDF A4 de la factura de un cobro del arqueo. Solo para facturas pagadas.
        public byte[] ExportarFacturaPDF(int nroFactura)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.GestionCaja);

            Factura_790MY factura = _facturaBll.ObtenerPorNumero(nroFactura);
            Pago_790MY pago = factura != null && factura.Estado == EstadoFactura_790MY.Pagada
                ? _pagoRepository.GetPorFactura(nroFactura)
                : null;
            if (pago == null)
                throw MensajesBLL_790MY.Regla("msg_ac_factura_no_pagada", FacturaBLL_790MY.FormatearComprobante(nroFactura));

            return _facturaBll.GenerarFacturaPDF(factura, pago);
        }

        // True si la caja de fecha ya se cerró (tabla CierresCaja).
        public bool EstaCajaCerrada(DateTime fecha)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.GestionCaja);
            return _cierreCajaRepository.Existe(fecha.Date);
        }

        // ─── Helpers ─────────────────────────────────────────────────────────

        // Banco simulado. Aprueba toda tarjeta válida salvo las que terminan en
        // TerminacionRechazoSimulado, y devuelve un código de autorización de 6 dígitos.
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

        // Saca los milisegundos, para que la fecha coincida con la que se guarda.
        private static DateTime Truncar(DateTime momento)
            => new DateTime(momento.Year, momento.Month, momento.Day, momento.Hour, momento.Minute, momento.Second);
    }
}
