using GestRehema.ViewModels;
using ModernWpf.Controls;
using System.Printing;
using System.Windows.Controls;

namespace GestRehema.Views
{
    /// <summary>
    /// Interaction logic for BillRecuDialog.xaml
    /// </summary>
    public partial class BillRecuDialog : ContentDialog
    {
        private readonly BillRecuViewModel _billRecuViewModel;

        public BillRecuDialog(BillRecuViewModel billRecuViewModel)
        {
            _billRecuViewModel = billRecuViewModel;
            DataContext = billRecuViewModel;
            PrimaryButtonClick += BillRecuDialog_PrimaryButtonClick;
            InitializeComponent();
        }

        private void BillRecuDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var billView = new BillRecuView(_billRecuViewModel);
            if (CheckVoirRecu.IsChecked == true)
            {
                var printPreview = new PrintPreview(billView);
                printPreview.ShowDialog();
            }

            PrintDialog printDialog = new();
            printDialog.PrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4);
            printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);

            
            printDialog.PrintVisual(billView, $"Impression du recu");
        }
    }
}
