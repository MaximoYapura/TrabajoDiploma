using BE_08YS;

namespace BLL_08YS
{
    // Una mesa con su estado en el plano del salón. Lo arma MesaBLL_790MY; no se guarda.
    public class MesaMapaDTO_790MY
    {
        public Mesa_790MY Mesa { get; }
        public EstadoMapa_790MY EstadoMapa { get; }
        public bool EsSeleccionable =>
            EstadoMapa == EstadoMapa_790MY.DisponibleRecomendada ||
            EstadoMapa == EstadoMapa_790MY.Disponible;

        // Las mesas de 1 o 2 personas se dibujan redondas.
        public bool EsCircular => Mesa.Capacidad <= 2;

        public MesaMapaDTO_790MY(Mesa_790MY mesa, EstadoMapa_790MY estadoMapa)
        {
            Mesa       = mesa;
            EstadoMapa = estadoMapa;
        }
    }
}
