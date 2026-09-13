using DAL_08YS.Repositories_Interfaces;
using MPP_08YS;
using Service_08YS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_08YS
{
    public class SqlUserRepository_08YS : Connection_08YS, IUserRepository_08YS
    {
        public SqlUserRepository_08YS(IDbFactory_08YS factory) : base(factory) { }
        private IDbDataParameter[] ToParameters(User_08YS user)
        {
            return new[]
            {
                Param("@Username",  user.Username),
                Param("@DNI",       user.DNI),
                Param("@Rol",     user.Rol.RolID),
                Param("@Nombre",    user.Nombre),
                Param("@Apellido",  user.Apellido),
                Param("@Hash",      user.Hash),
                Param("@Salt",      user.Salt),
                Param("@Email",     user.Email),
                Param("@Bloqueado", user.Bloqueado),
                Param("@Activo",    user.Activo),
                Param("@Idioma",    user.Idioma)
        };
        }

        //public List<User_08YS> GetAllRoots()
        //{
        //    DataTable dt = GetDataTable("SELECT * FROM Users");
        //    return UserMapper_08YS.FromDataTable(dt);
        //}
        public List<User_08YS> GetAll()
        {
            string query = @"
        SELECT u.Username, u.DNI, u.RolID, u.Nombre, u.Apellido, u.Hash, u.Salt, u.Email, u.Bloqueado, u.Activo, u.Idioma,
               r.Nombre AS NombreRol 
        FROM Users u 
        INNER JOIN Roles r ON u.RolID = r.RolID"; 
            DataTable dt = GetDataTable(query);
            return UserMapper_08YS.FromDataTable(dt);
        }

        public User_08YS GetByUsername(string username)
        {
            DataTable dt = GetDataTable("SELECT * FROM Users WHERE Username = @Username",
                                    new[] { Param("@Username", username) });

            if (dt.Rows.Count == 0) return null;

            return dt.Rows.Count > 0 ? UserMapper_08YS.FromDataRow(dt.Rows[0]) : null;
        }

        public void LockOut(string username)
        {
            ExecuteNonQuery
            (
                "UPDATE Users SET Bloqueado = 1 WHERE Username = @Username",
                new[] { Param("@Username", username) }
            );
        }

        public void Unlock(string username)
        {
            ExecuteNonQuery
            (
                "UPDATE Users SET Bloqueado = 0 WHERE Username = @Username",
                new[] { Param("@Username", username) }
            );
        }

        public void Create(User_08YS user)
        {
            ExecuteNonQuery
            (
                "INSERT INTO Users (Username, DNI, RolID, Nombre, Apellido, Hash, Salt, Email, Bloqueado, Activo,Idioma) " +
                "VALUES (@Username, @DNI, @Rol, @Nombre, @Apellido, @Hash, @Salt, @Email, @Bloqueado, @Activo,@Idioma)",
                ToParameters(user)
            );
        }

        public bool Exists(int dni)
        {
            return ExecuteScalar<int>
                (   
                    "SELECT COUNT(1) FROM Users WHERE DNI = @DNI",
                    new[] { Param("@DNI", dni) }
                ) > 0;
        }

        public void Modify(User_08YS user, string login)
        {
            ExecuteNonQuery
            (
                "UPDATE Users SET Email = @Email , RolID=@Rol WHERE Username = @Username",
                new[] { Param("@Email", user.Email),
                        Param("@Rol", user.Rol.RolID),
                        Param("@Username",user.Username)}
            );
        }

        public void UpdateState(string username,bool estadoNuevo)
        {
            ExecuteNonQuery
            ( 
                "UPDATE Users SET Activo = @Activo WHERE Username=@Username",
                new[] { Param("@Activo",estadoNuevo),
                        Param("@Username",username)}
            );
            
        }

        public void UpdatePassword(string username, string hashNuevo, string saltNuevo)
        {
            ExecuteNonQuery
            (
                "UPDATE Users SET Hash = @Hash, Salt = @Salt WHERE Username = @Username",
                new[] { Param("@Hash", hashNuevo),
                        Param("@Salt", saltNuevo),
                        Param("@Username", username) }
            );
        }
        public void UpdateLanguage(string username, string codigoIdioma)
        {
            ExecuteNonQuery("UPDATE Users SET Idioma = @idioma WHERE Username = @Username",
                new[] {
                    Param("@idioma", codigoIdioma),
                     Param("@Username", username)
                });
        }
    }
}
