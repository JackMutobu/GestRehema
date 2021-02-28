using FluentValidation;
using GestRehema.Entities;
using GestRehema.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using System;
using System.Reactive.Linq;
using DynamicData;
using System.Linq;

namespace GestRehema.ViewModels
{
    public class BaseSaleViewModel : ViewModelBaseWithValidation
    {
        protected readonly ISaleService _saleService;

        public BaseSaleViewModel(IValidator _validator) : base(_validator)
        {
            _saleService = Locator.Current.GetService<ISaleService>();
        }

        protected Sale UpdateOrAddSale(List<Payement> x)
        {
            var sale = PayementModel!.Sale;
            if (sale.Id <= 0)
                sale = _saleService.AddSale(sale);

            foreach (var payement in x)
            {
                var salePayement = new SalePayement()
                {
                    AmountPaid = payement.TotalPaid > PayementModel.TotalAmount ? PayementModel.TotalAmount.Value : payement.TotalPaid,
                    Date = DateTime.UtcNow,
                    SaleId = sale.Id,
                    PayementId = payement.Id
                };
                sale = _saleService.AddPayement(salePayement, sale.Id);
            }
            return sale;
        }

        public ReactiveCommand<List<Payement>, Sale> Pay { get; protected set; }

        public ReactiveCommand<Unit, SalePayementModel> AddPayement { get; protected set; }

        [ObservableAsProperty]
        public SalePayementModel? PayementModel { get; protected set; }


    }
}
