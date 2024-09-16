using System;
using Domain.ProductAggregate.Entities;
using MediatR;

namespace Domain.ProductAggregate.Events;

 public class ProductUpdatedEvent : INotification
    {
        public Product Product { get; }
        public ProductUpdatedEvent(Product product)
        {
            Product = product;
        }
    }
