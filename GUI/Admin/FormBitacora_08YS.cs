using BLL_08YS;
using FontAwesome.Sharp;
using CustomControls;
using Service_08YS;
using Service_08YS.Entities.Bitacora;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FormBitacora_08YS : Form,IIdiomaObserver_08YS
    {
        private BitacoraBLL_08YS _bll = BLLFactory_08YS.CreateBitacoraBLL();

        private int _indiceFilaActual = 0;

        public FormBitacora_08YS()
        {
            InitializeComponent();

            ConfigurarBoton(btnLimpiar);
            ConfigurarBoton(btnFiltrar);
            ConfigurarBoton(btnExportar);

            comboBoxModulo.DataSource = Enum.GetValues(typeof(Modulo));
            comboBoxEvento.DataSource = Enum.GetValues(typeof(Evento));
            comboBoxCriticidad.DataSource = Enum.GetValues(typeof(Criticidad));

            comboBoxModulo.SelectedIndex = -1;
            comboBoxEvento.SelectedIndex = -1;
            comboBoxCriticidad.SelectedIndex = -1;

            dtpDesde.Value = null;
            dtpHasta.Value = null;

            UpdateIdioma();
        }
        public void UpdateIdioma()
        {
            TraducirControles(this);
            TraducirColumnas();
        }
        private void TraducirColumnas()
        {
            if (dgvEventos.Columns.Count > 0)
            {
                if (dgvEventos.Columns.Contains("Evento")) dgvEventos.Columns["Evento"].HeaderText = TraductorManager_08YS.Instance.GetTexto("ColumnaEvento");
                if (dgvEventos.Columns.Contains("FechaHora")) dgvEventos.Columns["FechaHora"].HeaderText = TraductorManager_08YS.Instance.GetTexto("ColumnaFechaHora");
                if (dgvEventos.Columns.Contains("Modulo")) dgvEventos.Columns["Modulo"].HeaderText = TraductorManager_08YS.Instance.GetTexto("ColumnaModulo");
                if (dgvEventos.Columns.Contains("Criticidad")) dgvEventos.Columns["Criticidad"].HeaderText = TraductorManager_08YS.Instance.GetTexto("ColumnaCriticidad");
            }
        }
        private void TraducirControles(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                // Si el control tiene un Tag asignado, buscamos su traducción
                if (c.Tag != null && !string.IsNullOrWhiteSpace(c.Tag.ToString()))
                {
                    c.Text = TraductorManager_08YS.Instance.GetTexto(c.Tag.ToString());
                }

                // Si el control tiene hijos (como un Panel o GroupBox), hacemos recursividad
                if (c.HasChildren)
                {
                    TraducirControles(c);
                }
            }
        }
        private void FormBitacora_Load(object sender, EventArgs e) => CargarGrid();

        private void CargarGrid()
        {
            try
            {
                dgvEventos.DataSource = null;

                dgvEventos.DataSource = _bll.GetAll();
                dgvEventos.Columns["FechaHora"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                TraducirColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;

            // Configuración de Tipografías y Colores
            Font fuenteTitulo = new Font("Arial", 16, FontStyle.Bold);
            Font fuenteInfo = new Font("Arial", 10, FontStyle.Italic);
            Font fuenteEncabezado = new Font("Arial", 10, FontStyle.Bold);
            Font fuenteCuerpo = new Font("Arial", 9, FontStyle.Regular);

            Brush pincelNegro = Brushes.Black;
            Pen lapizGris = new Pen(Color.LightGray, 1);

            // Coordenadas iniciales de dibujo (Márgenes)
            int x = e.MarginBounds.Left;
            int y = e.MarginBounds.Top;

            // 1. Dibujar Título del Reporte
            g.DrawString("REPORTE DE AUDITORÍA Y BITÁCORA", fuenteTitulo, pincelNegro, x + 200, y);
            y += 30;

            // 2. Dibujar Metadatos
            g.DrawString($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", fuenteInfo, pincelNegro, x, y);
            g.DrawString("Sistema: GastroGest", fuenteInfo, pincelNegro, x + 600, y);
            y += 40; // Espacio antes de la tabla

            // 3. Definir el ancho fijo de cada columna
            int[] anchos = { 120, 180, 120, 300, 100 };
            string[] encabezados = { "Usuario", "Fecha y Hora", "Módulo", "Evento", "Criticidad" };

            // 4. Dibujar Encabezados de la Tabla
            int xTemporal = x;
            for (int i = 0; i < encabezados.Length; i++)
            {
                g.FillRectangle(Brushes.LightGray, xTemporal, y, anchos[i], 25);
                g.DrawRectangle(Pens.Black, xTemporal, y, anchos[i], 25);
                g.DrawString(encabezados[i], fuenteEncabezado, pincelNegro, xTemporal + 5, y + 5);

                xTemporal += anchos[i];
            }
            y += 25; // Bajamos el cursor al cuerpo de la tabla

            // 5. NUEVO: Recorrer con un bucle WHILE usando el puntero global para soportar múltiples páginas
            while (_indiceFilaActual < dgvEventos.Rows.Count)
            {
                DataGridViewRow fila = dgvEventos.Rows[_indiceFilaActual];

                if (fila.DataBoundItem is BitacoraEvento_08YS ev)
                {
                    xTemporal = x;

                    string[] datosFila = {
                        ev.Username ?? "SISTEMA",
                        ev.FechaHora.ToString("dd/MM/yyyy HH:mm:ss"),
                        ev.Modulo.ToString(),
                        ev.Evento.ToString(),
                        ev.Criticidad.ToString()
                    };

                    // Dibujar cada celda de la fila actual
                    for (int i = 0; i < datosFila.Length; i++)
                    {
                        g.DrawRectangle(lapizGris, xTemporal, y, anchos[i], 22);
                        g.DrawString(datosFila[i], fuenteCuerpo, pincelNegro, xTemporal + 5, y + 4);

                        xTemporal += anchos[i];
                    }

                    y += 22; // Avanzamos a la siguiente fila hacia abajo
                }

                _indiceFilaActual++; // Avanzamos el contador general de registros

                // Control de salto de página
                if (y > e.MarginBounds.Bottom - 40 && _indiceFilaActual < dgvEventos.Rows.Count)
                {
                    e.HasMorePages = true; // Le avisa a Windows que genere otra hoja
                    return; // Sale del método guardando la posición en _indiceFilaActual
                }
            }

            e.HasMorePages = false; // Terminó de dibujar todo el set de datos
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (dgvEventos.Rows.Count == 0)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_no_datos_exportar"), TraductorManager_08YS.Instance.GetTexto("atencion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // REINICIO CLAVE: Volvemos a cero el contador antes de mandar a imprimir
            _indiceFilaActual = 0;

            PrintDocument pd = new PrintDocument();
            pd.DefaultPageSettings.Landscape = true;
            pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);

            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = pd;

            if (printDialog.ShowDialog() == DialogResult.OK)
                pd.Print();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            if(!ValidarFechas()) return;

            try
            {
                BitacoraFiltro_08YS filtro = new BitacoraFiltro_08YS
                {
                    Username = txtUsername.Text,
                    TargetUsername = txtTargetUsername.Text,
                    FechaDesde = dtpDesde.Value.HasValue ? dtpDesde.Value.Value.Date : (DateTime?)null,
                    FechaHasta = dtpHasta.Value.HasValue ? dtpHasta.Value.Value.Date.AddDays(1).AddTicks(-1) : (DateTime?)null,
                    Modulo = comboBoxModulo.SelectedItem != null ? (Modulo?)comboBoxModulo.SelectedItem : null,
                    Evento = comboBoxEvento.SelectedItem != null ? (Evento?)comboBoxEvento.SelectedItem : null,
                    Criticidad = comboBoxCriticidad.SelectedItem != null ? (Criticidad?)comboBoxCriticidad.SelectedItem : null
                };

                dgvEventos.DataSource = null;
                dgvEventos.DataSource = _bll.Filtrar(filtro);
                dgvEventos.Columns["FechaHora"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool ValidarFechas()
        {
            DateTime? desde = dtpDesde.Value.HasValue ? dtpDesde.Value.Value.Date : (DateTime?)null;
            DateTime? hasta = dtpHasta.Value.HasValue ? dtpHasta.Value.Value.Date : (DateTime?)null;

            string tituloError = TraductorManager_08YS.Instance.GetTexto("error_validacion");

            if (!desde.HasValue && hasta.HasValue)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_fecha_partida_req"), tituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (desde.HasValue && !hasta.HasValue)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_fecha_fin_req"), tituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (desde.HasValue && desde.Value > DateTime.Today)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_fecha_partida_futura"), tituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (hasta.HasValue && hasta.Value > DateTime.Today)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_fecha_fin_futura"), tituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (desde.HasValue && hasta.HasValue && desde.Value > hasta.Value)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_fechas_invertidas"), tituloError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            txtTargetUsername.Text = "";
            comboBoxModulo.SelectedIndex = -1;
            comboBoxEvento.SelectedIndex = -1;
            comboBoxCriticidad.SelectedIndex = -1;
            dtpDesde.Value = null;
            dtpHasta.Value = null;
            CargarGrid();
        }

        private void comboBoxModulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Modulo? moduloSeleccionado = comboBoxModulo.SelectedItem as Modulo?;
            RefreshComboEventos(moduloSeleccionado);
        }

        private void RefreshComboEventos(Modulo? modulo)
        {
            comboBoxEvento.SelectedIndexChanged -= comboBoxEvento_SelectedIndexChanged;

            Evento? eventoAnterior = comboBoxEvento.SelectedItem as Evento?;

            Criticidad? criticidadActual = comboBoxCriticidad.SelectedItem as Criticidad?;

            var eventos = modulo.HasValue
                ? EventCatalog_08YS.GetEventsByModule(modulo.Value)
                : Enum.GetValues(typeof(Evento)).Cast<Evento>().ToList();

            if (criticidadActual.HasValue)
                eventos = eventos
                    .Where(ev => EventCatalog_08YS.GetMetadata(ev).Criticidad == criticidadActual.Value)
                    .ToList();

            comboBoxEvento.DataSource = eventos;

            if (!eventoAnterior.HasValue || !eventos.Contains(eventoAnterior.Value))
                comboBoxEvento.SelectedIndex = -1;
            else
                comboBoxEvento.SelectedItem = eventoAnterior.Value;

            comboBoxEvento.SelectedIndexChanged += comboBoxEvento_SelectedIndexChanged;
        }

        private void comboBoxEvento_SelectedIndexChanged(object sender, EventArgs e)
        {
            Evento? eventoSeleccionado = comboBoxEvento.SelectedItem as Evento?;
            if (!eventoSeleccionado.HasValue) return;

            var metadata = EventCatalog_08YS.GetMetadata(eventoSeleccionado.Value);

            // Protege módulo contra cascada
            comboBoxModulo.SelectedIndexChanged -= comboBoxModulo_SelectedIndexChanged;
            if ((Modulo?)comboBoxModulo.SelectedItem != metadata.Modulo)
                comboBoxModulo.SelectedItem = metadata.Modulo;
            comboBoxModulo.SelectedIndexChanged += comboBoxModulo_SelectedIndexChanged;

            // Protege criticidad contra cascada
            comboBoxCriticidad.SelectedIndexChanged -= comboBoxCriticidad_SelectedIndexChanged;
            comboBoxCriticidad.SelectedItem = metadata.Criticidad;
            comboBoxCriticidad.SelectedIndexChanged += comboBoxCriticidad_SelectedIndexChanged;
        }

        private void comboBoxCriticidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            Criticidad? criticidadSeleccionada = comboBoxCriticidad.SelectedItem as Criticidad?;
            if (!criticidadSeleccionada.HasValue) return;

            // Respeta el módulo actualmente seleccionado al filtrar por criticidad
            Modulo? moduloActual = comboBoxModulo.SelectedItem as Modulo?;

            var eventos = EventCatalog_08YS.GetEventsByCriticality(criticidadSeleccionada.Value);

            if (moduloActual.HasValue)
                eventos = eventos.Where(ev =>
                    EventCatalog_08YS.GetMetadata(ev).Modulo == moduloActual.Value).ToList();

            comboBoxEvento.SelectedIndexChanged -= comboBoxEvento_SelectedIndexChanged;

            comboBoxEvento.DataSource = eventos;

            var eventoActual = comboBoxEvento.SelectedItem as Evento?;
            if (!eventoActual.HasValue || !eventos.Contains(eventoActual.Value))
                comboBoxEvento.SelectedIndex = -1;

            comboBoxEvento.SelectedIndexChanged += comboBoxEvento_SelectedIndexChanged;
        }

        #region FrontEnd

        private void dgvEventos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvEventos.Columns[e.ColumnIndex].Name == "Criticidad")
            {
                if (e.Value != null)
                {
                    string valor = e.Value.ToString();
                    if (valor == "Critico")
                    {
                        e.CellStyle.ForeColor = Color.Red;
                    }
                    else if (valor == "Alto")
                    {
                        e.CellStyle.ForeColor = Color.OrangeRed;
                    }
                    else if (valor == "Medio")
                    {
                        e.CellStyle.ForeColor = Color.Gold;
                    }
                    else if (valor == "Bajo")
                    {
                        e.CellStyle.ForeColor = Color.LimeGreen;
                    }
                }
            }
        }

        private Color btnBackNormal = Color.FromArgb(5, 15, 45);
        private Color btnBackHover = Color.Goldenrod;

        private Color btnForeNormal = Color.Goldenrod;
        private Color btnForeHover = Color.FromArgb(5, 15, 45);

        private void ConfigurarBoton(IconButton btn)
        {
            btn.BackColor = btnBackNormal;
            btn.ForeColor = btnForeNormal;
            btn.IconColor = btnForeNormal;

            btn.MouseEnter += Boton_MouseEnter;
            btn.MouseLeave += Boton_MouseLeave;
        }

        private void Boton_MouseEnter(object sender, EventArgs e)
        {
            IconButton btn = (IconButton)sender;

            btn.BackColor = btnBackHover;
            btn.ForeColor = btnForeHover;
            btn.IconColor = btnForeHover;
        }

        private void Boton_MouseLeave(object sender, EventArgs e)
        {
            IconButton btn = (IconButton)sender;

            btn.BackColor = btnBackNormal;
            btn.ForeColor = btnForeNormal;
            btn.IconColor = btnForeNormal;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        #endregion
    }
}

