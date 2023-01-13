using System.Security.Cryptography;
using System.Text;
using Testapplication1.Models;

namespace Testapplication1.Services;

public class UserDAO
{
    private static string hashedPassword;
    public static Rangers? CurrentRanger;

    public static string FindUser(Rangers ranger)
    {

        using (var context = new DatabaseConnect())
        {
            hashedPassword = ComputeSha256Hash(ranger.Password);
            var rangerFound = (from r in context.Ranger
                where r.Username == ranger.Username
                where r.Password == hashedPassword
                select r).FirstOrDefault();
            if (rangerFound == null)
            {
                return "null";
            }
            else
            {
                CurrentRanger = rangerFound;
                if (rangerFound.IsAdmin == true)
                {
                    rangerFound.LoggedIn = true;
                    context.SaveChanges();
                    return "Admin";
                }
                else
                {
                    rangerFound.LoggedIn = true;
                    context.SaveChanges();
                    return "Ranger";
                }
            }
        }
    }

    public static void FindAndDeleteUser(Guid? id)
    {
        using (var context = new DatabaseConnect())
        {
            var removeRanger = (from r in context.Ranger 
                                where r.RangerID == id
                                select r).FirstOrDefault(); 

            if(removeRanger != null)
            {
                context.Ranger.Remove(removeRanger);
                context.SaveChanges();
            }
        }
    }
 
    public static void AddUser(Rangers ranger)
    {
        using (var context = new DatabaseConnect())
        {
            hashedPassword = ComputeSha256Hash(ranger.Password);
            var newRanger = new Rangers(Guid.NewGuid(), ranger.RangerName, ranger.Username, hashedPassword, ranger.PhoneNumber, ranger.Email, ranger.IsAdmin);
            newRanger.LoggedIn = false;
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
