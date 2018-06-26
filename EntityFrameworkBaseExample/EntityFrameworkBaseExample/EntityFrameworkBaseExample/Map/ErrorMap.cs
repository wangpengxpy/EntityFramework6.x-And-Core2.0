using EntityFrameworkBaseExample.Entity;
using System.Data.Entity.ModelConfiguration;

namespace EntityFrameworkBaseExample.Map
{
    public class ErrorMap : EntityTypeConfiguration<Error>
    {
        public ErrorMap()
        {
            ToTable("Errors");

            HasKey(k => k.ErrorId);

            Property(p => p.Active);
            Property(p => p.CommandType).HasColumnType("VARCHAR");
            Property(p => p.CreateDate);
            Property(p => p.Exception).HasColumnType("VARCHAR");
            Property(p => p.FileName);
            Property(p => p.InnerException).HasColumnType("VARCHAR");
            Property(p => p.Parameters);
            Property(p => p.Query).IsRequired().HasColumnType("VARCHAR");
            Property(p => p.RequestId);
            Property(p => p.TotalSeconds);
        }
    }
}