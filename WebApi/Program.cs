using Amazon.SQS;
using Application.Common.Interfaces;
using Application.Products.Commands.Handlers;
using Application.Products.Commands.Validators;
using Application.Products.Services;
using Application.Products.Services.Interfaces;
using Domain.Common.Interfaces;
using Domain.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Infrastructure.Messaging;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
 
// Configuración de MediatR
builder.Services.AddMediatR(typeof(CreateProductCommandHandler).Assembly);
// builder.Services.AddMediatR(typeof(UpdateProductCommandHandler).Assembly);

// Agregar FluentValidation
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

// Registrar validadores automáticamente desde el ensamblado
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add connections
builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAWSService<IAmazonSQS>();

// Add Interfaces
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IEventPublisher, SqsEventPublisher>();
builder.Services.AddScoped<IProductEventService, ProductEventService>();
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
await app.RunAsync();
