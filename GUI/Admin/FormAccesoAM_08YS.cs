using BLL_08YS;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using Service_08YS.Entities.Comparers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_08YS.Admin
{
    public partial class FormAccesoAM_08YS : Form,IIdiomaObserver_08YS
    {
        private readonly TipoEntidad _tipo;
        private readonly OperacionAM _operacion;
        private readonly FamiliaBLL_08YS _familiaBLL;
        private readonly RolBLL_08YS _rolBLL;
        private readonly Familia_08YS _familiaAEditar;
        private readonly Rol_08YS _rolAEditar;
        private readonly Action _onGuardado;
        private readonly Action _onCancelado;

        private HashSet<AccessComponent_08YS> _todosDisponibles =
            new HashSet<AccessComponent_08YS>(AccessComponentComparer_08YS.Instance);

        private HashSet<AccessComponent_08YS> _seleccionados =
            new HashSet<AccessComponent_08YS>(AccessComponentComparer_08YS.Instance);

        private HashSet<AccessComponent_08YS> _disponibles =
            new HashSet<AccessComponent_08YS>(AccessComponentComparer_08YS.Instance);

        // Evita cascada de eventos entre los dos dgv durante refreshes
        private bool _actualizandoSeleccion = false;

        private AccesoBLL_08YS BLL => _tipo == TipoEntidad.Familia
               ? (AccesoBLL_08YS)_familiaBLL
               :            _rolBLL;

        public FormAccesoAM_08YS(
            TipoEntidad tipo,
            OperacionAM operacion,
            FamiliaBLL_08YS familiaBLL,
            RolBLL_08YS rolBLL,
            Familia_08YS familiaAEditar,
            Rol_08YS rolAEditar,
            Action onGuardado,
            Action onCancelado)
        {
            InitializeComponent();
            _tipo = tipo;
            _operacion = operacion;
            _familiaBLL = familiaBLL;
            _rolBLL = rolBLL;
            _familiaAEditar = familiaAEditar;
            _rolAEditar = rolAEditar;
            _onGuardado = onGuardado;
            _onCancelado = onCancelado;

            dgvDisponibles.AutoGenerateColumns = false;
            dgvSeleccionados.AutoGenerateColumns = false;
        }

        private void FormAccesoAM_08YS_Load(object sender, EventArgs e)
        {
            ConfigurarFrontPorTipo();
            //ConfigurarBotones();
            CargarDatos();
            UpdateIdioma();
        }

        #region idiomas
        public void UpdateIdioma()
        {
            TraducirControles(this);
            string claveOperacion = _operacion == OperacionAM.Alta ? "Operacion_Alta" : "Operacion_Modificacion";
            string claveEntidad = _tipo == TipoEntidad.Familia ? "Entidad_Familia" : "Entidad_Rol";
            string operacionTraducida = TraductorManager_08YS.Instance.GetTexto(claveOperacion);
            string entidadTraducida = TraductorManager_08YS.Instance.GetTexto(claveEntidad);

            // Asignamos al Label del título respetando el idioma actual
            lblTitulo.Text = $"{operacionTraducida} {entidadTraducida}";

            txtNombre.PlaceholderText = TraductorManager_08YS.Instance.GetTexto("txtNombreAcceso_hint");

            TraducirColumnas();
        }

        private void TraducirColumnas()
        {
            // Traducimos ambas grillas con un único método helper interno
            TraducirGrillaComponentes(dgvSeleccionados);
            TraducirGrillaComponentes(dgvDisponibles);
        }

        private void TraducirGrillaComponentes(DataGridView dgv)
        {
            if (dgv == null || dgv.Columns.Count == 0) return;

            foreach (DataGridViewColumn columna in dgv.Columns)
            {
                // Evalúa si el nombre de la columna o el mapeo de la propiedad apuntan al Nombre
                if (columna.Name.Equals("Nombre", StringComparison.OrdinalIgnoreCase) ||
                    columna.DataPropertyName.Equals("Nombre", StringComparison.OrdinalIgnoreCase))
                {
                    columna.HeaderText = TraductorManager_08YS.Instance.GetTexto("ColumnaNombre");
                }

                // Evalúa discrepancias entre "Tipo", "TipoDisplay" o el DataPropertyName del wrapper ComponentRow
                if (columna.Name.Equals("Tipo", StringComparison.OrdinalIgnoreCase) ||
                    columna.Name.Equals("TipoDisplay", StringComparison.OrdinalIgnoreCase) ||
                    columna.DataPropertyName.Equals("TipoDisplay", StringComparison.OrdinalIgnoreCase))
                {
                    columna.HeaderText = TraductorManager_08YS.Instance.GetTexto("ColumnaTipo");
                }
            }
        }

        private static readonly Type[] _tiposTraducibles =
        {
            typeof(Label),
            typeof(Button),
            typeof(FontAwesome.Sharp.IconButton),
            typeof(CheckBox),
            typeof(RadioButton)
        };

        private void TraducirControles(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                bool esTraducible = _tiposTraducibles.Any(t => t.IsInstanceOfType(c));
                // Si el control tiene un Tag asignado, buscamos su traducción
                if (esTraducible && c.Tag != null && !string.IsNullOrWhiteSpace(c.Tag.ToString()))
                    c.Text = TraductorManager_08YS.Instance.GetTexto(c.Tag.ToString());

                // Si el control tiene hijos (como un Panel o GroupBox), hacemos recursividad
                if (c.HasChildren)
                    TraducirControles(c);
            }
        }
        #endregion

        private void ConfigurarFrontPorTipo()
        {
            bool esFamilia = _tipo == TipoEntidad.Familia;

            iconPictureBox.IconChar = esFamilia
                ? FontAwesome.Sharp.IconChar.LayerGroup
                : FontAwesome.Sharp.IconChar.UserShield;

            if (_operacion == OperacionAM.Modificacion)
                txtNombre.Text = esFamilia ? _familiaAEditar.Nombre : _rolAEditar.Nombre;
        }
        
        #region Datagrids

        private void CargarDatos()
        {
            if (_operacion == OperacionAM.Modificacion)
            {
                var precargados = _tipo == TipoEntidad.Familia
                    ? _familiaAEditar.Hijos
                    : _rolAEditar.Componentes;

                foreach (var c in precargados)
                    _seleccionados.Add(c);
            }

            int? familiaIdExcluir = _operacion == OperacionAM.Modificacion
                                 && _tipo      == TipoEntidad.Familia
                ? _familiaAEditar.FamiliaID
                : (int?) null;

            var todos = _tipo == TipoEntidad.Familia
                ? _familiaBLL.GetComponentesDisponibles(familiaIdExcluir)
                : _rolBLL.GetComponentesDisponibles();

            foreach (var c in todos)
                _todosDisponibles.Add(c);

            RecalcularDisponibles();
            RefrescarDGVSeleccionados();
            RefrescarDGVDisponibles();
            TraducirColumnas();
        }

        private void RefrescarDGV(DataGridView dgv, HashSet<AccessComponent_08YS> fuente)
        {
            ActualizarSeleccion(() =>
            {
                dgv.DataSource = null;
                dgv.DataSource = fuente
                    .OrderBy(c => c is Familia_08YS ? 0 : 1)
                    .ThenBy(c => c.Nombre)
                    .Select(c => new ComponentRow(c))
                    .ToList();
            });
            TraducirColumnas();
        }

        private void RefrescarDGVSeleccionados() => RefrescarDGV(dgvSeleccionados, _seleccionados);
        private void RefrescarDGVDisponibles() => RefrescarDGV(dgvDisponibles, _disponibles);

        public void ActualizarSeleccion(Action action)
        {
            _actualizandoSeleccion = true;
            action();
            _actualizandoSeleccion = false;
        }

        private void dgvSeleccionados_SelectionChanged(object sender, EventArgs e)
        {
            if (_actualizandoSeleccion) return;

            ActualizarSeleccion(() => dgvDisponibles.ClearSelection());

            var row = dgvSeleccionados.CurrentRow?.DataBoundItem as ComponentRow;
            if (row != null) ActualizarDetalle(row.Componente);
            else LimpiarDetalle();
        }

        private void dgvDisponibles_SelectionChanged(object sender, EventArgs e)
        {
            if (_actualizandoSeleccion) return;

            ActualizarSeleccion(() => dgvSeleccionados.ClearSelection());

            var row = dgvDisponibles.CurrentRow?.DataBoundItem as ComponentRow;
            if (row != null) ActualizarDetalle(row.Componente);
            else LimpiarDetalle();
        }

        private void RecalcularDisponibles()
        {
            var permsCubiertos = _seleccionados
                .SelectMany(c => c.GetPermisos())
                .Select(p => p.PermisoID)
                .ToHashSet();

            _disponibles = new HashSet<AccessComponent_08YS>(
                _todosDisponibles
                    .Where(c => !_seleccionados.Contains(c))
                    .Where(c => c.GetPermisos().Any()) // descarta vacíos por seguridad
                    .Where(c => !c.GetPermisos().All(p => permsCubiertos.Contains(p.PermisoID))),
                AccessComponentComparer_08YS.Instance);
        }

        #endregion

        #region Buttons

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (dgvDisponibles.CurrentRow == null)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_seleccionar_componente_add"), TraductorManager_08YS.Instance.GetTexto("sin_seleccion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var candidato = (dgvDisponibles.CurrentRow.DataBoundItem as ComponentRow)?.Componente;
            if (candidato == null)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_error_obtener_componente"), TraductorManager_08YS.Instance.GetTexto("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var resultado = BLL.EvaluarAgregarComponente(_seleccionados, candidato);

            switch (resultado.Resultado)
            {
                case ResultadoEvaluacion_08YS.Tipo.Valido:
                    EjecutarAgregado(candidato);
                    break;

                case ResultadoEvaluacion_08YS.Tipo.SugerenciaReemplazo:
                    var confirm = MessageBox.Show(
                        resultado.Mensaje, // Viene directo con lógica traducida desde la BLL o formateada
                        TraductorManager_08YS.Instance.GetTexto("reemplazo_sugerido"),
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question);

                    if (confirm == DialogResult.OK)
                    {
                        foreach (var reemplazar in resultado.ComponentesAReemplazar)
                            _seleccionados.Remove(reemplazar);

                        EjecutarAgregado(candidato);
                    }
                    break;

                case ResultadoEvaluacion_08YS.Tipo.ConflictoIrresoluble:
                    MessageBox.Show(resultado.Mensaje, TraductorManager_08YS.Instance.GetTexto("no_se_puede_agregar"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        private void EjecutarAgregado(AccessComponent_08YS componente)
        {
            _seleccionados.Add(componente);
            RecalcularDisponibles();

            LimpiarDetalle();
            RefrescarDGVSeleccionados();
            RefrescarDGVDisponibles();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvSeleccionados.CurrentRow == null)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_seleccionar_componente_del"), TraductorManager_08YS.Instance.GetTexto("sin_seleccion"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var componente = (dgvSeleccionados.CurrentRow.DataBoundItem as ComponentRow)?.Componente;
            if (componente == null)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_error_obtener_componente"), TraductorManager_08YS.Instance.GetTexto("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _seleccionados.Remove(componente);
            RecalcularDisponibles();

            LimpiarDetalle();
            RefrescarDGVSeleccionados();
            RefrescarDGVDisponibles();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombre.Text.Trim();

                if (_tipo == TipoEntidad.Familia)
                {
                    if (_operacion == OperacionAM.Alta)
                    {
                        _familiaBLL.Crear(nombre, _seleccionados);
                        MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_familia_creada_exito"), TraductorManager_08YS.Instance.GetTexto("exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        _familiaBLL.Modificar(_familiaAEditar.FamiliaID, nombre, _seleccionados);
                        MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_familia_modificada_exito"), TraductorManager_08YS.Instance.GetTexto("exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    if (_operacion == OperacionAM.Alta)
                    {
                        _rolBLL.Crear(nombre, _seleccionados);
                        MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_rol_creado_exito"), TraductorManager_08YS.Instance.GetTexto("exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        _rolBLL.Modificar(_rolAEditar.RolID, nombre, _seleccionados);
                        MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_rol_modificado_exito"), TraductorManager_08YS.Instance.GetTexto("exito"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                _onGuardado?.Invoke();
            }
            catch (NombreDuplicadoException_08YS)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_nombre_acceso_duplicado"),
                                TraductorManager_08YS.Instance.GetTexto("error_validacion"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (PermisosDuplicadosException_08YS)
            {
                MessageBox.Show(TraductorManager_08YS.Instance.GetTexto("msg_permisos_identicos_error"),
                                TraductorManager_08YS.Instance.GetTexto("error_validacion"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{TraductorManager_08YS.Instance.GetTexto("msg_error_inesperado")}{ex.Message}",
                                TraductorManager_08YS.Instance.GetTexto("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
            => _onCancelado?.Invoke();

        #endregion

        #region Panel Detalle 

        private void ActualizarDetalle(AccessComponent_08YS componente)
        {
            if (componente is Permiso_08YS permiso)
            {
                lblDescripcion.Text = string.IsNullOrWhiteSpace(permiso.Descripcion)
                    ? TraductorManager_08YS.Instance.GetTexto("msg_sin_descripcion")
                    : permiso.Descripcion;
                lblDescripcion.Visible = true;
                trvDetalle.Visible = false;
            }
            else if (componente is Familia_08YS familia)
            {
                trvDetalle.Nodes.Clear();
                var raiz = CrearNodo(familia);
                raiz.Expand();
                trvDetalle.Nodes.Add(raiz);
                trvDetalle.Visible = true;
                lblDescripcion.Visible = false;
            }
        }

        private void LimpiarDetalle()
        {
            lblDescripcion.Visible = false;
            trvDetalle.Visible = false;
            trvDetalle.Nodes.Clear();
        }

        private TreeNode CrearNodo(AccessComponent_08YS componente)
        {
            var nodo = new TreeNode(
                componente is Familia_08YS ? $"📁 {componente.Nombre}" : $"🔑 {componente.Nombre}");

            if (componente is Familia_08YS f)
                foreach (var hijo in f.Hijos)
                    nodo.Nodes.Add(CrearNodo(hijo));

            return nodo;
        }

        private void trvDetalle_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            bool seleccionado = (e.State & TreeNodeStates.Selected) != 0;

            Color colorFondo = seleccionado
                ? Color.FromArgb(5, 5, 100)
                : Color.FromArgb(5, 10, 40);

            Color colorTexto = seleccionado ? Color.Gold : Color.White;

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

        private class ComponentRow //clase auxiliar para mostrar componentes en el dgv debido a autogenerate columns false manteniendo control sobre las columnas y un acceso al objeto original
        {
            public AccessComponent_08YS Componente { get; }
            public string Nombre => Componente.Nombre;
            public string TipoDisplay => Componente is Familia_08YS ? "Familia" : "Permiso";

            public ComponentRow(AccessComponent_08YS c) => Componente = c;
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

    }
}
