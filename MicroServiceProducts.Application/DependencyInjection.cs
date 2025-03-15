
using Application.Categories.Commands.CreateCategory;
using Application.Products.Commands.CreateProduct;
using Application.Products.Services;
using Domain.Common.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using MicroServiceProducts.Application.Common.Behaviors;
using MicroServiceProducts.Application.Products.Services;
using MicroServiceProducts.Application.Products.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Registrar MediatR
        services.AddMediatR(typeof(DependencyInjection).Assembly);

        // Agregar FluentValidation y registrar los validadores
        services.AddValidatorsFromAssembly(typeof(CreateProductCommandValidator).Assembly);

        // Registrar el ValidationBehavior en el pipeline de MediatR
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        // Registro de servicios específicos de Application
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<ICategoryValidator, CategoryValidator>();

        return services;
    }
}
