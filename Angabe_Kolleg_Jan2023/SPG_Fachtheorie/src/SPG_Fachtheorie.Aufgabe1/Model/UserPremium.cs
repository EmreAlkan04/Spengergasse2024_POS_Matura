using System;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class UserPremium : User
    {
        public UserPremium(string firstName, string lastName, string email, DateTime startDate, DateTime? endDate, decimal monthlyFee, int mindestvertragsdauerInMonaten) : base(firstName: firstName, lastName: lastName, email: email, startDate: startDate, endDate: endDate)
        {
            MonthlyFee = monthlyFee;
            MindestvertragsdauerInMonaten = mindestvertragsdauerInMonaten;
        }

        public decimal MonthlyFee { get; set; }
        public int MindestvertragsdauerInMonaten { get; set; }
    }
}
