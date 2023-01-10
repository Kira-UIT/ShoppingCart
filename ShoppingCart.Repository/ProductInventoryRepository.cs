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
    public class ProductInventoryRepository : Repository<ProductInventory>, IProductInventoryRepository
    {
        private readonly DataContext _dbContext;
        public ProductInventoryRepository(DataContext context) : base(context)
        {
            _dbContext = context;
        }

        public IProductInventoryQuery BuildQuery()
        {
            return new ProductInventoryQuery(_dbContext.ProductInventories.AsQueryable(), _dbContext);
        }
    }
}
