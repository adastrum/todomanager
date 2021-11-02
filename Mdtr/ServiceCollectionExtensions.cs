using Microsoft.Extensions.DependencyInjection;

namespace Mdtr
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            //todo assembly scanning
            return services.AddScoped<IMediator, Mediator>();
        }
    }
}
