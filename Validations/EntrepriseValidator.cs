using FluentValidation;
using GestRehema.Entities;

namespace GestRehema.Validations
{
    public class EntrepriseValidator: AbstractValidator<Entreprise>
    {
        public EntrepriseValidator()
        {
            RuleFor(x => x.TauxDuJour)
                .Must(x => x > 0)
                .WithMessage("Le taux doit etre supérieur à 0");
        }
    }
}
