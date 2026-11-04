namespace GUI_08YS.Estilo
{
    // Ítem genérico de ComboBox: guarda el valor y muestra el texto traducido.
    internal sealed class OpcionCombo_790MY<T>
    {
        public T Valor { get; set; }
        public string Texto { get; set; }
        public override string ToString() => Texto;
    }
}
