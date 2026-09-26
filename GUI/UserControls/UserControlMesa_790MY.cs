using BE_08YS;
using Service_08YS;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CustomControls
{
    /// <summary>
    /// Tarjeta visual de una mesa. Es exclusivamente una responsabilidad de UI:
    /// no consulta SQL ni contiene reglas de negocio, solo dibuja el estado
    /// que le indican y notifica clicks vía el evento MesaClick.
    /// </summary>
    public class UserControlMesa_790MY : UserControl
    {
        private Mesa_790MY _mesa;
        private EstadoMesa_790MY _estadoVisual = EstadoMesa_790MY.Libre;
        private bool _seleccionable = true;
        private bool _seleccionada = false;

        public event EventHandler MesaClick;

        public UserControlMesa_790MY()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            Size = new Size(110, 90);
            Cursor = Cursors.Hand;

            Click += UserControlMesa_790MY_Click;
        }

        private void UserControlMesa_790MY_Click(object sender, EventArgs e)
        {
            if (!Seleccionable) return;
            MesaClick?.Invoke(this, EventArgs.Empty);
        }

        public Mesa_790MY Mesa
        {
            get => _mesa;
            set
            {
                _mesa = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Estado que determina el color de la tarjeta. Por defecto se puede
        /// sincronizar con Mesa.Estado, pero se expone independiente porque en
        /// algunos contextos (ej.: selección de mesa para un turno futuro) el
        /// estado físico actual de la mesa no es el dato relevante a mostrar.
        /// </summary>
        public EstadoMesa_790MY EstadoVisual
        {
            get => _estadoVisual;
            set
            {
                _estadoVisual = value;
                Invalidate();
            }
        }

        public bool Seleccionable
        {
            get => _seleccionable;
            set
            {
                _seleccionable = value;
                Cursor = value ? Cursors.Hand : Cursors.No;
                if (!value) Seleccionada = false;
                Invalidate();
            }
        }

        public bool Seleccionada
        {
            get => _seleccionada;
            set
            {
                _seleccionada = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var rect = new Rectangle(1, 1, Width - 3, Height - 3);

            using (var path = RoundedRect(rect, 10))
            {
                using (var brush = new SolidBrush(ColorDeFondo()))
                    g.FillPath(brush, path);

                Color colorBorde = Seleccionada ? Color.FromArgb(6, 20, 90) : Color.FromArgb(120, 120, 120);
                int grosorBorde = Seleccionada ? 3 : 1;
                using (var pen = new Pen(colorBorde, grosorBorde))
                    g.DrawPath(pen, path);

                if (!Seleccionable)
                {
                    using (var overlay = new SolidBrush(Color.FromArgb(140, Color.White)))
                        g.FillPath(overlay, path);
                }
            }

            string texto = _mesa != null
                ? string.Format(TraductorManager_08YS.Instance.GetTexto("UC_mesa_lblMesa"), _mesa.NroMesa)
                  + "\n"
                  + string.Format(TraductorManager_08YS.Instance.GetTexto("UC_mesa_lblPersonas"), _mesa.Capacidad)
                : string.Empty;

            var flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;
            TextRenderer.DrawText(g, texto, Font, ClientRectangle, Color.FromArgb(30, 30, 30), flags);
        }

        private Color ColorDeFondo()
        {
            switch (EstadoVisual)
            {
                case EstadoMesa_790MY.Libre:
                    return Color.FromArgb(198, 239, 206); // verde suave
                case EstadoMesa_790MY.Ocupada:
                    return Color.FromArgb(255, 199, 206); // rojo suave
                case EstadoMesa_790MY.Reservada:
                    return Color.FromArgb(255, 235, 156); // amarillo suave
                default:
                    return Color.Gainsboro;
            }
        }

        private static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
