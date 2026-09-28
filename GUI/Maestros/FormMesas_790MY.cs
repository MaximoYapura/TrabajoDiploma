using BE_08YS;
using BLL_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Service_08YS;

namespace GUI_08YS.Maestros
{
    public partial class FormMesas_790MY : Form, IIdiomaObserver_08YS
    {
        private enum ModoEdicion { Reposo, Alta, Modificacion }

        private static readonly Dictionary<string, Permisos> _mapaPermisos =
            new Dictionary<string, Permisos>
            {
                { nameof(btnAnadir_790MY), Permisos.CrearMesa },
                { nameof(btnModificar_790MY), Permisos.CrearMesa },
                { nameof(btnEliminar_790MY), Permisos.CrearMesa }
            };

        private readonly MesaBLL_790MY _mesaBll;
        private ModoEdicion _modo = ModoEdicion.Reposo;

        // Estado de la mesa que se esta editando: no es un campo del formulario
        // (solo se editan Número y Capacidad), pero hay que preservarlo al modificar
        // para no pisarlo con un valor por defecto.
        private EstadoMesa_790MY _estadoEdicion = EstadoMesa_790MY.Libre;

        public FormMesas_790MY()
        {
            InitializeComponent();
            _mesaBll = BLLFactory_790MY.CreateMesaBLL();
            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) => TraductorManager_08YS.Instance.Desuscribir(this);
            this.Text = TraductorManager_08YS.Instance.GetTexto("FM_titulo");

            // Icono corporativo (seguro: el recurso puede no existir aún en la Build actual)
            try
            {
                var icon = global::GUI_08YS.Properties.Resources.ResourceManager
                               .GetObject("FogonIcon") as System.Drawing.Icon;
                if (icon != null) this.Icon = icon;
            }
            catch { }
        }

        private void FormMesas_790MY_Load(object sender, EventArgs e)
        {
            PermissionFilter_08YS.Aplicar(this, _mapaPermisos);
            CargarGrilla();
            EstablecerModo(ModoEdicion.Reposo);
        }

        private void CargarGrilla()
        {
            dgvMesas_790MY.DataSource = null;
            dgvMesas_790MY.DataSource = _mesaBll.GetAll();
            dgvMesas_790MY.Columns[3].Visible = false; // Ocultar columna Estado (solo se muestra en detalle)
        }

        private void dgvMesas_790MY_SelectionChanged(object sender, EventArgs e)
        {
            if (_modo == ModoEdicion.Reposo)
                ActualizarHabilitacionBotonesAccion();
        }

        private void ActualizarHabilitacionBotonesAccion()
        {
            bool haySeleccion = dgvMesas_790MY.CurrentRow?.DataBoundItem is Mesa_790MY;
            btnModificar_790MY.Enabled = haySeleccion;
            btnEliminar_790MY.Enabled = haySeleccion;
        }

        private void EstablecerModo(ModoEdicion modo)
        {
            _modo = modo;
            bool editando = modo != ModoEdicion.Reposo;

            // NroMesa siempre es de solo lectura: se auto-asigna en Alta
            // y no se permite cambiar la PK en Modificación.
            txtNumero_790MY.Enabled  = editando;
            txtNumero_790MY.ReadOnly = true;

            nudCapacidad_790MY.Enabled = editando;

            btnAplicar_790MY.Enabled  = editando;
            btnCancelar_790MY.Enabled = editando;

            dgvMesas_790MY.Enabled  = !editando;
            btnAnadir_790MY.Enabled = !editando;

            if (!editando)
                ActualizarHabilitacionBotonesAccion();
            else
                btnModificar_790MY.Enabled = btnEliminar_790MY.Enabled = false;
        }

        private void LimpiarCampos()
        {
            txtNumero_790MY.Clear();
            nudCapacidad_790MY.Value = 1;
            _estadoEdicion = EstadoMesa_790MY.Libre;
        }

        private void btnAnadir_790MY_Click(object sender, EventArgs e)
        {
            LimpiarCampos();

            // Auto-asignar el próximo número disponible (Max + 1)
            try
            {
                int siguiente = _mesaBll.GetProximoNumero();
                txtNumero_790MY.Text = siguiente.ToString();
            }
            catch
            {
                txtNumero_790MY.Text = "1";
            }

            EstablecerModo(ModoEdicion.Alta);
            nudCapacidad_790MY.Focus();
        }

