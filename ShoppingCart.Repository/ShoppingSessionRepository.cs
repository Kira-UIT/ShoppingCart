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
    public class ShoppingSessionRepository : Repository<ShoppingSession>, IShoppingSessionRepository
    {
        private DataContext _dbContext;
        public ShoppingSessionRepository(DataContext context) : base(context)
        {
            _dbContext = context;
        }

        public IShoppingSessionQuery BuildQuery()
        {
            return new ShoppingSessionQuery(_dbContext.ShoppingSessions.AsQueryable(), _dbContext);
        }

        public override void Remove(ShoppingSession _object)
        {
            _object.IsDeleted = true;
            _object.DeletedAt = DateTime.UtcNow;
        }
    }
}
