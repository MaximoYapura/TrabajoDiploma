using BE_08YS;
using BLL_08YS;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GUI_08YS.Estilo;
using E = GUI_08YS.RF2.EstiloRF2_790MY;

namespace GUI_08YS.Maestros
{
    /// <summary>
    /// ABM de la carta (RF2). Precio vigente, categoría y stock físico de cada producto;
    /// la carta de Registrar Pedido solo muestra los activos. Validaciones, DV y
    /// bitácora los resuelve ProductoBLL_790MY (patente GestionProductos).
    /// </summary>
    public partial class FormProductos_790MY : Form, IIdiomaObserver_08YS, IAyudaContextual_790MY
    {
        private const int UmbralBajoStock = 5;

        private static readonly Dictionary<string, Permisos> _mapaPermisos =
            new Dictionary<string, Permisos>
            {
                { nameof(btnAnadir_790MY),    Permisos.GestionProductos },
                { nameof(btnModificar_790MY), Permisos.GestionProductos },
                { nameof(btnBaja_790MY),      Permisos.GestionProductos },
            };

        private readonly ProductoBLL_790MY _productoBll;
        private List<Producto_790MY> _productos = new List<Producto_790MY>();
        private ModoEdicion_790MY _modo = ModoEdicion_790MY.Reposo;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        public FormProductos_790MY()
        {
            InitializeComponent();
            _productoBll = BLLFactory_790MY.CreateProductoBLL();

            EstiloMaestros_790MY.EstilizarGrilla(dgvProductos_790MY);
            EstiloMaestros_790MY.EstilizarBoton(btnAnadir_790MY, true);
            EstiloMaestros_790MY.EstilizarBoton(btnModificar_790MY, false);
            EstiloMaestros_790MY.EstilizarBoton(btnBaja_790MY, false);
            EstiloMaestros_790MY.EstilizarBoton(btnGuardar_790MY, true);
            EstiloMaestros_790MY.EstilizarBoton(btnCancelar_790MY, false);

            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) => TraductorManager_08YS.Instance.Desuscribir(this);
        }

        private IWin32Window VentanaPropietaria => (this.TopLevelControl as Form) ?? (IWin32Window)this;

        private Producto_790MY Seleccionado => dgvProductos_790MY.CurrentRow?.Tag as Producto_790MY;

        private void FormProductos_790MY_Load(object sender, EventArgs e)
        {
            PermissionFilter_08YS.Aplicar(this, _mapaPermisos);
            UpdateIdioma();
            Cargar();
            EstablecerModo(ModoEdicion_790MY.Reposo);
        }

        #region Datos y grilla

        private void Cargar(int? seleccionarId = null)
        {
            try
            {
                _productos = _productoBll.GetAll();
            }
            catch (Exception ex)
            {
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
            }
            RenderizarGrilla(seleccionarId);
        }

        private void Filtro_Changed(object sender, EventArgs e)
        {
            if (_modo == ModoEdicion_790MY.Reposo) RenderizarGrilla(Seleccionado?.ProductoID);
        }

        private void RenderizarGrilla(int? seleccionarId)
        {
            string texto = txtBuscar_790MY.Text.Trim();
            var categoria = (cmbFiltroCategoria_790MY.SelectedItem as OpcionCombo_790MY<CategoriaProducto_790MY?>)?.Valor;

            var visibles = _productos
                .Where(p => texto.Length == 0
                    || p.ProductoID.ToString() == texto.TrimStart('0')
                    || p.Nombre.IndexOf(texto, StringComparison.CurrentCultureIgnoreCase) >= 0)
                .Where(p => !categoria.HasValue || p.Categoria == categoria.Value)
                .Where(p => !chkSoloActivos_790MY.Checked || p.Activo)
                .OrderBy(p => p.ProductoID)
                .ToList();

            dgvProductos_790MY.SuspendLayout();
            dgvProductos_790MY.Rows.Clear();
            foreach (var p in visibles)
            {
                int fila = dgvProductos_790MY.Rows.Add(
                    p.ProductoID.ToString("000"),
                    p.Nombre,
                    null,
                    E.Moneda(p.Precio),
                    null,
                    p.ControlaStock ? E.T("FP_si") : E.T("FP_no"),
                    null);
                dgvProductos_790MY.Rows[fila].Tag = p;
                if (p.ProductoID == seleccionarId)
                    dgvProductos_790MY.CurrentCell = dgvProductos_790MY.Rows[fila].Cells[0];
            }
            dgvProductos_790MY.ResumeLayout();

            if (!seleccionarId.HasValue)
                dgvProductos_790MY.ClearSelection();

            int alerta = _productos.Count(p => p.Activo && p.ControlaStock && p.PorcionesDisponibles <= UmbralBajoStock);
            lblResumen_790MY.Text = E.T("FP_resumen", _productos.Count, visibles.Count, alerta);
            MostrarSeleccion();
        }

