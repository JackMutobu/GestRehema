using System;

namespace GestRehema.Entities
{
    public class Wallet:IBaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public decimal AmountInDebt { get; set; }

        public decimal AmountInExcess { get; set; }

        public decimal AmountOwned { get; set; }

        public decimal Balance => (AmountOwned + AmountInExcess) >= AmountInDebt ? AmountOwned + AmountInExcess - AmountInDebt : 0;

        public Customer? Customer { get; set; }

        public Entreprise? Entreprise { get; set; }
    }
}
