

using Domain.Entities;
using MediatR;

namespace Application.Products.Queries
{
    public class GetProductQuery : IRequest<Product>
    {
        public int Id { get; }

        public GetProductQuery(int id)
        {
            Id = id;
        }
    }
}