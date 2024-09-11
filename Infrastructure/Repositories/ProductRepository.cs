

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

        public async Task UpdateAsync(Product product)
        {
            // Recuperar el producto existente de la base de datos
            var existingProduct = await _dataContext.Products.FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existingProduct != null)
            {
                // Utilizar Attach para marcar la entidad como modificada
                _dataContext.Products.Attach(existingProduct);

                // Actualizar las propiedades del producto
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price.Amount;
                existingProduct.Stock = product.Stock;

                // Guardar los cambios en la base de datos
                await _dataContext.SaveChangesAsync();
            }
        }
    }
}