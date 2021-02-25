using FluentValidation;
using GestRehema.Entities;
using System.Linq;

namespace GestRehema.Validations
{
    public class SaleValidation: AbstractValidator<Sale>
    {
        public SaleValidation()
        {
            RuleFor(x => x.ArticleSold)
                .Must(x => x.Count > 0)
                .WithMessage("Vous devez ajouter des produits au panier");

            RuleFor(x => x.CustomerId)
                .Must(x => x > 0)
                .WithMessage("Vous choisir un client");
        }
    }
}
