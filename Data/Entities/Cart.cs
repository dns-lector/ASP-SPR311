namespace ASP_SPR311.Data.Entities
{
    public class Cart
    {
        public Guid      Id           { get; set; }
        public Guid      UserAccessId { get; set; }
        public Guid?     ActionId     { get; set; }
        public DateTime  OpenAt       { get; set; }
        public DateTime? CloseAt      { get; set; }
        public int?      IsCanceled   { get; set; }
        public double    Price        { get; set; }


        public List<CartItem> CartItems  { get; set; } = new();
        public UserAccess     UserAccess { get; set; } = null!;
    }
}
