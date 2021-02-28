using GestRehema.ViewModels;
using ModernWpf.Controls;

namespace GestRehema.Views
{
    /// <summary>
    /// Interaction logic for PayementDialog.xaml
    /// </summary>
    public partial class SalePayementDialog : ContentDialog
    {
        public SalePayementDialog(BasePayementModel basePayementModel)
        {
            InitializeComponent();
            DataContext = basePayementModel;
        }
    }
}
