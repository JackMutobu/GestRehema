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
            Suppliers = new Collection<ArticleSupplier>();
            Supplies = new Collection<SupplyArticle>();
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

        public double InStock { get; set; }

        public string Category { get; set; } = null!;

        public string Conditionement { get; set; } = null!;

        public double QtyPerConditionement { get; set; }

        public double AwaitingDeliveryToCompany { get; set; }
        public DateTime? LastAwaitingDeliveryToCompanyUpdate { get; set; }

        public double AwaitingDeliveryToCustomers { get; set; }
        public DateTime? LastAwaitingDeliveryToCustomersUpdate { get; set; }

        public ICollection<SaleArticle> Sales { get; set; }

        public ICollection<SupplyArticle> Supplies { get; set; }

        public ICollection<ArticleSupplier> Suppliers { get; set; }

    }
}
