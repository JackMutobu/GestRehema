using GestRehema.Extensions;
using GestRehema.Services;
using GestRehema.ViewModels;
using Microsoft.Win32;
using ModernWpf.Controls;
using ReactiveUI;
using Splat;
using System;
using System.Linq;
using System.Reactive.Linq;
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
                .SubscribeOn(RxApp.MainThreadScheduler)
                .Subscribe(x => ProgIndicator.Visibility = x.ToVisibility());
        }

        private void ImgProduct_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Uri fileUri = new Uri(openFileDialog.FileName);
                var image = new BitmapImage(fileUri);
                var fileService = Locator.Current.GetService<IFileService>();

                var imageUrl = fileService.SaveImage(image.ToBitmap(), $"ProductImages/{fileUri.AbsolutePath.Split("/").Last()}");

                var imagePath = $"{Environment.CurrentDirectory.Replace("\\","/")}{imageUrl}";
                _viewModel.ImageUrl = imagePath;
            }
        }


    }
}
