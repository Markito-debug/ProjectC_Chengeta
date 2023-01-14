using System.Security.Cryptography;
using System.Text;

namespace Mqttlistener
{
    class Program
    {
        public static void Main(string[] args)
        {
            Client.client();
            AddRangers();
        }

        public static void AddRangers()
        {
            var dbContext = new ListenerDb();
            var newRanger = new Rangers(Guid.NewGuid(), "Bob", "Bob", ComputeSha256Hash("12345"), 813507673, "bob@mail.com", false);
            dbContext.Add(newRanger);
            dbContext.SaveChanges();
        }
        
        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
    
    
    
}