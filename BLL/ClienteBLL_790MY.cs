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
    public class ClienteBLL_790MY
    {
        private readonly IClienteRepository_790MY _clienteRepository;
        private readonly BitacoraBLL_08YS _bitacoraBll;

        public ClienteBLL_790MY(IClienteRepository_790MY clienteRepository, BitacoraBLL_08YS bitacoraBll)
        {
            _clienteRepository = clienteRepository;
            _bitacoraBll = bitacoraBll;
        }

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

            _bitacoraBll.RegistrarEvento(Evento.ClienteRegistrado, targetUsername: dni.ToString());
        }

        public void ActualizarCliente(Cliente_790MY cliente)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearCliente);

            ValidarDatos(cliente.DNI, cliente.Nombre, cliente.Apellido, cliente.Email, cliente.Telefono);

            if (!_clienteRepository.Exists(cliente.DNI))
                throw new ArgumentException("El cliente que intenta modificar no existe o fue dado de baja.");

            _clienteRepository.Update(cliente);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEvento(Evento.ClienteModificado, targetUsername: cliente.DNI.ToString());
        }

        public void EliminarCliente(int dni)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearCliente);

            if (!_clienteRepository.Exists(dni))
                throw new ArgumentException("El cliente que intenta eliminar no existe o ya fue dado de baja.");

            _clienteRepository.DeleteLogico(dni);
            DVManager_08YS.Recalcular();
            _bitacoraBll.RegistrarEvento(Evento.ClienteEliminado, targetUsername: dni.ToString());
        }

        public List<Cliente_790MY> GetAll()
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerClientes);
            return _clienteRepository.GetAll();
        }

        public bool Exists(int dni)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerClientes);
            return _clienteRepository.Exists(dni);
        }

        public Cliente_790MY GetByDni(int dni)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.VerClientes);
            return _clienteRepository.GetByDni(dni);
        }

        // ── Serialización / Deserialización XML de colecciones ──────────────
        //
        // Formato único: XML nativo de .NET (System.Xml.Serialization.XmlSerializer).
        // Todas las operaciones trabajan SIEMPRE sobre List<Cliente_790MY>: exportar
        // 1 cliente o N clientes produce el mismo documento (<ArrayOfCliente_790MY>),
        // y al importar se acepta tanto esa colección como un único <Cliente_790MY>
        // suelto, que se devuelve como lista de 1 elemento. No requieren permiso
        // adicional: el llamador ya lo validó en la GUI.

        private static readonly XmlSerializer _serializerLista   = new XmlSerializer(typeof(List<Cliente_790MY>));
        private static readonly XmlSerializer _serializerCliente = new XmlSerializer(typeof(Cliente_790MY));

        /// <summary>Serializa la lista de clientes a una cadena XML bien formada (UTF-8).</summary>
        /// <exception cref="ArgumentNullException">La lista es null.</exception>
        /// <exception cref="ArgumentException">La lista está vacía.</exception>
        public string SerializarClientesXML(List<Cliente_790MY> clientes)
        {
            if (clientes == null) throw new ArgumentNullException(nameof(clientes));
            if (clientes.Count == 0) throw new ArgumentException("Debe indicar al menos un cliente para serializar.");

            using (var writer = new Utf8StringWriter())
            {
                _serializerLista.Serialize(writer, clientes);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Deserializa el contenido XML a una lista de clientes. Admite una colección
        /// (&lt;ArrayOfCliente_790MY&gt;) o un único cliente (&lt;Cliente_790MY&gt;).
        /// </summary>
        /// <exception cref="ArgumentException">El contenido está vacío o no es un XML válido de clientes.</exception>
        public List<Cliente_790MY> DeserializarClientesXML(string contenidoXml)
        {
            if (string.IsNullOrWhiteSpace(contenidoXml))
                throw new ArgumentException("El contenido XML a deserializar está vacío.");

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
                throw new ArgumentException($"El contenido no es un XML válido de clientes: {ex.Message}", ex);
            }

            throw new ArgumentException("El XML no contiene clientes con el formato esperado.");
        }

        /// <summary>Serializa la lista de clientes y la guarda en el archivo XML indicado.</summary>
        public void SerializarArchivoXML(List<Cliente_790MY> clientes, string rutaArchivo)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo)) throw new ArgumentException("La ruta del archivo no puede estar vacía.");
            string xml = SerializarClientesXML(clientes);
            File.WriteAllText(rutaArchivo, xml, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        }

        /// <summary>Lee el archivo XML indicado y devuelve la lista de clientes que contiene.</summary>
        public List<Cliente_790MY> DeserializarArchivoXML(string rutaArchivo)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo)) throw new ArgumentException("La ruta del archivo no puede estar vacía.");
            if (!File.Exists(rutaArchivo)) throw new ArgumentException($"El archivo '{rutaArchivo}' no existe.");
            return DeserializarClientesXML(File.ReadAllText(rutaArchivo, Encoding.UTF8));
        }

        // StringWriter declara "utf-16" en el encabezado XML; este declara "utf-8",
        // que es la codificación con la que efectivamente se guarda el archivo.
        private sealed class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => new UTF8Encoding(false);
        }

        private static void ValidarDatos(int dni, string nombre, string apellido, string email, string telefono)
        {
            if (dni <= 0)
                throw new ArgumentException("El DNI debe ser un número positivo.");

            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(apellido))
                throw new ArgumentException("El apellido es obligatorio.");

            if (string.IsNullOrWhiteSpace(telefono))
                throw new ArgumentException("El teléfono es obligatorio.");

            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("El email ingresado no es válido.");
        }
    }
}
