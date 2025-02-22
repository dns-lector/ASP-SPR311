namespace ASP_SPR311.Models.User
{
    public class UserSignupFormModel
    {
        public String UserName     { get; set; } = null!;
        public String UserEmail    { get; set; } = null!;
        public String UserPhone    { get; set; } = null!;
        public String UserLogin    { get; set; } = null!;
        public String UserPassword { get; set; } = null!;
        public String UserRepeat   { get; set; } = null!;
    }
}
