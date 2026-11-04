namespace BLL_08YS
{
    // Cómo se ve cada mesa en el plano del salón para un turno. Lo calcula la BLL, no se guarda.
    public enum EstadoMapa_790MY
    {
        // Libre y es la de menor capacidad que alcanza para los comensales.
        DisponibleRecomendada,
        // Libre, pero hay otra que se ajusta mejor.
        Disponible,
        // Tiene una reserva en ese turno, o (si es el turno en curso) está ocupada ahora.
        Ocupada,
        // No le alcanza la capacidad para los comensales.
        Incompatible
    }
}
