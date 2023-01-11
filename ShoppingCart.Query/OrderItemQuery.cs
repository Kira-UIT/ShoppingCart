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
    public class OrderItemQuery : BaseQuery<OrderItem>, IOrderItemQuery
    {
        private readonly DataContext _dbContext;
        public OrderItemQuery(IQueryable<OrderItem> query, DataContext dbContext) : base(query)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
    }
}
