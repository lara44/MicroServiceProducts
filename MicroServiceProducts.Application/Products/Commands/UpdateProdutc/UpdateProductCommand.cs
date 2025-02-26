

using MediatR;

namespace Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public List<Guid> CategoryIds { get; set; } = new List<Guid>();

        public UpdateProductCommand(Guid id, string name, decimal price, int stock)
        {
            Id = id;
            Name = name;
            Price = price;
            Stock = stock;
        }
    }
}