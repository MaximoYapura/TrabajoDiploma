using DAL_08YS.Interfaces_Repositories;
using Service_08YS.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_08YS.SQL
{
    public class SqlDvRepository_08YS : Connection_08YS , IDvRepository_08YS
    {
        public SqlDvRepository_08YS(IDbFactory_08YS factory) : base(factory) { }

        public List<DataTable> GetTodasLasTablas()
        {
            DataSet ds = GetDataSet("sp_GetAllTablesParaDV", storedProcedure: true);
            var tablas = new List<DataTable>();
            for (int i = 0; i < ds.Tables.Count; i++)
                tablas.Add(ds.Tables[i]);
            return tablas;
        }

        public List<DigitoVerificador_08YS> GetDVGuardado()
        {
            DataTable dt = GetDataTable("sp_GetDVTodo", storedProcedure: true);
            return dt.AsEnumerable().Select(row => new DigitoVerificador_08YS
            {
                Tabla = row["Tabla"].ToString(),
                DVH = row["DVH"].ToString(),
                DVV = row["DVV"].ToString()
            }).ToList();
        }

        public void GuardarDVTabla(DigitoVerificador_08YS entrada)
            => ExecuteNonQuery("sp_UpsertDVTabla",
                new[]
                {
                Param("@Tabla", entrada.Tabla),
                Param("@DVH",   entrada.DVH),
                Param("@DVV",   entrada.DVV)
                },
                storedProcedure: true);
    }
}
