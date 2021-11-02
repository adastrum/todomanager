using FluentValidation;
using Mdtr;

namespace TodoManager.Api.Models
{
    public record CompleteTodoCommand(string Id) : ICommand;

    public class CompleteTodoCommandValidator : AbstractValidator<CompleteTodoCommand>
    {
        public CompleteTodoCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
