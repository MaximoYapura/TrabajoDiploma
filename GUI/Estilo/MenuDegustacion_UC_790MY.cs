using System;
using System.Windows.Forms;
using Service_08YS;

namespace GUI_08YS.Estilo
{
    /// <summary>Menú Degustación: ruta de prueba con accesos directos (Fase 4).</summary>
    public partial class MenuDegustacion_UC_790MY : UserControl, IIdiomaObserver_08YS
    {
        public event EventHandler<DestinoAyudaEventArgs_790MY> NavegacionSolicitada;

        public MenuDegustacion_UC_790MY()
        {
            InitializeComponent();
        }

        public void Refrescar() { }

        public void EnfocarPrimero() { Focus(); }

        public void UpdateIdioma() { }

        private void Ir(DestinoAyuda_790MY destino)
            => NavegacionSolicitada?.Invoke(this, new DestinoAyudaEventArgs_790MY(destino));
    }
}
