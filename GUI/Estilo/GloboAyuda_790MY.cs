using System;
using System.Drawing;
using System.Windows.Forms;
using T = GUI_08YS.Estilo.TemaFogon_790MY;

namespace GUI_08YS.Estilo
{
    /// <summary>
    /// Globo de las Sugerencias del Chef (Etapa 5, Fase 6, C8): control opaco (sin
    /// transparencia ni ventana propia) que AyudaSugerencias_790MY ubica junto al control
    /// resaltado. ← / → recorren las pistas y Esc cierra.
    /// </summary>
    public partial class GloboAyuda_790MY : UserControl
    {
        public event EventHandler AnteriorSolicitado;
        public event EventHandler SiguienteSolicitado;
        public event EventHandler CerrarSolicitado;

        private const int Borde = 2;

        public GloboAyuda_790MY()
        {
            InitializeComponent();
        }

        public void Mostrar(string titulo, string texto, string probaCon, int indice, int total,
                            string contador, string anterior, string siguiente, string terminar, string cerrar)
        {
            bool ultima = indice == total - 1;
            lblTitulo_790MY.Text = titulo;
            lblTexto_790MY.Text = texto;
            lblProbaCon_790MY.Text = probaCon ?? string.Empty;
            lblProbaCon_790MY.Visible = !string.IsNullOrEmpty(probaCon);
            lblContador_790MY.Text = contador;
            btnAnterior_790MY.Text = anterior;
            btnAnterior_790MY.Enabled = indice > 0;
            btnAnterior_790MY.Visible = total > 1;
            btnSiguiente_790MY.Text = ultima ? terminar : siguiente;
            btnCerrar_790MY.Text = cerrar;
            btnCerrar_790MY.Visible = !ultima;
            AccessibleName = titulo;
            AccessibleDescription = texto;
            PerformLayout();
            Invalidate();
        }

        public void EnfocarSiguiente() => btnSiguiente_790MY.Focus();

        private void btnAnterior_790MY_Click(object sender, EventArgs e) => AnteriorSolicitado?.Invoke(this, EventArgs.Empty);
        private void btnSiguiente_790MY_Click(object sender, EventArgs e) => SiguienteSolicitado?.Invoke(this, EventArgs.Empty);
        private void btnCerrar_790MY_Click(object sender, EventArgs e) => CerrarSolicitado?.Invoke(this, EventArgs.Empty);

        private void btnAnterior_790MY_EnabledChanged(object sender, EventArgs e)
        {
            bool activo = btnAnterior_790MY.Enabled;
            btnAnterior_790MY.ForeColor = activo ? T.Accion : T.TextoDeshabilitado;
            btnAnterior_790MY.BackColor = activo ? T.Tarjeta : T.Deshabilitado;
            btnAnterior_790MY.FlatAppearance.BorderColor = activo ? T.Accion : T.Deshabilitado;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left:
                    if (btnAnterior_790MY.Enabled) AnteriorSolicitado?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.Right:
                    SiguienteSolicitado?.Invoke(this, EventArgs.Empty);
                    return true;
                case Keys.Escape:
                    CerrarSolicitado?.Invoke(this, EventArgs.Empty);
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var lapiz = new Pen(T.Oro, Borde))
                e.Graphics.DrawRectangle(lapiz, Borde / 2, Borde / 2, Width - Borde, Height - Borde);
        }
    }
}
