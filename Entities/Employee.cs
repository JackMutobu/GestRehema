using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GestRehema.Entities
{
    public class Employee:Person,IBaseEntity
    {
        public Employee()
        {
            Sales = new Collection<Sale>();
            Expenses = new Collection<Expense>();
        }
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string Position { get; set; } = null!;

        public int EntrepriseId { get; set; }

        public Entreprise? Entreprise { get; set; }

        public Guid? UserId { get; set; }

        public User? User { get; set; }

        public ICollection<Sale> Sales { get; set; }

        public ICollection<Expense> Expenses { get; set; }
    }
}
