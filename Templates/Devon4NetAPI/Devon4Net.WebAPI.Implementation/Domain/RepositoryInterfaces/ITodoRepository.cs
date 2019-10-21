using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Devon4Net.Domain.UnitOfWork.Repository;
using Devon4Net.WebAPI.Implementation.Domain.Entities;

namespace Devon4Net.WebAPI.Implementation.Domain.RepositoryInterfaces
{
    public interface ITodoRepository : IRepository<Todos>
    {
        Task<IList<Todos>> GetTodo(Expression<Func<Todos, bool>> predicate = null);
        Task<Todos> GetTodoById(long id);
        Task<Todos> SetTodo(string description);
        Task<long> DeleteTodoById(long id);
    }
}
