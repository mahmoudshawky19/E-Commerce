using E_Commerce.Data;
using E_Commerce.Repositery.InterfaceCategory;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace E_Commerce.Repositery
{
    public class Repositery<T> : IRepositery<T> where T : class
    {
        ApplicationDbContext dbContext;// = new ApplicationDbContext();
        DbSet<T> dbSet;
        public Repositery(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<T>();
        }

        // CRUD
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void Edit(T entity)
        {
            dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Commit()
        {
            dbContext.SaveChanges();
        }

        public IQueryable<T> GetAll(Expression<Func<T, object>>[]? includeProp = null, Expression<Func<T, bool>>? expression = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;

            if (includeProp != null)
            {
                foreach (var item in includeProp)
                {
                    query = query.Include(item);
                }
            }

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        public T? GetOne(Expression<Func<T, object>>[]? includeProp = null, Expression<Func<T, bool>>? expression = null, bool tracked = true)
        {
            return GetAll(includeProp, expression, tracked).FirstOrDefault();
        }
    }
}
