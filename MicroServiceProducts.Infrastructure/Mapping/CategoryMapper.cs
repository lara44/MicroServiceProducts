using System;
using System.Data.Common;
using Domain.Category;
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
}
