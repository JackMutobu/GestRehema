using GestRehema.Entities;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System;
using GestRehema.Services;
using Splat;
using System.Reactive;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace GestRehema.ViewModels
{
    public class BasePayementModel:ReactiveObject
    {
        private readonly IWalletService _walletService;
        private readonly IPayementService _payementService;
        private readonly bool _isNewPayement;
        private readonly bool _isDepositToEntreprise;

        public BasePayementModel(Wallet wallet,Entreprise entreprise, bool isNewPayement, decimal? totalAmount = null, bool isDepositToEntreprise = false)
        {
            _walletService = Locator.Current.GetService<IWalletService>();
            _payementService = Locator.Current.GetService<IPayementService>();
            _isNewPayement = isNewPayement;
            _isDepositToEntreprise = isDepositToEntreprise;

            TotalAmount = totalAmount;
            Wallet = wallet;
            PayementMethod = Entities.PayementMethod.Cash;
            PayementMethods = Entities.PayementMethod.Methods;
            Entreprise = entreprise;

            var walletBalance = wallet.AmountInExcess + wallet.AmountOwned - wallet.AmountInDebt;

            this.WhenAnyValue(x => x.PaidInUsd)
                .Subscribe(x =>
                {
                    if (x < 0)
                        PaidInUsd = 0;
                    else
                        DeriveAmounts(entreprise.TauxDuJour, x, PaidInCDF,walletBalance);
                });

            this.WhenAnyValue(x => x.PaidInCDF)
                .Subscribe(x =>
                {
                    if (x < 0)
                        PaidInCDF = 0;
                    else
                        DeriveAmounts(Entreprise.TauxDuJour, PaidInUsd, x,walletBalance);
                });

            this.WhenAnyValue(x => x.AddExcessToCustomerWallet)
                .Subscribe(x => DeriveAmounts(Entreprise.TauxDuJour, PaidInUsd, PaidInCDF, walletBalance));

            this.WhenAnyValue(x => x.PayementMethod)
                .Subscribe(x => DeriveAmounts(Entreprise.TauxDuJour, PaidInUsd, PaidInCDF, walletBalance));

            this.WhenAnyValue(x => x.TotalPaid)
               .Subscribe(x => DeriveAmounts(Entreprise.TauxDuJour, PaidInUsd, PaidInCDF, walletBalance));

            Pay = ReactiveCommand.CreateFromTask<Unit, List<Payement>>(_ => Task.Run(() => Paying()));
            Pay.ThrownExceptions
                .Select(x => x.Message)
                .Subscribe(x => Errors = x);

            Pay.IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);
        }

        public List<string> PayementMethods { get; }

        [Reactive]
        public string PayementMethod { get;  set; }

        [Reactive]
        public Entreprise Entreprise { get; private set; }

        [Reactive]
        public decimal PaidInUsd { get;  set; }

        [Reactive]
        public decimal PaidInCDF { get;  set; }

        [Reactive]
        public decimal TotalPaid { get;  set; }

        [Reactive]
        public decimal Debt { get;  set; }

        [Reactive]
        public decimal ExcessInUsd { get;  set; }

        [Reactive]
        public decimal ExcessInFC { get;  set; }

        public decimal? TotalAmount { get; }

        [Reactive]
        public string? TransactionId { get;  set; }

        [Reactive]
        public string? AccountNumber { get;  set; }

        [Reactive]
        public string? PayementOrganization { get; set; }

        [Reactive]
        public decimal WalletBalance { get;  set; }

        public Wallet Wallet { get; }

        [Reactive]
        public string? Errors { get; protected set; }

        [ObservableAsProperty]
        public bool IsBusy { get; }

        [Reactive]
        public bool AddExcessToCustomerWallet { get;  set; }

        public ReactiveCommand<Unit, List<Payement>> Pay { get; }

        protected virtual void DeriveAmounts(decimal exchangeRate, decimal amountInUsd, decimal amountInCDF,decimal walletBalance)
        {
            TotalPaid = amountInUsd + decimal.Round((amountInCDF / exchangeRate), 2, MidpointRounding.AwayFromZero);
            if (TotalAmount != null && TotalAmount > 0)
            {
                Debt = TotalAmount > TotalPaid ? TotalAmount.Value - TotalPaid : 0;
                ExcessInUsd = TotalPaid > TotalAmount ? TotalPaid - TotalAmount.Value : 0;

                ExcessInFC = decimal.Round(ExcessInUsd * exchangeRate, 2, MidpointRounding.ToEven);
                Debt = decimal.Round(Debt, 2, MidpointRounding.AwayFromZero);
                TotalPaid = decimal.Round(TotalPaid, 2, MidpointRounding.AwayFromZero);
            }

            WalletBalance = GetWalletBalance(walletBalance, TotalPaid);
        }

        protected virtual decimal GetWalletBalance(decimal walletBalance,decimal totalPaid)
        {
            var balance = _isNewPayement switch
            {
                true => AddExcessToCustomerWallet ? ExcessInUsd + walletBalance - Debt : walletBalance - Debt,
                _ => AddExcessToCustomerWallet ? ExcessInUsd + walletBalance + totalPaid : walletBalance + totalPaid
            };

            if(TotalAmount != null && TotalAmount > 0)
            {
                balance = PayementMethod == Entities.PayementMethod.Wallet ? balance - totalPaid : balance;
            }
            else
            {
                balance = Wallet.Id == Entreprise.WalletId ? walletBalance - totalPaid : walletBalance + totalPaid;
            }

            return balance;
        }

        protected virtual List<Payement> Paying()
        {
            var payements = new List<Payement>();
            if(TotalPaid >= 0)
            {
                var payement = new Payement
                {
                    AmountInCDF = PaidInCDF,
                    AmountInUSD = PaidInUsd,
                    AccountNumber = AccountNumber,
                    Method = PayementMethod,
                    PayementOrganization = PayementOrganization,
                    ToCompany = true,
                    TransactionId = TransactionId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                if (Wallet.Id == Entreprise.WalletId)
                    payement.ToCompany = false;

                payement = _payementService.AddPayement(payement);
                payements.Add(payement);
                payement.SetTotalPaid(Entreprise.TauxDuJour);

                if (Wallet.Id != Entreprise.WalletId)
                {
                    if (TotalAmount != null && TotalAmount > 0)
                    {
                        if (ExcessInUsd > 0 && AddExcessToCustomerWallet)
                            _walletService.AddExcess(Wallet.Id, Entreprise.WalletId, ExcessInUsd, payement.Id);

                        if (_isNewPayement)
                            payement = AddDebt(payements, payement);
                        else
                            _walletService.AddExcess(Wallet.Id, Entreprise.WalletId, TotalPaid, payement.Id);

                        _walletService.AddToEntreprise(Entreprise.WalletId, TotalAmount.Value, payement.Id);
                    }
                    else
                        _walletService.AddExcess(Wallet.Id, Entreprise.WalletId, TotalPaid, payement.Id);
                }
                else
                {
                    if(!_isDepositToEntreprise)
                        _walletService.DeductFromEntreprise(Entreprise.WalletId, TotalPaid, payement.Id);
                    else
                        _walletService.AddToEntreprise(Entreprise.WalletId, TotalPaid, payement.Id);
                }
                    
                

                return payements;
            }
            throw new Exception("Le montant payé doit etre supérieur à 0");

        }

        protected virtual Payement AddDebt(List<Payement> payements, Payement payement)
        {
            if (Debt > 0)
            {
                if (Wallet.Balance > 0)
                {
                    decimal amountToPay = 0;
                    if (Wallet.Balance > Debt)
                    {
                        amountToPay = Debt;
                        Debt = 0;
                    }
                    else
                    {
                        amountToPay = Wallet.Balance;
                        Debt -= amountToPay;
                    }
                    TotalPaid += amountToPay;
                    payement = new Payement
                    {
                        AmountInUSD = amountToPay,
                        AccountNumber = Wallet.Id.ToString(),
                        Method = Entities.PayementMethod.Wallet,
                        PayementOrganization = "Ets Rehema",
                        ToCompany = true,
                        TransactionId = TransactionId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    payement = _payementService.AddPayement(payement);
                    payement.SetTotalPaid(Entreprise.TauxDuJour);
                    payements.Add(payement);
                }
                if (Debt > 0)
                    _walletService.AddDebt(Wallet.Id, Entreprise.WalletId, Debt, payement.Id);
            }

            return payement;
        }
    }
}
