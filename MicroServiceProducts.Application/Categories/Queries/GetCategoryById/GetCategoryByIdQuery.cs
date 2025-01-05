
using Domain.Category;
using MediatR;

namespace Application.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQuery : IRequest<Category>
{
    public Guid Id { get; set; }

    public GetCategoryByIdQuery(Guid id)
    {
        Id = id;
    }
}
