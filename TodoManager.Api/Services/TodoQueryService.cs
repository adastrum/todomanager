using Mdtr;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TodoManager.Api.Common;
using TodoManager.Api.Models;

namespace TodoManager.Api.Services
{
    public interface ITodoQueryService : IQueryHandler<GetTodosQuery, IReadOnlyCollection<Todo>>
    {
    }

    public class TodoQueryService : ITodoQueryService
    {
        private readonly IMemoryCache _cache;

        public TodoQueryService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<IReadOnlyCollection<Todo>> Handle(GetTodosQuery query, CancellationToken cancellationToken)
        {
            return _cache.Get<List<Todo>>(CacheKeys.Foos);
        }
    }
}
