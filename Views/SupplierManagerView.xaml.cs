using GestRehema.ViewModels;
using ReactiveUI;
using System.Reactive.Linq;
using System;
using GestRehema.Extensions;
using System.Collections.Generic;

namespace GestRehema.Views
{
    public class SupplierManagerViewBase : ReactiveWindow<SupplierManagerViewModel> { }
    /// <summary>
    /// Interaction logic for SupplierManagerView.xaml
    /// </summary>
    public partial class SupplierManagerView : SupplierManagerViewBase
    {
        public SupplierManagerView() { }

        public SupplierManagerView(List<string> locations)
        {
            InitializeComponent();
            ViewModel = new SupplierManagerViewModel(locations);

            this.WhenActivated(disposable =>
            {
                this.OneWayBind(this.ViewModel, vm => vm.Articles, v => v.ListArticles.ItemsSource);
                this.OneWayBind(this.ViewModel, vm => vm.Locations, v => v.ComboLocations.ItemsSource);
                this.Bind(this.ViewModel, vm => vm.SearchQuery, v => v.TxtSearchArticle.Text);
                this.WhenAnyValue(x => x.ViewModel!.Errors)
               .ObserveOn(RxApp.MainThreadScheduler)
               .Subscribe(value =>
               {
                   TxtError.Visibility = value?.ToVisibility() ?? System.Windows.Visibility.Collapsed;
                   TxtError.Text = value;
               });

                this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.ProgIndicator.Visibility, value => value.ToVisibility());
            });
        }
    }
}
