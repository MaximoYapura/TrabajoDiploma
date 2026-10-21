using BLL_08YS;
using GUI_08YS.Maestros;
using GUI_08YS.RF2;
using Service_08YS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using GUI_08YS.Estilo;
using E = GUI_08YS.RF2.EstiloRF2_790MY;

namespace GUI_08YS.Reportes
{
    /// <summary>
    /// Reporte inteligente: Análisis Predictivo de Canasta. Muestra las reglas de
    /// asociación entre productos (soporte y confianza, calculados por
    /// ReportePredictivoBLL_790MY sobre las cuentas cobradas reales de los últimos 90 días),
    /// filtradas por confianza mínima, y tarjetas de sugerencias de negocio con
    /// acciones simuladas. Si la BD todavía no tiene suficientes cobros, lo informa en
    /// lugar de mostrar reglas. Patente: GestionReportePredictivo.
    /// </summary>
    public partial class FormReportePredictivo_790MY : Form, IIdiomaObserver_08YS, IAyudaContextual_790MY
    {
        // Paleta oficial de la casa.
        private static readonly Color Bordo = Color.FromArgb(107, 29, 47);       // #6B1D2F
        private static readonly Color BordoHover = Color.FromArgb(86, 22, 37);
        private static readonly Color BordoSuave = Color.FromArgb(248, 236, 238);
        private static readonly Color Crema = Color.FromArgb(253, 251, 247);     // #FDFBF7
        private static readonly Color CremaTarjeta = Color.FromArgb(247, 241, 232);
        private static readonly Color CremaFila = Color.FromArgb(250, 246, 240);
        private static readonly Color Dorado = Color.FromArgb(139, 106, 58);
        private static readonly Color PistaBarra = Color.FromArgb(240, 232, 220);

        private readonly ReportePredictivoBLL_790MY _bll;
        private OrdenReglas_790MY _orden = OrdenReglas_790MY.PorConfianza;
        private List<ReglaAsociacion_790MY> _reglas = new List<ReglaAsociacion_790MY>();
        private List<SugerenciaInteligente_790MY> _sugerencias = new List<SugerenciaInteligente_790MY>();
        private ResumenCanasta_790MY _resumen;
        private decimal _soporteMaximo;
        private bool _listo;

        private readonly Font _fNumeroBarra = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        private readonly Font _fCardEncabezado = new Font("Segoe UI", 8.25F, FontStyle.Bold);
        private readonly Font _fCardTitulo = new Font("Segoe UI", 12.75F, FontStyle.Bold);
        private readonly Font _fCardTexto = new Font("Segoe UI", 9.25F);
        private readonly Font _fCardBoton = new Font("Segoe UI", 9.75F, FontStyle.Bold);

        public FormReportePredictivo_790MY()
        {
            InitializeComponent();
            _bll = BLLFactory_790MY.CreateReportePredictivoBLL();

            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) =>
            {
                TraductorManager_08YS.Instance.Desuscribir(this);
                foreach (var f in new[] { _fNumeroBarra, _fCardEncabezado, _fCardTitulo, _fCardTexto, _fCardBoton })
                    f.Dispose();
            };
        }

        private IWin32Window VentanaPropietaria => (this.TopLevelControl as Form) ?? (IWin32Window)this;

        /// <summary>La BD todavía no tiene suficientes cuentas cobradas para calcular asociaciones.</summary>
        private bool DatosInsuficientes => _resumen != null && !_resumen.DatosSuficientes;

        private static CultureInfo Cultura => TraductorManager_08YS.Instance.CulturaActual ?? CultureInfo.CurrentCulture;

