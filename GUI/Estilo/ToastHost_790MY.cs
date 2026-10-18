using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using T = GUI_08YS.Estilo.TemaFogon_790MY;

namespace GUI_08YS.Estilo
{
    /// <summary>
    /// Aviso de éxito no bloqueante (Etapa 2, E2-D1 / E2-D4): aparece en la esquina inferior
    /// derecha de un control, no toma el foco y se cierra solo a los 4 s (o con un clic).
    /// </summary>
    public partial class ToastHost_790MY : UserControl
    {
        private const int Margen = T.Espacio4;

        public ToastHost_790MY()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Muestra <paramref name="mensaje"/> sobre la esquina inferior derecha de
        /// <paramref name="ancla"/>. Un toast anterior sobre la misma ancla se reemplaza.
        /// </summary>
        public static void Mostrar(Control ancla, string mensaje)
        {
            if (ancla == null || ancla.IsDisposed) return;

            bool anclaEsContenedor = ancla is Form || ancla.Parent == null;
            Control contenedor = anclaEsContenedor ? ancla : ancla.Parent;

            foreach (var previo in contenedor.Controls.OfType<ToastHost_790MY>().ToList())
                previo.Cerrar();

            var toast = new ToastHost_790MY();
            toast.lblMensaje_790MY.Text = "✓  " + mensaje;
            toast.AccessibleName = mensaje;
            contenedor.Controls.Add(toast);
            toast.Size = toast.PreferredSize;

            Rectangle zona = anclaEsContenedor ? contenedor.ClientRectangle : ancla.Bounds;
            toast.Location = new Point(Math.Max(zona.Left, zona.Right - toast.Width - Margen),
                                       Math.Max(zona.Top, zona.Bottom - toast.Height - Margen));
            toast.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            toast.BringToFront();
            toast.tmrCierre_790MY.Start();
        }

        private void Cerrar()
        {
            tmrCierre_790MY.Stop();
            Parent?.Controls.Remove(this);
            Dispose();
        }

        private void tmrCierre_790MY_Tick(object sender, EventArgs e) => Cerrar();

        private void Toast_Click(object sender, EventArgs e) => Cerrar();

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var lapiz = new Pen(T.OkBorde))
                e.Graphics.DrawRectangle(lapiz, 0, 0, Width - 1, Height - 1);
        }
    }
}
