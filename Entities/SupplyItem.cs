using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GestRehema.Entities
{
    public class SupplyItem
    {
        public SupplyItem()
        {
            ArticlesSupplied = new Collection<SupplyArticle>();
            PayementHistory = new Collection<SupplyPayement>();
            DeliveryHistory = new Collection<SupplyDelivery>();
        }

        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime DateOperation { get; set; }

        public string ShortDateOperation => DateOperation.ToShortDateString();

        public string ShortUpdateAt => UpdatedAt.GetValueOrDefault().ToShortDateString();

        public string PayementStatus { get; set; } = SupplyPayementStatus.AwaitingPayement;

        public string DeliveryStatus { get; set; } = SupplyDeliveryStatus.AwaitingDelivery;

        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public int SupplyId { get; set; }
        public Supply? Supply { get; set; }


        public ICollection<SupplyArticle> ArticlesSupplied { get; set; }

        public ICollection<SupplyPayement> PayementHistory { get; set; }

        public ICollection<SupplyDelivery> DeliveryHistory { get; set; }
    }
}
