using BE_08YS;
using DAL_08YS.Interfaces_Repositories;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using Service_08YS.Entities.Bitacora;
using System;
using System.Collections.Generic;

namespace BLL_08YS
{
    /// <summary>
    /// Bitácora de cambios de productos (patente AuditarProductos). Consulta el historial
    /// de Productos_C, arma el diff contra la versión vigente y ejecuta el rollback:
    /// activar la versión dispara los triggers que restauran Productos y generan la nueva
    /// versión vigente; después se recalculan los DV y se registra en la bitácora.
    /// </summary>
    public class ProductoCBLL_790MY
    {
        public const int DiasPorDefecto = 30;

        private readonly IProductoCRepository_790MY _productoCRepository;
        private readonly ProductoBLL_790MY _productoBll;
        private readonly BitacoraBLL_08YS _bitacoraBll;

        public ProductoCBLL_790MY(IProductoCRepository_790MY productoCRepository,
                                  ProductoBLL_790MY productoBll,
                                  BitacoraBLL_08YS bitacoraBll)
        {
            _productoCRepository = productoCRepository ?? throw new ArgumentNullException(nameof(productoCRepository));
            _productoBll = productoBll ?? throw new ArgumentNullException(nameof(productoBll));
            _bitacoraBll = bitacoraBll ?? throw new ArgumentNullException(nameof(bitacoraBll));
        }

        /// <summary>
        /// Historial filtrado. Sin fechas se consultan los últimos <see cref="DiasPorDefecto"/>
        /// días; si solo falta una de las dos, se completa con ese mismo criterio.
        /// </summary>
        public List<Producto_C_790MY> ConsultarHistorial(int? productoID, string nombre, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.AuditarProductos);

            if (!fechaDesde.HasValue && !fechaHasta.HasValue)
                fechaDesde = FechaDesdePorDefecto;

            if (fechaDesde.HasValue && fechaHasta.HasValue && fechaDesde.Value.Date > fechaHasta.Value.Date)
                throw MensajesBLL_790MY.Regla("BC_msg_rango_invalido");

            if (productoID.HasValue && productoID.Value <= 0)
                throw MensajesBLL_790MY.Regla("BC_msg_codigo_invalido");

            return _productoCRepository.ObtenerHistorial(productoID, nombre, fechaDesde, fechaHasta);
        }

        public static DateTime FechaDesdePorDefecto => DateTime.Today.AddDays(-DiasPorDefecto);

        /// <summary>Versión vigente actual del producto (Act = 1), o null si no tiene.</summary>
        public Producto_C_790MY ObtenerVigente(int productoID)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.AuditarProductos);
            return _productoCRepository.ObtenerVigente(productoID);
        }

        /// <summary>Compara la versión seleccionada contra la vigente del mismo producto.</summary>
        public DiferenciaVersion_790MY ObtenerDiferencias(Producto_C_790MY seleccion, Producto_C_790MY vigente)
        {
            if (seleccion == null)
                throw new ArgumentNullException(nameof(seleccion));

            var diff = new DiferenciaVersion_790MY { Seleccion = seleccion, Vigente = vigente };
            if (vigente == null || vigente.ID == seleccion.ID)
                return diff;

            if (vigente.ProductoID != seleccion.ProductoID)
                throw new ArgumentException("Las versiones a comparar deben ser del mismo producto.", nameof(vigente));

            diff.DeltaPrecio = seleccion.Precio - vigente.Precio;
            diff.DeltaStock = seleccion.StockPorciones - vigente.StockPorciones;
            diff.CambiaPrecio = diff.DeltaPrecio != 0;
            diff.CambiaStock = diff.DeltaStock != 0;
            diff.CambiaEstado = seleccion.Activo != vigente.Activo;
            diff.CambiaCategoria = seleccion.Categoria != vigente.Categoria;
            diff.CambiaNombre = !string.Equals(seleccion.Nombre, vigente.Nombre, StringComparison.Ordinal);
            diff.CambiaControlaStock = seleccion.ControlaStock != vigente.ControlaStock;
            return diff;
        }

        /// <summary>
        /// Rollback: vuelve el producto a los datos de la versión histórica. El historial no
        /// se borra: si la versión difiere en datos de negocio queda como una fila vigente
        /// nueva, así que el rollback puede deshacerse restaurando la versión que estaba
        /// vigente antes; si difiere solo en stock, el trigger actualiza el stock de la
        /// vigente actual sin crear versión (producto_actualizado, A5).
        /// </summary>
        public void RestaurarVersion(Producto_C_790MY versionHistorica)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.AuditarProductos);

            if (versionHistorica == null)
                throw new ArgumentNullException(nameof(versionHistorica));

            if (versionHistorica.Act)
                throw MensajesBLL_790MY.Regla("BC_msg_ya_vigente", versionHistorica.ID);

            // Se relee de la base: la grilla puede estar desactualizada (otro rollback,
            // una modificación desde el ABM o un descuento de stock en el medio).
            Producto_C_790MY version = _productoCRepository.ObtenerPorId(versionHistorica.ID)
                ?? throw MensajesBLL_790MY.Regla("BC_msg_version_inexistente", versionHistorica.ID);
            if (version.Act)
                throw MensajesBLL_790MY.Regla("BC_msg_ya_vigente", version.ID);

            // Sin producto en Productos (borrado físico) el trigger no tiene qué restaurar.
            Producto_790MY actual = _productoBll.ObtenerPorId(version.ProductoID)
                ?? throw MensajesBLL_790MY.Regla("BC_msg_producto_eliminado", version.ProductoID);

            // Mismas reglas que ProductoBLL_790MY.ModificarProducto.
            if (_productoBll.ExisteNombre(version.Nombre, version.ProductoID))
                throw MensajesBLL_790MY.Regla("msg_rf2_producto_duplicado", version.Nombre);

            if (version.ControlaStock && version.StockPorciones < actual.PorcionesComprometidas)
                throw MensajesBLL_790MY.Regla("msg_rf2_producto_stock_bajo_comprometido", actual.PorcionesComprometidas);

            _productoCRepository.ActivarVersion(version.ID);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.ProductoVersionRestaurada,
                $"Prod {version.ProductoID} v{version.ID} {version.Nombre}");
        }
    }
}
