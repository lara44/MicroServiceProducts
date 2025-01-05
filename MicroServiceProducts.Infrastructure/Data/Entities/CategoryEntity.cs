using System;

namespace Infrastructure.Data.Entities;

public class CategoryEntity
{
    public Guid Id { get; set; }
    public string ?Name { get; set; }
    public ICollection<ProductCategoryEntity> ProductCategories { get; set; } = new List<ProductCategoryEntity>();
}
