using System;
using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace WebApi.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(
        ILogger<GlobalExceptionHandlingMiddleware> logger
    )
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió un error inesperado.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var errorMessage = "Error inesperado en el servidor.";
        var errorCode = "GENERIC_ERROR";
        object? errors = null;

        if (exception is ValidationException validationException)
        {
            // Manejo de errores de FluentValidation
            statusCode = HttpStatusCode.BadRequest; // 400 Bad Request
            errorMessage = "Errores de validación en la solicitud.";
            errorCode = "VALIDATION_ERROR";

            // Construir los errores de validación
            errors = validationException.Errors.Select(err => new
            {
                Field = err.PropertyName,
                Error = err.ErrorMessage
            });
        }
        // Manejo específico de DbUpdateException para manejar claves duplicadas en PostgreSQL
        else if (exception is DbUpdateException dbUpdateEx && dbUpdateEx.InnerException is PostgresException postgresEx)
        {
            // Verificar si es un error por clave duplicada (SqlState: "23505")
            if (postgresEx.SqlState == "23505")
            {
                statusCode = HttpStatusCode.Conflict;  // 409 Conflict para indicar un duplicado
                errorMessage = "Ya existe un registro con el mismo nombre. Los nombres deben ser únicos.";
                errorCode = "DUPLICATE_KEY_ERROR";
            }
            else
            {
                // Para otros errores de Postgres que no sean clave duplicada
                errorMessage = postgresEx.Message;
                errorCode = "DB_UPDATE_ERROR";
            }
        }
        else if (exception is KeyNotFoundException)
        {
            statusCode = HttpStatusCode.NotFound;
            errorMessage = "El recurso solicitado no fue encontrado. " + exception.Message;
            errorCode = "RESOURCE_NOT_FOUND";
        }
        else if (exception is ArgumentException)
        {
            statusCode = HttpStatusCode.BadRequest;
            errorMessage = "Solicitud inválida. " + exception.Message;
            errorCode = "INVALID_ARGUMENT";
        }
        else if (exception is InvalidOperationException)
        {
            statusCode = HttpStatusCode.Conflict;
            errorMessage = "Operación inválida. " + exception.Message;
            errorCode = "INVALID_OPERATION";
        }

        // Estructura de la respuesta de error
        var result = JsonSerializer.Serialize(new
        {
            error = errorMessage,
            errorCode = errorCode,
            errors = errors, // Incluye detalles adicionales si existen (validación)
            statusCode = (int)statusCode,
            timestamp = DateTime.UtcNow
        });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(result);
    }

}
