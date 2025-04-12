using ASP_SPR311.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASP_SPR311.Data
{
    public class DataAccessor(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        private String ImagePath => $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}/Shop/Image/";

        public List<Category> AllCategories()
        {
            var categories = _dataContext
                .Categories
                .Where(c => c.DeletedAt == null)
                .AsNoTracking() 
                .ToList();

            foreach (var category in categories)
            {
                category.ImageUrl = ImagePath + category.ImageUrl;
            }
            return categories;
        }

        public Category? GetCategory(String slug)
        {
            var category = _dataContext
               .Categories
               .Include(c => c.Products)  // заповнення навігаційних властивостей
               .AsNoTracking()
               .FirstOrDefault(c => c.Slug == slug)
               ;
            if (category != null) {
                category.ImageUrl = ImagePath + category.ImageUrl;
                foreach (var product in category!.Products)
                {
                    product.ImagesCsv = String.Join(',', 
                        product.ImagesCsv.Split(',').Select(i => ImagePath + i)
                    );
                }
            }
            return category;
        }
    }
}
/*
DAL - Data Access Layer - шар доступу до даних
Проміжний шар для централізації постачання даних, який дозволяє
перехід на інші джерела без внесення змін до контролерів / middleware тощо
 */
