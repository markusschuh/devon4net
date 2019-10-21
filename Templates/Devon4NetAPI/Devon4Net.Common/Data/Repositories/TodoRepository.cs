using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Devon4Net.Common.Business.TodoManagement.Service;
using Devon4Net.Common.Domain.Database;
using Devon4Net.Common.Domain.Entities;
using Devon4Net.Common.Domain.RepositoryInterfaces.TodoManagement;
using Devon4Net.Domain.UnitOfWork.Repository;
using Microsoft.Extensions.Logging;

namespace Devon4Net.Common.Data.Repositories
{
    public class TodoRepository : Repository<Todos>, ITodoRepository
    {
        private readonly ILogger<TodoService> _logger;

        public TodoRepository(TodoContext context, ILogger<TodoService> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<IList<Todos>> GetTodo(Expression<Func<Todos, bool>> predicate = null)
        {
            _logger.LogDebug("GetTodo method from TodoRepository TodoService");
            return await Get(predicate).ConfigureAwait(false);
        }

        public async Task<Todos> GetTodoById(long id)
        {
            _logger.LogDebug($"GetTodoById method from repository TodoService with value : {id}");
            return await GetFirstOrDefault(t => t.Id == id).ConfigureAwait(false);
        }

        public async Task<Todos> SetTodo(string description)
        {
            _logger.LogDebug($"SetTodo method from repository TodoService with value : {description}");
            if (string.IsNullOrEmpty(description) || string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("The 'Description' field can not be null.");
            }

            return await Create(new Todos{Description = description}).ConfigureAwait(false);
        }

        public async Task<long> DeleteTodoById(long id)
        {
            _logger.LogDebug($"DeleteTodoById method from repository TodoService with value : {id}");
            var deleted = await Delete(t => t.Id == id).ConfigureAwait(false);

            if (deleted)
            {
                return id;
            }

            throw  new ApplicationException($"The Todo entity {id} has not been deleted.");
        }

    }
}
