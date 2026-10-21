using BLL_08YS;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using System;
using GUI_08YS.Estilo;
using GUI_08YS.RF2;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Service_08YS.Entities.Acceso.ResultadoEvaluacion_08YS;

namespace GUI_08YS.Admin
{
    public enum TipoEntidad { Familia, Rol }
    public enum OperacionAM { Alta, Modificacion }

    public partial class FormGestionAcceso_08YS : Form, IIdiomaObserver_08YS, IAyudaContextual_790MY
    {
        private static readonly Dictionary<string, Permisos> _mapaFamilia =
            new Dictionary<string, Permisos>
            {
                { nameof(btnCrear),     Permisos.CrearFamilias     },
                { nameof(btnModificar), Permisos.ModificarFamilias },
                { nameof(btnEliminar),  Permisos.EliminarFamilias  },
            };

        private static readonly Dictionary<string, Permisos> _mapaRol =
            new Dictionary<string, Permisos>
            {
                { nameof(btnCrear),     Permisos.CrearRoles     },
                { nameof(btnModificar), Permisos.ModificarRoles },
                { nameof(btnEliminar),  Permisos.EliminarRoles  },
            };


        private readonly TipoEntidad _modo;
        private readonly FamiliaBLL_08YS _familiaBLL;
        private readonly RolBLL_08YS _rolBLL;
        private readonly Action<Form> _openChildForm;

        private IWin32Window VentanaPropietaria => (this.TopLevelControl as Form) ?? (IWin32Window)this;

        public FormGestionAcceso_08YS(TipoEntidad tipo, Action<Form> openChildForm)
        {
            InitializeComponent();
            _modo = tipo;
            _familiaBLL = BLLFactory_08YS.CreateFamiliaBLL();
            _rolBLL = BLLFactory_08YS.CreateRolBLL();
            _openChildForm = openChildForm;
            //string entidad = _modo == TipoEntidad.Familia ? "Familias" : "Roles";
            //Text = $"Gestión de {entidad}";
            //lblTitulo.Text = $"Gestión de {entidad}";

            dgvEntidades.AutoGenerateColumns = false;
        }

        private void FormGestionAcceso_Load(object sender, EventArgs e)
        {
            ConfigurarPorTipo();
            CargarGrid();
            UpdateIdioma();
        }
        #region Idioma
        public void UpdateIdioma()
        {
            // 1. Traduce primero todos los controles fijos usando los Tags
            TraducirControles(this);

            // 2. AHORA asignamos los títulos dinámicos consultando las claves del TraductorManager
            string claveEntidad = _modo == TipoEntidad.Familia ? "Entidad_Familias" : "Entidad_Roles";
            string gestionTraducida = TraductorManager_08YS.Instance.GetTexto("Texto_GestionDe"); // Debería retornar "Gestión de"
            string entidadTraducida = TraductorManager_08YS.Instance.GetTexto(claveEntidad);     // Retorna "Familias" o "Roles"

            // Armamos la cadena final en base al idioma actual
            string tituloFinal = $"{gestionTraducida} {entidadTraducida}";

            // Asignamos de forma segura para que no se pise
            this.Text = tituloFinal;
            lblTitulo.Text = tituloFinal;

            // 3. Traducimos las columnas de la grilla
            TraducirColumnas();
        }
        private void TraducirColumnas()
        {
            if (dgvEntidades.Columns.Count == 0) return;

            foreach (DataGridViewColumn columna in dgvEntidades.Columns)
            {
                // 1. Traducir la columna del Nombre
                if (columna.Name.Equals("Nombre", StringComparison.OrdinalIgnoreCase) ||
                    columna.DataPropertyName.Equals("Nombre", StringComparison.OrdinalIgnoreCase))
                {
                    columna.HeaderText = TraductorManager_08YS.Instance.GetTexto("ColumnaNombre");
                }

                // 2. Traducir la columna del Tipo (mapeada a 'TipoDisplay' en tu objeto anónimo)
                if (columna.Name.Equals("Tipo", StringComparison.OrdinalIgnoreCase) ||
                    columna.Name.Equals("TipoDisplay", StringComparison.OrdinalIgnoreCase) ||
                    columna.DataPropertyName.Equals("TipoDisplay", StringComparison.OrdinalIgnoreCase))
                {
                    columna.HeaderText = TraductorManager_08YS.Instance.GetTexto("ColumnaTipo");
                }
            }
        }
        private void TraducirControles(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                // Opcional: Podés saltear explícitamente el lblTitulo para que TraducirControles ni lo toque
                if (c == lblTitulo) continue;

                if (c.Tag != null && !string.IsNullOrWhiteSpace(c.Tag.ToString()))
                {
                    c.Text = TraductorManager_08YS.Instance.GetTexto(c.Tag.ToString());
                }

                if (c.HasChildren)
                {
                    TraducirControles(c);
                }
            }
        }
        #endregion
        private void AplicarPermisos()
        {
            var mapa = _modo == TipoEntidad.Familia ? _mapaFamilia : _mapaRol;
            PermissionFilter_08YS.Aplicar(this, mapa);
        }

