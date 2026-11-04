namespace DAL_08YS.Interfaces_Repositories
{
    // Cómo terminó el intento de marcar un pedido como Listo.
    public enum ResultadoDespacho_790MY
    {
        Ok,
        NoEncontrado,
        // El pedido ya estaba Listo; no se descuenta de nuevo.
        YaProcesado,
        // No alcanza el stock para descontar; se revierte todo.
        StockFisicoInsuficiente
    }
}
