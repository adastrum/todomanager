using FluentValidation;
using Mdtr;

namespace TodoManager.Api.Models
{
    public record RenameTodoCommand(string Id, string Name) : ICommand;

    public class RenameTodoCommandValidator : AbstractValidator<RenameTodoCommand>
    {
        public RenameTodoCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
