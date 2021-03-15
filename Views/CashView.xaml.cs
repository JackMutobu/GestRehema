using GestRehema.ViewModels;
using ReactiveUI;
using Splat;
using System.Reactive.Linq;
using System;
using GestRehema.Extensions;
using System.Windows;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Reactive.Threading.Tasks;
using GestRehema.Entities;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

namespace GestRehema.Views
{
    public class CashViewBase : ReactiveUserControl<CashViewModel> { }
    /// <summary>
    /// Interaction logic for CashView.xaml
    /// </summary>
    public partial class CashView : CashViewBase
    {
        public CashView()
        {
            InitializeComponent();
            ViewModel = Locator.Current.GetService<CashViewModel>() ?? new CashViewModel();
            DataContext = ViewModel;
            this.WhenActivated(disposables =>
            {
                Locator.CurrentMutable.RegisterConstant(ViewModel);

                this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.ProgIndicator.Visibility, value => value.ToVisibility());

                this.BindCommand(ViewModel, vm => vm.AddExpense, v => v.BtnAddExpense)
                        .DisposeWith(disposables);

                this.WhenAnyValue(x => x.ViewModel!.Errors)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(value =>
                    {
                        TxtError.Visibility = value?.ToVisibility() ?? Visibility.Collapsed;
                        TxtError.Text = value;
                    });

                this.ViewModel
                .AddExpense
                .SelectMany(x => ShowCashDialog(x).ToObservable())
                .Subscribe();

                this.ViewModel
                .AddExpense
                .SelectMany(x => x.Validate)
                .Where(x => !x.IsValid)
                .Subscribe(x => MessageBox.Show(x.Message));

                this.DtGridExpenses
                  .Events().SelectionChanged
                  .Where(x => x.AddedItems.Count > 0)
                  .Select(x => x.AddedItems[0] as Expense)
                  .Subscribe(x => ViewModel.SelectedExpense = x);

                this.DtGridExpenses
                   .Events().SelectionChanged
                   .Where(x => x.AddedItems.Count > 0)
                   .Select(x => x.AddedItems[0] as Expense)
                   .Where(x => x != null)
                   .Throttle(TimeSpan.FromMilliseconds(200))
                   .Subscribe(x => RefreshBindings());

                this.ViewModel.SaveExpense
                 .Throttle(TimeSpan.FromMilliseconds(200))
                 .Subscribe(x => RefreshBindings());

                this.ListCategories
                 .Events().SelectionChanged
                 .Where(x => x.AddedItems.Count > 0)
                 .Select(x => x.AddedItems[0] as string)
                 .Subscribe(x => ViewModel.SelectedCategory = x);

                this.ListCategories
                   .Events().SelectionChanged
                   .Where(x => x.AddedItems.Count > 0)
                   .Select(x => x.AddedItems[0] as string)
                   .Where(x => x != null)
                   .Throttle(TimeSpan.FromMilliseconds(200))
                   .Subscribe(x => RefreshBindings());

                DtPickerDate.DisplayDateEnd = DateTime.Now;

                Observable.FromEventPattern<SelectionChangedEventArgs>(DtPickerDate, "SelectedDateChanged")
                .Select(x => x.Sender as DatePicker)
                .Select(x => x.SelectedDate)
                .Subscribe(x => ViewModel.SelectedDate = x);

                Observable.FromEventPattern<SelectionChangedEventArgs>(DtPickerDate, "SelectedDateChanged")
                .Select(x => x.Sender as DatePicker)
                .Select(x => x.SelectedDate)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .Subscribe(x => RefreshBindings());

                BtnRefresh
                .Events().Click
                .Select(_ => 
                {
                    ViewModel.SelectedDate = null;
                    return new LoadParameter("", ViewModel.CurrentPage, ViewModel.ItemPerPage);
                })
                .InvokeCommand(ViewModel.LoadExpenses);

                BtnRefresh
                    .Events().Click
                    .Throttle(TimeSpan.FromMilliseconds(200))
                    .Subscribe(x => RefreshBindings());

                ViewModel
                .SaveExpense
                .Throttle(TimeSpan.FromMilliseconds(200))
                .Subscribe(x => RefreshBindings());

            });
        }

        private async Task ShowCashDialog(CashExpensePayementModel cashExpensePayementModel)
        {
            try
            {
                var dialog = new CashAddExpenseDialog(cashExpensePayementModel);
                await dialog.ShowAsync();
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
                DtGridExpenses.ItemsSource = null;
                DtGridExpenses.ItemsSource = ViewModel!.Expenses;
                BorderExpenseDetails.DataContext = null;
                BorderExpenseDetails.DataContext = ViewModel.SelectedExpense;
                TxtExpense.DataContext = null;
                TxtExpense.DataContext = ViewModel;
                ListCategories.ItemsSource = null;
                ListCategories.ItemsSource = ViewModel.Categories;
                BorderCash.DataContext = null;
                BorderCash.DataContext = ViewModel;
            });
        }
    }
}
