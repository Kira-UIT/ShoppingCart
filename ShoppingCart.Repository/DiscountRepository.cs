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
    public class DiscountRepository : Repository<Discount>, IDiscountRepository
    {
        private DataContext _dbContext;
        public DiscountRepository(DataContext context) : base(context)
        {
            _dbContext = context;
        }

        public IDiscountQuery BuildQuery()
        {
            return new DiscountQuery(_dbContext.Discounts.AsQueryable(), _dbContext);
        }

        public override void Remove(Discount _object)
        {
            _object.IsDeleted = true;
            _object.DeletedAt = DateTime.UtcNow;
        }
    }
}
