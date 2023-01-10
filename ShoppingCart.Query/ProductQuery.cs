﻿using ShoppingCart.Base.Queries;
using ShoppingCart.Data;
using ShoppingCart.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Query
{
    public class ProductQuery : BaseQuery<Product>, IProductQuery
    {
        private readonly DataContext _dbContext;
        public ProductQuery(IQueryable<Product> query, DataContext dbContext) : base(query)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(_dbContext)); ;
        }
    }
}
