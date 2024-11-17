
using Domain.Common.Models;
using Domain.Product.Events;

namespace Domain.Product;

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

            // AddDomainEvent(new ProductCreatedEvent(this));
        }

        public static Product Create(Guid id, string name, Price price, int stock)
        {
            var product = new Product(id, name, price, stock);
            product.AddDomainEvent(new ProductCreatedEvent(product));
            return product;
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
