using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Testapplication1.Models;
using Testapplication1.Services;
using Testapplication1.Views.Services;

namespace Testapplication1.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {

       public IActionResult Index()
        {

            using (var context = new DatabaseConnect())
            {
                var recentNotifs = context.Notifs.OrderByDescending(x => x.Time).Take(10).ToList();
                return recentNotifs != null ?
                                View(recentNotifs) :
                                Problem("Entity set 'DBModel.Notifs' is null.");
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


        public JsonResult GetBranch()
        {
            using (var context = new DatabaseConnect())
            {
                List<Notification> branches = new List<Notification>();
                var recentNotifs = context.Notifs.OrderByDescending(x => x.Time).Take(10).ToList();
                return Json(recentNotifs);
            }
        }  
            
        public IActionResult StatusToOpen(Guid id)
        {
            using (var context = new DatabaseConnect())
            {
                var statusChange = context.Notifs.Where(x => x.ID == id).FirstOrDefault();
                if (!(statusChange.Status == "Open"))
                {
                    statusChange.Status = "Open";
                }
                
                context.SaveChanges();
            }

            return RedirectToAction("Details", "Notifications", new { ID = id });
        }

        public IActionResult StatusToProgress(Guid id)
        {
            using (var context = new DatabaseConnect())
            {
                var statusChange = context.Notifs.Where(x => x.ID == id).FirstOrDefault();
                if (!(statusChange.Status == "In Progress"))
                {
                    statusChange.Status = "In Progress";
                }

                var rangerNotifCon = new ConnectionTable(Guid.NewGuid(), UserDAO.CurrentRanger.RangerID, id);
                context.Connections.AddRange(rangerNotifCon);
                context.SaveChanges();
            }

            return RedirectToAction("Details", "Notifications", new { ID = id });
        }

        public IActionResult StatusToClosed(Guid id)
        {
            using (var context = new DatabaseConnect())
            {
                var statusChange = context.Notifs.Where(x => x.ID == id).FirstOrDefault();
                if (!(statusChange.Status == "Closed"))
                {
                    statusChange.Status = "Closed";
                }

                context.SaveChanges();
            }

            return RedirectToAction("Details", "Notifications", new { ID = id });
        }

        public IActionResult ReturnHome()
        {
            if (UserDAO.CurrentRanger.IsAdmin)
            {
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index", "Notifications");
        }
    }
}
    
