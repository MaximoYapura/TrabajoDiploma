using Service_08YS.Entities.Comparers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_08YS.Entities.Acceso
{
    public class Rol_08YS
    {
        public int RolID { get; set; }
        public string Nombre { get; set; }

        private readonly List<AccessComponent_08YS> _componentes = new List<AccessComponent_08YS>();
        public IReadOnlyList<AccessComponent_08YS> Componentes => _componentes.AsReadOnly();

        public void Agregar(AccessComponent_08YS c) => _componentes.Add(c);

        public HashSet<Permiso_08YS> GetPermisos()
        {
            var resultado = new HashSet<Permiso_08YS>(PermisoComparer.Instance);
            foreach (var c in _componentes)
                resultado.UnionWith(c.GetPermisos());
            return resultado;
        }
        public override string ToString()
        {
            return $"{Nombre}";
        }
    }
}
