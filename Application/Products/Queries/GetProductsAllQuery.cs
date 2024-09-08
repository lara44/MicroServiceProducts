
using Domain.Entities;
using MediatR;

namespace Application.Products.Queries
{
    public class GetProductsAllQuery : IRequest<IEnumerable<Product>>
    {
        
    }
}