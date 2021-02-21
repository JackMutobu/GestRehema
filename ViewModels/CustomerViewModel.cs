using GestRehema.Entities;
using GestRehema.Services;
using GestRehema.Validations;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System;
using DynamicData;
using System.Reactive;
using System.Linq;

namespace GestRehema.ViewModels
{
    public class CustomerViewModel : ViewModelBaseWithValidation, IRoutableViewModel
    {
        private readonly NavigationRootViewModel _navigationRootViewModel;
        private ICustomerService _customerService;
        public CustomerViewModel(NavigationRootViewModel navigationRootViewModel = null!) : base(new CustomerValidation())
        {
            _navigationRootViewModel = navigationRootViewModel ?? Locator.Current.GetService<NavigationRootViewModel>();
            Model = new Customer();
            _customerService = Locator.Current.GetService<ICustomerService>();
            Customers = new ObservableCollection<Customer>();

            Validate = ReactiveCommand
              .Create<ValidationParameter<Customer>, string>(p => RaiseValidation(p.Model, p.PropertyName));
            Validate
                .Subscribe(x => Errors = x);
            ValidateModel();

            LoadCustomers = ReactiveCommand.CreateFromTask<LoadParameter, List<Customer>>
               (p =>
               {
                   CurrentPage = p.Skip;
                   ItemPerPage = p.Take;
                   return Task.Run(() => LoadingCustomers(p));
               }, Observable.Return(!IsBusy));
            LoadCustomers
                .Subscribe(x =>
                {
                    Customers.Clear();
                    Customers.AddRange(x);
                });
            LoadCustomers.ThrownExceptions
              .Select(x => x.Message)
              .Subscribe(x => Errors = x);
            LoadCustomers.IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);

            SaveCustomer = ReactiveCommand.CreateFromTask(() =>
            {
                Model.ImageUrl = ImageUrl;
                return Task.Run(() => _customerService.AddOrUpdateCustomer(Model));
            }, isValid);
            SaveCustomer
              .Subscribe(x =>
              {
                  InitializeFields();

                  if (!Customers.Any(x => x.Id == x.Id))
                      Customers.Insert(0, x);
                  else
                      LoadCustomers!.Execute(new LoadParameter("", CurrentPage, ItemPerPage)).Subscribe();
              });
            SaveCustomer.ThrownExceptions
              .Select(x => x.Message)
              .Subscribe(x => Errors = x);
            SaveCustomer.IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);

            this.WhenAnyValue(x => x.SearchQuery)
               .Select(term => term?.Trim())
               .DistinctUntilChanged()
               .Select(x => new LoadParameter(x, CurrentPage, ItemPerPage))
               .InvokeCommand(LoadCustomers);
        }

        private void InitializeFields()
        {
            Model = new Customer();
            Name = null;
            Adresse = null;
            NumTelephone = null;
            Email = null;
            CustomerType = CustomerTypes.First();
        }

        public Customer Model { get; set; }

        public int CurrentPage { get; set; } = 0;

        public int ItemPerPage { get; set; } = 100;

        [Reactive]
        public ObservableCollection<Customer> Customers { get; set; }

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
        public string? ImageUrl { get; set; }

        [Reactive]
        public string? SearchQuery { get; set; }

        public List<string> CustomerTypes { get; } = new List<string> { "Journalier", "Organization(ONG)", "Autre magasin" };

        public string? UrlPathSegment => nameof(CustomerViewModel);

        public IScreen HostScreen => _navigationRootViewModel;

        public ReactiveCommand<LoadParameter,List<Customer>> LoadCustomers { get; }

        public ReactiveCommand<Unit,Customer> SaveCustomer { get; }

        private List<Customer> LoadingCustomers(LoadParameter parameter)
        => string.IsNullOrEmpty(parameter.SearchQuery) switch
        {
            true => _customerService.GetCustomers(parameter.Skip, parameter.Take),
            _ => _customerService.SearchCustomers(parameter.SearchQuery!)
        };

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
