using GestRehema.Data;
using Splat;

namespace GestRehema.Services
{
    public class SupplyService
    {
        private readonly AppDbContext _dbContext;

        public SupplyService()
        {
            _dbContext = Locator.Current.GetService<AppDbContext>();

        }

       
    }
}
