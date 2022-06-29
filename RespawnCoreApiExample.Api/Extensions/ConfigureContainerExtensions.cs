using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace RespawnCoreApiExample.Api.Extensions;

public static class ConfigureContainerExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo() { Title = "RespawnCoreApiExample.Api", Version = "v1" });
        });

        return services;
    }
}