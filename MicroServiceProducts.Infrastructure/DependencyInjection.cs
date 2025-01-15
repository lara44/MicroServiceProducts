using System;
using Amazon.SQS;
using Domain.Category.Respositories;
using Domain.Product.Events;
using Domain.Product.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuración de DbContext con PostgreSQL
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );

        // Registro de servicios de AWS (SQS en este caso)
        services.AddAWSService<IAmazonSQS>();

        // Configurar AWS SQS con MassTransit
            services.AddMassTransit(configure =>
            {
                configure.UsingAmazonSqs((context, cfg) =>
                {
                    // Configuración del host (AWS SQS)
                    cfg.Host(configuration["AWS:Region"], h =>
                    {
                        h.AccessKey(configuration["AWS:AccessKey"]);
                        h.SecretKey(configuration["AWS:SecretKey"]);
                        h.Scope("dev", true);
                    });

                    // Configuración del mensaje para el evento ProductCreatedEvent
                    cfg.Message<ProductCreatedEvent>(configTopology =>
                    {
                        configTopology.SetEntityName("dev_Domain_Product_Events-ProductCreatedEvent"); // Tema existente
                    });

                    // Configuración de los endpoints para la publicación de mensajes
                    cfg.ConfigureEndpoints(context, new DefaultEndpointNameFormatter("dev-", false));
                });
            });

        // Registro de los repositorios en la capa de infraestructura
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        return services;
    }
}
