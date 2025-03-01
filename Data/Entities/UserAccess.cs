namespace ASP_SPR311.Data.Entities
{
    public class UserAccess
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public String RoleId { get; set; } = null!;
        public String Login { get; set; } = null!;
        public String Salt { get; set; } = null!;
        public String Dk { get; set; } = null!;

        // Навігаційні властивості - властивості (get; set;), що посилаються на 
        // інші сутності (Entities). EntityFramework може автоматично їх 
        // заповнювати через зв'язки
        public UserData UserData { get; set; } = null!;


        public override string ToString()
        {
            return $"UserAccess: Id({Id}), UserId({UserId}), RoleId({RoleId}), Login({Login})";
        }
    }
}
