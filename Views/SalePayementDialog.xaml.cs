using GestRehema.ViewModels;
using ModernWpf.Controls;

namespace GestRehema.Views
{
    /// <summary>
    /// Interaction logic for PayementDialog.xaml
    /// </summary>
    public partial class SalePayementDialog : ContentDialog
    {
        public SalePayementDialog(SaleManagerViewModel saleManagerViewModel)
        {
            InitializeComponent();
            DataContext = saleManagerViewModel;
        }

        public SalePayementDialog(SaleViewModel saleViewModel)
        {
            InitializeComponent();
            DataContext = saleViewModel;
        }
    }
}
