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
    public class DiscountQuery : BaseQuery<Discount>, IDiscountQuery
    {
        private readonly DataContext _dbContext;
        public DiscountQuery(IQueryable<Discount> query, DataContext dbContext) : base(query)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
    }
}
