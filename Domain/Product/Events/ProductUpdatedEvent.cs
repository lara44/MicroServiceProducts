
using MediatR;

namespace Domain.Product.Events;

public class ProductUpdatedEvent : INotification
{
    public Product Product { get; }
    public ProductUpdatedEvent(Product product)
    {
        Product = product;
    }
}
