using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Devon4Net.Common.Domain.Database;
using Devon4Net.Common.Domain.Entities;
using Devon4Net.Common.Domain.RepositoryInterfaces.TodoManagement;
using Devon4Net.Domain.UnitOfWork.Service;
using Devon4Net.Domain.UnitOfWork.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Devon4Net.Common.Business.TodoManagement.Service
{
    public class TodoService: Service<TodoContext>
    {
        private readonly ILogger<TodoService> _logger;
        private readonly ITodoRepository _todoRepository;

        public TodoService(IUnitOfWork<TodoContext> uoW, ILogger<TodoService> logger) : base(uoW)
        {
            _logger = logger;
            _todoRepository = uoW.Repository<ITodoRepository>();
        }

        public async Task<IList<Todos>> GetTodo(Expression<Func<Todos, bool>> predicate = null)
        {
            _logger.LogDebug("GetTodo method from service TodoService");
            return await _todoRepository.GetTodo(predicate).ConfigureAwait(false);
        }

        public async Task<Todos> GetTodoById(long id)
        {
            _logger.LogDebug($"GetTodoById method from service TodoService with value : {id}");
            return await _todoRepository.GetTodoById(id).ConfigureAwait(false);
        }

        public async Task<Todos> SetTodo(string description)
        {
            _logger.LogDebug($"SetTodo method from service TodoService with value : {description}");

            if (string.IsNullOrEmpty(description) || string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("The 'Description' field can not be null.");
            }

            return await _todoRepository.SetTodo(description).ConfigureAwait(false);
        }

        public async Task<long> DeleteTodoById(long id)
        {
            _logger.LogDebug($"DeleteTodoById method from service TodoService with value : {id}");
            return await _todoRepository.DeleteTodoById(id).ConfigureAwait(false);
        }
    }
}
