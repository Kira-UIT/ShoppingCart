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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly DataContext _dbContext;
        public ProductRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public IProductQuery BuildQuery()
        {
            return new ProductQuery(_dbContext.Products.AsQueryable(), _dbContext);
        }

        public override void Remove(Product _object)
        {
            _object.IsDeleted = true;
            _object.DeletedAt = DateTime.UtcNow;
        }
    }
}
