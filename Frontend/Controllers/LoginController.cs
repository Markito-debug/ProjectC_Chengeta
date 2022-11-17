using Microsoft.AspNetCore.Mvc;
using Testapplication1.Database;
using Testapplication1.Services;
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
            if (UserDAO.FindUser(model)=="Admin")
            {
                return RedirectToAction("Index", "Admin");
            } 
            else if (UserDAO.FindUser(model) == "Ranger")
            {
                return RedirectToAction("Index", "Notifications");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}