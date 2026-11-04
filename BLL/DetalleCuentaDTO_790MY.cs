using BE_08YS;
using System.Collections.Generic;

namespace BLL_08YS
{
    // La cuenta de una mesa tal como la muestra Caja y cobro.
    public class DetalleCuentaDTO_790MY
    {
        public int ReservaID { get; set; }
        public int MesaNumero { get; set; }
        public int CantidadComensales { get; set; }
        public string CamareroNombre { get; set; }
        public string ClienteNombre { get; set; }
        public List<ItemFactura_790MY> Items { get; set; } = new List<ItemFactura_790MY>();
        public ImportesCuenta_790MY Importes { get; set; }

        // Factura Abierta de la mesa, o null si todavía no se emitió.
        public Factura_790MY FacturaAbierta { get; set; }

        // Pedidos que todavía están en cocina. Mientras haya, no se puede cobrar.
        public int PedidosSinDespachar { get; set; }
    }
}