        private void ConfigurarPorTipo()
        {
            bool esFamilia = _modo == TipoEntidad.Familia;

            //lblTitulo.Text = esFamilia ? "Gestión de Familias" : "Gestión de Roles";
            iconPictureBox.IconChar = esFamilia
                ? FontAwesome.Sharp.IconChar.LayerGroup
                : FontAwesome.Sharp.IconChar.UserShield;

            AplicarPermisos();

            if( esFamilia && 
                   !SessionManager_08YS.Instance.HasPermission(Permisos.CrearFamilias    )  &&
                   !SessionManager_08YS.Instance.HasPermission(Permisos.ModificarFamilias)  &&
                   !SessionManager_08YS.Instance.HasPermission(Permisos.EliminarFamilias )
              )
              pnlBottom.Visible = false;

            else if( !esFamilia && 
                       !SessionManager_08YS.Instance.HasPermission(Permisos.CrearRoles    )  &&
                       !SessionManager_08YS.Instance.HasPermission(Permisos.ModificarRoles)  &&
                       !SessionManager_08YS.Instance.HasPermission(Permisos.EliminarRoles )
                   )
                   pnlBottom.Visible = false;
        }

        private void CargarGrid()
        {
            try
            {
                trvDetalle.Nodes.Clear();

                if (_modo == TipoEntidad.Familia)
                {
                    var familias = _familiaBLL.GetAll();
                    dgvEntidades.DataSource = familias
                        .Select(f => new { f.Nombre, TipoDisplay = "Familia", Entidad = (object)f })
                        .ToList();
                }
                else
                {
                    var roles = _rolBLL.GetAll();
                    dgvEntidades.DataSource = roles
                        .Select(r => new { r.Nombre, TipoDisplay = "Rol", Entidad = (object)r })
                        .ToList();
                }

                ActualizarEstadoBotones();
            }
            catch (Exception ex)
            {
                // Permiso o regla de negocio: banner; falla inesperada: MessageBox (E2-D1).
                EstiloRF2_790MY.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
            }
            TraducirColumnas();
        }

        private void AbrirAM(
            OperacionAM operacion,
            Familia_08YS familiaAEditar = null,
            Rol_08YS rolAEditar = null)
        {
            // Captura referencia a este form antes de que el panel lo reemplace
            var formGestion = this;

            Action onSave = () =>
            {
                formGestion.CargarGrid();
                _openChildForm(formGestion);   // vuelve a mostrar FormGestion actualizado
            };

            Action onCancel = () =>
                _openChildForm(formGestion);   // vuelve a FormGestion sin cambios

            var formABM = new FormAccesoAM_08YS(
                _modo,
                operacion,
                _familiaBLL,
                _rolBLL,
                familiaAEditar,
                rolAEditar,
                onSave,
                onCancel);

            _openChildForm(formABM);           // reemplaza FormGestion por FormAM en el panel del MDI
        }

        #region Buttons

