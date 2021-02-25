using GestRehema.ViewModels;
using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;

namespace GestRehema.Views
{
    /// <summary>
    /// Interaction logic for SaleDeliveryDialog.xaml
    /// </summary>
    public partial class SaleDeliveryDialog : ContentDialog
    {
        public SaleDeliveryDialog(SaleViewModel saleViewModel)
        {
            InitializeComponent();
            DataContext = saleViewModel;
            TxtQtyInConditionement.SetValue(ControlHelper.HeaderProperty, $"Qté  en {saleViewModel.DeliveryModel.Article.Conditionement}:");
            TxtQtyInUnitOfMeasure.SetValue(ControlHelper.HeaderProperty, $"Qté  en {saleViewModel.DeliveryModel.Article.UnitOfMeasure}:");

        }
    }
}
