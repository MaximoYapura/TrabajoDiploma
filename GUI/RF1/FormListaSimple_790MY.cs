using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GUI_08YS.RF1
{
    /// <summary>
    /// Dialogo generico para agregar/quitar valores de texto sueltos (emails o
    /// telefonos adicionales de un Cliente, por ahora). No conoce nada de Cliente
    /// ni de negocio: solo edita una lista de strings y la devuelve.
    /// </summary>
    public partial class FormListaSimple_790MY : Form
    {
        public List<string> Valores { get; private set; }

        public FormListaSimple_790MY(string titulo, IEnumerable<string> valoresIniciales)
        {
            InitializeComponent();

            this.Text = titulo;
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
    }
}
