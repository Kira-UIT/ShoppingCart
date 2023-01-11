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
    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        private readonly DataContext _dbContext;
        public ProductCategoryRepository(DataContext dataContext) : base(dataContext)
        {
            _dbContext = dataContext;
        }

        public IProductCategoryQuery BuildQuery()
        {
            return new ProductCategoryQuery(_dbContext.ProductCategories.AsQueryable(), _dbContext);
        }

        public override void Remove(ProductCategory _object)
        {
            _object.IsDeleted = true;
            _object.DeletedAt = DateTime.UtcNow;
        }
    }
}
