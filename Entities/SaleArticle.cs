using System;

namespace GestRehema.Entities
{
    public class SaleArticle
    {
        public int SaleId { get; set; }
        public Sale? Sale { get; set; }

        public int ArticleId { get; set; }
        public Article? Article { get; set; } 

        public double Quantity { get; set; }

        public double QtyInUnitOfMeasure => Quantity * Article?.QtyPerConditionement ?? 0;

        public decimal TotalAmount => decimal.Round((decimal)Quantity * UnitSellingPrice,2, MidpointRounding.AwayFromZero);

        public decimal Commission => decimal.Round((UnitSellingPrice - UnitRealSellingPrice) * (decimal)Quantity,2, MidpointRounding.AwayFromZero);

        public decimal UnitSellingPrice { get; set; }
        public decimal UnitRealSellingPrice { get; set; }
        public decimal UnitBuyingPrice { get; set; }

        public DateTime Date { get; set; }

    }
}
