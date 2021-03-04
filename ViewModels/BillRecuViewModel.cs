using GestRehema.Entities;
using GestRehema.Views;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;

namespace GestRehema.ViewModels
{
    public class BillRecuViewModel:ReactiveObject
    {
        public BillRecuViewModel(Entreprise entreprise,Customer customer,Payement payement)
        {
            Entreprise = entreprise;
            Customer = customer;
            Payement = payement;
            if (payement.SalePayement != null && payement.SalePayement.Sale != null)
                Sale = payement.SalePayement.Sale;
            Motives = BillRecuMotives.Motives;
            Payement.TransactionId ??= payement.Id.ToString();
            Payement.PayementOrganization ??= " - ";
            Payement.AccountNumber ??= " - ";
        }

        public Customer Customer { get; }

        public Entreprise Entreprise { get; }

        public Payement Payement { get; }

        public Sale? Sale { get; }

        public List<string> Motives { get; }

        [Reactive]
        public string? AmountInLetters { get; set; }

        [Reactive]
        public string? Motive { get; set; }

        
    }
}
