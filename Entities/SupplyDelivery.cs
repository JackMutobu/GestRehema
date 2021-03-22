using System;

namespace GestRehema.Entities
{
    public class SupplyDelivery
    {
        public int Id { get; set; }

        public int SupplyId { get; set; }
        public Supply? Supply { get; set; }

        public int ArticleId { get; set; }
        public Article? Article { get; set; }
        public double DeliveredQuantity { get; set; }

        public DateTime Date { get; set; }
    }
}
