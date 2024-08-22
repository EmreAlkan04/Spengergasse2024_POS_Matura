using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTSept2022.Aufgabe2.Domain
{
    public class Enrollment
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public Course CourseNavigation { get; set; } = default!;
        public string StudentRegistrationNumber { get; set; } = string.Empty;
        public Student StudentNavigation { get; set; } = default!;

        public List<Exam> Exams { get; set; } = new();
    }
}
