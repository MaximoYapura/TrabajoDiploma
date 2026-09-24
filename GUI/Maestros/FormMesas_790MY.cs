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

            txtNumero_790MY.Enabled = modo == ModoEdicion.Alta;
            txtCapacidad_790MY.Enabled = editando;

            btnAplicar_790MY.Enabled = editando;
            btnCancelar_790MY.Enabled = editando;

            dgvMesas_790MY.Enabled = !editando;
            btnAnadir_790MY.Enabled = !editando;

            if (!editando)
                ActualizarHabilitacionBotonesAccion();
            else
                btnModificar_790MY.Enabled = btnEliminar_790MY.Enabled = false;
        }

        private void LimpiarCampos()
        {
            txtNumero_790MY.Clear();
            txtCapacidad_790MY.Clear();
            _estadoEdicion = EstadoMesa_790MY.Libre;
        }

        private void btnAnadir_790MY_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            EstablecerModo(ModoEdicion.Alta);
            txtNumero_790MY.Focus();
        }

        private void btnModificar_790MY_Click(object sender, EventArgs e)
        {
            if (!(dgvMesas_790MY.CurrentRow?.DataBoundItem is Mesa_790MY seleccionada))
                return;

            txtNumero_790MY.Text = seleccionada.NroMesa.ToString();
            txtCapacidad_790MY.Text = seleccionada.Capacidad.ToString();
            _estadoEdicion = seleccionada.Estado;

            EstablecerModo(ModoEdicion.Modificacion);
        }

        private void btnEliminar_790MY_Click(object sender, EventArgs e)
        {
            if (!(dgvMesas_790MY.CurrentRow?.DataBoundItem is Mesa_790MY seleccionada))
                return;

            var respuesta = MessageBox.Show(
                $"¿Confirma dar de baja la mesa Nro. {seleccionada.NroMesa}? " +
                "Esto no afecta las reservas ya registradas para esa mesa.",
                "Confirmar baja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes) return;

            try
            {
                _mesaBll.EliminarMesa(seleccionada.NroMesa);
                MessageBox.Show("Mesa dada de baja correctamente.", "Éxito",
                                 MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarGrilla();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "No se pudo eliminar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception)
            {
                MessageBox.Show("Ocurrió un error inesperado al eliminar la mesa.", "Error",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAplicar_790MY_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtNumero_790MY.Text.Trim(), out int numero))
            {
                MessageBox.Show("El número de mesa debe ser un valor numérico.", "Dato inválido",
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtCapacidad_790MY.Text.Trim(), out int capacidad))
            {
                MessageBox.Show("La capacidad debe ser un valor numérico.", "Dato inválido",
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_modo == ModoEdicion.Alta)
                {
                    _mesaBll.RegistrarMesa(numero, capacidad);
                    MessageBox.Show("Mesa registrada correctamente.", "Éxito",
                                     MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (_modo == ModoEdicion.Modificacion)
                {
                    var mesa = new Mesa_790MY(numero, capacidad, _estadoEdicion);
                    _mesaBll.ModificarMesa(mesa);
                    MessageBox.Show("Mesa modificada correctamente.", "Éxito",
                                     MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LimpiarCampos();
                CargarGrilla();
                EstablecerModo(ModoEdicion.Reposo);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception)
            {
                MessageBox.Show("Ocurrió un error inesperado al guardar la mesa.", "Error",
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
            AplicarTraducciones(this);
        }

        private void AplicarTraducciones(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                if (c.Tag is string clave && !string.IsNullOrWhiteSpace(clave))
                    c.Text = TraductorManager_08YS.Instance.GetTexto(clave);
                if (c.HasChildren)
                    AplicarTraducciones(c);
            }
        }
        #endregion
    }
}
