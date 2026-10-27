using GUI_08YS.Properties;
using Service_08YS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using T = GUI_08YS.Estilo.TemaFogon_790MY;

namespace GUI_08YS.Estilo
{
    /// <summary>
    /// Maître digital: personaje con un globo de consejos que FormMDI_08YS muestra sobre el
    /// área de trabajo mientras no hay pantalla abierta. Los consejos salen de idiomas.json
    /// (MT_consejo_N_tipo / _titulo / _mensaje) y el tipo elige la pose. Sin rotación
    /// automática, sin sonido ni animaciones.
    /// </summary>
    public partial class AsistenteMaitre_UC_790MY : UserControl
    {
        private const int Cola = 12;              // ancho de la cola del globo, hacia el personaje
        private const int Radio = 14;
        private const float CentroCola = 24f;     // altura del vértice de la cola desde el borde superior
        private const float MitadBaseCola = 8f;
        private const int LineasMensaje = 4;
        private const string TipoBienvenida = "bienvenida";

        private sealed class Consejo
        {
            public string Tipo;
            public string Titulo;
            public string Mensaje;
        }

        // Resources crea un Bitmap nuevo en cada acceso: cada pose se carga una sola vez.
        private static readonly Dictionary<string, Image> _poses = new Dictionary<string, Image>();

        private List<Consejo> _consejos = new List<Consejo>();
        private int _indice;
        private Image _pose;
        private bool _compacto;

        public AsistenteMaitre_UC_790MY()
        {
            InitializeComponent();
        }

        /// <summary>Lo llama FormMDI_08YS.UpdateIdioma: recarga los consejos y conserva la posición.</summary>
        public void UpdateIdioma()
        {
            var tr = TraductorManager_08YS.Instance;
            CargarConsejos();
            lnkSiguiente_790MY.Text = tr.GetTexto("MT_siguiente");
            AccessibleName = tr.GetTexto("MT_accesible");
            MostrarConsejo();
        }

        /// <summary>
        /// Elige el modo según la marca de agua (<paramref name="logo"/>, en coordenadas del
        /// contenedor). Si el personaje completo la pisaría pasa a compacto; devuelve false si
        /// ni el compacto evita taparla, y entonces el asistente no debe mostrarse.
        /// </summary>
        public bool AjustarModo(Rectangle logo)
        {
            bool compacto = Pisa(picPersonaje_790MY, logo);
            if (compacto && (Pisa(picPersonajeCompacto_790MY, logo) || Pisa(pnlAviso_790MY, logo)))
                return false;

            if (compacto != _compacto || Region == null)
            {
                _compacto = compacto;
                picPersonaje_790MY.Visible = !compacto;
                pnlGlobo_790MY.Visible = !compacto;
                picPersonajeCompacto_790MY.Visible = compacto;
                pnlAviso_790MY.Visible = compacto;
                ActualizarRegion();
            }
            return true;
        }

        private bool Pisa(Control pieza, Rectangle logo)
            => new Rectangle(Left + pieza.Left, Top + pieza.Top, pieza.Width, pieza.Height).IntersectsWith(logo);

        #region Consejos

        private void CargarConsejos()
        {
            var tr = TraductorManager_08YS.Instance;
            string nombre = SessionManager_08YS.Instance.Current?.Nombre ?? string.Empty;
            var consejos = new List<Consejo>();
            for (int i = 1; Existe("MT_consejo_" + i + "_tipo"); i++)
            {
                string clave = "MT_consejo_" + i + "_";
                string tipo = tr.GetTexto(clave + "tipo").Trim().ToLowerInvariant();
                string titulo = tr.GetTexto(clave + "titulo");
                consejos.Add(new Consejo
                {
                    Tipo = tipo,
                    Titulo = tipo == TipoBienvenida ? string.Format(titulo, nombre) : titulo,
                    Mensaje = tr.GetTexto(clave + "mensaje"),
                });
            }
            _consejos = consejos;
            if (_indice >= _consejos.Count) _indice = 0;
        }

        // GetTexto devuelve «[clave]» cuando la clave no existe en el idioma activo.
        private static bool Existe(string clave)
            => TraductorManager_08YS.Instance.GetTexto(clave) != "[" + clave + "]";

        private void MostrarConsejo()
        {
            if (_consejos.Count == 0) return;
            var consejo = _consejos[_indice];

            _pose = Pose(consejo.Tipo);
            lblTitulo_790MY.Text = consejo.Titulo;
            lblMensaje_790MY.Text = RecortarALineas(consejo.Mensaje, LineasMensaje);
            lblAviso_790MY.Text = consejo.Titulo + " — " + consejo.Mensaje;
            AccessibleDescription = consejo.Titulo + ". " + consejo.Mensaje;

            picPersonaje_790MY.Invalidate();
            picPersonajeCompacto_790MY.Invalidate();
        }

        private void Siguiente()
        {
            if (_consejos.Count == 0) return;
            _indice = (_indice + 1) % _consejos.Count;
            MostrarConsejo();
        }

