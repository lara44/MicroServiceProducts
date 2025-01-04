
using Domain.Category;
using Domain.Category.Respositories;
using Infrastructure.Data;
using Infrastructure.Mapping;

namespace Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly DataContext _dataContext;

    public CategoryRepository(DataContext dataContext)
    {
        _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
    }

    public async Task AddSync(Category category)
    {
        await _dataContext.Categories.AddAsync(CategoryMapper.ToCategoryEntity(category));
        await _dataContext.SaveChangesAsync();
    }
}
