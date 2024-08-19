
using MediatR;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Products.Commands.Handlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product(Guid.NewGuid(), request.Name, request.Price, request.Stock);
            await _productRepository.AddAsync(product);
            return product.Id;
        }
    }
}