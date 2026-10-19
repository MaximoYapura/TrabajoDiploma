using BE_08YS;
using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using E = GUI_08YS.RF2.EstiloRF2_790MY;

namespace GUI_08YS.RF2
{
    /// <summary>Fuentes compartidas por todos los tickets del KDS (no se crea una por ticket).</summary>
    internal sealed class FuentesKds_790MY : IDisposable
    {
        public readonly Font Pedido  = new Font("Segoe UI", 15F, FontStyle.Bold);
        public readonly Font Mesa    = new Font("Segoe UI", 11.25F, FontStyle.Bold);
        public readonly Font Detalle = new Font("Segoe UI", 9.75F);
        public readonly Font Item    = new Font("Segoe UI", 12.5F);
        public readonly Font Cant    = new Font("Segoe UI", 12.5F, FontStyle.Bold);
        public readonly Font Chip    = new Font("Segoe UI", 9.75F, FontStyle.Bold);
        public readonly Font Boton   = new Font("Segoe UI", 12F, FontStyle.Bold);

        public void Dispose()
        {
            foreach (var f in new[] { Pedido, Mesa, Detalle, Item, Cant, Chip, Boton })
                f.Dispose();
        }
    }

    /// <summary>
    /// Etapa 3 (Opción C): un ticket del KDS que vive mientras el pedido esté en pantalla.
    /// <see cref="Aplicar"/> lo sincroniza con el pedido recién consultado:
    /// <list type="bullet">
    /// <item>Pendiente ↔ En preparación: cambia texto, ícono y colores del botón y de las cantidades.</item>
    /// <item>Cambio de contenido (ítems, mesa, ancho, paso a Listo): rearma solo los hijos de este ticket.</item>
    /// <item>Siempre: minutos de espera y semáforo, solo si cambió el minuto.</item>
    /// </list>
    /// El ticket nunca se destruye por un cambio de estado; el formulario lo mueve de columna.
    /// </summary>
    internal sealed class TicketKds_790MY : PanelRedondeado_790MY
    {
        /// <summary>
        /// Semáforo por tiempo de espera desde la solicitud (comandas Pendiente / EnPreparación):
        /// menos de 10 min = verde (a tiempo), 10 a 20 min = amarillo (atención),
        /// más de 20 min = rojo (demorado).
        /// </summary>
        internal enum NivelSemaforo { ATiempo, Atencion, Demorado }

        private const int MinutosAtencion = 10;
        private const int MinutosDemora = 20;

        private static readonly Color SemVerde       = Color.FromArgb(120, 200, 130);
        private static readonly Color SemVerdeFondo  = Color.FromArgb(32, 58, 40);
        private static readonly Color SemAmbar       = Color.FromArgb(244, 185, 66);
        private static readonly Color SemAmbarFondo  = Color.FromArgb(64, 52, 28);
        private static readonly Color SemRojo        = Color.FromArgb(240, 110, 100);
        private static readonly Color SemRojoFondo   = Color.FromArgb(72, 30, 30);

        private readonly FuentesKds_790MY _fuentes;
        private readonly Func<int?, string> _nombreCamarero;
        private readonly bool _puedeDespachar;
        private readonly Action<Pedido_790MY> _iniciar;
        private readonly Action<Pedido_790MY> _marcarListo;

        // Lo que se dibujó: si cambia, se rearman los hijos.
        private string _estructura;
        private EstadoPedido_790MY _estado;
        private int _minutos;
        private int _ancho;

        // Hijos que cambian sin rearmar.
        private EtiquetaChip_790MY _chipTiempo;
        private IconButton _boton;
        private readonly List<Label> _cantidades = new List<Label>();

