using GestRehema.Contants;
using GestRehema.Entities;

namespace GestRehema.ViewModels
{
    public class CustomerPayementModel:BasePayementModel
    {
        public CustomerPayementModel(Customer customer,Entreprise entreprise):base(customer.Wallet!,entreprise, PayementType.VirtualAccountDeposit)
        {
            Customer = customer;
            ShowDanger = customer.Wallet!.AmountInDebt > 0;
        }
        public Customer Customer { get; init; }

        public bool ShowDanger { get; }

    }
}
