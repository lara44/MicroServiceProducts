
using MediatR;
using Domain.Entities;
using Domain.Repositories;
using Application.Products.Services.Interfaces;
using Domain.ValueObjects;

namespace Application.Products.Commands.Handlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        public readonly IProductEventService _productEventService;

        public CreateProductCommandHandler(
            IProductRepository productRepository,
            IProductEventService productEventService
        )
        {
            _productRepository = productRepository;
            _productEventService = productEventService;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var price = new Price(request.Price);
            var product = new Product(Guid.NewGuid(), request.Name, price, request.Stock);
            await _productRepository.AddAsync(product);

            await _productEventService.PublishProductCreatedEventAsync(product, cancellationToken);
            return product.Id;
        }
    }
}