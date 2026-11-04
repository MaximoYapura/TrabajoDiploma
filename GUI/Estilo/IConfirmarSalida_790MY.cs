namespace GUI_08YS.Estilo
{
    // La implementan las pantallas que pueden tener algo sin guardar. FormMDI_08YS pregunta
    // antes de cerrar sesión o salir de la aplicación.
    public interface IConfirmarSalida_790MY
    {
        // True si se puede salir (no hay nada pendiente o el usuario lo descartó).
        bool ConfirmarSalida();
    }
}
