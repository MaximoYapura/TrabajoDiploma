using BE_08YS;
using BLL_08YS;
using GUI_08YS.Maestros;
using Service_08YS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using E = GUI_08YS.RF2.EstiloRF2_790MY;

namespace GUI_08YS.Admin
{
    /// <summary>
    /// Bitácora de Cambios - Auditoría de Productos (patente AuditarProductos).
    /// Lista las versiones de Productos_C, compara la versión seleccionada contra la
    /// vigente del mismo producto (inspector de diferencias) y permite restaurarla
    /// (rollback). Consultas, diff, DV y bitácora los resuelve ProductoCBLL_790MY.
    /// </summary>
    public partial class FormBitacoraCambios_790MY : Form, IIdiomaObserver_08YS
    {
        // Paleta oficial de la casa.
        private static readonly Color Bordo = Color.FromArgb(107, 29, 47);          // #6B1D2F
        private static readonly Color BordoHover = Color.FromArgb(86, 22, 37);
        private static readonly Color BordoSuave = Color.FromArgb(248, 236, 238);
        private static readonly Color CremaFila = Color.FromArgb(250, 246, 240);
        private static readonly Color Linea = Color.FromArgb(236, 228, 218);
        private static readonly Color VigenteFondo = Color.FromArgb(232, 244, 235);
        private static readonly Color VigenteBarra = Color.FromArgb(46, 125, 72);
        private static readonly Color CambioFondo = Color.FromArgb(252, 241, 222);
        private static readonly Color CambioTexto = Color.FromArgb(150, 92, 18);
        private static readonly Color BajaFondo = Color.FromArgb(252, 228, 228);
        private static readonly Color BajaTexto = Color.FromArgb(170, 30, 30);

        private static readonly CultureInfo CulturaMoneda = CultureInfo.GetCultureInfo("es-AR");

        private readonly ProductoCBLL_790MY _bll;
        private List<Producto_C_790MY> _historial = new List<Producto_C_790MY>();
        private DiferenciaVersion_790MY _diff;
        private readonly HashSet<int> _filasDiffResaltadas = new HashSet<int>();
        private bool _listo;
        private bool _cargando;

        private readonly Font _fFilaVigente = new Font("Segoe UI", 10.5F, FontStyle.Bold);
        private readonly Font _fPildora = new Font("Segoe UI", 9F, FontStyle.Bold);
        private readonly Font _fDiffEncabezado = new Font("Segoe UI", 9F, FontStyle.Bold);
        private readonly Font _fDiffTexto = new Font("Segoe UI", 9.75F);
        private readonly Font _fDiffValor = new Font("Segoe UI", 9.75F, FontStyle.Bold);
        private readonly Font _fDiffDelta = new Font("Segoe UI", 8.75F);

        public FormBitacoraCambios_790MY()
        {
            InitializeComponent();
            _bll = BLLFactory_790MY.CreateProductoCBLL();

            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) =>
            {
                TraductorManager_08YS.Instance.Desuscribir(this);
                foreach (var f in new[] { _fFilaVigente, _fPildora, _fDiffEncabezado, _fDiffTexto, _fDiffValor, _fDiffDelta })
                    f.Dispose();
            };
        }

        private IWin32Window VentanaPropietaria => (this.TopLevelControl as Form) ?? (IWin32Window)this;

        private static CultureInfo Cultura => TraductorManager_08YS.Instance.CulturaActual ?? CultureInfo.CurrentCulture;

        private Producto_C_790MY Seleccionado => dgvHistorial_790MY.CurrentRow?.Tag as Producto_C_790MY;

        private void FormBitacoraCambios_790MY_Load(object sender, EventArgs e)
        {
            EstiloMaestros_790MY.EstilizarGrilla(dgvHistorial_790MY);
            dgvHistorial_790MY.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvHistorial_790MY.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White;
            dgvHistorial_790MY.ColumnHeadersDefaultCellStyle.ForeColor = E.Texto;
            dgvHistorial_790MY.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            dgvHistorial_790MY.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvHistorial_790MY.ColumnHeadersHeight = 48;
            dgvHistorial_790MY.RowTemplate.Height = 40;
            dgvHistorial_790MY.DefaultCellStyle.SelectionBackColor = BordoSuave;
            foreach (var col in new[] { colID_790MY, colCodigo_790MY, colPrecio_790MY, colStock_790MY })
            {
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                col.DefaultCellStyle.Padding = new Padding(0, 0, 12, 0);
            }
            colCategoria_790MY.DefaultCellStyle.ForeColor = E.Gris;

            EstiloMaestros_790MY.EstilizarBoton(btnAplicar_790MY, true);
            EstiloMaestros_790MY.EstilizarBoton(btnLimpiar_790MY, false);
            EstiloMaestros_790MY.EstilizarBoton(btnSalir_790MY, false);
            btnSalir_790MY.FlatAppearance.BorderColor = E.BordeCard;

            dtpDesde_790MY.Value = ProductoCBLL_790MY.FechaDesdePorDefecto;
            dtpHasta_790MY.Value = DateTime.Today;
            dtpDesde_790MY.Checked = false;
            dtpHasta_790MY.Checked = false;

            tlpDiff_790MY.CellPaint += tlpDiff_790MY_CellPaint;

            _listo = true;
            UpdateIdioma();
            Cargar();
        }

