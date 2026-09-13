using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_08YS.Entities.Comparers
{
    public class AccessComponentComparer_08YS : IEqualityComparer<AccessComponent_08YS>
    {
        public static readonly AccessComponentComparer_08YS Instance = new AccessComponentComparer_08YS();
        private AccessComponentComparer_08YS() { }

        public bool Equals(AccessComponent_08YS x, AccessComponent_08YS y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;
            if (x.GetType() != y.GetType()) return false;

            if (x is Familia_08YS fa && y is Familia_08YS fb) return fa.FamiliaID == fb.FamiliaID;
            if (x is Permiso_08YS pa && y is Permiso_08YS pb) return pa.PermisoID == pb.PermisoID;
            return false;
        }

        public int GetHashCode(AccessComponent_08YS obj)
        {
            if (obj is Familia_08YS f)
                return (typeof(Familia_08YS).GetHashCode() * 397) ^ f.FamiliaID.GetHashCode();
            if (obj is Permiso_08YS p)
                return (typeof(Permiso_08YS).GetHashCode() * 397) ^ p.PermisoID.GetHashCode();
            return obj.GetHashCode();
        }
    }
}
