using System;

namespace GestRehema.Entities
{
    public class SupplyDelivery
    {
        public int Id { get; set; }

        public int SupplyItemId { get; set; }
        public SupplyItem? SupplyItem { get; set; }

        public int ArticleId { get; set; }
        public Article? Article { get; set; }
        public double DeliveredQuantity { get; set; }

        public DateTime Date { get; set; }
    }
}
