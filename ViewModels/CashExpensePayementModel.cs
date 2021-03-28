using GestRehema.Entities;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System;
using GestRehema.Contants;

namespace GestRehema.ViewModels
{
    public class CashExpensePayementModel : BasePayementModel
    {
        public CashExpensePayementModel(Wallet wallet, Entreprise entreprise,List<string> categories) : base(wallet, entreprise, PayementType.EntrepriseAccountDeposit)
        {
            Categories = categories;
            Title = null!;
            Description = null!;
            Owner = null!;
            Category = null!;

            Validate = ReactiveCommand.Create<Unit, (bool IsValid, string Message)>(_ =>
             {
                 if (string.IsNullOrEmpty(Title))
                     return (false,"Le titre est obligatoire");

                 if (string.IsNullOrEmpty(Owner))
                     return (false, "Veuillez spécifier la personne ayant effectué la dépense");

                 if (string.IsNullOrEmpty(Description))
                     return (false, "Veuillez spécifier la description de cette dépense");
                 if (string.IsNullOrEmpty(Category))
                     return (false, "Veuillez spécifier une catégorie pour cette dépense");

                 return (true, string.Empty);
             });

            Validate
                .Where(x => x.IsValid)
                .Select(_ => Unit.Default)
                .InvokeCommand(Pay);

            Validate
                .Where(x => !x.IsValid)
                .Subscribe(x => Errors = x.Message);
        }

        [Reactive]
        public string Title { get; set; }

        [Reactive]
        public string Description { get; set; }

        [Reactive]
        public string Owner { get; set; }

        [Reactive]
        public string Category { get; set; }

        public List<string> Categories { get; }

        public ReactiveCommand<Unit,(bool IsValid, string Message)> Validate { get; }


    }
}
