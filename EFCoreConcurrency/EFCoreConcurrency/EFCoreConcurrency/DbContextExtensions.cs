using Microsoft.EntityFrameworkCore;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System;
using System.Linq;

namespace EFCoreConcurrency
{
    public static partial class DbContextExtensions
    {
        public static int SaveChanges(this DbContext context, RefreshConflict refreshMode, int retryCount = 3)
        {
            if (retryCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(retryCount), $"{retryCount}必须大于0");
            }

            return context.SaveChanges(
            conflicts => conflicts.ToList().ForEach(tracking => tracking.Refresh(refreshMode)), retryCount);
        }

        public static int SaveChanges(
            this DbContext context, RefreshConflict refreshMode, RetryStrategy retryStrategy) =>
                context.SaveChanges(
                    conflicts => conflicts.ToList().ForEach(tracking => tracking.Refresh(refreshMode)), retryStrategy);
            
    }
}
