using BE_08YS;
using BLL_08YS;
using Service_08YS.Entities.Acceso;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GUI_08YS.Estilo;
using Service_08YS;
using T = GUI_08YS.Estilo.TemaFogon_790MY;

namespace GUI_08YS.Maestros
{
    // Maestro de mesas: las mesas se ven como tarjetas y a la derecha está la ficha para
    // alta, modificación y baja. Las reglas las controla MesaBLL_790MY.
    public partial class FormMesas_790MY : Form, IIdiomaObserver_08YS, IAyudaContextual_790MY
    {
        private static readonly Dictionary<string, Permisos> _mapaPermisos =
            new Dictionary<string, Permisos>
            {
                { nameof(btnAnadir_790MY), Permisos.CrearMesa },
                { nameof(btnModificar_790MY), Permisos.CrearMesa },
                { nameof(btnEliminar_790MY), Permisos.CrearMesa }
            };

        private readonly MesaBLL_790MY _mesaBll;
        private ModoEdicion_790MY _modo = ModoEdicion_790MY.Reposo;

        // Estado que tenía la mesa al empezar a editarla. El maestro solo ofrece Disponible y
        // Fuera de servicio; Ocupada y Reservada las manejan otros módulos y no se pisan.
        private EstadoMesa_790MY _estadoEdicion = EstadoMesa_790MY.Libre;
        private bool _activoEdicion = true;

        // Mesa elegida en el tablero.
        private Mesa_790MY _seleccionada;
        private readonly List<MesaCard_790MY> _tarjetas = new List<MesaCard_790MY>();

        public FormMesas_790MY()
        {
            InitializeComponent();
            _mesaBll = BLLFactory_790MY.CreateMesaBLL();
            TraductorManager_08YS.Instance.Suscribir(this);
            this.FormClosed += (s, e) => TraductorManager_08YS.Instance.Desuscribir(this);
            this.Text = TraductorManager_08YS.Instance.GetTexto("FM_titulo");
        }

        private void FormMesas_790MY_Load(object sender, EventArgs e)
        {
            PermissionFilter_08YS.Aplicar(this, _mapaPermisos);
            CargarTablero();
            EstablecerModo(ModoEdicion_790MY.Reposo);
        }

        #region Tablero
        private void CargarTablero()
        {
            int? nroAnterior = _seleccionada?.NroMesa;

            // Se ordenan por número: la mesa no guarda una posición.
            List<Mesa_790MY> mesas = _mesaBll.GetAll().OrderBy(m => m.NroMesa).ToList();

            flpTablero_790MY.SuspendLayout();
            foreach (MesaCard_790MY tarjeta in _tarjetas)
            {
                flpTablero_790MY.Controls.Remove(tarjeta);
                tarjeta.Dispose();
            }
            _tarjetas.Clear();

            foreach (Mesa_790MY mesa in mesas)
            {
                var tarjeta = new MesaCard_790MY { Mesa = mesa };
                tarjeta.Click += Tarjeta_Click;
                _tarjetas.Add(tarjeta);
                flpTablero_790MY.Controls.Add(tarjeta);
            }

            lblSinMesas_790MY.Visible = mesas.Count == 0;
            flpTablero_790MY.ResumeLayout(true);

            _seleccionada = nroAnterior.HasValue ? mesas.FirstOrDefault(m => m.NroMesa == nroAnterior.Value) : null;
            TraducirTarjetas();
            MarcarSeleccion();
        }

        private void Tarjeta_Click(object sender, EventArgs e)
        {
            if (_modo != ModoEdicion_790MY.Reposo) return;
            _seleccionada = ((MesaCard_790MY)sender).Mesa;
            // Un aviso sobre la mesa anterior (por ejemplo, una baja rechazada) ya no aplica.
            bnrAvisos_790MY.Ocultar();
            MarcarSeleccion();
        }

        private void MarcarSeleccion()
        {
            foreach (MesaCard_790MY tarjeta in _tarjetas)
                tarjeta.Seleccionada = _seleccionada != null && tarjeta.Mesa.NroMesa == _seleccionada.NroMesa;

            if (_modo == ModoEdicion_790MY.Reposo)
            {
                MostrarDetalle(_seleccionada);
                ActualizarHabilitacionBotonesAccion();
            }
        }

