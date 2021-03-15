using GestRehema.Entities;

namespace GestRehema.ViewModels
{
    public class CustomerPayementModel:BasePayementModel
    {
        public CustomerPayementModel(Customer customer,Entreprise entreprise, bool isDepositToEntreprise):base(customer.Wallet!,entreprise,false,isDepositToEntreprise: isDepositToEntreprise)
        {
            Customer = customer;
        }
        public Customer Customer { get; init; }

    }
}
