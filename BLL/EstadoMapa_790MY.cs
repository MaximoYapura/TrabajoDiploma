namespace BLL_08YS
{
    /// <summary>
    /// Estado visual/funcional de una mesa en el plano del salón para un turno dado.
    /// Calculado en BLL; no se persiste en BD.
    /// </summary>
    public enum EstadoMapa_790MY
    {
        /// <summary>Libre en ese turno Y es el mejor ajuste de capacidad para los comensales.</summary>
        DisponibleRecomendada,
        /// <summary>Libre en ese turno pero no es el mejor ajuste.</summary>
        Disponible,
        /// <summary>Tiene una reserva Confirmada para ese turno.</summary>
        Ocupada,
        /// <summary>Activa pero su capacidad es menor a los comensales solicitados.</summary>
        Incompatible
    }
}
