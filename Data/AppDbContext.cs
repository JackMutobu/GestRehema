﻿using GestRehema.Entities;
using GestRehema.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

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

        public DbSet<Wallet> Wallets { get; set; } = null!;

        public DbSet<WalletHistory> WalletHistories { get; set; } = null!;

        public DbSet<Payement> Payements { get; set; } = null!;

        public DbSet<Expense> Expenses { get; set; } = null!;

        public DbSet<Supplier> Suppliers { get; set; } = null!;

        public DbSet<Supply> Supplies { get; set; } = null!;

        public DbSet<SupplyItem> SupplyItems { get; set; } = null!;

        public DbSet<SupplyArticle> SupplyArticles { get; set; } = null!;

        public DbSet<SupplyDelivery> SupplyDeliveries { get; set; } = null!;

        public DbSet<SupplyPayement> SupplyPayements { get; set; } = null!;

        public DbSet<SupplyExpense> SupplyExpenses { get; set; } = null!;

        public DbSet<ArticleSupplier> ArticleSuppliers { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasIndex(x => x.Username)
                .IsUnique();

            modelBuilder.Entity<Wallet>()
               .HasData(new Wallet
               {
                   Id = -1,
                   CreatedAt = DateTime.UtcNow
               });

            modelBuilder.Entity<Entreprise>()
                .HasData(new Entreprise
                {
                    Id = -1,
                    Name = "Ets Rehema",
                    CreatedAt = DateTime.UtcNow,
                    Description = "Votre besoin en construction est assuré",
                    TauxDuJour = 2000,
                    WalletId = -1,
                    Slogan = "Chez FLORENCE",
                    RCCM = "BIA/RCCM/19-A-1320265",
                    IDNAT = "493-N50888J",
                    PoBox = "76",
                    Contact = $"+243971871546{Environment.NewLine}+243822903906{Environment.NewLine}+243819521649",
                    Location = "Bunia"
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
        .WithOne(c => c!.Customer!)
        .HasForeignKey<Organization>(b => b.CustomerId);

            modelBuilder.Entity<Customer>()
        .HasOne(isA => isA.Store)
        .WithOne(c => c!.Customer!)
        .HasForeignKey<Store>(b => b.CustomerId);

        modelBuilder
        .Entity<Employee>()
        .HasOne(e => e.User)
        .WithOne(e => e!.Employee!)
        .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<SupplyArticle>()
          .HasKey(sa => new {
              sa.ArticleId,
              sa.SupplyItemId
          });

            modelBuilder.Entity<ArticleSupplier>()
         .HasKey(sa => new {
             sa.ArticleId,
             sa.SupplierId
         });
            modelBuilder.Entity<SupplyArticle>()
                .HasOne(sa => sa.SupplyItem)
                .WithMany(s => s.ArticlesSupplied)
                .HasForeignKey(sa => sa.SupplyItemId);
            modelBuilder.Entity<SaleArticle>()
                        .HasOne(asa => asa.Article)
                        .WithMany(a => a.Sales)
                        .HasForeignKey(asa => asa.ArticleId);

            modelBuilder.Entity<SupplyItem>()
                       .HasOne(asa => asa.Supplier)
                       .WithMany(a => a.Supplies)
                       .HasForeignKey(asa => asa.SupplierId)
                       .OnDelete(DeleteBehavior.NoAction);



            base.OnModelCreating(modelBuilder);
        }

       

}
}
