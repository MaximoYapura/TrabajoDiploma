using BE_08YS;
using BLL_08YS;
using CustomControls;
using System;
using System.Windows.Forms;
using Service_08YS;

namespace GUI_08YS.RF1
{
    public partial class FormSeleccionarMesa_790MY : Form, IIdiomaObserver_08YS
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
            _mesaBll = BLLFactory_790MY.CreateMesaBLL();
            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) => TraductorManager_08YS.Instance.Desuscribir(this);

            UpdateIdioma();
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
                var t = TraductorManager_08YS.Instance;
                MessageBox.Show(ex.Message, t.GetTexto("titulo_datos_invalidos"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Tarjeta_MesaClick(object sender, EventArgs e)
        {
            var tarjeta = (UserControlMesa_790MY)sender;

            if (_tarjetaSeleccionada != null)
                _tarjetaSeleccionada.Seleccionada = false;

            if (_tarjetaSeleccionada == tarjeta)
            {
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
                var t = TraductorManager_08YS.Instance;
                MessageBox.Show(t.GetTexto("msg_sm_falta_seleccion"), t.GetTexto("titulo_falta_seleccionar"),
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

        #region i18n
        public void UpdateIdioma()
        {
            // Traduce automáticamente todos los controles que tengan su Tag configurado (incluyendo lblTituloSeleccionarMesa_790MY)
            TraducirControles(this);

            // Traduce el título de la ventana principal
            string textoTraducido = TraductorManager_08YS.Instance.GetTexto("SM_titulo");
            if (!string.IsNullOrEmpty(textoTraducido))
            {
                this.Text = textoTraducido;
                if (lblTituloSeleccionarMesa_790MY != null)
                {
                    lblTituloSeleccionarMesa_790MY.Text = textoTraducido;
                }
            }
            // Preservar la mesa seleccionada antes de regenerar las tarjetas
            int? nroMesaAnterior = MesaSeleccionada?.NroMesa;

            // Regenerar las tarjetas para que sus etiquetas reflejen el nuevo idioma
            CargarMesasDisponibles();
            lblTituloSeleccionarMesa_790MY.Text = TraductorManager_08YS.Instance.GetTexto("SM_titulo");
            // Restaurar la selección previa si la había
            if (nroMesaAnterior.HasValue)
            {
                foreach (Control c in flowLayoutMesas_790MY.Controls)
                {
                    if (c is UserControlMesa_790MY tarjeta && tarjeta.Mesa?.NroMesa == nroMesaAnterior.Value)
                    {
                        tarjeta.Seleccionada = true;
                        _tarjetaSeleccionada = tarjeta;
                        MesaSeleccionada = tarjeta.Mesa;
                        btnAceptar_790MY.Enabled = true;
                        break;
                    }
                }
            }
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
