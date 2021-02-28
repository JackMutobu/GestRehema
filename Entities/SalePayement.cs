using System;

namespace GestRehema.Entities
{
    public class SalePayement
    {
        public int Id { get; set; }

        public int SaleId { get; set; }
        public Sale? Sale {get;set;}

        public decimal AmountPaid { get; set; }

        public int PayementId { get; set; }
        public Payement? Payement { get; set; }

        public DateTime Date { get; set; }
    }
}
