using GestRehema.Extensions;
using GestRehema.Services;
using GestRehema.ViewModels;
using Microsoft.Win32;
using ModernWpf.Controls;
using Splat;
using System;
using System.Linq;
using System.Windows.Media.Imaging;

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
        }

        private void ImgItem_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    Uri fileUri = new Uri(openFileDialog.FileName);
                    var image = new BitmapImage(fileUri);
                    var fileService = Locator.Current.GetService<IFileService>();

                    var imageUrl = fileService.SaveImage(image.ToBitmap(), $"ProfileImages/{fileUri.AbsolutePath.Split("/").Last()}");

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
