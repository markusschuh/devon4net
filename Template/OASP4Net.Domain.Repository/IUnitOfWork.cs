using System;
using System.Threading.Tasks;

namespace OASP4Net.Domain.Repository{

    public interface IUnitOfWork : IDisposable
    {
        int Commit();
        Task<int> CommitAsync();
    }
}
