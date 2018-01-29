using System;
using System.Linq;
using System.Linq.Expressions;

namespace Oasp4net.DataAccessLayer.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T Create();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate = null);
        void Add(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        void Save();
        void Dispose();
    }
}