using BE_08YS;
using System;
using System.Collections.Generic;

namespace DAL_08YS.Interfaces_Repositories
{
    /// <summary>Acceso a la tabla espejo Productos_C (bitácora de cambios de productos).</summary>
    public interface IProductoCRepository_790MY
    {
        /// <summary>
        /// Versiones que cumplen los filtros (todos opcionales), de la más reciente a la más
        /// antigua. El nombre se busca por coincidencia parcial; fechaHasta incluye todo ese día.
        /// </summary>
        List<Producto_C_790MY> ObtenerHistorial(int? productoID, string nombre, DateTime? fechaDesde, DateTime? fechaHasta);

        /// <summary>Versión vigente (Act = 1) del producto, o null si no tiene.</summary>
        Producto_C_790MY ObtenerVigente(int productoID);

        Producto_C_790MY ObtenerPorId(int idHistorico);

        /// <summary>
        /// Marca la versión como vigente. El trigger producto_c_actualizado restaura sus
        /// datos en Productos, lo que a su vez genera la nueva versión vigente.
        /// </summary>
        void ActivarVersion(int idHistorico);
    }
}
