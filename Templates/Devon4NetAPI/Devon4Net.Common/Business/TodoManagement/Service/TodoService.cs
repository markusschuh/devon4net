using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Devon4Net.Common.Business.TodoManagement.Converters;
using Devon4Net.Common.Business.TodoManagement.Dto;
using Devon4Net.Common.Domain.Database;
using Devon4Net.Common.Domain.Entities;
using Devon4Net.Common.Domain.RepositoryInterfaces;
using Devon4Net.Domain.UnitOfWork.Service;
using Devon4Net.Domain.UnitOfWork.UnitOfWork;
using Devon4Net.Infrastructure.Log;

namespace Devon4Net.Common.Business.TodoManagement.Service
{
    public class TodoService: Service<TodoContext>, ITodoService
    {
        private readonly ITodoRepository _todoRepository;

        public TodoService(IUnitOfWork<TodoContext> uoW) : base(uoW)
        {
            _todoRepository = uoW.Repository<ITodoRepository>();
        }

        public async Task<IEnumerable<TodoDto>> GetTodo(Expression<Func<Todos, bool>> predicate = null)
        {
            Devon4NetLogger.Debug("GetTodo method from service TodoService");
            var result = await _todoRepository.GetTodo(predicate).ConfigureAwait(false);
            return result.Select(TodoConverter.ModelToDto);
        }

        public async Task<Todos> GetTodoById(long id)
        {
            Devon4NetLogger.Debug($"GetTodoById method from service TodoService with value : {id}");
            return await _todoRepository.GetTodoById(id).ConfigureAwait(false);
        }

        public async Task<Todos> SetTodo(string description)
        {
            Devon4NetLogger.Debug($"SetTodo method from service TodoService with value : {description}");

            if (string.IsNullOrEmpty(description) || string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("The 'Description' field can not be null.");
            }

            return await _todoRepository.SetTodo(description).ConfigureAwait(false);
        }

        public async Task<long> DeleteTodoById(long id)
        {
            Devon4NetLogger.Debug($"DeleteTodoById method from service TodoService with value : {id}");
            var todo = await _todoRepository.GetFirstOrDefault(t => t.Id == id).ConfigureAwait(false);

            if (todo == null)
            {
                throw new ArgumentException($"The provided Id {id} does not exists");
            }

            return await _todoRepository.DeleteTodoById(id).ConfigureAwait(false);
        }
    }
}
