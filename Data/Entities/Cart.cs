using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_SPR311.Data.Entities
{
    public record Cart
    {
        public Guid      Id           { get; set; }
        public Guid      UserAccessId { get; set; }
        public Guid?     ActionId     { get; set; }
        public DateTime  OpenAt       { get; set; }
        public DateTime? CloseAt      { get; set; }
        public int?      IsCanceled   { get; set; }

        [Column(TypeName = "decimal(15, 2)")]
        public decimal   Price        { get; set; }


        public List<CartItem> CartItems  { get; set; } = new();
        public UserAccess     UserAccess { get; set; } = null!;
    }
}
