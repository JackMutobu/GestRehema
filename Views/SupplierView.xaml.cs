using GestRehema.ViewModels;
using ReactiveUI;

namespace GestRehema.Views
{
    public class SupplierViewBase : ReactiveUserControl<SupplierViewModel> { }
    /// <summary>
    /// Interaction logic for SupplierView.xaml
    /// </summary>
    public partial class SupplierView : SupplierViewBase
    {
        public SupplierView()
        {
            InitializeComponent();
            ViewModel = new SupplierViewModel();
            BtnAddSupplier.Click += BtnAddSupplier_Click;
        }

        private void BtnAddSupplier_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var supplierManager = new SupplierManagerView(ViewModel.Locations);
            supplierManager.ShowDialog();
        }
    }
}
