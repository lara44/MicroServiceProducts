
using Domain.Category;
using Domain.Category.Respositories;
using MediatR;

namespace Application.Categories.Queries.GetCategoryById;



public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Category>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    public Task<Category> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        return _categoryRepository.GetByIdAsync(request.Id);
    }
}
