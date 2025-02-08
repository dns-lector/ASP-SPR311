using System.Diagnostics;
using ASP_SPR311.Models;
using ASP_SPR311.Services.Timestamp;
using Microsoft.AspNetCore.Mvc;

namespace ASP_SPR311.Controllers
{
    public class HomeController : Controller
    {
        // Інжекція - передача посилань на служби, зареєстровані у контейнері (див. Program.cs)
        // Приклад інжекції - _logger, який іде з "коробки"
        private readonly ILogger<HomeController> _logger;
        // для інжекції іншого сервісу необхідно оголосити змінну (бажано readonly)
        private readonly ITimestampService _timestampService;

        // додаємо до параметрів конструктора залежність від сервісу
        public HomeController(ILogger<HomeController> logger, ITimestampService timestampService)
        {
            _logger = logger;
            // зберігаємо передане посилання для подальшого вжитку
            _timestampService = timestampService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Intro()
        {
            return View();
        }
        
        public IActionResult Razor()
        {
            return View();
        }
        
        public IActionResult IoC()
        {
            ViewData["timestamp"] = _timestampService.Timestamp;
            ViewData["timestampCode"] = _timestampService.GetHashCode();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
