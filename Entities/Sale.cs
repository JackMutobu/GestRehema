using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GestRehema.Entities
{
    public class Sale : IBaseEntity
    {
        public Sale()
        {
            ArticleSold = new Collection<SaleArticle>();
            PayementHistory = new Collection<SalePayement>();
            DeliveryHistory = new Collection<SaleDelivery>();
        }
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public ICollection<SaleArticle> ArticleSold { get; set; }

        public ICollection<SalePayement> PayementHistory { get; set; }

        public ICollection<SaleDelivery> DeliveryHistory { get; set; }
    }
}
