using System;
using System.ComponentModel.DataAnnotations;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class DeletedAppointmentState : AppointmentState
    {
        protected DeletedAppointmentState()
        {
        }
        public DeletedAppointmentState(DateTime created, string type, Appointment appointment) : base(created:created, type:type)
        {
        }

        
    }
}