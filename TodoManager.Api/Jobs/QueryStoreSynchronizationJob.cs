using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoManager.Api.Common;
using TodoManager.Api.Infrastructure;
using TodoManager.Api.Models;
using TodoManager.Api.Services;

namespace TodoManager.Api.Jobs
{
    public class QueryStoreSynchronizationJob : IJob
    {
        private readonly TodoDbContext _todoDbContext;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly ILogger<QueryStoreSynchronizationJob> _logger;

        public QueryStoreSynchronizationJob(TodoDbContext todoDbContext, IMemoryCache cache, IMapper mapper,
            ILogger<QueryStoreSynchronizationJob> logger)
        {
            _todoDbContext = todoDbContext;
            _cache = cache;
            _mapper = mapper;
            _logger = logger;
        }

        public static string Name => "QueryStoreSynchronization";

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Synchronizing command and query stores");

            var entities = await _todoDbContext.Todos.ToListAsync(context.CancellationToken);

            var dtos = _mapper.Map<List<Todo>>(entities);

            _cache.Set(CacheKeys.Foos, dtos, new MemoryCacheEntryOptions { Size = dtos.Count });
        }
    }
}
