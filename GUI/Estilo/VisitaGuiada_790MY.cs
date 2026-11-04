using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Service_08YS;

namespace GUI_08YS.Estilo
{
    // Un paso de la Visita Guiada: el elemento real que se resalta y su explicación.
    public sealed class PasoVisita_790MY
    {
        public PasoVisita_790MY(Control objetivo, string titulo, string texto)
        {
            Objetivo = objetivo;
            Titulo = titulo;
            Texto = texto;
        }

        public Control Objetivo { get; }
        public string Titulo { get; }
        public string Texto { get; }
    }

    // Visita guiada del primer ingreso. Los pasos los arma la ventana principal con los
    // botones que el usuario ve según sus permisos. Esta clase saca una captura de la
    // ventana, la oscurece dejando un hueco sobre cada botón y pone el globo al lado.
    // Al completarla o saltearla se dispara Terminada.
    public sealed class VisitaGuiada_790MY
    {
        private const int Separacion = 4;
        private const int Distancia = 12;

        private readonly Control _host;
        private readonly VisitaOverlay_790MY _capa;
        private readonly GloboAyuda_790MY _globo;
        private List<PasoVisita_790MY> _pasos = new List<PasoVisita_790MY>();
        private int _indice;

        // El usuario completó o saltó la visita (no se dispara si se cortó por otra causa).
        public event EventHandler Terminada;

        // La visita se cerró por cualquier motivo (incluida Terminada).
        public event EventHandler Cerrada;

        public VisitaGuiada_790MY(Control host, VisitaOverlay_790MY capa, GloboAyuda_790MY globo)
        {
            _host = host;
            _capa = capa;
            _globo = globo;
            _globo.AnteriorSolicitado += (s, e) => Ir(_indice - 1);
            _globo.SiguienteSolicitado += (s, e) => { if (_indice < _pasos.Count - 1) Ir(_indice + 1); else Terminar(); };
            _globo.CerrarSolicitado += (s, e) => Terminar();
        }

        public bool Activa { get; private set; }

        // partes: Contenedores visibles de la ventana, para componer la captura.
        public bool Iniciar(IEnumerable<PasoVisita_790MY> pasos, IEnumerable<Control> partes)
        {
            Cortar();
            _pasos = pasos.Where(p => p.Objetivo != null && p.Objetivo.Visible && p.Objetivo.Width > 0).ToList();
            if (_pasos.Count == 0) return false;

            _capa.Captura = Capturar(partes);
            _capa.Bounds = _host.ClientRectangle;
            _capa.Visible = true;
            _capa.BringToFront();
            Activa = true;
            Ir(0);
            return true;
        }

        // Cierra la visita sin marcarla como vista (navegación, cambio de tamaño, cobro).
        public void Cortar()
        {
            if (!Activa && !_capa.Visible) return;
            Activa = false;
            _globo.Visible = false;
            _capa.Visible = false;
            _capa.Captura = null;
            _pasos.Clear();
            Cerrada?.Invoke(this, EventArgs.Empty);
        }

        private void Terminar()
        {
            if (!Activa) return;
            Cortar();
            Terminada?.Invoke(this, EventArgs.Empty);
        }

        private static string T(string clave) => TraductorManager_08YS.Instance.GetTexto(clave);

        private void Ir(int indice)
        {
            if (!Activa || indice < 0 || indice >= _pasos.Count) return;
            _indice = indice;
            var paso = _pasos[indice];

            Rectangle objetivo = _host.RectangleToClient(paso.Objetivo.RectangleToScreen(paso.Objetivo.ClientRectangle));
            Rectangle hueco = Rectangle.Intersect(Rectangle.Inflate(objetivo, Separacion, Separacion), LimiteDe(paso.Objetivo));
            _capa.Hueco = hueco;

            _globo.Mostrar(paso.Titulo, paso.Texto, null, indice, _pasos.Count,
                           string.Format(T("AY_sug_contador"), indice + 1, _pasos.Count),
                           T("AY_sug_anterior"), T("AY_sug_siguiente"), T("AY_vis_terminar"), T("AY_vis_saltar"));
            _globo.Visible = true;
            _globo.Size = _globo.PreferredSize;
            _globo.Location = UbicacionGlobo(hueco, _host.ClientRectangle, _globo.Size);
            _globo.BringToFront();
            _globo.EnfocarSiguiente();
        }

        // Zona en la que puede quedar el hueco: el contenedor de primer nivel que contiene al
        // elemento (barra lateral, barra superior), achicado por el borde que dibuja la capa.
        // Así el marco de un elemento que ocupa todo el ancho de la barra (el encabezado con
        // nombre y rol) no invade el área de contenido.
        private Rectangle LimiteDe(Control objetivo)
        {
            Control contenedor = objetivo;
            while (contenedor.Parent != null && contenedor.Parent != _host) contenedor = contenedor.Parent;
            Rectangle zona = _host.RectangleToClient(contenedor.RectangleToScreen(contenedor.ClientRectangle));
            int margen = VisitaOverlay_790MY.AnchoBorde;
            return Rectangle.Inflate(zona, -margen, -margen);
        }

        // A la derecha del elemento si entra (la barra está a la izquierda); si no, abajo o arriba.
        private static Point UbicacionGlobo(Rectangle hueco, Rectangle zona, Size globo)
        {
            int y = Math.Max(zona.Top + Distancia, Math.Min(hueco.Top, zona.Bottom - globo.Height - Distancia));
            if (hueco.Right + Distancia + globo.Width <= zona.Right)
                return new Point(hueco.Right + Distancia, y);
            int x = Math.Max(zona.Left + Distancia, Math.Min(hueco.Left, zona.Right - globo.Width - Distancia));
            if (hueco.Bottom + Distancia + globo.Height <= zona.Bottom)
                return new Point(x, hueco.Bottom + Distancia);
            return new Point(x, Math.Max(zona.Top + Distancia, hueco.Top - Distancia - globo.Height));
        }

        // Captura la ventana por partes con DrawToBitmap. Capturarla entera sale gris, y una
        // captura de pantalla fallaría si otra ventana la tapa.
        private Bitmap Capturar(IEnumerable<Control> partes)
        {
            var bmp = new Bitmap(Math.Max(1, _host.ClientSize.Width), Math.Max(1, _host.ClientSize.Height));
            try
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(_host.BackColor);
                    foreach (var parte in partes.Where(p => p != null && p.Visible && p.Width > 0 && p.Height > 0))
                    {
                        Rectangle destino = _host.RectangleToClient(parte.RectangleToScreen(parte.ClientRectangle));
                        using (var pieza = new Bitmap(parte.Width, parte.Height))
                        {
                            parte.DrawToBitmap(pieza, new Rectangle(Point.Empty, parte.Size));
                            g.DrawImageUnscaled(pieza, destino.Location);
                        }
                    }
                }
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException || ex is System.ComponentModel.Win32Exception)
            {
                // Sin captura la capa pinta un fondo liso: la visita sigue funcionando.
                bmp.Dispose();
                return null;
            }
            return bmp;
        }
    }
}
