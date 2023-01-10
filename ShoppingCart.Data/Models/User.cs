using Microsoft.AspNetCore.Identity;

namespace ShoppingCart.Data.Models
{
    public class User : IdentityUser<Guid>
    {
        public User()
        {
            CreatedAt = DateTime.UtcNow;
            ModifiedAt = DateTime.UtcNow;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public ICollection<UserAddress> UserAddress { get; set; }
        public ICollection<UserPayment> UserPayment { get; set; }
    }
}
