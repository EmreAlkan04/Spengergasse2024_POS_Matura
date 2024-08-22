using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTSept2022.Aufgabe2.Domain
{
    public class Exam
    {
        public int Id { get; set; }
        public int? Grade { get; set; }
        public string StudentPhoneNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string EnrollmentNumber { get; set; } = string.Empty;

        public int EnrollmentId { get; set; }
        public Enrollment EnrollmentNavigation { get; set; } = default!;
        public int ProfessorId { get; set; }
        public Professor ProfessorNavigation { get; set; } = default!;
        public int StudentRegistrationNumber { get; set; }
        public Student StudentNavigation { get; set; } = default!;
    }
}
