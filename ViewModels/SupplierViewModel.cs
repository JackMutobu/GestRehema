using GestRehema.Entities;
using GestRehema.Validations;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System;
using GestRehema.Services;
using System.Reactive.Linq;
using System.Reactive;
using System.Linq;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;
using GestRehema.Contants;

namespace GestRehema.ViewModels
{
    public class SupplierViewModel : ViewModelBaseWithValidation, IRoutableViewModel
    {
        private readonly NavigationRootViewModel _navigationRootViewModel;
        private readonly ISupplierService _supplierService;
        private readonly IArticleService _articleService;

        public SupplierViewModel(NavigationRootViewModel navigationRootViewModel = null!) : base(new SupplierValidation())
        {
            _navigationRootViewModel = navigationRootViewModel ?? Locator.Current.GetService<NavigationRootViewModel>();
            Locations = new ObservableCollection<string>();
            Suppliers = new ObservableCollection<Supplier>();
            Articles = new ObservableCollection<Article>();
            _supplierService = Locator.Current.GetService<ISupplierService>();
            _articleService = Locator.Current.GetService<IArticleService>();
            var types = SupplierType.GetList();
            types.Insert(0, "Tous");
            SupplierTypes = types;



            this.WhenAnyValue(x => x.SelectedSupplier)
                .Where(x => x != null)
                .Select(x => x.Wallet)
                .ToPropertyEx(this, x => x.SupplierWallet);

            LoadSuppliers = ReactiveCommand.CreateFromTask<LoadParameter, List<Supplier>>(param => Task.Run(() =>
            {
                CurrentPage = param.Skip;
                ItemPerPage = param.Take;

                return LoadingSupplies(param,string.IsNullOrEmpty(SelectedLocation) ? null : SelectedLocation, string.IsNullOrEmpty(SelectedSupplierType) ? null : SelectedSupplierType );
            }));
            LoadSuppliers
                .Select(x => new ObservableCollection<Supplier>(x))
                .ToPropertyEx(this, x => x.Suppliers);

            LoadSuppliers.ThrownExceptions
               .Select(x => x.Message)
               .Subscribe(x => Errors = x);
            LoadSuppliers.ThrownExceptions
                .Subscribe(x => Crashes.TrackError(x));
            LoadSuppliers
                .Where(x => SelectedSupplier == null)
                .Where(x => x.Count > 0)
                .Select(x => x.First())
                .Subscribe(x => SelectedSupplier = x);



            LoadLocations = ReactiveCommand.CreateFromTask<Unit, List<string>>(_ => Task.Run(() => _supplierService.GetLocations()));
            LoadLocations.ThrownExceptions
             .Select(x => x.Message)
             .Subscribe(x => Errors = x);
            LoadLocations.ThrownExceptions
               .Subscribe(x => Crashes.TrackError(x));
            LoadLocations
                .Select(x =>
                {
                    x.Insert(0, "Toutes");
                    return new ObservableCollection<string>(x);
                })
                .ToPropertyEx(this, x => x.Locations);

            this.WhenAnyValue(x => x.SearchQuery)
                .Where(x => x != null)
                .Skip(1)
                .DistinctUntilChanged()
                .Select(x =>  new LoadParameter(x, CurrentPage, ItemPerPage))
                .InvokeCommand(LoadSuppliers);

            this.WhenAnyValue(x => x.SelectedLocation)
                .Skip(1)
                .Where(x => !string.IsNullOrEmpty(x))
                .DistinctUntilChanged()
                .Select(x => new LoadParameter(SearchQuery, CurrentPage, ItemPerPage))
                .InvokeCommand(LoadSuppliers);

            this.WhenAnyValue(x => x.SelectedSupplierType)
                .Where(x => x != null)
                .Skip(1)
              .DistinctUntilChanged()
              .Select(x => new LoadParameter(SearchQuery, CurrentPage, ItemPerPage))
              .InvokeCommand(LoadSuppliers);

            LoadArticles = ReactiveCommand.CreateFromTask<Supplier?, List<Article>>(sup => Task.Run(() => _articleService.GetArticlesBySupplierId(sup.Id)), Observable.Return(!IsBusy));
            LoadArticles.ThrownExceptions
             .Select(x => x.Message)
             .Subscribe(x => Errors = x);
            LoadArticles.ThrownExceptions
               .Subscribe(x => Crashes.TrackError(x));
            LoadArticles.IsExecuting
            .ToPropertyEx(this,x => x.IsBusy);
            LoadArticles
                .Select(x => new ObservableCollection<Article>(x))
                .ToPropertyEx(this, x => x.Articles);

            this.WhenAnyValue(x => x.SelectedSupplier)
                .Where(x => x != null)
                .InvokeCommand(LoadArticles);

            AddSupplier = ReactiveCommand.Create<Unit, SupplierManagerViewModel>(_ =>
            {
                var locations = Locations.ToList();
                locations.RemoveAt(0);
                return new SupplierManagerViewModel(locations);
            });

            AddSupplier
                .SelectMany(x => x.SaveSupplier)
                .Select(x => new LoadParameter(SearchQuery, CurrentPage, ItemPerPage))
                .InvokeCommand(LoadSuppliers);
            AddSupplier
               .SelectMany(x => x.SaveSupplier)
               .Subscribe(x => SelectedSupplier = x);
            AddSupplier
                .SelectMany(x => x.SaveSupplier)
                .Subscribe(x => Analytics.TrackEvent(nameof(AnalyticsKeys.AddedSupplier)));
            AddSupplier.ThrownExceptions
               .Subscribe(x => Crashes.TrackError(x));

            UpdateSupplier = ReactiveCommand.Create<Unit, SupplierManagerViewModel>(_ =>
            {
                var locations = Locations.ToList();
                locations.RemoveAt(0);
                return new SupplierManagerViewModel(locations, SelectedSupplier);
            });

            UpdateSupplier
               .SelectMany(x => x.SaveSupplier)
               .Select(x => new LoadParameter(SearchQuery, CurrentPage, ItemPerPage))
               .InvokeCommand(LoadSuppliers);
            UpdateSupplier
               .SelectMany(x => x.SaveSupplier)
               .Subscribe(x => SelectedSupplier = x);
            UpdateSupplier
              .SelectMany(x => x.SaveSupplier)
             .Subscribe(x => Analytics.TrackEvent(nameof(AnalyticsKeys.UpdatedSupplier)));
            UpdateSupplier.ThrownExceptions
               .Subscribe(x => Crashes.TrackError(x));

            LoadSuppliers
                .Take(1)
                .Select(_ => Unit.Default)
                .InvokeCommand(LoadLocations);

        }

