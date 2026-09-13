using Service_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_08YS
{
    internal static class PermissionFilter_08YS
    {
        public static void Aplicar(Control raiz, Dictionary<string, Permisos> mapa)
        {
            foreach (var kvp in mapa)
            {
                var encontrados = raiz.Controls.Find(kvp.Key, searchAllChildren: true);
                if (encontrados.Length > 0)
                    encontrados[0].Visible = SessionManager_08YS.Instance.HasPermission(kvp.Value);
            }
        }

        public static void AplicarMenuStrip(ContextMenuStrip menu, Dictionary<string, Permisos> mapa)
            => AplicarItems(menu.Items, mapa);

        private static void AplicarItems(ToolStripItemCollection items, Dictionary<string, Permisos> mapa)
        {
            foreach (ToolStripItem item in items)
            {
                if (mapa.TryGetValue(item.Name, out var permiso))
                    item.Visible = SessionManager_08YS.Instance.HasPermission(permiso);

                // Recursivo por si hay submenús anidados
                if (item is ToolStripMenuItem menuItem && menuItem.HasDropDownItems)
                    AplicarItems(menuItem.DropDownItems, mapa);
            }
        }
    }
}
