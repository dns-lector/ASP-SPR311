using ASP_SPR311.Models.Home;
using ASP_SPR311.Models.User;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ASP_SPR311.Controllers
{
    public class UserController : Controller
    {
        private const String signupFormKey = "UserSignupFormModel";

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

                // видаляємо з сесії вилучені дані
                HttpContext.Session.Remove(signupFormKey);
            }
            return View(viewModel);
        }

        public RedirectToActionResult Register([FromForm] UserSignupFormModel formModel)
        {
            HttpContext.Session.SetString(            // Збереження у сесії
                signupFormKey,                // під ключем UserSignupFormModel
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
