using EntityFramework6.Entity;
using System.Data.Entity.ModelConfiguration;

namespace EntityFramework6.Map
{
    public class CategoryMap : EntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            ToTable("Categories");

            HasKey(k => k.CategoryId);

            Property(p => p.CategoryName).HasColumnType("VARCHAR").HasMaxLength(50);

            HasMany(p => p.Products).WithRequired(c => c.Category).HasForeignKey(k => k.CategoryId);
        }
    }
}
