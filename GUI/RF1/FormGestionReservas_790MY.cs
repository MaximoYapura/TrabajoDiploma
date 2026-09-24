using BE_08YS;
using BLL_08YS;
using System;
using System.Windows.Forms;
using Service_08YS;

namespace GUI_08YS.RF1
{
    public partial class FormGestionReservas_790MY : Form, IIdiomaObserver_08YS
    {
        private readonly ReservaBLL_790MY _reservaBll;

        public FormGestionReservas_790MY()
        {
            InitializeComponent();
            _reservaBll = BLLFactory_790MY.CreateReservaBLL();
            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) => TraductorManager_08YS.Instance.Desuscribir(this);
        }
          

        private void FormGestionReservas_790MY_Load(object sender, EventArgs e)
        {
            dtpDesde_790MY.Checked = false;
            dtpHasta_790MY.Checked = false;

            cmbEstado_790MY.Items.Clear();
            cmbEstado_790MY.Items.Add("(Todos)");
            cmbEstado_790MY.Items.Add(EstadoReserva_790MY.Confirmada.ToString());
            cmbEstado_790MY.Items.Add(EstadoReserva_790MY.Cancelada.ToString());
            cmbEstado_790MY.SelectedIndex = 0;

            BuscarReservas();
        }

        private void btnBuscar_790MY_Click(object sender, EventArgs e)
        {
            BuscarReservas();
        }

        private void BuscarReservas()
        {
            DateTime? desde = dtpDesde_790MY.Checked ? dtpDesde_790MY.Value.Date : (DateTime?)null;
            DateTime? hasta = dtpHasta_790MY.Checked ? dtpHasta_790MY.Value.Date : (DateTime?)null;

            int? dni = null;
            if (!string.IsNullOrWhiteSpace(txtDni_790MY.Text))
            {
                if (!int.TryParse(txtDni_790MY.Text.Trim(), out int dniParseado))
                {
                    MessageBox.Show("El DNI debe ser un valor numérico.", "Dato inválido",
                                     MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                dni = dniParseado;
            }

            EstadoReserva_790MY? estado = null;
            if (cmbEstado_790MY.SelectedIndex > 0)
                estado = (EstadoReserva_790MY)Enum.Parse(typeof(EstadoReserva_790MY), cmbEstado_790MY.SelectedItem.ToString());

            try
            {
                dgvReservas_790MY.DataSource = null;
                dgvReservas_790MY.DataSource = _reservaBll.Buscar(desde, hasta, dni, estado);
                ActualizarHabilitacionCancelar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al buscar reservas: {ex.Message}", "Error",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvReservas_790MY_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarHabilitacionCancelar();
        }

        private void ActualizarHabilitacionCancelar()
        {
            bool puedeCancelar = dgvReservas_790MY.CurrentRow?.DataBoundItem is Reserva_790MY seleccionada
                                  && seleccionada.Estado == EstadoReserva_790MY.Confirmada;

            btnCancelarReserva_790MY.Enabled = puedeCancelar;
        }

        private void btnCancelarReserva_790MY_Click(object sender, EventArgs e)
        {
            if (!(dgvReservas_790MY.CurrentRow?.DataBoundItem is Reserva_790MY seleccionada))
                return;

            var respuesta = MessageBox.Show(
                $"¿Confirma cancelar la reserva Nro. {seleccionada.ReservaID} " +
                $"(Mesa {seleccionada.MesaNumero}, {seleccionada.Fecha:dd/MM/yyyy} {seleccionada.Hora:hh\\:mm})?\n\n" +
                "Esto liberará automáticamente la mesa para ese turno.",
                "Confirmar cancelación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes) return;

            try
            {
                _reservaBll.CancelarReserva(seleccionada.ReservaID);
                MessageBox.Show("Reserva cancelada correctamente.", "Éxito",
                                 MessageBoxButtons.OK, MessageBoxIcon.Information);
                BuscarReservas();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "No se pudo cancelar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception)
            {
                MessageBox.Show("Ocurrió un error inesperado al cancelar la reserva.", "Error",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
