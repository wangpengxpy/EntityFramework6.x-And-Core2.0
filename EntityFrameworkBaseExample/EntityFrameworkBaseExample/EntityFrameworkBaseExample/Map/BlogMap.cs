using EntityFrameworkBaseExample.Entity;
using System.Data.Entity.ModelConfiguration;

namespace EntityFrameworkBaseExample.Map
{
    public class BlogMap : EntityTypeConfiguration<Blog>
    {
        public BlogMap()
        {
            //table
            ToTable("Blogs");

            //key
            HasKey(k => k.Id);

            //properties
            Property(p => p.Url).IsRequired().HasColumnType("VARCHAR").HasMaxLength(100);
            Property(p => p._Tags).HasColumnName("Tags");
            Property(p => p._Owner).HasColumnName("Owner");

            //ignore property
            Ignore(p => p.Owner);
            Ignore(p => p.Tags);

            //realationship
            HasMany(m => m.Posts).WithRequired(r => r.Blog).HasForeignKey(k => k.BlogId)
                .WillCascadeOnDelete(true);
        }
    }
}