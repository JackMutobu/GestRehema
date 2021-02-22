using ModernWpf.Controls;

namespace GestRehema.Views
{
    /// <summary>
    /// Interaction logic for ConfirmDialog.xaml
    /// </summary>
    public partial class ConfirmDialog : ContentDialog
    {
        public ConfirmDialog(string message)
        {
            InitializeComponent();
            TxtMessage.Text = message;
        }
    }
}
