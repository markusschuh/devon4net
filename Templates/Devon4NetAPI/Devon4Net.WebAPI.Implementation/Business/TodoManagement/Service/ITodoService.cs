using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Devon4Net.WebAPI.Implementation.Business.TodoManagement.Dto;
using Devon4Net.WebAPI.Implementation.Domain.Entities;

namespace Devon4Net.WebAPI.Implementation.Business.TodoManagement.Service
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoDto>> GetTodo(Expression<Func<Todos, bool>> predicate = null);
        Task<Todos> GetTodoById(long id);
        Task<Todos> SetTodo(string description);
        Task<long> DeleteTodoById(long id);
    }
}