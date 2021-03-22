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

namespace GestRehema.ViewModels
{
    public class SupplierManagerViewModel: ViewModelBaseWithValidation
    {
        private readonly IArticleService _articleService;
        private readonly ISupplierService _supplierService;

        public SupplierManagerViewModel(List<string> locations):base(new SupplierValidation())
        {
            _articleService = Locator.Current.GetService<IArticleService>();
            _supplierService = Locator.Current.GetService<ISupplierService>();
            Articles = new ObservableCollection<SupplierManagerArticle>();
            SelectedArticles = new List<Article>();
            Locations = locations;
            ImageUrl = "/Assets/Placeholder/profile.png";
            Model = new Supplier();

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
                    TechnicalCode = a.TechnicalCode
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
                    Model.ImageUrl = ImageUrl;
                    var regSupplier =  _supplierService.AddOrUpdateSupplier(Model);
                    _supplierService.AddArticlesToSupplier(regSupplier.Id, selectedArticles);
                    return regSupplier;
                }

                throw new Exception("Veuillez séléctionner les articles fournies par ce fournisseur");

               
            }), isValid);
            SaveSupplier.ThrownExceptions
                .Select(x => x.Message)
                .Subscribe(x => Errors = x);
            SaveSupplier.IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);

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

        public List<string> Locations { get; }

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
        }
    }
}
