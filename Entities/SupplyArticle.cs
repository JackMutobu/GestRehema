using System;

namespace GestRehema.Entities
{
    public class SupplyArticle
    {
        public int SupplyId { get; set; }
        public Supply? Supply { get; set; }

        public int ArticleId { get; set; }
        public Article? Article { get; set; }

        public double Quantity { get; set; }

        public double QtyInUnitOfMeasure => Quantity * Article?.QtyPerConditionement ?? 0;

        public decimal TotalAmount => decimal.Round((decimal)Quantity * UnitBuyingPrice, 2, MidpointRounding.AwayFromZero);

        public decimal UnitBuyingPrice { get; set; }

        public DateTime Date { get; set; }
    }
}
