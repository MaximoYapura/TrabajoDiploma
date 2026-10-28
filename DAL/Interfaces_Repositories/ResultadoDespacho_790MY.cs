namespace DAL_08YS.Interfaces_Repositories
{
    public enum ResultadoDespacho_790MY
    {
        Ok,
        NoEncontrado,
        /// <summary>El pedido ya estaba Listo / con StockDescontado: no se descuenta de nuevo.</summary>
        YaProcesado,
        /// <summary>El stock fisico no alcanza para descontar (se revierte todo).</summary>
        StockFisicoInsuficiente
    }
}
