using Newtonsoft.Json;
using System;

namespace Simple_Season_Management_with_Wpf_.Net_Core.Helpers
{
    public class SessionManager
    {
        private  readonly string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "session.txt");

        public void SaveSession(int userId)
        {
            var token = new
            {
                UserId = userId,
                ExpiryDate = DateTime.Now.AddHours(10)
            };

            var jsonString = JsonConvert.SerializeObject(token);

            // Encrypt jsonString here

            System.IO.File.WriteAllText(filePath, jsonString);
        }

        public void DeleteSession()
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
        public int? GetCurrentUserId()
        {
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }

            var encryptedToken = System.IO.File.ReadAllText(filePath);

            // Decrypt encryptedToken here

            var token = JsonConvert.DeserializeObject<dynamic>(encryptedToken);

            DateTime expiryDate = token?.ExpiryDate; // <-- Convert JValue to DateTime

            if (DateTime.Now > expiryDate) // <-- Now the comparison should work
            {
                // Session has expired
                System.IO.File.Delete(filePath);
                return null;
            }

            return token?.UserId;
        }
    }
}
