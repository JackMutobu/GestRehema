﻿using GestRehema.Data;
using GestRehema.Entities;
using GestRehema.Extensions;
using Microsoft.EntityFrameworkCore;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestRehema.Services
{
    public record ArticleStock(int ArticleId,decimal BuyinPrice, int Quantity);

    public interface IArticleService
    {
        Article AddOrUpdateArticle(Article article);
        Article? GetArticle(int articleId);
        List<Article> GetArticles(int skip = 0, int take = 100);
        List<Article> GetArticlesWithSale(int skip = 0, int take = 100);
        Article? GetArticleWithSales(int articleId);
        int UpdateArticle(Article article);
        ArticleStock UpdateStock(ArticleStock articleStock);
        int DeleteArticle(int articleId);
        List<Article> SearchArticles(string query);
        List<string?> GetCategories();
    }

    public class ArticleService : IArticleService
    {
        private readonly AppDbContext _dbContext;
        public ArticleService()
        {
            _dbContext = Locator.Current.GetService<AppDbContext>();
        }

        public List<Article> SearchArticles(string query)
            => _dbContext
            .Articles
            .Where(x => x.Name.ToLower().Contains(query.ToLower()) 
            || x.Id.ToString().Contains(query) 
            || (!string.IsNullOrEmpty(x.TechnicalCode) && x.TechnicalCode.ToLower().Contains(query.ToLower())))
            .ToList()
            .DistinctBy(x => x.Id)
            .ToList();

        public Article AddOrUpdateArticle(Article article)
        {
            var regArticle = _dbContext.Articles.SingleOrDefault(x => x.Id == article.Id);
            if (regArticle != null)
            {
                regArticle.ImageUrl = article.ImageUrl;
                regArticle.Name = article.Name;
                regArticle.SellingPrice = article.SellingPrice;
                regArticle.TechnicalCode = article.TechnicalCode;
                regArticle.UnitOfMeasure = article.UnitOfMeasure;
                regArticle.UpdatedAt = DateTime.UtcNow;
                regArticle.InStock = article.InStock;

                _dbContext.Articles.Update(regArticle);
                _dbContext.SaveChanges();
                return _dbContext.Articles.First(x => x.UpdatedAt == regArticle.UpdatedAt);
            }
            else
            {
                article.CreatedAt = DateTime.UtcNow;
                _dbContext.Articles.Add(article);
                _dbContext.SaveChanges();
                return _dbContext.Articles.First(x => x.CreatedAt == article.CreatedAt);
            }

        }

        public int UpdateArticle(Article article)
        {
            var regArticle = _dbContext.Articles.SingleOrDefault(x => x.Id == article.Id);
            if (regArticle != null)
            {
                regArticle.ImageUrl = article.ImageUrl;
                regArticle.Name = article.Name;
                regArticle.SellingPrice = article.SellingPrice;
                regArticle.TechnicalCode = article.TechnicalCode;
                regArticle.UnitOfMeasure = article.UnitOfMeasure;

                _dbContext.Articles.Update(regArticle);
                return _dbContext.SaveChanges();
            }

            throw new Exception("Article inconnu");
        }

        public int DeleteArticle(int articleId)
        {
            var regArticle = _dbContext.Articles.SingleOrDefault(x => x.Id == articleId);
            if(regArticle != null)
            {
                _dbContext.Articles.Remove(regArticle);
                return _dbContext.SaveChanges();
            }
            throw new Exception("Article inconnu");
        }

        public ArticleStock UpdateStock(ArticleStock articleStock)
        {
            if (articleStock.Quantity == 0 || articleStock.BuyinPrice == 0)
                throw new ArgumentException("La quantité et le prix d'achat doivent etre supérieur à 0", nameof(ArticleStock));

            var article = _dbContext.Articles.SingleOrDefault(x => x.Id == articleStock.ArticleId);
            if (article != null)
            {
                var currentStockValue = article.InStock * article.BuyingPrice;
                var newStockValue = articleStock.Quantity * articleStock.BuyinPrice;

                article.BuyingPrice = (currentStockValue + newStockValue) / article.InStock + articleStock.Quantity;

                _dbContext.Articles.Update(article);
                _dbContext.SaveChanges();

                return new ArticleStock(article.Id, article.BuyingPrice, article.InStock);
            }

            throw new Exception("Article inconnu");

        }

        public Article? GetArticle(int articleId)
        => _dbContext.Articles.SingleOrDefault(x => x.Id == articleId);

        public Article? GetArticleWithSales(int articleId)
        => _dbContext.Articles.Include(x => x.Sales)
            .SingleOrDefault(x => x.Id == articleId);

        public List<Article> GetArticles(int skip = 0, int take = 100)
            => _dbContext.Articles.Skip(skip).Take(take)
            .OrderByDescending(x => x.Id)
            .ToList();

        public List<Article> GetArticlesWithSale(int skip = 0, int take = 100)
           => _dbContext.Articles.Include(x => x.Sales).Skip(skip).Take(take)
            .ToList();

        public List<string?> GetCategories()
           => _dbContext.Articles.Select(x => x.Category)
            .ToList()
            .DistinctBy(x => x)
            .ToList();
    }
}
