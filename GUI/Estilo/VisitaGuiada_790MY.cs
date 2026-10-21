using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Service_08YS;

namespace GUI_08YS.Estilo
{
    /// <summary>Un paso de la Visita Guiada: el elemento real que se resalta y su explicación.</summary>
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

    /// <summary>
    /// Visita Guiada de primer ingreso (Etapa 5, Fase 7). Los pasos los arma el formulario
    /// principal a partir de lo que el usuario realmente ve (botones visibles según sus
    /// permisos); esta clase captura la ventana, la muestra atenuada con un hueco sobre cada
    /// elemento y ubica el globo. Completar o saltar la visita dispara <see cref="Terminada"/>.
    /// </summary>
    public sealed class VisitaGuiada_790MY
    {
        private const int Separacion = 4;
        private const int Distancia = 12;

        private readonly Control _host;
        private readonly VisitaOverlay_790MY _capa;
        private readonly GloboAyuda_790MY _globo;
        private List<PasoVisita_790MY> _pasos = new List<PasoVisita_790MY>();
        private int _indice;

        /// <summary>El usuario completó o saltó la visita (no se dispara si se cortó por otra causa).</summary>
        public event EventHandler Terminada;

        /// <summary>La visita se cerró por cualquier motivo (incluida <see cref="Terminada"/>).</summary>
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

        /// <param name="partes">Contenedores visibles de la ventana, para componer la captura.</param>
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

        /// <summary>Cierra la visita sin marcarla como vista (navegación, cambio de tamaño, cobro).</summary>
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
            Rectangle hueco = Rectangle.Inflate(objetivo, Separacion, Separacion);
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

        /// <summary>A la derecha del elemento si entra (la barra está a la izquierda); si no, abajo o arriba.</summary>
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

        /// <summary>
        /// Captura por partes con DrawToBitmap: sobre el formulario MDI completo dibuja el
        /// MdiClient gris en lugar del contenido (spike B2), y CopyFromScreen depende de que
        /// ninguna otra ventana tape la aplicación.
        /// </summary>
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
