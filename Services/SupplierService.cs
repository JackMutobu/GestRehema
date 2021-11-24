using GestRehema.Data;
using GestRehema.Entities;
using GestRehema.Extensions;
using Microsoft.EntityFrameworkCore;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestRehema.Services
{
    public interface ISupplierService
    {
        Supplier AddOrUpdateSupplier(Supplier supplier);
        int Deletesupplier(int supplierId);
        List<Supplier> GetSuppliers(int skip = 0, int take = 100, string? location = null, string? supplierType = null);
        Supplier? GetSupplier(int itemId);
        Wallet? GetWallet(int supplierId);
        List<Supplier> SearchSupplier(string query);
        void AddArticlesToSupplier(int supplierId, List<Article?> articles);
        List<string?> GetLocations();
    }

    public class SupplierService : ISupplierService
    {
        private readonly AppDbContext _dbContext;

        public SupplierService()
        {
            _dbContext = Locator.Current.GetService<AppDbContext>();
        }

        public List<Supplier> SearchSupplier(string query)
          => _dbContext
          .Suppliers
          .Include(x => x.Wallet)
          .Include(x => x.Articles)
          .ThenInclude(x => x.Article)
          .Where(x => x.Name.ToLower().Contains(query.ToLower())
          || x.Id.ToString().Contains(query)
          || x.Name.ToLower().Contains(query.ToLower()))
          .ToList()
          .OrderByDescending(x => x.UpdatedAt)
          .ThenBy(x => x.Id)
          .DistinctBy(x => x.Id)
          .ToList();


        public List<string?> GetLocations()
         => _dbContext.Suppliers
            .Select(x => x.Adresse)
            .ToList()
            .DistinctBy(x => x)
            .Union(new List<string>
            {
                "Bunia",
                "Butembo",
                "Kampala",
                "Chine"
            })
            .ToList();

        public Supplier AddOrUpdateSupplier(Supplier supplier)
        {
            var regItem = _dbContext.Suppliers.SingleOrDefault(x => x.Id == supplier.Id);
            if (regItem != null)
            {
                regItem.ImageUrl = supplier.ImageUrl;
                regItem.Name = supplier.Name;
                regItem.Adresse = supplier.Adresse;
                regItem.NumTelephone = supplier.NumTelephone;
                regItem.Email = supplier.Email;
                regItem.UpdatedAt = DateTime.UtcNow;

                _dbContext.Suppliers.Update(regItem);
                _dbContext.SaveChanges();
                return _dbContext.Suppliers.First(x => x.Id == regItem.Id);
            }
            else
            {
                supplier.CreatedAt = DateTime.UtcNow;
                supplier.UpdatedAt = DateTime.UtcNow;
                supplier.Wallet = new Wallet()
                {
                    CreatedAt = DateTime.UtcNow,
                };
                _dbContext.Suppliers.Add(supplier);
                _dbContext.SaveChanges();
                return _dbContext.Suppliers.First(x => x.CreatedAt == supplier.CreatedAt);
            }

        }

        public void AddArticlesToSupplier(int supplierId, List<Article?> articles)
        {
            var supplier = _dbContext.Suppliers
                .AsNoTracking()
                .Include(x => x.Articles)
                .ThenInclude(x => x.Article)
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == supplierId);

            if (articles.Count > 0 && supplier != null)
            {
                var regArticles = supplier.Articles.Select(x => x.Article);

                var itemsToAdd = articles
                    .Select(x => x.Id)
                    .Except(regArticles.Select(x => x.Id))
                    .Select(x => new ArticleSupplier { ArticleId = x, SupplierId = supplierId });

                var itemsToRemove = regArticles
                    .Select(x => x.Id)
                    .Except(articles.Select(x => x.Id))
                    .Select(x => _dbContext.ArticleSuppliers.AsNoTracking().First(y => y.ArticleId == x && y.SupplierId == supplier.Id));

                foreach (var item in itemsToAdd)
                    _dbContext.ArticleSuppliers.Add(item);

                foreach (var item in itemsToRemove)
                    _dbContext.ArticleSuppliers.Remove(item);

                _dbContext.SaveChanges();
            }
        }

        public int Deletesupplier(int supplierId)
        {
            var regsupplier = _dbContext.Suppliers.SingleOrDefault(x => x.Id == supplierId);
            if (regsupplier != null)
            {
                _dbContext.Suppliers.Remove(regsupplier);
                return _dbContext.SaveChanges();
            }
            throw new Exception("Fournisseur inconnu");
        }

        public Supplier? GetSupplier(int itemId)
       => _dbContext.Suppliers
            .Include(x => x.Wallet)
            .SingleOrDefault(x => x.Id == itemId);

        public Wallet? GetWallet(int supplierId)
            => _dbContext.Suppliers
            .Include(x => x.Wallet)
            .SingleOrDefault(x => x.Id == supplierId)?
            .Wallet;

        public List<Supplier> GetSuppliers(int skip = 0, int take = 100, string? location = null, string? supplierType = null)
            => _dbContext.Suppliers
            .Include(x => x.Wallet)
            .Include(x => x.Articles)
            .ThenInclude(x => x.Article)
            .Skip(skip)
            .Take(take)
            .Where(x => (location == null || x.Adresse == location) && (supplierType == null || string.IsNullOrEmpty(x.SupplierType) || x.SupplierType == supplierType))
            .OrderByDescending(x => x.UpdatedAt)
            .ThenByDescending(x => x.Id)
            .ToList();

        
    }
}
