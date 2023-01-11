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
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        private DataContext _dbContext;
        public OrderItemRepository(DataContext context) : base(context)
        {
            _dbContext = context;
        }

        public IOrderItemQuery BuildQuery()
        {
            return new OrderItemQuery(_dbContext.OrderItems.AsQueryable(), _dbContext);
        }

        public override void Remove(OrderItem _object)
        {
            _object.IsDeleted = true;
            _object.DeletedAt = DateTime.UtcNow;
        }
    }
}