        #region Datos

        private void Cargar(int? seleccionarProductoVigente = null, int? seleccionarId = null)
        {
            int? codigo = null;
            if (!string.IsNullOrWhiteSpace(txtCodigo_790MY.Text))
            {
                if (!int.TryParse(txtCodigo_790MY.Text.Trim(), out int valor) || valor <= 0)
                {
                    MessageBox.Show(VentanaPropietaria, E.T("BC_msg_codigo_invalido"), E.T("titulo_datos_invalidos"),
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCodigo_790MY.Focus();
                    return;
                }
                codigo = valor;
            }

            try
            {
                _historial = _bll.ConsultarHistorial(codigo, txtNombre_790MY.Text, FechaDesdeFiltro, FechaHastaFiltro);
            }
            catch (Exception ex)
            {
                E.MostrarError(VentanaPropietaria, ex);
                return;
            }

            // Mientras se rearma la grilla SelectionChanged se dispara con filas sin Tag:
            // el detalle se actualiza una sola vez al final.
            _cargando = true;
            dgvHistorial_790MY.SuspendLayout();
            dgvHistorial_790MY.Rows.Clear();
            foreach (Producto_C_790MY v in _historial)
            {
                int i = dgvHistorial_790MY.Rows.Add(
                    v.ID,
                    v.ProductoID,
                    v.Fecha.ToString("dd/MM/yyyy", Cultura),
                    v.Fecha.ToString("HH:mm:ss", Cultura),
                    v.Nombre,
                    TextoCategoria(v.Categoria),
                    Moneda(v.Precio),
                    v.StockPorciones,
                    TextoEstado(v.Activo),
                    v.Act ? "1" : "0");

                DataGridViewRow fila = dgvHistorial_790MY.Rows[i];
                fila.Tag = v;
                if (v.Act)
                {
                    fila.DefaultCellStyle.BackColor = VigenteFondo;
                    fila.DefaultCellStyle.SelectionBackColor = Color.FromArgb(214, 236, 220);
                    fila.DefaultCellStyle.Font = _fFilaVigente;
                }
            }
            dgvHistorial_790MY.ResumeLayout();

            SeleccionarFila(seleccionarProductoVigente, seleccionarId);
            _cargando = false;

            lblSinRegistros_790MY.Visible = _historial.Count == 0;
            UbicarMensajeVacio();
            MostrarEstado();
            MostrarDetalle();
        }

        private void SeleccionarFila(int? productoVigente, int? id)
        {
            dgvHistorial_790MY.ClearSelection();
            foreach (DataGridViewRow fila in dgvHistorial_790MY.Rows)
            {
                var v = (Producto_C_790MY)fila.Tag;
                bool coincide = (id.HasValue && v.ID == id.Value)
                             || (productoVigente.HasValue && v.ProductoID == productoVigente.Value && v.Act);
                if (!coincide) continue;

                dgvHistorial_790MY.CurrentCell = fila.Cells[colID_790MY.Index];
                fila.Selected = true;
                return;
            }

            if (dgvHistorial_790MY.Rows.Count > 0)
            {
                dgvHistorial_790MY.CurrentCell = dgvHistorial_790MY.Rows[0].Cells[colID_790MY.Index];
                dgvHistorial_790MY.Rows[0].Selected = true;
            }
        }

        private DateTime? FechaDesdeFiltro => dtpDesde_790MY.Checked ? dtpDesde_790MY.Value.Date : (DateTime?)null;

        private DateTime? FechaHastaFiltro => dtpHasta_790MY.Checked ? dtpHasta_790MY.Value.Date : (DateTime?)null;

        private void btnAplicar_790MY_Click(object sender, EventArgs e) => Cargar(seleccionarId: Seleccionado?.ID);

        private void btnLimpiar_790MY_Click(object sender, EventArgs e)
        {
            txtCodigo_790MY.Clear();
            txtNombre_790MY.Clear();
            dtpDesde_790MY.Value = ProductoCBLL_790MY.FechaDesdePorDefecto;
            dtpHasta_790MY.Value = DateTime.Today;
            dtpDesde_790MY.Checked = false;
            dtpHasta_790MY.Checked = false;
            Cargar();
        }

        private void Filtro_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.Handled = true;
            e.SuppressKeyPress = true;
            Cargar();
        }

