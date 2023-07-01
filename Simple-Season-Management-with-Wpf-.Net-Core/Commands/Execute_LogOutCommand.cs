using Simple_Season_Management_with_Wpf_.Net_Core.Helpers;
using Simple_Season_Management_with_Wpf_.Net_Core.Models;
using Simple_Season_Management_with_Wpf_.Net_Core.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Simple_Season_Management_with_Wpf_.Net_Core.Commands
{
    public class Execute_LogOutCommand : CommandBase
    {
        private readonly SessionManager _sessionManager;
        public Execute_LogOutCommand(SessionManager sessionManager)
        {
            _sessionManager = sessionManager ?? throw new ArgumentNullException(nameof(sessionManager));
        }

        public override void Execute(object? parameter)
        {
            var window = System.Windows.Application.Current.MainWindow;

            var loginWindow = new LoginWindow();
            Application.Current.MainWindow = loginWindow;
            loginWindow.Show();
            window.Close();
            _sessionManager.DeleteSession();
        }
    }
}
