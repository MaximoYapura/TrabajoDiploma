using BLL_08YS;
using System;
using System.Windows.Forms;

namespace GUI_08YS.RF1
{
    // Pantalla minima: solo listado de Mesas. No se pidio ABM completo de Mesas
    // en este pedido, así que no se implementa Añadir/Modificar/Eliminar acá.
    public partial class FormMesas_790MY : Form
    {
        private readonly MesaBLL_790MY _mesaBll;

        public FormMesas_790MY()
        {
            InitializeComponent();
            _mesaBll = BLLFactory_790MY.CreateMesaBLL();
        }

        private void FormMesas_790MY_Load(object sender, EventArgs e)
        {
            dgvMesas_790MY.DataSource = _mesaBll.GetAll();
        }
    }
}
