namespace DAL_08YS.Interfaces_Repositories
{
    public enum EstadoRegistroPedido_790MY
    {
        Ok,
        /// <summary>Algun producto no alcanza (stock fisico - porciones comprometidas).</summary>
        StockInsuficiente,
        /// <summary>Algun producto no existe, esta inactivo o no tiene precio vigente.</summary>
        ProductoNoVigente,
        /// <summary>La reserva dejo de estar Asistida antes del commit.</summary>
        ReservaNoAsistida
    }
}
