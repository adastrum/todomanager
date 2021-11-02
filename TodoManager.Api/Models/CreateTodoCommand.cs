using FluentValidation;
using Mdtr;

namespace TodoManager.Api.Models
{
    public record CreateTodoCommand(string Name) : ICommand;

    public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
    {
        public CreateTodoCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
