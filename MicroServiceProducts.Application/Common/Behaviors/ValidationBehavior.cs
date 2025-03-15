using FluentValidation;
using MediatR;

namespace MicroServiceProducts.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IValidator<TRequest>? _validator;

        public ValidationBehavior(IValidator<TRequest>? validator = null)
        {
            _validator = validator;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // Si no hay validador, continuar con la solicitud
            if (_validator is null)
            {
                return await next();
            }

            // Ejecutar validación
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            // Si hay errores, lanzar una excepción o manejar los errores
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .Select(f => new { f.PropertyName, f.ErrorMessage })
                    .ToList();

                throw new ValidationException("Errores de validación", validationResult.Errors);
            }

            // Continuar con la ejecución si la validación es exitosa
            return await next();
        }
    }