        private static Image Pose(string tipo)
        {
            if (_poses.TryGetValue(tipo, out var pose)) return pose;
            switch (tipo)
            {
                case TipoBienvenida: pose = Resources.Maitre01Bienvenida;  break;
                case "indicacion":   pose = Resources.Maitre03Indicacion;  break;
                case "alerta":       pose = Resources.Maitre04Alerta;      break;
                case "celebracion":  pose = Resources.Maitre05Celebracion; break;
                default:             pose = Resources.Maitre02Pensando;    break; // "consejo"
            }
            _poses[tipo] = pose;
            return pose;
        }

        /// <summary>Corta el mensaje por palabras y agrega «…» si no entra en el máximo de líneas.</summary>
        private string RecortarALineas(string texto, int lineas)
        {
            var fuente = lblMensaje_790MY.Font;
            int ancho = lblMensaje_790MY.MaximumSize.Width;
            int alto = fuente.Height * lineas;
            const TextFormatFlags formato = TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl;

            bool Entra(string t) => TextRenderer.MeasureText(t, fuente, new Size(ancho, int.MaxValue), formato).Height <= alto;

            if (string.IsNullOrEmpty(texto) || Entra(texto)) return texto;
            var palabras = texto.Split(' ');
            for (int n = palabras.Length - 1; n > 0; n--)
            {
                string candidato = string.Join(" ", palabras, 0, n).TrimEnd(',', ';', ':', '.') + "…";
                if (Entra(candidato)) return candidato;
            }
            return "…";
        }

        private void lnkSiguiente_790MY_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Siguiente();

        private void Aviso_Click(object sender, EventArgs e) => Siguiente();

        #endregion

        #region Dibujo

        /// <summary>
        /// Escala Zoom apoyada en el borde inferior: las poses vienen recortadas al ras con el
        /// torso cortado abajo, y el Zoom nativo las centra (la pose 01 quedaría flotando).
        /// </summary>
        private void Personaje_Paint(object sender, PaintEventArgs e)
        {
            if (_pose == null) return;
            var caja = ((Control)sender).ClientRectangle;
            float escala = Math.Min((float)caja.Width / _pose.Width, (float)caja.Height / _pose.Height);
            int ancho = (int)Math.Round(_pose.Width * escala);
            int alto = (int)Math.Round(_pose.Height * escala);
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.DrawImage(_pose, new Rectangle(caja.X + (caja.Width - ancho) / 2, caja.Bottom - alto, ancho, alto));
        }

        /// <summary>Globo y aviso compacto: cuerpo redondeado + cola a la derecha, sin sombra.</summary>
        private void Globo_Paint(object sender, PaintEventArgs e)
        {
            var panel = (Control)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var forma = CrearGlobo(new RectangleF(0, 0, panel.Width, panel.Height), 0.5f))
            using (var fondo = new SolidBrush(T.MaitreGlobo))
            using (var borde = new Pen(T.Oro, 1f))
            {
                e.Graphics.FillPath(fondo, forma);
                e.Graphics.DrawPath(borde, forma);
            }
        }

        /// <param name="caja">Rectángulo del panel (cuerpo + cola).</param>
        /// <param name="margen">0.5 para dibujar el borde de 1 px nítido; 0 para la Region.</param>
        private static GraphicsPath CrearGlobo(RectangleF caja, float margen)
        {
            float izq = caja.Left + margen, arriba = caja.Top + margen;
            float der = caja.Right - Cola - margen, abajo = caja.Bottom - margen;
            float punta = caja.Right - margen;
            float d = Radio * 2;
            float centro = caja.Top + Math.Min(CentroCola, caja.Height / 2f);
            float mitad = Math.Min(MitadBaseCola, caja.Height / 2f - Radio - margen);

            var forma = new GraphicsPath();
            forma.AddArc(izq, arriba, d, d, 180, 90);
            forma.AddArc(der - d, arriba, d, d, 270, 90);
            forma.AddLine(der, centro - mitad, punta, centro);
            forma.AddLine(punta, centro, der, centro + mitad);
            forma.AddArc(der - d, abajo - d, d, d, 0, 90);
            forma.AddArc(izq, abajo - d, d, d, 90, 90);
            forma.CloseFigure();
            return forma;
        }

        /// <summary>
        /// El fondo crema es opaco: la Region deja visible solo personaje ∪ globo (forma «en L»)
        /// para que el resto del rectángulo no tape la marca de agua de panel2.
        /// </summary>
        private void ActualizarRegion()
        {
            var personaje = _compacto ? picPersonajeCompacto_790MY : picPersonaje_790MY;
            var globo = _compacto ? pnlAviso_790MY : pnlGlobo_790MY;
            using (var forma = new GraphicsPath(FillMode.Winding))
            {
                forma.AddRectangle(personaje.Bounds);
                using (var contorno = CrearGlobo(globo.Bounds, 0f))
                    forma.AddPath(contorno, false);
                var anterior = Region;
                Region = new Region(forma);
                anterior?.Dispose();
            }
        }

        // El globo cambia de alto con cada consejo (72 a 180 px): la Region lo acompaña.
        private void pnlGlobo_790MY_SizeChanged(object sender, EventArgs e)
        {
            if (!_compacto && Region != null) ActualizarRegion();
        }

        #endregion
    }
}
