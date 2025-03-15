using ASP_SPR311.Data;
using ASP_SPR311.Data.Entities;
using ASP_SPR311.Models.Admin;
using ASP_SPR311.Services.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP_SPR311.Controllers
{
    public class AdminController(DataContext dataContext, IStorageService storageService) : Controller
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IStorageService _storageService = storageService;

        public IActionResult Index()
        {
            String? canCreate = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "CanCreate")?.Value;

            if(canCreate != "1")
            {
                Response.StatusCode = StatusCodes.Status403Forbidden;
                return NoContent();
            }
            AdminIndexViewModel viewModel = new()
            {
                Categories = _dataContext.Categories.ToList(),
            };

            return View(viewModel);
        }

        [HttpPost]
        public JsonResult AddProduct(ProductFormModel formModel)
        {
            double price;
            try { 
                price = double.Parse(formModel.Price, System.Globalization.CultureInfo.InvariantCulture); 
            }
            catch
            {
                price = double.Parse(formModel.Price.Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture);
            }
            Product product = new()
            {
                Id = Guid.NewGuid(),
                CategoryId = formModel.CategoryId,
                Name = formModel.Name,
                Description = formModel.Description,
                Slug = formModel.Slug,
                Price = price,
                Stock = formModel.Stock,
                ImagesCsv = String.Join(',', 
                    formModel
                    .Images
                    .Select(img => _storageService.SaveFile(img))
                ),
            };
            _dataContext.Products.Add(product);
            try
            {
                _dataContext.SaveChanges();
            }
            catch
            {
                // _storageService.DeleteFile(category.ImageUrl)
            }
            return Json(product);
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
                ImageUrl = _storageService.SaveFile(formModel.Image)
            };
            _dataContext.Categories.Add(category);
            try
            {
                _dataContext.SaveChanges();
            }
            catch
            {
                // _storageService.DeleteFile(category.ImageUrl)
            }
            return Json(category);
        }
    }
}
