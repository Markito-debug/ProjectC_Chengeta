using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Testapplication1.Models;
using Testapplication1.Services;

namespace Testapplication1.Controllers
{
    public class RangerController : Controller
    {
        private readonly ILogger<RangerController> _logger;
        public RangerController(ILogger<RangerController> logger)
        {
            this._logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}

