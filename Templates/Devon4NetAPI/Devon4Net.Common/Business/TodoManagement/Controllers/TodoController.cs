using System.Collections.Generic;
using System.Threading.Tasks;
using Devon4Net.Common.Business.TodoManagement.Dto;
using Devon4Net.Common.Business.TodoManagement.Service;
using Devon4Net.Common.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Devon4Net.Common.Business.TodoManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController: ControllerBase
    {
        private readonly ILogger<TodoController> _logger;
        private readonly ITodoService _todoService;

        public TodoController(ILogger<TodoController> logger, ITodoService todoService)
        {
            _logger = logger;
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
            _logger.LogDebug("Executing GetTodo from controller TodoController");
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
            _logger.LogDebug("Executing GetTodo from controller TodoController");
            return Ok(await _todoService.SetTodo(todoDescription).ConfigureAwait(false));
        }
    }
}
