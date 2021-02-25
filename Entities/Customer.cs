using System;

namespace GestRehema.Entities
{
    public class Customer:IBaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string Name { get; set; } = null!;

        public string? Adresse { get; set; }

        public string NumTelephone { get; set; } = null!;

        public string? Email { get; set; }

        public string CustomerType { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public Organization? Organization { get; set; }

        public Store? Store { get; set; }

        public int WalletId { get; set; }
        public Wallet? Wallet { get; set; }

    }
}