        public TicketKds_790MY(FuentesKds_790MY fuentes, Func<int?, string> nombreCamarero, bool puedeDespachar,
                               Action<Pedido_790MY> iniciar, Action<Pedido_790MY> marcarListo)
        {
            _fuentes = fuentes;
            _nombreCamarero = nombreCamarero;
            _puedeDespachar = puedeDespachar;
            _iniciar = iniciar;
            _marcarListo = marcarListo;
            Margin = new Padding(0, 0, 0, 14);
            Radio = 12;
        }

        public Pedido_790MY Pedido { get; private set; }

        public void Aplicar(Pedido_790MY pedido, int ancho)
        {
            Pedido = pedido;
            string estructura = Estructura(pedido, ancho);
            if (estructura != _estructura)
            {
                _estructura = estructura;
                Rearmar(ancho);
                return;
            }
            if (pedido.Estado != _estado)
                AplicarEstado();
            ActualizarTiempo();
        }

        /// <summary>Obliga a rearmar en el próximo <see cref="Aplicar"/> (por ejemplo, al cambiar el idioma).</summary>
        public void Invalidar() => _estructura = null;

        /// <summary>
        /// Todo lo que define qué hijos tiene el ticket. Pendiente y En preparación
        /// comparten estructura (solo cambian el botón y los colores); Listo no.
        /// </summary>
        private static string Estructura(Pedido_790MY p, int ancho)
        {
            var sb = new System.Text.StringBuilder()
                .Append(ancho).Append('|').Append(p.Estado == EstadoPedido_790MY.Listo)
                .Append('|').Append(p.MesaNumero).Append('|').Append(p.CamareroDNI)
                .Append('|').Append(p.Fecha.Ticks).Append('|').Append(p.Hora.Ticks).Append('|').Append(p.HoraFin?.Ticks);
            foreach (var i in p.Items)
                sb.Append(';').Append(i.NroItem).Append(',').Append(i.Cantidad)
                  .Append(',').Append(i.ProductoNombre).Append(',').Append(i.Observaciones);
            return sb.ToString();
        }

        // ─── Armado ──────────────────────────────────────────────────────────

