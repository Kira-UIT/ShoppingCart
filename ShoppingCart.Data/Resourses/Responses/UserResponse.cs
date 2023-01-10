using ShoppingCart.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Resourses.Responses
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime ExpireTime { get; set; }
        public string RefreshToken { get; set; }
    }

    public class ChangePasswordResponse
    {
    }

    public class GetCurrentUserResponse
    {
        public Guid Id { get; set; }
        public string UserName { get; set;}
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public ICollection<UserAddress> UserAddress { get; set; }
        public ICollection<UserPayment> UserPayment { get; set; }
    }

    public class RefreshTokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireTime { get; set; }
    }
}
