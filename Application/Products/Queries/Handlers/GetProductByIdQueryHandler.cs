
using Domain.ProductAggregate.Entities;
using Domain.ProductAggregate.Repositories;
using MediatR;

namespace Application.Products.Queries.Handlers
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Producto con ID: {request.Id} no encontrado");
            }
            return product;
        }
    }
}