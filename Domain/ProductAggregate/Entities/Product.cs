using System;
using Domain.ProductAggregate.Events;

namespace Domain.ProductAggregate.Entities;

public sealed class Product : AggregateRoot
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Price Price { get; private set; }
        public int Stock { get; private set; }

        public Product(Guid id, string name, Price price, int stock)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty.", nameof(name));
            if (stock < 0)
                throw new ArgumentException("Stock cannot be negative.", nameof(stock));

            Id = id;
            Name = name;
            Price = price;
            Stock = stock;

            AddDomainEvent(new ProductCreatedEvent(this));
        }

        public void UpdateProduct(string name, Price price, int stock)
        {
            Name = name;
            Price = price;
            Stock = stock;
            
            AddDomainEvent(new ProductUpdatedEvent(this));
        }

        public void ReduceStock(int quantity)
        {
            if (Stock < quantity)
            {
                throw new InvalidOperationException("Insufficient stock.");
            }
            Stock -= quantity;
        }
    }
