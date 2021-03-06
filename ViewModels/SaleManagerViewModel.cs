using DynamicData;
using DynamicData.Binding;
using FluentValidation;
using GestRehema.Entities;
using GestRehema.Services;
using GestRehema.Validations;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace GestRehema.ViewModels
{
    public class SaleManagerViewModel : BaseSaleViewModel
    {
        private SourceList<Customer> _customers { get; } = new SourceList<Customer>();
        private SourceList<Article> _articles { get; } = new SourceList<Article>();
        private readonly IObservableCollection<Customer> _targetCollectionCustomers = new ObservableCollectionExtended<Customer>();
        private readonly IObservableCollection<Article> _targetCollectionArticles = new ObservableCollectionExtended<Article>();
        private SourceList<SaleCartItem> _cartItems { get; } = new SourceList<SaleCartItem>();
        private readonly IObservableCollection<SaleCartItem> _targetCollectionCartItems = new ObservableCollectionExtended<SaleCartItem>();

        private readonly IArticleService _articleService;
        private readonly ICustomerService _customerService;

        public SaleManagerViewModel(SaleViewModel saleViewModel) : base(new SaleValidation())
        {
            _articleService = Locator.Current.GetService<IArticleService>();
            _customerService = Locator.Current.GetService<ICustomerService>();
            SelectedCustomer = new Customer();
            Sale = new Sale();
            SaleViewModel = saleViewModel;

            _customers.Connect()
            .ObserveOnDispatcher()
            .Bind(_targetCollectionCustomers)
            .Subscribe();

            _articles.Connect()
            .ObserveOnDispatcher()
            .Bind(_targetCollectionArticles)
            .Subscribe();

            _cartItems.Connect()
            .ObserveOnDispatcher()
            .Bind(_targetCollectionCartItems)
            .Subscribe();

            LoadArticles = ReactiveCommand.CreateFromTask<LoadParameter, List<Article>>(p => 
            {
                CurrentArticlePage = p.Skip;
                ArticleItemsPerPage = p.Take;
                SearchArticleQuery = p.SearchQuery ?? "";

                return Task.Run(() => LoadingArticles(p));
            });
            LoadArticles
                .ThrownExceptions
                .Select(x => x.Message)
                .Subscribe(x => Errors = x);
            LoadArticles
                .IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);
            LoadArticles
                .Subscribe(articles =>
                {
                    _articles.Clear();
                    _articles.AddRange(articles);
                });

            LoadCustomers = ReactiveCommand.CreateFromTask<LoadParameter, List<Customer>>(p => 
            {
                CurrentCustomerPage = p.Skip;
                CustomerItemsPerPage = p.Take;
                SearchCustomerQuery = p.SearchQuery ?? "";

                return Task.Run(() => LoadingCustomers(p));
            });
            LoadCustomers
                .ThrownExceptions
                .Select(x => x.Message)
                .Subscribe(x => Errors = x);
            LoadCustomers
                .IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);
            LoadCustomers
                .Subscribe(items =>
                {
                    _customers.Clear();
                    _customers.AddRange(items);
                });

            LoadArticles
                .Execute(new LoadParameter(SearchArticleQuery, CurrentArticlePage, ArticleItemsPerPage))
                .Subscribe();
            LoadCustomers
                .Execute(new LoadParameter(SearchCustomerQuery, CurrentCustomerPage, CustomerItemsPerPage))
                .Subscribe();

            CalculateCartSubTotal = ReactiveCommand.CreateFromTask<List<SaleCartItem>, decimal>(items => Task.Run(()  => items.Sum(x => x.Total)));
            CalculateCartSubTotal
                .ToPropertyEx(this, x => x.CartSubTotal);

            AddToCart = ReactiveCommand.CreateFromTask<int, List<SaleCartItem>>(id => Task.Run(() =>
             {
                 var cartItems = _cartItems.Items.ToList();
                 var regCartItem = cartItems.SingleOrDefault(x => x.Article.Id == id);
                 if (regCartItem == null)
                     cartItems.Add(new SaleCartItem(Articles.Single(x => x.Id == id),this));
                 else
                 {
                     var indexOf = cartItems.IndexOf(regCartItem);
                     regCartItem.QtyInConditionement += 1;
                     cartItems.RemoveAt(indexOf);
                     cartItems.Insert(indexOf, regCartItem);
                 }

                 return cartItems;
             }));
            AddToCart
                .Subscribe(x =>
                {
                    _cartItems.Clear();
                    _cartItems.AddRange(x);
                });
            AddToCart
                .ThrownExceptions
                .Select(x => x.Message)
                .Subscribe(x => Errors = x);
            AddToCart
                .InvokeCommand(CalculateCartSubTotal);

            RemoveFromCart = ReactiveCommand.CreateFromTask<int, List<SaleCartItem>>(id => Task.Run(() => 
            {
                var cartItems = _cartItems.Items.ToList();
                var regCartItem = cartItems.SingleOrDefault(x => x.Article.Id == id);
                if (regCartItem != null)
                    cartItems.Remove(regCartItem);

                return cartItems;
            }));
            RemoveFromCart
               .Subscribe(x =>
               {
                   _cartItems.Clear();
                   _cartItems.AddRange(x);
               });
            RemoveFromCart
                .ThrownExceptions
                .Select(x => x.Message)
                .Subscribe(x => Errors = x);
            RemoveFromCart
                .InvokeCommand(CalculateCartSubTotal);


            Validate = ReactiveCommand.CreateFromTask<Sale, string>(
                sale => Task.Run(() => validator.Validate(new ValidationContext<Sale>(sale)).ToString()));
            Validate
                .ThrownExceptions
                .Select(x => x.Message)
                .Subscribe(x => Errors = x);
            Validate
                .IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);
            Validate
                .Subscribe(errors => Errors = errors);

            AddPayement = ReactiveCommand.Create(() => new SalePayementModel(Sale, Entreprise, SelectedCustomer.Wallet ?? throw new Exception("Wallet can not be null"), CartSubTotal));

            Pay = ReactiveCommand.CreateFromTask<List<Payement>, Sale>(x => Task.Run(() =>
            {
                Sale sale = UpdateOrAddSale(x);
                return sale;
            }));
            Pay
                .ThrownExceptions
                .Select(x => x.Message)
                .Subscribe(x => Errors = x);
            Pay
                .IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);

            AddPayement
                .ToPropertyEx(this, x => x.PayementModel);

            AddPayement
                .SelectMany(x =>
                x.Pay)
                .Where(x => x.Count > 0)
                .InvokeCommand(Pay);

            Validate
                .Where(x => string.IsNullOrEmpty(x))
                .Select(_ => Unit.Default)
                .InvokeCommand(AddPayement);

            Charge = ReactiveCommand.CreateFromTask<Unit, Sale>(_ => Task.Run(() =>
             {
                 var sale = new Sale
                 {
                     CustomerId = SelectedCustomer?.Id ?? throw new Exception("Veuillez selectionner un client"),
                     SellerId = Employee.Id,
                     DateOperation = Entreprise.DateDuJour
                 };

                 foreach(var item in CartItems)
                 {
                     sale.ArticleSold.Add(new SaleArticle
                     {
                         Date = Entreprise.DateDuJour,
                         ArticleId = item.Article.Id,
                         Quantity = item.QtyInConditionement,
                         UnitBuyingPrice = item.Article.BuyingPrice,
                         UnitRealSellingPrice = item.Article.SellingPrice,
                         UnitSellingPrice = item.SellingPrice == null ? item.Article.SellingPrice : item.SellingPrice.Value
                     });
                 }

                 return sale;
             }));
            Charge
                .ThrownExceptions
                .Select(x => x.Message)
                .Subscribe(x => Errors = x);
            Charge
                .Subscribe(x => Sale = x);
            Charge
                .Select(_ => CartItems.ToList())
                .InvokeCommand(CalculateCartSubTotal);
            Charge
                .InvokeCommand(Validate);
            Pay
                .Select(_ => new LoadParameter(SaleViewModel?.SearchQuery, SaleViewModel!.CurrentPage, SaleViewModel.ItemPerPage))
                .InvokeCommand(SaleViewModel!.LoadSales);

            UpdateSellingPrice = ReactiveCommand.Create(() => SelectedSaleCartItem);

        }

        public SaleViewModel SaleViewModel { get; }

        public Sale Sale { get; private set; }

        [Reactive]
        public SaleCartItem? SelectedSaleCartItem { get; set; }

        public IObservableCollection<Customer> Customers => _targetCollectionCustomers;
        public IObservableCollection<Article> Articles => _targetCollectionArticles;
        public IObservableCollection<SaleCartItem> CartItems => _targetCollectionCartItems;

        public ReactiveCommand<LoadParameter,List<Article>> LoadArticles { get; }

        public ReactiveCommand<LoadParameter, List<Customer>> LoadCustomers { get; }

        public ReactiveCommand<int,List<SaleCartItem>> AddToCart { get; }

        public ReactiveCommand<int, List<SaleCartItem>> RemoveFromCart { get; }

        public ReactiveCommand<List<SaleCartItem>, decimal> CalculateCartSubTotal { get; }

        public ReactiveCommand<Unit, SaleCartItem?> UpdateSellingPrice { get; }

        public ReactiveCommand<Sale, string> Validate { get; }

        public ReactiveCommand<Unit, Sale> Charge { get; }

        [ObservableAsProperty]
        public decimal CartSubTotal { get; }

        public string? SearchCustomerQuery { get; set; }

        public int CurrentCustomerPage { get; set; } = 0;

        public int CustomerItemsPerPage { get; set; } = 100;

        public string? SearchArticleQuery { get; set; }

        public int CurrentArticlePage { get; set; } = 0;

        public int ArticleItemsPerPage { get; set; } = 100;

        [ObservableAsProperty]
        public Customer SelectedCustomer { get; }

        private List<Article> LoadingArticles(LoadParameter parameter)
        => string.IsNullOrEmpty(parameter.SearchQuery) switch
        {
            true => _articleService.GetArticles(parameter.Skip, parameter.Take),
            _ => _articleService.SearchArticles(parameter.SearchQuery!)
        };

        private List<Customer> LoadingCustomers(LoadParameter parameter)
        => string.IsNullOrEmpty(parameter.SearchQuery) switch
        {
            true => _customerService.GetCustomers(parameter.Skip, parameter.Take),
            _ => _customerService.SearchCustomers(parameter.SearchQuery!)
        };

    }
}