        private void txtCodigo_790MY_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        #endregion

        #region Estado

        private void MostrarEstado()
        {
            DateTime? desde = FechaDesdeFiltro, hasta = FechaHastaFiltro;
            string cantidad = _historial.Count.ToString("N0", Cultura);

            if (!desde.HasValue && !hasta.HasValue)
                lblEstado_790MY.Text = E.T("BC_estado_default", cantidad, ProductoCBLL_790MY.DiasPorDefecto,
                                           Fecha(ProductoCBLL_790MY.FechaDesdePorDefecto));
            else if (desde.HasValue && hasta.HasValue)
                lblEstado_790MY.Text = E.T("BC_estado_rango", cantidad, Fecha(desde.Value), Fecha(hasta.Value));
            else if (desde.HasValue)
                lblEstado_790MY.Text = E.T("BC_estado_desde", cantidad, Fecha(desde.Value));
            else
                lblEstado_790MY.Text = E.T("BC_estado_hasta", cantidad, Fecha(hasta.Value));

            Producto_C_790MY sel = Seleccionado;
            if (sel == null)
                lblSeleccion_790MY.Text = E.T("BC_seleccion_ninguna");
            else if (sel.Act)
                lblSeleccion_790MY.Text = E.T("BC_seleccion_vigente", sel.ID, sel.Nombre);
            else
                lblSeleccion_790MY.Text = E.T("BC_seleccion", sel.ID, sel.Nombre);
        }

        #endregion

        #region Inspector de diferencias

        private void dgvHistorial_790MY_SelectionChanged(object sender, EventArgs e)
        {
            if (!_listo || _cargando) return;
            MostrarEstado();
            MostrarDetalle();
        }

        private void MostrarDetalle()
        {
            Producto_C_790MY sel = Seleccionado;
            _diff = null;

            if (sel == null)
            {
                lblDetalleProducto_790MY.Text = "-";
                lblDetalleVersion_790MY.Text = string.Empty;
                lblDetalleResumen_790MY.Text = E.T("BC_detalle_vacio");
                LimpiarDiff();
                ActualizarBotonRestaurar();
                return;
            }

            lblDetalleProducto_790MY.Text = sel.Nombre;
            lblDetalleVersion_790MY.Text = E.T("BC_detalle_version", sel.ID, Fecha(sel.Fecha), sel.Fecha.ToString("HH:mm", Cultura));

            try
            {
                Producto_C_790MY vigente = sel.Act
                    ? sel
                    : _historial.FirstOrDefault(v => v.ProductoID == sel.ProductoID && v.Act)
                      ?? _bll.ObtenerVigente(sel.ProductoID);
                _diff = _bll.ObtenerDiferencias(sel, vigente);
            }
            catch (Exception ex)
            {
                lblDetalleResumen_790MY.Text = string.Empty;
                LimpiarDiff();
                ActualizarBotonRestaurar();
                E.MostrarError(VentanaPropietaria, ex);
                return;
            }

            if (_diff.EsVigente)
                lblDetalleResumen_790MY.Text = E.T("BC_resumen_vigente");
            else if (_diff.Vigente == null)
                lblDetalleResumen_790MY.Text = E.T("BC_resumen_sin_vigente");
            else if (_diff.CantidadCambios == 0)
                lblDetalleResumen_790MY.Text = E.T("BC_resumen_sin_cambios");
            else if (_diff.CantidadCambios == 1)
                lblDetalleResumen_790MY.Text = E.T("BC_resumen_un_cambio");
            else
                lblDetalleResumen_790MY.Text = E.T("BC_resumen_cambios", _diff.CantidadCambios);

            ArmarTablaDiff();
            ActualizarBotonRestaurar();
        }

