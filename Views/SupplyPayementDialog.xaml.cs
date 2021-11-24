using GestRehema.ViewModels;
using ModernWpf.Controls;

namespace GestRehema.Views
{
    /// <summary>
    /// Interaction logic for SupplyPayementDialog.xaml
    /// </summary>
    public partial class SupplyPayementDialog : ContentDialog
    {
        public SupplyPayementDialog(SupplyPayementModel supplyPayementModel)
        {
            InitializeComponent();
            DataContext = supplyPayementModel;
        }
    }
}
