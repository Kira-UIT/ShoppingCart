using ShoppingCart.Base.Queries;
using ShoppingCart.Base.Repositories;
using ShoppingCart.Data;
using ShoppingCart.Data.Models;
using ShoppingCart.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Repository
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        private DataContext _dbContext;
        public CartItemRepository(DataContext context) : base(context)
        {
            _dbContext = context;
        }

        public ICartItemQuery BuildQuery()
        {
            return new CartItemQuery(_dbContext.CartItems.AsQueryable(), _dbContext);
        }

        public override void Remove(CartItem _object)
        {
            _object.IsDeleted = true;
            _object.DeletedAt = DateTime.UtcNow;
        }
    }
}
