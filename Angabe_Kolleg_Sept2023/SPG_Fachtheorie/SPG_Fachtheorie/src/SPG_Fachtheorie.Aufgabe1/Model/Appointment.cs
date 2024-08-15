using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    /// <summary>
    /// Der Termin des Patienten.
    /// </summary>
    public class Appointment
    {

#pragma warning disable CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        protected Appointment() {         }
#pragma warning restore CS8618 // Ein Non-Nullable-Feld muss beim Beenden des Konstruktors einen Wert ungleich NULL enthalten. Erwägen Sie die Deklaration als Nullable.
        public Appointment(DateTime date, TimeSpan time, Patient patient, DateTime created, AppointmentState appointmentState)
        {
            Date = date;
            Time = time;
            Patient = patient;
            Created = created;
            AppointmentState = appointmentState;
        }

        [Key]
        public int AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public Patient Patient { get; set; }
        public DateTime Created { get; set; }
        
        public AppointmentState AppointmentState { get; set; }

        //public int AppointmentStateId { get; set; }
    }
}