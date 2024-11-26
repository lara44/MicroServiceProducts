

using Domain.Product;

namespace Infrastructure.Mapping
{
    public static class ProductMapper
    {
        public static Infrastructure.Data.Entities.Product ToProductEntity(Domain.Product.Product product)
        {
            var price = Price.Create(product.Price.Amount); 

            return new Infrastructure.Data.Entities.Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = price.Amount,
                Stock = product.Stock,
            };
        }
        public static Domain.Product.Product ToDomainProduct(Infrastructure.Data.Entities.Product product)
        {
            var price = Price.Create(product.Price); 
            return Domain.Product.Product.Create(
                product.Name,
                price,
                product.Stock
            );
        }
    }
}