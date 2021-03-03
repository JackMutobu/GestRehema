using GestRehema.Entities;
using ModernWpf.Controls;

namespace GestRehema.Views
{
    /// <summary>
    /// Interaction logic for BillRecuDialog.xaml
    /// </summary>
    public partial class BillRecuDialog : ContentDialog
    {
        public BillRecuDialog(Entreprise entreprise, Sale sale)
        {
            Entreprise = entreprise;
            Sale = sale;
            InitializeComponent();
        }

        public Entreprise Entreprise { get; }

        public Sale Sale { get; }
    }
}
