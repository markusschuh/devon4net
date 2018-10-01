using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Devon4Net.Domain.UnitOfWork.Pagination;

namespace Devon4Net.Domain.UnitOfWork.Repository
{
    /// <summary>
    ///     https://docs.microsoft.com/en-us/ef/core/querying/related-data
    ///     Lazy loading is not yet supported by EF Core. You can view the lazy loading item on our backlog to track this
    ///     feature.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        private DbContext Context { get; }
        protected DbSet<T> DbSet;

        public Repository(DbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }

        #region sync methods
        public IList<T> GetAll(Expression<Func<T, bool>> predicate = null)
        {
            return null != predicate ? DbSet.Where(predicate).ToList() : DbSet.AsEnumerable().ToList();
        }

        public PaginationResult<T> GetAllPaged(int currentPage, int pageSize, Expression<Func<T, bool>> predicate = null)
        {
            var result =  null != predicate ? DbSet.Where(predicate) : DbSet;
            return GetPaginationResult(currentPage, pageSize, ref result);
        }

        public async Task<PaginationResult<T>> GetAllpagedAsync(int currentPage, int pageSize, Expression<Func<T, bool>> predicate = null)
        {
            var result = null != predicate ? DbSet.Where(predicate) : DbSet;
            return await GetPaginationResultAsync(currentPage, pageSize, result);
        }

        public IList<T> GetAllInclude(IList<string> include, Expression<Func<T, bool>> predicate = null)
        {
            return DbSetWithProperties(include, predicate).AsEnumerable().ToList();
        }

        public T Get(Expression<Func<T, bool>> predicate = null)
        {
            return null != predicate ? DbSet.FirstOrDefault(predicate) : DbSet.FirstOrDefault();
        }

        public PaginationResult<T> GetPaged(int currentPage, int pageSize, Expression<Func<T, bool>> predicate = null)
        {
            
            var result =  predicate !=null ? DbSet.Where(predicate): DbSet;
            return GetPaginationResult(currentPage, pageSize, ref result);
        }

        public PaginationResult<T> GetAllIncludePaged(int currentPage, int pageSize, IList<string> include, Expression<Func<T, bool>> predicate = null)
        {
            return DbSetWithPropertiesPaged(currentPage, pageSize, include, predicate);
        }


        public T Create(T entity)
        {
            return DbSet.Add(entity).Entity;
        }

        public void Delete(T entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
        }

        public void DeleteById(object id)
        {
            var entityToDelete = DbSet.Find(id);
            DbSet.Remove(entityToDelete);
        }

        public void Delete(Expression<Func<T, bool>> where)
        {
            var objects = DbSet.Where(where).AsEnumerable();
            foreach (var item in objects)
                DbSet.Remove(item);
        }

        public void Edit(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public T FirstOrDefault(Expression<Func<T, bool>> where = null)
        {
            return DbSet.FirstOrDefault(where);
        }

        #endregion

        #region async methods
        public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null)
        {
            return predicate != null
                ? await DbSet.Where(predicate).ToListAsync()
                : await DbSet.ToListAsync();            
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate = null)
        {
            return predicate != null
                ? await DbSet.Where(predicate).FirstOrDefaultAsync()
                : await DbSet.FirstOrDefaultAsync();
        }
        public async Task<IList<T>> GetAllIncludeAsync(IList<string> include, Expression<Func<T, bool>> predicate = null)
        {
            return await DbSetWithProperties(include, predicate).ToListAsync();
        }

        public async Task<PaginationResult<T>> GetAllIncludePagedAsync(int currentPage, int pageSize, IList<string> include, Expression<Func<T, bool>> predicate = null)
        {
            return await DbSetWithPropertiesPagedAsync(currentPage,pageSize, include, predicate);
        }

        public async Task<PaginationResult<T>> GetPagedAsync(int currentPage, int pageSize, Expression<Func<T, bool>> predicate = null)
        {
            var result = predicate != null ? DbSet.Where(predicate): DbSet;
            return await GetPaginationResultAsync(currentPage, pageSize, result);
        }

        #endregion

        #region private methods
        private PaginationResult<T> GetPaginationResult(int currentPage, int pageSize, ref IQueryable<T> resultList)
        {
            var paginationResult = new PaginationResult<T>() { CurrentPage = currentPage, PageSize = pageSize, RowCount = resultList.Count() };

            var pageCount = (double)paginationResult.RowCount / pageSize;
            paginationResult.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (currentPage - 1) * pageSize;
            paginationResult.Results = resultList.Skip(skip).Take(pageSize).ToList();

            return paginationResult;
        }

        private IQueryable<T> DbSetWithProperties(IList<string> properties, Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> queryable = predicate != null ? DbSet.Where(predicate) : DbSet;
            return properties.Aggregate(queryable, (current, property) => current.Include(property));
        }

        private PaginationResult<T> DbSetWithPropertiesPaged(int currentPage, int pageSize, IList<string> properties, Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> queryable = predicate != null ? DbSet.Where(predicate) : DbSet;
            var queryResult = properties.Aggregate(queryable, (current, property) => current.Include(property));
            return GetPaginationResult(currentPage, pageSize, ref queryResult);
        }

        private async Task<PaginationResult<T>> DbSetWithPropertiesPagedAsync(int currentPage, int pageSize, IList<string> properties, Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> queryable = predicate != null ? DbSet.Where(predicate) : DbSet;
            var queryResult = properties.Aggregate(queryable, (current, property) => current.Include(property));
            return await GetPaginationResultAsync(currentPage, pageSize, queryResult);
        }

        private async Task<PaginationResult<T>> GetPaginationResultAsync(int currentPage, int pageSize, IQueryable<T> resultList)
        {
            var paginationResult = new PaginationResult<T>() { CurrentPage = currentPage, PageSize = pageSize, RowCount = resultList.Count() };

            var pageCount = (double)paginationResult.RowCount / pageSize;
            paginationResult.PageCount = (int)Math.Ceiling(pageCount);
            
            var skip = (currentPage - 1) * pageSize;
            paginationResult.Results = await resultList.Skip(skip).Take(pageSize).ToListAsync();
            
            return paginationResult;
        }
        #endregion


    }
}