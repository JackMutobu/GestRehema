using GestRehema.Entities;
using System.Windows.Controls;

namespace GestRehema.Views
{
    /// <summary>
    /// Interaction logic for SupplyItemView.xaml
    /// </summary>
    public partial class SupplyItemView : UserControl
    {
        public SupplyItemView(SupplyItem supplyItem)
        {
            InitializeComponent();
            DataContext = supplyItem;
        }
    }
}
