namespace GUI_08YS.Estilo
{
    /// <summary>
    /// Ítem genérico de ComboBox: guarda el valor y muestra el texto traducido.
    /// </summary>
    internal sealed class OpcionCombo_790MY<T>
    {
        public T Valor { get; set; }
        public string Texto { get; set; }
        public override string ToString() => Texto;
    }
}