        private void LimpiarDiff()
        {
            tlpDiff_790MY.SuspendLayout();
            foreach (Control c in tlpDiff_790MY.Controls.Cast<Control>().ToList())
            {
                tlpDiff_790MY.Controls.Remove(c);
                c.Dispose();
            }
            tlpDiff_790MY.RowStyles.Clear();
            tlpDiff_790MY.RowCount = 1;
            _filasDiffResaltadas.Clear();
            tlpDiff_790MY.ResumeLayout();
            tlpDiff_790MY.Invalidate();
        }

        /// <summary>
        /// Tabla Dato / Vigente hoy / Al restaurar. Las filas que cambian se resaltan y
        /// muestran el delta (histórico - vigente) debajo del valor a restaurar. Con la
        /// versión vigente seleccionada se muestran sus valores en ambas columnas.
        /// </summary>
        private void ArmarTablaDiff()
        {
            LimpiarDiff();
            Producto_C_790MY sel = _diff.Seleccion;
            Producto_C_790MY vig = _diff.Vigente;

            tlpDiff_790MY.SuspendLayout();
            int fila = 0;
            AgregarFilaDiff(fila++, E.T("BC_diff_dato"), E.T("BC_diff_vigente"), E.T("BC_diff_restaurar"), null, encabezado: true);

            if (vig != null)
            {
                AgregarFilaDiff(fila++, E.T("BC_diff_precio"), Moneda(vig.Precio), Moneda(sel.Precio),
                    _diff.CambiaPrecio ? MonedaConSigno(_diff.DeltaPrecio) : null, resaltar: _diff.CambiaPrecio);

                AgregarFilaDiff(fila++, E.T("BC_diff_stock"), vig.StockPorciones.ToString("N0", Cultura), sel.StockPorciones.ToString("N0", Cultura),
                    _diff.CambiaStock ? E.T("BC_delta_porciones", ConSigno(_diff.DeltaStock)) : null, resaltar: _diff.CambiaStock);

                AgregarFilaDiff(fila++, E.T("BC_diff_estado"), TextoEstado(vig.Activo), TextoEstado(sel.Activo),
                    null, resaltar: _diff.CambiaEstado);

                AgregarFilaDiff(fila++, E.T("BC_diff_categoria"), TextoCategoria(vig.Categoria), TextoCategoria(sel.Categoria),
                    null, resaltar: _diff.CambiaCategoria);

                AgregarFilaDiff(fila++, E.T("BC_diff_nombre"), vig.Nombre, sel.Nombre, null, resaltar: _diff.CambiaNombre);

                if (_diff.CambiaControlaStock)
                    AgregarFilaDiff(fila++, E.T("BC_diff_controla_stock"), TextoSiNo(vig.ControlaStock), TextoSiNo(sel.ControlaStock),
                        null, resaltar: true);
            }

            // Fila de relleno para que las filas de datos no se estiren.
            tlpDiff_790MY.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpDiff_790MY.RowCount = fila + 1;
            tlpDiff_790MY.ResumeLayout();
            tlpDiff_790MY.Invalidate();
        }

