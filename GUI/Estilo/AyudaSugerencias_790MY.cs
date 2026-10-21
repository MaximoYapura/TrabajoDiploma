using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Service_08YS;

namespace GUI_08YS.Estilo
{
    /// <summary>
    /// Sugerencias del Chef (Etapa 5, Fase 6): recorre las pistas que devuelve la pantalla
    /// activa (IAyudaContextual_790MY), resalta cada control con un marco de 4 paneles opacos
    /// finos (TemaFogon.Oro) y muestra el globo al lado (C8: sin transparencia ni ventanas
    /// propias, sin oscurecer la pantalla). Los controles visuales los declara el Designer
    /// del formulario principal; esta clase solo los ubica y los muestra u oculta.
    /// </summary>
    public sealed class AyudaSugerencias_790MY
    {
        private const int Grosor = 3;      // ancho de cada lado del marco
        private const int Separacion = 3;  // entre el control y el marco
        private const int Distancia = 10;  // entre el marco y el globo

        private readonly Control _host;
        private readonly Control _zona;    // área donde puede quedar el globo (contenido)
        private readonly GloboAyuda_790MY _globo;
        private readonly Control[] _marco;

        private readonly List<KeyValuePair<PistaAyuda_790MY, Control>> _pistas = new List<KeyValuePair<PistaAyuda_790MY, Control>>();
        private int _indice;
        private Control _focoPrevio;

        public AyudaSugerencias_790MY(Control host, Control zona, GloboAyuda_790MY globo,
                                      Control arriba, Control abajo, Control izquierda, Control derecha)
        {
            _host = host;
            _zona = zona;
            _globo = globo;
            _marco = new[] { arriba, abajo, izquierda, derecha };
            _globo.AnteriorSolicitado += (s, e) => Ir(_indice - 1);
            _globo.SiguienteSolicitado += (s, e) => { if (_indice < _pistas.Count - 1) Ir(_indice + 1); else Cerrar(); };
            _globo.CerrarSolicitado += (s, e) => Cerrar();
        }

        public bool Activas { get; private set; }

        /// <summary>
        /// Resuelve cada pista con Controls.Find sobre el hijo activo (si el control no existe
        /// o no está visible, la omite). Devuelve false si no quedó ninguna.
        /// </summary>
        public bool Mostrar(Control hijo, IReadOnlyList<PistaAyuda_790MY> pistas, Control focoPrevio)
        {
            Cerrar();
            if (hijo == null || pistas == null) return false;
            foreach (var p in pistas)
            {
                Control c = hijo.Controls.Find(p.NombreControl, true).FirstOrDefault();
                if (c != null && c.Visible && c.Width > 0 && c.Height > 0)
                    _pistas.Add(new KeyValuePair<PistaAyuda_790MY, Control>(p, c));
            }
            if (_pistas.Count == 0) return false;

            _focoPrevio = focoPrevio;
            Activas = true;
            Ir(0);
            return true;
        }

        public void Cerrar()
        {
            bool estaban = Activas;
            Activas = false;
            _pistas.Clear();
            foreach (var lado in _marco) lado.Visible = false;
            bool teniaFoco = _globo.ContainsFocus;
            _globo.Visible = false;
            if (estaban && teniaFoco && _focoPrevio != null && !_focoPrevio.IsDisposed && _focoPrevio.CanFocus)
                _focoPrevio.Focus();
            _focoPrevio = null;
        }

        private static string T(string clave) => TraductorManager_08YS.Instance.GetTexto(clave);

        private void Ir(int indice)
        {
            if (!Activas || indice < 0 || indice >= _pistas.Count) return;
            _indice = indice;
            var pista = _pistas[indice].Key;
            var control = _pistas[indice].Value;

            // Si el control está dentro de un contenedor con desplazamiento, traerlo a la vista.
            for (Control p = control.Parent; p != null && p != _host; p = p.Parent)
                if (p is ScrollableControl sc && sc.AutoScroll) { sc.ScrollControlIntoView(control); break; }

            Rectangle zona = _host.RectangleToClient(_zona.RectangleToScreen(_zona.ClientRectangle));
            Rectangle objetivo = Rectangle.Intersect(_host.RectangleToClient(control.RectangleToScreen(control.ClientRectangle)), zona);
            if (objetivo.IsEmpty) objetivo = _host.RectangleToClient(control.RectangleToScreen(control.ClientRectangle));
            UbicarMarco(Rectangle.Inflate(objetivo, Separacion, Separacion));

            string probaCon = string.IsNullOrEmpty(pista.ClaveProbaCon) ? null : T(pista.ClaveProbaCon);
            _globo.Mostrar(T(pista.ClaveTitulo), T(pista.ClaveTexto), probaCon, indice, _pistas.Count,
                           string.Format(T("AY_sug_contador"), indice + 1, _pistas.Count),
                           T("AY_sug_anterior"), T("AY_sug_siguiente"), T("AY_sug_terminar"), T("AY_sug_cerrar"));
            _globo.Visible = true;
            _globo.Size = _globo.PreferredSize;
            _globo.Location = UbicacionGlobo(Rectangle.Inflate(objetivo, Separacion + Grosor, Separacion + Grosor), zona, _globo.Size);
            foreach (var lado in _marco) lado.BringToFront();
            _globo.BringToFront();
            _globo.EnfocarSiguiente();
        }

        private void UbicarMarco(Rectangle r)
        {
            _marco[0].Bounds = new Rectangle(r.Left - Grosor, r.Top - Grosor, r.Width + 2 * Grosor, Grosor);
            _marco[1].Bounds = new Rectangle(r.Left - Grosor, r.Bottom, r.Width + 2 * Grosor, Grosor);
            _marco[2].Bounds = new Rectangle(r.Left - Grosor, r.Top, Grosor, r.Height);
            _marco[3].Bounds = new Rectangle(r.Right, r.Top, Grosor, r.Height);
            foreach (var lado in _marco) lado.Visible = true;
        }

        /// <summary>Debajo del control si entra; si no, arriba; si no, a la derecha o a la izquierda.</summary>
        private static Point UbicacionGlobo(Rectangle marco, Rectangle zona, Size globo)
        {
            int x = Math.Max(zona.Left + Distancia, Math.Min(marco.Left, zona.Right - globo.Width - Distancia));
            if (marco.Bottom + Distancia + globo.Height <= zona.Bottom)
                return new Point(x, marco.Bottom + Distancia);
            if (marco.Top - Distancia - globo.Height >= zona.Top)
                return new Point(x, marco.Top - Distancia - globo.Height);

            int y = Math.Max(zona.Top + Distancia, Math.Min(marco.Top, zona.Bottom - globo.Height - Distancia));
            if (marco.Right + Distancia + globo.Width <= zona.Right)
                return new Point(marco.Right + Distancia, y);
            return new Point(Math.Max(zona.Left + Distancia, marco.Left - Distancia - globo.Width), y);
        }
    }
}
