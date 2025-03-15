
using Domain.Product;
using Domain.Product.Repositories;
using Infrastructure.Data;
using Infrastructure.Data.Entities;
using Infrastructure.Mapping;
using MassTransit.Futures.Contracts;
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

            ProductMapper.MapCategoriesToProduct(product, productEntity);

            await _dataContext.Products.AddAsync(productEntity);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            var product = await _dataContext.Products
                .Include(p => p.ProductCategories!)
                .ThenInclude(pc => pc.Category)
                .FirstAsync(p => p.Id == id);
            if (product == null)
            {
                return null!;
            }
            return ProductMapper.MapToDomainForQuery(product);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _dataContext.Products
                 .Include(p => p.ProductCategories!)
                     .ThenInclude(pc => pc.Category)
                 .ToListAsync();
            var domainProducts = products.Select(ProductMapper.MapToDomainForQuery);
            return domainProducts;

        }

        public async Task UpdateAsync(Product product)
        {
            var existingProduct = await _dataContext.Products
                .Include(p => p.ProductCategories!)
                .ThenInclude(pc => pc.Category)
                .FirstAsync(p => p.Id == product.Id);

            if (existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Price = product.Price.Amount!;
                existingProduct.Stock = product.Stock;
                UpdateProductCategories(existingProduct, product);
                await _dataContext.SaveChangesAsync();
            }
        }

        public void UpdateProductCategories(ProductEntity existingProduct, Product product)
        {
            // Si la lista es null, inicializarla
            existingProduct.ProductCategories ??= new List<ProductCategoryEntity>();

            // Obtener IDs actuales y nuevos
            var existingCategoryIds = existingProduct.ProductCategories.Select(pc => pc.CategoryId).ToHashSet();
            var newCategoryIds = product.Categories.Select(c => c.Id).ToHashSet();

            // Agregar nuevas categorías
            var categoriesToAdd = newCategoryIds.Except(existingCategoryIds)
                .Select(categoryId => new ProductCategoryEntity
                {
                    Id = Guid.NewGuid(),
                    ProductId = existingProduct.Id,
                    CategoryId = categoryId
                }).ToList();

            if (categoriesToAdd.Any())
            {
                _dataContext.ProductCategories.AddRange(categoriesToAdd);
            }

            // Eliminar categorías no necesarias
            var categoriesToRemove = existingProduct.ProductCategories
                .Where(pc => !newCategoryIds.Contains(pc.CategoryId))
                .ToList();

            if (categoriesToRemove.Any())
            {
                _dataContext.ProductCategories.RemoveRange(categoriesToRemove);
            }
        }
    }
}