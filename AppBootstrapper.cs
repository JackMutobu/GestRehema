using GestRehema.Data;
using GestRehema.Services;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using Splat;
using System.Reflection;

namespace GestRehema
{
    public class AppBootstrapper
    {
        public AppBootstrapper(string connectionString)
        {
            var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            Locator.CurrentMutable.Register(() => new AppDbContext(contextOptions));

            Locator.CurrentMutable.Register<IUserService>(() => new UserService());
            Locator.CurrentMutable.Register<IEntrepriseService>(() => new EntrepriseService());
            Locator.CurrentMutable.Register<IFileService>(() => new FileService());
            Locator.CurrentMutable.Register<IArticleService>(() => new ArticleService());
            Locator.CurrentMutable.Register<ICustomerService>(() => new CustomerService());
            Locator.CurrentMutable.Register<ISaleService>(() => new SaleService());
            Locator.CurrentMutable.Register<IWalletService>(() => new WalletService());

            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetExecutingAssembly());
        }
    }
}
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }//Prevent C#9 errors
}
