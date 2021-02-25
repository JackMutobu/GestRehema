using GestRehema.Entities;
using GestRehema.Extensions;
using GestRehema.ViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows;

namespace GestRehema.Views
{
    public class SaleManagerBase: ReactiveWindow<SaleManagerViewModel> { }
    /// <summary>
    /// Interaction logic for SaleManager.xaml
    /// </summary>
    public partial class SaleManager : SaleManagerBase
    {
        public SaleManager() { }

        public SaleManager(SaleViewModel saleViewModel)
        {
            InitializeComponent();
            ViewModel = Locator.Current.GetService<SaleManagerViewModel>() ?? new SaleManagerViewModel(saleViewModel);
            DataContext = ViewModel;

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.ProgIndicator.Visibility, value => value.ToVisibility());

                this.OneWayBind(ViewModel, vm => vm.Articles, v => v.ListArticles.ItemsSource)
                .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.Customers, v => v.ListCustomers.ItemsSource)
                .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.CartItems, v => v.ListProduits.ItemsSource)
                .DisposeWith(disposables);

                this.TxtSearchArticle
                .Events().TextChanged
                .Select(x => x.Source as TextBox)
                .Select(x => new LoadParameter(x!.Text, ViewModel.CurrentArticlePage, ViewModel.ArticleItemsPerPage))
                .InvokeCommand(ViewModel.LoadArticles);

                this.TxtSearchCustomer
                .Events().TextChanged
                .Select(x => x.Source as TextBox)
                .Select(x => new LoadParameter(x!.Text, ViewModel.CurrentCustomerPage, ViewModel.CustomerItemsPerPage))
                .InvokeCommand(ViewModel.LoadCustomers);

                this.BtnAddCustomer
                .Events().Click
                .SelectMany(_ => ShowAddCustomerDialog().ToObservable())
                .Subscribe();

                this.ListCustomers
                .Events().SelectionChanged
                .Where(x => x.AddedItems.Count> 0)
                .Select(x => x.AddedItems[0] as Customer)
                .ToPropertyEx(this.ViewModel, x => x.SelectedCustomer);

                this.BindCommand(ViewModel, vm => vm.Charge, v => v.BtnFacture)
               .DisposeWith(disposables);

                this.ViewModel
                .Validate
                .Where(x => string.IsNullOrEmpty(x))
                .SelectMany(_ => ShowPayementDialog().ToObservable())
                .Subscribe();

                this.WhenAnyValue(x => x.ViewModel!.Errors)
                .SubscribeOn(RxApp.MainThreadScheduler)
                .Subscribe(value =>
                {
                    TxtError.Visibility = value?.ToVisibility() ?? Visibility.Collapsed;
                    TxtError.Text = value;
                });

                this
                .ViewModel
                .Pay
                .Subscribe(_ => this.Close());

            });
        }

        private async Task ShowAddCustomerDialog()
        {
            try
            {
                var viewModel = Locator.Current.GetService<CustomerViewModel>() ?? new CustomerViewModel();
                viewModel
                    .SaveCustomer
                    .Select(x => new LoadParameter("", ViewModel!.CurrentArticlePage, ViewModel.ArticleItemsPerPage))
                    .InvokeCommand(ViewModel!.LoadCustomers);

                CustomerAddDialog dialog = new CustomerAddDialog(viewModel);
                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                ViewModel!.Errors = ex.Message;
            }
        }

        private async Task ShowPayementDialog()
        {
            var payementDialog = new SalePayementDialog(ViewModel!);
            await payementDialog.ShowAsync();
        }
    }
}
