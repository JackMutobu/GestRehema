using GestRehema.ViewModels;
using ModernWpf.Controls;

namespace GestRehema.Views
{
    /// <summary>
    /// Interaction logic for SupplyUnitBuyingPriceDialog.xaml
    /// </summary>
    public partial class SupplyUnitBuyingPriceDialog : ContentDialog
    {
        public SupplyUnitBuyingPriceDialog(SupplyCartItem saleCartItem)
        {
            InitializeComponent();
            DataContext = saleCartItem;
            this.Title = $"Modifier le prix d'achat de {saleCartItem.Article.Name}";
            PrimaryButtonClick += SupplyUnitBuyingPriceDialog_PrimaryButtonClick; ;
        }

        private void SupplyUnitBuyingPriceDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
        }
    }
}
