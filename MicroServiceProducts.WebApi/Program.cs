
using Application;
using Infrastructure;
using WebApi;
using WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);
 
// Registrar servicios de Application
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddWebApiServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Registrar el middleware de manejo de excepciones
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();
await app.RunAsync();
