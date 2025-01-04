using System;

namespace Domain.Category.Respositories;

public interface ICategoryRepository
{
    Task AddSync(Category category); 
}
