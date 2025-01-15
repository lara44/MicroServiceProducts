using System;

namespace Domain.Category.Respositories;

public interface ICategoryRepository
{
    Task AddSync(Category category); 
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category> GetByIdAsync(Guid id);
}
