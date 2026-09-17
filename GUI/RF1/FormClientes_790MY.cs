using BE_08YS;
using BLL_08YS;
using BLL_08YS.Exceptions;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_08YS.RF1
{
    public partial class FormClientes_790MY : Form
    {
        private static readonly Dictionary<string, Permisos> _mapaPermisos =
            new Dictionary<string, Permisos>
            {
                { nameof(btnRegistrar_790MY), Permisos.CrearCliente }
            };

        private readonly ClienteBLL_790MY _clienteBll;

        // Modo "registro rapido": se activa cuando el formulario se abre desde otro
        // flujo (ej. Registrar Reserva) via ShowDialog para dar de alta un cliente
        // sobre la marcha. En ese modo no se muestra la grilla (no es la pantalla
        // de ABM completa) y al registrar con exito se cierra devolviendo DialogResult.OK.
        private readonly bool _modoRegistroRapido;

        public Cliente_790MY ClienteRegistrado { get; private set; }

        public FormClientes_790MY()
        {
            InitializeComponent();
            _clienteBll = BLLFactory_790MY.CreateClienteBLL();
        }

        public FormClientes_790MY(int dniPrellenado) : this()
        {
            _modoRegistroRapido = true;
            txtDni_790MY.Text = dniPrellenado.ToString();
            txtDni_790MY.Enabled = false;
            dgvClientes_790MY.Visible = false;
        }

        private void FormClientes_790MY_Load(object sender, EventArgs e)
        {
            PermissionFilter_08YS.Aplicar(this, _mapaPermisos);

            if (!_modoRegistroRapido)
                CargarGrilla();
        }

        private void CargarGrilla()
        {
            dgvClientes_790MY.DataSource = null;
            dgvClientes_790MY.DataSource = _clienteBll.GetAll();
        }

        private void LimpiarCampos()
        {
            txtDni_790MY.Clear();
            txtNombre_790MY.Clear();
            txtApellido_790MY.Clear();
            txtEmail_790MY.Clear();
            txtTelefono_790MY.Clear();
            txtDni_790MY.Focus();
        }

        private void btnLimpiar_790MY_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void btnRegistrar_790MY_Click(object sender, EventArgs e)
        {
            // Todo lo demás (duplicados, formato de email, campos vacíos) lo valida la BLL.
            if (string.IsNullOrWhiteSpace(txtDni_790MY.Text))
            {
                MessageBox.Show("Ingrese el DNI del cliente.", "Datos incompletos",
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtDni_790MY.Text.Trim(), out int dni))
            {
                MessageBox.Show("El DNI debe ser un valor numérico.", "Dato inválido",
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nombre = txtNombre_790MY.Text.Trim();
            string apellido = txtApellido_790MY.Text.Trim();
            string email = txtEmail_790MY.Text.Trim();
            string telefono = txtTelefono_790MY.Text.Trim();

            try
            {
                _clienteBll.RegistrarCliente(dni, nombre, apellido, email, telefono);

                MessageBox.Show("Cliente registrado correctamente.", "Éxito",
                                 MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (_modoRegistroRapido)
                {
                    ClienteRegistrado = new Cliente_790MY(dni, nombre, apellido, email, telefono);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    LimpiarCampos();
                    CargarGrilla();
                }
            }
            catch (ClienteDniDuplicadoException_790MY ex)
            {
                // Regla de negocio incumplida: DNI duplicado
                MessageBox.Show(ex.Message, "Operación no permitida",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException ex)
            {
                // Dato inválido según la BLL
                MessageBox.Show(ex.Message, "Datos inválidos",
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception)
            {
                // Última barrera: no exponemos detalles técnicos al usuario.
                MessageBox.Show("Ocurrió un error inesperado al registrar el cliente.", "Error",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
