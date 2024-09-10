
using MediatR;
using Domain.Entities;
using Domain.Repositories;
using Domain.ValueObjects;
using Domain.Common.Interfaces;

namespace Application.Products.Commands.Handlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public CreateProductCommandHandler(
            IProductRepository productRepository,
            IDomainEventDispatcher domainEventDispatcher
        )
        {
            _productRepository = productRepository;
            _domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var price = new Price(request.Price);
            var product = new Product(Guid.NewGuid(), request.Name, price, request.Stock);

            // 1. Guardar el producto en el repositorio (base de datos)
            await _productRepository.AddAsync(product);

            // 2. Despachar los eventos de dominio asociados (como el evento ProductCreatedEvent)
            await _domainEventDispatcher.DispatchEventsAsync(product);
            return product.Id;
        }
    }
}