using FluentValidation;
using GestRehema.Entities;

namespace GestRehema.Validations
{
    public class SupplierValidation:AbstractValidator<Supplier>
    {
        public SupplierValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Veuillez spécifier les noms du fournisseur");
            RuleFor(x => x.Adresse)
                .NotEmpty()
                .WithMessage("Veuillez spécifier le lieu du fourniseur");
            RuleFor(x => x.NumTelephone)
                .NotEmpty()
                .WithMessage("Veuillez spécifier le numéro de téléphone du fourniseur");
        }
    }
}
