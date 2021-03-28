using GestRehema.ViewModels;
using ReactiveUI;
using System.Reactive.Linq;
using System;
using GestRehema.Extensions;
using System.Windows.Media.Imaging;
using Splat;
using GestRehema.Services;
using Microsoft.Win32;
using GestRehema.Contants;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Reactive.Disposables;

namespace GestRehema.Views
{
    public class SupplierManagerViewBase : ReactiveWindow<SupplierManagerViewModel> { }
    /// <summary>
    /// Interaction logic for SupplierManagerView.xaml
    /// </summary>
    public partial class SupplierManagerView : SupplierManagerViewBase
    {
        public SupplierManagerView() { }

        public SupplierManagerView(SupplierManagerViewModel supplierManagerViewModel)
        {
            InitializeComponent();
            ViewModel = supplierManagerViewModel;
            DataContext = supplierManagerViewModel;

            this.WhenActivated(disposable =>
            {
                this.OneWayBind(this.ViewModel, vm => vm.Articles, v => v.ListArticles.ItemsSource)
                    .DisposeWith(disposable); ;
                this.BindCommand(ViewModel, vm => vm.SaveSupplier, v => v.BtnSave)
                    .DisposeWith(disposable);

                this.WhenAnyValue(x => x.ViewModel!.Errors)
               .ObserveOn(RxApp.MainThreadScheduler)
               .Subscribe(value =>
               {
                   TxtError.Visibility = value?.ToVisibility() ?? System.Windows.Visibility.Collapsed;
                   TxtError.Text = value;
               });

                this.ViewModel
                    .SaveSupplier
                    .Subscribe(_ => this.Close());

                this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.ProgIndicator.Visibility, value => value.ToVisibility());
            });

            this.BtnModifyImage
              .Events().Click
              .ObserveOn(RxApp.MainThreadScheduler)
              .Subscribe(_ => UploadImages());

            this.ImgItem
            .Events().MouseDown
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ => UploadImages());
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

                    var imageUrl = fileService.SaveImage(image.ToBitmap(), FilePath.SupplierImage, fileUri.AbsolutePath.Split("/").Last());
                    ViewModel!.ImageUrl = imageUrl;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
