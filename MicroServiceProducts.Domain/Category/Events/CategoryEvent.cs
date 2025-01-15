
using MediatR;

namespace Domain.Category.Events;

public class CategoryEvent : INotification
{
    public Category Category { get; }

    public CategoryEvent(Category category)
    {
        Category = category;
    }
}
