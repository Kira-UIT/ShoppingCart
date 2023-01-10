using Microsoft.EntityFrameworkCore;
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
    public class ProductInventoryQuery : BaseQuery<ProductInventory>, IProductInventoryQuery
    {
        private readonly DataContext _dbContext;
        public ProductInventoryQuery(IQueryable<ProductInventory> query, DataContext dbContext) : base(query)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(_dbContext));
        }
    }
}
