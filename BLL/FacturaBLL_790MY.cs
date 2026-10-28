using BE_08YS;
using DAL_08YS.Interfaces_Repositories;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using Service_08YS.Entities.Bitacora;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BLL_08YS
{
    /// <summary>
    /// CUN-007 Cobrar Venta: emisión de la cuenta / factura (primera mitad del flujo de cobro).
    ///
    /// La cuenta se arma con los ítems de los pedidos Listo (entregados) de la reserva,
    /// con el precio que quedó congelado en cada pedido. Al generar la factura se copia
    /// ese detalle a ItemsFactura, así el comprobante no cambia aunque cambie la carta.
    /// Si la mesa ya tenía una factura Abierta con otros importes (nuevos consumos u otro
    /// descuento), la anterior queda Anulada y se emite una nueva.
    /// </summary>
    public class FacturaBLL_790MY
    {
        private readonly IFacturaRepository_790MY _facturaRepository;
        private readonly ReservaBLL_790MY _reservaBll;
        private readonly PedidoBLL_790MY _pedidoBll;
        private readonly EmpleadoBLL_790MY _empleadoBll;
        private readonly BitacoraBLL_08YS _bitacoraBll;

        public FacturaBLL_790MY(IFacturaRepository_790MY facturaRepository, ReservaBLL_790MY reservaBll,
                                PedidoBLL_790MY pedidoBll, EmpleadoBLL_790MY empleadoBll,
                                BitacoraBLL_08YS bitacoraBll)
        {
            _facturaRepository = facturaRepository ?? throw new ArgumentNullException(nameof(facturaRepository));
            _reservaBll = reservaBll ?? throw new ArgumentNullException(nameof(reservaBll));
            _pedidoBll = pedidoBll ?? throw new ArgumentNullException(nameof(pedidoBll));
            _empleadoBll = empleadoBll ?? throw new ArgumentNullException(nameof(empleadoBll));
            _bitacoraBll = bitacoraBll ?? throw new ArgumentNullException(nameof(bitacoraBll));
        }

        // ═══════════════════════════════════════════════════════════════════
        //  Consulta de mesas y consumos
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// Mesas ocupadas (reservas Asistidas) que el usuario puede cobrar. Un camarero ve
        /// solo las que tiene asignadas; un usuario sin puesto de camarero (encargado / caja)
        /// ve todas. Lista vacía => la GUI informa el escenario alternativo 1.1.
        /// </summary>
        public List<MesaCobroDTO_790MY> ObtenerMesasParaCobrar()
        {
            ValidarAccesoCaja();

            Empleado_790MY camarero = ObtenerCamareroDeSesion();
            List<Reserva_790MY> reservas = camarero != null
                ? _reservaBll.ObtenerAsistidasPorCamarero(camarero.DNI)
                : _reservaBll.ObtenerAsistidas();

            Dictionary<int, Empleado_790MY> camareros = CamarerosPorDni();

            return reservas
                .OrderBy(r => r.MesaNumero)
                .Select(r => new MesaCobroDTO_790MY
                {
                    ReservaID = r.ReservaID,
                    MesaNumero = r.MesaNumero,
                    CantidadComensales = r.CantidadComensales,
                    ClienteNombre = r.ClienteNombreCompleto,
                    CamareroNombre = NombreCamarero(camareros, r.CamareroDNI),
                    Desde = r.Hora,
                    SubtotalConsumido = _facturaRepository.ObtenerConsumosFacturables(r.ReservaID).Sum(i => i.Subtotal),
                    TieneFacturaAbierta = _facturaRepository.GetAbiertaPorReserva(r.ReservaID) != null
                })
                .ToList();
        }

        /// <summary>
        /// CUN-007 pasos 2 y 3: consumos entregados de la reserva con subtotal, descuento,
        /// IVA (21 %) y total. Si ya existe una factura Abierta se informa para reutilizar
        /// su descuento. Lanza el escenario 1.1 si no hay consumos facturables.
        /// </summary>
        public DetalleCuentaDTO_790MY ObtenerDetalleConsumoMesa(int nroReserva,
            decimal valorDescuento = 0m, TipoDescuento_790MY tipoDescuento = TipoDescuento_790MY.Porcentaje)
        {
            ValidarAccesoCaja();
            Reserva_790MY reserva = ValidarReservaCobrable(nroReserva);

            List<ItemFactura_790MY> items = _facturaRepository.ObtenerConsumosFacturables(nroReserva);
            if (items.Count == 0)
                throw MensajesBLL_790MY.Regla("msg_fc_sin_consumos");

            Factura_790MY abierta = _facturaRepository.GetAbiertaPorReserva(nroReserva);

            return new DetalleCuentaDTO_790MY
            {
                ReservaID = reserva.ReservaID,
                MesaNumero = reserva.MesaNumero,
                CantidadComensales = reserva.CantidadComensales,
                ClienteNombre = reserva.ClienteNombreCompleto,
                CamareroNombre = NombreCamarero(CamarerosPorDni(), reserva.CamareroDNI),
                Items = items,
                Importes = CalcularImportes(items.Sum(i => i.Subtotal), valorDescuento, tipoDescuento),
                FacturaAbierta = abierta,
                PedidosSinDespachar = _pedidoBll.ContarNoDespachados(nroReserva)
            };
        }

        /// <summary>
        /// CUN-007 paso 3 (y escenario 3.1). Función pura: la GUI la usa para recalcular en
        /// tiempo real mientras se edita el descuento, y GenerarFactura la vuelve a aplicar.
        /// El IVA (21 %) se calcula sobre el subtotal con descuento; importes a 2 decimales.
        /// </summary>
        public static ImportesCuenta_790MY CalcularImportes(decimal subtotal, decimal valorDescuento, TipoDescuento_790MY tipoDescuento)
        {
            if (subtotal <= 0)
                throw MensajesBLL_790MY.Regla("msg_fc_sin_consumos");

            decimal montoDescuento, porcentaje;
            if (tipoDescuento == TipoDescuento_790MY.Porcentaje)
            {
                if (valorDescuento < 0 || valorDescuento > 100)
                    throw MensajesBLL_790MY.Regla("msg_fc_descuento_porcentaje_invalido");
                porcentaje = Redondear(valorDescuento);
                montoDescuento = Redondear(subtotal * porcentaje / 100m);
            }
            else
            {
                if (valorDescuento < 0 || valorDescuento > subtotal)
                    throw MensajesBLL_790MY.Regla("msg_fc_descuento_monto_invalido", subtotal.ToString("N2", CultureInfo.GetCultureInfo("es-AR")));
                montoDescuento = Redondear(valorDescuento);
                porcentaje = Redondear(montoDescuento * 100m / subtotal);
            }

            decimal neto = subtotal - montoDescuento;
            decimal iva = Redondear(neto * Factura_790MY.AlicuotaIVA);

            return new ImportesCuenta_790MY
            {
                Subtotal = subtotal,
                PorcentajeDescuento = porcentaje,
                MontoDescuento = montoDescuento,
                SubtotalNeto = neto,
                IVA = iva,
                MontoTotal = neto + iva
            };
        }

        // ═══════════════════════════════════════════════════════════════════
        //  CUN-007 Cobrar Venta — emisión de la factura
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>Descuento expresado como porcentaje (atajo de la firma general).</summary>
        public Factura_790MY GenerarFactura(int nroReserva, decimal porcentajeDescuento)
            => GenerarFactura(nroReserva, porcentajeDescuento, TipoDescuento_790MY.Porcentaje);

        /// <summary>
        /// CUN-007 paso 8: registra la factura Abierta con importes congelados y la
        /// devuelve completa (con mesa, cliente y camarero) para mostrar la cuenta.
        /// Si ya hay una factura Abierta con el mismo detalle e importes, se reutiliza
        /// (imprimir dos veces el pre-ticket no consume números de factura).
        /// </summary>
        public Factura_790MY GenerarFactura(int nroReserva, decimal valorDescuento, TipoDescuento_790MY tipoDescuento)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.GenerarFactura);
            Reserva_790MY reserva = ValidarReservaCobrable(nroReserva);

            List<ItemFactura_790MY> items = _facturaRepository.ObtenerConsumosFacturables(nroReserva);
            if (items.Count == 0)
                throw MensajesBLL_790MY.Regla("msg_fc_sin_consumos");

            ImportesCuenta_790MY importes = CalcularImportes(items.Sum(i => i.Subtotal), valorDescuento, tipoDescuento);

            Factura_790MY abierta = _facturaRepository.GetAbiertaPorReserva(nroReserva);
            if (abierta != null && MismoContenido(abierta, items, importes))
                return abierta;

            var factura = new Factura_790MY
            {
                NroReserva = nroReserva,
                FechaEmision = Truncar(DateTime.Now),
                Subtotal = importes.Subtotal,
                PorcentajeDescuento = importes.PorcentajeDescuento,
                MontoDescuento = importes.MontoDescuento,
                SubtotalNeto = importes.SubtotalNeto,
                IVA = importes.IVA,
                MontoTotal = importes.MontoTotal,
                Items = items
            };

            ResultadoGenerarFactura_790MY resultado = _facturaRepository.GenerarFactura(factura, out int? anulada);
            switch (resultado)
            {
                case ResultadoGenerarFactura_790MY.Ok:
                    break;
                case ResultadoGenerarFactura_790MY.YaPagada:
                    throw MensajesBLL_790MY.Regla("msg_fc_reserva_ya_pagada", reserva.MesaNumero);
                case ResultadoGenerarFactura_790MY.ReservaNoAsistida:
                    throw MensajesBLL_790MY.Regla("msg_rf2_reserva_no_asistida", nroReserva);
                default:
                    throw new InvalidOperationException(resultado.ToString());
            }

            DVManager_08YS.Recalcular();
            if (anulada.HasValue)
                _bitacoraBll.RegistrarEventoPostCommit(Evento.FacturaAnulada, $"Factura {anulada.Value} - Mesa {reserva.MesaNumero}");
            _bitacoraBll.RegistrarEventoPostCommit(Evento.FacturaGenerada, $"Factura {factura.NroFactura} - Mesa {reserva.MesaNumero}");

            return _facturaRepository.GetById(factura.NroFactura) ?? factura;
        }

        public Factura_790MY ObtenerFactura(int nroFactura)
        {
            ValidarAccesoCaja();
            return _facturaRepository.GetById(nroFactura);
        }

        // ═══════════════════════════════════════════════════════════════════
        //  Integridad del comprobante
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// SHA-256 de los datos que identifican el comprobante (número, reserva, fecha,
        /// importes y, si existe, el pago). Se imprime en el ticket para verificar que
        /// no fue alterado. Formato invariante: no depende del idioma del usuario.
        /// </summary>
        public static string CalcularHashComprobante(Factura_790MY factura, Pago_790MY pago = null)
        {
            if (factura == null) throw new ArgumentNullException(nameof(factura));

            var inv = CultureInfo.InvariantCulture;
            var sb = new StringBuilder()
                .Append(factura.NumeroComprobante).Append('|')
                .Append(factura.NroReserva).Append('|')
                .Append(factura.FechaEmision.ToString("yyyy-MM-ddTHH:mm:ss", inv)).Append('|')
                .Append(factura.Subtotal.ToString("F2", inv)).Append('|')
                .Append(factura.MontoDescuento.ToString("F2", inv)).Append('|')
                .Append(factura.IVA.ToString("F2", inv)).Append('|')
                .Append(factura.MontoTotal.ToString("F2", inv));

            foreach (var item in factura.Items.OrderBy(i => i.NroItem))
                sb.Append('|').Append(item.ProductoID).Append('x').Append(item.Cantidad)
                  .Append('@').Append(item.PrecioUnitario.ToString("F2", inv));

            if (pago != null)
                sb.Append('|').Append(pago.IdPago).Append('|').Append(pago.MetodoPago)
                  .Append('|').Append(pago.MontoAbonado.ToString("F2", inv))
                  .Append('|').Append(pago.FechaHoraPago.ToString("yyyy-MM-ddTHH:mm:ss", inv));

            using (var sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
                return string.Concat(hash.Select(b => b.ToString("x2")));
            }
        }

        /// <summary>Código corto de verificación (primeros 8 hex del hash): "7F3A-92C1".</summary>
        public static string CodigoVerificacion(string hash)
        {
            if (string.IsNullOrEmpty(hash) || hash.Length < 8) return string.Empty;
            string h = hash.Substring(0, 8).ToUpperInvariant();
            return h.Substring(0, 4) + "-" + h.Substring(4, 4);
        }

        // ─── Helpers ─────────────────────────────────────────────────────────

        internal static decimal Redondear(decimal valor) => Math.Round(valor, 2, MidpointRounding.AwayFromZero);

        private static DateTime Truncar(DateTime momento)
            => new DateTime(momento.Year, momento.Month, momento.Day, momento.Hour, momento.Minute, momento.Second);

        private static bool MismoContenido(Factura_790MY abierta, List<ItemFactura_790MY> items, ImportesCuenta_790MY importes)
        {
            if (abierta.Subtotal != importes.Subtotal || abierta.MontoDescuento != importes.MontoDescuento
                || abierta.MontoTotal != importes.MontoTotal || abierta.Items.Count != items.Count)
                return false;

            var actuales = items.Select(i => $"{i.ProductoID}|{i.Cantidad}|{i.PrecioUnitario}").OrderBy(s => s);
            var facturados = abierta.Items.Select(i => $"{i.ProductoID}|{i.Cantidad}|{i.PrecioUnitario}").OrderBy(s => s);
            return actuales.SequenceEqual(facturados);
        }

        /// <summary>Acceso al módulo Caja y cobro: alcanza con cualquiera de las dos patentes.</summary>
        private static void ValidarAccesoCaja()
        {
            if (!SessionManager_08YS.Instance.HasPermission(Permisos.RegistrarCobro))
                SessionManager_08YS.Instance.ValidatePermission(Permisos.GenerarFactura);
        }

        /// <summary>
        /// Precondición CUN-007: reserva Asistida (mesa ocupada) y, si el usuario es
        /// camarero, asignada a él.
        /// </summary>
        private Reserva_790MY ValidarReservaCobrable(int nroReserva)
        {
            Reserva_790MY reserva = _reservaBll.ObtenerPorId(nroReserva)
                ?? throw MensajesBLL_790MY.Regla("msg_rf2_reserva_inexistente", nroReserva);

            if (reserva.Estado != EstadoReserva_790MY.Asistida)
                throw MensajesBLL_790MY.Regla("msg_rf2_reserva_no_asistida", nroReserva);

            Empleado_790MY camarero = ObtenerCamareroDeSesion();
            if (camarero != null && reserva.CamareroDNI != camarero.DNI)
                throw MensajesBLL_790MY.Regla("msg_rf2_mesa_no_asignada", reserva.MesaNumero);

            return reserva;
        }

        /// <summary>
        /// Empleado camarero vinculado al usuario logueado, o null si el usuario no es
        /// camarero (encargado / caja): en ese caso puede cobrar cualquier mesa.
        /// </summary>
        private Empleado_790MY ObtenerCamareroDeSesion()
        {
            Empleado_790MY empleado = _empleadoBll.ObtenerEmpleadoDeSesion();
            return empleado != null && empleado.Activo && empleado.Puesto == PuestoEmpleado_790MY.Camarero
                ? empleado
                : null;
        }

        private Dictionary<int, Empleado_790MY> CamarerosPorDni()
            => _empleadoBll.ObtenerCamarerosActivos().ToDictionary(e => e.DNI);

        private static string NombreCamarero(Dictionary<int, Empleado_790MY> camareros, int? dni)
            => dni.HasValue && camareros.TryGetValue(dni.Value, out var e) ? $"{e.Nombre} {e.Apellido}" : "-";
    }
}
