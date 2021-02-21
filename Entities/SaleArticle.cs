using System;

namespace GestRehema.Entities
{
    public class SaleArticle
    {
        public int SaleId { get; set; }
        public Sale? Sale { get; set; }

        public int ArticleId { get; set; }
        public Article? Article { get; set; } 

        public int Quantity { get; set; }

        public decimal UnitSellingPrice { get; set; }
        public decimal UnitRealSellingPrice { get; set; }
        public decimal UnitBuyingPrice { get; set; }

        public DateTime Date { get; set; }

    }
}