        private void Rearmar(int ancho)
        {
            SuspendLayout();
            foreach (Control c in Controls.Cast<Control>().ToList())
                c.Dispose();
            _cantidades.Clear();
            _chipTiempo = null;
            _boton = null;

            var pedido = Pedido;
            bool listo = pedido.Estado == EstadoPedido_790MY.Listo;
            _ancho = ancho;
            _estado = pedido.Estado;
            _minutos = MinutosEspera(pedido);
            NivelSemaforo nivel = Nivel(_minutos);

            // Las comandas en curso llevan el borde del color del semáforo.
            Width = ancho;
            BackColor = listo ? Color.FromArgb(38, 56, 44) : E.KdsTarjeta;
            ColorBorde = listo ? Color.FromArgb(70, 120, 85) : ColorSemaforo(nivel);
            GrosorBorde = listo ? 1f : 2.5f;

            int x = 18, y = 14, interior = ancho - 36;

            // Encabezado: número de pedido + mesa
            var lblPedido = E.CrearLabel(E.T("KDS_pedido_nro", pedido.NroPedido), _fuentes.Pedido, E.KdsTexto);
            lblPedido.Location = new Point(x, y);
            var chipMesa = E.CrearChip(E.T("KDS_mesa", pedido.MesaNumero).ToUpper(), E.KdsTexto, E.KdsFondo, _fuentes.Mesa);
            chipMesa.Padding = new Padding(10, 4, 10, 4);
            Controls.Add(lblPedido);
            Controls.Add(chipMesa);
            chipMesa.Location = new Point(ancho - chipMesa.PreferredWidth - 18, y + 2);
            y += 40;

            if (!listo)
            {
                // Camarero, hora de solicitud y tiempo transcurrido
                var lblDetalle = E.CrearLabel(E.T("KDS_solicitado", _nombreCamarero(pedido.CamareroDNI), E.Hora(pedido.Hora)),
                                              _fuentes.Detalle, E.KdsGris);
                lblDetalle.Location = new Point(x, y + 4);
                Controls.Add(lblDetalle);

                _chipTiempo = E.CrearChip(TextoSemaforo(nivel, _minutos), FondoSemaforo(nivel),
                                          ColorSemaforo(nivel), _fuentes.Chip, ColorSemaforo(nivel));
                Controls.Add(_chipTiempo);
                _chipTiempo.Location = new Point(ancho - _chipTiempo.PreferredWidth - 18, y);
                y += 38;
            }

            // Ítems con sus observaciones
            foreach (var item in pedido.Items.OrderBy(i => i.NroItem))
            {
                var lblCant = E.CrearLabel(item.Cantidad + "×", _fuentes.Cant, listo ? E.KdsVerde : ColorAcento(pedido.Estado));
                lblCant.Location = new Point(x, y);
                var lblItem = E.CrearLabel(item.ProductoNombre, _fuentes.Item, E.KdsTexto, autoSize: false);
                lblItem.Location = new Point(x + 46, y);
                lblItem.Size = new Size(interior - 46, 26);
                Controls.Add(lblCant);
                Controls.Add(lblItem);
                _cantidades.Add(lblCant);
                y += 30;

                if (!string.IsNullOrWhiteSpace(item.Observaciones))
                {
                    var chipObs = E.CrearChip("⚠ " + item.Observaciones, Color.FromArgb(70, 56, 30), E.KdsAmbar, _fuentes.Chip);
                    chipObs.MaximumSize = new Size(interior - 46, 0);
                    chipObs.Location = new Point(x + 46, y);
                    Controls.Add(chipObs);
                    y += chipObs.PreferredHeight + 6;
                }
            }
            y += 6;

            if (listo)
            {
                var barra = new Label
                {
                    Text = E.T("KDS_finalizado", pedido.HoraFin.HasValue ? E.Hora(pedido.HoraFin.Value) : "-",
                               _nombreCamarero(pedido.CamareroDNI)),
                    Font = _fuentes.Chip,
                    ForeColor = E.KdsVerde,
                    BackColor = Color.FromArgb(28, 72, 40),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(10, 0, 10, 0),
                    Location = new Point(x, y),
                    Size = new Size(interior, 34)
                };
                Controls.Add(barra);
                y += 42;

                string descontado = string.Join(" · ", pedido.Items
                    .GroupBy(i => i.ProductoNombre)
                    .Select(g => $"{g.Key} −{g.Sum(i => i.Cantidad)}"));
                var lblStock = E.CrearLabel(E.T("KDS_stock_descontado", descontado), _fuentes.Detalle, E.KdsGris, autoSize: false);
                lblStock.Location = new Point(x, y);
                lblStock.Size = new Size(interior, TextRenderer.MeasureText(lblStock.Text, _fuentes.Detalle,
                    new Size(interior, 0), TextFormatFlags.WordBreak).Height + 4);
                Controls.Add(lblStock);
                y += lblStock.Height + 10;
            }
            else
            {
                _boton = new IconButton
                {
                    IconColor = E.KdsFondo,
                    IconSize = 22,
                    TextImageRelation = TextImageRelation.ImageBeforeText,
                    ImageAlign = ContentAlignment.MiddleCenter,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = _fuentes.Boton,
                    ForeColor = E.KdsFondo,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Location = new Point(x, y),
                    Size = new Size(interior, 52),
                    Enabled = _puedeDespachar
                };
                _boton.FlatAppearance.BorderSize = 0;
                _boton.Click += Boton_Click;
                Controls.Add(_boton);
                PintarBoton();
                y += 66;
            }

            Height = y;
            ResumeLayout();
        }

        /// <summary>Pendiente ↔ En preparación sin rearmar: botón y color de las cantidades.</summary>
        private void AplicarEstado()
        {
            _estado = Pedido.Estado;
            foreach (var lbl in _cantidades)
                lbl.ForeColor = ColorAcento(_estado);
            PintarBoton();
            if (_boton != null) _boton.Enabled = _puedeDespachar;
        }

