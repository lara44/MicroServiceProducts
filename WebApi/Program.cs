using Amazon.SQS;
using Application.Common.Interfaces;
using Application.Products.Commands.Handlers;
using Application.Products.Services;
using Application.Products.Services.Interfaces;
using Domain.Repositories;
using Infrastructure.Data;
using Infrastructure.Messaging;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
 
// Configuración de MediatR
builder.Services.AddMediatR(typeof(CreateProductCommandHandler).Assembly);

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
await app.RunAsync();
