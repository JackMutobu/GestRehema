using GestRehema.ViewModels;
using ModernWpf.Controls;

namespace GestRehema.Views
{
    /// <summary>
    /// Interaction logic for CashAddExpenseView.xaml
    /// </summary>
    public partial class CashAddExpenseDialog : ContentDialog
    {
        public CashAddExpenseDialog(CashExpensePayementModel cashExpensePayementModel)
        {
            InitializeComponent();
            DataContext = cashExpensePayementModel;
        }
    }
}
