using System.Drawing;
using System.Windows.Forms;

namespace GUI_08YS.Estilo
{
    /// <summary>
    /// Tokens visuales únicos de «El Fogón del Sur» (Rediseño UX, Etapa 1: P01–P10).
    /// Las pantallas se migran de a una a esta clase; EstiloRF2_790MY y
    /// EstiloMaestros_790MY conservan sus valores hasta que cada pantalla se migre.
    /// Regla P04: bordó sólido = acción. Los estados usan superficie suave + ícono + texto.
    /// </summary>
    internal static class TemaFogon_790MY
    {
        // ── Paleta (P03) ─────────────────────────────────────────────
        public static readonly Color Accion       = Color.FromArgb(107, 29, 47);   // #6B1D2F
        public static readonly Color AccionHover  = Color.FromArgb(86, 22, 37);    // #561625
        public static readonly Color AccionSuave  = Color.FromArgb(248, 236, 238); // selección / hover de secundarios
        public static readonly Color Marca        = Color.FromArgb(97, 13, 16);    // #610D10 (estimado del logo)
        public static readonly Color OroClaro     = Color.FromArgb(242, 217, 166); // #F2D9A6
        public static readonly Color Oro          = Color.FromArgb(232, 185, 100); // #E8B964
        public static readonly Color Crema        = Color.FromArgb(250, 248, 245); // #FAF8F5
        public static readonly Color Panel        = Color.FromArgb(244, 240, 233); // #F4F0E9
        public static readonly Color Tarjeta      = Color.White;
        public static readonly Color Linea        = Color.FromArgb(226, 216, 202); // #E2D8CA
        public static readonly Color Tinta        = Color.FromArgb(45, 42, 40);    // #2D2A28
        public static readonly Color TintaSuave   = Color.FromArgb(110, 100, 95);  // #6E645F
        public static readonly Color Deshabilitado     = Color.FromArgb(236, 232, 226);
        public static readonly Color TextoDeshabilitado = Color.FromArgb(163, 153, 144);
        public static readonly Color Foco         = Color.FromArgb(168, 115, 15);

        // ── Estados (P04): fondo de chip, superficie suave, texto, borde ─
        public static readonly Color OkFondo    = Color.FromArgb(225, 242, 230);
        public static readonly Color OkSuave    = Color.FromArgb(240, 248, 242);
        public static readonly Color OkTexto    = Color.FromArgb(30, 110, 60);
        public static readonly Color OkBorde    = Color.FromArgb(182, 220, 194);

        public static readonly Color AvisoFondo = Color.FromArgb(250, 236, 214);
        public static readonly Color AvisoSuave = Color.FromArgb(253, 246, 233);
        public static readonly Color AvisoTexto = Color.FromArgb(140, 90, 20);
        public static readonly Color AvisoBorde = Color.FromArgb(235, 203, 147);

        public static readonly Color OcupFondo  = Color.FromArgb(245, 230, 232);
        public static readonly Color OcupSuave  = Color.FromArgb(250, 240, 241);
        public static readonly Color OcupTexto  = Color.FromArgb(107, 29, 47);
        public static readonly Color OcupBorde  = Color.FromArgb(227, 191, 198);

        public static readonly Color FueraFondo = Color.FromArgb(236, 232, 226);
        public static readonly Color FueraTexto = Color.FromArgb(110, 100, 95);
        public static readonly Color FueraBorde = Color.FromArgb(185, 175, 164);

        public static readonly Color ErrorFondo = Color.FromArgb(252, 232, 230);
        public static readonly Color ErrorTexto = Color.FromArgb(179, 38, 30);

        // ── Tema oscuro de cocina (P05): solo definición, el KDS no se migra en Etapa 1 ─
        public static readonly Color KdsFondo    = Color.FromArgb(30, 24, 22);
        public static readonly Color KdsColumna  = Color.FromArgb(40, 33, 31);
        public static readonly Color KdsTarjeta  = Color.FromArgb(52, 44, 41);
        public static readonly Color KdsLinea    = Color.FromArgb(72, 62, 58);
        public static readonly Color KdsTinta    = Color.FromArgb(242, 236, 230);
        public static readonly Color KdsTintaSuave = Color.FromArgb(180, 170, 162);
        public static readonly Color KdsAmbar    = Color.FromArgb(244, 185, 66);
        public static readonly Color KdsAzul     = Color.FromArgb(99, 170, 240);
        public static readonly Color KdsVerde    = Color.FromArgb(120, 200, 130);
        public static readonly Color KdsRojo     = Color.FromArgb(242, 128, 116);

