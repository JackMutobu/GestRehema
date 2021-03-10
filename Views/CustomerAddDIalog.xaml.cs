using GestRehema.Contants;
using GestRehema.Extensions;
using GestRehema.Services;
using GestRehema.ViewModels;
using Microsoft.Win32;
using ModernWpf.Controls;
using Splat;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Reactive.Linq;
using ReactiveUI;

namespace GestRehema.Views
{
    /// <summary>
    /// Interaction logic for CustomerAddDialog.xaml
    /// </summary>
    public partial class CustomerAddDialog : ContentDialog
    {
        private readonly CustomerViewModel _viewModel;

        public CustomerAddDialog(CustomerViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            _viewModel = viewModel;

            ImgItem.MouseDown += ImgItem_MouseDown;

            this.BtnModifyImage
               .Events().Click
               .ObserveOn(RxApp.MainThreadScheduler)
               .Subscribe(_ => UploadImages());
        }

        private void ImgItem_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UploadImages();

        }

        private void UploadImages()
        {
            try
            {
                OpenFileDialog openFileDialog = new();
                if (openFileDialog.ShowDialog() == true)
                {
                    Uri fileUri = new(openFileDialog.FileName);
                    var image = new BitmapImage(fileUri);
                    var fileService = Locator.Current.GetService<IFileService>();

                    var imageUrl = fileService.SaveImage(image.ToBitmap(), FilePath.ProfileImage, fileUri.AbsolutePath.Split("/").Last());
                    _viewModel.ImageUrl = imageUrl;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
