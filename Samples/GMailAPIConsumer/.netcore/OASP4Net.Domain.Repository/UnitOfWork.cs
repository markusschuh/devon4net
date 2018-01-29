using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OASP4Net.Domain.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool Disposed { get; set; }
        private DbContext DbContext { get; }

        public UnitOfWork(DbContext context)
        {
            DbContext = context;
        }



        #region async methods
        public async Task<int> CommitAsync()
        {
            int result;

            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    result = await DbContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }

            }
            return result;
        }



        #endregion

        #region sync methods
        public int Commit()
        {
            int result;

            using (var transaction = DbContext.Database.BeginTransaction())
            {
                try
                {
                    result = DbContext.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }

            }
            return result;
        }
        #endregion

        #region dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed) return;
            if (disposing)
            {
                try
                {
                    DbContext?.Dispose();
                }
                catch (ObjectDisposedException)
                {
                    // do nothing, the objectContext has already been disposed
                }   
            }

            Disposed = true;
        }
        #endregion


        #region rollback
        public void Rollback()
        {
            DbContext
                .ChangeTracker
                .Entries()
                .ToList()
                .ForEach(x => x.Reload());
        }
        #endregion



    }
}
