using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Base.Extensions
{
    public static class UserManagerExtensions
    {
        public static Task<User> FindByUsernameAsync(this UserManager<User> userManager, string username)
        {
            return userManager?.Users?.FirstOrDefaultAsync(u => u.UserName == username);
        }
    }
}
