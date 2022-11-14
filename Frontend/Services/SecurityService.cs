using Testapplication1.Database;

namespace Testapplication1.Views.Services;

public class SecurityService
{
    public SecurityService()
    {

    }

    public bool IsValid(Rangers model)
    {
        return UserDAO.FindUser(model);
    }  
}