        // ── Barra lateral de FormMDI_08YS: conserva sus colores reales (Etapa 5) ─
        // Valores medidos sobre los IconButton del menú (FlatStyle.Flat sin FlatAppearance propio).
        public static readonly Color BarraFondo      = Color.FromArgb(60, 15, 15);
        public static readonly Color BarraHover      = Color.FromArgb(72, 18, 18);
        public static readonly Color BarraPresionado = Color.FromArgb(189, 73, 73);
        public static readonly Color BarraTinta      = Color.FromArgb(235, 195, 140);
        public const string BarraFamilia = "Microsoft Sans Serif";

        // ── Tipografía (P06, P07): Segoe UI en toda la aplicación, escala fija ─
        public const string Familia = "Segoe UI";
        public static Font FuenteTitulo()    => new Font(Familia, 16F, FontStyle.Bold);
        public static Font FuenteSubtitulo() => new Font(Familia, 12F, FontStyle.Bold);
        public static Font FuenteCuerpo()    => new Font(Familia, 10F);
        public static Font FuenteEtiqueta()  => new Font(Familia, 9F, FontStyle.Bold);
        public static Font FuenteAyuda()     => new Font(Familia, 8.25F);

        // ── Espaciado y radios (P08): múltiplos de 4, radios 6/10/12, sin sombras ─
        public const int Espacio1 = 4;
        public const int Espacio2 = 8;
        public const int Espacio3 = 12;
        public const int Espacio4 = 16;
        public const int Espacio6 = 24;
        public const int RadioControl = 6;
        public const int RadioTarjeta = 10;
        public const int RadioPanel   = 12;

        /// <summary>Botón primario (bordó sólido: la acción principal) o secundario (contorno).</summary>
        public static void EstilizarBoton(Button boton, bool primario)
        {
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 1;
            boton.FlatAppearance.BorderColor = Accion;
            boton.BackColor = primario ? Accion : Tarjeta;
            boton.ForeColor = primario ? Color.White : Accion;
            boton.FlatAppearance.MouseOverBackColor = primario ? AccionHover : AccionSuave;
            boton.FlatAppearance.MouseDownBackColor = primario ? AccionHover : AccionSuave;
            boton.UseVisualStyleBackColor = false;
            boton.Cursor = Cursors.Hand;
            boton.EnabledChanged -= Boton_EnabledChanged;
            boton.EnabledChanged += Boton_EnabledChanged;
            RecordarTipo(boton, primario);
            AplicarEstadoBoton(boton);
        }

        /// <summary>
        /// Grilla de maestros: encabezado sobrio sobre panel, filas altas con separador
        /// suave y selección en bordó suave con texto en tinta (sin el azul del sistema).
        /// </summary>
        public static void EstilizarGrilla(DataGridView dgv)
        {
            dgv.EnableHeadersVisualStyles = false;
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = Tarjeta;
            dgv.GridColor = Color.FromArgb(239, 232, 220);
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 38;
            dgv.RowHeadersVisible = false;
            dgv.RowTemplate.Height = 40;
            dgv.AllowUserToResizeRows = false;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Panel;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = TintaSuave;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Panel;
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = TintaSuave;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font(Familia, 9F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 0, 0);

            dgv.DefaultCellStyle.Font = new Font(Familia, 10F);
            dgv.DefaultCellStyle.ForeColor = Tinta;
            dgv.DefaultCellStyle.BackColor = Tarjeta;
            dgv.DefaultCellStyle.SelectionBackColor = AccionSuave;
            dgv.DefaultCellStyle.SelectionForeColor = Tinta;
            dgv.DefaultCellStyle.Padding = new Padding(8, 0, 0, 0);
        }

