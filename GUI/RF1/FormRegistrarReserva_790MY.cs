using BE_08YS;
using Service_08YS;
using BLL_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GUI_08YS.Maestros;
namespace GUI_08YS.RF1
{
    public partial class FormRegistrarReserva_790MY : Form, IIdiomaObserver_08YS
    {
        private readonly ClienteBLL_790MY _clienteBll;
        private readonly ReservaBLL_790MY _reservaBll;

        private Cliente_790MY _clienteActual;
        private Mesa_790MY _mesaSeleccionada;

        private static readonly Dictionary<string, Permisos> _mapaPermisos =
            new Dictionary<string, Permisos>
            {
                { nameof(btnConfirmar_790MY), Permisos.RegistrarReserva },
            };

        private class TurnoItem
        {
            public string Display { get; set; }
            public TimeSpan Hora { get; set; }
            public override string ToString() => Display;
        }

        public FormRegistrarReserva_790MY()
        {
            InitializeComponent();

            _clienteBll = BLLFactory_790MY.CreateClienteBLL();
            _reservaBll = BLLFactory_790MY.CreateReservaBLL();
            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) => TraductorManager_08YS.Instance.Desuscribir(this);
        }

        private void FormRegistrarReserva_790MY_Load(object sender, EventArgs e)
        {
            PermissionFilter_08YS.Aplicar(this, _mapaPermisos);

            dtpFecha_790MY.MinDate = DateTime.Today;
            dtpFecha_790MY.Value = DateTime.Today;

            cmbHora_790MY.DisplayMember = "Display";
            cmbHora_790MY.Items.Add(new TurnoItem { Display = "18:00 - 20:00", Hora = new TimeSpan(18, 0, 0) });
            cmbHora_790MY.Items.Add(new TurnoItem { Display = "20:00 - 22:00", Hora = new TimeSpan(20, 0, 0) });
            cmbHora_790MY.Items.Add(new TurnoItem { Display = "22:00 - 24:00", Hora = new TimeSpan(22, 0, 0) });

            nudComensales_790MY.Minimum = 1;
            nudComensales_790MY.Maximum = 20;
            nudComensales_790MY.Value = 2;

            LimpiarFormulario();
        }

        // Si cambia la fecha, el turno o la cantidad de comensales después de haber
        // elegido una mesa, esa mesa deja de ser válida para el nuevo contexto:
        // se limpia la selección para forzar a elegir de nuevo.
        private void InvalidarMesaSeleccionada(object sender, EventArgs e)
        {
            if (_mesaSeleccionada == null) return;

            _mesaSeleccionada = null;
            lblMesaSeleccionada_790MY.Text = TraductorManager_08YS.Instance.GetTexto("RR_lblMesaSinSeleccionar");
        }

