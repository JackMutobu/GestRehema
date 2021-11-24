using GestRehema.Extensions;
using GestRehema.ViewModels;
using ReactiveUI;
using System.Reactive.Linq;
using System.Windows;
using System;
using System.Threading.Tasks;
using System.Reactive.Disposables;

namespace GestRehema.Views
{
    public class SupplyManagerViewBase : ReactiveWindow<SupplyManagerViewModel> { }
    /// <summary>
    /// Interaction logic for SupplyManagerView.xaml
    /// </summary>
    public partial class SupplyManagerView : SupplyManagerViewBase
    {
        public SupplyManagerView() { }

        public SupplyManagerView(SupplyManagerViewModel supplierManagerViewModel)
        {
            InitializeComponent();
            ViewModel = supplierManagerViewModel;
            DataContext = ViewModel;
            this.Height = SystemParameters.FullPrimaryScreenHeight;

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.ProgIndicator.Visibility, value => value.ToVisibility());
                this.WhenAnyValue(x => x.ViewModel!.Errors)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(value =>
                {
                    TxtError.Visibility = value?.ToVisibility() ?? Visibility.Collapsed;
                    TxtError.Text = value;
                });

                this.ViewModel.AddToCart
                    .SelectMany(item => item.UpdateBuyingPrice)
                    .Subscribe(async item => await ShowBuyingUnitPirceDialog(item));

                this.ViewModel.Pay
                 .Subscribe(async item => await ShowPayementDialog(item));

                this.ViewModel.Save
                .Subscribe(_ => this.Close());
            });
        }

        private async Task ShowBuyingUnitPirceDialog(SupplyCartItem item)
        {
            try
            {
                var dialog = new SupplyUnitBuyingPriceDialog(item);
                await dialog.ShowAsync();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                ViewModel!.Errors = ex.Message;
            }
        }

        private async Task ShowPayementDialog(SupplyPayementModel payementModel)
        {
            try
            {
                var payementDialog = new SupplyPayementDialog(payementModel);
                await payementDialog.ShowAsync();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                ViewModel!.Errors = ex.Message;
            }
        }
    }
}
