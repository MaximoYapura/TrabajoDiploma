namespace BLL_08YS
{
    // Los tres tipos de sugerencia del reporte predictivo.
    public enum TipoSugerencia_790MY
    {
        // Regla de mayor confianza: armar un combo con ambos productos.
        CrearCombo,
        // Otra regla fuerte, promocionada el día de la semana con menos cuentas.
        PromocionDiaBajaDemanda,
        // Regla de mayor soporte: aviso de venta cruzada en la comanda del camarero.
        VentaCruzadaCamarero
    }
}
