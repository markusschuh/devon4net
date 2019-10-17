﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Devon4Net.Domain.UnitOfWork.Pagination;

namespace Devon4Net.Domain.UnitOfWork.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Delete(Expression<Func<T, bool>> predicate = null);
        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate = null);
        Task<T> GetLastOrDefault(Expression<Func<T, bool>> predicate = null);
        Task<IList<T>> Get(Expression<Func<T, bool>> predicate = null);
        Task<IList<T>> Get(IList<string> include, Expression<Func<T, bool>> predicate = null);
        Task<PaginationResult<T>> Get(int currentPage, int pageSize, IList<string> includedNestedFiels, Expression<Func<T, bool>> predicate = null);
        Task<PaginationResult<T>> Get(int currentPage, int pageSize, Expression<Func<T, bool>> predicate = null);
    }
}
