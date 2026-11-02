namespace GUI_08YS.Estilo
{
    /// <summary>
    /// Pantalla con trabajo sin guardar (Entrega 2, M8). FormMDI_08YS la consulta antes de
    /// cerrar sesión o salir; la navegación del menú lo resuelve con el FormClosing del hijo.
    /// </summary>
    public interface IConfirmarSalida_790MY
    {
        /// <summary>True si se puede salir (no hay nada pendiente o el usuario lo descartó).</summary>
        bool ConfirmarSalida();
    }
}
