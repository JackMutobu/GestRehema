﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GestRehema.Entities
{
    public class Supplier:IBaseEntity
    {
        public Supplier()
        {
            Articles = new Collection<ArticleSupplier>();
            Supplies = new Collection<Supply>();
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

        public ICollection<ArticleSupplier> Articles { get; set; }

        public ICollection<Supply> Supplies { get; set; }

    }
}
