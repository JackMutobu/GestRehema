using GestRehema.Entities;
using GestRehema.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System;

namespace GestRehema.ViewModels
{
    public class LoginModel
    {
        public string Username { get; set; } = "admin@rehema.com";
        public string Password { get; set; } = "aDmin@2021";
    }
    public record ValidateParam(LoginModel LoginModel,string PropertyName);

    public  record ValidationParameter<T>(T Model,string PropertyName) where T:class;

    public class LoginViewModel : ReactiveValidationObject
    {
        private readonly IUserService _userService;

        public LoginViewModel() : base(new LoginValidation())
        {
            LoginModel = new LoginModel();
            Username = LoginModel.Username;
            Password = LoginModel.Password;

            _userService = Locator.Current.GetService<IUserService>();
            
            Validate = ReactiveCommand
                .Create<ValidateParam,string>(p => RaiseValidation(p.LoginModel, p.PropertyName));
            Validate
                .Subscribe(x => Errors = x);

            this.WhenAnyValue(x => x.Username)
                .Where(x => x != null)
                .Select(x => 
                {
                    LoginModel.Username = x;
                    return new ValidateParam(LoginModel, nameof(LoginModel.Username));
                })
                .InvokeCommand(Validate);
            this.WhenAnyValue(x => x.Password)
                .Where(x => x != null)
               .Select(x =>
               {
                   LoginModel.Password = x;
                   return new ValidateParam(LoginModel, nameof(LoginModel.Password));
               })
               .InvokeCommand(Validate);

            Login = ReactiveCommand
                .CreateFromTask(() => Task.Run(() => _userService.Login(LoginModel.Username, LoginModel.Password)),isValid);

            Login.ThrownExceptions
                .Select(x => x.Message)
                .Subscribe(x => Errors = x);
            Login.IsExecuting
                .ToPropertyEx(this, x => x.IsBusy);
        }

        [Reactive]
        public string? Errors { get; set; }

        [ObservableAsProperty]
        public bool IsBusy { get; }

        [Reactive]
        public LoginModel LoginModel { get; set; }

        [Reactive]
        public string Password { get; set; }

        [Reactive]
        public string Username { get; set; }

        public ReactiveCommand<Unit,User?> Login { get; }

        public ReactiveCommand<ValidateParam,string> Validate { get; } 


    }
}
