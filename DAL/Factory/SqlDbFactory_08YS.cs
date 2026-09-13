using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_08YS
{
    public class SqlDbFactory_08YS : IDbFactory_08YS
    {
        private const string NombreClaveConfig = "TP_Ing_Soft";
        private const string ConnectionStringDefault =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TP_Ing_Soft;Integrated Security=True";

        // Seteable manualmente (ej: instalador, tests). Si queda vacía, se resuelve
        // desde App.config y, si tampoco hay nada ahí, se usa el default local.
        // TODAS las conexiones de la DAL pasan por acá (Connection_08YS recibe el
        // factory, y el factory siempre lee de esta propiedad).
        private static string _connectionStringOverride;

        public static string ConnectionString
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_connectionStringOverride))
                    return _connectionStringOverride;

                string desdeConfig = ConfigurationManager
                    .ConnectionStrings[NombreClaveConfig]?.ConnectionString;

                if (!string.IsNullOrWhiteSpace(desdeConfig))
                    return desdeConfig;

                return ConnectionStringDefault;
            }
            set { _connectionStringOverride = value; }
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(ConnectionString);

        public IDbCommand CreateCommand(string query, IDbConnection connection)
            => new SqlCommand(query, (SqlConnection)connection);

        public IDbDataParameter CreateParameter(string name, object value)
            => new SqlParameter(name, value ?? DBNull.Value);

        public IDbDataParameter CreateOutputParameter(string name)
       => new SqlParameter(name, SqlDbType.Int) { Direction = ParameterDirection.Output };

        public IDbDataParameter CreateTableValuedParameter(string name, DataTable value, string typeName)
            => new SqlParameter(name, SqlDbType.Structured) { TypeName = typeName, Value = value };

        public DataTable FillDataTable(IDbCommand command)
        {
            var dt = new DataTable();
            using (var adapter = new SqlDataAdapter((SqlCommand)command))
                adapter.Fill(dt);
            return dt;
        }

        public DataSet FillDataSet(IDbCommand command)
        {
            var ds = new DataSet();
            using (var adapter = new SqlDataAdapter((SqlCommand)command))
                adapter.Fill(ds);
            return ds;
        }

        public string GetConnectionString() => ConnectionString;
    }
}
