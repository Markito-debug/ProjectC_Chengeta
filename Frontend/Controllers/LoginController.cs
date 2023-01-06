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
            if (model.Username == null || model.Password == null) 
            {
                TempData["LoginFlag"] = "You are required to fill in both fields";
                return RedirectToAction("Index", "Login");
            }
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
                TempData["LoginFlag"] = "Incorrect username or password!";
                return RedirectToAction("Index", "Login");
            }
        }
        
        public IActionResult ProcessLogout()
        {
            using (var context = new DatabaseConnect())
            {
                var ranger = context.Ranger.Where(x=>x.RangerID == UserDAO.CurrentRanger.RangerID).FirstOrDefault();
                ranger.LoggedIn = false;
                context.SaveChanges();
            }
            return RedirectToAction("Index", "Login");
        }
    }
}