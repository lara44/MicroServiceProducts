
using Application.Categories.Commands.CreateCategory;
using Application.Products.Commands.CreateProduct;
using Application.Products.Services;
using Domain.Common.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Registrar MediatR
        services.AddMediatR(typeof(DependencyInjection).Assembly);

        // Agregar FluentValidation
        services.AddFluentValidationAutoValidation()
                        .AddFluentValidationClientsideAdapters();
                    
        // Registrar validadores automáticamente desde el ensamblado
        services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateCategoryValidator>();

        // Registro de servicios específicos de Application
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

        return services;
    }
}