        private void AgregarFilaDiff(int fila, string dato, string vigente, string restaurar, string delta,
                                     bool resaltar = false, bool encabezado = false)
        {
            tlpDiff_790MY.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            if (resaltar) _filasDiffResaltadas.Add(fila);

            Font fuente = encabezado ? _fDiffEncabezado : _fDiffTexto;
            Color color = encabezado ? E.Gris : E.Texto;
            var margen = new Padding(4, encabezado ? 4 : 10, 4, encabezado ? 6 : 10);

            tlpDiff_790MY.Controls.Add(CrearCelda(dato, fuente, color, margen), 0, fila);
            tlpDiff_790MY.Controls.Add(CrearCelda(vigente, fuente, color, margen), 1, fila);

            if (string.IsNullOrEmpty(delta))
            {
                tlpDiff_790MY.Controls.Add(CrearCelda(restaurar, resaltar ? _fDiffValor : fuente, color, margen), 2, fila);
                return;
            }

            var pila = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                BackColor = Color.Transparent,
                Margin = new Padding(0, margen.Top - 4, 0, margen.Bottom - 4),
                Padding = Padding.Empty
            };
            pila.Controls.Add(CrearCelda(restaurar, _fDiffValor, E.Texto, new Padding(4, 0, 4, 0)));
            pila.Controls.Add(CrearCelda(delta, _fDiffDelta, CambioTexto, new Padding(4, 1, 4, 0)));
            tlpDiff_790MY.Controls.Add(pila, 2, fila);
        }

        private static Label CrearCelda(string texto, Font fuente, Color color, Padding margen)
        {
            Label lbl = E.CrearLabel(texto, fuente, color);
            lbl.Margin = margen;
            lbl.MaximumSize = new Size(110, 0);
            return lbl;
        }

        private void tlpDiff_790MY_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            if (tlpDiff_790MY.Controls.Count == 0 || e.Row >= tlpDiff_790MY.RowCount - 1) return;

            if (_filasDiffResaltadas.Contains(e.Row))
                using (var pincel = new SolidBrush(CambioFondo))
                    e.Graphics.FillRectangle(pincel, e.CellBounds);

            using (var lapiz = new Pen(Linea))
                e.Graphics.DrawLine(lapiz, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
        }

        #endregion

        #region Restaurar (rollback)

        private void ActualizarBotonRestaurar()
        {
            bool puede = _diff != null && !_diff.EsVigente && _diff.Vigente != null;
            btnRestaurar_790MY.Enabled = puede;
            btnRestaurar_790MY.BackColor = puede ? Color.FromArgb(184, 84, 20) : E.Deshabilitado;
            btnRestaurar_790MY.ForeColor = puede ? Color.White : E.Gris;
            btnRestaurar_790MY.IconColor = btnRestaurar_790MY.ForeColor;
        }

        private void btnRestaurar_790MY_Click(object sender, EventArgs e)
        {
            Producto_C_790MY sel = Seleccionado;
            if (sel == null || _diff == null || _diff.EsVigente || _diff.Vigente == null) return;

            string detalle = _diff.CantidadCambios == 0
                ? E.T("BC_resumen_sin_cambios")
                : E.T("BC_msg_confirmar_cambios", _diff.CantidadCambios);
            var respuesta = MessageBox.Show(VentanaPropietaria,
                E.T("BC_msg_confirmar", sel.ID, sel.Nombre, Fecha(sel.Fecha)) + Environment.NewLine + Environment.NewLine + detalle,
                E.T("BC_btn_restaurar"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (respuesta != DialogResult.Yes) return;

            try
            {
                _bll.RestaurarVersion(sel);
            }
            catch (Exception ex)
            {
                E.MostrarError(VentanaPropietaria, ex);
                Cargar(seleccionarId: sel.ID);
                return;
            }

            E.MostrarExito(VentanaPropietaria, E.T("BC_msg_restaurado", sel.ID, sel.Nombre));

            // La versión restaurada vuelve como una fila vigente nueva (fecha actual): si el
            // filtro "hasta" la deja afuera, se mantiene seleccionada la versión de origen.
            Cargar(seleccionarProductoVigente: sel.ProductoID);
        }

        /// <summary>Atajo Alt+R para restaurar la versión seleccionada.</summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.R))
            {
                if (btnRestaurar_790MY.Enabled) btnRestaurar_790MY.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnSalir_790MY_Click(object sender, EventArgs e) => this.Close();

        #endregion

        #region Grilla

        private void dgvHistorial_790MY_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (!(dgvHistorial_790MY.Rows[e.RowIndex].Tag is Producto_C_790MY v)) return;

            if (e.ColumnIndex == colActivo_790MY.Index && !v.Activo)
            {
                PintarPildora(e, TextoEstado(false), BajaFondo, BajaTexto);
            }
            else if (e.ColumnIndex == colVigente_790MY.Index && v.Act)
            {
                e.PaintBackground(e.CellBounds, (e.State & DataGridViewElementStates.Selected) != 0);
                var circulo = new Rectangle(e.CellBounds.X + 10, e.CellBounds.Y + (e.CellBounds.Height - 18) / 2, 18, 18);
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var pincel = new SolidBrush(VigenteBarra))
                    e.Graphics.FillEllipse(pincel, circulo);
                using (var lapiz = new Pen(Color.White, 2F))
                    e.Graphics.DrawLines(lapiz, new[]
                    {
                        new Point(circulo.X + 5, circulo.Y + 9),
                        new Point(circulo.X + 8, circulo.Y + 12),
                        new Point(circulo.X + 13, circulo.Y + 6)
                    });
                var texto = new Rectangle(circulo.Right + 6, e.CellBounds.Y, e.CellBounds.Width - 40, e.CellBounds.Height);
                TextRenderer.DrawText(e.Graphics, "1", _fFilaVigente, texto, VigenteBarra,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                e.Handled = true;
            }
        }

        private void PintarPildora(DataGridViewCellPaintingEventArgs e, string texto, Color fondo, Color colorTexto)
        {
            e.PaintBackground(e.CellBounds, (e.State & DataGridViewElementStates.Selected) != 0);
            Size medida = TextRenderer.MeasureText(texto, _fPildora);
            var rect = new Rectangle(e.CellBounds.X + 6, e.CellBounds.Y + (e.CellBounds.Height - 24) / 2, medida.Width + 12, 24);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (var ruta = RF2.PanelRedondeado_790MY.CrearRuta(rect, 4))
            using (var pincel = new SolidBrush(fondo))
                e.Graphics.FillPath(pincel, ruta);
            TextRenderer.DrawText(e.Graphics, texto, _fPildora, rect, colorTexto,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            e.Handled = true;
        }

        /// <summary>Barra izquierda: verde en la versión vigente, bordó en la fila seleccionada.</summary>
        private void dgvHistorial_790MY_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridViewRow fila = dgvHistorial_790MY.Rows[e.RowIndex];
            Color? barra = fila.Selected ? Bordo : (fila.Tag is Producto_C_790MY v && v.Act ? VigenteBarra : (Color?)null);
            if (!barra.HasValue) return;

            using (var pincel = new SolidBrush(barra.Value))
                e.Graphics.FillRectangle(pincel, e.RowBounds.X, e.RowBounds.Y, 4, e.RowBounds.Height - 1);
        }

        private void dgvHistorial_790MY_Resize(object sender, EventArgs e) => UbicarMensajeVacio();

        private void UbicarMensajeVacio()
        {
            int top = dgvHistorial_790MY.Top + dgvHistorial_790MY.ColumnHeadersHeight;
            lblSinRegistros_790MY.Bounds = new Rectangle(dgvHistorial_790MY.Left, top, dgvHistorial_790MY.Width,
                                                         Math.Max(0, dgvHistorial_790MY.Height - dgvHistorial_790MY.ColumnHeadersHeight));
            lblSinRegistros_790MY.BringToFront();
        }

        #endregion

        #region Formato

        private static string Moneda(decimal valor) => "$ " + valor.ToString("N2", CulturaMoneda);

        private static string MonedaConSigno(decimal valor)
            => (valor < 0 ? "−" : "+") + Moneda(Math.Abs(valor));

        private static string ConSigno(int valor) => (valor > 0 ? "+" : valor < 0 ? "−" : "") + Math.Abs(valor);

        private static string Fecha(DateTime fecha) => fecha.ToString("dd/MM/yyyy", Cultura);

        private static string TextoEstado(bool activo) => E.T(activo ? "BC_estado_activo" : "BC_estado_baja");

        private static string TextoSiNo(bool valor) => E.T(valor ? "BC_si" : "BC_no");

        private static string TextoCategoria(CategoriaProducto_790MY categoria) => E.T("RP_cat_" + categoria);

        #endregion

        #region i18n

        public void UpdateIdioma()
        {
            E.TraducirTags(this);
            colID_790MY.HeaderText = E.T("BC_col_id");
            colCodigo_790MY.HeaderText = E.T("BC_col_codigo");
            colFecha_790MY.HeaderText = E.T("BC_col_fecha");
            colHora_790MY.HeaderText = E.T("BC_col_hora");
            colNombre_790MY.HeaderText = E.T("BC_col_nombre");
            colCategoria_790MY.HeaderText = E.T("BC_col_categoria");
            colPrecio_790MY.HeaderText = E.T("BC_col_precio");
            colStock_790MY.HeaderText = E.T("BC_col_stock");
            colActivo_790MY.HeaderText = E.T("BC_col_activo");
            colVigente_790MY.HeaderText = E.T("BC_col_vigente");

            string rol = SessionManager_08YS.Instance.Current?.Rol?.Nombre;
            string rolTraducido = string.IsNullOrEmpty(rol) ? string.Empty : E.T("rol_" + rol);
            if (string.IsNullOrEmpty(rolTraducido) || rolTraducido.StartsWith("[")) rolTraducido = rol;   // clave inexistente
            lblSesion_790MY.Text = E.T("BC_sesion", rolTraducido);

            if (!_listo) return;
            foreach (DataGridViewRow fila in dgvHistorial_790MY.Rows)
            {
                var v = (Producto_C_790MY)fila.Tag;
                fila.Cells[colCategoria_790MY.Index].Value = TextoCategoria(v.Categoria);
                fila.Cells[colActivo_790MY.Index].Value = TextoEstado(v.Activo);
            }
            MostrarEstado();
            MostrarDetalle();
        }

        #endregion
    }
}
