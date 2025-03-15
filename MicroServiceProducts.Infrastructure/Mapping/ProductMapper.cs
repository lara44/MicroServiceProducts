

using Domain.Category;
using Domain.Product;
using Infrastructure.Data.Entities;

namespace Infrastructure.Mapping
{
    public static class ProductMapper
    {
        public static ProductEntity ToProductEntity(Product product)
        {
            var price = Price.Create(product.Price.Amount); 

            return new ProductEntity
            {
                Id = product.Id,
                Name = product.Name,
                Price = price.Amount,
                Stock = product.Stock,
                ProductCategories = new List<ProductCategoryEntity>()
            };
        }
        public static Product ToDomainProduct(ProductEntity product)
        {
            var price = Price.Create(product.Price); 
            return Product.Create(
                product.Name,
                price,
                product.Stock
            );
        }

        public static Product MapToDomainForQuery(ProductEntity product)
        {
            var price = Price.Create(product.Price); 

            return Product.Restore(
                product.Id,
                product.Name,
                price,
                product.Stock,
                product.ProductCategories!.Select(pc => Category.GetCategory(pc.Category!.Id, pc.Category.Name!)).ToList()
            );
        }

        public static void MapCategoriesToProduct(Product product, ProductEntity productEntity)
        {
            productEntity.ProductCategories ??= new List<ProductCategoryEntity>();

            // Limpia categorías existentes (para evitar duplicados en updates)
            productEntity.ProductCategories.Clear();

            if (product.Categories != null && product.Categories.Any())
            {
                foreach (var category in product.Categories)
                {
                    var productCategory = new ProductCategoryEntity
                    {
                        Id = Guid.NewGuid(),
                        ProductId = productEntity.Id,
                        CategoryId = category.Id
                    };
                    productEntity.ProductCategories.Add(productCategory);
                }
            }
        }
    }
}