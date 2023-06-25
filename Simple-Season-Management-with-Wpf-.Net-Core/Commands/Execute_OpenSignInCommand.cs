using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Simple_Season_Management_with_Wpf_.Net_Core.Commands
{
    public class Execute_OpenSignInCommand : CommandBase
    {
        public override void Execute(object? parameter)
        {
            var window = System.Windows.Application.Current.MainWindow;
            window.Hide();

            var signInWindow = new SignInWindow();
            signInWindow.ShowDialog();

            window.Show();
        }
    }
}
