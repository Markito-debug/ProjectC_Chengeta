using Microsoft.AspNetCore.Mvc;
using Testapplication1.Database;
using Testapplication1.Views.Services;

namespace Testapplication1.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProcessLogin(Rangers model)
        {
            SecurityService security= new SecurityService();

            if (security.IsValid(model))
            {
                return View("standin");
            } 
            else
            {
                return View("LoginFailure", model);
            }
            
        }
    }
}