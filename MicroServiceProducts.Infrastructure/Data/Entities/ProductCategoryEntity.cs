using System;

namespace Infrastructure.Data.Entities;

public class ProductCategoryEntity
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public ProductEntity ?Product { get; set; }
    public Guid CategoryId { get; set; }
    public CategoryEntity ?Category { get; set; }
}
