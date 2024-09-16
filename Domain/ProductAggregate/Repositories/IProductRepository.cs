
using Domain.ProductAggregate.Entities;

namespace Domain.ProductAggregate.Repositories;

 public interface IProductRepository
    {
        Task<Product> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
    }
