using System;

namespace GestRehema.Entities
{
    public class Payement : IBaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string Method { get; set; } = null!;

        public int? TransactionId { get; set; }

        public string? PayementOrganization { get; set; }

        public string? AccountNumber { get; set; }

        public decimal AmountInUSD { get; set; }

        public decimal AmountInCDF { get; set; }

        public bool ToCompany { get; set; }

        public SalePayement? SalePayement { get; set; }

        public int? WalletId { get; set; }

        public Wallet? Wallet { get; set; }

    }
}
