using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTSept2022.Aufgabe2.Domain
{
    public class Student
    {
        [Key]
        public string RegistrationNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }

        //public List<Exam> Exams { get; set; } = new();
        public List<Enrollment> Enrollments { get; set; } = new();
    }
}
