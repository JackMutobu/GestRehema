using GestRehema.Entities;
using GestRehema.Extensions;
using Microsoft.EntityFrameworkCore;
using System;

namespace GestRehema.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Entreprise> Entreprises { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Employee> Employees { get; set; } = null!;

        public DbSet<Article> Articles { get; set; } = null!;

        public DbSet<Customer> Customers { get; set; } = null!;

        public DbSet<Sale> Sales { get; set; } = null!;

        public DbSet<SaleArticle> SaleArticles { get; set; } = null!;

        public DbSet<SalePayement> SalePayements { get; set; } = null!;

        public DbSet<SaleDelivery> SaleDeliveries { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(x => x.Username)
                .IsUnique();


            modelBuilder.Entity<Entreprise>()
                .HasData(new Entreprise
                {
                    Id = -1,
                    Name = "Ets Rehema",
                    CreatedAt = DateTime.UtcNow,
                    Description = "Votre besoin en construction est assuré",
                    TauxDuJour = 2000
                });
            var userId = Guid.NewGuid();
            modelBuilder.Entity<Employee>()
                .HasData(new Employee
                {
                    Nom = "Admin",
                    Postnom = "Informatique",
                    Prenom = "Rehema",
                    Position = "Admin",
                    EntrepriseId = -1,
                    Id = -1,
                    UserId = userId,
                    NumTelephone = "09786408335"
                });

            modelBuilder.Entity<User>()
                .HasData(new User
                {
                    Id = userId,
                    CreatedAt = DateTime.UtcNow,
                    AccessLevel = "all",
                    Role = UserRoles.SuperAdmin,
                    Username = "admin@rehema.com",
                    Password = "aDmin@2021".Hash(),
                });

            modelBuilder.Entity<SaleArticle>()
       .HasKey(sa => new { sa.ArticleId, sa.SaleId});
            modelBuilder.Entity<SaleArticle>()
                .HasOne(sa => sa.Sale)
                .WithMany(s => s.ArticleSold)
                .HasForeignKey(sa => sa.SaleId);
            modelBuilder.Entity<SaleArticle>()
                .HasOne(asa => asa.Article)
                .WithMany(a => a.Sales)
                .HasForeignKey(asa => asa.ArticleId);

            modelBuilder.Entity<Customer>()
        .HasOne(isAn => isAn.Organization)
        .WithOne(c => c.Customer!)
        .HasForeignKey<Organization>(b => b.CustomerId);

            modelBuilder.Entity<Customer>()
        .HasOne(isA => isA.Store)
        .WithOne(c => c.Customer!)
        .HasForeignKey<Store>(b => b.CustomerId);

        modelBuilder
        .Entity<Employee>()
        .HasOne(e => e.User)
        .WithOne(e => e.Employee!)
        .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }

    }
}
