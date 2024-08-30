using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.DependencyInjection;
using SPG_Fachtheorie.Aufgabe2.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPG_Fachtheorie.Aufgabe2
{
    public class AppointmentService
    {
        private readonly AppointmentContext _db;

        public AppointmentService(AppointmentContext db)
        {
            _db = db;
        }

        public bool AskForAppointment(Guid offerId, Guid studentId, DateTime date)
        {
            // TOTO: Implementiere die Methode
            //var schueler = _db.Students.FirstOrDefault(x => x.Id == studentId);
            //var angebot = _db.Offers
            //    .FirstOrDefault(x => x.Id == offerId && x.From == date);

            var appointment = _db.Appointments
                .Include(x => x.Offer)
                .Include(x => x.Student)
                .FirstOrDefault(x => x.Offer.Id == offerId || x.Student.Id == studentId);

            if (appointment == null || date != appointment.Date)
            {
                return false;
            }
            _db.Add(appointment);
            try
            {
                _db.SaveChanges();
            }
            catch (DbException e)
            {
                throw new ServiceException(e.Message);
            }
            return true;
        }

        public bool ConfirmAppointment(Guid appointmentId)
        {
            // TOTO: Implementiere die Methode
            var appintment = _db.Appointments
                //.Include(x => x.State)
                .FirstOrDefault(x => x.Id == appointmentId);
            if (appintment == null || appintment.State != Model.AppointmentState.AskedFor)
            {
                return false;
            }
            appintment.State = Model.AppointmentState.Confirmed;
            _db.Update(appintment);
            try
            {
                _db.SaveChanges();
            }
            catch (DbException e)
            {
                throw new ServiceException(e.Message);
            }
            return true;

            //return default;
        }

        public bool CancelAppointment(Guid appointmentId, Guid studentId)
        {
            var appointment = _db.Appointments
                 .Include(s => s.Student)
                 .FirstOrDefault(s => s.Id == appointmentId && s.StudentId == studentId ) ?? throw new ServiceException("appointment nicht gefunden");
            var student = appointment.Student;
            //if (appointment.State == AppointmentState.Cancelled) return false;
            if (student is Coach && appointment.State == AppointmentState.Cancelled || appointment.State == AppointmentState.TookPlace)
            {
                return false;
            }
            if(student is Student && appointment.State != AppointmentState.AskedFor)
            {
                return false;
            }
            appointment.State = AppointmentState.Cancelled;
            return true;
        }
    }
}
