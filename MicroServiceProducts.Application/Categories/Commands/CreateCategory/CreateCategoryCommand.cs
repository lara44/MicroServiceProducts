
using MediatR;

namespace Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand : IRequest<Guid>
{
    public string ?Name { get; set; }
}
