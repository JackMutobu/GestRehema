using GestRehema.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using GestRehema.Extensions;
using System.Reactive.Linq;
using System;
using Splat;
using GestRehema.Entities;
using System.Reactive;

namespace GestRehema.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Page, IViewFor<LoginViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
        .Register(nameof(ViewModel), typeof(LoginViewModel), typeof(LoginView));

        public LoginView()
        {

        }

        public LoginView(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            ViewModel = new LoginViewModel();

            this.WhenActivated(disposable =>
            {
                this.WhenAnyValue(x => x.ViewModel)
                    .BindTo(this, x => x.DataContext);

                this.BindCommand(ViewModel, vm => vm.Login, v => v.BtnLogin)
                .DisposeWith(disposable);

                this.WhenAnyValue(x => x.ViewModel!.Errors)
                .SubscribeOn(RxApp.MainThreadScheduler)
                .Subscribe(value =>
                {
                    TxtError.Visibility = value?.ToVisibility() ?? Visibility.Collapsed;
                    TxtError.Text = value;
                });

                this.ViewModel.Login
                .SubscribeOn(RxApp.MainThreadScheduler)
                    .Where(x => x != null)
                    .Subscribe(x => 
                    {
                        Locator.CurrentMutable.RegisterConstant(x);
                        _mainWindow.Content = new NavigationRootView();
                    });

                this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.ProgIndicator.Visibility, value => value.ToVisibility());

            });
        }

        private MainWindow _mainWindow;

        public LoginViewModel? ViewModel
        {
            get => (LoginViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object? IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (LoginViewModel)value!;
        }
    }
}
