using GestRehema.Entities;
using GestRehema.Validations;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System.Reactive;
using System;
using System.Reactive.Linq;
using GestRehema.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using DynamicData;
using DynamicData.Binding;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;
using GestRehema.Contants;

namespace GestRehema.ViewModels
{
    public record LoadParameter(string? SearchQuery,int Skip = 0, int Take = 100);
    public class StockViewModel : ViewModelBaseWithValidation, IRoutableViewModel
    {
        private NavigationRootViewModel _navigationRootViewModel;
        private IArticleService _articleService;
        private SourceList<Article> _items { get; } = new SourceList<Article>();
        private readonly IObservableCollection<Article> _targetCollectionItems = new ObservableCollectionExtended<Article>();

        public StockViewModel(NavigationRootViewModel screen = null!) : base(new ArticleValidation())
        {
            _navigationRootViewModel = screen ?? Locator.Current.GetService<NavigationRootViewModel>();
            HostScreen = _navigationRootViewModel;
            _articleService = Locator.Current.GetService<IArticleService>();
            Model = new Article();
            ImageUrl = "/Assets/Placeholder/product.jpg";
            Categories = new List<string>();
            _items.Connect()
            .ObserveOnDispatcher()
            .Bind(_targetCollectionItems)
            .Subscribe();

            Validate = ReactiveCommand
               .Create<ValidationParameter<Article>, string>(p => RaiseValidation(p.Model, p.PropertyName));
            Validate
                .Subscribe(x => Errors = x);

            ValidateModel();

            SaveArticle = ReactiveCommand
                .CreateFromTask(() => Task.Run(() =>
                 {
                     Model.ImageUrl = ImageUrl;
                     return _articleService.AddOrUpdateArticle(Model);
                 }), isValid);
            SaveArticle.ThrownExceptions
               .Select(x => x.Message)
               .Subscribe(x => Errors = x);
            SaveArticle.IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);
            SaveArticle.ThrownExceptions
              .Subscribe(x => Crashes.TrackError(x));
            SaveArticle
              .Subscribe(x =>
              {
                  InitializeFields();

                  if (!_items.Items.Any(y => y.Id == x.Id))
                      _items.Insert(0, x);
                  else
                  {
                      var updatedItem = _items.Items.Single(y => y.Id == x.Id);
                      int index = _items.Items.IndexOf(updatedItem);
                      _items.ReplaceAt(index, x);
                  }
              });


            LoadArticles = ReactiveCommand.CreateFromTask<LoadParameter, List<Article>>
                (p =>
                {
                    CurrentPage = p.Skip;
                    ItemPerPage = p.Take;
                    SearchQuery = p.SearchQuery;
                    return Task.Run(() => LoadingArticles(p));
                },Observable.Return(!IsBusy));

            LoadArticles
                .Subscribe(x => 
                {
                    _items.Clear();
                    _items.AddRange(x);
                });



            LoadArticles.ThrownExceptions
              .Select(x => x.Message)
              .Subscribe(x => Errors = x);
            LoadArticles.ThrownExceptions
             .Subscribe(x => Crashes.TrackError(x));
            LoadArticles.IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);

            SelectForUpdate = ReactiveCommand.Create<int, Article>(id => Articles.Single(x => x.Id == id));
            SelectForUpdate.ThrownExceptions
            .Select(x => x.Message)
            .Subscribe(x => Errors = x);
            SelectForUpdate.ThrownExceptions
             .Subscribe(x => Crashes.TrackError(x));

