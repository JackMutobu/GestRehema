using GestRehema.Entities;
using GestRehema.Extensions;
using GestRehema.ViewModels;
using ReactiveUI;
using Splat;
using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace GestRehema.Views
{
    public class SuppliyViewBase : ReactiveUserControl<SupplyViewModel> { }
    /// <summary>
    /// Interaction logic for SupplyView.xaml
    /// </summary>
    public partial class SupplyView : SuppliyViewBase
    {
        public SupplyView()
        {
            InitializeComponent();
            ViewModel = Locator.Current.GetService<SupplyViewModel>() ?? new SupplyViewModel();
            DataContext = ViewModel;
            this.WhenActivated(disposable =>
            {
                Locator.CurrentMutable.RegisterConstant(ViewModel);

                this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.ProgIndicator.Visibility, value => value.ToVisibility());
                this.WhenAnyValue(x => x.ViewModel!.Errors)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(value =>
                {
                    TxtError.Visibility = value?.ToVisibility() ?? System.Windows.Visibility.Collapsed;
                    TxtError.Text = value;
                });


                this.DtGridSupplies
                 .Events().SelectionChanged
                 .Where(x => x.AddedItems.Count > 0)
                 .Select(x => x.AddedItems[0] as Supply)
                 .Subscribe(x => ViewModel.SelectedSupply = x);

                this.DtGridSupplies
                   .Events().SelectionChanged
                   .Where(x => x.AddedItems.Count > 0)
                   .Select(x => x.AddedItems[0] as Supplier)
                   .Where(x => x != null)
                   .Throttle(TimeSpan.FromMilliseconds(200))
                   .Subscribe(x => RefreshBindings());

                this.ViewModel
                .LoadSupplyItems
                .Subscribe(items =>
                {
                    TabSupplyItems.Items.Clear();
                    items.ForEach(i => TabSupplyItems.Items.Add(CreateNewTab(i)));
                    TabSupplyItems.SelectedIndex = 0;
                });

                this.BtnRefresh
                .Events().Click
                .Select(_ => new LoadParameter(ViewModel.SearchQuery, ViewModel.CurrentPage, ViewModel.ItemsPerPage))
                .InvokeCommand(ViewModel.LoadSupplies);

            });


            this.ViewModel.AddSupply
                  .Subscribe(vm =>
                  {
                      SupplyManagerView supplyManagerView = new(vm);
                      supplyManagerView.ShowDialog();
                  });
        }

        private void RefreshBindings()
        {
            Dispatcher.Invoke(() =>
            {
                DtGridSupplies.ItemsSource = null;
                DtGridSupplies.ItemsSource = ViewModel!.Supplies;
            });
        }

        private TabItem CreateNewTab(SupplyItem item)
        {
            TabItem newItem = new TabItem
            {
                Header = $"Approvisionnement {item.Id}"
            };
            newItem.Content = new SupplyItemView(item);

            return newItem;
        }
    }
}
