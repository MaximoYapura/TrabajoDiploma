using BE_08YS;
using GUI_08YS.Estilo;
using Service_08YS;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace GUI_08YS.RF2
{
    /// <summary>
    /// Paleta y utilidades comunes de las pantallas de Atención en Mesa (RF2).
    /// Mismos tonos que FormRegistrarReserva_790MY para que el módulo se lea igual.
    /// </summary>
    internal static class EstiloRF2_790MY
    {
        // Salón / Comandas (tema claro)
        public static readonly Color Fondo         = Color.FromArgb(250, 248, 245);
        public static readonly Color FondoPanel    = Color.FromArgb(244, 240, 233);
        public static readonly Color Bordo         = Color.FromArgb(120, 25, 25);
        public static readonly Color BordoHover    = Color.FromArgb(95, 18, 18);
        public static readonly Color BordoSuave    = Color.FromArgb(253, 242, 242);
        public static readonly Color Texto         = Color.FromArgb(45, 42, 40);
        public static readonly Color Gris          = Color.FromArgb(110, 100, 95);
        public static readonly Color BordeCard     = Color.FromArgb(226, 216, 202);
        public static readonly Color Deshabilitado = Color.FromArgb(232, 224, 214);
        public static readonly Color PendFondo     = Color.FromArgb(250, 236, 214);
        public static readonly Color PendTexto     = Color.FromArgb(140, 90, 20);
        public static readonly Color PendBorde     = Color.FromArgb(214, 160, 70);
        public static readonly Color ListoFondo    = Color.FromArgb(225, 242, 230);
        public static readonly Color ListoTexto    = Color.FromArgb(30, 110, 60);
        public static readonly Color ListoBorde    = Color.FromArgb(70, 140, 100);

        // Cocina (KDS, tema oscuro)
        public static readonly Color KdsFondo      = Color.FromArgb(30, 24, 22);
        public static readonly Color KdsColumna    = Color.FromArgb(40, 33, 31);
        public static readonly Color KdsTarjeta    = Color.FromArgb(52, 44, 41);
        public static readonly Color KdsBorde      = Color.FromArgb(72, 62, 58);
        public static readonly Color KdsTexto      = Color.FromArgb(242, 236, 230);
        public static readonly Color KdsGris       = Color.FromArgb(180, 170, 162);
        public static readonly Color KdsAmbar      = Color.FromArgb(244, 185, 66);
        public static readonly Color KdsAzul       = Color.FromArgb(99, 170, 240);
        public static readonly Color KdsVerde      = Color.FromArgb(120, 200, 130);
        public static readonly Color KdsRojoFondo  = Color.FromArgb(255, 224, 224);
        public static readonly Color KdsRojoTexto  = Color.FromArgb(170, 30, 30);

        private static readonly CultureInfo CulturaMoneda = CultureInfo.GetCultureInfo("es-AR");

        /// <summary>Formato de precio de la carta: $14.500 (sin decimales si son cero).</summary>
        public static string Moneda(decimal valor)
            => "$" + valor.ToString(valor == decimal.Truncate(valor) ? "N0" : "N2", CulturaMoneda);

        public static string Iniciales(string nombre, string apellido)
        {
            string a = string.IsNullOrWhiteSpace(nombre) ? "" : nombre.Trim().Substring(0, 1);
            string b = string.IsNullOrWhiteSpace(apellido) ? "" : apellido.Trim().Substring(0, 1);
            return (a + b).ToUpperInvariant();
        }

        /// <summary>"Martín Quiroga" -> "Martín Q."</summary>
        public static string NombreCorto(Empleado_790MY empleado)
        {
            if (empleado == null) return string.Empty;
            string inicial = string.IsNullOrWhiteSpace(empleado.Apellido) ? "" : " " + empleado.Apellido.Trim()[0] + ".";
            return empleado.Nombre + inicial;
        }

        public static string Hora(TimeSpan hora) => $"{(int)hora.TotalHours:00}:{hora.Minutes:00}";

        /// <summary>Traduce recursivamente todo control cuyo Tag sea una clave de idiomas.json.</summary>
        public static void TraducirTags(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                if (!(c is TextBoxBase) && c.Tag is string clave && !string.IsNullOrWhiteSpace(clave))
                    c.Text = TraductorManager_08YS.Instance.GetTexto(clave);
                if (c.HasChildren)
                    TraducirTags(c);
            }
        }

        public static string T(string clave) => TraductorManager_08YS.Instance.GetTexto(clave);

        public static string T(string clave, params object[] args)
        {
            string plantilla = TraductorManager_08YS.Instance.GetTexto(clave);
            try { return string.Format(plantilla, args); }
            catch (FormatException) { return plantilla; }
        }

        /// <summary>
        /// Camino legacy (MessageBox nativo). Lo siguen usando
        /// las pantallas aún no migradas (FormFacturacionCobro_790MY se migró en la Etapa 4);
        /// las migradas usan <see cref="MostrarErrorEnBanner"/>.
        /// Reglas de negocio (ArgumentException) como advertencia, permisos como acceso
        /// denegado y cualquier otra falla como error.
        /// </summary>
        public static void MostrarError(IWin32Window owner, Exception ex)
        {
            if (ex is UnauthorizedAccessException)
                MessageBox.Show(owner, ex.Message, T("acceso_denegado"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
            else if (ex is ArgumentException)
                MessageBox.Show(owner, ex.Message, T("titulo_datos_invalidos"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                MessageBox.Show(owner, T("RF2_msg_error_inesperado", ex.Message), T("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>Camino legacy (MessageBox nativo); las pantallas migradas usan <see cref="MostrarExitoToast"/>.</summary>
        public static void MostrarExito(IWin32Window owner, string mensaje)
            => MessageBox.Show(owner, mensaje, T("exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);

        /// <summary>
        /// Etapa 2 (E2-D1/E2-D4): mismo criterio que <see cref="MostrarError"/>, pero las
        /// reglas de negocio y los permisos van al banner del formulario. Solo las fallas
        /// inesperadas conservan el MessageBox nativo (error fatal).
        /// Nombre distinto a propósito: una sobrecarga con Control capturaría las llamadas legacy.
        /// </summary>
        public static void MostrarErrorEnBanner(Banner_UC_790MY banner, Exception ex)
        {
            if (ex is UnauthorizedAccessException || ex is ArgumentException)
                banner.Mostrar(ex.Message, TipoBanner_790MY.Error, cerrable: true);
            else
                MessageBox.Show(banner.FindForm(), T("RF2_msg_error_inesperado", ex.Message), T("error"),
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>Etapa 2: éxito no bloqueante (toast de 4 s) sobre la esquina inferior derecha de <paramref name="ancla"/>.</summary>
        public static void MostrarExitoToast(Control ancla, string mensaje)
            => ToastHost_790MY.Mostrar(ancla, mensaje);

        public static Label CrearLabel(string texto, Font fuente, Color color, bool autoSize = true)
        {
            return new Label
            {
                Text = texto,
                Font = fuente,
                ForeColor = color,
                BackColor = Color.Transparent,
                AutoSize = autoSize,
                Margin = Padding.Empty
            };
        }

        public static EtiquetaChip_790MY CrearChip(string texto, Color fondo, Color colorTexto, Font fuente, Color? borde = null)
        {
            return new EtiquetaChip_790MY
            {
                Text = texto,
                BackColor = fondo,
                ForeColor = colorTexto,
                Font = fuente,
                ColorBorde = borde ?? Color.Transparent
            };
        }

        /// <summary>
        /// Hace que el clic sobre cualquier hijo de la tarjeta dispare la acción de la tarjeta
        /// (las tarjetas se arman con labels encima del panel).
        /// </summary>
        public static void PropagarClick(Control raiz, EventHandler manejador)
        {
            raiz.Click += manejador;
            raiz.Cursor = Cursors.Hand;
            foreach (Control hijo in raiz.Controls)
                if (!(hijo is ButtonBase))
                    PropagarClick(hijo, manejador);
        }
    }
}
