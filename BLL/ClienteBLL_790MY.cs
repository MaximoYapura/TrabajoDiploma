using BE_08YS;
using BLL_08YS.Exceptions;
using DAL_08YS;
using DAL_08YS.Interfaces_Repositories;
using Service_08YS;
using Service_08YS.Entities.Acceso;
using Service_08YS.Entities.Bitacora;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        public void RegistrarCliente(int dni, string nombre, string apellido, string email, string telefono, string direccion = null)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearCliente);

            ValidarDatos(dni, nombre, apellido, email, telefono);

            if (_clienteRepository.Exists(dni))
                throw new ClienteDniDuplicadoException_790MY(dni);

            var cliente = new Cliente_790MY(dni, nombre, apellido, email, telefono, direccion);

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
        }

        public void EliminarCliente(int dni)
        {
            SessionManager_08YS.Instance.ValidatePermission(Permisos.CrearCliente);

            if (!_clienteRepository.Exists(dni))
                throw new ArgumentException("El cliente que intenta eliminar no existe o ya fue dado de baja.");

            _clienteRepository.DeleteLogico(dni);
            DVManager_08YS.Recalcular();
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
