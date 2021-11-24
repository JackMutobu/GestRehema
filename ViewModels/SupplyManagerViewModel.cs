using GestRehema.Entities;
using GestRehema.Services;
using GestRehema.Validations;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AppCenter.Crashes;
using DynamicData;
using DynamicData.Binding;
using GestRehema.Contants;

namespace GestRehema.ViewModels
{
    public record ArticleLoadParameter(int? SupplierId, string? SearchQuery);
    public class SupplyManagerViewModel : ViewModelBaseWithValidation
    {
        private readonly IArticleService _articleService;
        private readonly ISupplierService _supplierService;
        private readonly ISupplyService _supplyService;
        private readonly IObservableCollection<SupplyCartItem> _targetCollectionCartItems = new ObservableCollectionExtended<SupplyCartItem>();
        private SourceList<SupplyCartItem> _cartItems  = new SourceList<SupplyCartItem>();
        public Supply _supply = new Supply();

        public SupplyManagerViewModel(Supply? supply = null) : base(new SupplyValidation())
        {
            _articleService = Locator.Current.GetService<IArticleService>();
            _supplierService = Locator.Current.GetService<ISupplierService>();
            _supplyService = Locator.Current.GetService<ISupplyService>();
            _cartItems.Connect()
            .ObserveOnDispatcher()
            .Bind(_targetCollectionCartItems)
            .Subscribe();
            _supply = supply ?? new Supply();

            

            Suppliers = new ObservableCollection<Supplier>();
            LoadSuppliers = ReactiveCommand.CreateFromTask<string?, List<Supplier>>(searchQuery => Task.Run(() =>
            {
                return string.IsNullOrEmpty(searchQuery) switch
                {
                    true => _supplierService.GetSuppliers(),
                    _ => _supplierService.SearchSupplier(searchQuery ?? string.Empty)
                };
            }), Observable.Return(!IsBusy));
            LoadSuppliers
                .Select(suppliers => new ObservableCollection<Supplier>(suppliers))
                .ToPropertyEx(this, x => x.Suppliers);
            LoadSuppliers
                .IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);
            LoadSuppliers.ThrownExceptions
              .Select(x => x.Message)
              .Subscribe(x => Errors = x);
            LoadSuppliers.ThrownExceptions
                .Subscribe(x => Crashes.TrackError(x));
            LoadSuppliers
                .Execute(string.Empty)
                .Subscribe();

            Articles = new ObservableCollection<Article>();
            LoadArticles = ReactiveCommand.CreateFromTask<ArticleLoadParameter, List<Article>>(param => Task.Run(() =>
            {
                return string.IsNullOrEmpty(param.SearchQuery) switch
                {
                    true => param.SupplierId == null ? _articleService.GetArticles() : _articleService.GetArticlesBySupplierId(param.SupplierId.Value),
                    _ => _articleService.SearchArticles(param.SearchQuery ?? string.Empty, param.SupplierId)
                };
            }), Observable.Return(!IsBusy));
            LoadArticles
                .Select(articles => new ObservableCollection<Article>(articles))
                .ToPropertyEx(this, x => x.Articles);
            LoadArticles.ThrownExceptions
              .Select(x => x.Message)
              .Subscribe(x => Errors = x);
            LoadArticles.ThrownExceptions
                .Subscribe(x => Crashes.TrackError(x));
            LoadArticles
                .IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);

            this.WhenAnyValue(x => x.SearchSupplierQuery)
                .Skip(1)
                .Catch((Exception ex) =>
                {
                    Errors = ex.Message;
                    Crashes.TrackError(ex);
                    return Observable.Empty<string>();
                })
                .InvokeCommand(LoadSuppliers);


            this.WhenAnyValue(x => x.SearchArticleQuery)
                .Skip(1)
                .Catch((Exception ex) =>
                {
                    Errors = ex.Message;
                    Crashes.TrackError(ex);
                    return Observable.Empty<string>();
                })
                .Select(query => new ArticleLoadParameter(SelectedSupplier?.Id, query))
                .InvokeCommand(LoadArticles);


            SelectedSupplier = new Supplier();
            LoadSuppliers
                .Where(suppliers => suppliers.Count > 0)
                .Select(suppliers => suppliers.First())
                .Subscribe(sup => SelectedSupplier = sup);
            this.WhenAnyValue(x => x.SelectedSupplier)
                .Skip(1)
                .Select(sup => new ArticleLoadParameter(sup?.Id, SearchSupplierQuery))
                .InvokeCommand(LoadArticles);

            this.WhenAnyValue(x => x.SelectedSupplier)
                .Skip(1)
                .Subscribe(_ => 
                {
                    _cartItems.Clear();
                    CartSubTotal = 0;
                });

