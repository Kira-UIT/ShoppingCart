using Microsoft.EntityFrameworkCore;
using ShoppingCart.Base.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Query
{
    public abstract class BaseQuery<TEntity> : IQuery<TEntity> where TEntity : class
    {
        protected IQueryable<TEntity> Query { get; set; }

        protected BaseQuery(IQueryable<TEntity> query)
        {
            Query = query ?? throw new ArgumentNullException(nameof(query));
        }

        public async Task<List<TOutput>> ToListAsync<TOutput>(System.Linq.Expressions.Expression<Func<TEntity, TOutput>> selector)
        {
            return await Query
                .Select(selector)
                .ToListAsync()
                .ConfigureAwait(false);
        }
    }
}