            SelectForUpdate
                .Subscribe(x =>
                {
                    ImageUrl = x.ImageUrl ?? "";
                    Name = x.Name;
                    TechnicalCode = x.TechnicalCode;
                    UnitOfMeasure = x.UnitOfMeasure;
                    SellingPrice = x.SellingPrice;
                    BuyingPrice = x.BuyingPrice;
                    InStock = x.InStock;
                    Category = x.Category;
                    Conditionement = x.Conditionement;
                    QtyPerConditionement = x.QtyPerConditionement;
                    Model = new Article()
                    {
                        Id = x.Id,
                        BuyingPrice = x.BuyingPrice,
                        SellingPrice = x.SellingPrice,
                        Name = x.Name,
                        InStock = x.InStock,
                        TechnicalCode = x.TechnicalCode,
                        UnitOfMeasure = x.UnitOfMeasure,
                        ImageUrl = x.ImageUrl,
                        Category = x.Category,
                        Conditionement = x.Conditionement,
                        QtyPerConditionement = x.QtyPerConditionement
                    };
                });

            SelectForDelete = ReactiveCommand.Create<int, Article>(id => Articles.Single(x => x.Id == id));
            SelectForDelete.ThrownExceptions
            .Select(x => x.Message)
            .Subscribe(x => Errors = x);
            SelectForDelete.ThrownExceptions
             .Subscribe(x => Crashes.TrackError(x));

