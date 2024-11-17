

using Domain.Product;
using MediatR;

namespace Application.Products.Queries.GetProductAll
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>>
    {
        
    }
}