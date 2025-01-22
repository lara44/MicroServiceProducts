
using Domain.Category;
using Domain.Common.Models;
using Domain.Product.Events;

namespace Domain.Product;

public sealed class Product : AggregateRoot
{
    public string Name { get; private set; }
    public Price Price { get; private set; }
    public int Stock { get; private set; }
    public List<Category.Category> Categories { get; private set; } = new List<Category.Category>();

    private Product(Guid Id, string name, Price price, int stock) : base(Id)
    {
        Name = name;
        Price = price;
        Stock = stock;
        Categories = new List<Category.Category>();
    }
    public static Product Create(string name, Price price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name cannot be empty.", nameof(name));
        if (stock < 0)
            throw new ArgumentException("Stock cannot be negative.", nameof(stock));

        var product = new Product(Guid.NewGuid(), name, price, stock);
        product.AddDomainEvent(new ProductCreatedEvent(product.Id, product.Name, product.Price.Amount, product.Stock));
        return product;
    }

    public void Update(string name, Price price, int stock)
    {
        Name = name;
        Price = price;
        Stock = stock;
        AddDomainEvent(new ProductUpdatedEvent(this));
    }

    public void AddCategory(Category.Category category)
    {
        if (Categories.Any(c => c.Id == category.Id))
            throw new InvalidOperationException("Category already assigned to the product.");

        Categories.Add(category);
    }

    public void RemoveCategory(Guid categoryId)
    {
        var category = Categories.FirstOrDefault(c => c.Id == categoryId);
        if (category == null)
            throw new InvalidOperationException("Category not found.");

        Categories.Remove(category);
    }

    public static Product GetProduct(Guid id, string name, Price price, int stock, List<Category.Category> categories)
    {
        var product = new Product(id, name, price, stock);
        
        foreach (var category in categories)
        {
            product.AddCategory(category);
        }

        return product;
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
