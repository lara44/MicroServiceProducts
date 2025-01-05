
using Domain.Category;
using Domain.Category.Respositories;
using Infrastructure.Data;
using Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        var categories = await _dataContext.Categories.ToListAsync();
        return categories.Select(CategoryMapper.MapToDomainForQuery);
    }

    public async Task<Category> GetByIdAsync(Guid id)
    {
        var category = await _dataContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null)
        {
            return null!;
        }
        return CategoryMapper.MapToDomainForQuery(category!);
    }
}
