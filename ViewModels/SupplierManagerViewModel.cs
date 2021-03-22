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

namespace GestRehema.ViewModels
{
    public class SupplierManagerViewModel: ViewModelBaseWithValidation
    {
        private readonly IArticleService _articleService;

        public SupplierManagerViewModel(List<string> locations):base(new SupplierValidation())
        {
            _articleService = Locator.Current.GetService<IArticleService>();
            Articles = new ObservableCollection<SupplierManagerArticle>();
            SelectedArticles = new List<Article>();
            Locations = locations;

            LoadArticles = ReactiveCommand.CreateFromTask<string?, List<Article>>(query => Task.Run(() => LoadingArticles(query)));
            LoadArticles
                .Select(x => new ObservableCollection<SupplierManagerArticle>(x.Select(a => new SupplierManagerArticle 
                { 
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
