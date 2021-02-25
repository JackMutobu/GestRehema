using System;

namespace GestRehema.Entities
{
    public class WalletHistory
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public int? AmountExcessWalletId { get; set; }

        public Wallet? AmountExcessWallet { get; set; }

        public int? AmountDebtWalletId { get; set; }

        public Wallet? AmountDebtWallet { get; set; }

        public string Description { get; set; } = null!;
    }
}
