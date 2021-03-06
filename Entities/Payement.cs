using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GestRehema.Entities
{
    public class Payement : IBaseEntity
    {
        public Payement()
        {
            WalletHistory = new Collection<WalletHistory>();

        }
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Date => CreatedAt.ToLocalTime().ToShortDateString();

        public DateTime? UpdatedAt { get; set; }

        public string Method { get; set; } = null!;

        public string? TransactionId { get; set; }

        public string? PayementOrganization { get; set; }

        public string? AccountNumber { get; set; }

        public decimal AmountInUSD { get; set; }

        public decimal AmountInCDF { get; set; }

        public decimal TotalPaid { get; set; }

        public bool ToCompany { get; set; }

        public SalePayement? SalePayement { get; set; }

        public ICollection<WalletHistory> WalletHistory { get; set; }

        public void SetTotalPaid(decimal exchangeRate)
        {
            TotalPaid = decimal.Round(AmountInUSD + (AmountInCDF / exchangeRate),2, MidpointRounding.AwayFromZero);
        }

    }
}
