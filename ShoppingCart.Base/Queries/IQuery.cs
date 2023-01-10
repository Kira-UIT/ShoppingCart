using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Base.Queries
{
    public interface IQuery<TEntity> where TEntity : class
    {
        Task<List<TOutput>> ToListAsync<TOutput>(Expression<Func<TEntity, TOutput>> selector);
    }
}
