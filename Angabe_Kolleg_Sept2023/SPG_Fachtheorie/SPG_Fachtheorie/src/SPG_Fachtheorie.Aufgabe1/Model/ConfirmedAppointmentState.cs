using System;
using System.ComponentModel.DataAnnotations;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class ConfirmedAppointmentState : AppointmentState
    {
        protected ConfirmedAppointmentState()
        {
        }
        public ConfirmedAppointmentState(DateTime created, string type, Appointment appointment, TimeSpan duration, string? infotext) : base(created: created, type: type)
        {
            Duration = duration;
            Infotext = infotext;
        }

       
        public TimeSpan Duration { get; set; }
        public string? Infotext { get; set; }
    }
}