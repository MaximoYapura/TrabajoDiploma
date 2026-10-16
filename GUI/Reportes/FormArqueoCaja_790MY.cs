using BE_08YS;
using BLL_08YS;
using FontAwesome.Sharp;
using GUI_08YS.Maestros;
using GUI_08YS.RF2;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using E = GUI_08YS.RF2.EstiloRF2_790MY;

namespace GUI_08YS.Reportes
{
    /// <summary>
    /// Reporte gerencial de Arqueo y Cierre de Caja: totales de la jornada (efectivo,
    /// tarjetas, propinas, descuentos y recaudación neta) y detalle de las cuentas
    /// cobradas. "Cerrar caja del día" deja constancia del cierre en la bitácora
    /// (PagoBLL_790MY.CerrarCaja). Patente: GestionCaja.
    /// </summary>
    public partial class FormArqueoCaja_790MY : Form, IIdiomaObserver_08YS
    {
        private static readonly CultureInfo CulturaMoneda = CultureInfo.GetCultureInfo("es-AR");
        private static readonly Color RojoImporte = Color.FromArgb(150, 30, 30);

        private readonly PagoBLL_790MY _pagoBll;

        private static readonly Dictionary<string, Permisos> _mapaPermisos =
            new Dictionary<string, Permisos>
            {
                { nameof(btnCerrarCaja_790MY), Permisos.GestionCaja },
            };

        /// <summary>Tarjeta KPI: clave del título, ícono y si es la destacada (recaudación neta).</summary>
        private sealed class Kpi
        {
            public string ClaveTitulo;
            public IconChar Icono;
            public bool Destacada;
            public bool EnRojo;
            public Label Titulo;
            public Label Valor;
        }

        private readonly List<Kpi> _kpis = new List<Kpi>
        {
            new Kpi { ClaveTitulo = "AC_kpi_efectivo",   Icono = IconChar.MoneyBill },
            new Kpi { ClaveTitulo = "AC_kpi_tarjetas",   Icono = IconChar.CreditCard },
            new Kpi { ClaveTitulo = "AC_kpi_propinas",   Icono = IconChar.Heart },
            new Kpi { ClaveTitulo = "AC_kpi_descuentos", Icono = IconChar.Percent, EnRojo = true },
            new Kpi { ClaveTitulo = "AC_kpi_neta",       Icono = IconChar.ArrowTrendUp, Destacada = true },
        };

        private readonly Font _fKpiTitulo = new Font("Segoe UI", 10.5F);
        private readonly Font _fKpiValor = new Font("Segoe UI", 21F, FontStyle.Bold);
        private readonly Font _fImporte = new Font("Segoe UI", 10.5F, FontStyle.Bold);

        private ReporteArqueoCaja_790MY _reporte;
        private bool _listo;

        public FormArqueoCaja_790MY()
        {
            InitializeComponent();
            _pagoBll = BLLFactory_790MY.CreatePagoBLL();

            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) =>
            {
                TraductorManager_08YS.Instance.Desuscribir(this);
                _fKpiTitulo.Dispose();
                _fKpiValor.Dispose();
                _fImporte.Dispose();
            };
        }

        private IWin32Window VentanaPropietaria => (this.TopLevelControl as Form) ?? (IWin32Window)this;

        private void FormArqueoCaja_790MY_Load(object sender, EventArgs e)
        {
            PermissionFilter_08YS.Aplicar(this, _mapaPermisos);

            EstiloMaestros_790MY.EstilizarGrilla(dgvCobros_790MY);
            dgvCobros_790MY.ColumnHeadersDefaultCellStyle.BackColor = E.Bordo;
            dgvCobros_790MY.ColumnHeadersDefaultCellStyle.SelectionBackColor = E.Bordo;
            dgvCobros_790MY.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(251, 248, 243);
            foreach (var col in new[] { colDescuento_790MY, colPropina_790MY, colImporte_790MY })
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            colImporte_790MY.DefaultCellStyle.Font = _fImporte;

            CrearTarjetasKpi();

            dtpFecha_790MY.ValueChanged -= dtpFecha_790MY_ValueChanged;
            dtpFecha_790MY.Value = DateTime.Today;
            dtpFecha_790MY.ValueChanged += dtpFecha_790MY_ValueChanged;

            _listo = true;
            UpdateIdioma();
            Cargar();
        }

