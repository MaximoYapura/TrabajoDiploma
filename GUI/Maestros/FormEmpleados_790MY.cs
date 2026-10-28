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
    /// ABM de empleados (RF2). El empleado es independiente de la seguridad: el vínculo
    /// con un usuario del sistema no se gestiona desde acá. Validaciones, DV y bitácora los resuelve
    /// EmpleadoBLL_790MY (patente CrearEmpleado).
    /// </summary>
    public partial class FormEmpleados_790MY : Form, IIdiomaObserver_08YS, IAyudaContextual_790MY
    {
        private static readonly Dictionary<string, Permisos> _mapaPermisos =
            new Dictionary<string, Permisos>
            {
                { nameof(btnAnadir_790MY),    Permisos.CrearEmpleado },
                { nameof(btnModificar_790MY), Permisos.CrearEmpleado },
                { nameof(btnBaja_790MY),      Permisos.CrearEmpleado },
            };

        private readonly EmpleadoBLL_790MY _empleadoBll;
        private List<Empleado_790MY> _empleados = new List<Empleado_790MY>();
        private ModoEdicion_790MY _modo = ModoEdicion_790MY.Reposo;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        public FormEmpleados_790MY()
        {
            InitializeComponent();
            _empleadoBll = BLLFactory_790MY.CreateEmpleadoBLL();

            EstiloMaestros_790MY.EstilizarGrilla(dgvEmpleados_790MY);
            EstiloMaestros_790MY.EstilizarBoton(btnAnadir_790MY, true);
            EstiloMaestros_790MY.EstilizarBoton(btnModificar_790MY, false);
            EstiloMaestros_790MY.EstilizarBoton(btnBaja_790MY, false);
            EstiloMaestros_790MY.EstilizarBoton(btnGuardar_790MY, true);
            EstiloMaestros_790MY.EstilizarBoton(btnCancelar_790MY, false);

            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) => TraductorManager_08YS.Instance.Desuscribir(this);
        }

        private IWin32Window VentanaPropietaria => (this.TopLevelControl as Form) ?? (IWin32Window)this;

        private Empleado_790MY Seleccionado
            => dgvEmpleados_790MY.CurrentRow?.Tag as Empleado_790MY;

        private void FormEmpleados_790MY_Load(object sender, EventArgs e)
        {
            PermissionFilter_08YS.Aplicar(this, _mapaPermisos);
            UpdateIdioma();
            Cargar();
            EstablecerModo(ModoEdicion_790MY.Reposo);
        }

        #region Datos y grilla

        private void Cargar(int? seleccionarDni = null)
        {
            try
            {
                _empleados = _empleadoBll.GetAll();
            }
            catch (Exception ex)
            {
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
            }
            RenderizarGrilla(seleccionarDni);
        }

        private void Filtro_Changed(object sender, EventArgs e)
        {
            if (_modo == ModoEdicion_790MY.Reposo) RenderizarGrilla(Seleccionado?.DNI);
        }

        private void RenderizarGrilla(int? seleccionarDni)
        {
            string texto = txtBuscar_790MY.Text.Trim();
            var cargo = (cmbFiltroCargo_790MY.SelectedItem as OpcionCombo_790MY<PuestoEmpleado_790MY?>)?.Valor;

            var visibles = _empleados
                .Where(emp => texto.Length == 0
                    || emp.DNI.ToString().StartsWith(texto)
                    || emp.NombreCompleto.IndexOf(texto, StringComparison.CurrentCultureIgnoreCase) >= 0)
                .Where(emp => !cargo.HasValue || emp.Puesto == cargo.Value)
                .Where(emp => !chkSoloActivos_790MY.Checked || emp.Activo)
                .OrderBy(emp => emp.Apellido).ThenBy(emp => emp.Nombre)
                .ToList();

            dgvEmpleados_790MY.SuspendLayout();
            dgvEmpleados_790MY.Rows.Clear();
            foreach (var emp in visibles)
            {
                int fila = dgvEmpleados_790MY.Rows.Add(
                    emp.DNI.ToString("N0", TraductorManager_08YS.Instance.CulturaActual),
                    emp.Nombre,
                    emp.Apellido,
                    E.T("FE_puesto_" + emp.Puesto),
                    null);
                dgvEmpleados_790MY.Rows[fila].Tag = emp;
                if (emp.DNI == seleccionarDni)
                    dgvEmpleados_790MY.CurrentCell = dgvEmpleados_790MY.Rows[fila].Cells[0];
            }
            dgvEmpleados_790MY.ResumeLayout();

            if (!seleccionarDni.HasValue)
                dgvEmpleados_790MY.ClearSelection();

            int activos = _empleados.Count(x => x.Activo);
            lblResumen_790MY.Text = E.T("FE_resumen", _empleados.Count, visibles.Count, activos, _empleados.Count - activos);
            MostrarSeleccion();
        }

        private void dgvEmpleados_790MY_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var emp = dgvEmpleados_790MY.Rows[e.RowIndex].Tag as Empleado_790MY;
            if (emp == null) return;

            if (e.ColumnIndex == colCargo_790MY.Index)
            {
                Color fondo, texto;
                switch (emp.Puesto)
                {
                    case PuestoEmpleado_790MY.Camarero: fondo = Color.FromArgb(252, 230, 232); texto = Color.FromArgb(140, 30, 50); break;
                    case PuestoEmpleado_790MY.Chef:     fondo = E.PendFondo; texto = E.PendTexto; break;
                    default:                            fondo = Color.FromArgb(226, 234, 246); texto = Color.FromArgb(40, 70, 120); break;
                }
                EstiloMaestros_790MY.PintarPildora(e, E.T("FE_puesto_" + emp.Puesto), fondo, texto);
            }
            else if (e.ColumnIndex == colEstado_790MY.Index)
            {
                EstiloMaestros_790MY.PintarEstado(e, emp.Activo, E.T("FE_activo"), E.T("FE_inactivo"));
            }
        }

        private void dgvEmpleados_790MY_SelectionChanged(object sender, EventArgs e)
        {
            if (_modo != ModoEdicion_790MY.Reposo) return;
            // Un aviso sobre el empleado anterior (por ejemplo, una baja rechazada) ya no aplica.
            bnrAvisos_790MY.Ocultar();
            MostrarSeleccion();
        }

        private void MostrarSeleccion()
        {
            var emp = Seleccionado;
            btnModificar_790MY.Enabled = emp != null;
            btnBaja_790MY.Enabled = emp != null && emp.Activo;

            if (emp == null) { LimpiarCampos(); return; }

            txtDni_790MY.Text = emp.DNI.ToString();
            txtNombre_790MY.Text = emp.Nombre;
            txtApellido_790MY.Text = emp.Apellido;
            SeleccionarPuesto(emp.Puesto);
            chkActivo_790MY.Checked = emp.Activo;
            ActualizarAvisoMesas(emp);
        }

        /// <summary>Aviso del escenario "camarero con mesas abiertas" (no puede cambiar de cargo ni darse de baja).</summary>
        private void ActualizarAvisoMesas(Empleado_790MY emp)
        {
            int abiertas = 0;
            if (emp != null && emp.Puesto == PuestoEmpleado_790MY.Camarero && emp.Activo)
            {
                try { abiertas = _empleadoBll.ContarMesasAbiertas(emp.DNI); }
                catch (Exception) { abiertas = 0; }
            }
            lblAviso_790MY.Visible = abiertas > 0;
            lblAviso_790MY.Text = E.T("FE_aviso_mesas", abiertas);
        }

        #endregion

        #region Combos

        private void CargarCombosFijos()
        {
            int filtro = Math.Max(0, cmbFiltroCargo_790MY.SelectedIndex);
            cmbFiltroCargo_790MY.Items.Clear();
            cmbFiltroCargo_790MY.Items.Add(new OpcionCombo_790MY<PuestoEmpleado_790MY?> { Valor = null, Texto = E.T("FE_todos_cargos") });

            object puestoActual = (cmbCargo_790MY.SelectedItem as OpcionCombo_790MY<PuestoEmpleado_790MY>)?.Valor;
            cmbCargo_790MY.Items.Clear();

            foreach (PuestoEmpleado_790MY p in Enum.GetValues(typeof(PuestoEmpleado_790MY)))
            {
                cmbFiltroCargo_790MY.Items.Add(new OpcionCombo_790MY<PuestoEmpleado_790MY?> { Valor = p, Texto = E.T("FE_puesto_" + p) });
                cmbCargo_790MY.Items.Add(new OpcionCombo_790MY<PuestoEmpleado_790MY> { Valor = p, Texto = E.T("FE_puesto_" + p) });
            }

            cmbFiltroCargo_790MY.SelectedIndexChanged -= Filtro_Changed;
            cmbFiltroCargo_790MY.SelectedIndex = Math.Min(filtro, cmbFiltroCargo_790MY.Items.Count - 1);
            cmbFiltroCargo_790MY.SelectedIndexChanged += Filtro_Changed;

            if (puestoActual is PuestoEmpleado_790MY puesto) SeleccionarPuesto(puesto);
        }

        private void SeleccionarPuesto(PuestoEmpleado_790MY puesto)
        {
            cmbCargo_790MY.SelectedItem = cmbCargo_790MY.Items.Cast<OpcionCombo_790MY<PuestoEmpleado_790MY>>()
                .FirstOrDefault(o => o.Valor == puesto);
        }

        #endregion

        #region Modo de edición

        private void EstablecerModo(ModoEdicion_790MY modo)
        {
            _modo = modo;
            bool editando = modo != ModoEdicion_790MY.Reposo;

            txtDni_790MY.ReadOnly = modo != ModoEdicion_790MY.Alta;
            txtNombre_790MY.ReadOnly = txtApellido_790MY.ReadOnly = !editando;
            cmbCargo_790MY.Enabled = chkActivo_790MY.Enabled = editando;

            btnGuardar_790MY.Visible = btnCancelar_790MY.Visible = editando;
            btnAnadir_790MY.Enabled = !editando;
            dgvEmpleados_790MY.Enabled = !editando;
            txtBuscar_790MY.Enabled = cmbFiltroCargo_790MY.Enabled = chkSoloActivos_790MY.Enabled = !editando;

            string clave = modo == ModoEdicion_790MY.Alta ? "FE_modo_alta" : modo == ModoEdicion_790MY.Modificacion ? "FE_modo_modificacion" : "FE_modo_consulta";
            chipModo_790MY.Text = E.T(clave);
            chipModo_790MY.BackColor = editando ? E.BordoSuave : Color.FromArgb(238, 232, 222);
            chipModo_790MY.ForeColor = editando ? E.Bordo : Color.FromArgb(80, 72, 66);
            lblModo_790MY.Text = E.T("FE_modo", chipModo_790MY.Text);

            bnrAvisos_790MY.Ocultar();
            errValidacion_790MY.Clear();

            if (editando)
            {
                btnModificar_790MY.Enabled = btnBaja_790MY.Enabled = false;
            }
            else
            {
                MostrarSeleccion();
            }
        }

        private void LimpiarCampos()
        {
            txtDni_790MY.Clear();
            txtNombre_790MY.Clear();
            txtApellido_790MY.Clear();
            SeleccionarPuesto(PuestoEmpleado_790MY.Camarero);
            chkActivo_790MY.Checked = true;
            lblAviso_790MY.Visible = false;
        }

        private void btnAnadir_790MY_Click(object sender, EventArgs e)
        {
            dgvEmpleados_790MY.ClearSelection();
            LimpiarCampos();
            EstablecerModo(ModoEdicion_790MY.Alta);
            txtDni_790MY.Focus();
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
            errValidacion_790MY.SetError(txtDni_790MY, string.Empty);
            if (!int.TryParse(txtDni_790MY.Text.Trim(), out int dni))
            {
                string mensaje = E.T("msg_rf2_empleado_dni_invalido");
                errValidacion_790MY.SetError(txtDni_790MY, mensaje);
                bnrAvisos_790MY.Mostrar(mensaje);
                txtDni_790MY.Focus();
                return;
            }

            var puesto = (cmbCargo_790MY.SelectedItem as OpcionCombo_790MY<PuestoEmpleado_790MY>)?.Valor ?? PuestoEmpleado_790MY.Camarero;
            // UsuarioDNI no se envía: el alta queda sin usuario y la DAL no lo modifica al
            // actualizar, así se conserva el vínculo existente (p. ej. el del administrador).
            var empleado = new Empleado_790MY(dni, txtNombre_790MY.Text, txtApellido_790MY.Text, puesto, activo: chkActivo_790MY.Checked);

            try
            {
                if (_modo == ModoEdicion_790MY.Alta)
                    _empleadoBll.RegistrarEmpleado(empleado);
                else
                    _empleadoBll.ModificarEmpleado(empleado);
            }
            catch (Exception ex)
            {
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
                return;
            }

            string exito = E.T(_modo == ModoEdicion_790MY.Alta ? "FE_msg_alta_ok" : "FE_msg_modificacion_ok", empleado.NombreCompleto);
            if (_modo == ModoEdicion_790MY.Alta)
            {
                // Alta (E2-D4): la ficha queda limpia y con foco en el DNI para cargar otro empleado.
                _modo = ModoEdicion_790MY.Reposo;
                Cargar();
                dgvEmpleados_790MY.ClearSelection();
                LimpiarCampos();
                EstablecerModo(ModoEdicion_790MY.Alta);
                txtDni_790MY.Focus();
            }
            else
            {
                // Modificación (E2-D4): queda elegido el empleado con los datos guardados.
                _modo = ModoEdicion_790MY.Reposo;
                Cargar(dni);
                EstablecerModo(ModoEdicion_790MY.Reposo);
            }
            E.MostrarExitoToast(dgvEmpleados_790MY, exito);
        }

        private void btnBaja_790MY_Click(object sender, EventArgs e)
        {
            var emp = Seleccionado;
            if (emp == null) return;

            bool confirmado = ConfirmModal_790MY.Confirmar(VentanaPropietaria,
                E.T("FE_confirmar_baja_titulo"),
                E.T("FE_msg_confirmar_baja", emp.NombreCompleto),
                E.T("FE_btnBaja"));
            if (!confirmado) return;

            try
            {
                _empleadoBll.DarDeBaja(emp.DNI);
            }
            catch (Exception ex)
            {
                E.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
                return;
            }
            Cargar(emp.DNI);
        }

        private void txtDni_790MY_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
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
            this.Text = E.T("FE_titulo");
            foreach (DataGridViewColumn col in dgvEmpleados_790MY.Columns)
                if (col.Tag is string clave) col.HeaderText = E.T(clave);
            SendMessage(txtBuscar_790MY.Handle, EM_SETCUEBANNER, (IntPtr)1, E.T("FE_buscar_placeholder"));

            if (_empleadoBll == null) return;
            CargarCombosFijos();
            if (_modo == ModoEdicion_790MY.Reposo)
            {
                RenderizarGrilla(Seleccionado?.DNI);
                EstablecerModo(ModoEdicion_790MY.Reposo);
            }
        }

        #endregion

        #region Ayuda contextual (Etapa 5)
        /// <summary>Sugerencias del Chef (F1): solo datos; el globo y el resalte los dibuja AyudaSugerencias_790MY.</summary>
        public IReadOnlyList<PistaAyuda_790MY> ObtenerPistas() => new[]
        {
            new PistaAyuda_790MY(nameof(txtBuscar_790MY), "AY_sug_em1_t", "AY_sug_em1_x"),
            new PistaAyuda_790MY(nameof(dgvEmpleados_790MY), "AY_sug_em2_t", "FE_ayuda"),
            new PistaAyuda_790MY(nameof(btnAnadir_790MY), "AY_sug_em3_t", "AY_sug_em3_x"),
            new PistaAyuda_790MY(nameof(btnBaja_790MY), "AY_sug_em4_t", "AY_sug_em4_x"),
        };
        #endregion
    }
}
