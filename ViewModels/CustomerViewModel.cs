using GestRehema.Entities;
using GestRehema.Services;
using GestRehema.Validations;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System;
using DynamicData;
using System.Reactive;
using System.Linq;
using DynamicData.Binding;
using System.Collections.ObjectModel;

namespace GestRehema.ViewModels
{
    public record LoadCustomerParameter(string? SearchQuery, int Skip=0, int Take=100, string? CustomerType = null);
    public class CustomerViewModel : ViewModelBaseWithValidation, IRoutableViewModel
    {
        private readonly NavigationRootViewModel _navigationRootViewModel;
        private ICustomerService _customerService;
        private readonly IWalletService _walletService;
        private readonly IPayementService _payementService;
        private SourceList<Customer> _customers { get; } = new SourceList<Customer>();
        private readonly IObservableCollection<Customer> _targetCollectionCustomers = new ObservableCollectionExtended<Customer>();


        public CustomerViewModel(NavigationRootViewModel navigationRootViewModel = null!) : base(new CustomerValidation())
        {
            _navigationRootViewModel = navigationRootViewModel ?? Locator.Current.GetService<NavigationRootViewModel>();
            Model = new Customer();
            _customerService = Locator.Current.GetService<ICustomerService>();
            _walletService = Locator.Current.GetService<IWalletService>();
            _payementService = Locator.Current.GetService<IPayementService>();
            ImageUrl = "/Assets/Placeholder/profile.png";

            _customers.Connect()
            .ObserveOnDispatcher()
            .Bind(_targetCollectionCustomers)
            .Subscribe();

            Validate = ReactiveCommand
              .Create<ValidationParameter<Customer>, string>(p => RaiseValidation(p.Model, p.PropertyName));
            Validate
                .Subscribe(x => Errors = x);
            ValidateModel();

            LoadCustomers = ReactiveCommand.CreateFromTask<LoadCustomerParameter, List<Customer>>
               (p =>
               {
                   CurrentPage = p.Skip;
                   ItemPerPage = p.Take;
                   SearchQuery = p.SearchQuery;
                   return Task.Run(() => LoadingCustomers(p));
               }, Observable.Return(!IsBusy));
            LoadCustomers
                .Subscribe(x =>
                {
                    _customers.Clear();
                    _customers.AddRange(x);
                });
            LoadCustomers.ThrownExceptions
              .Select(x => x.Message)
              .Subscribe(x => Errors = x);
            LoadCustomers.IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);

            SaveCustomer = ReactiveCommand.CreateFromTask(() =>
            {
                Model.ImageUrl = ImageUrl == "/Assets/Placeholder/profile.png" ? "" : ImageUrl;
                return Task.Run(() => _customerService.AddOrUpdateCustomer(Model));
            }, isValid);
            SaveCustomer
              .Subscribe(x =>
              {
                  InitializeFields();

                  if (!_customers.Items.Any(y => y.Id == x.Id))
                      _customers.Insert(0, x);
                  else
                  {
                      var updatedItem = _customers.Items.Single(y => y.Id == x.Id);
                      int index = _customers.Items.IndexOf(updatedItem);
                      _customers.ReplaceAt(index, x);
                  }
              });
            SaveCustomer.ThrownExceptions
              .Select(x => x.Message)
              .Subscribe(x => Errors = x);
            SaveCustomer.IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);

            SelectForDelete = ReactiveCommand.Create<int,Customer>(id => Customers.Single(x => x.Id == id));
            SelectForDelete.ThrownExceptions
            .Select(x => x.Message)
            .Subscribe(x => Errors = x);
            SelectForUpdate = ReactiveCommand.Create<int, Customer>(id => Customers.Single(x => x.Id == id));
            SelectForUpdate.ThrownExceptions
            .Select(x => x.Message)
            .Subscribe(x => Errors = x);

            SelectForUpdate
                .Subscribe(x =>
                {
                    ImageUrl = x.ImageUrl ?? "";
                    Name = x.Name;
                    Adresse = x.Adresse;
                    NumTelephone = x.NumTelephone;
                    Email = x.Email;
                    CustomerType = x.CustomerType;
                    Model = new Customer()
                    {
                        Id = x.Id,
                        NumTelephone = x.NumTelephone,
                        Email = x.Email,
                        Name = x.Name,
                        CustomerType = x.CustomerType,
                        Adresse = x.Adresse,
                        ImageUrl = x.ImageUrl,
                    };
                });

