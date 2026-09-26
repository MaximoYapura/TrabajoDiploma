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

        /// <summary>
        /// Wrapper para los ítems del ComboBox de estado. Permite mostrar el texto
        /// traducido sin romper la lógica de negocio que trabaja con el enum.
        /// </summary>
        private class EstadoItem
        {
            public string Display { get; set; }
            public EstadoReserva_790MY? Valor { get; set; }
            public override string ToString() => Display;
        }

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

            CargarEstados();
            BuscarReservas();
        }

        /// <summary>
        /// Carga (o recarga) el ComboBox de estados preservando el índice seleccionado.
        /// Se llama tanto en Load como en UpdateIdioma() para refrescar las traducciones.
        /// </summary>
        private void CargarEstados()
        {
            var t = TraductorManager_08YS.Instance;
            int indiceAnterior = cmbEstado_790MY.SelectedIndex < 0 ? 0 : cmbEstado_790MY.SelectedIndex;

            cmbEstado_790MY.Items.Clear();
            cmbEstado_790MY.Items.Add(new EstadoItem { Display = t.GetTexto("GR_cmbEstadoTodos"), Valor = null });
            cmbEstado_790MY.Items.Add(new EstadoItem { Display = t.GetTexto("GR_cmbConfirmada"),  Valor = EstadoReserva_790MY.Confirmada });
            cmbEstado_790MY.Items.Add(new EstadoItem { Display = t.GetTexto("GR_cmbCancelada"),   Valor = EstadoReserva_790MY.Cancelada });

            cmbEstado_790MY.SelectedIndex = (indiceAnterior >= 0 && indiceAnterior < cmbEstado_790MY.Items.Count)
                ? indiceAnterior : 0;
        }

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
            if (!string.IsNullOrWhiteSpace(txtDni_790MY.Text))
            {
                if (!int.TryParse(txtDni_790MY.Text.Trim(), out int dniParseado))
                {
                    MessageBox.Show(t.GetTexto("msg_gr_dni_invalido"), t.GetTexto("titulo_dato_invalido"),
                                     MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                dni = dniParseado;
            }

            // Extraer el enum directamente del wrapper; null = "Todos"
            EstadoReserva_790MY? estado = null;
            if (cmbEstado_790MY.SelectedItem is EstadoItem item)
                estado = item.Valor;

            try
            {
                dgvReservas_790MY.DataSource = null;
                dgvReservas_790MY.DataSource = _reservaBll.Buscar(desde, hasta, dni, estado);
                ActualizarHabilitacionCancelar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(t.GetTexto("msg_gr_error_buscar"), ex.Message),
                    t.GetTexto("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnLimpiar_790MY_Click(object sender, EventArgs e)
        {
            LimpiarFiltros();
        }

        private void LimpiarFiltros()
        {
            dtpDesde_790MY.Checked       = false;
            dtpHasta_790MY.Checked       = false;
            txtDni_790MY.Text            = string.Empty;
            cmbEstado_790MY.SelectedIndex = 0;
            BuscarReservas();
        }

        private void btnCancelarReserva_790MY_Click(object sender, EventArgs e)
        {
            if (!(dgvReservas_790MY.CurrentRow?.DataBoundItem is Reserva_790MY seleccionada))
                return;

            var t = TraductorManager_08YS.Instance;
            var respuesta = MessageBox.Show(
                string.Format(t.GetTexto("msg_gr_confirmar_cancelar"),
                              seleccionada.ReservaID,
                              seleccionada.MesaNumero,
                              seleccionada.Fecha.ToString("dd/MM/yyyy"),
                              seleccionada.Hora.ToString(@"hh\:mm")),
                t.GetTexto("titulo_confirmar_cancelacion"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes) return;

            try
            {
                _reservaBll.CancelarReserva(seleccionada.ReservaID);
                MessageBox.Show(t.GetTexto("msg_gr_cancelada_ok"), t.GetTexto("exito"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Information);
                BuscarReservas();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, t.GetTexto("titulo_no_se_pudo_cancelar"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception)
            {
                MessageBox.Show(t.GetTexto("msg_gr_error_cancelar"), t.GetTexto("error"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region i18n
        public void UpdateIdioma()
        {
            TraducirControles(this);

            // Refrescar el ComboBox de estado con los nuevos textos traducidos
            CargarEstados();

            // Encabezados de columnas del DGV (no son Controls, deben traducirse explícitamente)
            colReservaID_790MY.HeaderText   = TraductorManager_08YS.Instance.GetTexto("GR_colNro");
            colDniCliente_790MY.HeaderText  = TraductorManager_08YS.Instance.GetTexto("GR_colDni");
            colCliente_790MY.HeaderText     = TraductorManager_08YS.Instance.GetTexto("GR_colCliente");
            colMesa_790MY.HeaderText        = TraductorManager_08YS.Instance.GetTexto("GR_colMesa");
            colFecha_790MY.HeaderText       = TraductorManager_08YS.Instance.GetTexto("GR_colFecha");
            colHora_790MY.HeaderText        = TraductorManager_08YS.Instance.GetTexto("GR_colHora");
            colComensales_790MY.HeaderText  = TraductorManager_08YS.Instance.GetTexto("GR_colComensales");
            colEstado_790MY.HeaderText      = TraductorManager_08YS.Instance.GetTexto("GR_colEstado");
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
