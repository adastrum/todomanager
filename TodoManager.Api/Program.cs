using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using TodoManager.Api.Infrastructure;

namespace TodoManager.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            await EnsureDatabaseCreated(host);

            await host.RunAsync();
        }

        private static Task EnsureDatabaseCreated(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var todoDbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
            return todoDbContext.Database.EnsureCreatedAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
