using Simple_Season_Management_with_Wpf_.Net_Core.Commands;
using Simple_Season_Management_with_Wpf_.Net_Core.Helpers;
using Simple_Season_Management_with_Wpf_.Net_Core.Models;
using System.Security;
using System.Windows.Input;

namespace Simple_Season_Management_with_Wpf_.Net_Core.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private string? _username;
        public string? Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private SecureString _password = new();
        public SecureString Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand? SignInCommand { get; set; }
        public ICommand? LogInCommand { get; set; }
        public ICommand? LogOutCommand { get; set; }

        public UserViewModel()
        {
            try
            {
                var dbContext = ServiceLocator.GetService<UserDbContext>();

                SignInCommand = new Execute_OpenSignInCommand();
                LogInCommand = new Execute_LogInCommand(this, dbContext);
                LogOutCommand = new Execute_LogOutCommand();
            }
            catch (System.Exception) { }
        }
    }
}
