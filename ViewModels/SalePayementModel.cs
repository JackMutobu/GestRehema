using GestRehema.Entities;

namespace GestRehema.ViewModels
{
    public class SalePayementModel:BasePayementModel
    {
        public SalePayementModel(Sale sale,Entreprise entreprise,Wallet wallet, decimal totalAmount,bool isNewPayement) :base(wallet,entreprise, isNewPayement,totalAmount)
        {
            Sale = sale;

        }

        public Sale Sale { get; }
    }
}
