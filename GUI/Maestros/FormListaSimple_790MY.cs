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

            // Etapa 2 (E2-D4): los errores de campo se marcan con el ErrorProvider y se
            // borran solos al corregirse (al escribir o al elegir un valor de la lista).
            if (lstValores_790MY.Items.Contains(valor))
            {
                errValidacion_790MY.SetError(txtNuevoValor_790MY,
                    TraductorManager_08YS.Instance.GetTexto("msg_valor_duplicado"));
                txtNuevoValor_790MY.SelectAll();
                txtNuevoValor_790MY.Focus();
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
                errValidacion_790MY.SetError(lstValores_790MY,
                    TraductorManager_08YS.Instance.GetTexto("msg_seleccionar_valor"));
                lstValores_790MY.Focus();
                return;
            }

            lstValores_790MY.Items.Remove(lstValores_790MY.SelectedItem);
        }

        private void txtNuevoValor_790MY_TextChanged(object sender, EventArgs e)
            => errValidacion_790MY.SetError(txtNuevoValor_790MY, string.Empty);

        private void lstValores_790MY_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstValores_790MY.SelectedItem != null)
                errValidacion_790MY.SetError(lstValores_790MY, string.Empty);
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
