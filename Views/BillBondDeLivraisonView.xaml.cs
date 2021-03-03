using GestRehema.Entities;
using GestRehema.ViewModels;
using System.Collections.Generic;
using System.Windows.Controls;

namespace GestRehema.Views
{
    /// <summary>
    /// Interaction logic for BillBondDeLivraisonView.xaml
    /// </summary>
    public partial class BillBondDeLivraisonView : UserControl
    {
        public BillBondDeLivraisonView(Sale sale, Entreprise entreprise, Customer customer, List<SaleArticleItem> saleArticles, decimal cartSubTotal, decimal debt)
        {
            Entreprise = entreprise;
            Customer = customer;
            Sale = sale;
            SaleArticles = saleArticles;
            CartSubTotal = cartSubTotal;
            Debt = debt;
            InitializeComponent();
            DataContext = this;

        }

        public Entreprise Entreprise { get; }

        public Customer Customer { get; }

        public Sale Sale { get; }

        public decimal CartSubTotal { get; }

        public decimal Debt { get; }

        public List<SaleArticleItem> SaleArticles { get; }
    }
}
