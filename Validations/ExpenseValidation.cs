using FluentValidation;
using GestRehema.Entities;

namespace GestRehema.Validations
{
    public class ExpenseValidation: AbstractValidator<Expense>
    {
        public ExpenseValidation()
        {
            RuleFor(x => x.Amount)
                .Must(x => x > 0)
                .WithMessage("Le montant doit être supérieur à 0");
            RuleFor(x => x.Category)
                .NotEmpty()
                .WithMessage("Veuillez spécifier une catégorie pour cette dépense");
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Veuillez spécifier un titre pour cette dépense");
            RuleFor(x => x.Owner)
               .NotEmpty()
               .WithMessage("Veuillez spécifier la personne ayant reçu l'argent pour cette dépense");
        }
    }
}
