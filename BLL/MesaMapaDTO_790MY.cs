using BE_08YS;

namespace BLL_08YS
{
    /// <summary>
    /// DTO que combina la entidad Mesa con su estado dinámico en el mapa del salón.
    /// Construido por ObtenerMesasMapa; nunca se persiste.
    /// </summary>
    public class MesaMapaDTO_790MY
    {
        public Mesa_790MY Mesa { get; }
        public EstadoMapa_790MY EstadoMapa { get; }
        public bool EsSeleccionable =>
            EstadoMapa == EstadoMapa_790MY.DisponibleRecomendada ||
            EstadoMapa == EstadoMapa_790MY.Disponible;

        /// <summary>
        /// Verdadero cuando la mesa es para 1 o 2 personas.
        /// El control visual la dibuja como elipse en lugar de rectángulo redondeado.
        /// </summary>
        public bool EsCircular => Mesa.Capacidad <= 2;

        public MesaMapaDTO_790MY(Mesa_790MY mesa, EstadoMapa_790MY estadoMapa)
        {
            Mesa       = mesa;
            EstadoMapa = estadoMapa;
        }
    }
}
