using Simple_Season_Management_with_Wpf_.Net_Core.Helpers;
using Simple_Season_Management_with_Wpf_.Net_Core.Models;
using Simple_Season_Management_with_Wpf_.Net_Core.ViewModels;
using Simple_Season_Management_with_Wpf_.Net_Core.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Simple_Season_Management_with_Wpf_.Net_Core.Commands
{
    public class Execute_SignInCommand : CommandBase
    {
        private readonly SignInViewModel? _viewModel;
        private readonly UserDbContext _userDbContext;
        private readonly SessionManager _sessionManager = new SessionManager();

        public Execute_SignInCommand(SignInViewModel viewModel, UserDbContext userDbContext)
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _userDbContext = userDbContext ?? throw new ArgumentNullException(nameof(userDbContext));
        }

        public override void Execute(object? parameter)
        {
            try
            {
                if (!string.IsNullOrEmpty(_viewModel?.Username) && _viewModel.Password?.Length > 0)
                {
                    if (SecureStringHelper.CompareSecureStrings(_viewModel.Password, _viewModel.PasswordConfirmation))
                    {
                        string username = NormalizeUsername(_viewModel.Username);
                        SecureString password = _viewModel.Password;

                        int? userId = ValidateCredentials(username, password);
                        if (!userId.HasValue)
                        {
                            SignUp(username, password);
                            var currentWindow = System.Windows.Application.Current.MainWindow;
                            currentWindow.Hide();

                            var HomeWindow = new HomeWindow();
                            HomeWindow.Show();
                        }
                        else
                        {
                            throw new Exception("Username already.");
                        }
                    }
                    else
                    {
                        throw new Exception("Password not match.");
                    }
                }
                else
                {
                    throw new Exception("Invalid input.");
                }
            }
            catch (Exception ex)
            {
                if (_viewModel is not null)
                {
                    _viewModel.ErrorMessage = ex.Message;
                }
            }
        }

        private int? ValidateCredentials(string username, SecureString password)
        {
            var user = _userDbContext.Users.SingleOrDefault(u => u.UserName == username);
            if (user != null)
            {
                byte[] storedHash = Convert.FromBase64String(user.PasswordHash);
                byte[] storedSalt = Convert.FromBase64String(user.PasswordSalt);

                if (VerifyPassword(password, storedHash, storedSalt))
                    return user.User_id;
            }
            return null;
        }

        private bool VerifyPassword(SecureString password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var passwordPtr = IntPtr.Zero;
                string? readablePassword = null;
                try
                {
                    passwordPtr = Marshal.SecureStringToGlobalAllocUnicode(password);
                    readablePassword = Marshal.PtrToStringUni(passwordPtr);
                }
                finally
                {
                    Marshal.ZeroFreeGlobalAllocUnicode(passwordPtr);
                }

                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(readablePassword));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }
            return true;
        }

        public void SignUp(string username, SecureString password)
        {
            var existingUser = _userDbContext.Users.SingleOrDefault(u => u.UserName == username);
            if (existingUser != null)
            {
                MessageBox.Show("Username already exists.", "Season-Management", MessageBoxButton.OK, MessageBoxImage.Stop);
                return;
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            var users = new Users
            {
                UserName = username,
                PasswordHash = Convert.ToBase64String(passwordHash),
                PasswordSalt = Convert.ToBase64String(passwordSalt),
                Status = true
            };

            _userDbContext.Users.Add(users);
            _userDbContext.SaveChanges();
        }

        private void CreatePasswordHash(SecureString password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Marshal.PtrToStringBSTR(Marshal.SecureStringToBSTR(password))));
            }
        }

        public string NormalizeUsername(string username)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            username = username.ToLowerInvariant();
            username = username.Trim();
            username = System.Text.RegularExpressions.Regex.Replace(username, @"\s+", " ");
            username = System.Text.RegularExpressions.Regex.Replace(username, @"\W+", ""); // Remove invalid characters. Here only alphanumeric characters and underscores are allowed.
            return username;
        }
    }
}
