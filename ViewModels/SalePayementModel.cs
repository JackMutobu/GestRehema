using GestRehema.Entities;

namespace GestRehema.ViewModels
{
    public class SalePayementModel:BasePayementModel
    {
        public SalePayementModel(Sale sale,Entreprise entreprise,Wallet wallet, decimal totalAmount) :base(wallet,entreprise,totalAmount)
        {
            Sale = sale;

        }

        public Sale Sale { get; }
    }
}
