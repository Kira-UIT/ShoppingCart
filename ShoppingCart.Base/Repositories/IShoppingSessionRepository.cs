﻿using ShoppingCart.Base.Queries;
using ShoppingCart.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Base.Repositories
{
    public interface IShoppingSessionRepository : IRepository<ShoppingSession>
    {
        IShoppingSessionQuery BuildQuery();
    }
}
