using GUI_08YS.RF2;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using E = GUI_08YS.RF2.EstiloRF2_790MY;

namespace GUI_08YS.Maestros
{
    /// <summary>
    /// Estilo común de las grillas de los ABM de RF2 (Empleados, Productos):
    /// encabezado bordó, filas altas con separador suave y celdas de estado
    /// dibujadas como "píldoras".
    /// </summary>
    internal static class EstiloMaestros_790MY
    {
        public static readonly Color Encabezado = Color.FromArgb(110, 26, 46);

        public static void EstilizarGrilla(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = Color.White;
            dgv.GridColor = Color.FromArgb(236, 228, 218);
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 40;
            dgv.RowHeadersVisible = false;
            dgv.RowTemplate.Height = 46;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ReadOnly = true;
            dgv.MultiSelect = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Encabezado;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Encabezado;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);

            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10.5F);
            dgv.DefaultCellStyle.ForeColor = E.Texto;
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.SelectionBackColor = E.BordoSuave;
            dgv.DefaultCellStyle.SelectionForeColor = E.Texto;
            dgv.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
        }

        /// <summary>Dibuja el texto de la celda como una píldora de color (estado, cargo, categoría).</summary>
        public static void PintarPildora(DataGridViewCellPaintingEventArgs e, string texto, Color fondo, Color colorTexto)
        {
            e.PaintBackground(e.CellBounds, (e.State & DataGridViewElementStates.Selected) != 0);
            if (string.IsNullOrEmpty(texto)) { e.Handled = true; return; }

            using (var fuente = new Font("Segoe UI", 9F, FontStyle.Bold))
            {
                Size medida = TextRenderer.MeasureText(texto, fuente);
                var rect = new Rectangle(e.CellBounds.X + 10, e.CellBounds.Y + (e.CellBounds.Height - 24) / 2,
                                         medida.Width + 14, 24);

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath ruta = PanelRedondeado_790MY.CrearRuta(rect, 12))
                using (var relleno = new SolidBrush(fondo))
                    e.Graphics.FillPath(relleno, ruta);

                TextRenderer.DrawText(e.Graphics, texto, fuente, rect, colorTexto,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
            e.Handled = true;
        }

        public static void PintarEstado(DataGridViewCellPaintingEventArgs e, bool activo, string textoActivo, string textoInactivo)
        {
            if (activo)
                PintarPildora(e, "✓ " + textoActivo, E.ListoFondo, E.ListoTexto);
            else
                PintarPildora(e, "✕ " + textoInactivo, Color.FromArgb(252, 228, 228), Color.FromArgb(170, 30, 30));
        }

        /// <summary>Estilo de botón primario (bordó) o secundario (contorno).</summary>
        public static void EstilizarBoton(Button boton, bool primario)
        {
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 1;
            boton.FlatAppearance.BorderColor = E.Bordo;
            boton.BackColor = primario ? E.Bordo : Color.White;
            boton.ForeColor = primario ? Color.White : E.Bordo;
            boton.FlatAppearance.MouseOverBackColor = primario ? E.BordoHover : E.BordoSuave;
            boton.Cursor = Cursors.Hand;
        }
    }
}
