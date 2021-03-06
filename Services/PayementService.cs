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
        List<Payement> GetPayements(int walletId, int customerId);
        List<Payement> GetSalePayements(int customerId);
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
                .Where(x => x != null)
                .Include(x => x.Payement)
                .ThenInclude(x => x!.SalePayement)
                .ThenInclude(x => x!.Sale)
                .Where(x => x.AmountDebtWalletId == walletId || x.AmountExcessWalletId == walletId)
                .Select(x => x.Payement)
                .ToList();



            return finalPayement ?? new List<Payement>();
        }

        public List<Payement> GetPayements(int walletId, int customerId)
        => GetPayements(walletId)
            .Union(GetSalePayements(customerId))
            .OrderByDescending(x => x.UpdatedAt)
            .ThenBy(x => x.CreatedAt)
            .ToList();

        public List<Payement> GetSalePayements(int customerId)
        {
            var finalPayement = _dbContext.Sales
                .Where(x => x.CustomerId == customerId)
                .Include(x => x.PayementHistory)
                .ThenInclude(x => x.Payement)
                .OrderByDescending(x => x.UpdatedAt)
                .SelectMany(x => x.PayementHistory)
                .Select(x => x.Payement)
                .ToList();



            return finalPayement ?? new List<Payement>();
        }
    }

}