        private void btnModificar_790MY_Click(object sender, EventArgs e)
        {
            if (!(dgvMesas_790MY.CurrentRow?.DataBoundItem is Mesa_790MY seleccionada))
                return;

            txtNumero_790MY.Text       = seleccionada.NroMesa.ToString();
            nudCapacidad_790MY.Value   = seleccionada.Capacidad;
            _estadoEdicion             = seleccionada.Estado;

            EstablecerModo(ModoEdicion.Modificacion);
        }

        private void btnEliminar_790MY_Click(object sender, EventArgs e)
        {
            if (!(dgvMesas_790MY.CurrentRow?.DataBoundItem is Mesa_790MY seleccionada))
                return;

            var t = TraductorManager_08YS.Instance;
            var respuesta = MessageBox.Show(
                string.Format(t.GetTexto("msg_fm_confirmar_baja_mesa"), seleccionada.NroMesa),
                t.GetTexto("titulo_confirmar_baja"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes) return;

            try
            {
                _mesaBll.EliminarMesa(seleccionada.NroMesa);
                MessageBox.Show(t.GetTexto("msg_fm_baja_ok"), t.GetTexto("exito"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGrilla();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, t.GetTexto("no_se_puede_eliminar"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception)
            {
                MessageBox.Show(t.GetTexto("msg_fm_error_eliminar"), t.GetTexto("error"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAplicar_790MY_Click(object sender, EventArgs e)
        {
            var t = TraductorManager_08YS.Instance;

            if (!int.TryParse(txtNumero_790MY.Text.Trim(), out int numero))
            {
                MessageBox.Show(t.GetTexto("msg_fm_nro_invalido"), t.GetTexto("titulo_dato_invalido"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int capacidad = (int)nudCapacidad_790MY.Value;

            try
            {
                if (_modo == ModoEdicion.Alta)
                {
                    _mesaBll.RegistrarMesa(numero, capacidad);
                    MessageBox.Show(t.GetTexto("msg_fm_registrada_ok"), t.GetTexto("exito"),
                                     MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (_modo == ModoEdicion.Modificacion)
                {
                    var mesa = new Mesa_790MY(numero, capacidad, _estadoEdicion);
                    _mesaBll.ModificarMesa(mesa);
                    MessageBox.Show(t.GetTexto("msg_fm_modificada_ok"), t.GetTexto("exito"),
                                     MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LimpiarCampos();
                CargarGrilla();
                EstablecerModo(ModoEdicion.Reposo);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, t.GetTexto("titulo_datos_invalidos"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception)
            {
                MessageBox.Show(t.GetTexto("msg_fm_error_guardar"), t.GetTexto("error"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_790MY_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            EstablecerModo(ModoEdicion.Reposo);
        }

        private void btnSalir_790MY_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region i18n
        public void UpdateIdioma()
        {
            TraducirControles(this);

            if (dgvMesas_790MY.Columns.Count > 0)
            {
                if (dgvMesas_790MY.Columns.Contains("FM_colNumero"))
                    dgvMesas_790MY.Columns["FM_colNumero"].HeaderText = TraductorManager_08YS.Instance.GetTexto("FM_colNumero");
                else if (dgvMesas_790MY.Columns.Contains("Número"))
                    dgvMesas_790MY.Columns["Número"].HeaderText = TraductorManager_08YS.Instance.GetTexto("FM_colNumero");

              
                if (dgvMesas_790MY.Columns.Count >= 3)
                {
                    dgvMesas_790MY.Columns[0].HeaderText = TraductorManager_08YS.Instance.GetTexto("FM_colNumero");
                    dgvMesas_790MY.Columns[1].HeaderText = TraductorManager_08YS.Instance.GetTexto("FM_colCapacidad");
                    dgvMesas_790MY.Columns[2].HeaderText = TraductorManager_08YS.Instance.GetTexto("FM_colEstado");
                }
            }
            
            this.Text = TraductorManager_08YS.Instance.GetTexto("FM_titulo");
        }

        private void TraducirControles(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                // Omitir TextBox y RichTextBox: son entradas editables por el usuario
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
