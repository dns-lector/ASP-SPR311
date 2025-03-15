using ASP_SPR311.Data;
using ASP_SPR311.Models.Shop;
using ASP_SPR311.Services.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_SPR311.Controllers
{
    public class ShopController(DataContext dataContext, IStorageService storageService) : Controller
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IStorageService _storageService = storageService;

        public IActionResult Index()
        {
            ShopIndexViewModel viewModel = new()
            {
                Categories = _dataContext.Categories.ToList()
            };

            return View(viewModel);
        }

        public IActionResult Category([FromRoute] String id)
        {
            ShopCategoryViewModel viewModel = new()
            {
                Category = _dataContext
                .Categories
                .Include(c => c.Products)  // заповнення навігаційних властивостей
                .FirstOrDefault(c => c.Slug == id)
            };

            return View(viewModel);
        }

        public FileResult Image([FromRoute] String id)
        {
            return File(
                System.IO.File.ReadAllBytes(
                    _storageService.GetRealPath(id)), 
                "image/jpeg"
            );
        }
    }
}
