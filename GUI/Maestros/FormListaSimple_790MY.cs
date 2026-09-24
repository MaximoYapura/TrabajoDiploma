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
        public List<string> Valores { get; private set; }

        public FormListaSimple_790MY(string titulo, IEnumerable<string> valoresIniciales)
        {
            InitializeComponent();

            this.Text = titulo;
            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) => TraductorManager_08YS.Instance.Desuscribir(this);
            lblTitulo_790MY.Text = titulo;

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
                MessageBox.Show("Ese valor ya está en la lista.", "Duplicado",
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
                MessageBox.Show("Seleccioná un valor de la lista para quitar.", "Sin selección",
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
