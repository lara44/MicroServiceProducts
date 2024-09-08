
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Products.Queries.Handlers
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetProductsAllQuery, IEnumerable<Product>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> Handle(GetProductsAllQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetAllAsync();
        }
    }
}