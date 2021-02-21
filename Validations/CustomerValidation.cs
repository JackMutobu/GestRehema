using FluentValidation;
using GestRehema.Entities;

namespace GestRehema.Validations
{
    public class CustomerValidation: AbstractValidator<Customer>
    {
        public CustomerValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Le nom du client est obligatoire");

            RuleFor(x => x.NumTelephone)
               .NotEmpty()
               .WithMessage("Le numéro de téléphone du client est obligatoire");

            RuleFor(x => x.CustomerType)
               .NotEmpty()
               .WithMessage("Veuillez spécifier le type de client");
        }
    }
}
