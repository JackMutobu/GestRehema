using GestRehema.Entities;
using GestRehema.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System;
using GestRehema.Validations;
using System.Linq;
using System.Reactive;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;
using GestRehema.Contants;

namespace GestRehema.ViewModels
{
    public class SupplierManagerViewModel: ViewModelBaseWithValidation
    {
        private readonly IArticleService _articleService;
        private readonly ISupplierService _supplierService;
        private List<Article?> _articles = new List<Article?>();
        public SupplierManagerViewModel(List<string> locations, Supplier? model = null):base(new SupplierValidation())
        {
            _articleService = Locator.Current.GetService<IArticleService>();
            _supplierService = Locator.Current.GetService<ISupplierService>();
            Articles = new ObservableCollection<SupplierManagerArticle>();
            SelectedArticles = new List<Article>();
            Locations = locations;
            ImageUrl = "/Assets/Placeholder/profile.png";
            if(model == null)
                Model = new Supplier();
            else
            {
                Model = model;
                ImageUrl = Model.ImageUrl ?? "/Assets/Placeholder/profile.png";
                Name = Model.Name;
                Adresse = Model.Adresse;
                NumTelephone = Model.NumTelephone;
                Email = Model.Email;
                SupplierType = Model.SupplierType;
                _articles = Model.Articles.Select(x => x.Article).ToList();
            }

            Validate = ReactiveCommand
             .Create<ValidationParameter<Supplier>, string>(p => RaiseValidation(p.Model, p.PropertyName));
            Validate
                .Subscribe(x => Errors = x);
            ValidateModel();

            LoadArticles = ReactiveCommand.CreateFromTask<string?, List<Article>>(query => Task.Run(() => LoadingArticles(query)));
            LoadArticles
                .Select(x => new ObservableCollection<SupplierManagerArticle>(x.Select(a => new SupplierManagerArticle 
                { 
                    Id = a.Id,
                    ImageUrl = a.ImageUrl,
                    Name = a.Name,
                    InStock = a.InStock,
                    BuyingPrice = a.BuyingPrice,
                    SellingPrice = a.SellingPrice,
                    TechnicalCode = a.TechnicalCode,
                    Selected = _articles.Any(x => x.Id == a.Id)
                })))
                .ToPropertyEx(this,x => x.Articles);

            this.WhenAnyValue(x => x.SearchQuery)
                .InvokeCommand(LoadArticles);

            SaveSupplier = ReactiveCommand.CreateFromTask<Unit,Supplier>(_ => Task.Run(() =>
            {
                var selectedArticles = Articles
                                            .Where(x => x.Selected)
                                            .Select(x => _articleService.GetArticle(x.Id))
                                            .ToList();
                if(selectedArticles.Count > 0)
                {
                    Model.ImageUrl = ImageUrl == "/Assets/Placeholder/profile.png" ? null : ImageUrl;
                    var regSupplier =  _supplierService.AddOrUpdateSupplier(Model);
                    _supplierService.AddArticlesToSupplier(regSupplier.Id, selectedArticles);
                    return regSupplier;
                }

                throw new Exception("Veuillez séléctionner les articles fournies par ce fournisseur");

               
            }), isValid);
            SaveSupplier.ThrownExceptions
                .Select(x => x.Message)
                .Subscribe(x => Errors = x);
            SaveSupplier.ThrownExceptions
               .Subscribe(x => Crashes.TrackError(x));
            SaveSupplier.IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);
            SaveSupplier
                .Subscribe(x => Analytics.TrackEvent(nameof(AnalyticsKeys.SupplierSaved)));

        }

        public Supplier Model { get; set; }



        [Reactive]
        public string Name { get; set; } = null!;

        [Reactive]
        public string? Adresse { get; set; }

        [Reactive]
        public string NumTelephone { get; set; } = null!;

        [Reactive]
        public string? Email { get; set; }

        [Reactive]
        public string? ImageUrl { get; set; }

        [Reactive]
        public string? SupplierType { get; set; }

        public List<string> Locations { get; }

        public List<string> SupplierTypes => Entities.SupplierType.GetList();

        [ObservableAsProperty]
        public ObservableCollection<SupplierManagerArticle> Articles { get; }

        [ObservableAsProperty]
        public List<Article> SelectedArticles { get; }

        [Reactive]
        public string? SearchQuery { get; set; }

        public ReactiveCommand<string?,List<Article>> LoadArticles { get; }

        public ReactiveCommand<ValidationParameter<Supplier>, string> Validate { get; }

        public ReactiveCommand<Unit,Supplier> SaveSupplier { get; }

        private List<Article> LoadingArticles(string? searchQuery)
        => string.IsNullOrEmpty(searchQuery) switch
        {
            true => _articleService.GetArticles(),
            _ => _articleService.SearchArticles(searchQuery)
        };

        private void ValidateModel()
        {
            this.WhenAnyValue(x => x.Name)
                .Where(x => x != null)
                .Select(x =>
                {
                    Model.Name = x;
                    return new ValidationParameter<Supplier>(Model, nameof(Supplier.Name));
                })
                .InvokeCommand(Validate);

            this.WhenAnyValue(x => x.NumTelephone)
                .Where(x => x != null)
                .Select(x =>
                {
                    Model.NumTelephone = x;
                    return new ValidationParameter<Supplier>(Model, nameof(Supplier.NumTelephone));
                })
                .InvokeCommand(Validate);

            this.WhenAnyValue(x => x.Email)
               .Where(x => x != null)
               .Select(x =>
               {
                   Model.Email = x;
                   return new ValidationParameter<Supplier>(Model, nameof(Supplier.Email));
               })
               .InvokeCommand(Validate);

            this.WhenAnyValue(x => x.Adresse)
               .Where(x => x != null)
               .Select(x =>
               {
                   Model.Adresse = x;
                   return new ValidationParameter<Supplier>(Model, nameof(Supplier.Adresse));
               })
               .InvokeCommand(Validate);

            this.WhenAnyValue(x => x.SupplierType)
             .Where(x => x != null)
             .Select(x =>
             {
                 Model.SupplierType = x;
                 return new ValidationParameter<Supplier>(Model, nameof(Supplier.SupplierType));
             })
             .InvokeCommand(Validate);
        }
    }
}