        private void FormReportePredictivo_790MY_Load(object sender, EventArgs e)
        {
            EstiloMaestros_790MY.EstilizarGrilla(dgvReglas_790MY);
            dgvReglas_790MY.ColumnHeadersDefaultCellStyle.BackColor = Bordo;
            dgvReglas_790MY.ColumnHeadersDefaultCellStyle.SelectionBackColor = Bordo;
            dgvReglas_790MY.AlternatingRowsDefaultCellStyle.BackColor = CremaFila;
            dgvReglas_790MY.DefaultCellStyle.SelectionBackColor = BordoSuave;
            dgvReglas_790MY.RowTemplate.Height = 40;
            dgvReglas_790MY.ColumnHeadersHeight = 38;
            colNumero_790MY.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);

            _listo = true;
            AplicarEstiloOrden();
            UpdateIdioma();
            Cargar();
        }

        #region Datos

        private void Cargar()
        {
            try
            {
                _resumen = _bll.ObtenerResumen();
                _reglas = _bll.ObtenerReglasAsociacion(sldConfianza_790MY.Value, _orden);
                _sugerencias = _bll.ObtenerSugerenciasInteligentes();
            }
            catch (Exception ex)
            {
                _resumen = null;
                _reglas = new List<ReglaAsociacion_790MY>();
                _sugerencias = new List<SugerenciaInteligente_790MY>();
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
            }
            Mostrar();
            MostrarSugerencias();
        }

        /// <summary>Solo recalcula la grilla (filtro u orden): las sugerencias no dependen de ellos.</summary>
        private void RecargarReglas()
        {
            try
            {
                _reglas = _bll.ObtenerReglasAsociacion(sldConfianza_790MY.Value, _orden);
            }
            catch (Exception ex)
            {
                _reglas = new List<ReglaAsociacion_790MY>();
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
            }
            Mostrar();
        }

        private void Mostrar()
        {
            lblConfianza_790MY.Text = E.T("RI_confianza_minima", sldConfianza_790MY.Value);
            lblSubtitulo_790MY.Text = _resumen == null
                ? string.Empty
                : E.T("RI_subtitulo", _resumen.DiasAnalizados, _resumen.CuentasAnalizadas.ToString("N0", Cultura));

            _soporteMaximo = 0;
            foreach (var r in _reglas)
                if (r.Soporte > _soporteMaximo) _soporteMaximo = r.Soporte;

            dgvReglas_790MY.Rows.Clear();
            foreach (var r in _reglas)
            {
                int fila = dgvReglas_790MY.Rows.Add(
                    r.Numero.ToString(),
                    E.T("RI_combinacion", r.ProductoA, r.ProductoB),
                    Porcentaje(r.Soporte),
                    Porcentaje(r.Confianza));
                dgvReglas_790MY.Rows[fila].Tag = r;
                dgvReglas_790MY.Rows[fila].Cells[colCombinacion_790MY.Index].ToolTipText =
                    E.T("RI_tooltip_regla", r.ProductoA, r.ProductoB, r.CuentasConAyB, r.CuentasConA,
                        r.Lift.ToString("0.00", Cultura));
            }
            dgvReglas_790MY.ClearSelection();

            lblSinReglas_790MY.Text = DatosInsuficientes
                ? E.T("RI_datos_insuficientes")
                : E.T("RI_sin_reglas", sldConfianza_790MY.Value);
            lblSinReglas_790MY.Visible = _reglas.Count == 0;
            UbicarMensajeVacio();
        }

        private void sldConfianza_790MY_ValueChanged(object sender, EventArgs e)
        {
            if (_listo) RecargarReglas();
        }

        private void btnPorConfianza_790MY_Click(object sender, EventArgs e) => CambiarOrden(OrdenReglas_790MY.PorConfianza);

        private void btnPorSoporte_790MY_Click(object sender, EventArgs e) => CambiarOrden(OrdenReglas_790MY.PorSoporte);

        private void CambiarOrden(OrdenReglas_790MY orden)
        {
            if (_orden == orden) return;
            _orden = orden;
            AplicarEstiloOrden();
            RecargarReglas();
        }

        #endregion

        #region Grilla

        private void dgvReglas_790MY_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != colSoporte_790MY.Index && e.ColumnIndex != colConfianza_790MY.Index) return;
            if (!(dgvReglas_790MY.Rows[e.RowIndex].Tag is ReglaAsociacion_790MY regla)) return;

            bool esSoporte = e.ColumnIndex == colSoporte_790MY.Index;
            // Soporte relativo al mayor de la lista (para que se lea la comparación);
            // confianza sobre 100 %.
            decimal valor = esSoporte ? regla.Soporte : regla.Confianza;
            decimal escala = esSoporte ? Math.Max(1m, _soporteMaximo * 1.15m) : 100m;
            float proporcion = (float)Math.Min(1m, valor / escala);

            e.PaintBackground(e.CellBounds, true);
            Graphics g = e.Graphics;
            var anterior = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            const int anchoTexto = 78;
            Rectangle celda = e.CellBounds;
            var pista = new RectangleF(celda.X + 10, celda.Y + celda.Height / 2f - 4, Math.Max(10, celda.Width - anchoTexto - 22), 8);
            using (var ruta = PanelRedondeado_790MY.CrearRuta(pista, 4))
            using (var pincel = new SolidBrush(PistaBarra))
                g.FillPath(pincel, ruta);
            if (proporcion > 0)
            {
                var barra = new RectangleF(pista.X, pista.Y, Math.Max(8, pista.Width * proporcion), pista.Height);
                using (var ruta = PanelRedondeado_790MY.CrearRuta(barra, 4))
                using (var pincel = new SolidBrush(esSoporte ? Dorado : Bordo))
                    g.FillPath(pincel, ruta);
            }
            g.SmoothingMode = anterior;

            var areaTexto = new Rectangle(celda.Right - anchoTexto - 8, celda.Y, anchoTexto, celda.Height);
            TextRenderer.DrawText(g, e.FormattedValue?.ToString(), _fNumeroBarra, areaTexto, E.Texto,
                TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            e.Handled = true;
        }

        private void dgvReglas_790MY_Resize(object sender, EventArgs e) => UbicarMensajeVacio();

        private void UbicarMensajeVacio()
        {
            int top = dgvReglas_790MY.ColumnHeadersHeight;
            lblSinReglas_790MY.Bounds = new Rectangle(0, top, dgvReglas_790MY.ClientSize.Width,
                                                      Math.Max(0, dgvReglas_790MY.ClientSize.Height - top));
        }

        private void AplicarEstiloOrden()
        {
            EstilizarToggle(btnPorConfianza_790MY, _orden == OrdenReglas_790MY.PorConfianza);
            EstilizarToggle(btnPorSoporte_790MY, _orden == OrdenReglas_790MY.PorSoporte);
        }

        private static void EstilizarToggle(Button boton, bool activo)
        {
            boton.BackColor = activo ? Bordo : Color.White;
            boton.ForeColor = activo ? Color.White : Bordo;
            boton.FlatAppearance.MouseOverBackColor = activo ? BordoHover : BordoSuave;
            boton.FlatAppearance.MouseDownBackColor = activo ? BordoHover : BordoSuave;
        }

        #endregion

        #region Sugerencias inteligentes

        private void MostrarSugerencias()
        {
            tlpSugerencias_790MY.SuspendLayout();
            foreach (Control viejo in new List<Control>(EnumerarControles(tlpSugerencias_790MY)))
            {
                tlpSugerencias_790MY.Controls.Remove(viejo);
                viejo.Dispose();
            }

            if (_sugerencias.Count == 0)
            {
                string mensaje = DatosInsuficientes ? E.T("RI_datos_insuficientes") : E.T("RI_sin_sugerencias");
                var vacio = E.CrearLabel(mensaje, _fCardTexto, E.Gris, autoSize: false);
                vacio.Dock = DockStyle.Fill;
                vacio.TextAlign = ContentAlignment.MiddleLeft;
                tlpSugerencias_790MY.Controls.Add(vacio, 0, 0);
                tlpSugerencias_790MY.SetColumnSpan(vacio, 3);
            }

            for (int i = 0; i < _sugerencias.Count && i < 3; i++)
                tlpSugerencias_790MY.Controls.Add(CrearTarjeta(_sugerencias[i], i), i, 0);

            tlpSugerencias_790MY.ResumeLayout();
        }

        private static IEnumerable<Control> EnumerarControles(Control padre)
        {
            foreach (Control c in padre.Controls) yield return c;
        }

        private Control CrearTarjeta(SugerenciaInteligente_790MY sugerencia, int columna)
        {
            TextoSugerencia texto = Redactar(sugerencia);

            var card = new PanelRedondeado_790MY
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(columna == 0 ? 0 : 8, 0, columna == 2 ? 0 : 8, 0),
                BackColor = CremaTarjeta,
                ColorBorde = E.BordeCard,
                Radio = 10,
                Padding = new Padding(16, 12, 16, 14)
            };

            var contenido = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                ColumnCount = 1,
                RowCount = 4,
                Margin = Padding.Empty
            };
            contenido.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            contenido.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            contenido.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            contenido.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            contenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));

            var encabezado = E.CrearLabel(texto.Encabezado, _fCardEncabezado, Bordo);
            encabezado.AutoEllipsis = true;
            var titulo = E.CrearLabel(texto.Titulo, _fCardTitulo, E.Texto);
            titulo.Margin = new Padding(0, 2, 0, 4);
            var descripcion = E.CrearLabel(texto.Descripcion, _fCardTexto, E.Gris, autoSize: false);
            descripcion.Dock = DockStyle.Fill;

            var boton = new Button
            {
                Text = texto.Boton,
                Dock = DockStyle.Fill,
                Font = _fCardBoton,
                BackColor = Bordo,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 6, 0, 0),
                UseVisualStyleBackColor = false,
                Tag = sugerencia
            };
            boton.FlatAppearance.BorderSize = 0;
            boton.FlatAppearance.MouseOverBackColor = BordoHover;
            boton.FlatAppearance.MouseDownBackColor = BordoHover;
            boton.Click += BotonSugerencia_Click;

            contenido.Controls.Add(encabezado, 0, 0);
            contenido.Controls.Add(titulo, 0, 1);
            contenido.Controls.Add(descripcion, 0, 2);
            contenido.Controls.Add(boton, 0, 3);
            card.Controls.Add(contenido);
            return card;
        }

        private sealed class TextoSugerencia
        {
            public string Encabezado, Titulo, Descripcion, Boton, Confirmacion;
        }

        /// <summary>Arma los textos de la tarjeta en el idioma activo a partir de los datos de la BLL.</summary>
        private TextoSugerencia Redactar(SugerenciaInteligente_790MY s)
        {
            ReglaAsociacion_790MY r = s.Regla;
            string a = r.ProductoA, b = r.ProductoB;
            string confianza = Porcentaje(r.Confianza), soporte = Porcentaje(r.Soporte);

            switch (s.Tipo)
            {
                case TipoSugerencia_790MY.CrearCombo:
                    int deCadaDiez = (int)Math.Round(r.Confianza / 10m, MidpointRounding.AwayFromZero);
                    return new TextoSugerencia
                    {
                        Encabezado = E.T("RI_sug_encabezado_confianza", a, b, confianza),
                        Titulo = E.T("RI_sug_combo_titulo"),
                        Descripcion = E.T("RI_sug_combo_desc", NumeroEnPalabras(deCadaDiez), a, b),
                        Boton = E.T("RI_sug_combo_btn"),
                        Confirmacion = E.T("RI_msg_combo_ok", a, b)
                    };

                case TipoSugerencia_790MY.PromocionDiaBajaDemanda:
                    DayOfWeek dia = s.DiaMenorDemanda ?? DayOfWeek.Tuesday;
                    string dias = DiaEnPlural(dia);
                    return new TextoSugerencia
                    {
                        Encabezado = E.T("RI_sug_encabezado_confianza", a, b, confianza),
                        Titulo = E.T("RI_sug_promo_titulo", dias),
                        Descripcion = E.T("RI_sug_promo_desc", dias, a, b),
                        Boton = E.T("RI_sug_promo_btn", dias),
                        Confirmacion = E.T("RI_msg_promo_ok", a, b, dias)
                    };

                default:
                    return new TextoSugerencia
                    {
                        Encabezado = E.T("RI_sug_encabezado_soporte", a, b, soporte),
                        Titulo = E.T("RI_sug_cruzada_titulo"),
                        Descripcion = E.T("RI_sug_cruzada_desc", a, b),
                        Boton = E.T("RI_sug_cruzada_btn"),
                        Confirmacion = E.T("RI_msg_cruzada_ok", a, b)
                    };
            }
        }

        /// <summary>Acción simulada: no modifica la carta ni las comandas, solo confirma el pedido.</summary>
        private void BotonSugerencia_Click(object sender, EventArgs e)
        {
            if (!((sender as Control)?.Tag is SugerenciaInteligente_790MY sugerencia)) return;
            TextoSugerencia texto = Redactar(sugerencia);

            // Se avisa que la acción es simulada antes de confirmarla, no después.
            bool confirmado = ConfirmModal_790MY.Confirmar(VentanaPropietaria,
                E.T("RI_msg_confirmar", texto.Titulo), E.T("RI_msg_simulada"), texto.Boton);
            if (!confirmado) return;

            E.MostrarExitoToast(this, texto.Confirmacion);
        }

        #endregion

        #region Formato

        private static string Porcentaje(decimal valor) => valor.ToString("0.0", Cultura) + " %";

        private static bool EsIngles => Cultura.TwoLetterISOLanguageName == "en";

        /// <summary>"martes" / "sábados" / "Tuesdays".</summary>
        private static string DiaEnPlural(DayOfWeek dia)
        {
            string nombre = Cultura.DateTimeFormat.GetDayName(dia);
            if (EsIngles) return nombre + "s";
            nombre = nombre.ToLower(Cultura);
            return nombre.EndsWith("s") ? nombre : nombre + "s";
        }

        private static string NumeroEnPalabras(int n)
        {
            string clave = "RI_num_" + Math.Max(0, Math.Min(10, n));
            return E.T(clave);
        }

        #endregion

        #region i18n

        public void UpdateIdioma()
        {
            E.TraducirTags(this);
            colNumero_790MY.HeaderText = E.T("RI_col_numero");
            colCombinacion_790MY.HeaderText = E.T("RI_col_combinacion");
            colSoporte_790MY.HeaderText = E.T("RI_col_soporte");
            colConfianza_790MY.HeaderText = E.T("RI_col_confianza");

            if (!_listo) return;
            Mostrar();
            MostrarSugerencias();
        }

        #endregion

        #region Ayuda contextual (Etapa 5)
        /// <summary>Sugerencias del Chef (F1): solo datos; el globo y el resalte los dibuja AyudaSugerencias_790MY.</summary>
        public IReadOnlyList<PistaAyuda_790MY> ObtenerPistas() => new[]
        {
            new PistaAyuda_790MY(nameof(sldConfianza_790MY), "AY_sug_pc1_t", "AY_sug_pc1_x"),
            new PistaAyuda_790MY(nameof(dgvReglas_790MY), "AY_sug_pc2_t", "AY_sug_pc2_x"),
            new PistaAyuda_790MY(nameof(btnPorConfianza_790MY), "AY_sug_pc3_t", "AY_sug_pc3_x"),
            new PistaAyuda_790MY(nameof(flpTituloSugerencias_790MY), "AY_sug_pc4_t", "RI_msg_simulada"),
        };
        #endregion
    }
}
