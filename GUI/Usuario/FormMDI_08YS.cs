using BLL_08YS;
using FontAwesome.Sharp;
using GUI;
using GUI_08YS.Admin;
using GUI_08YS.RF1;
using GUI_08YS.RF2;
using GUI_08YS.Estilo;
using GUI_08YS.Maestros;
using GUI_08YS.Reportes;
using GUI_08YS.Properties;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_08YS
{
    public partial class FormMDI_08YS : Form,IIdiomaObserver_08YS
    {
        private static readonly Dictionary<string, Permisos> _mapaMenuAdmin =
            new Dictionary<string, Permisos>
            {
                { nameof(gestionUsuarioToolStripMenuItem),   Permisos.VerUsuarios    },
                { nameof(bitacoraToolStripMenuItem),         Permisos.VerBitacora    },
                { nameof(familiasToolStripMenuItem),         Permisos.VerFamilias    },
                { nameof(rolesToolStripMenuItem),            Permisos.VerRoles       },
                { nameof(gestionRespaldosToolStripMenuItem), Permisos.VerRespaldos   },
                { nameof(bitacoraCambiosToolStripMenuItem_790MY), Permisos.AuditarProductos },
            };

        private static readonly Dictionary<string, Permisos> _mapaMenuMaestros =
            new Dictionary<string, Permisos>
            {
                { nameof(clientesToolStripMenuItem), Permisos.VerClientes },
                { nameof(mesasToolStripMenuItem),    Permisos.VerMesas    },
                { nameof(empleadosToolStripMenuItem_790MY), Permisos.CrearEmpleado    },
                { nameof(productosToolStripMenuItem_790MY), Permisos.GestionProductos },
            };

        private static readonly Dictionary<string, Permisos> _mapaMenuReservas =
            new Dictionary<string, Permisos>
            {
                { nameof(registrarReservaToolStripMenuItem), Permisos.RegistrarReserva },
            };

        // RF2 Atención en mesa: una patente por caso de uso (CUN-004, CUN-005, CUN-008).
        private static readonly Dictionary<string, Permisos> _mapaMenuAtencion =
            new Dictionary<string, Permisos>
            {
                { nameof(abrirMesaToolStripMenuItem_790MY),       Permisos.AbrirMesa       },
                { nameof(registrarPedidoToolStripMenuItem_790MY), Permisos.RegistrarPedido },
                { nameof(despacharPedidoToolStripMenuItem_790MY), Permisos.DespacharPedido },
            };

        private static readonly Dictionary<string, Permisos> _mapaMenuReportes =
            new Dictionary<string, Permisos>
            {
                { nameof(reporteReservasToolStripMenuItem),  Permisos.VerReservas },
                { nameof(arqueoCajaToolStripMenuItem_790MY), Permisos.GestionCaja },
                { nameof(reportePredictivoToolStripMenuItem_790MY), Permisos.GestionReportePredictivo },
            };

        // Perfil: Relogin solo para roles con acceso administrativo (VerUsuarios).
        // VerBitacora implica el mismo nivel de privilegio, pero PermissionFilter mapea
        // un ítem a un único permiso; usamos VerUsuarios como permiso representativo del
        // perfil admin. Si tu modelo de roles separa VerBitacora de VerUsuarios, considera
        // añadir lógica OR directa debajo del AplicarMenuStrip.
        private static readonly Dictionary<string, Permisos> _mapaMenuPerfil =
            new Dictionary<string, Permisos>
            {
                { nameof(ReLoginToolStripMenuItem), Permisos.VerUsuarios },
            };

        public event Action CerrarSesion;
        private UserBLL_08YS _userBLL;

        public FormMDI_08YS()
        {
            InitializeComponent();
            _userBLL = BLLFactory_08YS.CreateUserBLL();
            lblRolSistema.Text = SessionManager_08YS.Instance.Current.Rol.Nombre;
            lblNombreApellido.Text= SessionManager_08YS.Instance.Current.Nombre + " " + SessionManager_08YS.Instance.Current.Apellido;
            ConfigurarCombo();
            AplicarPermisos();
            panelAyuda.Preparar();
            TraductorManager_08YS.Instance.CambiarIdioma(SessionManager_08YS.Instance.Current.Idioma);
            TraductorManager_08YS.Instance.Suscribir(this);
            SessionManager_08YS.Instance.SesionInvalidada += OnSesionInvalidada;
            UpdateIdioma();

            // Icono corporativo: barra de tareas y Alt+Tab (el formulario no tiene barra de título).
            this.Icon = Resources.FogonIcon;
        }

        private void OnSesionInvalidada()
        {
            MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_sesion_invalidada"),
                            TraductorManager_08YS.Instance.GetTexto("sesion_invalidada"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
            this.Close();
        }

        private void ConfigurarCombo()
        {
            RefrescarContenidoComboIdioma();
        }

        private void RefrescarContenidoComboIdioma()
        {
            IdiomaCombobox.SelectedIndexChanged -= IdiomaCombobox_SelectedIndexChanged;

            int indexTemporal = IdiomaCombobox.SelectedIndex;

            IdiomaCombobox.Items.Clear();
            IdiomaCombobox.Items.Add(TraductorManager_08YS.Instance.GetTexto("idioma_es"));
            IdiomaCombobox.Items.Add(TraductorManager_08YS.Instance.GetTexto("idioma_en"));

            if (indexTemporal < 0)
                IdiomaCombobox.SelectedIndex = (SessionManager_08YS.Instance.Current.Idioma == "en") ? 1 : 0;
            else
                IdiomaCombobox.SelectedIndex = indexTemporal;

            IdiomaCombobox.SelectedIndexChanged += IdiomaCombobox_SelectedIndexChanged;
        }

        private void AplicarPermisos()
        {
            btnAdministrativo.Visible = SessionManager_08YS.Instance.HasPermission(Permisos.VerUsuarios)
                                     || SessionManager_08YS.Instance.HasPermission(Permisos.VerBitacora)
                                     || SessionManager_08YS.Instance.HasPermission(Permisos.VerFamilias)
                                     || SessionManager_08YS.Instance.HasPermission(Permisos.VerRoles)
                                     || SessionManager_08YS.Instance.HasPermission(Permisos.AuditarProductos);

            gestionAccesosToolStripMenuItem.Visible = SessionManager_08YS.Instance.HasPermission(Permisos.VerRoles)
                                                   || SessionManager_08YS.Instance.HasPermission(Permisos.VerFamilias);

            PermissionFilter_08YS.AplicarMenuStrip(AdministrativoDropDownMenu, _mapaMenuAdmin);

            // Maestros: visible si el rol tiene acceso al módulo de maestros
            // (o si puede gestionar empleados / la carta, aunque no tenga el resto de Maestros)
            btnMaestros.Visible = SessionManager_08YS.Instance.HasPermission(Permisos.VerMaestros)
                               || SessionManager_08YS.Instance.HasPermission(Permisos.CrearEmpleado)
                               || SessionManager_08YS.Instance.HasPermission(Permisos.GestionProductos);

            PermissionFilter_08YS.AplicarMenuStrip(MaestrosDropDownMenu, _mapaMenuMaestros);

            // Reservar: visible si tiene permiso para registrar reservas
            btnReservar.Visible = SessionManager_08YS.Instance.HasPermission(Permisos.RegistrarReserva);

            PermissionFilter_08YS.AplicarMenuStrip(ReservasDropDownMenu, _mapaMenuReservas);

            // Atención en Mesa: visible si tiene al menos una de las patentes de RF2
            bool accesoCaja = SessionManager_08YS.Instance.HasPermission(Permisos.GenerarFactura)
                           || SessionManager_08YS.Instance.HasPermission(Permisos.RegistrarCobro);

            btnAtencion_790MY.Visible = SessionManager_08YS.Instance.HasPermission(Permisos.AbrirMesa)
                                     || SessionManager_08YS.Instance.HasPermission(Permisos.RegistrarPedido)
                                     || SessionManager_08YS.Instance.HasPermission(Permisos.DespacharPedido)
                                     || accesoCaja;

            PermissionFilter_08YS.AplicarMenuStrip(AtencionDropDownMenu_790MY, _mapaMenuAtencion);

            // Caja y cobro (CUN-006 / CUN-007): alcanza con cualquiera de las dos patentes.
            cajaCobroToolStripMenuItem_790MY.Visible = accesoCaja;

            // Reportes: visible si el rol tiene acceso al módulo de reportes, al arqueo de caja
            // o al análisis predictivo de canasta
            btnReportes_790MY.Visible = SessionManager_08YS.Instance.HasPermission(Permisos.VerReportes)
                                     || SessionManager_08YS.Instance.HasPermission(Permisos.GestionCaja)
                                     || SessionManager_08YS.Instance.HasPermission(Permisos.GestionReportePredictivo);

            PermissionFilter_08YS.AplicarMenuStrip(ReportesDropDownMenu, _mapaMenuReportes);

            // Perfil: aplica filtro de permisos sobre el ContextMenuStrip del Perfil.
            // _mapaMenuPerfil mapea Relogin → VerUsuarios.
            // Si el rol también habilita Relogin por VerBitacora (sin VerUsuarios),
            // la línea siguiente restaura la visibilidad para ese caso.
            PermissionFilter_08YS.AplicarMenuStrip(PerfilDropDownMenu, _mapaMenuPerfil);
            if (SessionManager_08YS.Instance.HasPermission(Permisos.VerBitacora))
                ReLoginToolStripMenuItem.Visible = true;
        }

        #region idioma
        public void UpdateIdioma()
        {
            TraducirControles(this);

            // lblRolSistema no tiene Tag; traducirlo explícitamente con la clave "rol_<Nombre>"
            var rolNombre = SessionManager_08YS.Instance.Current?.Rol?.Nombre;
            if (!string.IsNullOrEmpty(rolNombre))
            {
                string rolTrad = TraductorManager_08YS.Instance.GetTexto("rol_" + rolNombre);
                lblRolSistema.Text = string.IsNullOrEmpty(rolTrad) ? rolNombre : rolTrad;
            }

            if (AdministrativoDropDownMenu != null)
                TraducirMenuFlotanteCustom(AdministrativoDropDownMenu);

            if (MaestrosDropDownMenu != null)
                TraducirMenuFlotanteCustom(MaestrosDropDownMenu);

            if (ReservasDropDownMenu != null)
                TraducirMenuFlotanteCustom(ReservasDropDownMenu);

            if (ReportesDropDownMenu != null)
                TraducirMenuFlotanteCustom(ReportesDropDownMenu);

            if (AtencionDropDownMenu_790MY != null)
                TraducirMenuFlotanteCustom(AtencionDropDownMenu_790MY);

            if (PerfilDropDownMenu != null)
                TraducirMenuFlotanteCustom(PerfilDropDownMenu);

            // La fila de Ayuda no usa Tag: traduce su texto, ToolTip y nombre accesible.
            btnAyuda.UpdateIdioma();
            panelAyuda.UpdateIdioma();
            _sugerencias?.Cerrar();

            RefrescarContenidoComboIdioma();
        }

        private void TraducirMenuFlotanteCustom(ContextMenuStrip menuFlotante)
        {
            foreach (ToolStripItem item in menuFlotante.Items)
                TraducirItemsDesplegables(item);
        }

        private void TraducirControles(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                if (c.HasChildren && !(c is ToolStrip))
                    TraducirControles(c);

                if (c is ToolStrip barraMenu)
                {
                    TraducirBarraHerramientas(barraMenu);
                    continue;
                }

                if (c is IconButton botonIcono)
                {
                    if (botonIcono.Tag != null && !string.IsNullOrWhiteSpace(botonIcono.Tag.ToString()))
                        botonIcono.Text = TraductorManager_08YS.Instance.GetTexto(botonIcono.Tag.ToString());
                    continue;
                }

                if (c.Tag != null && !string.IsNullOrWhiteSpace(c.Tag.ToString()))
                    c.Text = TraductorManager_08YS.Instance.GetTexto(c.Tag.ToString());
            }
        }

        private void TraducirBarraHerramientas(ToolStrip barra)
        {
            foreach (ToolStripItem item in barra.Items)
                TraducirItemsDesplegables(item);
        }

        private void TraducirItemsDesplegables(ToolStripItem item)
        {
            if (item.Tag != null && !string.IsNullOrWhiteSpace(item.Tag.ToString()))
                item.Text = TraductorManager_08YS.Instance.GetTexto(item.Tag.ToString());

            if (item is ToolStripDropDownItem itemDesplegable && itemDesplegable.HasDropDownItems)
            {
                foreach (ToolStripItem subItem in itemDesplegable.DropDownItems)
                    TraducirItemsDesplegables(subItem);
            }
        }
        #endregion

        private void FormMDI_Load(object sender, EventArgs e)
        {
            AdministrativoDropDownMenu.IsMainMenu = true;
            MaestrosDropDownMenu.IsMainMenu = true;
            ReservasDropDownMenu.IsMainMenu = true;
            ReportesDropDownMenu.IsMainMenu = true;
            AtencionDropDownMenu_790MY.IsMainMenu = true;
            PerfilDropDownMenu.IsMainMenu = true;

            // Marca de agua corporativa sobre el área de contenido (panel2)
            panel2.Paint += Panel2_PintarMarcaDeAgua;
        }

        /// <summary>
        /// Dibuja el logo de El Fogón del Sur centrado sobre panel2,
        /// escalado al 40 % del ancho del área de contenido, con opacidad reducida
        /// para no interferir con los formularios hijos.
        /// </summary>
        private void Panel2_PintarMarcaDeAgua(object sender, PaintEventArgs e)
        {
            Bitmap img;
            try   { img = Resources.LogoElFogon; }
            catch { return; }

            if (img == null) return;

            var panel = (Panel)sender;

            // Si hay un formulario hijo abierto, no dibujar la marca de agua
            if (panel.Controls.Count > 0) return;

            int w = (int)(panel.ClientSize.Width * 0.40);
            if (w <= 0) return;
            int h = (int)(img.Height * ((float)w / Math.Max(1, img.Width)));
            int x = (panel.ClientSize.Width  - w) / 2;
            int y = (panel.ClientSize.Height - h) / 2;

            // Dibujar con opacidad al 12 % (Matrix33 controla el canal alfa)
            using (var attr = new ImageAttributes())
            {
                var cm = new ColorMatrix { Matrix33 = 0.12f };
                attr.SetColorMatrix(cm);
                e.Graphics.DrawImage(
                    img,
                    new Rectangle(x, y, w, h),
                    0, 0, img.Width, img.Height,
                    GraphicsUnit.Pixel,
                    attr);
            }
        }

        /// <summary>
        /// Etapa 4 (E4-D6): mientras un formulario hijo ejecuta una operación que no debe
        /// interrumpirse (el cobro), se bloquean el menú lateral (incluido cerrar sesión), el
        /// cambio de idioma y el cierre de la aplicación. panel2 queda habilitado para que el
        /// formulario hijo siga respondiendo.
        /// </summary>
        public void BloquearNavegacion(bool bloquear)
        {
            _navegacionBloqueada = bloquear;
            panelLateral.Enabled = !bloquear;
            panelIdioma.Enabled = !bloquear;
            // Etapa 5 (A4): la ayuda no queda abierta durante el cobro.
            if (bloquear)
            {
                _sugerencias?.Cerrar();
                if (panelAyuda.Visible) CerrarAyuda();
            }
        }

        /// <summary>Se dispara cuando se canceló un cierre por <see cref="BloquearNavegacion"/>; el hijo avisa el motivo.</summary>
        public event Action CierreBloqueado;

        private bool _navegacionBloqueada;

        private void FormMDI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_navegacionBloqueada && e.CloseReason != CloseReason.WindowsShutDown)
            {
                e.Cancel = true;
                CierreBloqueado?.Invoke();
                return;
            }

            SessionManager_08YS.Instance.SesionInvalidada -= OnSesionInvalidada;

            if (SessionManager_08YS.Instance.IsLogged)
            {
                _userBLL.Logout();
                CerrarSesion?.Invoke();
            }
        }

        public void OpenChildForm(Form childForm)
        {
            panel2.Controls.Clear();
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            childForm.AutoScaleMode = AutoScaleMode.None;

            panel2.SuspendLayout();
            childForm.SuspendLayout();

            panel2.Controls.Add(childForm);
            panel2.Tag = childForm;

            ForceCustomControlsLayout(childForm);

            childForm.ResumeLayout(false);
            panel2.ResumeLayout(false);

            if(childForm is IIdiomaObserver_08YS observer)
            {
                TraductorManager_08YS.Instance.Suscribir(observer);
                observer.UpdateIdioma();
            }

            childForm.Load += (s, e) => TraductorManager_08YS.Instance.Suscribir(childForm as IIdiomaObserver_08YS);
            childForm.FormClosed += (s, e) => TraductorManager_08YS.Instance.Desuscribir(childForm as IIdiomaObserver_08YS);

            childForm.Show();
            childForm.Refresh();
        }

        private void ForceCustomControlsLayout(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl.HasChildren)
                    ForceCustomControlsLayout(ctrl);

                var onResize = ctrl.GetType().GetMethod("OnResize",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);
                onResize?.Invoke(ctrl, new object[] { EventArgs.Empty });

                if (ctrl is FontAwesome.Sharp.IconButton iconBtn)
                {
                    var updateImage = iconBtn.GetType()
                        .GetMethod("UpdateImage",
                            System.Reflection.BindingFlags.NonPublic |
                            System.Reflection.BindingFlags.Instance |
                            System.Reflection.BindingFlags.FlattenHierarchy);
                    updateImage?.Invoke(iconBtn, null);
                    iconBtn.Invalidate();
                    iconBtn.Update();
                }

                if (ctrl is Label lbl && lbl.AutoSize)
                {
                    lbl.MaximumSize = Size.Empty;
                    lbl.Size = Size.Empty;
                    lbl.Refresh();
                }
                if (ctrl is CustomControls.IconPlaceholderTextBox txt)
                {
                    bool hasText = !string.IsNullOrEmpty(txt.RealText);
                    txt.InnerTextBox.Visible = hasText || txt.InnerTextBox.Focused;
                    txt.Invalidate();
                }
            }
        }

        #region Menu

        #region Administrativo
        private void btnAdministrativo_Click(object sender, EventArgs e)
        {
            AdministrativoDropDownMenu.Show(btnAdministrativo, btnAdministrativo.Width, 0);
        }

        private void gestionUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerUsuarios);
            OpenChildForm(new FormGestionUsuarios_08YS());
        }

        private void bitacoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerBitacora);
            OpenChildForm(new FormBitacora_08YS());
        }

        private void familiasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerFamilias);
            OpenChildForm(new FormGestionAcceso_08YS(TipoEntidad.Familia, OpenChildForm));
        }

        private void rolesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerRoles);
            OpenChildForm(new FormGestionAcceso_08YS(TipoEntidad.Rol, OpenChildForm));
        }

        private void gestionRespaldosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerRespaldos);
            OpenChildForm(new FormGestionRespaldos_08YS());
        }

        private void bitacoraCambiosToolStripMenuItem_790MY_Click(object sender, EventArgs e)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.AuditarProductos);
            OpenChildForm(new FormBitacoraCambios_790MY());
        }
        #endregion

        #region Maestros
        private void btnMaestros_Click(object sender, EventArgs e)
        {
            MaestrosDropDownMenu.Show(btnMaestros, btnMaestros.Width, 0);
        }

        private void clientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerClientes);
            OpenChildForm(new FormClientes_790MY());
        }

        private void mesasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerMesas);
            OpenChildForm(new FormMesas_790MY());
        }

        private void empleadosToolStripMenuItem_790MY_Click(object sender, EventArgs e)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearEmpleado);
            OpenChildForm(new FormEmpleados_790MY());
        }

        private void productosToolStripMenuItem_790MY_Click(object sender, EventArgs e)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.GestionProductos);
            OpenChildForm(new FormProductos_790MY());
        }
        #endregion

        #region Reservas
        private void btnReservar_Click(object sender, EventArgs e)
        {
            ReservasDropDownMenu.Show(btnReservar, btnReservar.Width, 0);
        }

        private void registrarReservaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.RegistrarReserva);
            OpenChildForm(new FormRegistrarReserva_790MY());
        }

        #endregion

        #region Atención en Mesa
        private void btnAtencion_790MY_Click(object sender, EventArgs e)
        {
            AtencionDropDownMenu_790MY.Show(btnAtencion_790MY, btnAtencion_790MY.Width, 0);
        }

        private void abrirMesaToolStripMenuItem_790MY_Click(object sender, EventArgs e)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.AbrirMesa);
            OpenChildForm(new FormAbrirMesa_790MY());
        }

        private void registrarPedidoToolStripMenuItem_790MY_Click(object sender, EventArgs e)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.RegistrarPedido);
            OpenChildForm(new FormRegistrarPedido_790MY());
        }

        private void despacharPedidoToolStripMenuItem_790MY_Click(object sender, EventArgs e)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.DespacharPedido);
            OpenChildForm(new FormCocinaKDS_790MY());
        }

        private void cajaCobroToolStripMenuItem_790MY_Click(object sender, EventArgs e)
        {
            if (!SessionManager_08YS.Instance.HasPermission(Permisos.RegistrarCobro))
                SessionManager_08YS.Instance.ValidatePermission(Permisos.GenerarFactura);
            OpenChildForm(new FormFacturacionCobro_790MY());
        }
        #endregion

        #region Reportes
        private void btnReportes_790MY_Click(object sender, EventArgs e)
        {
            ReportesDropDownMenu.Show(btnReportes_790MY, btnReportes_790MY.Width, 0);
        }

        private void reporteReservasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerReservas);
            OpenChildForm(new FormReporteReservas_790MY());
        }

        private void arqueoCajaToolStripMenuItem_790MY_Click(object sender, EventArgs e)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.GestionCaja);
            OpenChildForm(new FormArqueoCaja_790MY());
        }

        private void reportePredictivoToolStripMenuItem_790MY_Click(object sender, EventArgs e)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.GestionReportePredictivo);
            OpenChildForm(new FormReportePredictivo_790MY());
        }
        #endregion

        #region Perfil
        private void btnPerfil_Click(object sender, EventArgs e)
        {
            PerfilDropDownMenu.Show(btnPerfil, btnPerfil.Width, 0);
        }

        private void cambiarContraseñaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCambiarContraseña_08YS form = new FormCambiarContraseña_08YS();

            form.FormClosed += (s, args) =>
            {
                if (form.DialogResult == DialogResult.OK)
                {
                    CerrarSesion?.Invoke();
                    this.Close();
                }
            };
            OpenChildForm(form);
        }

        private void ReLoginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLogin_08YS frmRelogin = new FormLogin_08YS(true);
            frmRelogin.ShowDialog();
        }

        private void cerrarSesionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var respuesta = MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_cerrar_sesion"),
                                        TraductorManager_08YS.Instance.GetTexto("cerrar_sesion"),
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                CerrarSesion?.Invoke();
                this.Close();
            }
        }
        #endregion

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            var respuesta = MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_cerrar_sesion"),
                                       TraductorManager_08YS.Instance.GetTexto("cerrar_sesion"),
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                _userBLL.Logout();
                CerrarSesion?.Invoke();
                this.Close();
            }
        }

        #endregion

        #region Ayuda
        /// <summary>
        /// F1 desde cualquier pantalla (spike B1: llega aquí con el foco dentro del hijo
        /// embebido). Se consume aunque la navegación esté bloqueada. No se usa
        /// HelpRequested: Windows lo envía también con un ConfirmModal abierto.
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F1)
            {
                if (!_navegacionBloqueada) AbrirAyuda(contextual: true);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void btnAyuda_Click(object sender, EventArgs e)
        {
            if (!_navegacionBloqueada) AbrirAyuda(contextual: false);
        }

        /// <summary>Control con el foco antes de abrir el panel, para devolvérselo al cerrar.</summary>
        private Control _focoAntesDeAyuda;

        private AyudaSugerencias_790MY _sugerencias;

        private AyudaSugerencias_790MY Sugerencias =>
            _sugerencias ?? (_sugerencias = new AyudaSugerencias_790MY(this, panel2, globoAyuda,
                pnlResalteArriba, pnlResalteAbajo, pnlResalteIzquierda, pnlResalteDerecha));

        /// <summary>
        /// F1 (contextual): con el panel abierto lo cierra; con sugerencias activas las cierra;
        /// si la pantalla activa implementa IAyudaContextual_790MY, muestra sus sugerencias;
        /// si no, abre el panel. La fila de Ayuda (no contextual) siempre alterna el panel.
        /// </summary>
        private void AbrirAyuda(bool contextual)
        {
            if (Sugerencias.Activas)
            {
                Sugerencias.Cerrar();
                if (contextual) return;
            }
            if (panelAyuda.Visible)
            {
                CerrarAyuda();
                return;
            }
            if (contextual && panel2.Tag is IAyudaContextual_790MY pantalla && panel2.Tag is Form hijo
                && panel2.Controls.Contains(hijo)
                && Sugerencias.Mostrar(hijo, pantalla.ObtenerPistas(), ControlConFoco(this)))
                return;
            MostrarAyuda();
        }

        private void panel2_ControlRemoved(object sender, ControlEventArgs e) => _sugerencias?.Cerrar();

        private void FormMDI_Resize(object sender, EventArgs e) => _sugerencias?.Cerrar();

        private void MostrarAyuda()
        {
            _focoAntesDeAyuda = ControlConFoco(this);
            panelAyuda.Visible = true;
            panelAyuda.AlMostrar();
        }

        private void CerrarAyuda()
        {
            bool tenia = panelAyuda.ContainsFocus;
            panelAyuda.Visible = false;
            if (!tenia) return;
            var previo = _focoAntesDeAyuda;
            _focoAntesDeAyuda = null;
            if (previo != null && !previo.IsDisposed && previo.CanFocus)
                previo.Focus();
            else if (panel2.Tag is Form hijo && !hijo.IsDisposed)
                hijo.Focus();
        }

        private static Control ControlConFoco(ContainerControl contenedor)
        {
            Control c = contenedor.ActiveControl;
            while (c is ContainerControl interno && interno.ActiveControl != null)
                c = interno.ActiveControl;
            return c;
        }

        private void panelAyuda_CerrarSolicitado(object sender, EventArgs e) => CerrarAyuda();

        private void panelAyuda_NavegacionSolicitada(object sender, DestinoAyudaEventArgs_790MY e)
            => IrAPantalla(e.Destino);

        /// <summary>
        /// G5: acceso directo del Menú Degustación. Reutiliza el manejador del menú principal
        /// (PerformClick), así hereda ValidatePermission y OpenChildForm sin duplicarlos.
        /// Devuelve false si el ítem no está disponible para el rol actual.
        /// </summary>
        public bool IrAPantalla(DestinoAyuda_790MY destino)
        {
            if (_navegacionBloqueada) return false;
            ToolStripMenuItem item;
            switch (destino)
            {
                case DestinoAyuda_790MY.Clientes:         item = clientesToolStripMenuItem; break;
                case DestinoAyuda_790MY.RegistrarReserva: item = registrarReservaToolStripMenuItem; break;
                case DestinoAyuda_790MY.AbrirMesa:        item = abrirMesaToolStripMenuItem_790MY; break;
                case DestinoAyuda_790MY.RegistrarPedido:  item = registrarPedidoToolStripMenuItem_790MY; break;
                case DestinoAyuda_790MY.Cocina:           item = despacharPedidoToolStripMenuItem_790MY; break;
                case DestinoAyuda_790MY.CajaCobro:        item = cajaCobroToolStripMenuItem_790MY; break;
                default: return false;
            }
            // Available refleja la visibilidad que dejó PermissionFilter (el menú no está desplegado).
            if (!item.Available || !item.Enabled) return false;
            item.PerformClick();
            return true;
        }
        #endregion

        #region BarraSuperior
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void button1_Click_1(object sender, EventArgs e) { Application.Exit(); }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
        }

        private void button3_Click(object sender, EventArgs e) { this.WindowState = FormWindowState.Minimized; }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        #endregion

        private void IdiomaCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idiomaSeleccionado = IdiomaCombobox.SelectedIndex == 1 ? "en" : "es";
            _userBLL.CambiarIdiomaUsuario(idiomaSeleccionado);
            TraductorManager_08YS.Instance.CambiarIdioma(idiomaSeleccionado);
        }
    }
}
