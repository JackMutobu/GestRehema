using GestRehema.Entities;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System;
using System.Reactive.Linq;

namespace GestRehema.ViewModels
{
    public class SalePayementModel:ReactiveObject
    {
        public SalePayementModel(Sale sale,decimal exchangeRate,decimal totalAmount)
        {
            TotalAmount = totalAmount;
            Sale = sale;
            PayementMethod = Entities.PayementMethod.Cash;
            PayementMethods = Entities.PayementMethod.Methods;

            this.WhenAnyValue(x => x.PaidInUsd)
                .Subscribe(x =>
                {
                    if (x < 0)
                        PaidInUsd = 0;
                    else
                        DeriveAmounts(exchangeRate, x, PaidInCDF);
                });

            this.WhenAnyValue(x => x.PaidInCDF)
                .Subscribe(x =>
                {
                    if (x < 0)
                        PaidInCDF = 0;
                    else
                        DeriveAmounts(exchangeRate, PaidInUsd, x);
                });
        }

        private void DeriveAmounts(decimal exchangeRate, decimal amountInUsd,decimal amountInCDF)
        {
            TotalPaid = amountInUsd + decimal.Round((amountInCDF / exchangeRate), 2, MidpointRounding.AwayFromZero);
            Debt = TotalAmount > TotalPaid ? TotalAmount - TotalPaid : 0;
            ExcessInUsd = TotalPaid > TotalAmount ? TotalPaid - TotalAmount : 0;
            ExcessInFC = decimal.Round(ExcessInUsd * exchangeRate, 2, MidpointRounding.ToEven);
        }

        public Sale Sale { get; init; }

        public List<string> PayementMethods { get; }

        [Reactive]
        public string PayementMethod { get; set; }

        [Reactive]
        public decimal Debt { get; private set; }

        [Reactive]
        public decimal ExcessInUsd { get; private set; }

        [Reactive]
        public decimal ExcessInFC { get; private set; }

        [Reactive]
        public decimal PaidInUsd { get; set; }

        [Reactive]
        public decimal PaidInCDF { get; set; }

        [Reactive]
        public decimal TotalPaid { get; private set; }

        [Reactive]
        public bool AddExcessToCustomerWallet { get; set; }

        public decimal TotalAmount { get; }
    }
}
