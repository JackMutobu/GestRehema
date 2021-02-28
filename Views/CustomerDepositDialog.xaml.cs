using GestRehema.ViewModels;
using ModernWpf.Controls;

namespace GestRehema.Views
{
    /// <summary>
    /// Interaction logic for CustomerDepositDialog.xaml
    /// </summary>
    public partial class CustomerDepositDialog : ContentDialog
    {
        public CustomerDepositDialog(CustomerViewModel customerViewModel)
        {
            InitializeComponent();
            DataContext = customerViewModel;

        }
    }
}
