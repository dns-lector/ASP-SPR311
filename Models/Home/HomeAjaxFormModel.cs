using Microsoft.AspNetCore.Mvc;

namespace ASP_SPR311.Models.Home
{
    public class HomeAjaxFormModel
    {
        [ModelBinder(Name ="userName")]
        public String UserName { get; set; } = null!;


        [ModelBinder(Name = "userEmail")]
        public String UserEmail { get; set; } = null!;
    }
}
