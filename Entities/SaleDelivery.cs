using System;

namespace GestRehema.Entities
{
    public class SaleDelivery
    {
        public int Id { get; set; }

        public int SaleId { get; set; }
        public Sale? Sale { get; set; }

        public int ArticleId { get; set; }
        public Article? Article { get; set; }
        public double DeliveredQuantity { get; set; }

        public DateTime Date { get; set; }
    }
}
