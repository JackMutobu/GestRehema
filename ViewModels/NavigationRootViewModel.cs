using GestRehema.Entities;
using GestRehema.Services;
using GestRehema.Validations;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace GestRehema.ViewModels
{
    public class NavigationRootViewModel : ViewModelBaseWithValidation, IScreen
    {
        private readonly IEntrepriseService _entrepriseService;
        public NavigationRootViewModel() : base(new EntrepriseValidator())
        {
            _entrepriseService = Locator.Current.GetService<IEntrepriseService>();
            Locator.CurrentMutable.RegisterConstant(this);
            TauxDuJour = Entreprise.TauxDuJour;
            DateDuJour = Entreprise.DateDuJour;
            Title = "Sélectionner un Menu";

            Router = new RoutingState();

            UpdateEntreprise = ReactiveCommand.CreateFromTask(() =>
            {
                Entreprise.TauxDuJour = TauxDuJour;
                Entreprise.DateDuJour = DateDuJour;

                return Task.Run(() => _entrepriseService.UpdateEntrepriseInfo(Entreprise));
            });
            UpdateEntreprise
                .IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);
            UpdateEntreprise.ThrownExceptions
                .Select(x => x.Message)
                .Subscribe(x => Errors = x);
            UpdateEntreprise
                .Subscribe(x =>
                {
                    Entreprise = x;
                    User!.Employee!.Entreprise = x;
                    Locator.CurrentMutable.RegisterConstant(User);
                });

            GoBack = Router.NavigateBack;
            NavigateToStock = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new StockViewModel(this)));

            NavigateToCustomer = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new CustomerViewModel(this)));

            NavigateToSale = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new SaleViewModel(this)));

            Initialize();
        }

        [Reactive]
        public decimal TauxDuJour { get; set; }

        [Reactive]
        public DateTime DateDuJour { get; set; }

        public RoutingState Router { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToStock { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToCustomer { get; }

        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToSale { get; }

        public ReactiveCommand<Unit, Unit> GoBack { get; }

        public ReactiveCommand<Unit,Entreprise> UpdateEntreprise { get; }


        private void Initialize()
        {
            DateDuJour = DateTime.Now;
            UpdateEntreprise.Execute().Subscribe();
        }
    }
}
