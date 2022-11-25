﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Testapplication1.Database;
using Testapplication1.Models;
using Testapplication1.Services;

namespace Testapplication1.Controllers;

public class AdminController : Controller
{
    private readonly ILogger<AdminController> _logger;

    public AdminController(ILogger<AdminController> logger)
    {
        _logger = logger;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult AddRanger()
    {
        return View();
    }
    public IActionResult DeleteRanger()
    {
        using (var context = new DatabaseConnect())
        {
            var allRangers = context.Ranger.OrderBy(x => x.RangerName).ToList();
            return allRangers != null ?
                View(allRangers) :
                Problem("Entity set 'DBModel.Ranger'  is null.");
        }
    }
  public IActionResult ShowResult(string Searched)
   {
       using (var context = new DatabaseConnect())
       {
           var searched = context.Ranger.Where(s=>s.RangerName.Contains(Searched)/* || s.Email.Contains(Searched)*/).ToList();
           return searched != null ?
               View(searched) :
               Problem("No ranger is found.");
       }
   }    
    public IActionResult ProcessDeleteRanger(Guid? id)
    {
        UserDAO.FindAndDeleteUser(id);
        return RedirectToAction("Index", "Admin");
    }
    
    public IActionResult ProcessAddRanger(Rangers model)
    {
        UserDAO.AddUser(model);
        return RedirectToAction("Index", "Admin");
    }
    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}