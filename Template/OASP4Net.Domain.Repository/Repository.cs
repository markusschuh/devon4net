using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OASP4Net.Domain.Repository
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
        public virtual IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null)
        {
            return null != predicate ? DbSet.Where(predicate) : DbSet.AsEnumerable();
        }

        public virtual T Get(Expression<Func<T, bool>> predicate = null)
        {
            return null != predicate ? DbSet.FirstOrDefault(predicate) : DbSet.FirstOrDefault();
        }

        public virtual T Create(T entity)
        {
            return DbSet.Add(entity).Entity;
        }

        public virtual void Delete(T entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);
        }

        public virtual void DeleteById(object id)
        {
            var entityToDelete = DbSet.Find(id);
            DbSet.Remove(entityToDelete);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            var objects = DbSet.Where(where).AsEnumerable();
            foreach (var item in objects)
                DbSet.Remove(item);
        }

        public virtual void Save()
        {
            Context.SaveChanges();
        }

        public virtual void Edit(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> where = null)
        {
            return DbSet.FirstOrDefault(where);
        }
        #endregion


        #region async methods
        public virtual async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null)
        {
            return predicate != null
                ? await DbSet.Where(predicate).ToArrayAsync()
                : await DbSet.ToArrayAsync();
        }

        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate = null)
        {
            return predicate != null
                ? await DbSet.Where(predicate).FirstOrDefaultAsync()
                : await DbSet.FirstOrDefaultAsync();
        }


        #endregion


    }
}