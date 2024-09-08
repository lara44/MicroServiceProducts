

using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Products.Commands
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Currency { get; set; } = "COP";
        public int Stock { get; set; }
    }
}