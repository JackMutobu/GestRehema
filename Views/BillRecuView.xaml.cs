using GestRehema.ViewModels;
using System;
using System.Windows.Controls;

namespace GestRehema.Views
{
    /// <summary>
    /// Interaction logic for BillRecuView.xaml
    /// </summary>
    public partial class BillRecuView : UserControl
    {
        public BillRecuView(BillRecuViewModel billRecuViewModel)
        {
            if (string.IsNullOrEmpty(billRecuViewModel.AmountInLetters))
                throw new ArgumentException("Veuillez spécifier le montant en toutes lettres", nameof(billRecuViewModel.AmountInLetters));
            InitializeComponent();
            DataContext = billRecuViewModel;
        }
    }
}
