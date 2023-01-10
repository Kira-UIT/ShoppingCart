using ShoppingCart.Base.Queries;
using ShoppingCart.Data;
using ShoppingCart.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Query
{
    public class ShoppingSessionQuery : BaseQuery<ShoppingSession>, IShoppingSessionQuery
    {
        private readonly DataContext _dbContext;
        public ShoppingSessionQuery(IQueryable<ShoppingSession> query, DataContext dbContext) : base(query)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
    }
}
