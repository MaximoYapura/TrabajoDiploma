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
    public partial class FormMesas_790MY : Form, IIdiomaObserver_08YS
    {
        private enum ModoEdicion { Reposo, Alta, Modificacion }

        private static readonly Dictionary<string, Permisos> _mapaPermisos =
            new Dictionary<string, Permisos>
            {
                { nameof(btnAnadir_790MY), Permisos.CrearMesa },
                { nameof(btnModificar_790MY), Permisos.CrearMesa },
                { nameof(btnEliminar_790MY), Permisos.CrearMesa }
            };

        private readonly MesaBLL_790MY _mesaBll;
        private ModoEdicion _modo = ModoEdicion.Reposo;

        // Estado original de la mesa que se esta editando. El maestro solo ofrece
        // «Disponible» / «Fuera de servicio»; Ocupada y Reservada son estados operativos
        // que manejan Reservas y Atención en mesa, y se preservan al modificar para no
        // pisarlos (ver EstadoAGuardar).
        private EstadoMesa_790MY _estadoEdicion = EstadoMesa_790MY.Libre;
        private bool _activoEdicion = true;

        // Mesa elegida en el tablero (reemplaza a la fila actual de la grilla anterior).
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
            EstablecerModo(ModoEdicion.Reposo);
        }

        #region Tablero
        private void CargarTablero()
        {
            int? nroAnterior = _seleccionada?.NroMesa;

            // Orden automático por número (P11): el modelo de Mesa no guarda posición.
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
            if (_modo != ModoEdicion.Reposo) return;
            _seleccionada = ((MesaCard_790MY)sender).Mesa;
            // Un aviso sobre la mesa anterior (por ejemplo, una baja rechazada) ya no aplica.
            bnrAvisos_790MY.Ocultar();
            MarcarSeleccion();
        }

        private void MarcarSeleccion()
        {
            foreach (MesaCard_790MY tarjeta in _tarjetas)
                tarjeta.Seleccionada = _seleccionada != null && tarjeta.Mesa.NroMesa == _seleccionada.NroMesa;

            if (_modo == ModoEdicion.Reposo)
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

        /// <summary>El maestro trabaja con dos estados: «Fuera de servicio» o «Disponible» (todo lo demás).</summary>
        private static bool EstaFueraDeServicio(EstadoMesa_790MY estado)
            => estado == EstadoMesa_790MY.FueraDeServicio;

        private void MostrarEstado(EstadoMesa_790MY? estado)
        {
            bool fuera = estado.HasValue && EstaFueraDeServicio(estado.Value);
            rdoFueraServicio_790MY.Checked = fuera;
            rdoDisponible_790MY.Checked = estado.HasValue && !fuera;
        }

        /// <summary>
        /// Estado que se envía a la BLL al modificar. Elegir «Disponible» solo restaura a
        /// Libre una mesa que estaba fuera de servicio; si estaba Ocupada o Reservada se
        /// conserva ese estado operativo, que pertenece a otros módulos.
        /// </summary>
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

        private void EstablecerModo(ModoEdicion modo)
        {
            _modo = modo;
            bool editando = modo != ModoEdicion.Reposo;

            // NroMesa siempre es de solo lectura (ReadOnly = true en el Designer):
            // se auto-asigna en Alta y no se permite cambiar la PK en Modificación.
            txtNumero_790MY.Enabled  = editando;

            nudCapacidad_790MY.Enabled = editando;

            // El estado solo se cambia al modificar: la BLL da de alta toda mesa como Libre.
            rdoDisponible_790MY.Enabled    = modo == ModoEdicion.Modificacion;
            rdoFueraServicio_790MY.Enabled = modo == ModoEdicion.Modificacion;

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
                case ModoEdicion.Alta:
                    lblDetalleTitulo_790MY.Text = t.GetTexto("FM_detalle_nueva");
                    lblDetalleAyuda_790MY.Text = string.Empty;
                    break;
                case ModoEdicion.Modificacion:
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

        /// <summary>Ficha limpia con el próximo número y foco en la capacidad (también tras un alta, E2-D4).</summary>
        private void PrepararAlta()
        {
            LimpiarCampos();

            // Auto-asignar el próximo número disponible (Max + 1)
            try
            {
                int siguiente = _mesaBll.GetProximoNumero();
                txtNumero_790MY.Text = siguiente.ToString();
            }
            catch
            {
                txtNumero_790MY.Text = "1";
            }

            EstablecerModo(ModoEdicion.Alta);
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

            EstablecerModo(ModoEdicion.Modificacion);
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
            EstadoMesa_790MY estado = _modo == ModoEdicion.Modificacion ? EstadoAGuardar() : EstadoMesa_790MY.Libre;

            try
            {
                if (_modo == ModoEdicion.Alta)
                {
                    _mesaBll.RegistrarMesa(numero, capacidad);

                    // Alta (E2-D4): el tablero muestra la mesa nueva y la ficha queda lista para otra.
                    _seleccionada = null;
                    _modo = ModoEdicion.Reposo;
                    CargarTablero();
                    PrepararAlta();
                    ToastHost_790MY.Mostrar(flpTablero_790MY,
                        string.Format(t.GetTexto("msg_fm_registrada_ok"), numero));
                    return;
                }

                // Modificación (E2-D4): queda elegida la mesa con los datos guardados.
                var mesa = new Mesa_790MY(numero, capacidad, estado, _activoEdicion);
                _mesaBll.ModificarMesa(mesa);

                _seleccionada = new Mesa_790MY(numero, capacidad, estado);
                LimpiarCampos();
                _modo = ModoEdicion.Reposo;
                CargarTablero();
                EstablecerModo(ModoEdicion.Reposo);
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

        /// <summary>Error inesperado (base de datos, excepción no prevista): se conserva el MessageBox nativo (E2-D1).</summary>
        private void MostrarErrorFatal(string mensaje)
        {
            MessageBox.Show(this, mensaje, TraductorManager_08YS.Instance.GetTexto("error"),
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnCancelar_790MY_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            EstablecerModo(ModoEdicion.Reposo);
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
    }
}