        private void btnBuscarCliente_790MY_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtDniCliente_790MY.Text.Trim(), out int dni))
            {
                MessageBox.Show(
                    TraductorManager_08YS.Instance.GetTexto("msg_rr_dni_invalido"),
                    TraductorManager_08YS.Instance.GetTexto("titulo_dato_invalido"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Escenario 1: cliente activo — GetByDni filtra por Activo = 1.
                _clienteActual = _clienteBll.GetByDni(dni);

                if (_clienteActual != null)
                {
                    MostrarCliente(_clienteActual);
                    return;
                }

                // Escenario 2: DNI existe en BD pero inactivo (baja lógica).
                // Exists no filtra por Activo, por lo que devuelve true en ese caso.
                if (_clienteBll.Exists(dni))
                {
                    MessageBox.Show(
                        TraductorManager_08YS.Instance.GetTexto("msg_rr_cliente_inactivo"),
                        TraductorManager_08YS.Instance.GetTexto("titulo_cliente_inactivo"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Escenario 3: DNI no existe en absoluto — ofrecer registro rápido.
                var respuesta = MessageBox.Show(
                    TraductorManager_08YS.Instance.GetTexto("msg_rr_cliente_no_encontrado_preg"),
                    TraductorManager_08YS.Instance.GetTexto("titulo_cliente_no_encontrado"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (respuesta != DialogResult.Yes) return;

                using (var frmCliente = new FormClientes_790MY(dni))
                {
                    if (frmCliente.ShowDialog(this.FindForm()) == DialogResult.OK)
                    {
                        _clienteActual = frmCliente.ClienteRegistrado;
                        MostrarCliente(_clienteActual);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(TraductorManager_08YS.Instance.GetTexto("msg_rr_error_buscar_cliente"), ex.Message),
                    TraductorManager_08YS.Instance.GetTexto("error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarCliente(Cliente_790MY cliente)
        {
            lblClienteInfo_790MY.Text = cliente != null
                ? $"{cliente.Nombre} {cliente.Apellido}"
                : TraductorManager_08YS.Instance.GetTexto("RR_lblClienteNoEncontrado");
        }

        private void btnSeleccionarMesa_790MY_Click(object sender, EventArgs e)
        {
            if (cmbHora_790MY.SelectedItem == null)
            {
                MessageBox.Show(
                    TraductorManager_08YS.Instance.GetTexto("msg_rr_falta_turno"),
                    TraductorManager_08YS.Instance.GetTexto("titulo_falta_turno"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TimeSpan hora = ((TurnoItem)cmbHora_790MY.SelectedItem).Hora;
            int comensales = (int)nudComensales_790MY.Value;

            using (var frmMesa = new FormSeleccionarMesa_790MY(dtpFecha_790MY.Value, hora, comensales))
            {
                if (frmMesa.ShowDialog(this.FindForm()) == DialogResult.OK)
                {
                    _mesaSeleccionada = frmMesa.MesaSeleccionada;
                    lblMesaSeleccionada_790MY.Text = string.Format(
                        TraductorManager_08YS.Instance.GetTexto("RR_lblMesaSeleccionada"),
                        _mesaSeleccionada.NroMesa, _mesaSeleccionada.Capacidad);
                }
            }
        }

        private void btnConfirmar_790MY_Click(object sender, EventArgs e)
        {
            if (_clienteActual == null)
            {
                MessageBox.Show(
                    TraductorManager_08YS.Instance.GetTexto("msg_rr_falta_cliente"),
                    TraductorManager_08YS.Instance.GetTexto("titulo_falta_cliente"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_mesaSeleccionada == null)
            {
                MessageBox.Show(
                    TraductorManager_08YS.Instance.GetTexto("msg_rr_falta_mesa"),
                    TraductorManager_08YS.Instance.GetTexto("titulo_falta_mesa_sel"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbHora_790MY.SelectedItem == null)
            {
                MessageBox.Show(
                    TraductorManager_08YS.Instance.GetTexto("msg_rr_falta_turno2"),
                    TraductorManager_08YS.Instance.GetTexto("titulo_falta_turno"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TimeSpan hora = ((TurnoItem)cmbHora_790MY.SelectedItem).Hora;
            int comensales = (int)nudComensales_790MY.Value;

            try
            {
                _reservaBll.RegistrarReserva(_clienteActual.DNI, _mesaSeleccionada, dtpFecha_790MY.Value, hora, comensales);

                MessageBox.Show(
                    TraductorManager_08YS.Instance.GetTexto("msg_reserva_registrada"),
                    TraductorManager_08YS.Instance.GetTexto("exito"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarFormulario();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message,
                    TraductorManager_08YS.Instance.GetTexto("titulo_datos_invalidos"),
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(TraductorManager_08YS.Instance.GetTexto("msg_rr_error_registrar"), ex.Message),
                    TraductorManager_08YS.Instance.GetTexto("error"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFormulario()
        {
            _clienteActual = null;
            _mesaSeleccionada = null;

            txtDniCliente_790MY.Clear();
            lblClienteInfo_790MY.Text = string.Empty;
            lblMesaSeleccionada_790MY.Text = TraductorManager_08YS.Instance.GetTexto("RR_lblMesaSinSeleccionar");

            dtpFecha_790MY.Value = DateTime.Today;
            if (cmbHora_790MY.Items.Count > 0) cmbHora_790MY.SelectedIndex = -1;
            nudComensales_790MY.Value = 2;

            txtDniCliente_790MY.Focus();
        }

        #region i18n
        public void UpdateIdioma()
        {
            AplicarTraducciones(this);

            // Refrescar textos dinámicos no cubiertos por Tags: el label de mesa
            // seleccionada puede mostrar un texto formateado o el placeholder
            // "sin seleccionar", y ambos deben cambiar al cambiar de idioma.
            lblMesaSeleccionada_790MY.Text = _mesaSeleccionada != null
                ? string.Format(
                    TraductorManager_08YS.Instance.GetTexto("RR_lblMesaSeleccionada"),
                    _mesaSeleccionada.NroMesa, _mesaSeleccionada.Capacidad)
                : TraductorManager_08YS.Instance.GetTexto("RR_lblMesaSinSeleccionar");
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
