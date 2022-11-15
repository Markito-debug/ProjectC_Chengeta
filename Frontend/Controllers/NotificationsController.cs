﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Testapplication1.Database;

namespace Testapplication1.Controllers
{
    public class NotificationsController : Controller
    {

        // GET: Notifications
        public IActionResult Index()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseConnect>();
            optionsBuilder.UseNpgsql("Host=localhost:5432;Username=postgres;Password=blub;Database=Chengeta");
            using (var context = new DatabaseConnect(optionsBuilder.Options))
            {
                var recentNotifs = context.Notifs.OrderByDescending(x => x.Time).Take(10).ToList();
                return recentNotifs != null ?
                                View(recentNotifs) :
                                Problem("Entity set 'DBModel.Notifs'  is null.");
            }

        }

        // GET: Notifications/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DatabaseConnect>();
            optionsBuilder.UseNpgsql("Host=localhost:5432;Username=postgres;Password=blub;Database=Chengeta");
            using (var context = new DatabaseConnect(optionsBuilder.Options))
            {
                if (id == null || context.Notifs == null)
                {
                    return NotFound();
                }

                var notification = await context.Notifs
                    .FirstOrDefaultAsync(m => m.ID == id);
                if (notification == null)
                {
                    return NotFound();
                }

                ViewBag.Sound = notification.Sound;
                return View(notification);

            }

        }
    }
}
