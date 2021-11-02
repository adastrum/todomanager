using FluentValidation.AspNetCore;
using Mdtr;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using TodoManager.Api.Common;
using TodoManager.Api.Extensions;
using TodoManager.Api.Infrastructure;
using TodoManager.Api.Jobs;
using TodoManager.Api.Models;
using TodoManager.Api.Services;

namespace TodoManager.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddDbContext<TodoDbContext>(options => options.UseSqlite("Data Source=TodoManager.db"));
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<TodoCommandService>();
            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                var jobKey = new JobKey(QueryStoreSynchronizationJob.Name);

                q.AddJob<QueryStoreSynchronizationJob>(j => j.WithIdentity(jobKey));

                var intervalInSeconds = _configuration.GetValue("queryStoreSynchronizationIntervalInSeconds",
                    defaultValue: Constants.DefaultQueryStoreSynchronizationIntervalInSeconds);

                q.AddTrigger(t => t
                    .ForJob(jobKey)
                    .WithIdentity($"{jobKey.Name} trigger")
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromSeconds(intervalInSeconds))
                    .RepeatForever()));
            });
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
            services.AddMemoryCache(cache =>
            {
                cache.SizeLimit = 1024;
            });

            //todo assembly scanning
            services.AddScoped<ICommandHandler<CreateTodoCommand>, TodoCommandService>();
            services.AddScoped<ICommandHandler<RenameTodoCommand>, TodoCommandService>();
            services.AddScoped<ICommandHandler<CompleteTodoCommand>, TodoCommandService>();
            services.AddScoped<IQueryHandler<GetTodosQuery, IReadOnlyCollection<Todo>>, TodoQueryService>();
            services.AddMediator();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseApplicationExceptionHandling();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
