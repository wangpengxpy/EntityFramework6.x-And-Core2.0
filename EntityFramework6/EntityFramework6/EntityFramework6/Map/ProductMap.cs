using EntityFramework6.Entity;
using System.Data.Entity.ModelConfiguration;

namespace EntityFramework6.Map
{
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            ToTable("Products");

            HasKey(k => k.ProductId);

            Property(p => p.ProductName).HasColumnType("VARCHAR").HasMaxLength(50);
        }
    }
}
