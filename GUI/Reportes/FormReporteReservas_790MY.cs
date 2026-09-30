using BE_08YS;
using BLL_08YS;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Service_08YS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace GUI_08YS.Reportes
{
    public partial class FormReporteReservas_790MY : Form, IIdiomaObserver_08YS
    {
        private readonly ReservaBLL_790MY _reservaBll;
        private List<Reserva_790MY> _ultimoResultado;

        // ── Últimos filtros aplicados (para incluirlos en el PDF) ─────────────
        private DateTime? _filtroDesde;
        private DateTime? _filtroHasta;
        private int?      _filtroDni;

        // ── Panel de resumen de filtros aplicados ───────────────────────────
        private Panel  _pnlResumenFiltros;
        private Label  _lblPrefFechas;
        private Label  _lblValFechas;
        private Label  _lblPrefCliente;
        private Label  _lblValCliente;

        /// <summary>
        /// Wrapper para los ítems del ComboBox de estado.
        /// </summary>
        private class EstadoItem
        {
            public string Display { get; set; }
            public EstadoReserva_790MY? Valor { get; set; }
            public override string ToString() => Display;
        }

        public FormReporteReservas_790MY()
        {
            InitializeComponent();
            _reservaBll = BLLFactory_790MY.CreateReservaBLL();
            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) => TraductorManager_08YS.Instance.Desuscribir(this);
        }

        private void FormReporteReservas_790MY_Load(object sender, EventArgs e)
        {
            dtpDesde_790MY.Checked = false;
            dtpHasta_790MY.Checked = false;
            CargarEstados();
            btnGenerarPDF_790MY.Enabled = false;
            InicializarPanelResumen();
        }

        /// <summary>
        /// Crea programáticamente el panel de resumen de filtros aplicados,
        /// que se muestra entre el panel de filtros y el DataGridView.
        /// </summary>
        private void InicializarPanelResumen()
        {
            var t = TraductorManager_08YS.Instance;

            _pnlResumenFiltros = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 34,
                BackColor = System.Drawing.Color.FromArgb(240, 235, 228),
                Padding   = new Padding(8, 0, 8, 0)
            };

            // Prefijo "Filtro Fechas:"
            _lblPrefFechas = new Label
            {
                AutoSize  = true,
                Font      = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(80, 40, 40),
                Location  = new System.Drawing.Point(8, 10),
                Text      = t.GetTexto("REP_lbl_filtro_fechas") ?? "Filtro Fechas:"
            };

            // Valor dinámico de fechas
            _lblValFechas = new Label
            {
                AutoSize  = true,
                Font      = new System.Drawing.Font("Microsoft Sans Serif", 8.25f),
                ForeColor = System.Drawing.Color.FromArgb(60, 60, 60),
                Location  = new System.Drawing.Point(106, 10),
                Text      = "—"
            };

            // Separador vertical: simple Label con "|"
            var lblSep = new Label
            {
                AutoSize  = true,
                Font      = new System.Drawing.Font("Microsoft Sans Serif", 8.25f),
                ForeColor = System.Drawing.Color.FromArgb(160, 140, 120),
                Location  = new System.Drawing.Point(340, 10),
                Text      = "|"
            };

            // Prefijo "Filtro Cliente:"
            _lblPrefCliente = new Label
            {
                AutoSize  = true,
                Font      = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(80, 40, 40),
                Location  = new System.Drawing.Point(360, 10),
                Text      = t.GetTexto("REP_lbl_filtro_cliente") ?? "Filtro Cliente:"
            };

            // Valor dinámico de cliente
            _lblValCliente = new Label
            {
                AutoSize  = true,
                Font      = new System.Drawing.Font("Microsoft Sans Serif", 8.25f),
                ForeColor = System.Drawing.Color.FromArgb(60, 60, 60),
                Location  = new System.Drawing.Point(460, 10),
                Text      = "—"
            };

            _pnlResumenFiltros.Controls.AddRange(new System.Windows.Forms.Control[]
                { _lblPrefFechas, _lblValFechas, lblSep, _lblPrefCliente, _lblValCliente });

            // Insertar DESPUÉS de pnlFiltros (ambos son DockStyle.Top; el último en
            // añadirse aparece justo debajo del primero en el z-order de docking).
            this.Controls.Add(_pnlResumenFiltros);
        }

        /// <summary>
        /// Actualiza las etiquetas del resumen con los filtros que se acaban de aplicar.
        /// </summary>
        private void ActualizarResumenFiltros(DateTime? desde, DateTime? hasta, int? dni)
        {
            var t = TraductorManager_08YS.Instance;
            string todos = t.GetTexto("REP_txt_todos") ?? "Todos";

            // Rango de fechas
            if (desde.HasValue || hasta.HasValue)
            {
                string dDesde = desde.HasValue ? desde.Value.ToString("dd/MM/yyyy") : "—";
                string dHasta = hasta.HasValue ? hasta.Value.ToString("dd/MM/yyyy") : "—";
                _lblValFechas.Text = $"{dDesde}  →  {dHasta}";
            }
            else
            {
                _lblValFechas.Text = todos;
            }

            // Cliente
            _lblValCliente.Text = dni.HasValue ? dni.Value.ToString() : todos;
        }

        private void LimpiarResumenFiltros()
        {
            if (_lblValFechas  != null) _lblValFechas.Text  = "—";
            if (_lblValCliente != null) _lblValCliente.Text = "—";
        }

        // ───────────────────────────── ComboBox de estado ─────────────────────────────

        private void CargarEstados()
        {
            var t = TraductorManager_08YS.Instance;
            int idx = cboEstado_790MY.SelectedIndex < 0 ? 0 : cboEstado_790MY.SelectedIndex;

            cboEstado_790MY.Items.Clear();
            cboEstado_790MY.Items.Add(new EstadoItem { Display = t.GetTexto("REP_cmbEstadoTodos"), Valor = null });
            cboEstado_790MY.Items.Add(new EstadoItem { Display = t.GetTexto("REP_cmbConfirmada"),  Valor = EstadoReserva_790MY.Confirmada });
            cboEstado_790MY.Items.Add(new EstadoItem { Display = t.GetTexto("REP_cmbCancelada"),   Valor = EstadoReserva_790MY.Cancelada });

            cboEstado_790MY.SelectedIndex = (idx >= 0 && idx < cboEstado_790MY.Items.Count) ? idx : 0;
        }

        // ───────────────────────────── Búsqueda ─────────────────────────────

        private void btnBuscar_790MY_Click(object sender, EventArgs e)
        {
            BuscarReservas();
        }

        private void BuscarReservas()
        {
            var t = TraductorManager_08YS.Instance;

            DateTime? desde = dtpDesde_790MY.Checked ? dtpDesde_790MY.Value.Date : (DateTime?)null;
            DateTime? hasta = dtpHasta_790MY.Checked ? dtpHasta_790MY.Value.Date : (DateTime?)null;

            int? dni = null;
            if (!string.IsNullOrWhiteSpace(txtDniCliente_790MY.Text))
            {
                if (!int.TryParse(txtDniCliente_790MY.Text.Trim(), out int dniParseado))
                {
                    MessageBox.Show(t.GetTexto("msg_rep_dni_invalido"), t.GetTexto("titulo_dato_invalido"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                dni = dniParseado;
            }

            EstadoReserva_790MY? estado = null;
            if (cboEstado_790MY.SelectedItem is EstadoItem item)
                estado = item.Valor;

            try
            {
                _ultimoResultado = _reservaBll.Buscar(desde, hasta, dni, estado);
                dgvReporteReservas_790MY.DataSource = null;
                dgvReporteReservas_790MY.DataSource = _ultimoResultado;
                btnGenerarPDF_790MY.Enabled = _ultimoResultado != null && _ultimoResultado.Count > 0;

                // Mostrar resumen de filtros en el panel descriptivo y guardarlos para el PDF
                ActualizarResumenFiltros(desde, hasta, dni);
                _filtroDesde = desde;
                _filtroHasta = hasta;
                _filtroDni   = dni;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(t.GetTexto("msg_rep_error_buscar"), ex.Message),
                    t.GetTexto("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ───────────────────────────── Formateo de celdas ─────────────────────────────

        private void dgvReporteReservas_790MY_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == colRepHora_790MY.Index && e.Value is TimeSpan ts)
            {
                e.Value = ts.ToString(@"hh\:mm");
                e.FormattingApplied = true;
            }
            else if (e.ColumnIndex == colRepEstado_790MY.Index && e.Value is EstadoReserva_790MY estado)
            {
                var t = TraductorManager_08YS.Instance;
                e.Value = estado == EstadoReserva_790MY.Confirmada
                    ? t.GetTexto("REP_cmbConfirmada")
                    : t.GetTexto("REP_cmbCancelada");
                e.FormattingApplied = true;
            }
        }

        // ───────────────────────────── Generación de PDF ─────────────────────────────

        private void btnGenerarPDF_790MY_Click(object sender, EventArgs e)
        {
            var t = TraductorManager_08YS.Instance;

            if (_ultimoResultado == null || _ultimoResultado.Count == 0)
            {
                MessageBox.Show(t.GetTexto("msg_rep_sin_datos_pdf"), t.GetTexto("titulo_dato_invalido"),
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter   = "PDF (*.pdf)|*.pdf";
                dlg.FileName = $"ReporteReservas_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
                dlg.Title    = t.GetTexto("REP_btnGenerarPDF");

                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    GenerarPDF(dlg.FileName);
                    MessageBox.Show(t.GetTexto("msg_rep_pdf_guardado"), t.GetTexto("exito"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Abrir el PDF automáticamente
                    Process.Start(new ProcessStartInfo(dlg.FileName) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format(t.GetTexto("msg_rep_pdf_error"), ex.Message),
                                    t.GetTexto("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void GenerarPDF(string rutaArchivo)
        {
            var t       = TraductorManager_08YS.Instance;
            var session = SessionManager_08YS.Instance;

            // ── Paleta de colores y fuentes ──
            var colorBordo  = new BaseColor(120, 25, 25);
            var colorGris   = new BaseColor(80, 80, 80);
            var colorCrema  = new BaseColor(250, 248, 245);

            var fuenteTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16f,
                                                         iTextSharp.text.Font.BOLD, colorBordo);
            var fuenteSub    = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9f,
                                                         iTextSharp.text.Font.NORMAL, colorGris);
            var fuenteAudit  = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8.5f,
                                                         iTextSharp.text.Font.NORMAL, colorGris);
            var fuenteAuditB = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8.5f,
                                                         iTextSharp.text.Font.BOLD, colorGris);
            var fuenteEnc    = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9f,
                                                         iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            var fuenteDato   = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8.5f,
                                                         iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            // ── Documento A4 horizontal (landscape) ──
            var doc = new Document(PageSize.A4.Rotate(), 30f, 30f, 40f, 30f);
            PdfWriter.GetInstance(doc, new FileStream(rutaArchivo, FileMode.Create));
            doc.Open();

            // ── Encabezado: logo (izq.) + auditoría (der.) ──
            var tablaEnc = new PdfPTable(2) { WidthPercentage = 100f, SpacingAfter = 6f };
            tablaEnc.SetWidths(new float[] { 1.6f, 2.4f });

            // Celda logo
            string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "logo_fogon.png");
            PdfPCell celdaLogo;
            if (File.Exists(logoPath))
            {
                var img = iTextSharp.text.Image.GetInstance(logoPath);
                img.ScaleToFit(135f, 55f);
                celdaLogo = new PdfPCell { Border = PdfPCell.NO_BORDER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 2f };
                celdaLogo.AddElement(img);
            }
            else
            {
                celdaLogo = new PdfPCell(new Phrase("El Fogón del Sur", fuenteTitulo))
                { Border = PdfPCell.NO_BORDER, VerticalAlignment = Element.ALIGN_MIDDLE };
            }
            tablaEnc.AddCell(celdaLogo);

            // Celda auditoría
            string usuario = session.Current != null
                ? $"{session.Current.Nombre} {session.Current.Apellido} ({session.Current.Rol?.Nombre})"
                : "-";

            // Las claves ya contienen el texto completo (ej. "Generado por:"), sin ": " extra.
            var phraseAudit = new Phrase();
            phraseAudit.Add(new Chunk((t.GetTexto("REP_pdf_generadoPor") ?? "Generado por:") + " ", fuenteAuditB));
            phraseAudit.Add(new Chunk(usuario + "\n", fuenteAudit));
            phraseAudit.Add(new Chunk((t.GetTexto("REP_pdf_fechaEmision") ?? "Fecha de emisión:") + " ", fuenteAuditB));
            phraseAudit.Add(new Chunk(DateTime.Now.ToString("dd/MM/yyyy HH:mm"), fuenteAudit));

            tablaEnc.AddCell(new PdfPCell(phraseAudit)
            {
                Border              = PdfPCell.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment   = Element.ALIGN_MIDDLE,
                PaddingBottom       = 2f
            });

            doc.Add(tablaEnc);

            // ── Título ──
            doc.Add(new Paragraph(t.GetTexto("REP_pdf_titulo"), fuenteTitulo)
            {
                Alignment    = Element.ALIGN_CENTER,
                SpacingAfter = 4f
            });

            // ── Bloque informativo: filtros aplicados ─────────────────────────
            {
                string todos      = t.GetTexto("REP_pdf_todos")         ?? "Todos";
                string lblPeriodo = t.GetTexto("REP_pdf_periodo")        ?? "Período:";
                string lblCliente = t.GetTexto("REP_pdf_filtro_cliente") ?? "Cliente DNI:";

                string valPeriodo;
                if (_filtroDesde.HasValue || _filtroHasta.HasValue)
                {
                    string dDesde = _filtroDesde.HasValue ? _filtroDesde.Value.ToString("dd/MM/yyyy") : "—";
                    string dHasta = _filtroHasta.HasValue ? _filtroHasta.Value.ToString("dd/MM/yyyy") : "—";
                    valPeriodo = $"{dDesde}  –  {dHasta}";
                }
                else
                {
                    valPeriodo = todos;
                }

                string valCliente = _filtroDni.HasValue ? _filtroDni.Value.ToString() : todos;

                var fuenteFiltroB = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8.5f,
                                        iTextSharp.text.Font.BOLD,   new BaseColor(120, 25, 25));
                var fuenteFiltroN = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8.5f,
                                        iTextSharp.text.Font.NORMAL, new BaseColor(50, 50, 50));

                var colorFiltro = new BaseColor(248, 243, 238);
                var borderFiltro = new BaseColor(200, 180, 160);

                var tablaFiltros = new PdfPTable(4) { WidthPercentage = 100f, SpacingAfter = 6f };
                tablaFiltros.SetWidths(new float[] { 1.1f, 2.9f, 1.4f, 2.6f });

                PdfPCell CeldaFiltro(string texto, iTextSharp.text.Font fuente, int align = Element.ALIGN_LEFT) =>
                    new PdfPCell(new Phrase(texto, fuente))
                    {
                        BackgroundColor     = colorFiltro,
                        BorderColor         = borderFiltro,
                        BorderWidth         = 0.5f,
                        Padding             = 4f,
                        HorizontalAlignment = align,
                        VerticalAlignment   = Element.ALIGN_MIDDLE
                    };

                tablaFiltros.AddCell(CeldaFiltro(lblPeriodo, fuenteFiltroB, Element.ALIGN_RIGHT));
                tablaFiltros.AddCell(CeldaFiltro(valPeriodo, fuenteFiltroN));
                tablaFiltros.AddCell(CeldaFiltro(lblCliente, fuenteFiltroB, Element.ALIGN_RIGHT));
                tablaFiltros.AddCell(CeldaFiltro(valCliente, fuenteFiltroN));

                doc.Add(tablaFiltros);
            }

            // ── Tabla de datos (8 columnas) ──
            float[] anchos = { 0.6f, 1.1f, 2.5f, 0.7f, 1.2f, 1.0f, 1.1f, 1.2f };
            var tabla = new PdfPTable(8) { WidthPercentage = 100f, SpacingBefore = 4f };
            tabla.SetWidths(anchos);

            // Encabezados con fondo bordó y borde bordó
            string[] encabezados =
            {
                t.GetTexto("REP_colNro"),        t.GetTexto("REP_colDni"),         t.GetTexto("REP_colCliente"),
                t.GetTexto("REP_colMesa"),        t.GetTexto("REP_colFecha"),       t.GetTexto("REP_colHora"),
                t.GetTexto("REP_colComensales"),  t.GetTexto("REP_colEstado")
            };

            foreach (var enc in encabezados)
            {
                tabla.AddCell(new PdfPCell(new Phrase(enc, fuenteEnc))
                {
                    BackgroundColor     = colorBordo,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding             = 5f,
                    BorderColor         = colorBordo,
                    BorderWidth         = 0.5f
                });
            }

            // Filas de datos con bordes bordó
            for (int i = 0; i < _ultimoResultado.Count; i++)
            {
                var r  = _ultimoResultado[i];
                var bg = (i % 2 == 0) ? BaseColor.WHITE : colorCrema;

                string nombre = string.IsNullOrWhiteSpace(r.ClienteNombreCompleto)
                                ? r.ClienteDNI.ToString()
                                : r.ClienteNombreCompleto;

                string estadoTexto = r.Estado == EstadoReserva_790MY.Confirmada
                                     ? t.GetTexto("REP_cmbConfirmada")
                                     : t.GetTexto("REP_cmbCancelada");

                void Ag(string texto, int alin = Element.ALIGN_CENTER) =>
                    tabla.AddCell(new PdfPCell(new Phrase(texto, fuenteDato))
                    {
                        BackgroundColor     = bg,
                        HorizontalAlignment = alin,
                        Padding             = 4f,
                        BorderColor         = colorBordo,
                        BorderWidth         = 0.5f
                    });

                Ag(r.ReservaID.ToString());
                Ag(r.ClienteDNI.ToString());
                Ag(nombre, Element.ALIGN_LEFT);
                Ag(r.MesaNumero.ToString());
                Ag(r.Fecha.ToString("dd/MM/yyyy"));
                Ag(r.Hora.ToString(@"hh\:mm"));
                Ag(r.CantidadComensales.ToString());
                Ag(estadoTexto);
            }

            doc.Add(tabla);

            // ── Pie: total de registros ──
            doc.Add(new Paragraph(
                $"{t.GetTexto("REP_pdf_total")}: {_ultimoResultado.Count}", fuenteSub)
            {
                Alignment     = Element.ALIGN_RIGHT,
                SpacingBefore = 6f
            });

            doc.Close();
        }

        // ───────────────────────────── Limpiar ─────────────────────────────

        private void btnLimpiar_790MY_Click(object sender, EventArgs e)
        {
            LimpiarFiltros();
        }

        private void LimpiarFiltros()
        {
            dtpDesde_790MY.Checked        = false;
            dtpHasta_790MY.Checked        = false;
            txtDniCliente_790MY.Text       = string.Empty;
            cboEstado_790MY.SelectedIndex  = 0;
            dgvReporteReservas_790MY.DataSource = null;
            _ultimoResultado               = null;
            btnGenerarPDF_790MY.Enabled    = false;
            _filtroDesde = null;
            _filtroHasta = null;
            _filtroDni   = null;
            LimpiarResumenFiltros();
        }

        // ───────────────────────────── Cerrar ─────────────────────────────

        private void btnCerrar_790MY_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ───────────────────────────── i18n ─────────────────────────────

        #region i18n
        public void UpdateIdioma()
        {
            TraducirControles(this);
            CargarEstados();
            this.Text = TraductorManager_08YS.Instance.GetTexto("REP_titulo");

            // Traducir prefijos del panel de resumen de filtros
            if (_lblPrefFechas  != null)
                _lblPrefFechas.Text  = TraductorManager_08YS.Instance.GetTexto("REP_lbl_filtro_fechas")  ?? "Filtro Fechas:";
            if (_lblPrefCliente != null)
                _lblPrefCliente.Text = TraductorManager_08YS.Instance.GetTexto("REP_lbl_filtro_cliente") ?? "Filtro Cliente:";

            // Encabezados de columnas del DGV (no son Controls, deben traducirse explícitamente)
            colRepID_790MY.HeaderText          = TraductorManager_08YS.Instance.GetTexto("REP_colNro");
            colRepDni_790MY.HeaderText         = TraductorManager_08YS.Instance.GetTexto("REP_colDni");
            colRepCliente_790MY.HeaderText     = TraductorManager_08YS.Instance.GetTexto("REP_colCliente");
            colRepMesa_790MY.HeaderText        = TraductorManager_08YS.Instance.GetTexto("REP_colMesa");
            colRepFecha_790MY.HeaderText       = TraductorManager_08YS.Instance.GetTexto("REP_colFecha");
            colRepHora_790MY.HeaderText        = TraductorManager_08YS.Instance.GetTexto("REP_colHora");
            colRepComensales_790MY.HeaderText  = TraductorManager_08YS.Instance.GetTexto("REP_colComensales");
            colRepEstado_790MY.HeaderText      = TraductorManager_08YS.Instance.GetTexto("REP_colEstado");
        }

        private void TraducirControles(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                if (c is TextBox || c is RichTextBox)
                    continue;

                if (c.Tag is string clave && !string.IsNullOrWhiteSpace(clave))
                    c.Text = TraductorManager_08YS.Instance.GetTexto(clave);

                if (c.HasChildren)
                    TraducirControles(c);
            }
        }
        #endregion
    }
}