        // Texto de ayuda dentro de un TextBox vacío (cue banner nativo de Windows).
        private const int EM_SETCUEBANNER = 0x1501;

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern System.IntPtr SendMessage(System.IntPtr hWnd, int msg, System.IntPtr wParam, string lParam);

        /// <summary>Muestra <paramref name="texto"/> en gris mientras el TextBox está vacío, incluso con foco.</summary>
        public static void PonerMarcaDeAgua(TextBox caja, string texto)
        {
            if (caja.IsHandleCreated)
                SendMessage(caja.Handle, EM_SETCUEBANNER, (System.IntPtr)1, texto ?? string.Empty);
            else
                caja.HandleCreated += (s, e) => SendMessage(caja.Handle, EM_SETCUEBANNER, (System.IntPtr)1, texto ?? string.Empty);
        }

        /// <summary>
        /// Opción de un selector segmentado (RadioButton con Appearance = Button):
        /// la opción elegida va en bordó sólido; las demás, con contorno.
        /// </summary>
        public static void EstilizarOpcion(RadioButton opcion)
        {
            opcion.FlatStyle = FlatStyle.Flat;
            opcion.FlatAppearance.BorderSize = 1;
            opcion.FlatAppearance.CheckedBackColor = Accion;
            opcion.FlatAppearance.MouseOverBackColor = AccionSuave;
            opcion.UseVisualStyleBackColor = false;
            opcion.Cursor = Cursors.Hand;
            opcion.CheckedChanged -= Opcion_Cambio;
            opcion.CheckedChanged += Opcion_Cambio;
            opcion.EnabledChanged -= Opcion_Cambio;
            opcion.EnabledChanged += Opcion_Cambio;
            AplicarEstadoOpcion(opcion);
        }

        private static void Opcion_Cambio(object sender, System.EventArgs e)
            => AplicarEstadoOpcion((RadioButton)sender);

        private static void AplicarEstadoOpcion(RadioButton opcion)
        {
            if (!opcion.Enabled)
            {
                opcion.BackColor = opcion.Checked ? Linea : Deshabilitado;
                opcion.ForeColor = opcion.Checked ? Tinta : TextoDeshabilitado;
                opcion.FlatAppearance.BorderColor = opcion.Checked ? FueraBorde : Deshabilitado;
                opcion.FlatAppearance.CheckedBackColor = Linea;
                return;
            }
            opcion.FlatAppearance.CheckedBackColor = Accion;
            opcion.FlatAppearance.MouseOverBackColor = opcion.Checked ? AccionHover : AccionSuave;
            opcion.BackColor = opcion.Checked ? Accion : Tarjeta;
            opcion.ForeColor = opcion.Checked ? Color.White : Accion;
            opcion.FlatAppearance.BorderColor = Accion;
        }

        private static void Boton_EnabledChanged(object sender, System.EventArgs e)
            => AplicarEstadoBoton((Button)sender);

        // Un botón FlatStyle.Flat deshabilitado conserva su BackColor bordó y se confunde con
        // uno activo: se pinta con la superficie de «deshabilitado» para que se lea como tal.
        private static void AplicarEstadoBoton(Button boton)
        {
            bool primario = EsPrimario(boton);
            if (boton.Enabled)
            {
                boton.BackColor = primario ? Accion : Tarjeta;
                boton.ForeColor = primario ? Color.White : Accion;
                boton.FlatAppearance.BorderColor = Accion;
            }
            else
            {
                boton.BackColor = Deshabilitado;
                boton.ForeColor = TextoDeshabilitado;
                boton.FlatAppearance.BorderColor = Deshabilitado;
            }
        }

        // El Tag de los botones ya guarda la clave de idioma, así que el tipo de botón
        // (primario/secundario) se recuerda aparte.
        private static readonly System.Runtime.CompilerServices.ConditionalWeakTable<Button, object> _primarios =
            new System.Runtime.CompilerServices.ConditionalWeakTable<Button, object>();

        private static void RecordarTipo(Button boton, bool primario)
        {
            _primarios.Remove(boton);
            if (primario) _primarios.Add(boton, null);
        }

        private static bool EsPrimario(Button boton) => _primarios.TryGetValue(boton, out _);
    }
}
