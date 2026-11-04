using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using T = GUI_08YS.Estilo.TemaFogon_790MY;

namespace GUI_08YS.Estilo
{
    // Aviso de éxito abajo a la derecha. No toma el foco y se cierra solo a los 4 segundos
    // (o con un clic).
    public partial class ToastHost_790MY : UserControl
    {
        private const int Margen = T.Espacio4;

        public ToastHost_790MY()
        {
            InitializeComponent();
        }

        // Muestra el mensaje en la esquina inferior derecha de ancla. Si ya había uno, lo reemplaza.
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
