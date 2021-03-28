using GestRehema.Entities;
using GestRehema.Services;
using GestRehema.Validations;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace GestRehema.ViewModels
{
    public class CashViewModel : ViewModelBaseWithValidation,IRoutableViewModel
    {
        private readonly NavigationRootViewModel _navigationRootViewModel;
        private readonly IExpenseService _expenseService;
        private readonly IWalletService _walletService;

        public CashViewModel(NavigationRootViewModel navigationRootViewModel = null!) : base(new ExpenseValidation())
        {
            _navigationRootViewModel = navigationRootViewModel ?? Locator.Current.GetService<NavigationRootViewModel>();
            _expenseService = Locator.Current.GetService<IExpenseService>();
            _walletService = Locator.Current.GetService<IWalletService>();
            SelectedCategory = "Toutes";

            LoadWallet = ReactiveCommand.CreateFromTask<int,Wallet>(walletId => Task.Run(() => Locator.Current.GetService<IWalletService>().GetWallet(walletId)));
            LoadWallet
                .Subscribe(x => EntrepriseWallet = x);
            LoadCategories = ReactiveCommand.CreateFromTask<Unit, List<string>>(_ => Task.Run(() => 
            {
                var categories = _expenseService.GetCategories();
                categories.Insert(0, "Toutes");
                return categories;
            }));
            LoadCategories
                .Select(x => new ObservableCollection<string>(x))
                .ToPropertyEx(this, x => x.Categories);
            LoadCategories
             .ThrownExceptions
             .Select(x => x.Message)
             .Subscribe(x => Errors = x);
            LoadCategories
                .IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);

            SaveExpense = ReactiveCommand.CreateFromTask<(CashExpensePayementModel PayementModel, List<Payement> Payements), Expense>(param => Task.Run(() =>  _expenseService.AddOrUpdateExpense(new Expense
            {
                Description = param.PayementModel!.Description,
                Title = param.PayementModel.Title,
                Amount = param.PayementModel.TotalPaid,
                Category = param.PayementModel.Category,
                EmployeeId = Employee.Id,
                Owner = param.PayementModel.Owner,
                PayementId = param.Payements.First().Id,
                CreatedAt = Entreprise.DateDuJour
            })));
            SaveExpense
            .ThrownExceptions
            .Select(x => x.Message)
            .Subscribe(x => Errors = x);
            SaveExpense
                .IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);
            SaveExpense
               .Select(_ => Unit.Default)
               .InvokeCommand(LoadCategories);
            SaveExpense
                .Select(_ => Entreprise.Id)
                .InvokeCommand(LoadWallet);

            AddExpense = ReactiveCommand.Create<Unit, CashExpensePayementModel>(_ => 
            {
                var categories = Categories != null ? Categories.ToList() : new List<string>();
                if (categories.Count > 0)
                    categories.Remove("Toutes");
                return new CashExpensePayementModel(_walletService.GetWallet(Entreprise.WalletId) ?? throw new ArgumentException("Ce wallet est invalide"), Entreprise, categories);
            });
            AddExpense
                .ToPropertyEx(this, x => x.PayementModel);
            AddExpense
                .SelectMany(x => x.Pay)
                .Where(x => x.Count > 0)
                .Where(x => PayementModel != null)
                .Select(x => (PayementModel, x))
                .InvokeCommand(SaveExpense);

            LoadExpenses = ReactiveCommand.CreateFromTask<LoadParameter, List<Expense>>(param => Task.Run(() =>
             {
                 CurrentPage = param.Skip;
                 ItemPerPage = param.Take;
                 
                 return LoadingExpenses(param,SelectedCategory == "Toutes" ? null : SelectedCategory,SelectedDate);
             }), Observable.Return(!IsBusy));
            LoadExpenses
              .ThrownExceptions
              .Select(x => x.Message)
              .Subscribe(x => Errors = x);
            LoadExpenses
                .IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);
            SaveExpense
                .Select(_ => new LoadParameter("", CurrentPage, ItemPerPage))
                .InvokeCommand(LoadExpenses);

            LoadExpenses
                .Select(_ => Unit.Default)
                .InvokeCommand(LoadCategories);
            LoadExpenses
                .Where(x => x.Count > 0)
                .Select(x => x.First())
                .Subscribe(x => SelectedExpense = x);
            LoadExpenses
                .Select(x => new ObservableCollection<Expense>(x))
                .ToPropertyEx(this, x => x.Expenses);
            LoadExpenses
                .Where(x => x != null && x.Count > 0)
                .Select(x => x.Sum(s => s.Amount))
                .Subscribe(x => TotalExpense = x);
            LoadExpenses
                .Select(_ => Entreprise.Id)
                .InvokeCommand(LoadWallet);

            this.WhenAnyValue(x => x.SelectedCategory)
                .Select(_ => new LoadParameter("", CurrentPage, ItemPerPage))
                .InvokeCommand(LoadExpenses);
            this.WhenAnyValue(x => x.SelectedDate)
                .Where(x => x != null)
               .Select(_ => new LoadParameter("", CurrentPage, ItemPerPage))
               .InvokeCommand(LoadExpenses);

            RefreshWallet = ReactiveCommand.CreateFromTask<Unit, Wallet>(_ => Task.Run(() => _walletService.RegularizeWallet(Entreprise.WalletId, true)));
            RefreshWallet
                .Subscribe(x => EntrepriseWallet = x);

        }

        public string? UrlPathSegment => nameof(CashViewModel);

        public IScreen HostScreen => _navigationRootViewModel;

        [ObservableAsProperty]
        public CashExpensePayementModel? PayementModel { get;}

        [Reactive]
        public CustomerPayementModel DepositPayementModel { get; private set; }

        [Reactive]
        public string SelectedCategory { get; set; }

        [Reactive]
        public Wallet EntrepriseWallet { get; private set; }

        [Reactive]
        public DateTime? SelectedDate { get; set; }

        [Reactive]
        public Expense? SelectedExpense { get; set; }

        [Reactive]
        public decimal TotalExpense { get; private set; }

        [ObservableAsProperty]
        public ObservableCollection<string> Categories { get; }

        [ObservableAsProperty]
        public ObservableCollection<Expense> Expenses { get; }

        public ReactiveCommand<Unit,CashExpensePayementModel> AddExpense { get; }

        public ReactiveCommand<Unit, List<string>> LoadCategories { get; }

        public ReactiveCommand<LoadParameter, List<Expense>> LoadExpenses { get; }

        public ReactiveCommand<int, Wallet> LoadWallet { get; }

        public ReactiveCommand<Unit, Wallet> RefreshWallet { get; }

        public ReactiveCommand<(CashExpensePayementModel PayementModel,List<Payement> Payements), Expense> SaveExpense { get; }

        private List<Expense> LoadingExpenses(LoadParameter parameter, string? category, DateTime? date = null)
         => _expenseService.GetExpenses(category, date, parameter.Skip, parameter.Take);

        public int CurrentPage { get; set; } = 0;

        public int ItemPerPage { get; set; } = 100;
    }
}
