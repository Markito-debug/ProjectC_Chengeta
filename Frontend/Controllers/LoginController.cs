using Microsoft.AspNetCore.Mvc;
using Testapplication1.Models;
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
        
        public IActionResult ProcessLogout()
        {   
            // How to change the LoggedIn back to false
            using (var context = new DatabaseConnect())
            {
                //context.Rangers find current ranger and set LoggedIn to false
                //ranger.LoggedIn = false;
                context.SaveChanges();
            }
            return RedirectToAction("Index", "Login");
        }
    }
}