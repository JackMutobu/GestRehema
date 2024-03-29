﻿using GestRehema.Entities;
using GestRehema.Extensions;
using GestRehema.ViewModels;
using ReactiveUI;
using Splat;
using System.Reactive.Linq;
using System.Windows.Controls.Primitives;
using System.Linq;
using System;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Reactive.Threading.Tasks;
using System.Printing;
using System.Windows.Media;
using System.Windows;
using System.Windows.Documents;

namespace GestRehema.Views
{
    public class SaleViewBase : ReactiveUserControl<SaleViewModel> { }
    /// <summary>
    /// Interaction logic for SaleView.xaml
    /// </summary>
    public partial class SaleView : SaleViewBase
    {
        public SaleView()
        {
            InitializeComponent();
            ViewModel = Locator.Current.GetService<SaleViewModel>() ?? new SaleViewModel();
            DataContext = ViewModel;

            this.WhenActivated(disposables =>
            {
                Locator.CurrentMutable.RegisterConstant(ViewModel);

                this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.ProgIndicator.Visibility, value => value.ToVisibility());

                this.OneWayBind(ViewModel, vm => vm.Sales, v => v.DtGridSales.ItemsSource);


                this.DtGridSales
                    .Events().SelectionChanged
                    .Where(x => x.AddedItems.Count > 0)
                    .Select(x => x.AddedItems[0] as Sale)
                    .Where(x => x != null)
                    .Select(x => x!.Id)
                    .InvokeCommand(ViewModel.LoadSale);

                this.DtGridSales
                    .Events().SelectionChanged
                    .Where(x => x.AddedItems.Count > 0)
                    .Select(x => x.AddedItems[0] as Sale)
                    .Where(x => x != null)
                    .Select(x => x!.Id)
                    .InvokeCommand(ViewModel.LoadSale);

                BtnAddSale
                    .Events().Click
                    .SubscribeOn(RxApp.MainThreadScheduler)
                    .Subscribe(_ =>
                    {
                        SaleManager sale = new SaleManager(ViewModel);
                        sale.ShowDialog();
                    },exec => Console.WriteLine(exec.Message));

                this.WhenAnyValue(x => x.ViewModel!.Errors)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(value =>
                {
                    TxtError.Visibility = value?.ToVisibility() ?? System.Windows.Visibility.Collapsed;
                    TxtError.Text = value;
                });

                this.TxtSearch
               .Events().TextChanged
               .Select(x => x.Source as TextBox)
               .Select(x => new LoadParameter(x!.Text, ViewModel.CurrentPage, ViewModel.ItemPerPage))
               .InvokeCommand(ViewModel.LoadSales);

                this.DtGridSales
                    .Events().SelectionChanged
                    .Where(x => x.AddedItems.Count > 0)
                    .Select(x => x.AddedItems[0] as Sale)
                    .Where(x => x != null)
                    .Throttle(TimeSpan.FromMilliseconds(200))
                    .Subscribe(x => RefreshBindings());

                this.BtnRefresh
                .Events().Click
                .Select(_ => new LoadParameter(ViewModel.SearchQuery, ViewModel.CurrentPage, ViewModel.ItemPerPage))
                .InvokeCommand(ViewModel.LoadSales);

                this.BtnRefresh
                    .Events().Click
                    .Throttle(TimeSpan.FromMilliseconds(500))
                    .Subscribe(x => RefreshBindings());

                this.BtnDeliverAll
                    .Events().Click
                    .Select(_ => 
                    ViewModel!.SelectedSale!.Id)
                    .InvokeCommand(ViewModel!.DeliverAll);

                this.ViewModel
                .AddPayement
                .SelectMany(x => ShowPayementDialog(x).ToObservable())
                .Subscribe();

                this.ViewModel
                .AddDelivery
                .SelectMany(_ => ShowDeliveryDialog().ToObservable())
                .Subscribe();

                this.ViewModel.WhenAnyValue(x => x.Debt)
                .DistinctUntilChanged()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => BtnAddPayement.IsEnabled = x > 0);

                this.ViewModel.WhenAnyValue(x => x.CanAddDelivery)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => BtnDeliverAll.IsEnabled = x);

