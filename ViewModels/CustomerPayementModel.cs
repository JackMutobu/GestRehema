using GestRehema.Entities;

namespace GestRehema.ViewModels
{
    public class CustomerPayementModel:BasePayementModel
    {
        public CustomerPayementModel(Customer customer,Entreprise entreprise):base(customer.Wallet!,entreprise)
        {
            Customer = customer;
        }
        public Customer Customer { get; init; }

    }
}
