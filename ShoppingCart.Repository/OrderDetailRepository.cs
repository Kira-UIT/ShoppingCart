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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private DataContext _dbContext;
        public OrderDetailRepository(DataContext context) : base(context)
        {
            _dbContext = context;
        }

        public IOrderDetailQuery BuildQuery()
        {
            return new OrderDetailQuery(_dbContext.OrderDetails.AsQueryable(), _dbContext);
        }

        public override void Remove(OrderDetail _object)
        {
            _object.IsDeleted = true;
            _object.DeletedAt = DateTime.UtcNow;
        }
    }
}
