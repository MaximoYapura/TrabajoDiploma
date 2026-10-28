using BE_08YS;
using System.Collections.Generic;

namespace BLL_08YS
{
    /// <summary>Detalle de consumo de una mesa listo para revisar / facturar.</summary>
    public class DetalleCuentaDTO_790MY
    {
        public int ReservaID { get; set; }
        public int MesaNumero { get; set; }
        public int CantidadComensales { get; set; }
        public string CamareroNombre { get; set; }
        public string ClienteNombre { get; set; }
        public List<ItemFactura_790MY> Items { get; set; } = new List<ItemFactura_790MY>();
        public ImportesCuenta_790MY Importes { get; set; }

        /// <summary>Factura Abierta vigente (null si todavía no se generó la cuenta).</summary>
        public Factura_790MY FacturaAbierta { get; set; }

        /// <summary>Pedidos Pendiente/EnPreparacion: impiden el cobro hasta que cocina los despache.</summary>
        public int PedidosSinDespachar { get; set; }
    }
}
