using System;

namespace GUI_08YS.Estilo
{
    /// <summary>Pantallas a las que lleva el Menú Degustación (flujo real de FormMDI_08YS).</summary>
    public enum DestinoAyuda_790MY
    {
        Clientes,
        RegistrarReserva,
        AbrirMesa,
        RegistrarPedido,
        Cocina,
        CajaCobro
    }

    public class DestinoAyudaEventArgs_790MY : EventArgs
    {
        public DestinoAyudaEventArgs_790MY(DestinoAyuda_790MY destino) { Destino = destino; }

        public DestinoAyuda_790MY Destino { get; }
    }
}
