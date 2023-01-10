using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using ShoppingCart.Base.Repositories;
using ShoppingCart.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Repository
{ 
    public class Repository<T> : IRepository<T> where T : class
    {
        protected DbSet<T> DbSet;
        private readonly DataContext _context;
        public Repository(DataContext context)
        {
            DbSet = context.Set<T>();
            _context = context;
        }

        /// <summary>
        /// Implement Create method
        /// </summary>
        /// <param name="_object"></param>
        public void Create(T _object) => DbSet.Add(_object);

        /// <summary>
        /// Create async
        /// </summary>
        /// <param name="_object"></param>
        /// <returns></returns>
        public async Task CreateAsync(T _object) => await DbSet.AddAsync(_object);

        /// <summary>
        /// Implement Remove method
        /// </summary>
        /// <param name="_object"></param>
        public virtual void Remove(T _object)
        {
            _context.Entry(_object).State = EntityState.Modified;
        }

        public async Task Delete(T _object)
        {
            await Task.FromResult(DbSet.Remove(_object));
        }

        /// <summary>
        /// Implement GetAll method
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll() => DbSet.AsNoTracking();
        /// <summary>
        /// Implement GetAllAsync method
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllAsync() => await DbSet.AsNoTracking().ToListAsync();
       
        /// <summary>
        /// Implement GetId method
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public T GetById(Guid Id)
        {
            var data = DbSet.Find(Id);
            _context.Attach(data);
            //_context.Entry(data).State = EntityState.Modified;
            return data;
        }

        /// <summary>
        /// Get by Id async
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>T</returns>
        public async Task<T> GetByIdAsync(Guid Id)
        {

            var data = await DbSet.FindAsync(Id);
            if (data is not null)
            {
                _context.Entry(data).State = EntityState.Modified;
            }
            //_context.Attach(data);
            return data;
        }

        /// <summary>
        /// Implement Update method
        /// </summary>
        /// <param name="_object"></param>
        /// <returns></returns>
        public bool Update(T _object)
        {
            DbSet.Attach(_object);
            _context.Entry(_object).State = EntityState.Modified;
            return true;
        }

        /// <summary>
        /// Find by Condition
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool asNoTracking = true) => asNoTracking ? DbSet.Where(expression).AsNoTracking() : DbSet.Where(expression);


        /// <summary>
        /// Count all records
        /// </summary>
        /// <returns></returns>
        public async Task<int> CountAsync() => await DbSet.CountAsync().ConfigureAwait(false);

        public IQueryable<T> Table()
        {
            return this.DbSet;
        }

        //Include
        public IEnumerable<T> Get(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = "")
        {
            IQueryable<T> query = this.DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
    }
}
