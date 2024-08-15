using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class AppointmentState
    {

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        protected AppointmentState()
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        {
        }
        public AppointmentState(DateTime created, string type)
        {

            Created = created;
            Type = type;
            //Appointment = appointment;
        }

        [Key, ForeignKey("Appointment")]
        public int AppointmentStateId { get; set; }
        public Appointment? Appointment { get; set; }
        public DateTime Created { get; set; }
        public string Type { get; set; }

    }
}