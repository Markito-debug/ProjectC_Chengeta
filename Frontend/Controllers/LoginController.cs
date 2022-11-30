﻿using Microsoft.AspNetCore.Mvc;
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
                return RedirectToAction("Index", "Ranger");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        
        public IActionResult ProcessLogout()
        {
            using (var context = new DatabaseConnect())
            {
                var ranger = context.Ranger.Where(x=>x.RangerID == UserDAO.CurrentRanger.RangerID).First();
                ranger.LoggedIn = false;
                context.SaveChanges();
            }
            return RedirectToAction("Index", "Login");
        }
    }
}