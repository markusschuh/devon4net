using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OASP4Net.Domain.Repository
{

    public interface IEntityService<T> where T : class
    {
        void Create(T entity);
        void Delete(T entity);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null);
        T Get(Expression<Func<T, bool>> predicate = null);
        void Update(T entity);
        void DeleteById(object id);
        void Delete(Expression<Func<T, bool>> where);

        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate = null);
    }
}
