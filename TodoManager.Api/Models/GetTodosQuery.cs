using Mdtr;
using System.Collections.Generic;

namespace TodoManager.Api.Models
{
    public record GetTodosQuery() : IQuery<IReadOnlyCollection<Todo>>;
}
