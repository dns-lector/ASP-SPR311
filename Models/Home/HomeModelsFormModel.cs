using Microsoft.AspNetCore.Mvc;

namespace ASP_SPR311.Models.Home
{
    public class HomeModelsFormModel
    {
        [FromForm(Name = "user-name")]
        public String UserName { get; set; } = null!;


        [FromForm(Name = "user-email")]
        public String UserEmail { get; set; } = null!;
    }
}
