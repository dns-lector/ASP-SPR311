using Microsoft.AspNetCore.Mvc;

namespace ASP_SPR311.Models.Admin
{
    public class CategoryFormModel
    {
        [FromForm(Name = "category-name")]
        public String Name { get; set; } = null!;

        
        [FromForm(Name = "category-description")]
        public String Description { get; set; } = null!;
        
        
        [FromForm(Name = "category-slug")]
        public String Slug { get; set; } = null!;


        [FromForm(Name = "category-image")]
        public IFormFile Image { get; set; } = null!;


    }
}
