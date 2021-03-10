using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
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
            AppCenter.Start("32f60010-1949-405c-a5ab-51b9c0cd6be9", typeof(Analytics), typeof(Crashes));

            var connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            _appBootstrapper = new AppBootstrapper(connectionString);

            FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;
        }
    }
}
