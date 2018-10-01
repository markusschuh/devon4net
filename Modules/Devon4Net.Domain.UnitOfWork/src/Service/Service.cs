using Microsoft.EntityFrameworkCore;
using Devon4Net.Domain.Context;
using Devon4Net.Domain.UnitOfWork.UnitOfWork;

namespace Devon4Net.Domain.UnitOfWork.Service
{
    public class Service<TContext> : IService where TContext: Devon4NetBaseContext
    {
        public IUnitOfWork<TContext> UoW { get; }

        public Service(IUnitOfWork<TContext> uoW)
        {
            UoW = uoW;
        }
    }
}
