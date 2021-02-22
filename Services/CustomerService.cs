using GestRehema.Data;
using GestRehema.Entities;
using GestRehema.Extensions;
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
        List<Customer> GetCustomers(int skip = 0, int take = 100);
        List<Customer> SearchCustomers(string query);
    }

    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _dbContext;

        public CustomerService()
        {
            _dbContext = Locator.Current.GetService<AppDbContext>();
        }

        public List<Customer> SearchCustomers(string query)
            => _dbContext
            .Customers
            .Where(x => x.Name.ToLower().Contains(query.ToLower())
            || x.Id.ToString().Contains(query)
            || (!string.IsNullOrEmpty(x.NumTelephone) && x.NumTelephone.ToLower().Contains(query.ToLower())))
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
                item!.CreatedAt = DateTime.UtcNow;
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
       => _dbContext.Customers.SingleOrDefault(x => x.Id == itemId);

        public List<Customer> GetCustomers(int skip = 0, int take = 100)
            => _dbContext.Customers.Skip(skip)
            .Take(take)
            .OrderByDescending(x => x.Id)
            .ToList();
    }
}
