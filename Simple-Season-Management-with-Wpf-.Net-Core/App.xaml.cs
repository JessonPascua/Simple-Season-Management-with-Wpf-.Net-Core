using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Simple_Season_Management_with_Wpf_.Net_Core.Helpers;
using Simple_Season_Management_with_Wpf_.Net_Core.Models;
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
        public static IHost? _host { get; set; }

        private readonly SessionManager _sessionManager = new();
        public App()
        {
            const string appName = "thisApp";
            _mutex = new Mutex(true, appName, out bool createdNew);

            if (!createdNew)
            {
                Current.Shutdown();
            }

            _host = Host.CreateDefaultBuilder()
                 .ConfigureAppConfiguration((context, builder) =>
                 {
                     builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                 })
                .ConfigureServices((hostContext, services) =>
                {
                    var connectionString = hostContext.Configuration.GetConnectionString("SQLiteConnection");
                    services.AddDbContext<UserDbContext>(options => options.UseSqlite(connectionString));
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            int? userId = _sessionManager.GetCurrentUserId();

            if (userId.HasValue)
            {
                MessageBox.Show($"User is logged in with id: {userId.Value}");
                var startup = new HomeWindow();
                startup.Show();
            }
            else
            {
                var startup = new LoginWindow();
                startup.Show();
                MessageBox.Show("No valid session found.");
            }

        }

        protected override  void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _mutex?.ReleaseMutex();
        }
    }
}
