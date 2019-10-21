using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Devon4Net.Common.Business.TodoManagement.Dto;
using Devon4Net.Common.Domain.Entities;

namespace Devon4Net.Common.Business.TodoManagement.Service
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoDto>> GetTodo(Expression<Func<Todos, bool>> predicate = null);
        Task<Todos> GetTodoById(long id);
        Task<Todos> SetTodo(string description);
        Task<long> DeleteTodoById(long id);
    }
}