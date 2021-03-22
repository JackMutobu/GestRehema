using GestRehema.Entities;
using GestRehema.Extensions;
using GestRehema.ViewModels;
using ModernWpf.Controls;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Windows.Controls;

namespace GestRehema.Views
{
    /// <summary>
    /// Interaction logic for NavigationRootView.xaml
    /// </summary>
    public class NavigationRootBase : ReactiveUserControl<NavigationRootViewModel> { }
    public partial class NavigationRootView : NavigationRootBase
    {
        public NavigationRootView()
        {
            InitializeComponent();

            ViewModel = new NavigationRootViewModel();
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.Router, x => x.RoutedViewHost.Router)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm!.Employee!.Fullname, v => v.TxtLogedInUser.Text);
                this.OneWayBind(ViewModel, vm => vm!.Employee!.Fullname, v => v.ProfileAvatar.DisplayName);
                this.OneWayBind(ViewModel, vm => vm!.Employee!.Position, v => v.TxtLogedInUserPosition.Text);

                this.OneWayBind(ViewModel, vm => vm!.Entreprise!.TauxDuJour, v => v.TxtTauxDuJour.Text);
                this.OneWayBind(ViewModel, vm => vm!.Entreprise!.Date, v => v.TxtDateDuJour.Text);
                this.OneWayBind(ViewModel, vm => vm!.Entreprise!.Name, v => v.TxtName.Text);

                this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.ProgIndicator.Visibility, value => value.ToVisibility());

                this.ViewModel.UpdateEntreprise
                .SubscribeOn(RxApp.MainThreadScheduler)
                .Subscribe(x =>
                {
                    TxtDateDuJour.Text = x.Date;
                    TxtTauxDuJour.Text = x.TauxDuJour.ToString();
                    TxtName.Text = x.Name;
                });

            });

            BtnModifyTaux.Click += BtnModifyTaux_Click;
            DateDuJourCalender.SelectedDatesChanged += DateDuJourCalender_SelectedDatesChanged;
            DateDuJourCalender.DisplayDateEnd = DateTime.Now;
            NavView.BackRequested += NavView_BackRequested;
            NavView.SelectionChanged += NavView_SelectionChanged;

        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var navItem = args.SelectedItem as NavigationViewItem;
            if (navItem!.Content.ToString() == "Stock")
                ViewModel!.NavigateToStock.Execute().Subscribe();
            else if(navItem!.Content.ToString() == "Clients")
                ViewModel!.NavigateToCustomer.Execute().Subscribe();
            else if (navItem!.Content.ToString() == "Ventes")
                ViewModel!.NavigateToSale.Execute().Subscribe();
            else if (navItem!.Content.ToString() == "Caisse")
                ViewModel!.NavigateToCash.Execute().Subscribe();
            else if(navItem!.Content.ToString() == "Fournisseurs")
                ViewModel!.NavigateToSupply.Execute().Subscribe();
        }

        private async void NavView_BackRequested(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewBackRequestedEventArgs args)
        {
            await ViewModel!.GoBack.Execute().ToTask();
        }

        private async void DateDuJourCalender_SelectedDatesChanged(object? sender, SelectionChangedEventArgs e)
        {
            ViewModel!.DateDuJour = e.AddedItems.Count > 0 ? (DateTime)e.AddedItems[0]! : ViewModel.DateDuJour;
            await ViewModel.UpdateEntreprise.Execute().ToTask();
            FlyoutCalender.Hide();
        }

        private async void BtnModifyTaux_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                ModifierTauxDialog modifierTauxDialog = new ModifierTauxDialog(ViewModel!);
                await modifierTauxDialog.ShowAsync();
            }
            catch(Exception ex)
            {
                ViewModel!.Errors = ex.Message;
            }

        }
    }
}
