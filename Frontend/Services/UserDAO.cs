using Microsoft.EntityFrameworkCore;
using Testapplication1.Database;

namespace Testapplication1.Services;

public class UserDAO
{
    public static string FindUser(Rangers ranger)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseConnect>();
        optionsBuilder.UseNpgsql("Host=localhost:5432;Username=postgres;Password=blub;Database=Chengeta");
        using (var context = new DatabaseConnect(optionsBuilder.Options))
        {
            var rangerFound = (from r in context.Ranger
                where r.Login == ranger.Login
                where r.Password == ranger.Password
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
}
