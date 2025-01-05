
using Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<ProductCategoryEntity> ProductCategories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductEntity>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<ProductEntity>().HasKey(p => p.Id);
            modelBuilder.Entity<ProductEntity>().Property(p => p.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<CategoryEntity>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<CategoryEntity>().HasKey(p => p.Id);
            modelBuilder.Entity<CategoryEntity>().Property(p => p.Name).IsRequired().HasMaxLength(100);
        }
    }
}