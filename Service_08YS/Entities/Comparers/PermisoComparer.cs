using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Service_08YS.Entities.Comparers
{
    public class PermisoComparer : IEqualityComparer<Permiso_08YS>
    {
        public static readonly PermisoComparer Instance = new PermisoComparer();
        private PermisoComparer() { }

        public bool Equals(Permiso_08YS x, Permiso_08YS y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            return x.PermisoID == y.PermisoID;
        }

        public int GetHashCode(Permiso_08YS obj) => obj.PermisoID.GetHashCode();
    }
}
