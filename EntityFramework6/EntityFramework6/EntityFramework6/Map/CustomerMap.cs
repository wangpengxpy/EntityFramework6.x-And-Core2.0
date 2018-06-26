using EntityFramework6.Enity;
using System.Data.Entity.ModelConfiguration;

namespace EntityFramework6.Map
{
    public class CustomerMap : EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            //table  
            ToTable("Customers");

            //key
            HasKey(t => t.Id);

            //properties  
            Property(t => t.Name).HasColumnType("VARCHAR").HasMaxLength(50).IsRequired();
            Property(t => t.Email).HasColumnType("VARCHAR").HasMaxLength(20).IsRequired();
            Property(t => t.CreatedTime).IsRequired();
            Property(t => t.ModifiedTime).IsRequired();
        }
    }
}
