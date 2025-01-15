

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

            return Product.GetProduct(
                product.Id,
                product.Name,
                price,
                product.Stock,
                product.ProductCategories!.Select(pc => Category.GetCategory(pc.Category!.Id, pc.Category.Name)).ToList()
            );
        }
    }
}