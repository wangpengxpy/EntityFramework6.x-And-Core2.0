using Microsoft.EntityFrameworkCore;
using Polly;
using System;

namespace EFCoreConcurrency
{
    public class DbQueryCommit : IDisposable
    {

        private readonly EFCoreContext context;

        public DbQueryCommit(EFCoreContext context) => this.context = context;

        public TEntity Query<TEntity>(params object[] keys) where TEntity : class =>
            this.context.Set<TEntity>().Find(keys);

        public int Commit(Action change)
        {
            change();
            return context.SaveChanges();
        }

        public int Commit(Action change, Action<DbUpdateConcurrencyException> handleException, int retryCount = 3)
        {
            change();

            Policy
            .Handle<DbUpdateConcurrencyException>(ex => ex.Entries.Count > 0)
            .Or<ArgumentException>(ex => ex.ParamName == "example")
            .WaitAndRetry(3, retryAttempt => TimeSpan.FromSeconds(10))
            .Execute(() => context.SaveChanges());

            return context.SaveChanges();
        }

        public int DatabaseWin(Action change, Action<DbUpdateConcurrencyException> handleException)
        {
            change();
            try
            {
                return context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                return 0;
            }

        }
        public DbSet<TEntity> Set<TEntity>() where TEntity : class => context.Set<TEntity>();

        public void Dispose() => context.Dispose();
    }
}
