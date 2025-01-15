
using Domain.Category.Events;
using Domain.Common.Models;

namespace Domain.Category;

public sealed class Category : AggregateRoot
{
    public string ?Name { get; private set; }
    private Category(Guid Id, string name) : base(Id)
    {
        Name = name;
    }

    public static Category Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty.", nameof(name));

        var category = new Category(Guid.NewGuid(), name);
        category.AddDomainEvent(new CategoryEvent(category));
        return category;
    }

    public static Category GetCategory(Guid id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty.", nameof(name));

        return new Category(id, name);
    }
}
