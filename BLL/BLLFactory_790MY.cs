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
    }
}
