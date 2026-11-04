using System;

namespace DAL_08YS.Interfaces_Repositories
{
    // Cierres de caja (tabla CierresCaja). La clave primaria es la fecha, así un día
    // no se puede cerrar dos veces aunque dos terminales lo intenten a la vez.
    public interface ICierreCajaRepository_790MY
    {
        // Graba el cierre del día. Devuelve false si esa fecha ya estaba cerrada.
        bool Registrar(DateTime fecha, decimal neta, int cantidadCuentas, string usuario);

        // True si ese día ya tiene cierre.
        bool Existe(DateTime fecha);
    }
}