        private void TraducirTarjetas()
        {
            var t = TraductorManager_08YS.Instance;
            foreach (MesaCard_790MY tarjeta in _tarjetas)
                tarjeta.EstablecerTextos(t.GetTexto("FM_tarjeta_mesa"),
                                         TextoEstado(tarjeta.Mesa.Estado),
                                         TextoCapacidad(tarjeta.Mesa.Capacidad));
        }

        private static string TextoEstado(EstadoMesa_790MY estado)
            => TraductorManager_08YS.Instance.GetTexto(EstaFueraDeServicio(estado)
                   ? "FM_estado_FueraDeServicio" : "FM_estado_Disponible");

        private static string TextoCapacidad(int capacidad)
        {
            var t = TraductorManager_08YS.Instance;
            return capacidad == 1
                ? t.GetTexto("FM_tarjeta_lugar")
                : string.Format(t.GetTexto("FM_tarjeta_lugares"), capacidad);
        }
        #endregion

        #region Panel de detalle
        private void MostrarDetalle(Mesa_790MY mesa)
        {
            var t = TraductorManager_08YS.Instance;

            if (mesa == null)
            {
                lblDetalleTitulo_790MY.Text = t.GetTexto("FM_detalle_titulo");
                lblDetalleAyuda_790MY.Text = t.GetTexto("FM_detalle_vacio");
                txtNumero_790MY.Clear();
                nudCapacidad_790MY.Value = nudCapacidad_790MY.Minimum;
                MostrarEstado(null);
                return;
            }

            lblDetalleTitulo_790MY.Text = string.Format(t.GetTexto("FM_detalle_mesa"), mesa.NroMesa);
            lblDetalleAyuda_790MY.Text = string.Empty;
            txtNumero_790MY.Text = mesa.NroMesa.ToString();
            nudCapacidad_790MY.Value = Math.Max(nudCapacidad_790MY.Minimum,
                                       Math.Min(nudCapacidad_790MY.Maximum, mesa.Capacidad));
            MostrarEstado(mesa.Estado);
        }

        // El maestro trabaja con dos estados: «Fuera de servicio» o «Disponible» (todo lo demás).
        private static bool EstaFueraDeServicio(EstadoMesa_790MY estado)
            => estado == EstadoMesa_790MY.FueraDeServicio;

        private void MostrarEstado(EstadoMesa_790MY? estado)
        {
            bool fuera = estado.HasValue && EstaFueraDeServicio(estado.Value);
            rdoFueraServicio_790MY.Checked = fuera;
            rdoDisponible_790MY.Checked = estado.HasValue && !fuera;
        }

        // Estado que se manda a la BLL al modificar. "Disponible" solo devuelve a Libre una
        // mesa que estaba fuera de servicio; si estaba Ocupada o Reservada, se deja así.
        private EstadoMesa_790MY EstadoAGuardar()
        {
            if (rdoFueraServicio_790MY.Checked)
                return EstadoMesa_790MY.FueraDeServicio;
            return EstaFueraDeServicio(_estadoEdicion) ? EstadoMesa_790MY.Libre : _estadoEdicion;
        }

        private void pnlDetalle_790MY_Paint(object sender, PaintEventArgs e)
        {
            using (var lapiz = new Pen(T.Linea))
                e.Graphics.DrawLine(lapiz, 0, 0, 0, pnlDetalle_790MY.Height);
        }
        #endregion

        private void ActualizarHabilitacionBotonesAccion()
        {
            bool haySeleccion = _seleccionada != null;
            btnModificar_790MY.Enabled = haySeleccion;
            btnEliminar_790MY.Enabled = haySeleccion;
        }

        private void EstablecerModo(ModoEdicion_790MY modo)
        {
            _modo = modo;
            bool editando = modo != ModoEdicion_790MY.Reposo;

            // El número es siempre de solo lectura: lo pone el sistema en el alta y no se cambia.
            txtNumero_790MY.Enabled  = editando;

            nudCapacidad_790MY.Enabled = editando;

            // El estado solo se cambia al modificar: la BLL da de alta toda mesa como Libre.
            rdoDisponible_790MY.Enabled    = modo == ModoEdicion_790MY.Modificacion;
            rdoFueraServicio_790MY.Enabled = modo == ModoEdicion_790MY.Modificacion;

            btnAplicar_790MY.Enabled  = editando;
            btnCancelar_790MY.Enabled = editando;

            flpTablero_790MY.Enabled = !editando;
            btnAnadir_790MY.Enabled  = !editando;

            if (!editando)
                ActualizarHabilitacionBotonesAccion();
            else
                btnModificar_790MY.Enabled = btnEliminar_790MY.Enabled = false;

            bnrAvisos_790MY.Ocultar();
            errValidacion_790MY.Clear();
            ActualizarTituloDetalle();
        }

