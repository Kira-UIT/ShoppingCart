using ShoppingCart.Base.Queries;
using ShoppingCart.Data.Models;
using ShoppingCart.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Query
{
    public class CartItemQuery : BaseQuery<CartItem>, ICartItemQuery
    {
        private readonly DataContext _dbContext;
        public CartItemQuery(IQueryable<CartItem> query, DataContext dbContext) : base(query)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
    }
}
