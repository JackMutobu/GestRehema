using FluentValidation;
using FluentValidation.Results;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;

namespace GestRehema.ViewModels
{
    public class ReactiveValidationObject: ReactiveObject, INotifyDataErrorInfo
    {
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = null!;

        protected readonly IValidator validator;
        private IEnumerable<ValidationFailure> Errors;
        private bool _hasErrors;
        private bool _isValid;

        protected readonly IObservable<bool> isValid;

        
        public bool HasErrors { get => _hasErrors; private set => this.RaiseAndSetIfChanged(ref _hasErrors, value); }
        public bool IsValid { get => _isValid; private set => this.RaiseAndSetIfChanged(ref _isValid, value); }

        public ReactiveValidationObject(IValidator _validator)
        {
            Errors = Enumerable.Empty<ValidationFailure>();
            validator = _validator ?? throw new ArgumentNullException(nameof(_validator));
            isValid = this.WhenAnyValue(x => x.IsValid)
                .Skip(1)
                .Select(_ => !HasErrors)
                .StartWith(false);
        }

        public IEnumerable GetErrors(string propertyName)
           => Errors.Where(x => x.PropertyName == propertyName);

        protected string RaiseValidation<T>(T validContext,params string[] propertyName) where T:class
        {
            var ret = validator.Validate(new ValidationContext<T>(validContext));
            Errors = ret.Errors;
            HasErrors = ret.Errors.Count > 0;
            IsValid = ret.IsValid;
            var errors = string.Empty;
            foreach (var item in propertyName)
            {
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(item));

                var propertyErrors = Errors.Where(x => x.PropertyName == item)
                    .Select(x => x.ErrorMessage);
                if(propertyErrors.Count() > 0)
                {
                    foreach(var error in propertyErrors)
                    {
                        errors = string.IsNullOrEmpty(errors) ? error : errors + Environment.NewLine + error;
                    }
                }
            }

            return errors;
        }
    }
}
