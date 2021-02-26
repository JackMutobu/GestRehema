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
            this.Height = System.Windows.SystemParameters.FullPrimaryScreenHeight;
        }
    }
}
