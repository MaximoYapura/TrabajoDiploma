using DAL_08YS;
using DAL_08YS.Interfaces_Repositories;
using DAL_08YS.SQL;

namespace BLL_08YS
{
    public static class BLLFactory_790MY
    {
        public static ClienteBLL_790MY CreateClienteBLL()
        {
            IClienteRepository_790MY repo = new SqlClienteRepository_790MY();
            BitacoraBLL_08YS bitacoraBll = BLLFactory_08YS.CreateBitacoraBLL();
            return new ClienteBLL_790MY(repo, bitacoraBll);
        }

        public static MesaBLL_790MY CreateMesaBLL()
        {
            IMesaRepository_790MY    mesaRepo    = new SqlMesaRepository_790MY();
            IReservaRepository_790MY reservaRepo = new SqlReservaRepository_790MY();
            BitacoraBLL_08YS         bitacoraBll = BLLFactory_08YS.CreateBitacoraBLL();
            return new MesaBLL_790MY(mesaRepo, reservaRepo, bitacoraBll);
        }

        public static ReservaBLL_790MY CreateReservaBLL()
        {
            IReservaRepository_790MY  reservaRepo  = new SqlReservaRepository_790MY();
            ClienteBLL_790MY          clienteBll   = CreateClienteBLL();
            MesaBLL_790MY             mesaBll      = CreateMesaBLL();
            EmpleadoBLL_790MY         empleadoBll  = CreateEmpleadoBLL();
            BitacoraBLL_08YS          bitacoraBll  = BLLFactory_08YS.CreateBitacoraBLL();
            return new ReservaBLL_790MY(reservaRepo, clienteBll, mesaBll, empleadoBll, bitacoraBll);
        }

        public static EmpleadoBLL_790MY CreateEmpleadoBLL()
        {
            IDbFactory_08YS factory = new SqlDbFactory_08YS();
            return new EmpleadoBLL_790MY(
                new SqlEmpleadoRepository_790MY(),
                new SqlUserRepository_08YS(factory),
                new SqlReservaRepository_790MY(),
                BLLFactory_08YS.CreateBitacoraBLL());
        }

        public static ProductoBLL_790MY CreateProductoBLL()
        {
            return new ProductoBLL_790MY(
                new SqlProductoRepository_790MY(),
                BLLFactory_08YS.CreateBitacoraBLL());
        }

        public static PedidoBLL_790MY CreatePedidoBLL()
        {
            return new PedidoBLL_790MY(
                new SqlPedidoRepository_790MY(),
                new SqlProductoRepository_790MY(),
                new SqlReservaRepository_790MY(),
                new SqlEmpleadoRepository_790MY(),
                BLLFactory_08YS.CreateBitacoraBLL());
        }

        public static FacturaBLL_790MY CreateFacturaBLL()
        {
            return new FacturaBLL_790MY(
                new SqlFacturaRepository_790MY(),
                new SqlReservaRepository_790MY(),
                new SqlPedidoRepository_790MY(),
                new SqlEmpleadoRepository_790MY(),
                BLLFactory_08YS.CreateBitacoraBLL());
        }

        public static PagoBLL_790MY CreatePagoBLL()
        {
            return new PagoBLL_790MY(
                new SqlPagoRepository_790MY(),
                CreateFacturaBLL(),
                new SqlCierreCajaRepository_790MY(),
                BLLFactory_08YS.CreateBitacoraBLL());
        }

        public static ReportePredictivoBLL_790MY CreateReportePredictivoBLL()
        {
            return new ReportePredictivoBLL_790MY(new SqlFacturaRepository_790MY());
        }

        public static ProductoCBLL_790MY CreateProductoCBLL()
        {
            return new ProductoCBLL_790MY(
                new SqlProductoCRepository_790MY(),
                new SqlProductoRepository_790MY(),
                BLLFactory_08YS.CreateBitacoraBLL());
        }
    }
}
