using System;

namespace DAL_08YS.Interfaces_Repositories
{
    /// <summary>
    /// Cierres de caja diarios (tabla CierresCaja, PK = Fecha). La PK garantiza en la base
    /// que una jornada se cierre una sola vez, aunque dos terminales cierren a la vez.
    /// </summary>
    public interface ICierreCajaRepository_790MY
    {
        /// <summary>
        /// Inserta el cierre del día. Devuelve false si esa fecha ya estaba cerrada
        /// (violación de la clave primaria).
        /// </summary>
        bool Registrar(DateTime fecha, decimal neta, int cantidadCuentas, string usuario);

        /// <summary>True si existe un cierre para el día calendario indicado.</summary>
        bool Existe(DateTime fecha);
    }
}
