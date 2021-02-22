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
using GestRehema.Events;

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

                //MessageBus.Current.Listen<string>(DataGridEvents.CustomersUpdated)
                //.ObserveOn(RxApp.MainThreadScheduler)
                //.Subscribe(x => 
                //{
                //    var message = x;
                //});

                //this.ViewModel.WhenAnyValue(x => x.Customers)
                //.Subscribe(x =>
                //{
                //    var test = x;
                //});



                this.WhenAnyValue(x => x.ViewModel!.Errors)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(value =>
                {
                    TxtError.Visibility = value?.ToVisibility() ?? System.Windows.Visibility.Collapsed;
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
                .Select(x => new LoadParameter(x!.Text, ViewModel.CurrentPage, ViewModel.ItemPerPage))
                .InvokeCommand(ViewModel.LoadCustomers);

                this.BtnRefresh
                .Events().Click
                .Select(x => new LoadParameter(ViewModel.SearchQuery, ViewModel.CurrentPage, ViewModel.ItemPerPage))
                .InvokeCommand(ViewModel.LoadCustomers);

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
    }
}
