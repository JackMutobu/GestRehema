using FluentValidation;

namespace GestRehema.ViewModels
{
    public class LoginValidation: AbstractValidator<LoginModel>
    {
        public LoginValidation()
        {
            RuleFor(lg => lg.Username)
                .NotEmpty()
                .WithMessage("Veuillez entrer votre nom d'utilisateur");

            RuleFor(lg => lg.Password)
                .NotEmpty()
                .WithMessage("Veuillez entrer votre mot de passe");
        }
    }
}
