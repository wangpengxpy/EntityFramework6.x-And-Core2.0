using EntityFramework6.Enity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace EntityFramework6.Map
{
    public class OrderMap : EntityTypeConfiguration<Order>
    {
        public OrderMap()
        {
            //table  
            ToTable("Orders");

            //key  
            HasKey(t => t.Id)
                .Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //fields  
            Property(t => t.Quantity);
            Property(t => t.Price).HasPrecision(18,4);
            Property(t => t.CustomerId);
            Property(t => t.CreatedTime);
            Property(t => t.ModifiedTime);
            Property(t => t.Code).HasMaxLength(400);

            //relationship  
            HasRequired(t => t.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(t => t.CustomerId)
                .WillCascadeOnDelete(false);
        }
    }
}
