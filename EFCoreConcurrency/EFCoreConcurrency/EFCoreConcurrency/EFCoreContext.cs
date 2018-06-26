using EFCore.Model;
using Microsoft.EntityFrameworkCore;

namespace EFCoreConcurrency
{
    public class EFCoreContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(@"Server=.;Database=EFCoreDb;Trusted_Connection=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>(pc => 
            {
                pc.ToTable("Blog").HasKey(k => k.Id);

                pc.Property(p => p.Name).IsRequired();
                pc.Property(p => p.Url).IsRequired();
                pc.Property(p => p.Count).IsRequired();

                pc.Property(p => p.RowVersion).IsRequired().IsRowVersion().ValueGeneratedOnAddOrUpdate();
            });
        }
    }

}
