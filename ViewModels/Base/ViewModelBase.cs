using GestRehema.Entities;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace GestRehema.ViewModels
{
    public class ViewModelBase:ReactiveObject
    {
        public ViewModelBase()
        {
            User = Locator.Current.GetService<User>();
            Employee = User!.Employee!;
            Entreprise = User!.Employee!.Entreprise!;
            Title = "";

        }

        [Reactive]
        public User User { get; set; }

        [Reactive]
        public Entreprise Entreprise { get; set; }

        [Reactive]
        public Employee Employee { get; set; }

        [ObservableAsProperty]
        public bool IsBusy { get; }

        [Reactive]
        public string? Errors { get; set; }

        [Reactive]
        public string Title { get; set; }
    }
}
