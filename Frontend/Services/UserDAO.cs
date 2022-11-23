using Microsoft.EntityFrameworkCore;
using Testapplication1.Database;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Npgsql;

namespace Testapplication1.Services;

public class UserDAO
{
    private static string hashedPassword;

    public static string FindUser(Rangers ranger)
    {

        using (var context = new DatabaseConnect())
        {
            hashedPassword = ComputeSha256Hash(ranger.Password);
            var rangerFound = (from r in context.Ranger
                where r.Login == ranger.Login
                where r.Password == hashedPassword
                select r).FirstOrDefault();
            if (rangerFound == null)
            {
                return "null";
            }
            else
            {
                if (rangerFound.IsAdmin == true)
                {
                    return "Admin";
                }
                else
                {
                    return "Ranger";
                }
            }
        }
    }
    public static void AddUser(Rangers ranger)
    {
        using (var context = new DatabaseConnect())
        {
            hashedPassword = ComputeSha256Hash(ranger.Password);
            var newRanger = new Rangers(Guid.NewGuid(), ranger.RangerName, ranger.Login, hashedPassword, ranger.PhoneNumber, ranger.Email, ranger.IsAdmin);
            context.Ranger.AddRange(newRanger);
            context.SaveChanges();
        }
    }
    static string ComputeSha256Hash(string rawData)
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
