using GestRehema.ViewModels;
using ModernWpf.Controls;

namespace GestRehema.Views
{
    /// <summary>
    /// Interaction logic for ModifierTauxDialog.xaml
    /// </summary>
    public partial class ModifierTauxDialog : ContentDialog
    {
        private NavigationRootViewModel _viewModel;

        public ModifierTauxDialog(NavigationRootViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = viewModel;
        }
    }
}
