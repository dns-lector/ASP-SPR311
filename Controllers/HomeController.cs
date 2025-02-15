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
        // �������� - �������� �������� �� ������, ����������� � ��������� (���. Program.cs)
        // ������� �������� - _logger, ���� ��� � "�������"
        private readonly ILogger<HomeController> _logger;
        // ��� �������� ������ ������ ��������� ��������� ����� (������ readonly)
        private readonly ITimestampService _timestampService;

        // ������ �� ��������� ������������ ��������� �� ������
        public HomeController(ILogger<HomeController> logger, ITimestampService timestampService)
        {
            _logger = logger;
            // �������� �������� ��������� ��� ���������� ������
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
            // ���������� �� � �������� � ��� ��� �� ����� Register
            if (HttpContext.Session.Keys.Contains("HomeModelsFormModel"))
            {
                // ���������� ��'��� ����� � ������������� �����
                viewModel.FormModel =
                    JsonSerializer.Deserialize<HomeModelsFormModel>(
                        HttpContext.Session.GetString("HomeModelsFormModel")!
                    );

                // ��������� � ��� ������� ���
                HttpContext.Session.Remove("HomeModelsFormModel");
            }
            return View(viewModel);
        }

        public RedirectToActionResult Register(HomeModelsFormModel formModel)
        {
            HttpContext.Session.SetString(            // ���������� � ���
                "HomeModelsFormModel",                // �� ������ HomeModelsFormModel
                JsonSerializer.Serialize(formModel)   // ������������� ��'���� formModel
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

<form> ------------>Register->View()         | ������� ������� = ��������� 
                                   |         | ����� ���� ��������, �������,
   <----------------------------- HTML       | ������ �������� �����


!! ���������� HTML �� ������ � ������ (�������� �������) - ������ ���������


       POST /Register
<form> -------------->Register->X-->�������� ��� �� ���������� ������
                                |            | ��� ���������� ����� �� ��������
    <----------------------Redirect /Models  | ��������������� ����-���� https://learn.microsoft.com/ru-ru/aspnet/core/fundamentals/app-state?view=aspnetcore-9.0
    |
    |                     �������� ���
    |   GET /Models           |  
    ------------------>Models-->View()       | ��������� ������� - ���������
                                    |        | GET /Models, ��� �� ������ 
    <----------------------------- HTML      | ���




////////////////////// AJAX ///////////////////////

<form>
submit -->X
       preventDefault()
          |                 formData
         fetch--------------------------> Action()-->Json()
                            json                          |
         .then()<-----------------------------------------  
 */
