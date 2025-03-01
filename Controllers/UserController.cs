using ASP_SPR311.Data;
using ASP_SPR311.Models.User;
using ASP_SPR311.Services.Kdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;

namespace ASP_SPR311.Controllers
{
    public class UserController(DataContext dataContext, IKdfService kdfService) : Controller
    {
        private const String signupFormKey = "UserSignupFormModel";
        private readonly DataContext _dataContext = dataContext;
        private readonly IKdfService _kdfService = kdfService;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Signup()
        {
            UserSignupViewModel viewModel = new();

            // перевіряємо чи є збережені у сесії дані від форми Register
            if (HttpContext.Session.Keys.Contains(signupFormKey))
            {
                // відновлюємо об'єкт моделі з серіалізованого стану
                viewModel.FormModel =
                    JsonSerializer.Deserialize<UserSignupFormModel>(
                        HttpContext.Session.GetString(signupFormKey)!
                    );
                // проводимо валідацію переданих даних
                viewModel.ValidationErrors = ValidateSignupFormModel(viewModel.FormModel);

                // Перевіряємо, якщо немає помилок валідації, то реєструємо у БД
                if(viewModel.ValidationErrors.Count == 0)
                {
                    Guid userId = Guid.NewGuid();
                    _dataContext.UsersData.Add(new()
                    {
                        Id = userId,
                        Name = viewModel.FormModel!.UserName,
                        Email = viewModel.FormModel!.UserEmail,
                        Phone = viewModel.FormModel!.UserPhone,
                    });
                    String salt = Guid.NewGuid().ToString()[..16];
                    _dataContext.UserAccesses.Add(new()
                    {
                         Id = Guid.NewGuid(),
                         UserId = userId,
                         Login = viewModel.FormModel!.UserLogin,
                         RoleId = "guest",
                         Salt = salt,
                         Dk = _kdfService.DerivedKey(
                             viewModel.FormModel!.UserPassword,
                             salt),
                    });
                    _dataContext.SaveChanges();
                }

                // видаляємо з сесії вилучені дані
                HttpContext.Session.Remove(signupFormKey);
            }
            return View(viewModel);
        }

        public IActionResult Signin()
        {
            // 'Basic' HTTP Authentication Scheme  https://datatracker.ietf.org/doc/html/rfc7617#section-2
            // Дані автентифікації приходять у заголовку Authorization
            // за схемою  Authentication: Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==
            // де дані - Base64 закодована послідовність "login:password"
            String authHeader = Request.Headers.Authorization.ToString();
            if (String.IsNullOrEmpty(authHeader))
            {
                return Json(new { status = 401, message = "Authorization header required" });
            }
            String scheme = "Basic ";
            if( ! authHeader.StartsWith(scheme))
            {
                return Json(new { status = 401, message = $"Authorization scheme must be {scheme}" });
            }
            String credentials = authHeader[scheme.Length..];
            String authData;
            try 
            { 
                authData = System.Text.Encoding.UTF8.GetString( 
                    Base64UrlTextEncoder.Decode(credentials)
                ); 
            }
            catch
            {
                return Json(new { status = 401, message = $"Not valid Base64 code '{credentials}'" });
            }
            // authData == "login:password"
            String[] parts = authData.Split(':', 2);
            if(parts.Length != 2)
            {
                return Json(new { status = 401, message = $"Not valid credentials format (missing ':'?)" });
            }
            String login = parts[0];
            String password = parts[1];
            var userAccess = _dataContext.UserAccesses.FirstOrDefault(ua => ua.Login == login);
            if(userAccess == null)
            {
                return Json(new { status = 401, message = "Credentials rejected" });
            }
            if(_kdfService.DerivedKey(password, userAccess.Salt) != userAccess.Dk)
            {
                return Json(new { status = 401, message = "Credentials rejected." });
            }
            // Зберігаємо у сесію відомості про автентифікацію
            HttpContext.Session.SetString("userAccessId", userAccess.Id.ToString());

            return Json(new { status = 200, message = "OK" });
        }
        public RedirectToActionResult Register([FromForm] UserSignupFormModel formModel)
        {
            HttpContext.Session.SetString(            // Збереження у сесії
                signupFormKey,
                JsonSerializer.Serialize(formModel)   // серіалізованого об'єкту formModel
            );
            return RedirectToAction(nameof(Signup));
        }

        private Dictionary<String, String> ValidateSignupFormModel(UserSignupFormModel? formModel)
        {
            Dictionary<String, String> errors = [];
            if(formModel == null)
            {
                errors["Model"] = "Дані не передані";
            }
            else
            {
                if (String.IsNullOrEmpty(formModel.UserName))
                {
                    errors[nameof(formModel.UserName)] = "Ім'я необхідно ввести";
                }
                if (String.IsNullOrEmpty(formModel.UserEmail))
                {
                    errors[nameof(formModel.UserEmail)] = "E-mail необхідно ввести";
                }
                if (String.IsNullOrEmpty(formModel.UserPhone))
                {
                    errors[nameof(formModel.UserPhone)] = "Телефон необхідно ввести";
                }
                if (String.IsNullOrEmpty(formModel.UserLogin))
                {
                    errors[nameof(formModel.UserLogin)] = "Логін необхідно ввести";
                }
                else
                {
                    if(_dataContext
                        .UserAccesses
                        .FirstOrDefault(ua => ua.Login == formModel.UserLogin) != null )
                    {
                        errors[nameof(formModel.UserLogin)] = "Логін у вжитку. Виберіть інший";
                    }
                }

                if (String.IsNullOrEmpty(formModel.UserPassword))
                {
                    errors[nameof(formModel.UserPassword)] = "Пароль необхідно ввести";
                }
                if (formModel.UserPassword != formModel.UserRepeat)
                {
                    errors[nameof(formModel.UserRepeat)] = "Паролі не однакові";
                }
            }
            return errors;
        }
    }
}
/* Автентифікація - підтвердження особи, одержання "посвідчення" (токену)
 * Авторизація - підтвердження права доступу автентифікованої "особи" до
 *   певного контенту
 * У залежності від архітектури системи токени зберігаються або
 *  - у клієнта (розподілена архітектура, SPA)
 *  - у сервера (серверна активність, ASP)
 *  
 * За технологією ASP використовуються НТТР-сесії для збереження даних між
 * запитами. При кожному запиті перевіряється наявність у сесії токена і 
 * приймається рішення щодо авторизації.
 *  
 <auth-form> --> site.js --X
                 fetch ---->  ASP
                   ?   <---  auth-status     
                 + reload()
                 - error

Base64 - кодування з 64-ма символами
                               A        B        C
ASCII(8 біт): ABC -->       01000001 01000010 01000011
ділимо по 6 біт:            010000 010100 001001 000011
визначаємо Base64-символи     Q      U      J       D
код для "АВС" - "QUJD"
Якщо на 6 не ділиться, то вживається символ вирівнювання "="
    A        B     
 01000001 01000010 
 010000 010100 0010(00)
    Q      U     I  =            --> QUI=

 */

/* Д.З. Розширити таблицю даних про користувача, ввести поля різного типу
 * (дата, число ціле, число дробове, тощо) з різною обов'язковістю
 * - дата реєстрації
 * - дата народження
 * - розмір одягу/взуття
 * - розмір пальця (кільця)
 * - соц.мережа (URL)
 * ...
 * Реалізувати валідацію / виведення повідомлень про помилки
 * Провести тестування - до ДЗ додати скріншоти ключових екранів
 */
