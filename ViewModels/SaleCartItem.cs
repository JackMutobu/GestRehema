using GestRehema.Entities;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace GestRehema.ViewModels
{
    public class SaleCartItem:ReactiveObject
    {
        public SaleCartItem(Article article, SaleManagerViewModel saleManagerViewModel)
        {
            Article = article;

            this.WhenAnyValue(x => x.Total)
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
            HeaderInConditionement = $"Qté en {article.Conditionement}";
            HeaderInUnitOfMeasure = $"Qté en {article.UnitOfMeasure}";

            PriceInConditionement = $"Prix de vente par {article.Conditionement}";
            PriceInUnitOfMeasure = $"Prix de vente par {article.UnitOfMeasure}";

            this.WhenAnyValue(x => x.SellingPrice)
                .Where(x => x!= null)
                .DistinctUntilChanged()
                .Subscribe(x =>
                {
                    if (x <= 0)
                        SellingPrice = Article.SellingPrice;
                    else
                    {
                        SellingPricePerUnitOfMeasure = x / (decimal)Article.QtyPerConditionement;
                        Total = (decimal)QtyInConditionement * x!.Value;
                        Total = decimal.Round(Total, 2, MidpointRounding.AwayFromZero);
                    }
                });

            this.WhenAnyValue(x => x.SellingPricePerUnitOfMeasure)
                 .Where(x => x != null)
                .DistinctUntilChanged()
                .Subscribe(x => SellingPrice = x * (decimal)Article.QtyPerConditionement);

            SellingPrice = Article.SellingPrice;
            UpdateSellingPrice = ReactiveCommand.Create(() => 
            {
                saleManagerViewModel.SelectedSaleCartItem = this;
            });

            saleManagerViewModel.UpdateSellingPrice
                .Where(x => x != null && x.Article.Id == Article.Id)
                .Subscribe(x =>
                {
                    SellingPrice = x!.SellingPrice;
                });

        }

        public string HeaderInUnitOfMeasure { get; }

        public string PriceInUnitOfMeasure { get; }

        public string HeaderInConditionement { get; }

        public string PriceInConditionement { get; }

        public Article Article { get; init; }

        [Reactive]
        public double QtyInUnitOfMeasure { get; set; }

        [Reactive]
        public decimal? SellingPrice { get; set; }

        [Reactive]
        public decimal? SellingPricePerUnitOfMeasure { get; set; }

        [Reactive]
        public double QtyInConditionement { get; set; }

        [Reactive]
        public decimal Total { get; private set; }

        public ReactiveCommand<Unit,Unit> UpdateSellingPrice { get; }


    }
}
