using System;

namespace GestRehema.Entities
{
    public class Employee:Person,IBaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string Position { get; set; } = null!;

        public int EntrepriseId { get; set; }

        public Entreprise? Entreprise { get; set; }

        public Guid? UserId { get; set; }

        public User? User { get; set; }
    }
}
