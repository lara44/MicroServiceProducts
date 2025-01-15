
using WebApi.Middlewares;

namespace WebApi;
public static class DependencyInjection
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        // Add services to the container.
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddTransient<GlobalExceptionHandlingMiddleware>();

        return services;
    }
}
