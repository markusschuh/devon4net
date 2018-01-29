using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OASP4Net.Domain.Repository
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null);
        T Get(Expression<Func<T, bool>> predicate = null);
        T Create(T entity);
        void Delete(T entity);
        void DeleteById(object id);
        void Delete(Expression<Func<T, bool>> where);
        void Save();
        void Edit(T entity);

        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate = null);

    }
}