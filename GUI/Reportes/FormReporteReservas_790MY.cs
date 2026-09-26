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
            var t = TraductorManager_08YS.Instance;

            // Documento A4 horizontal (landscape)
            var doc = new Document(PageSize.A4.Rotate(), 30f, 30f, 40f, 30f);
            PdfWriter.GetInstance(doc, new FileStream(rutaArchivo, FileMode.Create));
            doc.Open();

            // ── Título ──
            var fuenteTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16f,
                                                         iTextSharp.text.Font.BOLD,
                                                         new BaseColor(120, 25, 25));
            doc.Add(new Paragraph(t.GetTexto("REP_pdf_titulo"), fuenteTitulo)
            {
                Alignment   = Element.ALIGN_CENTER,
                SpacingAfter = 4f
            });

            // ── Subtítulo: fecha de generación ──
            var fuenteSub = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9f,
                                                      iTextSharp.text.Font.NORMAL,
                                                      new BaseColor(80, 80, 80));
            doc.Add(new Paragraph(
                $"{t.GetTexto("REP_pdf_generado")}: {DateTime.Now:dd/MM/yyyy HH:mm}", fuenteSub)
            {
                Alignment    = Element.ALIGN_CENTER,
                SpacingAfter = 10f
            });

            // ── Tabla (8 columnas) ──
            float[] anchos = { 0.6f, 1.1f, 2.5f, 0.7f, 1.2f, 1.0f, 1.1f, 1.2f };
            var tabla = new PdfPTable(8) { WidthPercentage = 100f, SpacingBefore = 4f };
            tabla.SetWidths(anchos);

            // Encabezados
            var fuenteEnc = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9f,
                                                      iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            var colorEnc = new BaseColor(120, 25, 25);
            string[] encabezados =
            {
                t.GetTexto("REP_colNro"), t.GetTexto("REP_colDni"), t.GetTexto("REP_colCliente"),
                t.GetTexto("REP_colMesa"), t.GetTexto("REP_colFecha"), t.GetTexto("REP_colHora"),
                t.GetTexto("REP_colComensales"), t.GetTexto("REP_colEstado")
            };

            foreach (var enc in encabezados)
            {
                tabla.AddCell(new PdfPCell(new Phrase(enc, fuenteEnc))
                {
                    BackgroundColor     = colorEnc,
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    Padding             = 5f
                });
            }

            // Filas de datos
            var fuenteDato = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8.5f,
                                                       iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            var colorPar = new BaseColor(250, 248, 245);

            for (int i = 0; i < _ultimoResultado.Count; i++)
            {
                var r  = _ultimoResultado[i];
                var bg = (i % 2 == 0) ? BaseColor.WHITE : colorPar;

                string nombre = string.IsNullOrWhiteSpace(r.ClienteNombreCompleto)
                                ? r.ClienteDNI.ToString()
                                : r.ClienteNombreCompleto;

                string estadoTexto = r.Estado == EstadoReserva_790MY.Confirmada
                                     ? t.GetTexto("REP_cmbConfirmada")
                                     : t.GetTexto("REP_cmbCancelada");

                void Ag(string texto, int alin = Element.ALIGN_CENTER) =>
                    tabla.AddCell(new PdfPCell(new Phrase(texto, fuenteDato))
                    { BackgroundColor = bg, HorizontalAlignment = alin, Padding = 4f });

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
                Alignment    = Element.ALIGN_RIGHT,
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
