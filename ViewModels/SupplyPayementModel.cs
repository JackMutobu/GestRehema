using GestRehema.Entities;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;
using System;

namespace GestRehema.ViewModels
{
    public class SupplyPayementModel : BasePayementModel
    {
        public SupplyPayementModel(Wallet wallet, Entreprise entreprise, string payementType, decimal? totalAmount = null) : base(wallet, entreprise, payementType, totalAmount)
        {
            if (totalAmount == null)
                throw new ArgumentNullException(nameof(totalAmount), "Le montant total doit etre superieur à 0");

            PayementMethods.Remove(Entities.PayementMethod.Wallet);
            this.WhenAnyValue(x => x.Errors)
                .Select(errors => string.IsNullOrEmpty(errors))
                .ToPropertyEx(this, x => x.ShowErros);

            this.WhenAnyValue(x => x.ExcessInUsd)
                .Where(excess => excess > 0)
                .Select(_ => "Le montant payé doit etre inféreur ou égal au montant à payer ")
                .Subscribe(error => Errors = error);

            this.WhenAnyValue(x => x.ExcessInUsd)
               .Where(excess => excess <= 0)
               .Subscribe(error => Errors = string.Empty);

            this.WhenAnyValue(x => x.ExcessInUsd)
                .Where(excess => excess > 0)
                .Subscribe(_ =>
                {
                    PaidInCDF = 0;
                    PaidInUsd = TotalAmount!.Value;
                });
        }

        [ObservableAsProperty]
        public bool ShowErros { get; }
    }
}
