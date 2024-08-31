using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using SPG_Fachtheorie.Aufgabe2.Infrastructure;
using SPG_Fachtheorie.Aufgabe2.Model;
using System;
using System.Linq;

namespace SPG_Fachtheorie.Aufgabe2.Services
{
    public record ContingentStatistics(int SoldTickets, int ReservedTickets, Show Show);

    public class EventService
    {
        private readonly EventContext _db;

        public EventService(EventContext db)
        {
            _db = db;
        }

        public ContingentStatistics CalcContingentStatistics(int contingentId)
        {
            var contigent = _db.Contingents
                .Include(s => s.Show)
                .Include(s => s.Tickets)
                .FirstOrDefault(s => s.Id == contingentId) ?? throw new EventServiceException("nicht gefunden");
            var sold = contigent.Tickets
                .Where(s => s.TicketState == TicketState.Sold).Sum(s => s.Pax + 1);
            var reserved = contigent.Tickets
                .Where(s => s.TicketState == TicketState.Reserved).Sum(s => s.Pax + 1);
            return new ContingentStatistics(sold, reserved, contigent.Show);

            //throw new NotImplementedException();
        }

        public int CreateReservation(int guestId, int contingentId, int pax, DateTime dateTime)
        {   
            var contigent = _db.Contingents
                .Include(s => s.Show)
                .Include(s => s.Tickets)
                .ThenInclude(s => s.Guest)
                .FirstOrDefault(s => s.Id == contingentId) ?? throw new EventServiceException("Invalid contingent id");
            if (contigent.Show.Date < dateTime.AddDays(14)) throw new EventServiceException("The show is too close in time");
            var guest = _db.Guests
                .Include(s => s.Tickets)
                .ThenInclude(s => s.Contingent)
                .ThenInclude(s => s.Show)
                .FirstOrDefault(s => s.Id == guestId) ?? throw new EventServiceException("Invalid guest id");
            if (guest.Tickets.Any(s => s.ReservationDateTime == dateTime || s.Contingent.Id == contingentId))
            {
                throw new EventServiceException("A reservation or purchase has already been made for this contingent");
            }
            if (contigent.AvailableTickets < contigent.Tickets.Count() + pax + 1)
            {
                throw new EventServiceException("Show is sold out");
            }
            var newTicket = new Ticket(guest, contigent, TicketState.Reserved, dateTime, pax);
            _db.Tickets.Add(newTicket);
            try
            {
                _db.SaveChanges();
            }
            catch(DbUpdateException ex)
            {
                throw new EventServiceException(ex.Message);
            }
            return newTicket.Id;
            //throw new NotImplementedException();
        }
    }
}