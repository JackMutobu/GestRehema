using GestRehema.ViewModels;
using ReactiveUI;
using System.Reactive.Linq;
using System;
using GestRehema.Extensions;
using System.Linq;
using System.Windows.Controls.Primitives;
using GestRehema.Entities;

namespace GestRehema.Views
{
    public class SupplierViewBase : ReactiveUserControl<SupplierViewModel> { }
    /// <summary>
    /// Interaction logic for SupplierView.xaml
    /// </summary>
    public partial class SupplierView : SupplierViewBase
    {
        public SupplierView()
        {
            InitializeComponent();
            ComboLocations.Text = "Toutes";
            ComboSupplierType.Text = "Tous";
            ViewModel = new SupplierViewModel();

            this.WhenActivated(disposable =>
            {
                this.WhenAnyValue(x => x.ViewModel)
                    .BindTo(this, x => x.DataContext);

                this.WhenAnyValue(x => x.ViewModel!.Errors)
               .ObserveOn(RxApp.MainThreadScheduler)
               .Subscribe(value =>
               {
                   TxtError.Visibility = value?.ToVisibility() ?? System.Windows.Visibility.Collapsed;
                   TxtError.Text = value;
               });
                this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.ProgIndicator.Visibility, value => value.ToVisibility());

                this.ViewModel
                    .AddSupplier
                    .Subscribe(x => ShowSupplierManager(x));
                this.ViewModel
                    .UpdateSupplier
                    .Subscribe(x => ShowSupplierManager(x));

                this.ViewModel
                .LoadSuppliers
                .Throttle(TimeSpan.FromMilliseconds(200))
                .Subscribe(_ => RefreshBindings());

                this.DtGridSuppliers
                  .Events().SelectionChanged
                  .Where(x => x.AddedItems.Count > 0)
                  .Select(x => x.AddedItems[0] as Supplier)
                  .Subscribe(x => ViewModel.SelectedSupplier = x);

                this.DtGridSuppliers
                   .Events().SelectionChanged
                   .Where(x => x.AddedItems.Count > 0)
                   .Select(x => x.AddedItems[0] as Supplier)
                   .Where(x => x != null)
                   .Throttle(TimeSpan.FromMilliseconds(200))
                   .Subscribe(x => RefreshBindings());

                this.BtnRefresh
                .Events().Click
                .Select(_ => new LoadParameter(ViewModel.SearchQuery, ViewModel.CurrentPage, ViewModel.ItemPerPage))
                .InvokeCommand(ViewModel.LoadSuppliers);


            });

        }
        private void ShowSupplierManager(SupplierManagerViewModel supplierManagerViewModel)
        {
            var supplierManager = new SupplierManagerView(supplierManagerViewModel);
            supplierManager.ShowDialog();
        }

        private void RefreshBindings()
        {
            Dispatcher.Invoke(() =>
            {
                DtGridSuppliers.ItemsSource = null;
                DtGridSuppliers.ItemsSource = ViewModel!.Suppliers;
                BorderSupplier.DataContext = null;
                BorderSupplier.DataContext = ViewModel;
                BorderWallet.DataContext = null;
                BorderWallet.DataContext = ViewModel;
                ListArticles.ItemsSource = null;
                ListArticles.ItemsSource = ViewModel.Articles;
            });
        }
    }
}
