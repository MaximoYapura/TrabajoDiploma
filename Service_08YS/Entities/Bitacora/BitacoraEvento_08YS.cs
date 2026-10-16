using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_08YS.Entities.Bitacora
{
    public enum Criticidad
    {
        Critico,
        Alto,
        Medio,
        Bajo
    }

    public enum Modulo
    {
        Usuarios,
        Login,
        Perfiles,
        Clientes,
        Mesas,
        Reservas,
        Sistema,
        // RF2 (790MY). Valores explícitos: la columna Bitacora.Modulo guarda el int.
        Empleados = 7,
        Pedidos = 8,
        Stock = 9,
        Facturacion = 10
    }

    public enum Evento
    {
        LoginExitoso,
        LoginFallido,
        UsuarioCreado,
        UsuarioBloqueado,
        UsuarioDesbloqueado,
        UsuarioDeshabilitado,
        UsuarioHabilitado,
        UsuarioEmailModificado,
        UsuarioRolModificado,
        CambioContraseña,
        FamiliaCreada,
        FamiliaEliminada,
        FamiliaModificada,
        RolCreado,
        RolEliminado,
        RolModificado,
        ClienteRegistrado = 20,
        ClienteModificado = 21,
        ClienteEliminado = 22,
        MesaRegistrada = 23,
        MesaModificada = 24,
        MesaEliminada = 25,
        ReservaCancelada = 26,
        ReservaRegistrada = 27,
        RecalculoDVFallido = 28,
        MesaEstadoActualizado = 29,

        // ── RF2 Atención en mesa (790MY) ─────────────────────────────────
        // Valores explícitos y nunca reutilizados: Bitacora.Evento persiste el int.
        ReservaAsistida = 30,
        ReservaFinalizada = 31,
        MesaAbierta = 32,
        WalkInRegistrado = 33,
        CamareroAsignado = 34,
        EmpleadoRegistrado = 35,
        EmpleadoModificado = 36,
        PedidoRegistrado = 37,
        PedidoRechazadoStock = 38,
        PedidoEnPreparacion = 39,
        PedidoListo = 40,
        StockDescontado = 41,
        ReservaRechazadaDisponibilidad = 42,
        StockDescuentoFallido = 43,
        ProductoRegistrado = 44,
        ProductoModificado = 45,
        ProductoDadoDeBaja = 46,
        EmpleadoDadoDeBaja = 47,

        // ── RF2 Facturación y cobro (790MY) ──────────────────────────────
        FacturaGenerada = 48,
        FacturaAnulada = 49,
        PagoRegistrado = 50,
        PagoRechazado = 51,
        CierreCajaRealizado = 52
    }

    public class BitacoraEvento_08YS
    {
        public string Username { get; set; }  // actor: quien ejecuta
        public string TargetUsername { get; set; }  // sujeto: sobre quien se ejecuta (nullable)
        public DateTime FechaHora { get; set; }
        public Modulo Modulo { get; set; }
        public Evento Evento { get; set; }
        public Criticidad Criticidad { get; set; }

        public BitacoraEvento_08YS(string username, DateTime fechaHora, Modulo modulo, Evento evento, Criticidad criticidad, string targetUsername = null)
        {
            Username = username;
            TargetUsername = targetUsername;
            FechaHora = fechaHora;
            Modulo = modulo;
            Evento = evento;
            Criticidad = criticidad;
        }

        public BitacoraEvento_08YS() { }
    }

    public class BitacoraFiltro_08YS
    {
        public string Username { get; set; } = null;

        public DateTime? FechaDesde { get; set; }

        public DateTime? FechaHasta { get; set; }

        public Modulo? Modulo { get; set; }

        public Evento? Evento { get; set; }

        public Criticidad? Criticidad { get; set; }

        public string TargetUsername { get; set; } = null;
    }
}