        #region Datos

        private void Cargar()
        {
            try
            {
                _reporte = _pagoBll.GenerarReporteArqueoCaja(dtpFecha_790MY.Value.Date);
            }
            catch (Exception ex)
            {
                _reporte = null;
                E.MostrarError(VentanaPropietaria, ex);
            }
            Mostrar();
        }

        private void Mostrar()
        {
            var r = _reporte ?? new ReporteArqueoCaja_790MY { Fecha = dtpFecha_790MY.Value.Date };

            _kpis[0].Valor.Text = Moneda(r.TotalEfectivo);
            _kpis[1].Valor.Text = Moneda(r.TotalTarjetas);
            _kpis[2].Valor.Text = Moneda(r.TotalPropinas);
            _kpis[3].Valor.Text = Moneda(r.TotalDescuentos);
            _kpis[4].Valor.Text = Moneda(r.RecaudacionNeta);

            string fecha = r.Fecha.ToString("dddd dd/MM/yyyy", TraductorManager_08YS.Instance.CulturaActual);
            lblResumen_790MY.Text = E.T("AC_resumen", fecha, r.CantidadCuentas, Moneda(r.TicketPromedio));

            dgvCobros_790MY.Rows.Clear();
            foreach (var c in r.Cobros)
            {
                int fila = dgvCobros_790MY.Rows.Add(
                    c.FechaHoraPago.ToString("HH:mm"),
                    E.T("AC_mesa_nro", c.MesaNumero),
                    c.MetodoPago == MetodoPago_790MY.Efectivo ? E.T("CJ_efectivo").Trim() : E.T("CJ_tarjeta").Trim(),
                    c.MontoDescuento == 0 ? "—" : "- " + Moneda(c.MontoDescuento),
                    c.MontoPropina == 0 ? "—" : Moneda(c.MontoPropina),
                    Moneda(c.ImporteCobrado));
                dgvCobros_790MY.Rows[fila].Tag = c;
                if (c.MontoDescuento > 0)
                    dgvCobros_790MY.Rows[fila].Cells[colDescuento_790MY.Index].Style.ForeColor = RojoImporte;
            }
            dgvCobros_790MY.ClearSelection();

            lblSinCobros_790MY.Text = E.T("msg_ac_sin_cobros", r.Fecha.ToString("dd/MM/yyyy"));
            lblSinCobros_790MY.Visible = r.CantidadCuentas == 0;
            UbicarMensajeVacio();
            btnCerrarCaja_790MY.Enabled = r.CantidadCuentas > 0;
            btnSiguiente_790MY.Enabled = dtpFecha_790MY.Value.Date < DateTime.Today;
        }

        private void dtpFecha_790MY_ValueChanged(object sender, EventArgs e) => Cargar();

        private void btnAnterior_790MY_Click(object sender, EventArgs e)
            => dtpFecha_790MY.Value = dtpFecha_790MY.Value.Date.AddDays(-1);

        private void btnSiguiente_790MY_Click(object sender, EventArgs e)
        {
            if (dtpFecha_790MY.Value.Date < DateTime.Today)
                dtpFecha_790MY.Value = dtpFecha_790MY.Value.Date.AddDays(1);
        }

