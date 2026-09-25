using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Service_08YS;

namespace GUI_08YS.Maestros
{
    /// <summary>
    /// Dialogo generico para agregar/quitar valores de texto sueltos (emails o
    /// telefonos adicionales de un Cliente, por ahora). No conoce nada de Cliente
    /// ni de negocio: solo edita una lista de strings y la devuelve.
    /// </summary>
    public partial class FormListaSimple_790MY : Form, IIdiomaObserver_08YS
    {
        private readonly string _tituloKey;

        public List<string> Valores { get; private set; }

        public FormListaSimple_790MY(string tituloKey, IEnumerable<string> valoresIniciales)
        {
            InitializeComponent();

            _tituloKey = tituloKey;
            this.Text = TraductorManager_08YS.Instance.GetTexto(tituloKey);
            lblTitulo_790MY.Text = TraductorManager_08YS.Instance.GetTexto(tituloKey);

            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) => TraductorManager_08YS.Instance.Desuscribir(this);

            UpdateIdioma();

            foreach (var valor in valoresIniciales ?? Enumerable.Empty<string>())
                lstValores_790MY.Items.Add(valor);
        }

        private void btnAgregar_790MY_Click(object sender, EventArgs e)
        {
            string valor = txtNuevoValor_790MY.Text.Trim();

            if (string.IsNullOrWhiteSpace(valor))
                return;

            if (lstValores_790MY.Items.Contains(valor))
            {
                var t = TraductorManager_08YS.Instance;
                MessageBox.Show(t.GetTexto("msg_valor_duplicado"), t.GetTexto("titulo_duplicado"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lstValores_790MY.Items.Add(valor);
            txtNuevoValor_790MY.Clear();
            txtNuevoValor_790MY.Focus();
        }

        private void btnQuitar_790MY_Click(object sender, EventArgs e)
        {
            if (lstValores_790MY.SelectedItem == null)
            {
                var t = TraductorManager_08YS.Instance;
                MessageBox.Show(t.GetTexto("msg_seleccionar_valor"), t.GetTexto("msg_sin_seleccion_lista"),
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lstValores_790MY.Items.Remove(lstValores_790MY.SelectedItem);
        }

        private void btnAceptar_790MY_Click(object sender, EventArgs e)
        {
            Valores = lstValores_790MY.Items.Cast<string>().ToList();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_790MY_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #region i18n
        public void UpdateIdioma()
        {
            TraducirControles(this);

            // lblTitulo_790MY no tiene Tag en el Designer; se actualiza manualmente con la clave almacenada.
            lblTitulo_790MY.Text = TraductorManager_08YS.Instance.GetTexto(_tituloKey);
            this.Text = TraductorManager_08YS.Instance.GetTexto(_tituloKey);
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