        private void dgvProductos_790MY_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var p = dgvProductos_790MY.Rows[e.RowIndex].Tag as Producto_790MY;
            if (p == null) return;

            if (e.ColumnIndex == colCategoria_790MY.Index)
            {
                Color fondo, texto;
                switch (p.Categoria)
                {
                    case CategoriaProducto_790MY.Entrada:        fondo = Color.FromArgb(238, 232, 222); texto = Color.FromArgb(90, 74, 50); break;
                    case CategoriaProducto_790MY.PlatoPrincipal: fondo = Color.FromArgb(252, 230, 232); texto = Color.FromArgb(140, 30, 50); break;
                    case CategoriaProducto_790MY.Postre:         fondo = Color.FromArgb(240, 230, 248); texto = Color.FromArgb(100, 50, 130); break;
                    default:                                     fondo = Color.FromArgb(226, 234, 246); texto = Color.FromArgb(40, 70, 120); break;
                }
                EstiloMaestros_790MY.PintarPildora(e, E.T("RP_cat_" + p.Categoria), fondo, texto);
            }
            else if (e.ColumnIndex == colStock_790MY.Index)
            {
                PintarStock(e, p);
            }
            else if (e.ColumnIndex == colEstado_790MY.Index)
            {
                EstiloMaestros_790MY.PintarEstado(e, p.Activo, E.T("FE_activo"), E.T("FE_inactivo"));
            }
            else if (e.ColumnIndex == colControlaStock_790MY.Index)
            {
                e.CellStyle.ForeColor = p.ControlaStock ? E.Bordo : E.Gris;
            }
        }

        /// <summary>Stock disponible (físico - comprometido) con alerta "Bajo" / "Agotado".</summary>
        private void PintarStock(DataGridViewCellPaintingEventArgs e, Producto_790MY p)
        {
            if (!p.ControlaStock)
            {
                EstiloMaestros_790MY.PintarPildora(e, E.T("FP_sin_control"), Color.FromArgb(238, 232, 222), E.Gris);
                return;
            }

            int disponible = Math.Max(0, p.PorcionesDisponibles);
            string chip = disponible == 0 ? E.T("RP_agotado") : disponible <= UmbralBajoStock ? E.T("FP_bajo") : null;

            if (chip == null)
            {
                e.PaintBackground(e.CellBounds, (e.State & DataGridViewElementStates.Selected) != 0);
                TextRenderer.DrawText(e.Graphics, disponible.ToString(), e.CellStyle.Font,
                    new Rectangle(e.CellBounds.X + 12, e.CellBounds.Y, e.CellBounds.Width - 12, e.CellBounds.Height),
                    E.Texto, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                e.Handled = true;
                return;
            }

            Color fondo = disponible == 0 ? E.Deshabilitado : E.PendFondo;
            Color texto = disponible == 0 ? E.Texto : E.PendTexto;
            EstiloMaestros_790MY.PintarPildora(e, $"{chip} · {disponible}", fondo, texto);
        }

        private void dgvProductos_790MY_SelectionChanged(object sender, EventArgs e)
        {
            if (_modo != ModoEdicion_790MY.Reposo) return;
            // Un aviso sobre el producto anterior (por ejemplo, una baja rechazada) ya no aplica.
            bnrAvisos_790MY.Ocultar();
            MostrarSeleccion();
        }

        private void MostrarSeleccion()
        {
            var p = Seleccionado;
            btnModificar_790MY.Enabled = p != null;
            btnBaja_790MY.Enabled = p != null && p.Activo;

            if (p == null) { LimpiarCampos(); return; }

            lblId_790MY.Text = E.T("FP_id", p.ProductoID.ToString("000"));
            txtNombre_790MY.Text = p.Nombre;
            SeleccionarCategoria(p.Categoria);
            nudPrecio_790MY.Value = Math.Min(nudPrecio_790MY.Maximum, p.Precio);
            nudStock_790MY.Value = Math.Min(nudStock_790MY.Maximum, Math.Max(0, p.StockPorciones));
            txtComprometido_790MY.Text = p.PorcionesComprometidas.ToString();
            chkControlaStock_790MY.Checked = p.ControlaStock;
            chkActivo_790MY.Checked = p.Activo;
        }

        #endregion

        #region Combos

        private void CargarCombosFijos()
        {
            int filtro = Math.Max(0, cmbFiltroCategoria_790MY.SelectedIndex);
            object categoriaActual = (cmbCategoria_790MY.SelectedItem as OpcionCombo_790MY<CategoriaProducto_790MY>)?.Valor;

            cmbFiltroCategoria_790MY.Items.Clear();
            cmbFiltroCategoria_790MY.Items.Add(new OpcionCombo_790MY<CategoriaProducto_790MY?> { Valor = null, Texto = E.T("FP_todas_categorias") });
            cmbCategoria_790MY.Items.Clear();

            foreach (CategoriaProducto_790MY c in Enum.GetValues(typeof(CategoriaProducto_790MY)))
            {
                cmbFiltroCategoria_790MY.Items.Add(new OpcionCombo_790MY<CategoriaProducto_790MY?> { Valor = c, Texto = E.T("RP_cat_" + c) });
                cmbCategoria_790MY.Items.Add(new OpcionCombo_790MY<CategoriaProducto_790MY> { Valor = c, Texto = E.T("RP_cat_" + c) });
            }

            cmbFiltroCategoria_790MY.SelectedIndexChanged -= Filtro_Changed;
            cmbFiltroCategoria_790MY.SelectedIndex = Math.Min(filtro, cmbFiltroCategoria_790MY.Items.Count - 1);
            cmbFiltroCategoria_790MY.SelectedIndexChanged += Filtro_Changed;

            if (categoriaActual is CategoriaProducto_790MY categoria) SeleccionarCategoria(categoria);
        }

        private void SeleccionarCategoria(CategoriaProducto_790MY categoria)
        {
            cmbCategoria_790MY.SelectedItem = cmbCategoria_790MY.Items.Cast<OpcionCombo_790MY<CategoriaProducto_790MY>>()
                .FirstOrDefault(o => o.Valor == categoria);
        }

        #endregion

        #region Modo de edición

        private void EstablecerModo(ModoEdicion_790MY modo)
        {
            _modo = modo;
            bool editando = modo != ModoEdicion_790MY.Reposo;

            txtNombre_790MY.ReadOnly = !editando;
            cmbCategoria_790MY.Enabled = nudPrecio_790MY.Enabled = chkControlaStock_790MY.Enabled = chkActivo_790MY.Enabled = editando;
            nudStock_790MY.Enabled = editando && chkControlaStock_790MY.Checked;

            btnGuardar_790MY.Visible = btnCancelar_790MY.Visible = editando;
            btnAnadir_790MY.Enabled = !editando;
            dgvProductos_790MY.Enabled = !editando;
            txtBuscar_790MY.Enabled = cmbFiltroCategoria_790MY.Enabled = chkSoloActivos_790MY.Enabled = !editando;

            string clave = modo == ModoEdicion_790MY.Alta ? "FE_modo_alta" : modo == ModoEdicion_790MY.Modificacion ? "FE_modo_modificacion" : "FE_modo_consulta";
            chipModo_790MY.Text = E.T(clave);
            chipModo_790MY.BackColor = editando ? E.BordoSuave : Color.FromArgb(238, 232, 222);
            chipModo_790MY.ForeColor = editando ? E.Bordo : Color.FromArgb(80, 72, 66);
            lblModo_790MY.Text = E.T("FE_modo", chipModo_790MY.Text);

            bnrAvisos_790MY.Ocultar();

            if (editando)
                btnModificar_790MY.Enabled = btnBaja_790MY.Enabled = false;
            else
                MostrarSeleccion();
        }

        private void LimpiarCampos()
        {
            lblId_790MY.Text = E.T("FP_id", E.T("FP_id_nuevo"));
            txtNombre_790MY.Clear();
            SeleccionarCategoria(CategoriaProducto_790MY.PlatoPrincipal);
            nudPrecio_790MY.Value = 0;
            nudStock_790MY.Value = 0;
            txtComprometido_790MY.Text = "0";
            chkControlaStock_790MY.Checked = true;
            chkActivo_790MY.Checked = true;
        }

        private void chkControlaStock_790MY_CheckedChanged(object sender, EventArgs e)
        {
            nudStock_790MY.Enabled = _modo != ModoEdicion_790MY.Reposo && chkControlaStock_790MY.Checked;
        }

        private void btnAnadir_790MY_Click(object sender, EventArgs e)
        {
            dgvProductos_790MY.ClearSelection();
            LimpiarCampos();
            EstablecerModo(ModoEdicion_790MY.Alta);
            txtNombre_790MY.Focus();
        }

        private void btnModificar_790MY_Click(object sender, EventArgs e)
        {
            if (Seleccionado == null) return;
            EstablecerModo(ModoEdicion_790MY.Modificacion);
            txtNombre_790MY.Focus();
        }

        private void btnCancelar_790MY_Click(object sender, EventArgs e)
        {
            EstablecerModo(ModoEdicion_790MY.Reposo);
        }

        private void btnGuardar_790MY_Click(object sender, EventArgs e)
        {
            var categoria = (cmbCategoria_790MY.SelectedItem as OpcionCombo_790MY<CategoriaProducto_790MY>)?.Valor
                            ?? CategoriaProducto_790MY.PlatoPrincipal;

            var producto = new Producto_790MY(
                _modo == ModoEdicion_790MY.Modificacion ? Seleccionado?.ProductoID ?? 0 : 0,
                txtNombre_790MY.Text,
                nudPrecio_790MY.Value,
                (int)nudStock_790MY.Value,
                chkActivo_790MY.Checked)
            {
                Categoria = categoria,
                ControlaStock = chkControlaStock_790MY.Checked
            };

            try
            {
                if (_modo == ModoEdicion_790MY.Alta)
                    _productoBll.RegistrarProducto(producto);
                else
                    _productoBll.ModificarProducto(producto);
            }
            catch (Exception ex)
            {
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
                return;
            }

            string exito = E.T(_modo == ModoEdicion_790MY.Alta ? "FP_msg_alta_ok" : "FP_msg_modificacion_ok", producto.Nombre);
            if (_modo == ModoEdicion_790MY.Alta)
            {
                // Alta (E2-D4): la ficha queda limpia y con foco en el nombre para cargar otro producto.
                _modo = ModoEdicion_790MY.Reposo;
                Cargar();
                dgvProductos_790MY.ClearSelection();
                LimpiarCampos();
                EstablecerModo(ModoEdicion_790MY.Alta);
                txtNombre_790MY.Focus();
            }
            else
            {
                // Modificación (E2-D4): queda elegido el producto con los datos guardados.
                _modo = ModoEdicion_790MY.Reposo;
                Cargar(producto.ProductoID);
                EstablecerModo(ModoEdicion_790MY.Reposo);
            }
            E.MostrarExitoToast(dgvProductos_790MY, exito);
        }

        private void btnBaja_790MY_Click(object sender, EventArgs e)
        {
            var p = Seleccionado;
            if (p == null) return;

            bool confirmado = ConfirmModal_790MY.Confirmar(VentanaPropietaria,
                E.T("FP_confirmar_baja_titulo"),
                E.T("FP_msg_confirmar_baja", p.Nombre),
                E.T("FP_btnBaja"));
            if (!confirmado) return;

            try
            {
                _productoBll.DarDeBaja(p.ProductoID);
            }
            catch (Exception ex)
            {
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
                return;
            }
            Cargar(p.ProductoID);
        }

        #endregion

        #region Estética e i18n

        private void pnlDetalle_790MY_Paint(object sender, PaintEventArgs e)
        {
            using (var linea = new Pen(E.BordeCard))
                e.Graphics.DrawLine(linea, 0, 0, 0, pnlDetalle_790MY.Height);
        }

        public void UpdateIdioma()
        {
            E.TraducirTags(this);
            this.Text = E.T("FP_titulo");
            foreach (DataGridViewColumn col in dgvProductos_790MY.Columns)
                if (col.Tag is string clave) col.HeaderText = E.T(clave);
            SendMessage(txtBuscar_790MY.Handle, EM_SETCUEBANNER, (IntPtr)1, E.T("FP_buscar_placeholder"));

            if (_productoBll == null) return;
            CargarCombosFijos();
            if (_modo == ModoEdicion_790MY.Reposo)
            {
                RenderizarGrilla(Seleccionado?.ProductoID);
                EstablecerModo(ModoEdicion_790MY.Reposo);
            }
        }

        #endregion

        #region Ayuda contextual (Etapa 5)
        /// <summary>Sugerencias del Chef (F1): solo datos; el globo y el resalte los dibuja AyudaSugerencias_790MY.</summary>
        public IReadOnlyList<PistaAyuda_790MY> ObtenerPistas() => new[]
        {
            new PistaAyuda_790MY(nameof(txtBuscar_790MY), "AY_sug_pr1_t", "AY_sug_pr1_x"),
            new PistaAyuda_790MY(nameof(dgvProductos_790MY), "AY_sug_pr2_t", "FP_ayuda"),
            new PistaAyuda_790MY(nameof(nudStock_790MY), "AY_sug_pr3_t", "FP_lblStockAyuda"),
            new PistaAyuda_790MY(nameof(chkActivo_790MY), "AY_sug_pr4_t", "FP_lblActivoAyuda"),
        };
        #endregion
    }
}
