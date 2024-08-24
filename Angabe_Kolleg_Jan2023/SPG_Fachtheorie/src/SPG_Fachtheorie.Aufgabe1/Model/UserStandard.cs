using System;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class UserStandard : User
    {
        public UserStandard(string firstName, string lastName, string email, DateTime startDate, DateTime? endDate, bool informed) : base(firstName: firstName, lastName:  lastName, email:  email, startDate: startDate, endDate: endDate)
        {
            Informed = informed;
        }

        public bool Informed { get; set; }
    }
}