        private void PintarBoton()
        {
            if (_boton == null) return;
            bool pendiente = _estado == EstadoPedido_790MY.Pendiente;
            _boton.Text = pendiente ? E.T("KDS_btn_iniciar") : E.T("KDS_btn_listo");
            _boton.IconChar = pendiente ? IconChar.Play : IconChar.Check;
            _boton.BackColor = pendiente ? E.KdsAzul : E.KdsVerde;
        }

        // La acción recarga el KDS, que puede rearmar este ticket y destruir el botón: se
        // difiere para no disponerlo dentro de su propio Click, y se deshabilita para que
        // un doble toque no la dispare dos veces.
        private void Boton_Click(object sender, EventArgs e)
        {
            var boton = (IconButton)sender;
            boton.Enabled = false;
            var pedido = Pedido;
            bool pendiente = _estado == EstadoPedido_790MY.Pendiente;
            BeginInvoke((Action)(() =>
            {
                if (pendiente) _iniciar(pedido);
                else _marcarListo(pedido);
                // Si falló, los datos no cambiaron y el botón sigue en pantalla.
                if (!boton.IsDisposed) boton.Enabled = _puedeDespachar;
            }));
        }

        /// <summary>Minutos, texto y colores del semáforo, solo si cambió el minuto.</summary>
        public void ActualizarTiempo()
        {
            if (_chipTiempo == null) return; // los listos no muestran espera
            int minutos = MinutosEspera(Pedido);
            if (minutos == _minutos) return;

            NivelSemaforo anterior = Nivel(_minutos), nivel = Nivel(minutos);
            _minutos = minutos;

            _chipTiempo.Text = TextoSemaforo(nivel, minutos);
            if (nivel != anterior)
            {
                _chipTiempo.BackColor = FondoSemaforo(nivel);
                _chipTiempo.ForeColor = ColorSemaforo(nivel);
                _chipTiempo.ColorBorde = ColorSemaforo(nivel);
                ColorBorde = ColorSemaforo(nivel);
            }
            _chipTiempo.Location = new Point(_ancho - _chipTiempo.PreferredWidth - 18, _chipTiempo.Top);
        }

        // ─── Semáforo por tiempo ─────────────────────────────────────────────

        private static Color ColorAcento(EstadoPedido_790MY estado)
            => estado == EstadoPedido_790MY.Pendiente ? Color.FromArgb(240, 140, 120) : E.KdsAzul;

        internal static int MinutosEspera(Pedido_790MY pedido)
            => Math.Max(0, (int)(DateTime.Now - (pedido.Fecha.Date + pedido.Hora)).TotalMinutes);

        internal static NivelSemaforo Nivel(int minutos)
        {
            if (minutos < MinutosAtencion) return NivelSemaforo.ATiempo;
            if (minutos <= MinutosDemora) return NivelSemaforo.Atencion;
            return NivelSemaforo.Demorado;
        }

        private static Color ColorSemaforo(NivelSemaforo nivel)
        {
            switch (nivel)
            {
                case NivelSemaforo.ATiempo: return SemVerde;
                case NivelSemaforo.Atencion: return SemAmbar;
                default: return SemRojo;
            }
        }

        private static Color FondoSemaforo(NivelSemaforo nivel)
        {
            switch (nivel)
            {
                case NivelSemaforo.ATiempo: return SemVerdeFondo;
                case NivelSemaforo.Atencion: return SemAmbarFondo;
                default: return SemRojoFondo;
            }
        }

        private static string TextoSemaforo(NivelSemaforo nivel, int minutos)
        {
            switch (nivel)
            {
                case NivelSemaforo.ATiempo: return E.T("KDS_chip_a_tiempo", minutos);
                case NivelSemaforo.Atencion: return E.T("KDS_chip_atencion", minutos);
                default: return E.T("KDS_demorado", minutos);
            }
        }
    }
}
