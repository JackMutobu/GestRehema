using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GestRehema.Entities
{
    public class Article:IBaseEntity
    {
        public Article()
        {
            Sales = new Collection<SaleArticle>();
        }

        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string Name { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string? TechnicalCode { get; set; }

        public string? UnitOfMeasure { get; set; }

        public decimal SellingPrice { get; set; } 

        public decimal BuyingPrice { get; set; } 

        public int InStock { get; set; }

        public int AwaitingDeliveryToCompany { get; set; }
        public DateTime? LastAwaitingDeliveryToCompanyUpdate { get; set; }

        public int AwaitingDeliveryToCustomers { get; set; }
        public DateTime? LastAwaitingDeliveryToCustomersUpdate { get; set; }

        public ICollection<SaleArticle> Sales { get; set; }

    }
}
