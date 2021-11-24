using GestRehema.Entities;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
using System.Reactive.Linq;
using System;

namespace GestRehema.ViewModels
{
    public class SupplyCartItem : ReactiveObject
    {
        public SupplyCartItem(Article article)
        {
            Article = article;

            this.WhenAnyValue(x => x.QtyInConditionement)
                .DistinctUntilChanged()
                .Subscribe(qty =>
                {
                    if (qty <= 0)
                        QtyInConditionement = 1;
                    else
                    {
                        QtyInUnitOfMeasure = Article.QtyPerConditionement * qty;
                        Total = decimal.Round(BuyingPrice == null ? (decimal)qty * Article.SellingPrice : (decimal)qty * BuyingPrice.Value, 2, MidpointRounding.AwayFromZero);
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

            this.WhenAnyValue(x => x.BuyingPrice)
                .Where(x => x != null)
                .DistinctUntilChanged()
                .Subscribe(x =>
                {
                    if (x <= 0)
                        BuyingPrice = Article.BuyingPrice;
                    else
                    {
                        BuyingPricePerUnitOfMeasure = x / (decimal)Article.QtyPerConditionement;
                        Total = decimal.Round((decimal)QtyInConditionement * x!.Value, 2, MidpointRounding.AwayFromZero);
                    }
                });

            this.WhenAnyValue(x => x.BuyingPricePerUnitOfMeasure)
                 .Where(x => x != null)
                .DistinctUntilChanged()
                .Subscribe(x => BuyingPrice = x * (decimal)Article.QtyPerConditionement);

            BuyingPrice = Article.BuyingPrice;

          
            this.WhenAnyValue(x => x.Total)
                .Subscribe(x => BuyingPrice = x / (decimal)QtyInConditionement);

            UpdateBuyingPrice = ReactiveCommand.Create<Unit, SupplyCartItem>(_ => this);
        }

        public string HeaderInUnitOfMeasure { get; }

        public string PriceInUnitOfMeasure { get; }

        public string HeaderInConditionement { get; }

        public string PriceInConditionement { get; }

        public Article Article { get; init; }

        [Reactive]
        public double QtyInUnitOfMeasure { get; set; }

        [Reactive]
        public decimal? BuyingPrice { get; set; }

        [Reactive]
        public decimal? BuyingPricePerUnitOfMeasure { get; set; }

        [Reactive]
        public double QtyInConditionement { get; set; }

        [Reactive]
        public decimal Total { get; set; }

        public ReactiveCommand<Unit, SupplyCartItem> UpdateBuyingPrice { get; }
    }
}
