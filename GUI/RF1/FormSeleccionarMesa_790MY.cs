using BE_08YS;
using BLL_08YS;
using CustomControls;
using System;
using System.Windows.Forms;

namespace GUI_08YS.RF1
{
    public partial class FormSeleccionarMesa_790MY : Form
    {
        private readonly MesaBLL_790MY _mesaBll;
        private readonly DateTime _fecha;
        private readonly TimeSpan _hora;
        private readonly int _comensales;

        private UserControlMesa_790MY _tarjetaSeleccionada;

        public Mesa_790MY MesaSeleccionada { get; private set; }

        public FormSeleccionarMesa_790MY(DateTime fecha, TimeSpan hora, int comensales)
        {
            InitializeComponent();

            _fecha = fecha;
            _hora = hora;
            _comensales = comensales;
            _mesaBll = BLLFactory_08YS.CreateMesaBLL();
        }

        private void FormSeleccionarMesa_790MY_Load(object sender, EventArgs e)
        {
            CargarMesasDisponibles();
        }

        private void CargarMesasDisponibles()
        {
            flowLayoutMesas_790MY.Controls.Clear();
            _tarjetaSeleccionada = null;
            MesaSeleccionada = null;
            btnAceptar_790MY.Enabled = false;

            try
            {
                var disponibles = _mesaBll.BuscarDisponibles(_fecha, _hora, _comensales);

                lblSinResultados_790MY.Visible = disponibles.Count == 0;

                foreach (var mesa in disponibles)
                {
                    var tarjeta = new UserControlMesa_790MY
                    {
                        Mesa = mesa,
                        EstadoVisual = EstadoMesa_790MY.Libre, // todo lo que llega acá ya está disponible para este turno
                        Seleccionable = true,
                        Margin = new Padding(8)
                    };
                    tarjeta.MesaClick += Tarjeta_MesaClick;
                    flowLayoutMesas_790MY.Controls.Add(tarjeta);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Tarjeta_MesaClick(object sender, EventArgs e)
        {
            var tarjeta = (UserControlMesa_790MY)sender;

            if (_tarjetaSeleccionada != null)
                _tarjetaSeleccionada.Seleccionada = false;

            if (_tarjetaSeleccionada == tarjeta)
            {
                // click sobre la misma tarjeta ya seleccionada -> deseleccionar
                _tarjetaSeleccionada = null;
                MesaSeleccionada = null;
            }
            else
            {
                tarjeta.Seleccionada = true;
                _tarjetaSeleccionada = tarjeta;
                MesaSeleccionada = tarjeta.Mesa;
            }

            btnAceptar_790MY.Enabled = MesaSeleccionada != null;
        }

        private void btnAceptar_790MY_Click(object sender, EventArgs e)
        {
            if (MesaSeleccionada == null)
            {
                MessageBox.Show("Seleccioná una mesa antes de continuar.", "Falta seleccionar",
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_790MY_Click(object sender, EventArgs e)
        {
            MesaSeleccionada = null;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
