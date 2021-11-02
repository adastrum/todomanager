using Mdtr;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TodoManager.Api.Models;

namespace TodoManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TodosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Task<IActionResult> GetTodos(CancellationToken cancellationToken)
        {
            //todo bind request to query
            var query = new GetTodosQuery();

            return ExecuteQuery<GetTodosQuery, IReadOnlyCollection<Todo>>(query, cancellationToken);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTodo(CreateTodoCommand command, CancellationToken cancellationToken) =>
            await AcceptCommand(command, cancellationToken);

        [HttpPost("rename")]
        public async Task<IActionResult> CreateTodo(RenameTodoCommand command, CancellationToken cancellationToken) =>
            await AcceptCommand(command, cancellationToken);

        [HttpPost("complete")]
        public async Task<IActionResult> CompleteTodo(CompleteTodoCommand command, CancellationToken cancellationToken) =>
            await AcceptCommand(command, cancellationToken);

        private async Task<IActionResult> ExecuteQuery<TQuery, TResult>(TQuery query, CancellationToken cancellationToken)
            where TQuery : IQuery<TResult>
        {
            var todos = await _mediator.SendQuery<TQuery, TResult>(query, cancellationToken);

            return Ok(todos);
        }

        private async Task<IActionResult> AcceptCommand<TCommand>(TCommand command, CancellationToken cancellationToken)
            where TCommand : ICommand
        {
            await _mediator.SendCommand(command, cancellationToken);

            return Accepted();
        }
    }
}
