using FluentValidation;
using GestRehema.Entities;

namespace GestRehema.Validations
{
    public class ArticleValidation: AbstractValidator<Article>
    {
        public ArticleValidation()
        {
            RuleFor(x =>  x.BuyingPrice)
                .Must(v => v > 0)
                .WithMessage("Prix d'achat doit etre supérieur à 0");
            RuleFor(x => x.SellingPrice)
                .Must(v => v > 0)
                .WithMessage("Prix d'achat doit etre supérieur à 0");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Le nom du produit est obligatoire");
        }
    }
}
