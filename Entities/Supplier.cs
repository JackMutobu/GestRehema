using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GestRehema.Entities
{
    public class Supplier:IBaseEntity
    {
        public Supplier()
        {
            Articles = new Collection<ArticleSupplier>();
            Supplies = new Collection<SupplyItem>();
        }
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string Name { get; set; } = null!;

        public string? Adresse { get; set; }

        public string NumTelephone { get; set; } = null!;

        public string? Email { get; set; }

        public string? ImageUrl { get; set; }

        public int WalletId { get; set; }
        public Wallet? Wallet { get; set; }

        public string SupplierType { get; set; } = Entities.SupplierType.Ordinary;

        public ICollection<ArticleSupplier> Articles { get; set; }

        public ICollection<SupplyItem> Supplies { get; set; }

    }
}