        private void ActualizarTituloDetalle()
        {
            var t = TraductorManager_08YS.Instance;
            switch (_modo)
            {
                case ModoEdicion_790MY.Alta:
                    lblDetalleTitulo_790MY.Text = t.GetTexto("FM_detalle_nueva");
                    lblDetalleAyuda_790MY.Text = string.Empty;
                    break;
                case ModoEdicion_790MY.Modificacion:
                    lblDetalleTitulo_790MY.Text = string.Format(t.GetTexto("FM_detalle_editar"), txtNumero_790MY.Text);
                    lblDetalleAyuda_790MY.Text = string.Empty;
                    break;
                default:
                    MostrarDetalle(_seleccionada);
                    break;
            }
        }

        private void LimpiarCampos()
        {
            txtNumero_790MY.Clear();
            nudCapacidad_790MY.Value = 1;
            _estadoEdicion = EstadoMesa_790MY.Libre;
            _activoEdicion = true;
            MostrarEstado(_estadoEdicion);
        }

        private void btnAnadir_790MY_Click(object sender, EventArgs e)
        {
            PrepararAlta();
        }

        // Ficha limpia con el próximo número y el foco en la capacidad (también después de un alta).
        private void PrepararAlta()
        {
            LimpiarCampos();

            // El número es el más alto más uno.
            try
            {
                int siguiente = _mesaBll.GetProximoNumero();
                txtNumero_790MY.Text = siguiente.ToString();
            }
            catch
            {
                txtNumero_790MY.Text = "1";
            }

            EstablecerModo(ModoEdicion_790MY.Alta);
            nudCapacidad_790MY.Focus();
        }

        private void btnModificar_790MY_Click(object sender, EventArgs e)
        {
            if (!(_seleccionada is Mesa_790MY seleccionada))
                return;

            txtNumero_790MY.Text       = seleccionada.NroMesa.ToString();
            nudCapacidad_790MY.Value   = seleccionada.Capacidad;
            _estadoEdicion             = seleccionada.Estado;
            _activoEdicion             = seleccionada.Activo;
            MostrarEstado(_estadoEdicion);

            EstablecerModo(ModoEdicion_790MY.Modificacion);
            nudCapacidad_790MY.Focus();
        }

        private void btnEliminar_790MY_Click(object sender, EventArgs e)
        {
            if (!(_seleccionada is Mesa_790MY seleccionada))
                return;

            var t = TraductorManager_08YS.Instance;
            bool confirmado = ConfirmModal_790MY.Confirmar(this,
                string.Format(t.GetTexto("FM_confirmar_baja_titulo"), seleccionada.NroMesa),
                t.GetTexto("msg_fm_confirmar_baja_mesa"),
                t.GetTexto("FM_confirmar_baja_accion"));

            if (!confirmado) return;

            try
            {
                _mesaBll.EliminarMesa(seleccionada.NroMesa);
                _seleccionada = null;
                CargarTablero();
                ToastHost_790MY.Mostrar(flpTablero_790MY,
                    string.Format(t.GetTexto("msg_fm_baja_ok"), seleccionada.NroMesa));
            }
            catch (ArgumentException ex)
            {
                bnrAvisos_790MY.Mostrar(string.Format(t.GetTexto("FM_err_bll_baja"), ex.Message));
            }
            catch (Exception)
            {
                MostrarErrorFatal(t.GetTexto("msg_fm_error_eliminar"));
            }
        }

