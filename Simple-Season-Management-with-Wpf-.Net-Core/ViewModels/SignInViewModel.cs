using Simple_Season_Management_with_Wpf_.Net_Core.Commands;
using Simple_Season_Management_with_Wpf_.Net_Core.Helpers;
using Simple_Season_Management_with_Wpf_.Net_Core.Models;
using System.Security;
using System.Windows.Input;

namespace Simple_Season_Management_with_Wpf_.Net_Core.ViewModels
{
    public class SignInViewModel : ViewModelBase
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

        private SecureString _passwordConfirmation = new();
        public SecureString PasswordConfirmation
        {
            get { return _passwordConfirmation; }
            set
            {
                _passwordConfirmation = value;
                OnPropertyChanged(nameof(PasswordConfirmation));
            }
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }


        public ICommand? OpenSignInCommand { get; set; }
        public ICommand? SignInCommand { get; set; }

        public SignInViewModel()
        {
            try
            {
                var dbContext = ServiceLocator.GetService<UserDbContext>();

                SignInCommand = new Execute_SignInCommand(this, dbContext);
            }
            catch (System.Exception) { }
        }
    }
}
