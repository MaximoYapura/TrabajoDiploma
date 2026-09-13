using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_08YS.Entities.Acceso
{
    public abstract class AccessComponent_08YS
    {
        public int ID { get; set; }
        public string Nombre { get; set; }

        public abstract HashSet<Permiso_08YS> GetPermisos();
    }
}
