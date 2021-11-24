using GestRehema.Entities;
using GestRehema.Services;
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

namespace GestRehema.ViewModels
{
    public class SupplyViewModel : ViewModelBase, IRoutableViewModel
    {
        private readonly NavigationRootViewModel _navigationRootViewModel;
        private readonly ISupplyService _supplyService;

        public SupplyViewModel(NavigationRootViewModel navigationRootViewModel = null!)
        {
            _navigationRootViewModel = navigationRootViewModel ?? Locator.Current.GetService<NavigationRootViewModel>();
            AddSupply = ReactiveCommand.Create<Unit, SupplyManagerViewModel>(_ => new SupplyManagerViewModel());

            Supplies = new ObservableCollection<Supply>();
            CurrentPage = 0;
            ItemsPerPage = 100;
            SelectedSupply = new Supply();
            SupplyItems = new ObservableCollection<SupplyItem>();

            _supplyService = Locator.Current.GetService<ISupplyService>();
            LoadSupplies = ReactiveCommand.CreateFromTask<LoadParameter, List<Supply>>(param => Task.Run(() => 
             {
                 CurrentPage = param.Skip;
                 ItemsPerPage = param.Take;
                 return _supplyService.GetSupplies(param.Skip, param.Take, param.SearchQuery);
             }));
            LoadSupplies
                .Select(supplies => new ObservableCollection<Supply>(supplies))
                .ToPropertyEx(this, x => x.Supplies);

            this.WhenAnyValue(x => x.SearchQuery)
                .Select(x => new LoadParameter(x, CurrentPage, ItemsPerPage))
                .InvokeCommand(LoadSupplies);

            LoadSupplyItems = ReactiveCommand.CreateFromTask<Supply, List<SupplyItem>>(supply => Task.Run(() => _supplyService.GetSupplyItems(supply.Id)));

            this.WhenAnyValue(x => x.SelectedSupply)
                .Skip(1)
                .Where(x => x != null)
                .InvokeCommand(LoadSupplyItems);
                

            LoadSupplies
                .Where(x => x.Count > 0)
                .Select(x => x.First())
                .Subscribe(x => SelectedSupply = x);
            AddSupply
                 .SelectMany(x => x.Save)
                 .Select(_ => new LoadParameter(SearchQuery, CurrentPage, ItemsPerPage))
                 .InvokeCommand(LoadSupplies);



        }
        public string? UrlPathSegment => nameof(SupplyViewModel);

        public IScreen HostScreen => _navigationRootViewModel;

        [Reactive]
        public int CurrentPage { get; set; }

        [Reactive]
        public int ItemsPerPage { get; set; }

        [Reactive]
        public string? SearchQuery { get; set; }

        [Reactive]
        public Supply SelectedSupply { get; set; }

        public ReactiveCommand<Unit,SupplyManagerViewModel> AddSupply { get; }

        public ReactiveCommand<LoadParameter,List<Supply>> LoadSupplies { get; }

        public ReactiveCommand<Supply, List<SupplyItem>> LoadSupplyItems { get; }

        [ObservableAsProperty]
        public ObservableCollection<Supply> Supplies { get; }

        [ObservableAsProperty]
        public ObservableCollection<SupplyItem> SupplyItems { get; }

    }
}
