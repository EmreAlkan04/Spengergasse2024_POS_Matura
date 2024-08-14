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
            var contingent = _db.Contingents
                .Include(c => c.Tickets) // Ensure that the Tickets are loaded
                .Include(c => c.Show)
                .FirstOrDefault(c => c.Id == contingentId ); // Get the contingent

            if (contingent == null)
            {
                throw new ArgumentException($"No contingent found with ID {contingentId}", nameof(contingentId));
            }

            // Count the sold and reserved tickets
            int SoldTickets = contingent.Tickets.Where(t => t.TicketState == TicketState.Sold).Sum(t => t.Pax + 1);
            int ReservedTickets = contingent.Tickets.Where(t => t.TicketState == TicketState.Reserved).Sum(t => t.Pax + 1);
            Show OwnedShow = contingent.Show;

            // Return the statistics
            return new ContingentStatistics(SoldTickets, ReservedTickets, OwnedShow);
        }

        public int CreateReservation(int guestId, int contingentId, int pax, DateTime dateTime)
        {
            var guest = _db.Guests.Find(guestId);
            var contingent = _db.Contingents.Find(contingentId);
            if (guest == null)
            {
                throw new EventServiceException("Invalid guest id");
            }
            if (contingent == null)
            {
                throw new EventServiceException("Invalid contingent id");
            }



            if(contingent.AvailableTickets < (pax+1))
            {
                throw new EventServiceException("Show is sold out");
            }
           

            if(dateTime < DateTime.Now.AddDays(14))
            {
                throw new EventServiceException("The show is too close in time");
            }

           if(guest.Tickets.Any(t => t.Contingent.Id == contingentId && t.ReservationDateTime == dateTime))
            {
                throw new EventServiceException("A reservation or purchase has already been made for this contingent");
            }


            var ticket = new Ticket(guest, contingent, TicketState.Reserved, dateTime, pax);

            _db.Add(ticket);
            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                throw new EventServiceException("An error occurred while saving the reservation", e);
            }

            return guest.Id;
        }
    }
}