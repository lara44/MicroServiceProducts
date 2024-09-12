
using Domain.Entities;
using MediatR;

namespace Domain.Events
{
    public class ProductUpdatedEvent : INotification
    {
        public Product Product { get; }
        public ProductUpdatedEvent(Product product)
        {
            Product = product;
        }
    }
}