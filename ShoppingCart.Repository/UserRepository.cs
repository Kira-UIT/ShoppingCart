using ShoppingCart.Base.Repositories;
using ShoppingCart.Data;
using ShoppingCart.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private DataContext _context;
        public UserRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