            DeleteArticle = ReactiveCommand.CreateFromTask<int, int>(id => Task.Run(() => _articleService.DeleteArticle(id)));
            DeleteArticle.ThrownExceptions
            .Select(x => x.Message)
            .Subscribe(x => Errors = x);
            DeleteArticle.ThrownExceptions
             .Subscribe(x => Crashes.TrackError(x));
            DeleteArticle.IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);
            DeleteArticle
                .Select(x => new LoadParameter(SearchQuery, CurrentPage, ItemPerPage))
                .InvokeCommand(LoadArticles);


            LoadCategories = ReactiveCommand.CreateFromTask(() => Task.Run(() => _articleService.GetCategories()));
            LoadCategories.ThrownExceptions
            .Select(x => x.Message)
            .Subscribe(x => Errors = x);
            LoadCategories.ThrownExceptions
             .Subscribe(x => Crashes.TrackError(x));
            LoadCategories
                .Subscribe(categories => Categories = categories);

            SaveArticle
                .Select(_ => Unit.Default)
                .InvokeCommand(LoadCategories);
            SaveArticle
               .Subscribe(x => Analytics.TrackEvent(nameof(AnalyticsKeys.ArticleSaved)));


            LoadArticles.Execute(new LoadParameter(SearchQuery, CurrentPage, ItemPerPage))
                .Select(_ => Unit.Default)
                .InvokeCommand(LoadCategories);

            LoadArticle = ReactiveCommand.CreateFromTask<int,Article?>(id => Task.Run(() => Locator.Current.GetService<IArticleService>().GetArticle(id)));
            LoadArticle
                .Where(x => x != null)
                .Subscribe(x => SelectedArticle = x);
            LoadArticles
                .Where(x => x != null && x.Count > 0)
                .Select(x => x.First().Id)
                .InvokeCommand(LoadArticle);
            SaveArticle
                .Select(x => x.Id)
                .InvokeCommand(LoadArticle);

        }

        private void InitializeFields()
        {
            Model = new Article();
            Name = null;
            TechnicalCode = null;
            UnitOfMeasure = null;
            SellingPrice = null;
            BuyingPrice = null;
            InStock = null;
            ImageUrl = "/Assets/Placeholder/product.jpg";
            QtyPerConditionement = null;
            Conditionement = null;
            Category = null;
        }

        public int CurrentPage { get; set; } = 0;

        public int ItemPerPage { get; set; } = 100;

        [Reactive]
        public Article Model { get; set; }

        public IObservableCollection<Article> Articles => _targetCollectionItems;

        [Reactive]
        public List<string> Categories { get; set; }

        [Reactive]
        public string ImageUrl { get; set; }

        [Reactive]
        public string? Name { get; set; }

        [Reactive]
        public string? TechnicalCode { get; set; }

        [Reactive]
        public string? UnitOfMeasure { get; set; }

        [Reactive]
        public decimal? SellingPrice { get; set; }

        [Reactive]
        public decimal? BuyingPrice { get; set; }

        [Reactive]
        public double? InStock { get; set; }

        [Reactive]
        public string? SearchQuery { get; set; }

        [Reactive]
        public string? Category { get; set; }

        [Reactive]
        public string? Conditionement { get; set; }

        [Reactive]
        public double? QtyPerConditionement { get; set; }

        [Reactive]
        public Article? SelectedArticle { get; set; }



        public string? UrlPathSegment => nameof(StockViewModel);

        public IScreen HostScreen { get; }

        public ReactiveCommand<Unit, Article> SaveArticle { get; }

        public ReactiveCommand<int,Article> SelectForUpdate { get; }

        public ReactiveCommand<int, Article> SelectForDelete{ get; }

        public ReactiveCommand<int, int> DeleteArticle { get; }

        public ReactiveCommand<ValidationParameter<Article>, string> Validate { get; }

        public ReactiveCommand<LoadParameter, List<Article>> LoadArticles { get; }

        public ReactiveCommand<Unit, List<string>> LoadCategories { get; }

        public ReactiveCommand<int, Article?> LoadArticle { get; }


        private void ValidateModel()
        {
            this.WhenAnyValue(x => x.Name)
                .Where(x => x != null)
                .Select(x =>
                {
                    Model.Name = x;
                    return new ValidationParameter<Article>(Model, nameof(Article.Name));
                })
                .InvokeCommand(Validate);

            this.WhenAnyValue(x => x.TechnicalCode)
                .Where(x => x != null)
                .Select(x =>
                {
                    Model.TechnicalCode = x;
                    return new ValidationParameter<Article>(Model, nameof(Article.TechnicalCode));
                })
                .InvokeCommand(Validate);

            this.WhenAnyValue(x => x.UnitOfMeasure)
               .Where(x => x != null)
               .Select(x =>
               {
                   Model.UnitOfMeasure = x;
                   return new ValidationParameter<Article>(Model, nameof(Article.UnitOfMeasure));
               })
               .InvokeCommand(Validate);

            this.WhenAnyValue(x => x.SellingPrice)
               .Where(x => x != null)
               .Select(x =>
               {
                   Model.SellingPrice = x.Value;
                   return new ValidationParameter<Article>(Model, nameof(Article.SellingPrice));
               })
               .InvokeCommand(Validate);

            this.WhenAnyValue(x => x.BuyingPrice)
              .Where(x => x != null)
              .Select(x =>
              {
                  Model.BuyingPrice = x.Value;
                  return new ValidationParameter<Article>(Model, nameof(Article.BuyingPrice));
              })
              .InvokeCommand(Validate);

            this.WhenAnyValue(x => x.InStock)
             .Where(x => x != null)
             .Select(x =>
             {
                 Model.InStock = x.Value;
                 return new ValidationParameter<Article>(Model, nameof(Article.InStock));
             })
             .InvokeCommand(Validate);

            this.WhenAnyValue(x => x.Category)
             .Where(x => x != null)
             .Select(x =>
             {
                 Model.Category = x;
                 return new ValidationParameter<Article>(Model, nameof(Article.Category));
             })
             .InvokeCommand(Validate);

            this.WhenAnyValue(x => x.QtyPerConditionement)
            .Where(x => x != null)
            .Select(x =>
            {
                Model.QtyPerConditionement = x.Value;
                return new ValidationParameter<Article>(Model, nameof(Article.QtyPerConditionement));
            })
            .InvokeCommand(Validate);

            this.WhenAnyValue(x => x.Conditionement)
            .Where(x => x != null)
            .Select(x =>
            {
                Model.Conditionement = x;
                return new ValidationParameter<Article>(Model, nameof(Article.Conditionement));
            })
            .InvokeCommand(Validate);
        }

        private List<Article> LoadingArticles(LoadParameter parameter)
         => string.IsNullOrEmpty(parameter.SearchQuery) switch
         {
             true => _articleService.GetArticles(parameter.Skip, parameter.Take),
             _ => _articleService.SearchArticles(parameter.SearchQuery!)
         };
    }
}
