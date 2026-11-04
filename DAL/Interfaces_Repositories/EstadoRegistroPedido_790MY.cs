namespace DAL_08YS.Interfaces_Repositories
{
    // Cómo terminó el intento de grabar un pedido.
    public enum EstadoRegistroPedido_790MY
    {
        Ok,
        // A algún producto no le alcanzan las porciones disponibles.
        StockInsuficiente,
        // Algún producto no existe, está dado de baja o no tiene precio.
        ProductoNoVigente,
        // La mesa se cerró mientras se grababa el pedido.
        ReservaNoAsistida
    }
}
