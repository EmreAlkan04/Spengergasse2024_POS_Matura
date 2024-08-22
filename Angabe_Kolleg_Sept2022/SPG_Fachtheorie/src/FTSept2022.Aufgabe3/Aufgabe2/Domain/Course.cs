using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTSept2022.Aufgabe2.Domain
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public int Ects { get; set; }
        public int MaxStudents { get; set; }

        public int ProfessorId { get; set; }
        public Professor ProfessorNavigation { get; set; } = default!;

        public List<Enrollment> Enrollments { get; set; } = new();
        public List<AllowedExamDate> AllowedExamDates { get; set; } = new();
    }
}
