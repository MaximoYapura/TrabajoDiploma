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
        public FormClientes_790MY()
        {
            InitializeComponent();
            _clienteBll = BLLFactory_08YS.CreateClienteBLL();
        }

        private void FormClientes_790MY_Load(object sender, EventArgs e)
        {
            PermissionFilter_08YS.Aplicar(this, _mapaPermisos);
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

            try
            {
                _clienteBll.RegistrarCliente(
                    dni,
                    txtNombre_790MY.Text.Trim(),
                    txtApellido_790MY.Text.Trim(),
                    txtEmail_790MY.Text.Trim(),
                    txtTelefono_790MY.Text.Trim());

                MessageBox.Show("Cliente registrado correctamente.", "Éxito",
                                 MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCampos();
                CargarGrilla();
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
                // Nota: esto también captura cualquier fallo de SessionManager_08YS.ValidatePermission
                // (no se agregó un catch específico porque no se confirmó el tipo exacto que lanza).
                MessageBox.Show("Ocurrió un error inesperado al registrar el cliente.", "Error",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
