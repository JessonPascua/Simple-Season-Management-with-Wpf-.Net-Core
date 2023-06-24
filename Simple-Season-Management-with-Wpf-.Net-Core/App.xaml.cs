using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Simple_Season_Management_with_Wpf_.Net_Core
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex? _mutex = null;

        public App()
        {
            const string appName = "thisApp";
            _mutex = new Mutex(true, appName, out bool createdNew);

            if (!createdNew)
            {
                Current.Shutdown();
            }

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var startup = new LoginWindow();
            startup.Show();
        }

        protected override  void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _mutex?.ReleaseMutex();
        }
    }
}
