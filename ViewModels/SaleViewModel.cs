using DynamicData;
using DynamicData.Binding;
using GestRehema.Entities;
using GestRehema.Services;
using GestRehema.Validations;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace GestRehema.ViewModels
{
    public class SaleViewModel : ViewModelBaseWithValidation, IRoutableViewModel
    {
        private readonly NavigationRootViewModel _navigationRootViewModel;
        private readonly ISaleService _saleService;
        private readonly IWalletService _walletService;

        private SourceList<Sale> _sales { get; } = new SourceList<Sale>();
        private readonly IObservableCollection<Sale> _targetCollectionSales = new ObservableCollectionExtended<Sale>();

        public SaleViewModel(NavigationRootViewModel navigationRootViewModel = null!) : base(new SaleValidation())
        {
            _navigationRootViewModel = navigationRootViewModel ?? Locator.Current.GetService<NavigationRootViewModel>();
            _saleService = Locator.Current.GetService<ISaleService>();
            _walletService = Locator.Current.GetService<IWalletService>();

            _sales.Connect()
            .ObserveOnDispatcher()
            .Bind(_targetCollectionSales)
            .Subscribe();

            LoadSales = ReactiveCommand.CreateFromTask<LoadParameter, List<Sale>>(p =>
             {
                 CurrentPage = p.Skip;
                 ItemPerPage = p.Take;
                 SearchQuery = p.SearchQuery;
                 return Task.Run(() => LoadingSales(p));
             },Observable.Return(!IsBusy));
            LoadSales
                .Subscribe(x =>
                {
                    _sales.Clear();
                    _sales.AddRange(x);
                });
            LoadSales.ThrownExceptions
              .Select(x => x.Message)
              .Subscribe(x => Errors = x);
            LoadSales.IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);

            LoadSale = ReactiveCommand.CreateFromTask<int, Sale>(id => Task.Run(() => Locator.Current.GetService<ISaleService>().GetSale(id)));
            LoadSale
                .Subscribe(x => SelectedSale = x);

            LoadSale.ThrownExceptions
             .Select(x => x.Message)
             .Subscribe(x => Errors = x);
            LoadSales
                .Where(x => x.Count > 0)
                .Select(x => x.First().Id)
                .InvokeCommand(LoadSale);
            LoadSale
                .Select(sale => new ObservableCollection<SaleArticleItem>(GetSaleArticles(sale)))
                .ToPropertyEx(this, x => x.SaleArticles);
            LoadSale
              .Select(sale => GetSaleArticles(sale).Sum(x => x.TotalAmount))
              .ToPropertyEx(this, x => x.TotalToPay);
            LoadSale
                .Select(x => x.PayementHistory.Sum(x => x.AmountPaid))
                .ToPropertyEx(this, x => x.TotalPaid);

            LoadSale
              .Select(sale => GetSaleArticles(sale).Sum(x => x.TotalAmount))
              .CombineLatest(LoadSale.Select(x => x.PayementHistory.Sum(x => x.AmountPaid)),
              (totAmount,totPaid) => totAmount - totPaid)
              .ToPropertyEx(this, x => x.Debt);


            LoadSale
              .Select(sale => GetSaleArticles(sale))
              .Select(x => x.Sum(x => x.Quantity) > x.Sum(x => x.DeliveredQuantity))
              .ToPropertyEx(this, x => x.CanAddDelivery);

            LoadSales
                .Execute(new LoadParameter(SearchQuery, CurrentPage, ItemPerPage))
                .Subscribe();

            AddPayement = ReactiveCommand.Create(() => new SalePayementModel(SelectedSale!, Entreprise.TauxDuJour, Debt));
            AddPayement
                .ToPropertyEx(this, x => x.PayementModel);

            Pay = ReactiveCommand.CreateFromTask<Unit, Sale>(_ => Task.Run(() =>
            {
                if (PayementModel == null)
                    throw new Exception("Veuillez procéder au paiement");

                var sale = PayementModel.Sale;
                var payement = new SalePayement()
                {
                    AmountPaid = PayementModel.TotalPaid > PayementModel.TotalAmount ? PayementModel.TotalAmount : PayementModel.TotalPaid,
                    Date = Entreprise.DateDuJour,
                    SaleId = sale.Id,
                    Payement = new Payement
                    {
                        AmountInCDF = PayementModel.PaidInCDF,
                        AmountInUSD = PayementModel.PaidInUsd,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        Method = PayementModel.PayementMethod,
                        ToCompany = true
                    }
                };

                if (PayementModel.ExcessInUsd > 0 && PayementModel.AddExcessToCustomerWallet)
                    _walletService.AddExcess(SelectedSale!.Customer!.WalletId, Entreprise.WalletId, PayementModel.ExcessInUsd);

                if (PayementModel.Debt > 0)
                    _walletService.AddDebt(SelectedSale!.Customer!.WalletId, Entreprise.WalletId, PayementModel.Debt);

                _walletService.AddToEntreprise(Entreprise.WalletId, PayementModel.TotalAmount);

                sale = _saleService.AddPayement(payement, sale.Id);

                return sale;
            }), Observable.Return(!IsBusy));
            Pay
                .Select(x => x.Id)
                .InvokeCommand(LoadSale);
            Pay
                .Subscribe(x =>
                {
                    var sale = _sales.Items.Single(y => y.Id == x.Id);
                    _sales.Remove(sale);
                    _sales.Insert(0, x);
                });
            Pay
                .ThrownExceptions
                .Select(x => x.Message)
                .Subscribe(x => Errors = x);
            Pay
                .IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);

            AddDelivery = ReactiveCommand.Create<int,SaleDeliveryModel>(articleId => 
            {
                var saleArticle = SaleArticles.Single(x => x.Article.Id == articleId);
                return new SaleDeliveryModel(SelectedSale!, saleArticle.Article, saleArticle.Quantity - saleArticle.DeliveredQuantity);
            });
            AddDelivery
                .ToPropertyEx(this, x => x.DeliveryModel);

            Deliver = ReactiveCommand.CreateFromTask<Unit, Sale>(_ => Task.Run(() =>
            {
                if (DeliveryModel == null)
                    throw new Exception("Veuillez ajouter une livraison");

                var sale = DeliveryModel.Sale;
                var delivery = new SaleDelivery()
                {
                    DeliveredQuantity = DeliveryModel.QtyInConditionement,
                    Date = Entreprise.DateDuJour,
                    SaleId = sale.Id,
                    ArticleId = DeliveryModel.Article.Id
                };

                sale = _saleService.AddDelivery(delivery, sale.Id);

                return sale;
            }), Observable.Return(!IsBusy));
            Deliver
               .Select(x => x.Id)
               .InvokeCommand(LoadSale);
            Deliver
                .Subscribe(x =>
                {
                    var sale = _sales.Items.Single(y => y.Id == x.Id);
                    _sales.Remove(sale);
                    _sales.Insert(0, x);
                });
            Deliver
                .ThrownExceptions
                .Select(x => x.Message)
                .Subscribe(x => Errors = x);
            Deliver
                .IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);

            DeliverAll = ReactiveCommand.CreateFromTask<int, Sale>(saleId => Task.Run(() => _saleService.DeliverAll(saleId)));
            DeliverAll
               .Select(x => x.Id)
               .InvokeCommand(LoadSale);
            DeliverAll
                .Subscribe(x =>
                {
                    var sale = _sales.Items.Single(y => y.Id == x.Id);
                    _sales.Remove(sale);
                    _sales.Insert(0, x);
                });
            DeliverAll
                .ThrownExceptions
                .Select(x => x.Message)
                .Subscribe(x => Errors = x);
            DeliverAll
                .IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);

            //PrintBill = ReactiveCommand.Create(() => );

        }

        public int CurrentPage { get; set; } = 0;

        public int ItemPerPage { get; set; } = 100;

        [Reactive]
        public string? SearchQuery { get; set; }

        [Reactive]
        public Sale? SelectedSale { get; set; }

        [ObservableAsProperty]
        public ObservableCollection<SaleArticleItem> SaleArticles { get; }

        [ObservableAsProperty]
        public bool CanAddDelivery { get;}

        [ObservableAsProperty]
        public decimal TotalPaid { get; }

        [ObservableAsProperty]
        public decimal TotalToPay { get; }

        [ObservableAsProperty]
        public decimal Debt { get; }

        [ObservableAsProperty]
        public SalePayementModel PayementModel { get; }

        [ObservableAsProperty]
        public SaleDeliveryModel DeliveryModel { get; }


        public IObservableCollection<Sale> Sales => _targetCollectionSales;

        public string? UrlPathSegment => nameof(SaleViewModel);

        public IScreen HostScreen => _navigationRootViewModel;

        public ReactiveCommand<LoadParameter, List<Sale>> LoadSales { get; }

        public ReactiveCommand<int, Sale> LoadSale { get; }

        public ReactiveCommand<Unit, Sale> Pay { get; }

        public ReactiveCommand<Unit, SalePayementModel> AddPayement { get; }

        public ReactiveCommand<Unit, Sale> Deliver { get; }

        public ReactiveCommand<int, SaleDeliveryModel> AddDelivery { get; }

        public ReactiveCommand<int, Sale> DeliverAll { get; }

        public ReactiveCommand<Unit, Unit> PrintBill { get; }

        private List<Sale> LoadingSales(LoadParameter parameter)
        => string.IsNullOrEmpty(parameter.SearchQuery) switch
        {
            true => _saleService.GetSalesWithCustomers(parameter.Skip, parameter.Take),
            _ => _saleService.SearchSales(parameter.SearchQuery!)
        };

        private IEnumerable<SaleArticleItem> GetSaleArticles(Sale sale)
        {
            foreach(var item in sale.ArticleSold)
                yield return new SaleArticleItem(item.Article!,this)
                {
                    DeliveredQuantity = sale.DeliveryHistory.Where(x => x.ArticleId == item.ArticleId).Sum(x => x.DeliveredQuantity),
                    Quantity = item.Quantity,
                    SaleId = sale.Id,
                    UnitBuyingPrice = item.UnitBuyingPrice,
                    UnitRealSellingPrice = item.UnitRealSellingPrice,
                    UnitSellingPrice = item.UnitSellingPrice,
                };
        }
    }
}
