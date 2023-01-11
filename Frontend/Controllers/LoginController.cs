using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Testapplication1.Models;
using Testapplication1.Services;

namespace Testapplication1.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
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

            var findRanger = UserDAO.FindUser(model);
            if (findRanger != null)
            {
                if (findRanger.IsAdmin)
                {
                    var token = GenerateToken(UserDAO.CurrentRanger);
                    
                    Response.Cookies.Append(Constants.ChengetaToken, token, new CookieOptions
                    {
                        HttpOnly = true, SameSite = SameSiteMode.Strict
                    } );
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    var token = GenerateToken(UserDAO.CurrentRanger);
                    Response.Cookies.Append(Constants.ChengetaToken, token, new CookieOptions
                    {
                        HttpOnly = true, SameSite = SameSiteMode.Strict
                    } );
                    return RedirectToAction("Index", "Notifications");
                }
            }
            TempData["LoginFlag"] = "Incorrect username or password!";
            return RedirectToAction("Index", "Login");
        }

        private string GenerateToken(Rangers ranger)
        {
            string role;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            if (ranger.IsAdmin)
            {
                role = "Admin";
            }
            else
            {
                role = "Ranger";
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, ranger.RangerID.ToString()),
                new Claim(ClaimTypes.NameIdentifier, ranger.Username),
                new Claim(ClaimTypes.Email, ranger.Email),
                new Claim(ClaimTypes.Role, role),
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public IActionResult ProcessLogout()
        {
            using (var context = new DatabaseConnect())
            {
                var ranger = context.Ranger.Where(x => x.RangerID == UserDAO.CurrentRanger.RangerID).FirstOrDefault();
                ranger.LoggedIn = false;
                context.SaveChanges();
            }

            return RedirectToAction("Index", "Login");
        }
    }
}