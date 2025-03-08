namespace ASP_SPR311.Data.Entities
{
    public class Category
    {
        public Guid   Id          { get; set; }
        public Guid?  ParentId    { get; set; }
        public String Name        { get; set; } = null!;
        public String Description { get; set; } = null!;
        public String Slug        { get; set; } = null!;
        public String ImageUrl    { get; set; } = null!;
    }
}
