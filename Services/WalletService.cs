using GestRehema.Data;
using GestRehema.Entities;
using Splat;
using System;
using System.Linq;

namespace GestRehema.Services
{
    public interface IWalletService
    {
        Wallet AddDebt(int walletId, int entrepriseWalletId, decimal amount, int? payementId = null);
        Wallet AddExcess(int walletId, int entrepriseWalletId, decimal amount, int? payementId = null);
        Wallet AddToEntreprise(int entrepriseWalletId, decimal amount, int? payementId = null);
    }

    public class WalletService : IWalletService
    {
        private readonly AppDbContext _dbContext;

        public WalletService()
        {
            _dbContext = Locator.Current.GetService<AppDbContext>();
        }

        public Wallet AddDebt(int walletId, int entrepriseWalletId, decimal amount, int? payementId = null)
        {
            if (walletId == entrepriseWalletId)
                throw new ArgumentException("Comptes virtuels invalides");
            if (amount <= 0)
                throw new ArgumentException("Le montant doit etre supérieur à 0");

            var wallet = _dbContext.Wallets.Single(x => x.Id == walletId);
            var entrepriseWallet = _dbContext.Wallets.Single(x => x.Id == entrepriseWalletId);
            wallet.AmountInDebt += amount;
            entrepriseWallet.AmountInExcess += amount;
            wallet.UpdatedAt = DateTime.UtcNow;
            entrepriseWallet.UpdatedAt = DateTime.UtcNow;
            var newWalletHistory = new WalletHistory
            {
                Date = DateTime.UtcNow,
                Description = $"Ajout d'une dette au compte {walletId}",
                Amount = amount,
                AmountDebtWalletId = walletId,
                AmountExcessWalletId = entrepriseWalletId,
                PayementId = payementId
            };

            entrepriseWallet = RegularizeWallet(entrepriseWallet, true);
            wallet = RegularizeWallet(wallet);

            _dbContext.Wallets.Update(wallet);
            _dbContext.Wallets.Update(entrepriseWallet);
            _dbContext.WalletHistories.Add(newWalletHistory);
            _dbContext.SaveChanges();

            return wallet;
        }

        public Wallet AddExcess(int walletId, int entrepriseWalletId, decimal amount, int? payementId = null)
        {
            if (walletId == entrepriseWalletId)
                throw new ArgumentException("Comptes virtuels invalides");
            if (amount <= 0)
                throw new ArgumentException("Le montant doit etre supérieur à 0");

            var wallet = _dbContext.Wallets.Single(x => x.Id == walletId);
            var entrepriseWallet = _dbContext.Wallets.Single(x => x.Id == entrepriseWalletId);
            wallet.AmountInExcess += amount;
            entrepriseWallet.AmountInDebt += amount;
            wallet.UpdatedAt = DateTime.UtcNow;
            entrepriseWallet.UpdatedAt = DateTime.UtcNow;
            var newWalletHistory = new WalletHistory
            {
                Date = DateTime.UtcNow,
                Description = $"Ajout d'une dette au compte {entrepriseWalletId}",
                Amount = amount,
                AmountDebtWalletId = walletId,
                AmountExcessWalletId = entrepriseWalletId,
                PayementId = payementId
            };

            entrepriseWallet = RegularizeWallet(entrepriseWallet, true);
            wallet = RegularizeWallet(wallet);

            _dbContext.Wallets.Update(wallet);
            _dbContext.Wallets.Update(entrepriseWallet);
            _dbContext.WalletHistories.Add(newWalletHistory);
            _dbContext.SaveChanges();

            return wallet;
        }

        public Wallet AddToEntreprise(int entrepriseWalletId, decimal amount, int? payementId = null)
        {
            if (amount <= 0)
                throw new ArgumentException("Le montant doit etre supérieur à 0");

            var entrepriseWallet = _dbContext.Wallets.Single(x => x.Id == entrepriseWalletId);
            entrepriseWallet.AmountOwned += amount;
            entrepriseWallet.UpdatedAt = DateTime.UtcNow;

            var newWalletHistory = new WalletHistory
            {
                Date = DateTime.UtcNow,
                Description = $"Ajout de {amount}$ à la caisse",
                Amount = amount,
                AmountDebtWalletId = entrepriseWalletId,
                AmountExcessWalletId = entrepriseWalletId,
                PayementId = payementId
            };

            _dbContext.Wallets.Update(entrepriseWallet);
            _dbContext.WalletHistories.Add(newWalletHistory);
            _dbContext.SaveChanges();

            return entrepriseWallet;
        }

        private Wallet RegularizeWallet(Wallet wallet, bool isEntrerpise = false)
        {
            var balance = (wallet.AmountInExcess - wallet.AmountInDebt) + wallet.AmountOwned;
            if (balance >= 0)
            {
                wallet.AmountInExcess = isEntrerpise ? 0 : balance;
                wallet.AmountInDebt = 0;
                wallet.AmountOwned = isEntrerpise ? balance : 0;
            }
            else
            {
                wallet.AmountInDebt = balance * -1;
                wallet.AmountInExcess = 0;
                wallet.AmountOwned = 0;
            }
            return wallet;
        }

    }
}
