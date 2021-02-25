using GestRehema.Entities;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace GestRehema.ViewModels
{
    public class SaleCartItem:ReactiveObject
    {
        public SaleCartItem(Article article, SaleManagerViewModel saleManagerViewModel)
        {
            Article = article;

            this.WhenAnyValue(x => x.Total)
                .DistinctUntilChanged()
                .Select(_ => saleManagerViewModel.CartItems.ToList())
                .InvokeCommand(saleManagerViewModel.CalculateCartSubTotal);

            this.WhenAnyValue(x => x.QtyInConditionement)
                .DistinctUntilChanged()
                .Subscribe(qty =>
                {
                    if (qty <= 0)
                        QtyInConditionement = 1;
                    else
                    {
                        QtyInUnitOfMeasure = Article.QtyPerConditionement * qty;
                        Total = SellingPrice == null ? (decimal)qty * Article.SellingPrice : (decimal)qty * SellingPrice.Value;
                        Total = decimal.Round(Total, 2, MidpointRounding.AwayFromZero);
                    }
                });

            this.WhenAnyValue(x => x.QtyInUnitOfMeasure)
                .DistinctUntilChanged()
                .Subscribe(qty => QtyInConditionement = qty / Article.QtyPerConditionement);

                QtyInConditionement = 1;

        }
        public Article Article { get; init; }

        [Reactive]
        public double QtyInUnitOfMeasure { get; set; }

        [Reactive]
        public decimal? SellingPrice { get; set; }

        [Reactive]
        public double QtyInConditionement { get; set; }

        [Reactive]
        public decimal Total { get; private set; }


    }
}
