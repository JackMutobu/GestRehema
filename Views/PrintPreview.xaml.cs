using System.Windows;
using System.Windows.Documents;

namespace GestRehema.Views
{
    /// <summary>
    /// Interaction logic for PrintPreview.xaml
    /// </summary>
    public partial class PrintPreview : Window
    {
        public PrintPreview(FixedDocumentSequence document)
        {
            InitializeComponent();
            //PreviewD.Document = document;
        }

        public PrintPreview(BillView billView)
        {
            InitializeComponent();
            this.Content = billView;
            this.Height = SystemParameters.FullPrimaryScreenHeight;
        }

        public PrintPreview(BillBondDeLivraisonView billView)
        {
            InitializeComponent();
            this.Content = billView;
            this.Height = SystemParameters.FullPrimaryScreenHeight;
        }

        public PrintPreview(BillRecuView billView)
        {
            InitializeComponent();
            this.Content = billView;
            this.Height = SystemParameters.FullPrimaryScreenHeight;
        }
    }
}
