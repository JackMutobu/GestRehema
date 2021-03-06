using GestRehema.Entities;
using ReactiveUI;
using System.Reactive;

namespace GestRehema.ViewModels
{
    public class SaleArticleItem
    {
        public SaleArticleItem(Article article,SaleViewModel saleViewModel)
        {
            Article = article;
            AddDelivery = ReactiveCommand.Create(() => this.Article.Id);
            AddDelivery
                .InvokeCommand(saleViewModel.AddDelivery);
        }

        public int SaleId { get; set; }

        public Article Article { get; }

        public double Quantity { get; set; }

        public double QtyInUnitOfMeasure => Quantity * Article.QtyPerConditionement;

        public decimal TotalAmount => decimal.Round((decimal)Quantity * UnitSellingPrice,2, System.MidpointRounding.AwayFromZero);

        public double RemainingQty => QtyInUnitOfMeasure - DeliverdQtyInUnitOfMeasure;

        public decimal Commission => decimal.Round((UnitSellingPrice - UnitRealSellingPrice) * (decimal)Quantity,2, System.MidpointRounding.AwayFromZero);

        public decimal UnitSellingPrice { get; set; }
        public decimal UnitRealSellingPrice { get; set; }
        public decimal UnitBuyingPrice { get; set; }

        public double DeliveredQuantity { get; set; }
        public double DeliverdQtyInUnitOfMeasure => DeliveredQuantity * Article.QtyPerConditionement;

        public double AwaitingDelivery => Quantity - DeliveredQuantity;
        public double AwaitingDeliveryInUnitOfMeasure => AwaitingDelivery * Article.QtyPerConditionement;

        public ReactiveCommand<Unit, int> AddDelivery { get; }


    }
}
