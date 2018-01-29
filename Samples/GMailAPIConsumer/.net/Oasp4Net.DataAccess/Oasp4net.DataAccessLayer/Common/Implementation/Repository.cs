using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Oasp4net.DataAccessLayer.Common.Interfaces;

namespace Oasp4net.DataAccessLayer.Common.Implementation
{
    public abstract class Repository<TDatabaseContext, T> : IRepository<T> where T : class where TDatabaseContext : DbContext, new()
    {
        public TDatabaseContext Context { get; set; } = new TDatabaseContext();

        public virtual T Create()
        {
            return Context.Set<T>().Create();
        }

        public virtual IQueryable<T> FindAll(Expression<Func<T, bool>> predicate = null)
        {
            return null != predicate ? Context.Set<T>().Where(predicate) : Context.Set<T>();
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> where = null)
        {
            return FindAll(where).FirstOrDefault();
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = Context.Set<T>().Where(predicate);
            return query;
        }

        public virtual void Add(T entity)
        {
            Context.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            var objects = Context.Set<T>().Where(where).AsEnumerable();
            foreach (var item in objects)
            {
                Context.Set<T>().Remove(item);
            }
        }

        public virtual void Save()
        {
            Context.SaveChanges();
        }

        public virtual void Dispose()
        {
            Context?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
