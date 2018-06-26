using System.Data.Entity;
using EF.Core;
using System.Data.Entity.Infrastructure;

namespace EF.Data
{
    public interface IDbContext
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : BaseEntity;
        int SaveChanges();
    }
}
