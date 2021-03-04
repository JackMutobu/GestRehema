using GestRehema.Views;
using ModernWpf;
using ReactiveUI;
using System.Reactive.Linq;
using System.Windows;
using System;
using Squirrel;

namespace GestRehema
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new LoginView(this);

            
            ThemeButton.Click += ThemeButton_Click;
            this.Events()
                .Loaded
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(async _ =>
                {
                    try
                    {
                        using var updateManager = new UpdateManager(@"E:\Project2021\FabProject\Publish");
                        await updateManager.UpdateApp();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                },ex => MessageBox.Show(ex.Message));
        }


        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ThemeManager.Current.ActualApplicationTheme == ApplicationTheme.Dark)
            {
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
            }
            else
            {
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
            }
        }

    }
}
