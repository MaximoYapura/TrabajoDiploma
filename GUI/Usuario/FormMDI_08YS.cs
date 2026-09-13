using BLL_08YS;
using FontAwesome.Sharp;
using GUI;
using GUI_08YS.Admin;
using GUI_08YS.Properties;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_08YS
{
    public partial class FormMDI_08YS : Form,IIdiomaObserver_08YS
    {
        // Mapeo de controles del menu administrativo a los permisos necesarios para verlos y ejecutarlos
        private static readonly Dictionary<string, Permisos> _mapaMenuAdmin =
            new Dictionary<string, Permisos>
            {
                { nameof(gestionUsuarioToolStripMenuItem),   Permisos.VerUsuarios    },
                { nameof(bitacoraToolStripMenuItem),         Permisos.VerBitacora    },
                { nameof(familiasToolStripMenuItem),         Permisos.VerFamilias    },
                { nameof(rolesToolStripMenuItem),            Permisos.VerRoles       },
                { nameof(gestionRespaldosToolStripMenuItem), Permisos.VerRespaldos   },
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
            TraductorManager_08YS.Instance.CambiarIdioma(SessionManager_08YS.Instance.Current.Idioma);
            TraductorManager_08YS.Instance.Suscribir(this);
            SessionManager_08YS.Instance.SesionInvalidada += OnSesionInvalidada;
            UpdateIdioma();
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
            // Bloqueamos el evento para evitar recursividad infinita al limpiar/agregar ítems
            IdiomaCombobox.SelectedIndexChanged -= IdiomaCombobox_SelectedIndexChanged;

            int indexTemporal = IdiomaCombobox.SelectedIndex;

            IdiomaCombobox.Items.Clear();
            // Obtenemos los nombres de los idiomas traducidos dinámicamente
            IdiomaCombobox.Items.Add(TraductorManager_08YS.Instance.GetTexto("idioma_es")); // "Español"
            IdiomaCombobox.Items.Add(TraductorManager_08YS.Instance.GetTexto("idioma_en")); // "Inglés"

            // Si es la primera carga, inicializamos según la sesión del usuario
            if (indexTemporal < 0)
            {
                IdiomaCombobox.SelectedIndex = (SessionManager_08YS.Instance.Current.Idioma == "en") ? 1 : 0;
            }
            else
            {
                IdiomaCombobox.SelectedIndex = indexTemporal;
            }

            IdiomaCombobox.SelectedIndexChanged += IdiomaCombobox_SelectedIndexChanged;
        }
        private void AplicarPermisos()
        {
            // Botón "Administrativo" del panel lateral — solo visible si tiene algún permiso admin
            btnAdministrativo.Visible = SessionManager_08YS.Instance.HasPermission(Permisos.VerUsuarios)
                                     || SessionManager_08YS.Instance.HasPermission(Permisos.VerBitacora)
                                     || SessionManager_08YS.Instance.HasPermission(Permisos.VerFamilias)
                                     || SessionManager_08YS.Instance.HasPermission(Permisos.VerRoles   );

            gestionAccesosToolStripMenuItem.Visible = SessionManager_08YS.Instance.HasPermission(Permisos.VerRoles   )
                                                   || SessionManager_08YS.Instance.HasPermission(Permisos.VerFamilias);

            // Items del menu desplegable de admin
            PermissionFilter_08YS.AplicarMenuStrip(AdministrativoDropDownMenu, _mapaMenuAdmin);
        }

        #region idioma
        public void UpdateIdioma()
        {
            // 1. Traduce los controles nativos y paneles del formulario (los botones del panel lateral, labels, etc.)
            TraducirControles(this);

            // 2. TRADUCCIÓN EXPLÍCITA DE MENÚS FLOTANTES COMPOSITE
            // Como no están en Controls, los enviamos individualmente a procesar
            if (AdministrativoDropDownMenu != null)
            {
                TraducirMenuFlotanteCustom(AdministrativoDropDownMenu);
            }

            if (PerfilDropDownMenu != null)
            {
                TraducirMenuFlotanteCustom(PerfilDropDownMenu);
            }
            RefrescarContenidoComboIdioma();
        }

        // NUEVO MÉTODO: Dedicado a los ContextMenuStrip / DropdownMenuStrip personalizados
        private void TraducirMenuFlotanteCustom(ContextMenuStrip menuFlotante)
        {
            foreach (ToolStripItem item in menuFlotante.Items)
            {
                TraducirItemsDesplegables(item);
            }
        }

        private void TraducirControles(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                // 1. Si el control tiene hijos (un Panel, GroupBox, FlowLayoutPanel, etc.) y NO es una barra de herramientas,
                // nos metemos inmediatamente a traducirlos de forma recursiva primero.
                if (c.HasChildren && !(c is ToolStrip))
                {
                    TraducirControles(c);
                }

                // 2. SI ES UNA BARRA DE MENÚ (MenuStrip / ToolStrip tradicional)
                if (c is ToolStrip barraMenu)
                {
                    TraducirBarraHerramientas(barraMenu);
                    continue;
                }

                // 3. SI ES UN ICONBUTTON (Como btnPerfil, btnAdministrativo, btnReservar, btnCerrarSesion)
                if (c is IconButton botonIcono)
                {
                    if (botonIcono.Tag != null && !string.IsNullOrWhiteSpace(botonIcono.Tag.ToString()))
                    {
                        botonIcono.Text = TraductorManager_08YS.Instance.GetTexto(botonIcono.Tag.ToString());
                    }
                    continue;
                }

                // 4. Control común y corriente (Labels, Checkbox, etc.)
                if (c.Tag != null && !string.IsNullOrWhiteSpace(c.Tag.ToString()))
                {
                    c.Text = TraductorManager_08YS.Instance.GetTexto(c.Tag.ToString());
                }
            }
        }

        // Recorre los ítems que están adentro de la barra de herramientas de FontAwesome
        private void TraducirBarraHerramientas(ToolStrip barra)
        {
            foreach (ToolStripItem item in barra.Items)
            {
                TraducirItemsDesplegables(item);
            }
        }

        // Se mete de forma recursiva en los submenús del Dropdown (sirve para ToolStrip y ContextMenuStrip)
        private void TraducirItemsDesplegables(ToolStripItem item)
        {
            if (item.Tag != null && !string.IsNullOrWhiteSpace(item.Tag.ToString()))
            {
                item.Text = TraductorManager_08YS.Instance.GetTexto(item.Tag.ToString());
            }

            if (item is ToolStripDropDownItem itemDesplegable && itemDesplegable.HasDropDownItems)
            {
                foreach (ToolStripItem subItem in itemDesplegable.DropDownItems)
                {
                    TraducirItemsDesplegables(subItem); // Recursividad para sub-ítems anidados
                }
            }
        }
        #endregion

        private void FormMDI_Load(object sender, EventArgs e)
        {
            AdministrativoDropDownMenu.IsMainMenu = true;
            PerfilDropDownMenu.IsMainMenu = true;
        }

        private void FormMDI_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (MessageBox.Show($"{Resources.ContinuarCierreSesion} \n\n{Resources.ConfirmarCierreSesion}", Resources.Advertencia, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
            //{
            //    e.Cancel = true;
            //}
            SessionManager_08YS.Instance.SesionInvalidada -= OnSesionInvalidada;

            if (SessionManager_08YS.Instance.IsLogged)
            {
                _userBLL.Logout();
                CerrarSesion?.Invoke(); // Desoculta el Username y limpia Singleton
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

            ForceCustomControlsLayout(childForm);   // ← MOVER antes del Show

            childForm.ResumeLayout(false);          // ← false: no forzar redibujado todavía
            panel2.ResumeLayout(false);

            if(childForm is IIdiomaObserver_08YS observer)
            {
                TraductorManager_08YS.Instance.Suscribir(observer); // Suscribir al nuevo formulario al cambio de idioma
                observer.UpdateIdioma(); // Forzar actualización inmediata del idioma al abrir la pantalla
            }

            childForm.Load += (s, e) => TraductorManager_08YS.Instance.Suscribir(childForm as IIdiomaObserver_08YS); // Suscribir al nuevo formulario al cambio de idioma
            childForm.FormClosed += (s, e) => TraductorManager_08YS.Instance.Desuscribir(childForm as IIdiomaObserver_08YS); // Desuscribir al cerrar

            childForm.Show();                       // ← mostrar DESPUÉS de que todo esté listo
            childForm.Refresh();                    // ← forzar un pintado limpio único
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
                    lbl.MaximumSize = Size.Empty;  // elimina cualquier límite
                    lbl.Size = Size.Empty;         // fuerza recálculo desde cero
                    lbl.Refresh();
                }
                if (ctrl is CustomControls.IconPlaceholderTextBox txt)
                {
                    // Forzar estado visual correcto
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
                    // Ejecutamos el cierre de sesión e invocamos el evento para desocultar el Username
                    CerrarSesion?.Invoke();
                    this.Close(); // Cerramos el MDI
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

        #region BarraSuperior

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }


        #endregion

        private void IdiomaCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idiomaSeleccionado = IdiomaCombobox.SelectedIndex == 1 ? "en" : "es";

            // 1. Sincronizamos la memoria de la sesión actual a través de la BLL (asumiendo que tenés la referencia '_userBll')
            // Si no tenés la instancia de la BLL inyectada o mapeada en el MDI, accedés directo:
            //SessionManager_08YS.Instance.Current.Idioma = idiomaSeleccionado;
            _userBLL.CambiarIdiomaUsuario(idiomaSeleccionado);
            // 2. Le avisamos al Manager para que muten todas las pantallas abiertas por el Observer
            TraductorManager_08YS.Instance.CambiarIdioma(idiomaSeleccionado);
        }

    }
}
