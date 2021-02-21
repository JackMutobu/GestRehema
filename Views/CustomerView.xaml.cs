using GestRehema.Extensions;
using GestRehema.ViewModels;
using ReactiveUI;
using Splat;
using System.Reactive.Linq;
using System;
using System.Windows.Controls.Primitives;
using System.Threading.Tasks;
using System.Reactive.Threading.Tasks;

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
    }
}