        public string? UrlPathSegment => nameof(SupplierViewModel);

        public IScreen HostScreen => _navigationRootViewModel;

        public List<string> SupplierTypes { get; }

        [ObservableAsProperty]
        public ObservableCollection<string> Locations { get; }

        [ObservableAsProperty]
        public ObservableCollection<Supplier> Suppliers { get; }

        [Reactive]
        public Supplier? SelectedSupplier { get; set; }

        [ObservableAsProperty]
        public Wallet? SupplierWallet { get; }

        [ObservableAsProperty]
        public ObservableCollection<Article> Articles { get; }

        [Reactive]
        public string? SearchQuery { get; set; }

        [Reactive]
        public string? SelectedLocation { get; set; }

        [Reactive]
        public string? SelectedSupplierType { get; set; }

        public int CurrentPage { get; set; } = 0;

        public int ItemPerPage { get; set; } = 100;

        public ReactiveCommand<LoadParameter,List<Supplier>> LoadSuppliers { get; }

        public ReactiveCommand<Unit, SupplierManagerViewModel> AddSupplier { get; }

        public ReactiveCommand<Unit, SupplierManagerViewModel> UpdateSupplier { get; }

        public ReactiveCommand<Supplier?, List<Article>> LoadArticles { get; }

        public ReactiveCommand<Unit, List<string>> LoadLocations { get; }

        public List<Supplier> LoadingSupplies(LoadParameter parameter, string? location, string? supplierType)
            => string.IsNullOrEmpty(parameter.SearchQuery) switch
            {
                true => _supplierService.GetSuppliers(parameter.Skip, parameter.Take, location == "Toutes" ? null : location, supplierType == "Tous" ? null : supplierType),
                _ => _supplierService.SearchSupplier(parameter.SearchQuery ?? "")
            };
    }
}
