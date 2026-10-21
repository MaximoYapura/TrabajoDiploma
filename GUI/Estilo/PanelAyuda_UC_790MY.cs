using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using Service_08YS;
using T = GUI_08YS.Estilo.TemaFogon_790MY;

namespace GUI_08YS.Estilo
{
    /// <summary>
    /// Panel lateral derecho de ayuda (Etapa 5, C1): hermano de panel2 en FormMDI_08YS, así
    /// que OpenChildForm (que limpia panel2) nunca lo toca y la pantalla activa sigue operativa.
    /// Pestañas: Ficha de Ingredientes (solo Modo Demostración + SuperAdmin) y Menú Degustación.
    /// </summary>
    public partial class PanelAyuda_UC_790MY : UserControl, IIdiomaObserver_08YS
    {
        /// <summary>Nombre exacto del rol en la base (Roles.Nombre, script.sql).</summary>
        private const string RolFicha = "SuperAdmin";

        private const int TabFicha = 0;
        private const int TabDegustacion = 1;

        private bool _fichaPermitida;

        /// <summary>El usuario pidió cerrar el panel (botón ✕ o Esc con el foco adentro).</summary>
        public event EventHandler CerrarSolicitado;

        /// <summary>Un paso del Menú Degustación pidió abrir su pantalla.</summary>
        public event EventHandler<DestinoAyudaEventArgs_790MY> NavegacionSolicitada;

        public PanelAyuda_UC_790MY()
        {
            InitializeComponent();
        }

        /// <summary>
        /// G3: la Ficha existe solo con ModoDemostracion = true en App.config y el usuario
        /// actual con rol SuperAdmin. Si no, se quita y se descarta: sin pestaña ni datos.
        /// </summary>
        public void Preparar()
        {
            _fichaPermitida = ModoDemostracionActivo()
                && string.Equals(SessionManager_08YS.Instance.Current?.Rol?.Nombre, RolFicha, StringComparison.Ordinal);

            if (!_fichaPermitida && ucFicha_790MY != null)
            {
                pnlContenido_790MY.Controls.Remove(ucFicha_790MY);
                ucFicha_790MY.Dispose();
                ucFicha_790MY = null;
                pnlTabs_790MY.Visible = false;
            }
            MostrarPestana(_fichaPermitida ? TabFicha : TabDegustacion);
            if (_fichaPermitida) tabAyuda_790MY.SeleccionarSinAnimacion(TabFicha);
        }

        public bool FichaPermitida => _fichaPermitida;

        private static bool ModoDemostracionActivo()
        {
            try
            {
                return bool.TryParse(ConfigurationManager.AppSettings["ModoDemostracion"], out bool activo) && activo;
            }
            catch (ConfigurationErrorsException)
            {
                return false;
            }
        }

        /// <summary>Al abrir el panel: datos en vivo y foco en la primera opción.</summary>
        public void AlMostrar()
        {
            ucFicha_790MY?.Cargar();
            ucDegustacion_790MY.Refrescar();
            if (pnlTabs_790MY.Visible) tabAyuda_790MY.Focus();
            else ucDegustacion_790MY.EnfocarPrimero();
        }

        public void UpdateIdioma()
        {
            var tr = TraductorManager_08YS.Instance;
            lblTitulo_790MY.Text = tr.GetTexto("AY_panel_titulo");
            AccessibleName = lblTitulo_790MY.Text;
            btnCerrar_790MY.AccessibleName = tr.GetTexto("AY_panel_cerrar");
            tabAyuda_790MY.ActualizarTextos();
            ucFicha_790MY?.UpdateIdioma();
            ucDegustacion_790MY.UpdateIdioma();
        }

        private void MostrarPestana(int indice)
        {
            bool ficha = indice == TabFicha && ucFicha_790MY != null;
            if (ucFicha_790MY != null) ucFicha_790MY.Visible = ficha;
            ucDegustacion_790MY.Visible = !ficha;
        }

        private void tabAyuda_790MY_SeleccionCambiada(object sender, EventArgs e)
            => MostrarPestana(tabAyuda_790MY.IndiceSeleccionado);

        private void btnCerrar_790MY_Click(object sender, EventArgs e)
            => CerrarSolicitado?.Invoke(this, EventArgs.Empty);

        private void ucDegustacion_790MY_NavegacionSolicitada(object sender, DestinoAyudaEventArgs_790MY e)
            => NavegacionSolicitada?.Invoke(this, e);

        /// <summary>Esc cierra el panel cuando el foco está adentro (F1 sigue hacia la MDI).</summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                CerrarSolicitado?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var lapiz = new Pen(T.Linea))
                e.Graphics.DrawLine(lapiz, 0, 0, 0, Height);
        }
    }
}
