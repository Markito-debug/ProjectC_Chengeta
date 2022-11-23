using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Testapplication1.Database;
using Testapplication1.Views.Notifications;

namespace Testapplication1.Controllers
{
    public class NotificationsController : Controller
    {
        public IActionResult Index()
        {

            using (var context = new DatabaseConnect())
            {
                var recentNotifs = context.Notifs.OrderByDescending(x => x.Time).Take(10).ToList();
                return recentNotifs != null ?
                                View(recentNotifs) :
                                Problem("Entity set 'DBModel.Notifs'  is null.");
            }

        }

        public async Task<IActionResult> Details(Guid? id)
        {
            using (var context = new DatabaseConnect())
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

        public IActionResult SaveNotification(Guid id)
        {
            SaveNotif.SavedNotification(id);
            return RedirectToAction("Index", "Notifications");
        }
    }
}