            Delete = ReactiveCommand.CreateFromTask<int, int>(id => Task.Run(() => _customerService.DeleteCustomer(id)));
            Delete.ThrownExceptions
             .Select(x => x.Message)
             .Subscribe(x => Errors = x);
            Delete.IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);
            Delete
                .Select(_ => new LoadCustomerParameter(SearchQuery, CurrentPage, ItemPerPage,SelectedCustomerType))
                .InvokeCommand(LoadCustomers);

            this.WhenAnyValue(x => x.SelectedCustomerType)
                .Select(x => new LoadCustomerParameter(SearchQuery, CurrentPage, ItemPerPage, x))
                .InvokeCommand(LoadCustomers);

            LoadCustomers
                .Where(x => x != null && x.Count > 0)
                .Select(x => x.FirstOrDefault())
                .Subscribe(x => SelectedCustomer = x);

            this.WhenAnyValue(x => x.SelectedCustomer)
                .Where(x => x != null)
                .Select(x => _customerService.GetWallet(x!.Id))
                .Subscribe(x => CustomerWallet = x);

            this.WhenAnyValue(x => x.SelectedCustomer)
               .Where(x => x != null)
               .Subscribe(x => PayementModel = new CustomerPayementModel(x!, Entreprise));

            Pay = ReactiveCommand.CreateFromTask<Unit, Wallet>(_ => Task.Run(() => _walletService.AddExcess(CustomerWallet!.Id,Entreprise.WalletId,PayementModel!.TotalPaid)));
            Pay.ThrownExceptions
            .Select(x => x.Message)
            .Subscribe(x => Errors = x);

            Pay.IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);

            Pay
                .Subscribe(x => CustomerWallet = x);

            LoadPayements = ReactiveCommand.CreateFromTask<int, List<Payement>>(walletId => Task.Run(() => _payementService.GetPayements(walletId)));

            LoadPayements
                .Where(p => p != null)
                .Select(x => 
                {
                    foreach(var p in x)
                    {
                        if(p != null)
                        {
                            p.TotalPaid = p.AmountInUSD + p.AmountInCDF / Entreprise.TauxDuJour;
                        }
                    }
                    return new ObservableCollection<Payement>(x.Where(y => y != null));
                })
                .ToPropertyEx(this, x => x.Payements);
            LoadPayements
                .Select(x => x.Where(y=> y != null).FirstOrDefault())
                .Where(x => x != null)
                .Subscribe(x => SelectedPayement = x!);
            LoadPayements.ThrownExceptions
            .Select(x => x.Message)
            .Subscribe(x => Errors = x);

            this.WhenAnyValue(x => x.CustomerWallet)
              .Where(x => x != null)
              .Select(x => x.Id)
              .InvokeCommand(LoadPayements);





        }

        private void InitializeFields()
        {
            Model = new Customer();
            Name = null;
            Adresse = null;
            NumTelephone = null;
            Email = null;
            CustomerType = CustomerTypes.First();
            ImageUrl = "/Assets/Placeholder/profile.png";
        }

        public Customer Model { get; set; }

        public int CurrentPage { get; set; } = 0;

        public int ItemPerPage { get; set; } = 100;

        public IObservableCollection<Customer> Customers => _targetCollectionCustomers;

        [Reactive]
        public string? Name { get; set; }

        [Reactive]
        public string? Adresse { get; set; }

        [Reactive]
        public string? NumTelephone { get; set; }

        [Reactive]
        public string? Email { get; set; }

        [Reactive]
        public string? CustomerType { get; set; }

        [Reactive]
        public string? SelectedCustomerType { get; set; }

        [Reactive]
        public string ImageUrl { get; set; }

        [Reactive]
        public string? SearchQuery { get; set; }

        [Reactive]
        public Customer? SelectedCustomer { get; set; }

        [Reactive]
        public Wallet? CustomerWallet { get; private set; }

        [Reactive]
        public CustomerPayementModel PayementModel { get; private set; }


        public List<string> CustomerTypes { get; } = new List<string> { "Journalier", "Organization(ONG)", "Autre magasin" };

        public List<string> SelectCustomerTypes { get; } = new List<string> { "Tous","Journalier", "Organization(ONG)", "Autre magasin" };

        public string? UrlPathSegment => nameof(CustomerViewModel);

        public IScreen HostScreen => _navigationRootViewModel;

        public ReactiveCommand<LoadCustomerParameter,List<Customer>> LoadCustomers { get; }

        public ReactiveCommand<int, List<Payement>> LoadPayements { get; }

        [ObservableAsProperty]
        public ObservableCollection<Payement> Payements { get; }

        [Reactive]
        public Payement SelectedPayement { get; set; }

        public ReactiveCommand<Unit,Customer> SaveCustomer { get; }

        public ReactiveCommand<int, Customer> SelectForUpdate { get; }

        public ReactiveCommand<int, Customer> SelectForDelete { get; }

        public ReactiveCommand<int, int> Delete { get; }

        public ReactiveCommand<Unit, Wallet> Pay { get; }

        private List<Customer> LoadingCustomers(LoadCustomerParameter parameter)
        {
            string? customerType = parameter.CustomerType == "Tous" ? null : parameter.CustomerType;
            return string.IsNullOrEmpty(parameter.SearchQuery) switch
            {
                true => _customerService.GetCustomers(parameter.Skip, parameter.Take, customerType),
                _ => _customerService.SearchCustomers(parameter.SearchQuery!, customerType)
            };
        }

        public ReactiveCommand<ValidationParameter<Customer>, string> Validate { get; }

        private void ValidateModel()
        {
            this.WhenAnyValue(x => x.Name)
                .Where(x => x != null)
                .Select(x =>
                {
                    Model.Name = x;
                    return new ValidationParameter<Customer>(Model, nameof(Customer.Name));
                })
                .InvokeCommand(Validate);

            this.WhenAnyValue(x => x.NumTelephone)
                .Where(x => x != null)
                .Select(x =>
                {
                    Model.NumTelephone = x;
                    return new ValidationParameter<Customer>(Model, nameof(Customer.NumTelephone));
                })
                .InvokeCommand(Validate);

            this.WhenAnyValue(x => x.Email)
               .Where(x => x != null)
               .Select(x =>
               {
                   Model.Email = x;
                   return new ValidationParameter<Customer>(Model, nameof(Customer.Email));
               })
               .InvokeCommand(Validate);

            this.WhenAnyValue(x => x.Adresse)
               .Where(x => x != null)
               .Select(x =>
               {
                   Model.Adresse = x;
                   return new ValidationParameter<Customer>(Model, nameof(Customer.Adresse));
               })
               .InvokeCommand(Validate);

            this.WhenAnyValue(x => x.CustomerType)
              .Where(x => x != null)
              .Select(x =>
              {
                  Model.CustomerType = x;
                  return new ValidationParameter<Customer>(Model, nameof(Customer.CustomerType));
              })
              .InvokeCommand(Validate);
        }
    }
}
