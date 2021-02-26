using System.Configuration;
using System.Windows;

namespace GestRehema
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private  AppBootstrapper? _appBootstrapper;

        protected override void OnStartup(StartupEventArgs e)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            _appBootstrapper = new AppBootstrapper(connectionString);

            FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;
        }
    }
}
