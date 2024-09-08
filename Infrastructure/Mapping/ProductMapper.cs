

using Domain.ValueObjects;

namespace Infrastructure.Mapping
{
    public static class ProductMapper
    {
        public static Infrastructure.Data.Entities.Product ToProductEntity(Domain.Entities.Product product)
        {
            var price = new Price(product.Price.Amount); 

            return new Infrastructure.Data.Entities.Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = price.Amount,
                Stock = product.Stock,
            };
        }
        public static Domain.Entities.Product ToDomainProduct(Infrastructure.Data.Entities.Product product)
        {
            var price = new Price(product.Price); 
            return new Domain.Entities.Product(
                product.Id,
                product.Name,
                price,
                product.Stock
            );
        }
    }
}