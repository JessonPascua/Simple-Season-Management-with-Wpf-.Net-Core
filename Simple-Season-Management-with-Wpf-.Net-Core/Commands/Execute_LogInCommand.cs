using Simple_Season_Management_with_Wpf_.Net_Core.Helpers;
using Simple_Season_Management_with_Wpf_.Net_Core.Models;
using Simple_Season_Management_with_Wpf_.Net_Core.ViewModels;
using Simple_Season_Management_with_Wpf_.Net_Core.Views;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
#pragma warning disable CS8604

namespace Simple_Season_Management_with_Wpf_.Net_Core.Commands
{
    public class Execute_LogInCommand : CommandBase
    {
        private readonly LoginViewModel? _viewModel;
        private readonly UserDbContext _userDbContext;
        private readonly SessionManager _sessionManager = new SessionManager();

        public Execute_LogInCommand(LoginViewModel viewModel, UserDbContext userDbContext)
        {
            _viewModel = viewModel;
            _userDbContext = userDbContext;
        }
        public override void Execute(object? parameter)
        {
            try
            {
                if (!string.IsNullOrEmpty(_viewModel?.Username) && _viewModel.Password?.Length > 0)
                {
                    string username = _viewModel.Username;
                    SecureString password = _viewModel.Password;

                    int? userId = ValidateCredentials(username, password);
                    if (userId.HasValue)
                    {
                        _sessionManager.SaveSession(userId.Value);
                        var startup = new HomeWindow();
                        startup.Show();
                    }
                    else
                    {
                         throw new Exception("Incorrect username or password.");
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
            // Username not found or password verification failed
            return null;
        }

        private bool VerifyPassword(SecureString password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                // Convert SecureString password to string
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
    }
}
