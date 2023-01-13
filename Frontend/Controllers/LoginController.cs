using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Testapplication1.Models;
using Testapplication1.Services;

namespace Testapplication1.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ProcessLogin(Rangers model)
        {
            if (model.Username == null || model.Password == null) 
            {
                TempData["LoginFlag"] = "You are required to fill in both fields";
                return RedirectToAction("Index", "Login");
            }
            if (UserDAO.FindUser(model)=="Admin")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, UserDAO.CurrentRanger.RangerName),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimIdentity = new ClaimsIdentity(claims, "Login");
                
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));
                return RedirectToAction("Index", "Admin");
            } 
            else if (UserDAO.FindUser(model) == "Ranger")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, UserDAO.CurrentRanger.RangerName),
                    new Claim(ClaimTypes.Role, "Ranger")
                };

                var claimIdentity = new ClaimsIdentity(claims, "Login");
                
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));
                return RedirectToAction("Index", "Notifications");
            }
            else
            {
                TempData["LoginFlag"] = "Incorrect username or password!";
                return RedirectToAction("Index", "Login");
            }
        }
        
        public async Task<IActionResult> ProcessLogout()
        {
            using (var context = new DatabaseConnect())
            {
                var ranger = context.Ranger.Where(x=>x.RangerID == UserDAO.CurrentRanger.RangerID).FirstOrDefault();
                ranger.LoggedIn = false;
                context.SaveChanges();
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }
    }
}