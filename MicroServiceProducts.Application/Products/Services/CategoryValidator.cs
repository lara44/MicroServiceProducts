using Domain.Category;
using Domain.Category.Respositories;
using MicroServiceProducts.Application.Products.Services.Interfaces;

namespace MicroServiceProducts.Application.Products.Services;

public class CategoryValidator : ICategoryValidator
{
    private readonly ICategoryRepository _categoryRepository;
    public CategoryValidator(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<Category>> ValidateCategories(List<Guid> categoryIds)
    {
        var categories = new List<Category>();
        foreach (var categoryId in categoryIds)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                throw new ArgumentException($"Category with Id {categoryId} not found.");
            }
            categories.Add(category);
        }
        return categories;
    }
}
