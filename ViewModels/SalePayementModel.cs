using GestRehema.Contants;
using GestRehema.Entities;

namespace GestRehema.ViewModels
{
    public class SalePayementModel:BasePayementModel
    {
        public SalePayementModel(Sale sale,Entreprise entreprise,Wallet wallet, decimal totalAmount, string payementType) :base(wallet,entreprise, payementType,totalAmount)
        {
            Sale = sale;

        }

        public Sale Sale { get; }
    }
}
