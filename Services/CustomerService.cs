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
    public interface ICustomerService
    {
        Customer AddOrUpdateCustomer(Customer item);
        int DeleteCustomer(int itemId);
        Customer? GetCustomer(int itemId);
        List<Customer> GetCustomers(int skip = 0, int take = 100, string? customerType = null);
        List<Customer> SearchCustomers(string query, string? customerType = null);
        Wallet? GetWallet(int customerId);
    }

    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _dbContext;

        public CustomerService()
        {
            _dbContext = Locator.Current.GetService<AppDbContext>();
        }

        public List<Customer> SearchCustomers(string query, string? customerType = null)
            => _dbContext
            .Customers
            .Include(x => x.Wallet)
            .Where(x => x.Name.ToLower().Contains(query.ToLower())
            || x.Id.ToString().Contains(query)
            || (!string.IsNullOrEmpty(x.NumTelephone) && x.NumTelephone.ToLower().Contains(query.ToLower()))
            && (customerType == null || x.CustomerType == customerType))
            .ToList()
            .DistinctBy(x => x.Id)
            .ToList();

        public Customer AddOrUpdateCustomer(Customer item)
        {
            var regItem = _dbContext.Customers.SingleOrDefault(x => x.Id == item.Id);
            if (regItem != null)
            {
                regItem.ImageUrl = item.ImageUrl;
                regItem.Name = item.Name;
                regItem.Adresse = item.Adresse;
                regItem.NumTelephone = item.NumTelephone;
                regItem.Email = item.Email;
                regItem.UpdatedAt = DateTime.UtcNow;

                _dbContext.Customers.Update(regItem);
                _dbContext.SaveChanges();
                return _dbContext.Customers.First(x => x.UpdatedAt == regItem.UpdatedAt);
            }
            else
            {
                regItem = _dbContext.Customers.FirstOrDefault(x => x.Name.ToLower() == item.Name.ToLower() && x.NumTelephone.ToLower() == item.NumTelephone.ToLower());
                if (regItem != null)
                    throw new Exception("Un client avec le meme nom et numéro de téléphone existe déjà");

                item!.CreatedAt = DateTime.UtcNow;
                item.Wallet = new Wallet()
                {
                    CreatedAt = DateTime.UtcNow,
                };
                _dbContext.Customers.Add(item);
                _dbContext.SaveChanges();
                return _dbContext.Customers.First(x => x.CreatedAt == item.CreatedAt);
            }

        }

        public int DeleteCustomer(int itemId)
        {
            var regItem = _dbContext.Customers.SingleOrDefault(x => x.Id == itemId);
            if (regItem != null)
            {
                _dbContext.Customers.Remove(regItem);
                return _dbContext.SaveChanges();
            }
            throw new Exception("Client inconnu");
        }

        public Customer? GetCustomer(int itemId)
       => _dbContext.Customers
            .Include(x => x.Wallet)
            .SingleOrDefault(x => x.Id == itemId);

        public Wallet? GetWallet(int customerId)
            => _dbContext.Customers
            .Include(x => x.Wallet)
            .SingleOrDefault(x => x.Id == customerId)?
            .Wallet;

        public List<Customer> GetCustomers(int skip = 0, int take = 100, string? customerType = null)
            => _dbContext.Customers
            .Include(x => x.Wallet)
            .Skip(skip)
            .Take(take)
            .Where(x => (customerType == null || x.CustomerType == customerType))
            .OrderByDescending(x => x.Id)
            .ToList();
    }
}
