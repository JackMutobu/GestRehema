﻿using GestRehema.ViewModels;
using ReactiveUI;
using Splat;
using GestRehema.Extensions;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Reactive.Threading.Tasks;
using System.Windows.Controls.Primitives;
using GestRehema.Entities;

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

                this.ViewModel.SelectForUpdate
                 .SubscribeOn(RxApp.MainThreadScheduler)
                 .Subscribe(async x => await ShowAddDialog());

                this.ViewModel
                .SelectForDelete
                .SubscribeOn(RxApp.MainThreadScheduler)
                .SelectMany(x => ShowConfirmDialog($"Voulez-vous vraiment supprimé {x.Name}?", x.Id).ToObservable())
                .Subscribe();

                this.BtnAddProduct
                .Events().Click
                .SelectMany(_ => ShowAddDialog().ToObservable())
                .Subscribe();

                this.BtnRefresh
               .Events().Click
               .Select(x => new LoadParameter(ViewModel.SearchQuery, ViewModel.CurrentPage, ViewModel.ItemPerPage))
               .InvokeCommand(ViewModel.LoadArticles);

                this.BtnRefresh
                   .Events().Click
                   .Throttle(TimeSpan.FromMilliseconds(500))
                   .Subscribe(x => RefreshBindings());

                this.DtGridArticle
                  .Events().SelectionChanged
                  .Where(x => x.AddedItems.Count > 0)
                  .Select(x => x.AddedItems[0] as Article)
                  .Subscribe(x => ViewModel.SelectedArticle = x);

                this.DtGridArticle
                   .Events().SelectionChanged
                   .Where(x => x.AddedItems.Count > 0)
                   .Select(x => x.AddedItems[0] as Article)
                   .Where(x => x != null)
                   .Throttle(TimeSpan.FromMilliseconds(200))
                   .Subscribe(x => RefreshBindings());

                this.ViewModel.LoadArticle
                 .Throttle(TimeSpan.FromMilliseconds(200))
                 .Subscribe(x => RefreshBindings());

            });

            TxtSearchArticle.TextChanged += TxtSearchArticle_TextChanged;
        }

        private void TxtSearchArticle_TextChanged(ModernWpf.Controls.AutoSuggestBox sender, ModernWpf.Controls.AutoSuggestBoxTextChangedEventArgs args)
        {

            ViewModel!.LoadArticles.Execute(new LoadParameter(sender.Text, ViewModel.CurrentPage, ViewModel.ItemPerPage)).Subscribe();
        }

        private async Task ShowAddDialog()
        {
            try
            {
                StockAddArticleDialog stockAddArticleDialog = new StockAddArticleDialog(ViewModel!);
                await stockAddArticleDialog.ShowAsync();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                ViewModel!.Errors = ex.Message;
            }
        }

        private async Task ShowConfirmDialog(string message,int articleId)
        {
            try
            {
                var confirmDialog = new ConfirmDialog(message);
                var result = await confirmDialog.ShowAsync();

                if (result == ModernWpf.Controls.ContentDialogResult.Primary)
                    ViewModel!.DeleteArticle.Execute(articleId).Subscribe();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                ViewModel!.Errors = ex.Message;
            }
        }

        private void RefreshBindings()
        {
            Dispatcher.Invoke(() =>
            {
                DtGridArticle.ItemsSource = null;
                DtGridArticle.ItemsSource = ViewModel!.Articles;
                BorderArticleDetails.DataContext = null;
                BorderArticleDetails.DataContext = ViewModel!.SelectedArticle;
                BtnModifyArticle.DataContext = null;
                BtnModifyArticle.DataContext = ViewModel;
                BtnDeleteArticle.DataContext = null;
                BtnDeleteArticle.DataContext = ViewModel;
            });
        }

    }
}
