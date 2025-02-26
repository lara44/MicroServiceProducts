using Domain.Category;

namespace MicroServiceProducts.Application.Products.Services.Interfaces;
public interface ICategoryValidator
{
    public Task<List<Category>> ValidateCategories(List<Guid> categoryIds);
}
