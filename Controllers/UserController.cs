using ASP_SPR311.Data;
using ASP_SPR311.Models.User;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ASP_SPR311.Controllers
{
    public class UserController(DataContext dataContext) : Controller
    {
        private const String signupFormKey = "UserSignupFormModel";
        private readonly DataContext _dataContext = dataContext;

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
                    String salt = "salt";
                    _dataContext.UserAccesses.Add(new()
                    {
                         Id = Guid.NewGuid(),
                         UserId = userId,
                         Login = viewModel.FormModel!.UserLogin,
                         RoleId = "guest",
                         Salt = salt,
                         Dk = salt + viewModel.FormModel!.UserPassword,
                    });
                    _dataContext.SaveChanges();
                }

                // видаляємо з сесії вилучені дані
                HttpContext.Session.Remove(signupFormKey);
            }
            return View(viewModel);
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
