using ASP_SPR311.Data;
using ASP_SPR311.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace ASP_SPR311.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class ApiCategoryController(DataAccessor dataAccessor) : ControllerBase
    {
        private readonly DataAccessor _dataAccessor = dataAccessor;

        [HttpGet]
        public RestResponse Categories()   // назва - довільна, маршрутизація за [HttpGet]
        {
            return new() 
            { 
                Service = "API Products Categories",
                DataType = "array",
                CacheTime = 600,
                Data = _dataAccessor.AllCategories(),
            };
        }

        [HttpGet("{id}")]
        public RestResponse Category(String id)
        {
            return new()
            {
                Service = "API Products Categories",
                DataType = "object",
                CacheTime = 600,
                Data = _dataAccessor.GetCategory(id),
            };
        }
    }
}
/*
CORS policy (CORP)  https://learn.microsoft.com/en-us/aspnet/core/security/cors
Cross-Origin Resource Sharing - правила міждоменного обміну ресурсами
Дані від сервера з іншого хосту/порту/протоколу (http/https)
мають бути проігноровані якщо в них немає позначок про:
загальний доступ -  Access-Control-Allow-Origin
дозволені методи -  Access-Control-Allow-Methods
дозволені заголовки -  Access-Control-Allow-Headers
+ перед надсиланням основного запиту (окрім GET) необхідно 
  надіслати попередній запит (preflight) методом OPTIONS, 
  одержати відомості про
  CORS і, за їх відсутності чи невідповідності, не надсилати основний запит.


MVC Controller:                    API Controller

однакові методи запиту,            Однакова адреса,
маршрутизація за назвами           маршрутизація за методами запиту
GET /Shop/Category                 GET  /api/category
GET /Shop/Cart                     POST /api/category
GET /Shop/Product                  PUT  /api/category

Повернення IActionResult           Повернення object/string
частіше за все View(html)          Частіше за все JSON (object)

Більша функціональність,           Менша функціональність,
складніші                          легші
 
Д.З. Реалізувати сервіс "популярні товари", який видає 5 товарів з найбільшими
залишками. На домашній сторінці сайту додати підрозділ "популярні товари"
в нижній частині сторінки.

 */