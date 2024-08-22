using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTSept2022.Aufgabe2.Domain
{
    public class AllowedExamDate
    {
        public Guid Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        
        public int CourseId { get; set; }
        public Course CourseNavigation { get; set; } = default!;
    }
}
