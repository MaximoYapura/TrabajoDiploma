using BE_08YS;
using System;
using System.Collections.Generic;

namespace DAL_08YS.Interfaces_Repositories
{
    public interface IReservaRepository_790MY
    {
        void Add(Reserva_790MY reserva);

        // Todos los filtros son opcionales (null = sin filtrar por ese campo).
        List<Reserva_790MY> Buscar(DateTime? desde, DateTime? hasta, int? clienteDni, EstadoReserva_790MY? estado);

        void CancelarReserva(int reservaId);
    }
}
