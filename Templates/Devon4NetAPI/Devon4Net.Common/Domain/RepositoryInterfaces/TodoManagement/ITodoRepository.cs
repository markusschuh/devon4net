using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Devon4Net.Common.Domain.Entities;

namespace Devon4Net.Common.Domain.RepositoryInterfaces.TodoManagement
{
    public interface ITodoRepository
    {
        Task<IList<Todos>> GetTodo(Expression<Func<Todos, bool>> predicate = null);
        Task<Todos> GetTodoById(long id);
        Task<Todos> SetTodo(string description);
        Task<long> DeleteTodoById(long id);

    }
}
