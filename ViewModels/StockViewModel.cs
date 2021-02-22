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
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using DynamicData.Binding;

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
            Categories = new List<string?>();
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
            LoadArticles.IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);
            SaveArticle
                .Select(x => new LoadParameter(SearchQuery, CurrentPage, ItemPerPage))
                .InvokeCommand(LoadArticles);

            SelectForUpdate = ReactiveCommand.Create<int, Article>(id => Articles.Single(x => x.Id == id));
            SelectForUpdate.ThrownExceptions
            .Select(x => x.Message)
            .Subscribe(x => Errors = x);

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

            DeleteArticle = ReactiveCommand.CreateFromTask<int, int>(id => Task.Run(() => _articleService.DeleteArticle(id)));
            DeleteArticle.ThrownExceptions
            .Select(x => x.Message)
            .Subscribe(x => Errors = x);
            DeleteArticle.IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);
            DeleteArticle
                .Select(x => new LoadParameter(SearchQuery, CurrentPage, ItemPerPage))
                .InvokeCommand(LoadArticles);


            //this.WhenAnyValue(x => x.SearchQuery)
            //    .Select(term => term?.Trim())
            //    .DistinctUntilChanged()
            //    .Select(x => new LoadParameter(x,CurrentPage,ItemPerPage))
            //    .InvokeCommand(LoadArticles);

            LoadCategories = ReactiveCommand.CreateFromTask(() => Task.Run(() => _articleService.GetCategories()));
            LoadCategories.ThrownExceptions
            .Select(x => x.Message)
            .Subscribe(x => Errors = x);
            LoadCategories
                .Subscribe(categories => Categories = categories);


            LoadArticles.Execute(new LoadParameter(SearchQuery, CurrentPage, ItemPerPage))
                .Select(_ => Unit.Default)
                .InvokeCommand(LoadCategories);

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
        }

        public int CurrentPage { get; set; } = 0;

        public int ItemPerPage { get; set; } = 100;

        [Reactive]
        public Article Model { get; set; }

        public IObservableCollection<Article> Articles => _targetCollectionItems;

        [Reactive]
        public List<string?> Categories { get; set; }

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
        public int? InStock { get; set; }

        [Reactive]
        public string? SearchQuery { get; set; }

        [Reactive]
        public string? Category { get; set; }

        [Reactive]
        public string? Conditionement { get; set; }

        [Reactive]
        public int? QtyPerConditionement { get; set; }



        public string? UrlPathSegment => nameof(StockViewModel);

        public IScreen HostScreen { get; }

        public ReactiveCommand<Unit, Article> SaveArticle { get; }

        public ReactiveCommand<int,Article> SelectForUpdate { get; }

        public ReactiveCommand<int, Article> SelectForDelete{ get; }

        public ReactiveCommand<int, int> DeleteArticle { get; }

        public ReactiveCommand<ValidationParameter<Article>, string> Validate { get; }

        public ReactiveCommand<LoadParameter, List<Article>> LoadArticles { get; }

        public ReactiveCommand<Unit, List<string?>> LoadCategories { get; }


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
