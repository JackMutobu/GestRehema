using GestRehema.ViewModels;
using ReactiveUI;
using Splat;
using GestRehema.Extensions;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace GestRehema.Views
{
    public class StockViewBase : ReactiveUserControl<StockViewModel> { }
    /// <summary>
    /// Interaction logic for StockView.xaml
    /// </summary>
    public partial class StockView : StockViewBase
    {
        public StockView()
        {
            InitializeComponent();
            ViewModel = Locator.Current.GetService<StockViewModel>() ?? new StockViewModel();
            DataContext = ViewModel;
            this.WhenActivated(disposables =>
            {
                Locator.CurrentMutable.RegisterConstant(ViewModel);

                this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.ProgIndicator.Visibility, value => value.ToVisibility());

                this.OneWayBind(ViewModel, vm => vm.Articles, v => v.DtGridArticle.ItemsSource);

                this.WhenAnyValue(x => x.ViewModel!.Errors)
                .SubscribeOn(RxApp.MainThreadScheduler)
                .Subscribe(value =>
                {
                    TxtError.Visibility = value?.ToVisibility() ?? System.Windows.Visibility.Collapsed;
                    TxtError.Text = value;
                });

                //this.ViewModel
                //.LoadArticles
                //.Subscribe(articles =>
                //{
                //    DtGridArticle.ItemsSource = articles;
                //    DtGridArticle.Items.Refresh();
                //});


                this.ViewModel.SelectForUpdate
                 .SubscribeOn(RxApp.MainThreadScheduler)
                 .Subscribe(async x => await ShowAddDialog());

            });

            BtnAddProduct.Click += BtnAddProduct_Click;
            TxtSearchArticle.TextChanged += TxtSearchArticle_TextChanged;
        }

        private void TxtSearchArticle_TextChanged(ModernWpf.Controls.AutoSuggestBox sender, ModernWpf.Controls.AutoSuggestBoxTextChangedEventArgs args)
        {
            ViewModel!.LoadArticles.Execute(new LoadParameter(sender.Text, ViewModel.CurrentPage, ViewModel.ItemPerPage)).Subscribe();
        }

        private async void BtnAddProduct_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await ShowAddDialog();
        }

        private async Task ShowAddDialog()
        {
            try
            {
                StockAddArticleDialog stockAddArticleDialog = new StockAddArticleDialog(ViewModel!);
                await stockAddArticleDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                ViewModel!.Errors = ex.Message;
            }
        }
    }
}
