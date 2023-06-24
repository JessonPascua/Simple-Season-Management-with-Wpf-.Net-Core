using Simple_Season_Management_with_Wpf_.Net_Core.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Simple_Season_Management_with_Wpf_.Net_Core.Commands
{
    public class Execute_LogInCommand : CommandBase
    {
        private readonly ViewModel.ViewModel? _viewModel;
        private readonly SessionManager _sessionManager = new SessionManager();

        public Execute_LogInCommand(ViewModel.ViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        public override void Execute(object? parameter)
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
                    MessageBox.Show("Incorrect username or password.", "Season-Managemnent", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
            }
            else
            {
                MessageBox.Show("Invalid input.", "Season-Managemnent", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private int? ValidateCredentials(string username, SecureString password)
        {
            using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=Z:\mainline\database\reference\mainline.s3db;Version=3;"))
            {
                conn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM tblUser WHERE Username = @Username", conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            byte[] storedHash = Convert.FromBase64String(reader["PasswordHash"].ToString());
                            byte[] storedSalt = Convert.FromBase64String(reader["PasswordSalt"].ToString());

                            if (VerifyPassword(password, storedHash, storedSalt))
                                return Convert.ToInt32(reader["User_ID"]);
                            else
                                return null;
                        }
                        else
                        {
                            // Username not found
                            return null;
                        }
                    }
                }
            }
        }

        private bool VerifyPassword(SecureString password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                // Convert SecureString password to string
                var passwordPtr = IntPtr.Zero;
                string readablePassword = null;
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
