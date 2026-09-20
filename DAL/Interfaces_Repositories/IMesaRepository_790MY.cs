using BE_08YS;
using System;
using System.Collections.Generic;

namespace DAL_08YS.Interfaces_Repositories
{
    public interface IMesaRepository_790MY
    {
        // Existencia "cruda": no filtra por Activo. Sirve para prevenir una violacion
        // de clave primaria al dar de alta un numero que ya existe (aunque este dado
        // de baja logicamente), y para validar que una mesa a modificar/eliminar exista.
        bool Exists(int nroMesa);

        // Filtra por Activo = 1 (borrado logico).
        List<Mesa_790MY> GetAll();

        // Filtra por Activo = 1 ademas de capacidad y disponibilidad de turno.
        // No filtra por Mesa.Estado: ese campo refleja el estado físico en tiempo real
        // (util para una pantalla de piso "ahora"), pero no predice la disponibilidad
        // de la mesa en una fecha/turno futuro, que depende exclusivamente de Reservas.
        List<Mesa_790MY> GetDisponibles(DateTime fecha, TimeSpan hora, int comensales);

        void Add(Mesa_790MY mesa);
        void Update(Mesa_790MY mesa);
        void DeleteLogico(int nroMesa);

        void UpdateEstado(int nroMesa, EstadoMesa_790MY estado);
    }
}
