using GestRehema.Data;
using GestRehema.Entities;
using Microsoft.EntityFrameworkCore;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestRehema.Services
{
    public interface IPayementService
    {
        Payement AddPayement(Payement payement);
        List<Payement> GetPayements(int walletId);
    }

    public class PayementService : IPayementService
    {
        private readonly AppDbContext _dbContext;

        public PayementService()
        {
            _dbContext = Locator.Current.GetService<AppDbContext>();
        }

        public Payement AddPayement(Payement payement)
        {
            payement.CreatedAt = DateTime.UtcNow;
            payement.UpdatedAt = payement.CreatedAt;
            _dbContext.Payements.Add(payement);
            _dbContext.SaveChanges();
            return _dbContext.Payements.Single(x => x.CreatedAt == payement.CreatedAt);
        }

        public List<Payement> GetPayements(int walletId)
        {
            var finalPayement = _dbContext.WalletHistories
                .Include(x => x.Payement)
                .Where(x => x.AmountDebtWalletId == walletId || x.AmountExcessWalletId == walletId)
                .Select(x => x.Payement)
                .ToList();
                


            return finalPayement;
        }
    }
}
