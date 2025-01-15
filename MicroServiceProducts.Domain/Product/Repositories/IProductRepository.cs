

namespace Domain.Product.Repositories;

 public interface IProductRepository
    {
        Task<Product> GetByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task AddAsync(Product product, List<Guid> categoryIds);
        Task UpdateAsync(Product product);
    }
