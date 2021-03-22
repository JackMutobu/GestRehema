using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace GestRehema.ViewModels
{

    public class SupplierManagerArticle:ReactiveObject
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public string? TechnicalCode { get; set; }

        public string? UnitOfMeasure { get; set; }

        public decimal SellingPrice { get; set; }

        public decimal BuyingPrice { get; set; }

        public double InStock { get; set; }

        public string Category { get; set; } = null!;

        public string Conditionement { get; set; } = null!;

        public double QtyPerConditionement { get; set; }

        [Reactive]
        public bool Selected { get; set; }
    }
}
