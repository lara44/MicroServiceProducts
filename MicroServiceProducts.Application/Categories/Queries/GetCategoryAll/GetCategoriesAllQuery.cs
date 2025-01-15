
using Domain.Category;
using MediatR;

namespace Application.Categories.Queries.GetCategoryAll;

public class GetCategoriesAllQuery : IRequest<IEnumerable<Category>>
{

}
