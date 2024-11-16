
using Domain.Product;
using MediatR;

namespace Domain.Product.Events;

public class ProductCreatedEvent : INotification
    {
        public Product Product { get; }

        public ProductCreatedEvent(Product product)
        {
            Product = product;
        }
    }