        private void btnCrear_Click(object sender, EventArgs e) => AbrirAM(OperacionAM.Alta);

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvEntidades.CurrentRow == null) return;

            dynamic row = dgvEntidades.CurrentRow.DataBoundItem;

            //infiero el dynamic en funcion del tipo de entidad que se esta gestionando
            if (_modo == TipoEntidad.Familia)
                AbrirAM(OperacionAM.Modificacion, familiaAEditar: (Familia_08YS)row.Entidad);
            else
                AbrirAM(OperacionAM.Modificacion, rolAEditar: (Rol_08YS)row.Entidad);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvEntidades.CurrentRow == null) return;

            dynamic row = dgvEntidades.CurrentRow.DataBoundItem;
            string nombre = row.Nombre;
            object entidad = row.Entidad;

            string pregunta = string.Format(TraductorManager_08YS.Instance.GetTexto("msg_eliminar_preg"), nombre);
            bool confirmado = ConfirmModal_790MY.Confirmar(VentanaPropietaria,
                TraductorManager_08YS.Instance.GetTexto("confirmar_eliminacion"),
                pregunta,
                TraductorManager_08YS.Instance.GetTexto("btnEliminar"));

            if (!confirmado) return;

            bnrAvisos_790MY.Ocultar();
            try
            {
                if (_modo == TipoEntidad.Familia)
                    _familiaBLL.Eliminar(((Familia_08YS)entidad).FamiliaID);
                else
                    _rolBLL.Eliminar(((Rol_08YS)entidad).RolID);

                CargarGrid();
            }
            catch (ComponenteEnUsoException_08YS)
            {
                bnrAvisos_790MY.Mostrar(
                    string.Format(TraductorManager_08YS.Instance.GetTexto("msg_acceso_en_uso"), nombre),
                    TipoBanner_790MY.Aviso);
            }
            catch (Exception ex)
            {
                EstiloRF2_790MY.MostrarErrorEnBanner(bnrAvisos_790MY, ex);
            }
        }

        private void ActualizarEstadoBotones()
        {
            bool hay = dgvEntidades.CurrentRow != null;
            btnModificar.Enabled = hay;
            btnEliminar.Enabled = hay;
        }

        #endregion

        private void dgvEntidades_SelectionChanged(object sender, EventArgs e)
        {
            ActualizarEstadoBotones();
            ActualizarTreeView();
        }

        #region Treeview

        private void ActualizarTreeView()
        {
            trvDetalle.Nodes.Clear();
            if (dgvEntidades.CurrentRow == null) return;

            // La columna Entidad guarda el objeto original
            dynamic row = dgvEntidades.CurrentRow.DataBoundItem;
            object entidad = row.Entidad;

            TreeNode raiz;
            if (_modo == TipoEntidad.Familia)
                raiz = CrearNodo((Familia_08YS)entidad);
            else
            {
                var rol = (Rol_08YS)entidad;
                raiz = new TreeNode(rol.Nombre);
                PopularNodos(rol.Componentes, raiz.Nodes);
            }

            raiz.Expand();
            trvDetalle.Nodes.Add(raiz);
        }

        private TreeNode CrearNodo(AccessComponent_08YS componente)
        {
            var nodo = new TreeNode(
                componente is Familia_08YS ? $"📁 {componente.Nombre}" : $"🔑 {componente.Nombre}");

            if (componente is Familia_08YS f)
                PopularNodos(f.Hijos, nodo.Nodes);

            return nodo;
        }

        private void PopularNodos(IEnumerable<AccessComponent_08YS> componentes, TreeNodeCollection destino)
        {
            foreach (var c in componentes)
                destino.Add(CrearNodo(c));
        }

        private void trvDetalle_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            bool seleccionado = (e.State & TreeNodeStates.Selected) != 0;

            Color colorFondo = seleccionado
                ? Color.FromArgb(211, 84, 0)
                : Color.FromArgb(250, 248, 245);

            Color colorTexto = seleccionado ? Color.FromArgb(253, 246, 227) : Color.FromArgb(45, 42, 40);

            using (var brush = new SolidBrush(colorFondo))
                e.Graphics.FillRectangle(brush, e.Bounds);

            TextRenderer.DrawText(
                e.Graphics,
                e.Node.Text,
                trvDetalle.Font,
                e.Bounds,
                colorTexto,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
        }

        #endregion

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        #region Ayuda contextual (Etapa 5)
        /// <summary>Sugerencias del Chef (F1): solo datos; el globo y el resalte los dibuja AyudaSugerencias_790MY.</summary>
        public IReadOnlyList<PistaAyuda_790MY> ObtenerPistas() => new[]
        {
            new PistaAyuda_790MY(nameof(dgvEntidades), "AY_sug_ga1_t", "AY_sug_ga1_x"),
            new PistaAyuda_790MY(nameof(btnCrear), "AY_sug_ga2_t", "AY_sug_ga2_x"),
            new PistaAyuda_790MY(nameof(btnModificar), "AY_sug_ga3_t", "AY_sug_ga3_x"),
        };
        #endregion
    }
}
