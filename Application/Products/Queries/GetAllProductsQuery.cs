

using Domain.ProductAggregate.Entities;
using MediatR;

namespace Application.Products.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>>
    {
        
    }
}