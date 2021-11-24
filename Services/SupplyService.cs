using GestRehema.Data;
using GestRehema.Entities;
using Microsoft.EntityFrameworkCore;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestRehema.Services
{
    public interface ISupplyService
    {
        SupplyPayement AddPayement(SupplyPayement supplyPayement);
        Supply AddSupply(Supply supply);
        SupplyItem AddSupplyItem(SupplyItem supplyItem);
        List<Supply> GetSupplies(int skip = 0, int take = 100, string? searchQuery = null);
        List<SupplyItem> GetSupplyItems(int supplyId);
    }

    public class SupplyService : ISupplyService
    {
        private readonly AppDbContext _dbContext;

        public SupplyService()
        {
            _dbContext = Locator.Current.GetService<AppDbContext>();

        }

        public Supply AddSupply(Supply supply)
        {
            var regSupply = _dbContext.Supplies.FirstOrDefault(x => x.Id == supply.Id);
            if (regSupply == null)
            {
                supply.CreatedAt = DateTime.UtcNow;
                supply.UpdatedAt = DateTime.UtcNow;
                _dbContext.Supplies.Add(supply);
                _dbContext.SaveChanges();
                return _dbContext.Supplies.First(x => x.CreatedAt == supply.CreatedAt);
            }

            throw new Exception("Supply already exist");
        }

        public SupplyItem AddSupplyItem(SupplyItem supplyItem)
        {
            var regSupplyItem = _dbContext.SupplyItems.FirstOrDefault(x => x.Id == supplyItem.Id);
            if (regSupplyItem == null)
            {
                supplyItem.CreatedAt = DateTime.UtcNow;
                supplyItem.UpdatedAt = DateTime.UtcNow;
                _dbContext.SupplyItems.Add(supplyItem);
                _dbContext.SaveChanges();
                return _dbContext.SupplyItems.First(x => x.CreatedAt == supplyItem.CreatedAt);
            }
            throw new Exception("SupplyItem already registered");
        }

        public SupplyPayement AddPayement(SupplyPayement supplyPayement)
        {
            if (supplyPayement.SupplyItemId <= 0)
                throw new ArgumentException(nameof(supplyPayement.SupplyItemId), "SupplyItemId is not correct");

            supplyPayement.Date = DateTime.UtcNow;
            _dbContext.SupplyPayements.Add(supplyPayement);
            _dbContext.SaveChanges();

            return _dbContext.SupplyPayements.First(x => x.Date == supplyPayement.Date);
        }

        public List<Supply> GetSupplies(int skip = 0, int take = 100, string? searchQuery = null)
         => _dbContext
            .Supplies
            .Include(x => x.SupplyItems)
            .ThenInclude(x => x.Supplier)
            .Where(x => (string.IsNullOrEmpty(searchQuery)
            || x.DeliveryStatus == searchQuery
            || x.DeliveryStatus == searchQuery
            || x.SupplyItems.Any(s => s.Supplier!.Name.Contains(searchQuery) || s.Supplier.NumTelephone.Contains(searchQuery))))
            .Skip(skip)
            .Take(take)
            .OrderByDescending(x => x.DateOperation)
            .ToList();

        public List<SupplyItem> GetSupplyItems(int supplyId)
            => _dbContext
            .SupplyItems
            .Include(x => x.Supplier)
            .Include(x => x.ArticlesSupplied)
            .Include(x => x.DeliveryHistory)
            .Include(x => x.PayementHistory)
            .Where(x => x.SupplyId == supplyId)
            .ToList();

    }
}
