using System.Diagnostics;
using System.Text.Json;
using ASP_SPR311.Models;
using ASP_SPR311.Models.Home;
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

        public ViewResult Models()
        {
            HomeModelsViewModel viewModel = new();
            // перевіряємо чи є збережені у сесії дані від форми Register
            if (HttpContext.Session.Keys.Contains("HomeModelsFormModel"))
            {
                // відновлюємо об'єкт моделі з серіалізованого стану
                viewModel.FormModel =
                    JsonSerializer.Deserialize<HomeModelsFormModel>(
                        HttpContext.Session.GetString("HomeModelsFormModel")!
                    );

                // видаляємо з сесії вилучені дані
                HttpContext.Session.Remove("HomeModelsFormModel");
            }
            return View(viewModel);
        }

        public RedirectToActionResult Register(HomeModelsFormModel formModel)
        {
            HttpContext.Session.SetString(            // Збереження у сесії
                "HomeModelsFormModel",                // під ключем HomeModelsFormModel
                JsonSerializer.Serialize(formModel)   // серіалізованого об'єкту formModel
            );                                        // 
            return RedirectToAction(nameof(Models));
        }

        public JsonResult Ajax(HomeModelsFormModel formModel)
        {
            return Json(formModel);
        }
        public JsonResult AjaxJson([FromBody] HomeAjaxFormModel formModel)
        {
            return Json(formModel);
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
/*

Browser            Server(ASP)

<form> ------------>Register->View()         | Оновити сторінку = повторити 
                                   |         | даний блок операцій, зокрема,
   <----------------------------- HTML       | наново надіслати форму


!! Формування HTML на запити з даними (особливо формами) - вимагає перегляду


       POST /Register
<form> -------------->Register->X-->Зберегти дані до наступного запиту
                                |            | Для збереження даних між запитами
    <----------------------Redirect /Models  | використовується НТТР-сесія https://learn.microsoft.com/ru-ru/aspnet/core/fundamentals/app-state?view=aspnetcore-9.0
    |
    |                     відновити дані
    |   GET /Models           |  
    ------------------>Models-->View()       | Оновлення сторінки - оновлення
                                    |        | GET /Models, яка не передає 
    <----------------------------- HTML      | дані




////////////////////// AJAX ///////////////////////

<form>
submit -->X
       preventDefault()
          |                 formData
         fetch--------------------------> Action()-->Json()
                            json                          |
         .then()<-----------------------------------------  
 */
