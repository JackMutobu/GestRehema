using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GestRehema.Entities
{
    public class Entreprise:IBaseEntity
    {
        public Entreprise()
        {
            Employees = new Collection<Employee>();
        }
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? LogoUrl { get; set; }

        public decimal TauxDuJour { get; set; }

        public DateTime DateDuJour { get; set; } = DateTime.UtcNow;

        public string Date => DateDuJour.ToString("dd/MM/yyyy");

        public ICollection<Employee> Employees { get; set; }

    }
}
