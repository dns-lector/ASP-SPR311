namespace ASP_SPR311.Data.Entities
{
    public class UserRole
    {
        public String Id          { get; set; } = null!;
        public String Description { get; set; } = null!;
        public int    CanCreate   { get; set; }
        public int    CanRead     { get; set; }
        public int    CanUpdate   { get; set; }
        public int    CanDelete   { get; set; }


        public override string ToString()
        {
            return $"{Id} ({Description}): {CanCreate}/{CanRead}/{CanUpdate}/{CanDelete}";
        }
    }
}
