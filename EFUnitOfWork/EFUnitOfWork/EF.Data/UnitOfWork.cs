using EF.Core;
using System;
using System.Collections.Concurrent;

namespace EF.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly EFDbContext context;
        private bool disposed;
        private ConcurrentDictionary<string, object> repositories;

        public UnitOfWork(EFDbContext context)
        {
            this.context = context;
        }

        public UnitOfWork()
        {
            context = new EFDbContext();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void RollBack()
        {

        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public Repository<T> Repository<T>() where T : BaseEntity
        {
            if (repositories == null)
            {
                repositories = new ConcurrentDictionary<string, object>();
            }

            var type = typeof(T).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), context);
                repositories.TryAdd(type, repositoryInstance);
            }
            return (Repository<T>)repositories[type];
        }
    }
}
