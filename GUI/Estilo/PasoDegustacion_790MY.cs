using System;
using System.Drawing;
using System.Windows.Forms;
using T = GUI_08YS.Estilo.TemaFogon_790MY;

namespace GUI_08YS.Estilo
{
    /// <summary>
    /// Un paso del Menú Degustación (Etapa 5, Fase 4): título, pista, casilla «Hecho» (solo
    /// en memoria) y botón «Ir». Sin permiso, «Ir» queda deshabilitado con el motivo visible.
    /// </summary>
    public partial class PasoDegustacion_790MY : UserControl
    {
        /// <summary>El usuario pulsó «Ir».</summary>
        public event EventHandler IrSolicitado;

        public PasoDegustacion_790MY()
        {
            InitializeComponent();
        }

        public bool Hecho => chkHecho_790MY.Checked;

        public bool IrHabilitado => btnIr_790MY.Enabled;

        public void Configurar(string titulo, string pista, bool permitido, string motivo,
                               string textoIr, string textoHecho)
        {
            lblTitulo_790MY.Text = titulo;
            lblPista_790MY.Text = pista;
            btnIr_790MY.Text = textoIr;
            btnIr_790MY.AccessibleName = $"{textoIr}: {titulo}";
            btnIr_790MY.AccessibleDescription = permitido ? pista : motivo;
            chkHecho_790MY.Text = textoHecho;
            chkHecho_790MY.AccessibleName = $"{textoHecho}: {titulo}";
            btnIr_790MY.Enabled = permitido;
            lblMotivo_790MY.Text = motivo;
            lblMotivo_790MY.Visible = !permitido;
        }

        public bool EnfocarIr()
        {
            if (!btnIr_790MY.Enabled) return false;
            btnIr_790MY.Focus();
            return true;
        }

        private void btnIr_790MY_Click(object sender, EventArgs e) => IrSolicitado?.Invoke(this, EventArgs.Empty);

        // Un botón Flat deshabilitado conserva el bordó y parece activo (TemaFogon P04).
        private void btnIr_790MY_EnabledChanged(object sender, EventArgs e)
        {
            bool activo = btnIr_790MY.Enabled;
            btnIr_790MY.BackColor = activo ? T.Accion : T.Deshabilitado;
            btnIr_790MY.ForeColor = activo ? Color.White : T.TextoDeshabilitado;
            btnIr_790MY.FlatAppearance.BorderColor = activo ? T.Accion : T.Deshabilitado;
        }

        private void chkHecho_790MY_CheckedChanged(object sender, EventArgs e)
        {
            lblTitulo_790MY.ForeColor = chkHecho_790MY.Checked ? T.OkTexto : T.Tinta;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var lapiz = new Pen(chkHecho_790MY.Checked ? T.OkBorde : T.Linea))
                e.Graphics.DrawRectangle(lapiz, 0, 0, Width - 1, Height - 1);
        }
    }
}
