using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System;
using System.Collections.Generic;

namespace EFCoreConcurrency
{
    public class TransientDetection<TException> : ITransientErrorDetectionStrategy
    where TException : Exception
    {
        public bool IsTransient(Exception ex) => ex is TException;
    }

    public static partial class DbContextExtensions
    {
        public static int SaveChanges(
       this DbContext context, Action<IEnumerable<EntityEntry>> resolveConflicts, int retryCount = 3)
        {
            if (retryCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(retryCount), $"{retryCount} must be greater than 0.");
            }

            for (int retry = 1; retry < retryCount; retry++)
            {
                try
                {
                    return context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException exception) when (retry < retryCount)
                {
                    resolveConflicts(exception.Entries);
                }
            }
            return context.SaveChanges();
        }
        public static int SaveChanges(
            this DbContext context, Action<IEnumerable<EntityEntry>> resolveConflicts, RetryStrategy retryStrategy)
        {
            RetryPolicy retryPolicy = new RetryPolicy(
                errorDetectionStrategy: new TransientDetection<DbUpdateConcurrencyException>(),
                retryStrategy: retryStrategy);
            retryPolicy.Retrying += (sender, e) =>
                resolveConflicts(((DbUpdateConcurrencyException)e.LastException).Entries);
            return retryPolicy.ExecuteAction(context.SaveChanges);
        }
    }
}
