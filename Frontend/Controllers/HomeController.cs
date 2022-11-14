using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Testapplication1.Database;
using Testapplication1.Models;

namespace Testapplication1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult NotificationInfo()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DatabaseConnect>();
        optionsBuilder.UseNpgsql("Host=localhost:5432;Username=postgres;Password=1109;Database=Chengeta");
        using (var context = new DatabaseConnect(optionsBuilder.Options))
        {
            var getNotif = (from n in context.Notifs
                select n).ToList();
            
            Random rnd = new Random();
            int index = rnd.Next(getNotif.Count());
            
            ViewBag.Time = getNotif[index].Time;
            ViewBag.Sound_Type = getNotif[index].Sound_Type;
            ViewBag.Probability = getNotif[index].Probability;
            ViewBag.Sound = getNotif[index].Sound;            
        }

        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}