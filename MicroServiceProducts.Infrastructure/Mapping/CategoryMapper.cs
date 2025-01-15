using System;
using System.Data.Common;
using Domain.Category;
using Domain.Product;
using Infrastructure.Data.Entities;

namespace Infrastructure.Mapping;

public static class CategoryMapper
{
    public static CategoryEntity ToCategoryEntity(Category category)
    {
        return new CategoryEntity
        {
            Id = category.Id,
            Name = category.Name
        };
    }

      public static Category MapToDomainForQuery(CategoryEntity category)
        {
            return Category.GetCategory(
                category.Id,
                category.Name!
            );
        }
}
