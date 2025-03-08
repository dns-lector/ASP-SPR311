using ASP_SPR311.Data;
using ASP_SPR311.Data.Entities;
using ASP_SPR311.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP_SPR311.Controllers
{
    public class AdminController(DataContext dataContext) : Controller
    {
        private readonly DataContext _dataContext = dataContext;

        public IActionResult Index()
        {
            String? canCreate = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "CanCreate")?.Value;

            if(canCreate != "1")
            {
                Response.StatusCode = StatusCodes.Status403Forbidden;
                return NoContent();
            }

            return View();
        }

        [HttpPost]
        public JsonResult AddCategory(CategoryFormModel formModel)
        {
            Category category = new()
            {
                Id = Guid.NewGuid(),
                ParentId = null,
                Name = formModel.Name,
                Description = formModel.Description,
                Slug = formModel.Slug,
                ImageUrl = "???"
            };
            _dataContext.Categories.Add(category);
            _dataContext.SaveChanges();
            return Json(category);
        }
    }
}
