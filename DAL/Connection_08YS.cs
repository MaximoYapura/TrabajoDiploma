using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_08YS
{
    public abstract class Connection_08YS
    {
        private readonly IDbFactory_08YS _factory;

        protected Connection_08YS(IDbFactory_08YS factory)
        {
            _factory = factory;
        }

        // Helper para crear parametros de los repositorios concretos sin necesitar una referencia directa al factory
        protected IDbDataParameter Param(string name, object value)
            => _factory.CreateParameter(name, value);

        protected IDbDataParameter ParamOutput(string name)
            => _factory.CreateOutputParameter(name);

        protected IDbDataParameter ParamTVP(string name, DataTable value, string typeName = "dbo.IntList")
            => _factory.CreateTableValuedParameter(name, value, typeName);

        protected DataTable GetDataTable(string query, IDbDataParameter[] parameters = null, bool storedProcedure = false)
        {
            using (IDbConnection conn = _factory.CreateConnection())
            using (IDbCommand cmd = _factory.CreateCommand(query, conn))
            {
                if (storedProcedure)
                    cmd.CommandType = CommandType.StoredProcedure;
                
                if (parameters != null)
                    foreach (var p in parameters)
                        cmd.Parameters.Add(p);

                return _factory.FillDataTable(cmd);
            }
        }

        protected DataSet GetDataSet(string query, IDbDataParameter[] parameters = null, bool storedProcedure = false)
        {
            using (IDbConnection conn = _factory.CreateConnection())
            using (IDbCommand cmd = _factory.CreateCommand(query, conn))
            {
                if (storedProcedure) 
                    cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                    foreach (var p in parameters) cmd.Parameters.Add(p);

                return _factory.FillDataSet(cmd);
            }
        }

        protected bool ExecuteNonQuery(string query, IDbDataParameter[] parameters = null, bool storedProcedure = false)
        {
            using (IDbConnection conn = _factory.CreateConnection())
            using (IDbCommand cmd = _factory.CreateCommand(query, conn))
            {
                if (storedProcedure)
                    cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                    foreach (var p in parameters)
                        cmd.Parameters.Add(p);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        protected T ExecuteScalar<T>(string query, IDbDataParameter[] parameters = null, bool storedProcedure = false)
        {
            using (IDbConnection conn = _factory.CreateConnection())
            using (IDbCommand cmd = _factory.CreateCommand(query, conn))
            {
                if (storedProcedure)
                    cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                    foreach (var p in parameters)
                        cmd.Parameters.Add(p);

                conn.Open();
                object result = cmd.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                    return default(T);

                return (T)Convert.ChangeType(result, typeof(T));
            }
        }
    }
}
