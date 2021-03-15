﻿// <auto-generated />
using System;
using GestRehema.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GestRehema.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210311123403_UpdateWalletEntity")]
    partial class UpdateWalletEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GestRehema.Entities.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("AwaitingDeliveryToCompany")
                        .HasColumnType("float");

                    b.Property<double>("AwaitingDeliveryToCustomers")
                        .HasColumnType("float");

                    b.Property<decimal>("BuyingPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Conditionement")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("InStock")
                        .HasColumnType("float");

                    b.Property<DateTime?>("LastAwaitingDeliveryToCompanyUpdate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastAwaitingDeliveryToCustomersUpdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("QtyPerConditionement")
                        .HasColumnType("float");

                    b.Property<decimal>("SellingPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TechnicalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UnitOfMeasure")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("GestRehema.Entities.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Adresse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustomerType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumTelephone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("WalletId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WalletId")
                        .IsUnique();

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("GestRehema.Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Adresse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EntrepriseId")
                        .HasColumnType("int");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumTelephone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Postnom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Prenom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfileUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("EntrepriseId");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = -1,
                            CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            EntrepriseId = -1,
                            Nom = "Admin",
                            NumTelephone = "09786408335",
                            Position = "Admin",
                            Postnom = "Informatique",
                            Prenom = "Rehema",
                            UserId = new Guid("ac6bd2fd-c45b-4523-9ca0-433eaf0d5ed3")
                        });
                });

            modelBuilder.Entity("GestRehema.Entities.Entreprise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateDuJour")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IDNAT")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PoBox")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RCCM")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Slogan")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TauxDuJour")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("WalletId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WalletId")
                        .IsUnique();

                    b.ToTable("Entreprises");

                    b.HasData(
                        new
                        {
                            Id = -1,
                            Contact = "+243971871546\r\n+243822903906\r\n+243819521649",
                            CreatedAt = new DateTime(2021, 3, 11, 12, 34, 0, 968, DateTimeKind.Utc).AddTicks(9174),
                            DateDuJour = new DateTime(2021, 3, 11, 12, 34, 0, 968, DateTimeKind.Utc).AddTicks(6637),
                            Description = "Votre besoin en construction est assuré",
                            IDNAT = "493-N50888J",
                            Location = "Bunia",
                            Name = "Ets Rehema",
                            PoBox = "76",
                            RCCM = "BIA/RCCM/19-A-1320265",
                            Slogan = "Chez FLORENCE",
                            TauxDuJour = 2000m,
                            WalletId = -1
                        });
                });

            modelBuilder.Entity("GestRehema.Entities.Expense", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EmpleyeeId")
                        .HasColumnType("int");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("Owner")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PayementId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("PayementId")
                        .IsUnique();

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("GestRehema.Entities.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.ToTable("Organization");
                });

            modelBuilder.Entity("GestRehema.Entities.Payement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("AmountInCDF")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("AmountInUSD")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PayementOrganization")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ToCompany")
                        .HasColumnType("bit");

                    b.Property<decimal>("TotalPaid")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TransactionId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Payements");
                });

            modelBuilder.Entity("GestRehema.Entities.Sale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOperation")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeliveryStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("PayementStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SellerId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Sales");
                });

            modelBuilder.Entity("GestRehema.Entities.SaleArticle", b =>
                {
                    b.Property<int>("ArticleId")
                        .HasColumnType("int");

                    b.Property<int>("SaleId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.Property<decimal>("UnitBuyingPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("UnitRealSellingPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("UnitSellingPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ArticleId", "SaleId");

                    b.HasIndex("SaleId");

                    b.ToTable("SaleArticles");
                });

            modelBuilder.Entity("GestRehema.Entities.SaleDelivery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ArticleId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("DeliveredQuantity")
                        .HasColumnType("float");

                    b.Property<int>("SaleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.HasIndex("SaleId");

                    b.ToTable("SaleDeliveries");
                });

            modelBuilder.Entity("GestRehema.Entities.SalePayement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("AmountPaid")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("PayementId")
                        .HasColumnType("int");

                    b.Property<int>("SaleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PayementId")
                        .IsUnique();

                    b.HasIndex("SaleId");

                    b.ToTable("SalePayements");
                });

            modelBuilder.Entity("GestRehema.Entities.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.ToTable("Store");
                });

            modelBuilder.Entity("GestRehema.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccessLevel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ac6bd2fd-c45b-4523-9ca0-433eaf0d5ed3"),
                            AccessLevel = "all",
                            CreatedAt = new DateTime(2021, 3, 11, 12, 34, 0, 970, DateTimeKind.Utc).AddTicks(9385),
                            Password = "10000.WcJqw1ohRrDAgUg+8okdiw==.StHvxejP/BLTjKmBrDAXFCSVb34EL1lic8hRrT1jCTI=",
                            Role = "SuperAdmin",
                            Username = "admin@rehema.com"
                        });
                });

            modelBuilder.Entity("GestRehema.Entities.Wallet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("AmountInDebt")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("AmountInExcess")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("AmountOwned")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Wallets");

                    b.HasData(
                        new
                        {
                            Id = -1,
                            AmountInDebt = 0m,
                            AmountInExcess = 0m,
                            AmountOwned = 0m,
                            CreatedAt = new DateTime(2021, 3, 11, 12, 34, 0, 966, DateTimeKind.Utc).AddTicks(6150)
                        });
                });

            modelBuilder.Entity("GestRehema.Entities.WalletHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("AmountDebtWalletId")
                        .HasColumnType("int");

                    b.Property<int?>("AmountExcessWalletId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PayementId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AmountDebtWalletId");

                    b.HasIndex("AmountExcessWalletId");

                    b.HasIndex("PayementId");

                    b.ToTable("WalletHistories");
                });

            modelBuilder.Entity("GestRehema.Entities.Customer", b =>
                {
                    b.HasOne("GestRehema.Entities.Wallet", "Wallet")
                        .WithOne("Customer")
                        .HasForeignKey("GestRehema.Entities.Customer", "WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("GestRehema.Entities.Employee", b =>
                {
                    b.HasOne("GestRehema.Entities.Entreprise", "Entreprise")
                        .WithMany("Employees")
                        .HasForeignKey("EntrepriseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestRehema.Entities.User", "User")
                        .WithOne("Employee")
                        .HasForeignKey("GestRehema.Entities.Employee", "UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Entreprise");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GestRehema.Entities.Entreprise", b =>
                {
                    b.HasOne("GestRehema.Entities.Wallet", "Wallet")
                        .WithOne("Entreprise")
                        .HasForeignKey("GestRehema.Entities.Entreprise", "WalletId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Wallet");
                });

            modelBuilder.Entity("GestRehema.Entities.Expense", b =>
                {
                    b.HasOne("GestRehema.Entities.Employee", "Employee")
                        .WithMany("Expenses")
                        .HasForeignKey("EmployeeId");

                    b.HasOne("GestRehema.Entities.Payement", "Payement")
                        .WithOne("Expense")
                        .HasForeignKey("GestRehema.Entities.Expense", "PayementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Payement");
                });

            modelBuilder.Entity("GestRehema.Entities.Organization", b =>
                {
                    b.HasOne("GestRehema.Entities.Customer", "Customer")
                        .WithOne("Organization")
                        .HasForeignKey("GestRehema.Entities.Organization", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("GestRehema.Entities.Sale", b =>
                {
                    b.HasOne("GestRehema.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestRehema.Entities.Employee", null)
                        .WithMany("Sales")
                        .HasForeignKey("EmployeeId");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("GestRehema.Entities.SaleArticle", b =>
                {
                    b.HasOne("GestRehema.Entities.Article", "Article")
                        .WithMany("Sales")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestRehema.Entities.Sale", "Sale")
                        .WithMany("ArticleSold")
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("Sale");
                });

            modelBuilder.Entity("GestRehema.Entities.SaleDelivery", b =>
                {
                    b.HasOne("GestRehema.Entities.Article", "Article")
                        .WithMany()
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestRehema.Entities.Sale", "Sale")
                        .WithMany("DeliveryHistory")
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("Sale");
                });

            modelBuilder.Entity("GestRehema.Entities.SalePayement", b =>
                {
                    b.HasOne("GestRehema.Entities.Payement", "Payement")
                        .WithOne("SalePayement")
                        .HasForeignKey("GestRehema.Entities.SalePayement", "PayementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestRehema.Entities.Sale", "Sale")
                        .WithMany("PayementHistory")
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Payement");

                    b.Navigation("Sale");
                });

            modelBuilder.Entity("GestRehema.Entities.Store", b =>
                {
                    b.HasOne("GestRehema.Entities.Customer", "Customer")
                        .WithOne("Store")
                        .HasForeignKey("GestRehema.Entities.Store", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("GestRehema.Entities.WalletHistory", b =>
                {
                    b.HasOne("GestRehema.Entities.Wallet", "AmountDebtWallet")
                        .WithMany()
                        .HasForeignKey("AmountDebtWalletId");

                    b.HasOne("GestRehema.Entities.Wallet", "AmountExcessWallet")
                        .WithMany()
                        .HasForeignKey("AmountExcessWalletId");

                    b.HasOne("GestRehema.Entities.Payement", "Payement")
                        .WithMany("WalletHistory")
                        .HasForeignKey("PayementId");

                    b.Navigation("AmountDebtWallet");

                    b.Navigation("AmountExcessWallet");

                    b.Navigation("Payement");
                });

            modelBuilder.Entity("GestRehema.Entities.Article", b =>
                {
                    b.Navigation("Sales");
                });

            modelBuilder.Entity("GestRehema.Entities.Customer", b =>
                {
                    b.Navigation("Organization");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("GestRehema.Entities.Employee", b =>
                {
                    b.Navigation("Expenses");

                    b.Navigation("Sales");
                });

            modelBuilder.Entity("GestRehema.Entities.Entreprise", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("GestRehema.Entities.Payement", b =>
                {
                    b.Navigation("Expense");

                    b.Navigation("SalePayement");

                    b.Navigation("WalletHistory");
                });

            modelBuilder.Entity("GestRehema.Entities.Sale", b =>
                {
                    b.Navigation("ArticleSold");

                    b.Navigation("DeliveryHistory");

                    b.Navigation("PayementHistory");
                });

            modelBuilder.Entity("GestRehema.Entities.User", b =>
                {
                    b.Navigation("Employee");
                });

            modelBuilder.Entity("GestRehema.Entities.Wallet", b =>
                {
                    b.Navigation("Customer");

                    b.Navigation("Entreprise");
                });
#pragma warning restore 612, 618
        }
    }
}
