using Mdtr;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TodoManager.Api.Exceptions;
using TodoManager.Api.Infrastructure;
using TodoManager.Api.Models;

namespace TodoManager.Api.Services
{
    public interface ITodoCommandService : ICommandHandler<CreateTodoCommand>, ICommandHandler<RenameTodoCommand>,
        ICommandHandler<CompleteTodoCommand>
    {
    }

    public class TodoCommandService : ITodoCommandService
    {
        private readonly TodoDbContext _todoDbContext;

        public TodoCommandService(TodoDbContext todoDbContext)
        {
            _todoDbContext = todoDbContext;
        }

        public async Task Handle(CreateTodoCommand command, CancellationToken cancellationToken)
        {
            var entity = new Core.Todo(command.Name);

            _todoDbContext.Todos.Add(entity);

            await _todoDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Handle(RenameTodoCommand command, CancellationToken cancellationToken)
        {
            var entity = await GetExistingTodo(command.Id, cancellationToken);

            entity.Rename(command.Name);

            await _todoDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Handle(CompleteTodoCommand command, CancellationToken cancellationToken)
        {
            var entity = await GetExistingTodo(command.Id, cancellationToken);

            entity.Complete();

            await _todoDbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task<Core.Todo> GetExistingTodo(string todoId, CancellationToken cancellationToken) =>
            (await _todoDbContext.Todos.FirstOrDefaultAsync(x => x.Id == todoId, cancellationToken))
                ?? throw new EntityNotFoundException($"Todo with Id = '{todoId}' was not found");
    }
}
