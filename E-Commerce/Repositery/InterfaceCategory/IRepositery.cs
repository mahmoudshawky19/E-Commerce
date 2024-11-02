using E_Commerce.Models;
using System.Linq.Expressions;

namespace E_Commerce.Repositery.InterfaceCategory
{
    public interface IRepositery <T> where T : class
    {
        IQueryable<T> GetAll(Expression<Func<T, object>>[]? includeProp = null, Expression<Func<T, bool>>? expression = null, bool tracked = true);

        T? GetOne(Expression<Func<T, object>>[]? includeProp = null, Expression<Func<T, bool>>? expression = null, bool tracked = true);
        void Add(T category);
        void Edit(T category);
        void Delete(T category);
        void Commit();
    }
}
