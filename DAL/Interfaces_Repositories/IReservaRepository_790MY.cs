using BE_08YS;
using System;
using System.Collections.Generic;

namespace DAL_08YS.Interfaces_Repositories
{
    public interface IReservaRepository_790MY
    {
        /// <summary>
        /// Alta atomica: dentro de una transaccion verifica que la mesa exista, este
        /// en servicio, tenga capacidad y no tenga otra reserva que la ocupe en el
        /// mismo (Fecha, Hora); recien entonces inserta. Bloquea el rango con
        /// UPDLOCK/HOLDLOCK para que dos puestos no confirmen la misma mesa/turno.
        /// Si ocuparMesaFisica = true (Walk-in) ademas exige que la mesa no este
        /// fisicamente Ocupada y la marca como Ocupada en la misma transaccion.
        /// Devuelve false (sin cambios) si la mesa ya no esta disponible.
        /// Asigna reserva.ReservaID cuando inserta.
        /// </summary>
        bool AddSiMesaDisponible(Reserva_790MY reserva, bool ocuparMesaFisica);

        Reserva_790MY GetById(int reservaId);

        // Todos los filtros son opcionales (null = sin filtrar por ese campo).
        List<Reserva_790MY> Buscar(DateTime? desde, DateTime? hasta, int? clienteDni, EstadoReserva_790MY? estado);

        /// <summary>Reservas Asistidas (mesas abiertas) a cargo del camarero.</summary>
        List<Reserva_790MY> GetAsistidasPorCamarero(int camareroDni);

        /// <summary>
        /// CUN-004. En una transaccion: Confirmada -> Asistida + CamareroDNI y
        /// Mesa -> Ocupada. False (sin cambios) si la reserva ya no esta Confirmada
        /// o la mesa esta fisicamente ocupada / fuera de servicio.
        /// </summary>
        bool AbrirReserva(int reservaId, int camareroDni);
    }
}
