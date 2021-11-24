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
using GestRehema.Contants;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace GestRehema.ViewModels
{
    public class BasePayementModel:ReactiveObject
    {
        private readonly IWalletService _walletService;
        private readonly IPayementService _payementService;
        private string _payementType;
        public BasePayementModel(Wallet wallet,Entreprise entreprise, string payementType, decimal? totalAmount = null)
        {
            _walletService = Locator.Current.GetService<IWalletService>();
            _payementService = Locator.Current.GetService<IPayementService>();
            _payementType = payementType;

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
            Pay.ThrownExceptions
               .Subscribe(x => Crashes.TrackError(x));
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
            var balance = (_payementType == PayementType.NewSalePayement)  switch
            {
                true => ( PayementMethod == Entities.PayementMethod.Wallet) switch
                { 
                    true => AddExcessToCustomerWallet ? ExcessInUsd + walletBalance - Debt - totalPaid : walletBalance - Debt - totalPaid,
                    _ => AddExcessToCustomerWallet ? ExcessInUsd + walletBalance - Debt : walletBalance - Debt
                }, 
                _ =>  (_payementType == PayementType.SaleNewPayement) switch
                {
                    true => (PayementMethod == Entities.PayementMethod.Wallet) switch
                    {
                        true => AddExcessToCustomerWallet ? ExcessInUsd + walletBalance - totalPaid : walletBalance - totalPaid,
                        _ => (Wallet.AmountInDebt > 0) switch
                        {
                            true => AddExcessToCustomerWallet ? ExcessInUsd + walletBalance + DeductFromWallet(Wallet.AmountInDebt,totalPaid).AmountDeducted : walletBalance + DeductFromWallet(Wallet.AmountInDebt, totalPaid).AmountDeducted,
                            _ => AddExcessToCustomerWallet ? ExcessInUsd + walletBalance : walletBalance
                        }
                    },
                    _ => AddExcessToCustomerWallet ? ExcessInUsd + walletBalance + totalPaid : walletBalance + totalPaid
                }

            };


            if(!(TotalAmount != null && TotalAmount > 0))
                balance = Wallet.Id == Entreprise.WalletId ? walletBalance - totalPaid : walletBalance + totalPaid;

            return balance;
        }

        protected virtual List<Payement> Paying()
        {
            var payements = new List<Payement>();
            if(TotalPaid >= 0)
            {
                var payement = SavePayement(payements);
                switch (_payementType)
                {
                    case PayementType.NewSalePayement:
                        if (TotalAmount != null && TotalAmount > 0)
                        {
                            if (ExcessInUsd > 0 && AddExcessToCustomerWallet)
                                _walletService.AddExcess(Wallet.Id, Entreprise.WalletId, ExcessInUsd, payement.Id);

                            if (Debt > 0)
                            {
                                var result = PayFromVirtualAccount(Wallet, Debt);
                                if (result.Debt > 0)
                                    _walletService.AddDebt(Wallet.Id, Entreprise.WalletId, result.Debt, payement.Id);
                                if (result.Payement != null)
                                    payements.Add(result.Payement);
                            }

                            if (TotalPaid > 0)
                            {
                                if (payement.Method == Entities.PayementMethod.Wallet)
                                    _walletService.AddDebt(Wallet.Id, Entreprise.WalletId, TotalPaid, payement.Id);
                                else
                                    _walletService.AddToEntreprise(Entreprise.WalletId, TotalPaid, payement.Id);
                            }
                        }
                        break;
                    case PayementType.SaleNewPayement:
                        if (TotalAmount != null && TotalAmount > 0)
                        {
                            if (ExcessInUsd > 0 && AddExcessToCustomerWallet)
                                _walletService.AddExcess(Wallet.Id, Entreprise.WalletId, ExcessInUsd, payement.Id);

                            if (payement.Method == Entities.PayementMethod.Wallet)
                                _walletService.AddDebt(Wallet.Id, Entreprise.WalletId, TotalPaid, payement.Id);
                            else
                            {
                                if (Wallet.AmountInDebt > 0)
                                {
                                    var deductionResult = DeductFromWallet(Wallet.AmountInDebt, TotalPaid);
                                    _walletService.AddExcess(Wallet.Id, Entreprise.WalletId, deductionResult.AmountDeducted, payement.Id);
                                    TotalPaid = deductionResult.RemainingDebt;
                                }

                                if (TotalPaid > 0)
                                    _walletService.AddToEntreprise(Entreprise.WalletId, TotalPaid, payement.Id);
                            }
                        }
                        break;
                    case PayementType.EntrepriseAccountDeposit:
                        _walletService.AddToEntreprise(Entreprise.WalletId, TotalPaid, payement.Id);
                        break;
                    case PayementType.VirtualAccountDeposit:
                        _walletService.AddExcess(Wallet.Id, Entreprise.WalletId, TotalPaid, payement.Id);
                        break;
                    case PayementType.EntrepriseAccountWithdrawal:
                        _walletService.DeductFromEntreprise(Entreprise.WalletId, TotalPaid, payement.Id);
                        break;
                    case PayementType.NewSupplyPayement:
                        if(Debt > 0)
                            _walletService.AddExcess(Wallet.Id, Entreprise.WalletId, Debt, payement.Id);
                        if (TotalPaid > 0)
                            _walletService.DeductFromEntreprise(Entreprise.WalletId, TotalPaid, payement.Id);
                        break;
                    case PayementType.SupplyNewPayement:
                        if (TotalPaid > 0)
                            _walletService.AddDebt(Wallet.Id, Entreprise.WalletId, TotalPaid);
                        break;
                }

                Analytics.TrackEvent( AnalyticsKeys.NewPayement, new Dictionary<string, string> 
                {
                    { "Type", PayementMethod },
                });

                return payements;
            }
            throw new Exception("Le montant payé doit etre supérieur à 0");

        }

        private Payement SavePayement(List<Payement> payements)
        {
            var payement = new Payement
            {
                AmountInCDF = PaidInCDF,
                AmountInUSD = PaidInUsd,
                AccountNumber = AccountNumber,
                Method = PayementMethod,
                PayementOrganization = PayementOrganization,
                ToCompany = Wallet.Id == Entreprise.WalletId ? false : true,
                TransactionId = TransactionId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            payement = _payementService.AddPayement(payement);
            payements.Add(payement);
            payement.SetTotalPaid(Entreprise.TauxDuJour);
            return payement;
        }

        private (decimal Debt, Payement? Payement) PayFromVirtualAccount(Wallet wallet, decimal totalAmount)
        {
            if (wallet.Balance > 0)
            {
                var deductionResult = DeductFromWallet(Wallet.Balance, totalAmount);
                var payement = new Payement
                {
                    AmountInUSD = deductionResult.AmountDeducted,
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

               
                _walletService.AddDebt(wallet.Id, Entreprise.WalletId, deductionResult.AmountDeducted, payement.Id);

                return (deductionResult.RemainingDebt, payement);
            }

            return (totalAmount, null);
        }

        private (decimal AmountDeducted, decimal RemainingDebt) DeductFromWallet(decimal walletBalance, decimal amountToDeduct)
        {
            decimal amountToPay;
            decimal remainingDebt = 0;
            if (walletBalance > amountToDeduct)
            {
                amountToPay = amountToDeduct;
                remainingDebt = 0;
            }
            else
            {
                amountToPay = walletBalance;
                remainingDebt = amountToDeduct - amountToPay;
            }

            return ( amountToPay, remainingDebt);
        }
    }
}
