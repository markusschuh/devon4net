using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Devon4Net.Domain.Context;
using Devon4Net.Domain.UnitOfWork.Repository;

namespace Devon4Net.Domain.UnitOfWork.UnitOfWork
{
    public interface IUnitOfWork<TContext> : IDisposable where TContext : Devon4NetBaseContext
    {
        int Commit();
        Task<int> CommitAsync();
        IRepository<T> Repository<T>() where T : class;
    }
}
