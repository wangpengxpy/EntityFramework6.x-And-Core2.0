using EntityFrameworkBaseExample.Entity;
using System.Data.Entity.ModelConfiguration;

namespace EntityFrameworkBaseExample.Map
{
    public class PostMap : EntityTypeConfiguration<Post>
    {
        public PostMap()
        {
            //table
            ToTable("Posts");

            //key
            HasKey(k => k.Id);

            //properties
            Property(p => p.Title).IsRequired().HasColumnType("VARCHAR").HasMaxLength(50);
            Property(p => p.Content);

        }
    }
}