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
        Perfiles
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
        RolModificado
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
