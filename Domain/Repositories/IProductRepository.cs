
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int id);
        // Task<IEnumerable<Product>> GetAllAsync();
        Task AddAsync(Product product);
        // Task UpdateAsync(Product product);
        // Task DeleteAsync(Product product);
    }
}