        // Guarda el alta o la modificación con MesaBLL_790MY.
        private void btnAplicar_790MY_Click(object sender, EventArgs e)
        {
            var t = TraductorManager_08YS.Instance;

            // El número se asigna solo y no se edita; si no es válido, se ofrece reintentar el alta.
            errValidacion_790MY.SetError(txtNumero_790MY, string.Empty);
            if (!int.TryParse(txtNumero_790MY.Text.Trim(), out int numero) || numero <= 0)
            {
                string mensaje = t.GetTexto("msg_fm_nro_invalido");
                errValidacion_790MY.SetError(txtNumero_790MY, mensaje);
                bnrAvisos_790MY.Mostrar(mensaje);
                return;
            }

            int capacidad = (int)nudCapacidad_790MY.Value;
            EstadoMesa_790MY estado = _modo == ModoEdicion_790MY.Modificacion ? EstadoAGuardar() : EstadoMesa_790MY.Libre;

            try
            {
                if (_modo == ModoEdicion_790MY.Alta)
                {
                    _mesaBll.RegistrarMesa(numero, capacidad);

                    // Después de un alta se ve la mesa nueva y la ficha queda lista para otra.
                    _seleccionada = null;
                    _modo = ModoEdicion_790MY.Reposo;
                    CargarTablero();
                    PrepararAlta();
                    ToastHost_790MY.Mostrar(flpTablero_790MY,
                        string.Format(t.GetTexto("msg_fm_registrada_ok"), numero));
                    return;
                }

                // Después de modificar queda elegida la mesa guardada.
                var mesa = new Mesa_790MY(numero, capacidad, estado, _activoEdicion);
                _mesaBll.ModificarMesa(mesa);

                _seleccionada = new Mesa_790MY(numero, capacidad, estado);
                LimpiarCampos();
                _modo = ModoEdicion_790MY.Reposo;
                CargarTablero();
                EstablecerModo(ModoEdicion_790MY.Reposo);
                ToastHost_790MY.Mostrar(flpTablero_790MY,
                    string.Format(t.GetTexto("msg_fm_modificada_ok"), numero));
            }
            catch (ArgumentException ex)
            {
                // Reglas de la BLL: límite de mesas, número repetido, capacidad, etc.
                bnrAvisos_790MY.Mostrar(string.Format(t.GetTexto("FM_err_bll_guardar"), ex.Message));
            }
            catch (Exception)
            {
                MostrarErrorFatal(t.GetTexto("msg_fm_error_guardar"));
            }
        }

        // Error inesperado (por ejemplo, de la base): se muestra en un MessageBox.
        private void MostrarErrorFatal(string mensaje)
        {
            MessageBox.Show(this, mensaje, TraductorManager_08YS.Instance.GetTexto("error"),
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnCancelar_790MY_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            EstablecerModo(ModoEdicion_790MY.Reposo);
        }

        private void btnSalir_790MY_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region i18n
        public void UpdateIdioma()
        {
            TraducirControles(this);
            TraducirTarjetas();
            ActualizarTituloDetalle();

            this.Text = TraductorManager_08YS.Instance.GetTexto("FM_titulo");
        }

        private void TraducirControles(Control contenedor)
        {
            foreach (Control c in contenedor.Controls)
            {
                // Omitir TextBox y RichTextBox: son entradas editables por el usuario
                if (c is TextBox || c is RichTextBox)
                    continue;

                if (c.Tag is string clave && !string.IsNullOrWhiteSpace(clave))
                    c.Text = TraductorManager_08YS.Instance.GetTexto(clave);

                if (c.HasChildren)
                    TraducirControles(c);
            }
        }
        #endregion

        #region Ayuda contextual (Etapa 5)
        // Sugerencias del Chef (F1): solo datos; el globo y el resalte los dibuja AyudaSugerencias_790MY.
        public IReadOnlyList<PistaAyuda_790MY> ObtenerPistas() => new[]
        {
            new PistaAyuda_790MY(nameof(flpTablero_790MY), "AY_sug_me1_t", "FM_detalle_vacio"),
            new PistaAyuda_790MY(nameof(btnAnadir_790MY), "AY_sug_me2_t", "FM_ayuda_numero"),
            new PistaAyuda_790MY(nameof(nudCapacidad_790MY), "AY_sug_me3_t", "AY_sug_me3_x"),
            new PistaAyuda_790MY(nameof(rdoFueraServicio_790MY), "AY_sug_me4_t", "FM_ayuda_estado"),
        };
        #endregion
    }
}
