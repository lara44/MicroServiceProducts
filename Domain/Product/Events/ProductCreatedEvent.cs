namespace Domain.Product.Events;

public class ProductCreatedEvent
{
    public Guid Id { get; }
    public string Name { get; }
    public decimal Price { get; }
    public int Stock { get; }

    public ProductCreatedEvent(Guid id, string name, decimal price, int stock)
    {
        Id = id;
        Name = name;
        Price = price;
        Stock = stock;
    }
}
