

namespace Infrastructure.Mapping
{
    public static class ProductMapper
    {
        public static Infrastructure.Data.Entities.Product ToProductEntity(Domain.Entities.Product product)
        {
            return new Infrastructure.Data.Entities.Product
            {
                Id = product.Id.GetHashCode(),
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
            };
        }
        public static Domain.Entities.Product ToDomainProduct(Infrastructure.Data.Entities.Product product)
        {
            return new Domain.Entities.Product(
                Guid.NewGuid(), 
                product.Name,
                product.Price,
                product.Stock
            );
        }
    }
}