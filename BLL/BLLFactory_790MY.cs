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
            IMesaRepository_790MY repo = new SqlMesaRepository_790MY();
            BitacoraBLL_08YS bitacoraBll = BLLFactory_08YS.CreateBitacoraBLL();
            return new MesaBLL_790MY(repo, bitacoraBll);
        }

        public static ReservaBLL_790MY CreateReservaBLL()
        {
            IReservaRepository_790MY reservaRepo = new SqlReservaRepository_790MY();
            IClienteRepository_790MY clienteRepo = new SqlClienteRepository_790MY();
            return new ReservaBLL_790MY(reservaRepo, clienteRepo);
        }
    }
}
