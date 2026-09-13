using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI_08YS.UserControls
{
    /// <summary>
    /// Caché estático de bitmaps de íconos FontAwesome.
    /// ToBitmap() carga la fuente TTF desde el ensamblado cada vez que se llama.
    /// Este caché garantiza que cada combinación (ícono, fuente, tamaño, color)
    /// se carga una sola vez en toda la vida de la aplicación.
    /// </summary>
    internal static class IconCache
    {
        private static readonly Dictionary<(IconChar, IconFont, int, Color), Bitmap> _cache
            = new Dictionary<(IconChar, IconFont, int, Color), Bitmap>();

        public static Bitmap Get(IconChar icon, IconFont font, int size, Color color)
        {
            if (icon == IconChar.None) return null;

            var key = (icon, font, size, color);
            if (!_cache.TryGetValue(key, out var bmp))
            {
                bmp = icon.ToBitmap(font, size, color);
                _cache[key] = bmp;
            }
            return bmp;
        }
    }
}
