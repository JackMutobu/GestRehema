using GestRehema.Entities;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive.Linq;
using System;

namespace GestRehema.ViewModels
{
    public class SaleDeliveryModel:ReactiveObject
    {
        public SaleDeliveryModel(Sale sale,Article article,double totalQty)
        {
            Sale = sale;
            Article = article;

            this.WhenAnyValue(x => x.QtyInConditionement)
                .DistinctUntilChanged()
                .Subscribe(qty =>
                {
                    if (qty <= 0 || qty > totalQty)
                        QtyInConditionement = 1;
                    else
                        QtyInUnitOfMeasure = article.QtyPerConditionement * qty;

                    RemainingConditionement = totalQty > qty ? totalQty - qty : 0;
                    RemainingQtyUnitOfMeasure = RemainingConditionement * article.QtyPerConditionement;
                });

            this.WhenAnyValue(x => x.QtyInUnitOfMeasure)
                .DistinctUntilChanged()
                .Subscribe(qty => QtyInConditionement = qty / article.QtyPerConditionement);

            QtyInConditionement = 1;
            TotalQtyInUnitOfMeasure = totalQty * article.QtyPerConditionement;
            TotalQtyInConditionement = totalQty;
        }

        public Sale Sale { get; }

        [Reactive]
        public double QtyInUnitOfMeasure { get; set; }

        [Reactive]
        public double QtyInConditionement { get; set; }

        [Reactive]
        public double RemainingQtyUnitOfMeasure { get; set; }

        [Reactive]
        public double  RemainingConditionement { get; set; }

        public Article Article { get; set; }

        [Reactive]
        public double TotalQtyInUnitOfMeasure { get; private set; }

        [Reactive]
        public double TotalQtyInConditionement { get; private set; }
    }
}
