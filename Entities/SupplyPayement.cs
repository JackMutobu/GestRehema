using System;

namespace GestRehema.Entities
{
    public class SupplyPayement
    {
        public int Id { get; set; }

        public int SupplyItemId { get; set; }
        public SupplyItem? SupplyItem { get; set; }

        public decimal AmountPaid { get; set; }

        public int PayementId { get; set; }
        public Payement? Payement { get; set; }

        public DateTime Date { get; set; }
    }
}
