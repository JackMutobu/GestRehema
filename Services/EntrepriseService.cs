using GestRehema.Data;
using GestRehema.Entities;
using GestRehema.Extensions;
using Microsoft.EntityFrameworkCore;
using Splat;
using System;
using System.Linq;

namespace GestRehema.Services
{
    public interface IEntrepriseService
    {
        Entreprise GetEntreprise();
        int RegisterEmployee(Employee employee);
        Entreprise UpdateEntrepriseInfo(Entreprise entreprise);
    }

    public class EntrepriseService : IEntrepriseService
    {
        private readonly AppDbContext _dbContext;
        private readonly IUserService _userService;

        public EntrepriseService()
        {
            _dbContext = Locator.Current.GetService<AppDbContext>(); ;
            _userService = Locator.Current.GetService<IUserService>(); ;
        }

        public Entreprise GetEntreprise()
            => _dbContext.Entreprises
            .Include(x => x.Employees)
            .First();

        public Entreprise UpdateEntrepriseInfo(Entreprise entreprise)
        {
            var regEntreprise = _dbContext.Entreprises.SingleOrDefault(x => x.Id == entreprise.Id);
            if (regEntreprise != null)
            {
                var updateDate = DateTime.UtcNow;
                regEntreprise.DateDuJour = entreprise.DateDuJour;
                regEntreprise.Description = entreprise.Description;
                regEntreprise.TauxDuJour = entreprise.TauxDuJour;
                regEntreprise.UpdatedAt = updateDate;
                regEntreprise.Name = entreprise.Name;
                regEntreprise.LogoUrl = entreprise.LogoUrl;

                _dbContext.Entreprises.Update(regEntreprise);
                _dbContext.SaveChanges();

                return _dbContext.Entreprises.First(x => x.UpdatedAt == updateDate);
            }
            throw new Exception("Entreprise non retrouvé");
        }

        public int RegisterEmployee(Employee employee)
        {
            var user = _userService.Register(new User()
            {
                Role = employee.Position,
                Username = string.IsNullOrEmpty(employee.Email)
                        ? $"{employee.Prenom.Substring(0, 2)}{employee.Postnom}@rehema.com"
                        : employee.Email,
                Password = PasswordExtensions.CreatePassword(5)
            });

            if (user != null)
            {
                employee.UserId = user.Id;
                _dbContext.Employees.Add(employee);
                return _dbContext.SaveChanges();
            }
            throw new Exception("Une erreure est survenue lors de l'enregistrement de cet employé");

        }
    }
}
