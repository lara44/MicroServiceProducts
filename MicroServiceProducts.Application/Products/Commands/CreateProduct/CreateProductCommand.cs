

using MediatR;

namespace Application.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public List<Guid> CategoryIds { get; set; } = new List<Guid>();
    }
}