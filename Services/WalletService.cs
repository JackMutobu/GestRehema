using GestRehema.Data;
using GestRehema.Entities;
using Splat;
using System;
using System.Linq;

namespace GestRehema.Services
{
    public interface IWalletService
    {
        Wallet AddDebt(int walletId, int entrepriseWalletId, decimal amount);
        Wallet AddExcess(int walletId, int entrepriseWalletId, decimal amount);
        Wallet AddToEntreprise(int entrepriseWalletId, decimal amount);
    }

    public class WalletService : IWalletService
    {
        private readonly AppDbContext _dbContext;

        public WalletService()
        {
            _dbContext = Locator.Current.GetService<AppDbContext>();
        }

        public Wallet AddDebt(int walletId, int entrepriseWalletId, decimal amount)
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
                AmountExcessWalletId = entrepriseWalletId
            };

            _dbContext.Wallets.Update(wallet);
            _dbContext.Wallets.Update(entrepriseWallet);
            _dbContext.WalletHistories.Add(newWalletHistory);
            _dbContext.SaveChanges();

            return wallet;
        }

        public Wallet AddExcess(int walletId, int entrepriseWalletId, decimal amount)
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
                AmountExcessWalletId = entrepriseWalletId
            };

            _dbContext.Wallets.Update(wallet);
            _dbContext.Wallets.Update(entrepriseWallet);
            _dbContext.WalletHistories.Add(newWalletHistory);
            _dbContext.SaveChanges();

            return wallet;
        }

        public Wallet AddToEntreprise(int entrepriseWalletId, decimal amount)
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
                AmountExcessWalletId = entrepriseWalletId
            };

            _dbContext.Wallets.Update(entrepriseWallet);
            _dbContext.WalletHistories.Add(newWalletHistory);
            _dbContext.SaveChanges();

            return entrepriseWallet;
        }
    }
}