            AddToCart = ReactiveCommand.CreateFromTask<int, SupplyCartItem>(articleId => Task.Run(() => new SupplyCartItem(Articles.First(x => x.Id == articleId))));
            AddToCart
                .Select(item => 
                {
                    var supplyArticles = _cartItems.Items.ToList();
                    var regSupplyArticle = supplyArticles!.FirstOrDefault(x => x.Article.Id == item.Article.Id);

                    if (regSupplyArticle == null)
                        supplyArticles.Add(item);
                    else
                    {
                        var regIndex = supplyArticles.IndexOf(regSupplyArticle);
                        regSupplyArticle.QtyInConditionement += 1;
                        supplyArticles.RemoveAt(regIndex);
                        supplyArticles.Insert(regIndex, regSupplyArticle);
                    }

                    return supplyArticles;
                })
                .Subscribe(items =>
                {
                    _cartItems.Clear();
                    _cartItems.AddRange(items);
                });
            AddToCart
                .SelectMany(item => item.WhenAnyValue(x => x.Total))
                .Subscribe(_ => CartSubTotal = _cartItems.Items.Sum(x => x.Total));
            AddToCart.ThrownExceptions
              .Select(x => x.Message)
              .Subscribe(x => Errors = x);
            AddToCart.ThrownExceptions
                .Subscribe(x => Crashes.TrackError(x));

            RemoveFromCart = ReactiveCommand.CreateFromTask<int, SupplyCartItem>(articleId => Task.Run(() => _cartItems.Items.First(x => x.Article.Id == articleId)));
            RemoveFromCart
                .Subscribe(item => CartSubTotal -= item.Total);
            RemoveFromCart
                .Select(item =>
                {
                    var cartItems = _cartItems.Items.ToList();
                    var regCartItem = cartItems.SingleOrDefault(x => x.Article.Id == item.Article.Id);
                    if (regCartItem != null)
                        cartItems.Remove(regCartItem);

                    return cartItems;
                })
                .Subscribe(items =>
                 {
                     _cartItems.Clear();
                     _cartItems.AddRange(items);
                 });
            RemoveFromCart.ThrownExceptions
              .Select(x => x.Message)
              .Subscribe(x => Errors = x);
            RemoveFromCart.ThrownExceptions
                .Subscribe(x => Crashes.TrackError(x));

            Pay = ReactiveCommand.Create<Unit, SupplyPayementModel>(_ => 
            {
                if (SelectedSupplier == null)
                    throw new Exception("Veuillez sélectionner un fournisseur");
                if (SupplyCartitems.Count == 0)
                    throw new Exception("Veuillez ajouter des articles au panier");

                return new SupplyPayementModel(SelectedSupplier!.Wallet!, Entreprise, PayementType.NewSupplyPayement, CartSubTotal);
            });
            Pay.ThrownExceptions
             .Select(x => x.Message)
             .Subscribe(x => Errors = x);
            Pay.ThrownExceptions
                .Subscribe(x => Crashes.TrackError(x));

            Save = ReactiveCommand.CreateFromTask<List<Payement>, SupplyItem>(payements => Task.Run(() => 
            {
                if (supply == null)
                {
                    supply = new Supply
                    {
                        DateOperation = Entreprise.DateDuJour,
                        DeliveryStatus = SupplyDeliveryStatus.AwaitingDelivery,
                        PayementStatus = SupplyPayementStatus.AwaitingPayement,
                    };

                    supply = _supplyService.AddSupply(supply);
                }
                var supplyItem = new SupplyItem
                {
                    DateOperation = Entreprise.DateDuJour,
                    DeliveryStatus = SupplyDeliveryStatus.AwaitingDelivery,
                    PayementStatus = SupplyPayementStatus.AwaitingPayement,
                    SupplyId = supply.Id,
                    SupplierId = SelectedSupplier.Id
                };

                foreach (var item in SupplyCartitems)
                {
                    supplyItem.ArticlesSupplied.Add(new SupplyArticle
                    {
                        Date = Entreprise.DateDuJour,
                        Quantity = item.QtyInConditionement,
                        UnitBuyingPrice = item.BuyingPrice!.Value,
                        ArticleId = item.Article.Id
                    });
                }

                supplyItem = _supplyService.AddSupplyItem(supplyItem);

                foreach (var item in payements)
                {
                    item.SetTotalPaid(Entreprise.TauxDuJour);
                    var supplyPayement = new SupplyPayement
                    {
                        Date = Entreprise.DateDuJour,
                        AmountPaid = item.TotalPaid,
                        PayementId = item.Id,
                        SupplyItemId = supplyItem.Id
                    };
                    _supplyService.AddPayement(supplyPayement);
                }

                return supplyItem;
            }));
            Pay
                .SelectMany(model => model.Pay)
                .InvokeCommand(Save);

        }

        [Reactive]
        public decimal CartSubTotal { get; private set; }

        [ObservableAsProperty]
        public ObservableCollection<Article> Articles { get; }

        [ObservableAsProperty]
        public ObservableCollection<Supplier> Suppliers { get; }

        public IObservableCollection<SupplyCartItem> SupplyCartitems => _targetCollectionCartItems;

        [Reactive]
        public string? SearchArticleQuery { get; set; }

        [Reactive]
        public string? SearchSupplierQuery { get; set; }

        [Reactive]
        public Supplier SelectedSupplier { get; set;}

        public ReactiveCommand<ArticleLoadParameter,List<Article>> LoadArticles { get; }

        public ReactiveCommand<string?,List<Supplier>> LoadSuppliers { get; }

        public ReactiveCommand<int,SupplyCartItem> AddToCart { get; }

        public ReactiveCommand<int,SupplyCartItem> RemoveFromCart { get; }

        public ReactiveCommand<Unit, SupplyPayementModel> Pay { get; }

        public ReactiveCommand<List<Payement>, SupplyItem> Save { get; }
    }
}