                this.ViewModel.WhenAnyValue(x => x.SaleArticles)
                .Where(x => x!= null && x.Count > 0)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x => RefreshBindings());

                this.BtnPrint
                .Events().Click
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    try
                    {
                        PrintDialog printDialog = new PrintDialog();
                        printDialog.PrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4);
                        PrintCapabilities capabilities = printDialog.PrintQueue
                            .GetPrintCapabilities(printDialog.PrintTicket);

                        BillView billView = new BillView(ViewModel!.SelectedSale!, ViewModel.Entreprise, ViewModel!.SelectedSale!.Customer!, ViewModel.SaleArticles.ToList(), ViewModel.TotalToPay, ViewModel.Debt);
                        printDialog.PrintVisual(billView, $"Imprimer facture {ViewModel!.SelectedSale!.Id}");
                    }
                    catch (Exception ex)
                    {
                        ViewModel!.Errors = ex.Message;
                    }
                });

                this.BtnPrint
                .Events().MouseRightButtonDown
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    try
                    {
                        BillView billView = new BillView(ViewModel!.SelectedSale!, ViewModel.Entreprise, ViewModel!.SelectedSale!.Customer!, ViewModel.SaleArticles.ToList(), ViewModel.TotalToPay, ViewModel.Debt);
                        var printPreview = new PrintPreview(billView);
                        printPreview.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        ViewModel!.Errors = ex.Message;
                    }

                });

                this.BtnPrintDeliveryBill
               .Events().Click
               .ObserveOn(RxApp.MainThreadScheduler)
               .Subscribe(x =>
               {
                   try
                   {
                       PrintDialog printDialog = new PrintDialog();
                       printDialog.PrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4);
                       PrintCapabilities capabilities = printDialog.PrintQueue
                           .GetPrintCapabilities(printDialog.PrintTicket);

                       var billView = new BillBondDeLivraisonView(ViewModel!.SelectedSale!, ViewModel.Entreprise, ViewModel!.SelectedSale!.Customer!, ViewModel.SaleArticles.ToList(), ViewModel.TotalToPay, ViewModel.Debt);
                       printDialog.PrintVisual(billView, $"Imprimer Bon de livraison {ViewModel!.SelectedSale!.Id}");
                   }
                   catch (Exception ex)
                   {
                       ViewModel!.Errors = ex.Message;
                   }
                   
               });

                this.BtnPrintDeliveryBill
                .Events().MouseRightButtonDown
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    try
                    {
                        var billView = new BillBondDeLivraisonView(ViewModel!.SelectedSale!, ViewModel.Entreprise, ViewModel!.SelectedSale!.Customer!, ViewModel.SaleArticles.ToList(), ViewModel.TotalToPay, ViewModel.Debt);
                        var printPreview = new PrintPreview(billView);
                        printPreview.ShowDialog();
                    }
                    catch(Exception ex)
                    {
                        ViewModel!.Errors = ex.Message;
                    }
                });

            });
        }

        private void RefreshBindings()
        {
            Dispatcher.Invoke(() =>
            {
                BorderSelectCustomer.DataContext = null;
                TxtDateOperation.DataContext = null;
                TxtEntrepriseLocation.DataContext = null;
                TxtSaleId.DataContext = null;
                BorderSelectCustomer.DataContext = ViewModel!.SelectedSale!.Customer;
                TxtDateOperation.DataContext = ViewModel!.SelectedSale!;
                TxtEntrepriseLocation.DataContext = ViewModel!.Entreprise;
                TxtSaleId.DataContext = ViewModel!.SelectedSale;
                ListSaleArticles.ItemsSource = null;
                ListSaleArticles.ItemsSource = ViewModel.SaleArticles;
                BorderPayementSummary.DataContext = null;
                BorderPayementSummary.DataContext = ViewModel;
                
            });
        }

        private async Task ShowPayementDialog(SalePayementModel salePayementModel)
        {
            try
            {
                var payementDialog = new SalePayementDialog(salePayementModel);
                await payementDialog.ShowAsync();
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

        private async Task ShowDeliveryDialog()
        {
            try
            {
                var deliveryDialog = new SaleDeliveryDialog(ViewModel!);
                await deliveryDialog.ShowAsync();
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
    }
}
