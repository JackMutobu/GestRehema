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
    public interface ISaleService
    {
        Sale AddPayement(SalePayement salePayement, int saleId);
        Sale AddSale(Sale sale);
        Sale? GetSale(int saleId);
        List<Sale> GetSalesWithCustomers(int skip = 0, int take = 100);
        List<Sale> SearchSales(string query);
        Sale AddDelivery(SaleDelivery saleDelivery, int saleId);
        Sale DeliverAll(int saleId);
    }

    public class SaleService : ISaleService
    {
        private readonly AppDbContext _dbContext;
        private readonly IArticleService _articleService;

        public SaleService()
        {
            _dbContext = Locator.Current.GetService<AppDbContext>();
            _articleService = Locator.Current.GetService<IArticleService>();
        }

        public List<Sale> SearchSales(string query)
            => _dbContext
            .Sales
            .Include(x => x.Customer)
            .Where(x => x.Id.ToString().Contains(query.ToLower())
            || x.Customer!.Name.ToLower().Contains(query))
            .OrderByDescending(x => x.UpdatedAt)
            .ToList()
            .DistinctBy(x => x.Id)
            .ToList();

        public Sale? GetSale(int saleId)
            => _dbContext.Sales
            .Include(x => x.Customer)
            .Include(x => x.DeliveryHistory)
            .Include(x => x.PayementHistory)
            .Include(x => x.ArticleSold)
            .ThenInclude(x => x.Article)
            .SingleOrDefault(x => x.Id == saleId);

        public List<Sale> GetSalesWithCustomers(int skip = 0, int take = 100)
           => _dbContext.Sales
            .Include(x => x.Customer)
            .Skip(skip)
            .Take(take)
           .OrderByDescending(x => x.UpdatedAt)
           .ToList();

        public List<Sale> GetSalesWithPayements(int skip = 0, int take = 100)
           => _dbContext.Sales
            .Include(x => x.PayementHistory)
            .ThenInclude(x => x.Payement)
            .Skip(skip)
            .Take(take)
            .OrderByDescending(x => x.UpdatedAt)
            .ToList();

        public Sale AddSale(Sale sale)
        {
            if (sale == null)
                throw new ArgumentException("Veuillez spécifier une vente");
            if (sale.CustomerId < 0)
                throw new ArgumentException("Veuillez spécifier le client de la vente");
            if (sale.ArticleSold.Count == 0)
                throw new ArgumentException("Veuillez ajouter les articles au panier");

            foreach(var item in sale.ArticleSold)
                item.Sale = null;

            sale.CreatedAt = DateTime.UtcNow;
            sale.UpdatedAt = DateTime.UtcNow;
            _dbContext.Sales.Add(sale);
            _dbContext.SaveChanges();
            return _dbContext.Sales.Single(x => x.CreatedAt == sale.CreatedAt);
        }

        public Sale AddPayement(SalePayement salePayement, int saleId)
        {
            var sale = _dbContext.Sales
                .Include(x => x.PayementHistory)
                .Include(x => x.ArticleSold)
                .Single(x => x.Id == saleId);
            if (salePayement.AmountPaid < 0)
                throw new ArgumentException("Le montant payé doit etre supérieur à 0");

            sale.PayementStatus = GetPayementStatus(salePayement, sale);
            sale.UpdatedAt = DateTime.UtcNow;

            salePayement.SaleId = saleId;
            _dbContext.SalePayements.Add(salePayement);
            _dbContext.Sales.Update(sale);
            _dbContext.SaveChanges();
            return _dbContext.Sales
                .Include(x => x.PayementHistory)
                .ThenInclude(x => x.Payement)
                .Single(x => x.Id == saleId);
        }

       public Sale AddDelivery(SaleDelivery saleDelivery,int saleId)
        {
            var sale = _dbContext.Sales
                .Include(x => x.DeliveryHistory)
                .Include(x => x.ArticleSold)
                .Single(x => x.Id == saleId);

            if (saleDelivery.DeliveredQuantity < 0)
                throw new ArgumentException("Le quantité livré doit etre supérieur à 0");

            sale.DeliveryStatus = GetDeliverytStatus(saleDelivery, sale);
            sale.UpdatedAt = DateTime.UtcNow;

            saleDelivery.SaleId = saleId;
            _dbContext.SaleDeliveries.Add(saleDelivery);
            _dbContext.Sales.Update(sale);
            _articleService.ReduceStock(saleDelivery.DeliveredQuantity, saleDelivery.ArticleId);
            _dbContext.SaveChanges();
            return _dbContext.Sales
                .Include(x => x.DeliveryHistory)
                .Single(x => x.Id == saleId);
        }

        public Sale DeliverAll(int saleId)
        {
            var sale =  _dbContext.Sales
                        .Include(x => x.DeliveryHistory)
                        .Include(x => x.ArticleSold)
                        .Single(x => x.Id == saleId);

            foreach(var item in sale.ArticleSold)
            {
                var deliveredQty = sale.DeliveryHistory.Where(x => x.ArticleId == item.ArticleId).Sum(x => x.DeliveredQuantity);
                var remainingQty = item.Quantity - deliveredQty;
                if(remainingQty > 0)
                {
                    var saleDelivery = new SaleDelivery
                    {
                        Date = DateTime.UtcNow,
                        DeliveredQuantity = remainingQty,
                        ArticleId = item.ArticleId,
                        SaleId = sale.Id
                    };

                    _articleService.ReduceStock(saleDelivery.DeliveredQuantity, saleDelivery.ArticleId);
                    _dbContext.SaleDeliveries.Add(saleDelivery);
                }
            }

            sale.DeliveryStatus = SaleDeliveryStatus.Delivered;
            sale.UpdatedAt = DateTime.UtcNow;
            _dbContext.Sales.Update(sale);
            _dbContext.SaveChanges();
            return _dbContext.Sales
                .Include(x => x.DeliveryHistory)
                .Single(x => x.Id == saleId);
        }

        private  string GetPayementStatus(SalePayement salePayement, Sale sale)
        {
            var totalAmount = decimal.Round(sale.ArticleSold.Sum(x => (decimal)x.Quantity * x.UnitSellingPrice),2, MidpointRounding.AwayFromZero);
            var totalPaid = decimal.Round(sale.PayementHistory.Sum(x => x.AmountPaid),2, MidpointRounding.AwayFromZero);

            if (totalAmount == totalPaid)
                throw new InvalidOperationException("Cette vente a déjà été entièrement payé");

            var payementBalance = totalAmount - (totalPaid + salePayement.AmountPaid);

            if (payementBalance < 0)
                throw new InvalidOperationException("Le montant de ce paiement est supérieur au montant restant");

            if (payementBalance == 0)
                return SalePayementStatus.Paid;
            else if (payementBalance == totalAmount)
                return SalePayementStatus.AwaitingPayement;
            else
                return SalePayementStatus.InPayement;
        }

        private string GetDeliverytStatus(SaleDelivery saleDelivery, Sale sale)
        {
            var totalQty = Math.Round(sale.ArticleSold.Sum(x => x.Quantity),5, MidpointRounding.AwayFromZero);
            var totalDelivered = Math.Round(sale.DeliveryHistory.Sum(x => x.DeliveredQuantity),5, MidpointRounding.AwayFromZero);
            var newDelivery = Math.Round(saleDelivery.DeliveredQuantity, 5, MidpointRounding.AwayFromZero);

            if (totalQty == totalDelivered)
                throw new InvalidOperationException("Ce produit a déjà été entièrement livré");

            var remainingQty = Math.Round(totalQty - (totalDelivered + newDelivery),5, MidpointRounding.ToEven);

            if(remainingQty < 0)
                throw new InvalidOperationException("La quantité livré est supérieur à la quantité en attente");

            if (remainingQty == 0)
                return SaleDeliveryStatus.Delivered;
            else if (remainingQty == totalQty)
                return SaleDeliveryStatus.AwaitingDelivery;
            else
                return SaleDeliveryStatus.InDelivery;
        }
    }
}
