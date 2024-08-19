

using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Data;
using Infrastructure.Mapping;

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
    }
}