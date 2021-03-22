using GestRehema.Validations;
using ReactiveUI;
using Splat;
using System.Collections.Generic;

namespace GestRehema.ViewModels
{
    public class SupplierViewModel : ViewModelBaseWithValidation, IRoutableViewModel
    {
        private readonly NavigationRootViewModel _navigationRootViewModel;

        public SupplierViewModel(NavigationRootViewModel navigationRootViewModel = null!): base(new SupplierValidation())
        {
            _navigationRootViewModel = navigationRootViewModel ?? Locator.Current.GetService<NavigationRootViewModel>();
            Locations = new List<string>
            {
                "Bunia",
                "Butembo",
                "Kampala",
                "Chine"
            };
        }

        public string? UrlPathSegment => nameof(SupplierViewModel);

        public IScreen HostScreen => _navigationRootViewModel;

        public List<string> Locations { get; }
    }
}
