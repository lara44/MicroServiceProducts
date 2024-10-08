

using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _dataContext;

        public ProductRepository(DataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task AddAsync(Product product)
        {
            var productEntity = ProductMapper.ToProductEntity(product);
            await _dataContext.Products.AddAsync(productEntity);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            var product = await _dataContext.Products.FindAsync(id); 
            if (product == null) 
            {
                return null!;
            }
            return ProductMapper.ToDomainProduct(product);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _dataContext.Products.ToListAsync(); 
            var domainProducts = products.Select(ProductMapper.ToDomainProduct);
            return domainProducts;
        }
    }
}