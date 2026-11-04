using BE_08YS;
using BLL_08YS.Exceptions;
using DAL_08YS;
using DAL_08YS.Interfaces_Repositories;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using Service_08YS.Entities.Bitacora;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace BLL_08YS
{
    // Reglas del maestro de clientes, más la exportación e importación en XML.
    public class ClienteBLL_790MY
    {
        private readonly IClienteRepository_790MY _clienteRepository;
        private readonly IReservaRepository_790MY _reservaRepository;
        private readonly BitacoraBLL_08YS _bitacoraBll;

        public ClienteBLL_790MY(IClienteRepository_790MY clienteRepository, IReservaRepository_790MY reservaRepository,
                                BitacoraBLL_08YS bitacoraBll)
        {
            _clienteRepository = clienteRepository;
            _reservaRepository = reservaRepository;
            _bitacoraBll = bitacoraBll;
        }

        // Alta de cliente. El DNI no se puede repetir, ni siquiera con un cliente dado de baja.
        public void RegistrarCliente(int dni, string nombre, string apellido, string email, string telefono,
            string direccion = null, List<string> emailsAdicionales = null, List<string> telefonosAdicionales = null)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearCliente);

            ValidarDatos(dni, nombre, apellido, email, telefono);

            if (_clienteRepository.Exists(dni))
                throw new ClienteDniDuplicadoException_790MY(dni);

            var cliente = new Cliente_790MY(dni, nombre, apellido, email, telefono, direccion)
            {
                EmailsAdicionales = emailsAdicionales ?? new List<string>(),
                TelefonosAdicionales = telefonosAdicionales ?? new List<string>()
            };

            _clienteRepository.Create(cliente);

          
            DVManager_08YS.Recalcular();

            _bitacoraBll.RegistrarEventoPostCommit(Evento.ClienteRegistrado, dni.ToString());
        }

        // Modificación de un cliente activo.
        public void ActualizarCliente(Cliente_790MY cliente)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearCliente);

            ValidarDatos(cliente.DNI, cliente.Nombre, cliente.Apellido, cliente.Email, cliente.Telefono);

            // GetByDni solo trae activos: un cliente dado de baja no se modifica.
            if (_clienteRepository.GetByDni(cliente.DNI) == null)
                throw MensajesBLL_790MY.Regla("msg_fc_cliente_inexistente_modificar");

            _clienteRepository.Update(cliente);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.ClienteModificado, cliente.DNI.ToString());
        }

        // Baja lógica. No se puede si el cliente tiene reservas confirmadas de hoy en adelante.
        public void EliminarCliente(int dni)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearCliente);

            if (_clienteRepository.GetByDni(dni) == null)
                throw MensajesBLL_790MY.Regla("msg_fc_cliente_inexistente_baja");

            // Lee IReservaRepository_790MY directo: pasar por ReservaBLL_790MY armaría un
            // ciclo, porque ReservaBLL ya depende de ClienteBLL.
            int reservasPendientes = _reservaRepository
                .Buscar(DateTime.Today, null, dni, EstadoReserva_790MY.Confirmada).Count;
            if (reservasPendientes > 0)
                throw MensajesBLL_790MY.Regla("msg_fc_cliente_con_reservas", reservasPendientes);

            _clienteRepository.DeleteLogico(dni);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEventoPostCommit(Evento.ClienteEliminado, dni.ToString());
        }

        // Clientes activos.
        public List<Cliente_790MY> GetAll()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerClientes);
            return _clienteRepository.GetAll();
        }

        // True si el DNI existe, aunque el cliente esté dado de baja.
        public bool Exists(int dni)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerClientes);
            return _clienteRepository.Exists(dni);
        }

        // Cliente activo con ese DNI, o null.
        public Cliente_790MY GetByDni(int dni)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerClientes);
            return _clienteRepository.GetByDni(dni);
        }

        // True si hay un cliente activo con ese DNI. No valida permiso porque lo usa
        // ReservaBLL_790MY, que ya validó el suyo (Recepción no tiene VerClientes).
        internal bool ExisteActivo(int dni)
            => _clienteRepository.GetByDni(dni) != null;

        // Cliente activo con ese DNI, o null. Para el walk-in; no valida permiso.
        internal Cliente_790MY ObtenerActivo(int dni)
            => _clienteRepository.GetByDni(dni);

        // True si el DNI existe pero el cliente está dado de baja. Para el walk-in.
        internal bool ExisteInactivo(int dni)
            => _clienteRepository.Exists(dni) && _clienteRepository.GetByDni(dni) == null;

        // ── Exportar e importar XML ──────────────────────────────────────────
        // Se usa XmlSerializer de .NET y siempre se trabaja con una lista de clientes,
        // sea uno o varios. Al importar se acepta una lista o un cliente suelto.
        // Piden VerClientes, igual que el listado.

        private static readonly XmlSerializer _serializerLista   = new XmlSerializer(typeof(List<Cliente_790MY>));
        private static readonly XmlSerializer _serializerCliente = new XmlSerializer(typeof(Cliente_790MY));

        // Pasa la lista de clientes a un texto XML (UTF-8). La lista no puede estar vacía.
        public string SerializarClientesXML(List<Cliente_790MY> clientes)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerClientes);
            if (clientes == null) throw new ArgumentNullException(nameof(clientes));
            if (clientes.Count == 0) throw MensajesBLL_790MY.Regla("msg_fc_xml_lista_vacia");

            using (var writer = new Utf8StringWriter())
            {
                _serializerLista.Serialize(writer, clientes);
                return writer.ToString();
            }
        }

        // Lee un texto XML y devuelve los clientes. Si el XML no es válido lanza ArgumentException.
        public List<Cliente_790MY> DeserializarClientesXML(string contenidoXml)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerClientes);
            if (string.IsNullOrWhiteSpace(contenidoXml))
                throw MensajesBLL_790MY.Regla("msg_fc_xml_vacio");

            string texto = contenidoXml.Trim();
            try
            {
                using (var reader = XmlReader.Create(new StringReader(texto)))
                {
                    if (_serializerLista.CanDeserialize(reader))
                        return (List<Cliente_790MY>)_serializerLista.Deserialize(reader) ?? new List<Cliente_790MY>();
                }

                using (var reader = XmlReader.Create(new StringReader(texto)))
                {
                    if (_serializerCliente.CanDeserialize(reader))
                        return new List<Cliente_790MY> { (Cliente_790MY)_serializerCliente.Deserialize(reader) };
                }
            }
            catch (Exception ex) when (ex is XmlException || ex is InvalidOperationException)
            {
                // XML mal formado o con tipos de datos incorrectos.
                throw new ArgumentException(MensajesBLL_790MY.Texto("msg_fc_xml_invalido", ex.Message), ex);
            }

            throw MensajesBLL_790MY.Regla("msg_fc_xml_sin_clientes");
        }

        // Exporta la lista de clientes a un archivo XML.
        public void SerializarArchivoXML(List<Cliente_790MY> clientes, string rutaArchivo)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerClientes);
            if (string.IsNullOrWhiteSpace(rutaArchivo)) throw MensajesBLL_790MY.Regla("msg_fc_ruta_vacia");
            string xml = SerializarClientesXML(clientes);
            File.WriteAllText(rutaArchivo, xml, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        }

        // Lee un archivo XML y devuelve los clientes que tiene. No los graba en la base.
        public List<Cliente_790MY> DeserializarArchivoXML(string rutaArchivo)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerClientes);
            if (string.IsNullOrWhiteSpace(rutaArchivo)) throw MensajesBLL_790MY.Regla("msg_fc_ruta_vacia");
            if (!File.Exists(rutaArchivo)) throw MensajesBLL_790MY.Regla("msg_fc_archivo_inexistente", rutaArchivo);
            return DeserializarClientesXML(File.ReadAllText(rutaArchivo, Encoding.UTF8));
        }

        // StringWriter pone "utf-16" en el encabezado del XML; este pone "utf-8", que es como se guarda.
        private sealed class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => new UTF8Encoding(false);
        }

        // Validaciones de los datos del cliente.
        private static void ValidarDatos(int dni, string nombre, string apellido, string email, string telefono)
        {
            // DNI de 7 u 8 dígitos, igual que en la pantalla. Un 0 o un negativo no pasan.
            if (!Regex.IsMatch(dni.ToString(), @"^\d{7,8}$"))
                throw MensajesBLL_790MY.Regla("FC_val_dni_formato");

            if (string.IsNullOrWhiteSpace(nombre))
                throw MensajesBLL_790MY.Regla("FC_val_nombre");

            if (string.IsNullOrWhiteSpace(apellido))
                throw MensajesBLL_790MY.Regla("FC_val_apellido");

            if (string.IsNullOrWhiteSpace(telefono))
                throw MensajesBLL_790MY.Regla("msg_fc_celular_requerido");

            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw MensajesBLL_790MY.Regla("FC_val_email_formato");
        }
    }
}
