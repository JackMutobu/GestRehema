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
    public class SaleViewModel : BaseSaleViewModel, IRoutableViewModel
    {
        private readonly NavigationRootViewModel _navigationRootViewModel;
        private SourceList<Sale> _sales { get; } = new SourceList<Sale>();
        private readonly IObservableCollection<Sale> _targetCollectionSales = new ObservableCollectionExtended<Sale>();

        public SaleViewModel(NavigationRootViewModel navigationRootViewModel = null!) : base(new SaleValidation())
        {
            _navigationRootViewModel = navigationRootViewModel ?? Locator.Current.GetService<NavigationRootViewModel>();

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

            LoadSale.ThrownExceptions
             .Select(x => x.Message)
             .Subscribe(x => Errors = x);
            LoadSale
                .Subscribe(x => 
                SelectedSale = x);
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
              .Select(x => 
              {
                  var totalQty = Math.Round(x.Sum(x => x.Quantity),5, MidpointRounding.ToEven);
                  var totalDeliveredQty = Math.Round(x.Sum(x => x.DeliveredQuantity),5, MidpointRounding.ToEven);
                  return totalQty > totalDeliveredQty;
              })
              .Select(x => x)
              .ToPropertyEx(this, x => x.CanAddDelivery);

            LoadSales
                .Execute(new LoadParameter(SearchQuery, CurrentPage, ItemPerPage))
                .Subscribe();

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


            AddPayement = ReactiveCommand.Create(() => new SalePayementModel(SelectedSale!, Entreprise, Locator.Current.GetService<ICustomerService>().GetWallet(SelectedSale!.Customer!.Id) ?? throw new Exception("Wallet can not be null"), Debt));
            AddPayement
                .ToPropertyEx(this, x => x.PayementModel);

            AddPayement
                .SelectMany(x =>
                x.Pay)
                .Where(x => x.Count > 0)
                .InvokeCommand(Pay);

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

        }


        public int CurrentPage { get; set; } = 0;

        public int ItemPerPage { get; set; } = 100;

        [Reactive]
        public string? SearchQuery { get; set; }

        [ObservableAsProperty]
        public ObservableCollection<SaleArticleItem> SaleArticles { get; }

        [ObservableAsProperty]
        public bool CanAddDelivery { get;}

        [Reactive]
        public Sale? SelectedSale { get; protected set; }

        [ObservableAsProperty]
        public decimal TotalToPay { get; protected set; }

        [ObservableAsProperty]
        public decimal TotalPaid { get; }

        [ObservableAsProperty]
        public decimal Debt { get; }

        [ObservableAsProperty]
        public SaleDeliveryModel? DeliveryModel { get; }


        public IObservableCollection<Sale> Sales => _targetCollectionSales;

        public string? UrlPathSegment => nameof(SaleViewModel);

        public IScreen HostScreen => _navigationRootViewModel;

        public ReactiveCommand<LoadParameter, List<Sale>> LoadSales { get; }

        public ReactiveCommand<int, Sale> LoadSale { get; }

        public ReactiveCommand<Unit, Sale> Deliver { get; }

        public ReactiveCommand<int, SaleDeliveryModel> AddDelivery { get; }

        public ReactiveCommand<int, Sale> DeliverAll { get; }

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
