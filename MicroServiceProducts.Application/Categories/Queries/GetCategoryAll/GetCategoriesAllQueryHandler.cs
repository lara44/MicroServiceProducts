
using Domain.Category;
using Domain.Category.Respositories;
using MediatR;

namespace Application.Categories.Queries.GetCategoryAll;

public class GetCategoriesAllQueryHandler : IRequestHandler<GetCategoriesAllQuery, IEnumerable<Category>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesAllQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }
    public async Task<IEnumerable<Category>> Handle(GetCategoriesAllQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories;
    }
}
