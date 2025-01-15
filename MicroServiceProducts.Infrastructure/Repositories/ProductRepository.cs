
using Domain.Product;
using Domain.Product.Repositories;
using Infrastructure.Data;
using Infrastructure.Data.Entities;
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

        public async Task AddAsync(Product product, List<Guid> categoryIds)
        {
            var productEntity = ProductMapper.ToProductEntity(product);
            
            // Asocia las categorías al producto
            if (categoryIds != null && categoryIds.Any())
            {
                foreach (var categoryId in categoryIds)
                {
                    var productCategory = new ProductCategoryEntity
                    {
                        Id = Guid.NewGuid(),
                        ProductId = productEntity.Id,
                        CategoryId = categoryId
                    };
                    productEntity.ProductCategories ??= new List<ProductCategoryEntity>();
                    productEntity.ProductCategories.Add(productCategory);
                }
            }

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
            var existingProduct = await _dataContext.Products.FirstOrDefaultAsync(p => p.Id == product.Id);

            if (existingProduct != null)
            {
                var productEntity = ProductMapper.ToProductEntity(product);
                _dataContext.Entry(existingProduct).CurrentValues.SetValues(productEntity);
            }

            await _dataContext.SaveChangesAsync();
        }
    }
}