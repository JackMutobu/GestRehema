using GestRehema.Extensions;
using GestRehema.Services;
using GestRehema.ViewModels;
using Microsoft.Win32;
using ModernWpf.Controls;
using ModernWpf.Controls.Primitives;
using ReactiveUI;
using Splat;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace GestRehema.Views
{
    /// <summary>
    /// Interaction logic for StockAddArticleDialog.xaml
    /// </summary>
    public partial class StockAddArticleDialog : ContentDialog
    {
        private readonly StockViewModel _viewModel;

        public StockAddArticleDialog(StockViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = viewModel;
            ImgProduct.MouseDown += ImgProduct_MouseDown;

            viewModel
                .SaveArticle
                .IsExecuting
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => ProgIndicator.Visibility = x.ToVisibility());

            ComboCategories.ItemsSource = viewModel.Categories;

            this.TxtConditionement
               .Events().TextChanged
               .Select(x => x.Source as TextBox)
               .Select(x => x.Text)
               .ObserveOn(RxApp.MainThreadScheduler)
               .Subscribe(x =>
               {
                   TxtQtyConditionemement.SetValue(ControlHelper.HeaderProperty, $"Qté par Conditionement en {TxtUnitOfMeasure.Text}/{x}:");
                   TxtPrixDAchat.SetValue(ControlHelper.HeaderProperty, $"Prix d'achat par {x} en $:");
                   TxtPrixDeVente.SetValue(ControlHelper.HeaderProperty, $"Prix de vente par {x} en $:");
                   TxtQteEnStock.SetValue(ControlHelper.HeaderProperty, $"Qté en Stock en nombre de {x}:");
               });
        }

        private void ImgProduct_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    Uri fileUri = new Uri(openFileDialog.FileName);
                    var image = new BitmapImage(fileUri);
                    var fileService = Locator.Current.GetService<IFileService>();

                    var imageUrl = fileService.SaveImage(image.ToBitmap(), $"ProductImages/{fileUri.AbsolutePath.Split("/").Last()}");

                    var imagePath = $"{Environment.CurrentDirectory.Replace("\\", "/")}{imageUrl}";
                    _viewModel.ImageUrl = imagePath;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }


    }
}
