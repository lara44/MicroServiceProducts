
using Domain.Entities;
using MediatR;

namespace Domain.Events
{
    public class ProductCreatedEvent : INotification
    {
        public Product Product { get; }

        public ProductCreatedEvent(Product product)
        {
            Product = product;
        }
    }
}