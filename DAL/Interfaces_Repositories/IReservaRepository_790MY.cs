using BE_08YS;
using System;
using System.Collections.Generic;

namespace DAL_08YS.Interfaces_Repositories
{
    // Acceso a la tabla Reservas.
    public interface IReservaRepository_790MY
    {
        // Inserta la reserva solo si la mesa sigue libre en ese turno, todo en una
        // transacción con bloqueo, para que dos puestos no tomen la misma mesa.
        // Con ocuparMesaFisica (walk-in) además exige que la mesa no esté ocupada y la
        // deja Ocupada. Devuelve false si la mesa ya no estaba disponible.
        bool AddSiMesaDisponible(Reserva_790MY reserva, bool ocuparMesaFisica);

        Reserva_790MY GetById(int reservaId);

        // Todos los filtros son opcionales: null no filtra.
        List<Reserva_790MY> Buscar(DateTime? desde, DateTime? hasta, int? clienteDni, EstadoReserva_790MY? estado);

        // Mesas abiertas a cargo del camarero.
        List<Reserva_790MY> GetAsistidasPorCamarero(int camareroDni);

        // Abre la mesa de una reserva: la pasa a Asistida con el camarero y deja la mesa
        // Ocupada. Devuelve false si la reserva ya no estaba Confirmada o la mesa no
        // estaba disponible (por ejemplo, la abrió otro puesto).
        bool AbrirReserva(int reservaId, int camareroDni);
    }
}
