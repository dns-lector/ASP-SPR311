using System.Text.Json.Serialization;

namespace ASP_SPR311.Data.Entities
{
    public class AccessToken
    {
        public Guid      Jti { get; set; }
        public Guid      Sub { get; set; }   // UserAccessId
        public Guid      Aud { get; set; }   // UserId
        public DateTime  Iat { get; set; } = DateTime.Now;
        public DateTime? Nbf { get; set; }   // Not before
        public DateTime  Exp { get; set; }
        public String?   Iss { get; set; }   // Issuer

        // [JsonIgnore]
        public UserData User { get; set; } = null!;

        [JsonIgnore]
        public UserAccess UserAccess { get; set; } = null!;
    }
}
/* Д.З. Впровадити картки товарів на сторінці окремої категорії
 *  * створити сторінку окремого товару, реалізувати її роботу
 *  
 * При автентифікації перевірити чи є вже у БД активний токен, 
 * даного користувача (UserAccess) якщо є, то подовжити 
 * його дію і не створювати новий 
 * 
 * Визначитись з темою курсової ("клон" популярного сайту)
 * Закласти проєкт, реалізувати основну роботу.
 */
