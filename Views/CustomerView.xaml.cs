using GestRehema.Extensions;
using GestRehema.ViewModels;
using ReactiveUI;
using Splat;
using System.Reactive.Linq;
using System;
using System.Windows.Controls.Primitives;
using System.Threading.Tasks;
using System.Reactive.Threading.Tasks;
using System.Windows.Controls;
using GestRehema.Entities;
using System.Windows;
using ReactiveUI.Fody.Helpers;
using System.Printing;

namespace GestRehema.Views
{
    public class CustomerViewBase : ReactiveUserControl<CustomerViewModel> { }
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : CustomerViewBase
    {
        public CustomerView()
        {
            InitializeComponent();
            ViewModel = Locator.Current.GetService<CustomerViewModel>() ?? new CustomerViewModel();
            DataContext = ViewModel;

            this.WhenActivated(disposables =>
            {
                Locator.CurrentMutable.RegisterConstant(ViewModel);

                this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.ProgIndicator.Visibility, value => value.ToVisibility());

                this.OneWayBind(ViewModel, vm => vm.Customers, v => v.DtGridClient.ItemsSource);

                this.WhenAnyValue(x => x.ViewModel!.Errors)
                    .SubscribeOn(RxApp.MainThreadScheduler)
               .Subscribe(value =>
               {
                   TxtError.Visibility = value?.ToVisibility() ?? Visibility.Collapsed;
                   TxtError.Text = value;
               });

                this.BtnAddCustomer
                .Events().Click
                .SelectMany(_ => ShowAddDialog().ToObservable())
                .Subscribe();

                this.ViewModel
                .SelectForUpdate
                .SubscribeOn(RxApp.MainThreadScheduler)
                .SelectMany(_ => ShowAddDialog().ToObservable())
                .Subscribe();

                this.ViewModel
                .SelectForDelete
                .SubscribeOn(RxApp.MainThreadScheduler)
                .SelectMany(x => ShowConfirmDialog($"Voulez-vous vraiment supprimé {x.Name}?", x.Id).ToObservable())
                .Subscribe();

                this.TxtSearch
                .Events().TextChanged
                .Select(x => x.Source as TextBox)
                .Select(x => new LoadCustomerParameter(x!.Text, ViewModel.CurrentPage, ViewModel.ItemPerPage, ViewModel.SelectedCustomerType))
                .InvokeCommand(ViewModel.LoadCustomers);

                this.BtnRefresh
                .Events().Click
                .Select(x => new LoadCustomerParameter(ViewModel.SearchQuery, ViewModel.CurrentPage, ViewModel.ItemPerPage,ViewModel.SelectedCustomerType))
                .InvokeCommand(ViewModel.LoadCustomers);

                this.BtnRefresh
                  .Events().Click
                  .Throttle(TimeSpan.FromMilliseconds(500))
                  .Subscribe(x => RefreshBindings());

                this.ComboCustomers
                .Events().SelectionChanged
                .Where(x => x.AddedItems.Count > 0)
                .Select(x => x.AddedItems[0] as string)
                .Subscribe(x => ViewModel.SelectedCustomerType = x);

                this.DtGridClient
                   .Events().SelectionChanged
                   .Where(x => x.AddedItems.Count > 0)
                   .Select(x => x.AddedItems[0] as Customer)
                   .Subscribe(x => ViewModel.SelectedCustomer = x);

                this.DtGridClient
                   .Events().SelectionChanged
                   .Where(x => x.AddedItems.Count > 0)
                   .Select(x => x.AddedItems[0] as Customer)
                   .Where(x => x != null)
                   .Throttle(TimeSpan.FromMilliseconds(200))
                   .Subscribe(x => RefreshBindings());

                this.ViewModel.SaveCustomer
                   .Throttle(TimeSpan.FromMilliseconds(200))
                   .Subscribe(x => RefreshBindings());

                this.BtnDeposit
                .Events().Click
                .SubscribeOn(RxApp.MainThreadScheduler)
                .Subscribe(_ => ShowDepositDialog().ToObservable());

                this.ViewModel.Pay
                .Throttle(TimeSpan.FromMilliseconds(100))
                .Subscribe(_ => RefreshBindings());

                ListPayements
                .Events().SelectionChanged
                .Where(x => x.AddedItems.Count > 0)
                .Select(x => x.AddedItems[0] as Payement)
                .Subscribe(x =>  ViewModel.SelectedPayement = x!);

                ListPayements
               .Events().SelectionChanged
               .Where(x => x.AddedItems.Count > 0)
               .Throttle(TimeSpan.FromMilliseconds(100))
               .Subscribe(_ => RefreshBindings());

                this.ViewModel
                .WhenAnyValue(x => x.Payements)
                .Where(x => x != null)
                .Subscribe(x => BorderPayements.Visibility = x.Count > 0 ? Visibility.Visible : Visibility.Collapsed);

                this.BtnPrint
              .Events().Click
              .ObserveOn(RxApp.MainThreadScheduler)
              .Subscribe(async x =>
              {
                  BillRecuDialog billRecuDialog = new(new BillRecuViewModel(ViewModel.Entreprise, ViewModel.SelectedCustomer ?? throw new ArgumentException("Veuillez séléctionner un client",nameof(ViewModel.SelectedCustomer)), ViewModel.SelectedPayement)); ;
                  await billRecuDialog.ShowAsync();
              });

            });
        }
        private async Task ShowAddDialog()
        {
            try
            {
                CustomerAddDialog dialog = new CustomerAddDialog(ViewModel!);
                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                ViewModel!.Errors = ex.Message;
            }
        }

        private async Task ShowDepositDialog()
        {
            try
            {
                var dialog = new CustomerDepositDialog(ViewModel!);
                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                ViewModel!.Errors = ex.Message;
            }
        }

        private async Task ShowConfirmDialog(string message, int itemId)
        {
            try
            {
                var confirmDialog = new ConfirmDialog(message);
                var result = await confirmDialog.ShowAsync();

                if (result == ModernWpf.Controls.ContentDialogResult.Primary)
                    ViewModel!.Delete.Execute(itemId).Subscribe();
            }
            catch (Exception ex)
            {
                ViewModel!.Errors = ex.Message;
            }
        }

        private void RefreshBindings()
        {
            Dispatcher.Invoke(() =>
            {
                DtGridClient.ItemsSource = null;
                DtGridClient.ItemsSource = ViewModel!.Customers;
                BorderCustomer.DataContext = null;
                BorderCustomer.DataContext = ViewModel;
                BorderWallet.DataContext = null;
                BorderWallet.DataContext = ViewModel;
                BorderPayements.DataContext = null;
                BorderPayements.DataContext = ViewModel.SelectedPayement;
                ListPayements.ItemsSource = null;
                ListPayements.ItemsSource = ViewModel.Payements;
            });
        }
    }
}
