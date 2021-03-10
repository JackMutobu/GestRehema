using GestRehema.ViewModels;
using ModernWpf.Controls;

namespace GestRehema.Views
{
    /// <summary>
    /// Interaction logic for ModifySellingPriceDialog.xaml
    /// </summary>
    public partial class SaleUnitSellingPriceDialog : ContentDialog
    {
        public SaleUnitSellingPriceDialog(SaleManagerViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            this.Title = $"Modifier le prix de vente de {viewModel!.SelectedSaleCartItem!.Article.Name}";
            PrimaryButtonClick += SaleUnitSellingPriceDialog_PrimaryButtonClick;
        }

        private void SaleUnitSellingPriceDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            this.Hide();
        }
    }
}
