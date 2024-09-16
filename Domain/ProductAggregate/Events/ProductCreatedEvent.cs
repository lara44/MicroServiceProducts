
using Domain.ProductAggregate.Entities;
using MediatR;

namespace Domain.ProductAggregate.Events;

public class ProductCreatedEvent : INotification
    {
        public Product Product { get; }

        public ProductCreatedEvent(Product product)
        {
            Product = product;
        }
    }