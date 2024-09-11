using Domain.Common.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Products.Commands.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly ILogger<UpdateProductCommandHandler> _logger;

        public UpdateProductCommandHandler(
            IProductRepository productRepository,  
            IDomainEventDispatcher domainEventDispatcher,
            ILogger<UpdateProductCommandHandler> logger
        )
        {
            _productRepository = productRepository;
            _domainEventDispatcher = domainEventDispatcher;
            _logger = logger;
        }
        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product == null)
            {
                throw new KeyNotFoundException($"El producto con el ID {request.Id} no fue encontrado.");
            }

            var price = new Price(request.Price);
            product.UpdateProduct(request.Name, price, request.Stock);

            await _productRepository.UpdateAsync(product);
            
            _logger.LogInformation("Despachar Evento de dominio ProductUpdateEvent para el producto {ProductId}", product.Id);
            await _domainEventDispatcher.DispatchEventsAsync(product);
            return Unit.Value;
        }
    }
}