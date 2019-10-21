using System.Collections.Generic;
using System.Threading.Tasks;
using Devon4Net.Infrastructure.Log;
using Devon4Net.WebAPI.Implementation.Business.TodoManagement.Dto;
using Devon4Net.WebAPI.Implementation.Business.TodoManagement.Service;
using Microsoft.AspNetCore.Mvc;

namespace Devon4Net.WebAPI.Implementation.Business.TodoManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController: ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController( ITodoService todoService)
        {
            _todoService = todoService;
        }


        /// <summary>
        /// Gets the entire list of TODOS
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<TodoDto>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetTodo()
        {
            Devon4NetLogger.Debug("Executing GetTodo from controller TodoController");
            return Ok(await _todoService.GetTodo().ConfigureAwait(false));
        }

        /// <summary>
        /// Gets the entire list of TODOS
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(TodoDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Create(string todoDescription)
        {
            Devon4NetLogger.Debug("Executing GetTodo from controller TodoController");
            return Ok(await _todoService.SetTodo(todoDescription).ConfigureAwait(false));
        }

        /// <summary>
        /// Gets the entire list of TODOS
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(long), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Delete(long todoId)
        {
            Devon4NetLogger.Debug("Executing GetTodo from controller TodoController");
            return Ok(await _todoService.DeleteTodoById(todoId).ConfigureAwait(false));
        }
    }
}
