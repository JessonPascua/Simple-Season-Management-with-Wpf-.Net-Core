using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Simple_Season_Management_with_Wpf_.Net_Core.Helpers;
using Simple_Season_Management_with_Wpf_.Net_Core.Models;
using Simple_Season_Management_with_Wpf_.Net_Core.ViewModels;
using Simple_Season_Management_with_Wpf_.Net_Core.Views;
using System.Threading;
using System.Windows;

namespace Simple_Season_Management_with_Wpf_.Net_Core
{
    public partial class App : Application
    {
        private static Mutex? _mutex = null;
        public static IHost? _host { get; private set; }
        private readonly SessionManager? _sessionManager;

        public App()
        {
            const string appName = "thisApp";
            _mutex = new Mutex(true, appName, out bool createdNew);

            if (!createdNew)
            {
                Current.Shutdown();
            }

            _host = Host.CreateDefaultBuilder()
              .ConfigureAppConfiguration((hostingContext, builder) =>
              {
                  builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
              })
              .ConfigureServices((hostContext, services) =>
              {
                  var connectionString = hostContext.Configuration.GetConnectionString("SQLiteConnection");
                  services.AddDbContext<UserDbContext>(options => options.UseSqlite(connectionString));

                  services.AddTransient<LoginViewModel>();
                  services.AddTransient<SignInViewModel>();
                  services.AddSingleton<SessionManager>();
                  services.AddSingleton<ViewModelLocator>();
              })
              .Build();

            if (_host is not null)
            {
                _sessionManager = _host.Services.GetRequiredService<SessionManager>();
                ServiceLocator.ServiceProvider = _host.Services;
            }
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            await _host!.StartAsync();

            // Get the ViewModelLocator from the DI container and add it to the resources
            var viewModelLocator = _host.Services.GetRequiredService<ViewModelLocator>();
            Resources.Add("ViewModelLocator", viewModelLocator);

            //var applicationServices = _host.Services.GetRequiredService<ApplicationInitializeServices>();
            //applicationServices.Initialize();

            Window startup;
            int? userId = _sessionManager?.GetCurrentUserId();

            if (userId.HasValue)
            {
                MessageBox.Show($"User is logged in with id: {userId.Value}");
                startup = new HomeWindow();
            }
            else
            {
                startup = new LoginWindow();
                MessageBox.Show("No valid session found.");
            }

            startup.Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            await _host!.StopAsync();
            _mutex?.ReleaseMutex();
            _mutex?.Dispose();
        }
    }
}
