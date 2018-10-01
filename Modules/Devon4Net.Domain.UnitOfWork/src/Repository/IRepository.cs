using Devon4Net.Domain.UnitOfWork.Pagination;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Devon4Net.Domain.UnitOfWork.Repository
{
    public interface IRepository<T> where T : class
    {
        IList<T> GetAll(Expression<Func<T, bool>> predicate = null);
        PaginationResult<T> GetAllPaged(int currentPage, int pageSize, Expression<Func<T, bool>> predicate = null);        
        T Get(Expression<Func<T, bool>> predicate = null);
        IList<T> GetAllInclude(IList<string> include, Expression<Func<T, bool>> predicate = null);
        PaginationResult<T> GetPaged(int currentPage, int pageSize, Expression<Func<T, bool>> predicate = null);
        PaginationResult<T> GetAllIncludePaged(int currentPage, int pageSize, IList<string> include, Expression<Func<T, bool>> predicate = null);
        T Create(T entity);
        void Delete(T entity);
        void DeleteById(object id);
        void Delete(Expression<Func<T, bool>> where);
        void Edit(T entity);
        
        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null);
        Task<PaginationResult<T>> GetAllpagedAsync(int currentPage, int pageSize, Expression<Func<T, bool>> predicate = null);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate = null);
        Task<IList<T>> GetAllIncludeAsync(IList<string> include, Expression<Func<T, bool>> predicate = null);
        Task<PaginationResult<T>> GetAllIncludePagedAsync(int currentPage, int pageSize, IList<string> include, Expression<Func<T, bool>> predicate = null);
        Task<PaginationResult<T>> GetPagedAsync(int currentPage, int pageSize, Expression<Func<T, bool>> predicate = null);
    }
}