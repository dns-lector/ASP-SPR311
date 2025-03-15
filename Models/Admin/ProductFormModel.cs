using Microsoft.AspNetCore.Mvc;

namespace ASP_SPR311.Models.Admin
{
    public class ProductFormModel
    {
        [FromForm(Name = "category-id")]
        public Guid CategoryId { get; set; }
        
        [FromForm(Name = "product-name")]
        public String Name { get; set; } = null!;

        [FromForm(Name = "product-description")]
        public String? Description { get; set; } = null!;

        [FromForm(Name = "product-slug")]
        public String? Slug { get; set; } = null!;

        [FromForm(Name = "product-price")]
        public String Price { get; set; } = "";

        [FromForm(Name = "product-stock")]
        public int Stock { get; set; }

        [FromForm(Name = "product-image")]
        public IFormFile[] Images { get; set; } = [];
    }
}
