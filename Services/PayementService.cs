using GestRehema.Data;
using GestRehema.Entities;
using Splat;
using System;
using System.Linq;

namespace GestRehema.Services
{
    public interface IPayementService
    {
        Payement AddPayement(Payement payement);
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
    }
}