        private void btnCerrarCaja_790MY_Click(object sender, EventArgs e)
        {
            DateTime fecha = dtpFecha_790MY.Value.Date;
            var respuesta = MessageBox.Show(VentanaPropietaria,
                E.T("msg_ac_confirmar_cierre", fecha.ToString("dd/MM/yyyy")),
                E.T("AC_btn_cerrar_caja").Trim(), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (respuesta != DialogResult.Yes) return;

            try
            {
                ReporteArqueoCaja_790MY cierre = _pagoBll.CerrarCaja(fecha);
                _reporte = cierre;
                Mostrar();
                E.MostrarExito(VentanaPropietaria, E.T("msg_ac_cierre_ok", fecha.ToString("dd/MM/yyyy"),
                    cierre.CantidadCuentas, Moneda(cierre.TotalEfectivo), Moneda(cierre.TotalTarjetas),
                    Moneda(cierre.TotalPropinas), Moneda(cierre.RecaudacionNeta)));
            }
            catch (Exception ex)
            {
                E.MostrarError(VentanaPropietaria, ex);
            }
        }

        #endregion

        #region Presentación

        private void CrearTarjetasKpi()
        {
            tlpKpis_790MY.SuspendLayout();
            for (int i = 0; i < _kpis.Count; i++)
            {
                Kpi kpi = _kpis[i];
                Color fondo = kpi.Destacada ? E.Bordo : Color.White;
                Color texto = kpi.Destacada ? Color.White : E.Texto;

                var card = new PanelRedondeado_790MY
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(i == 0 ? 0 : 8, 0, i == _kpis.Count - 1 ? 0 : 8, 0),
                    BackColor = fondo,
                    ColorBorde = kpi.Destacada ? E.Bordo : E.BordeCard,
                    Radio = 10,
                    Padding = new Padding(18, 16, 12, 12)
                };

                var icono = new IconPictureBox
                {
                    IconChar = kpi.Icono,
                    IconColor = kpi.Destacada ? Color.White : E.Gris,
                    IconSize = 22,
                    BackColor = Color.Transparent,
                    Size = new Size(24, 24),
                    Location = new Point(18, 18)
                };
                kpi.Titulo = E.CrearLabel(E.T(kpi.ClaveTitulo), _fKpiTitulo, texto);
                kpi.Titulo.Location = new Point(48, 19);
                kpi.Valor = E.CrearLabel("$ 0", _fKpiValor,
                                         kpi.EnRojo ? RojoImporte : texto);
                kpi.Valor.Location = new Point(14, 54);

                card.Controls.Add(icono);
                card.Controls.Add(kpi.Titulo);
                card.Controls.Add(kpi.Valor);
                tlpKpis_790MY.Controls.Add(card, i, 0);
            }
            tlpKpis_790MY.ResumeLayout();
        }

        private void dgvCobros_790MY_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // Medio de pago como píldora (efectivo verde / tarjeta azul).
            if (e.RowIndex < 0 || e.ColumnIndex != colMedio_790MY.Index) return;
            if (!(dgvCobros_790MY.Rows[e.RowIndex].Tag is CobroArqueo_790MY cobro)) return;

            bool efectivo = cobro.MetodoPago == MetodoPago_790MY.Efectivo;
            EstiloMaestros_790MY.PintarPildora(e, e.FormattedValue?.ToString(),
                efectivo ? E.ListoFondo : Color.FromArgb(226, 236, 250),
                efectivo ? E.ListoTexto : Color.FromArgb(30, 70, 140));
        }

        private void dgvCobros_790MY_Resize(object sender, EventArgs e) => UbicarMensajeVacio();

        private void UbicarMensajeVacio()
        {
            int top = dgvCobros_790MY.ColumnHeadersHeight;
            lblSinCobros_790MY.Bounds = new Rectangle(0, top, dgvCobros_790MY.ClientSize.Width,
                                                      Math.Max(0, dgvCobros_790MY.ClientSize.Height - top));
        }

        private static string Moneda(decimal valor)
            => "$ " + valor.ToString(valor == decimal.Truncate(valor) ? "N0" : "N2", CulturaMoneda);

        #endregion

        #region i18n

        public void UpdateIdioma()
        {
            E.TraducirTags(this);
            colHora_790MY.HeaderText = E.T("AC_col_hora");
            colMesa_790MY.HeaderText = E.T("AC_col_mesa");
            colMedio_790MY.HeaderText = E.T("AC_col_medio");
            colDescuento_790MY.HeaderText = E.T("AC_col_descuento");
            colPropina_790MY.HeaderText = E.T("AC_col_propina");
            colImporte_790MY.HeaderText = E.T("AC_col_importe");

            if (!_listo) return;
            foreach (var kpi in _kpis)
                if (kpi.Titulo != null) kpi.Titulo.Text = E.T(kpi.ClaveTitulo);
            Mostrar();
        }

        #endregion
    }
}
