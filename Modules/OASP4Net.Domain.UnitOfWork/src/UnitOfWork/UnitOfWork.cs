using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OASP4Net.Domain.Context;
using OASP4Net.Domain.UnitOfWork.Repository;

namespace OASP4Net.Domain.UnitOfWork.UnitOfWork
{

    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : OASP4NetBaseContext

    {
        public virtual IDictionary<Type, object> Repositories { get; set; }

        private bool Disposed { get; set; }
        private TContext Context { get; }

        public UnitOfWork(TContext context)
        {
            Context = context;
            Repositories = new Dictionary<Type, object>();
        }

        public virtual IRepository<T> Repository<T>() where T : class
        {
            if (Repositories.Keys.Contains(typeof(T)))
            {
                return Repositories[typeof(T)] as IRepository<T>;
            }

            IRepository<T> repository = new Repository<T>(Context);
            Repositories.Add(typeof(T), repository);
            return repository;
        }


        #region async methods

        public async Task<int> CommitAsync()
        {
            int result;
            var transaction = await Context.Database.BeginTransactionAsync();
            {
                try
                {
                    result = await Context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}:{ex.InnerException}");
                    transaction.Rollback();
                    throw ex;
                }

            }
            return result;
        }



        #endregion

        #region sync methods

        public int Commit()
        {
            var result = int.MinValue;

            using (var transaction = Context.Database.BeginTransaction())
            {
                try
                {
                    result = Context.SaveChanges();
                    transaction.Commit();                    
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        Console.WriteLine($"Error: concurrency conflicts for {entry.Metadata.Name}");
                        var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();

                            foreach (var property in proposedValues.Properties)
                            {
                                var proposedValue = proposedValues[property];
                                var databaseValue = databaseValues[property];

                                Console.WriteLine($"Proposed value: { proposedValues[property]} | Data base value: { databaseValues[property] }");
                            }

                            // Refresh original values to bypass next concurrency check
                            entry.OriginalValues.SetValues(databaseValues);        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}:{ex.InnerException}");
                    transaction.Rollback();
                    throw ex;
                }                
            }
            return result;
        }

        #endregion

        #region dispose

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed) return;
            if (disposing)
            {
                try
                {
                    Context?.Dispose();
                }
                catch (ObjectDisposedException ex)
                {
                    Console.WriteLine($"{ex.Message}:{ex.InnerException}");
                    // do nothing, the objectContext has already been disposed
                }
            }

            Disposed = true;
        }

        #endregion

        #region rollback

        public void Rollback()
        {
            Context
                .ChangeTracker
                .Entries()
                .ToList()
                .ForEach(x => x.Reload());
        }

        #endregion
    }
}
